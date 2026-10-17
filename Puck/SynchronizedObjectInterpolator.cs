using System;
using UnityEngine;

// Token: 0x02000086 RID: 134
public class SynchronizedObjectInterpolator
{
	// Token: 0x1700006C RID: 108
	// (get) Token: 0x0600042C RID: 1068 RVA: 0x00018421 File Offset: 0x00016621
	public bool HasSamples
	{
		get
		{
			return this.samples.Count > 0;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x0600042D RID: 1069 RVA: 0x00018431 File Offset: 0x00016631
	public float SampleInterval
	{
		get
		{
			return this.sampleInterval;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x0600042E RID: 1070 RVA: 0x00018439 File Offset: 0x00016639
	public float ExtraLag
	{
		get
		{
			return this.extraLag;
		}
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x00018441 File Offset: 0x00016641
	public SynchronizedObjectInterpolator(int sampleCapacity)
	{
		this.samples = new SynchronizedObjectSampleRing(sampleCapacity);
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x00018460 File Offset: 0x00016660
	public bool HasDataAtLeastAsNewAs(double serverTime)
	{
		return this.hasReceivedData && serverTime <= this.lastReceivedServerTime;
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x00018478 File Offset: 0x00016678
	public void MarkReceived(double serverTime)
	{
		this.hasReceivedData = true;
		this.lastReceivedServerTime = serverTime;
		this.isAsleep = false;
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x00018490 File Offset: 0x00016690
	public bool Append(Vector3 position, Quaternion rotation, Vector3 linearVelocity, Vector3 angularVelocity, double serverTime, float tickInterval, byte tickRateDivisor)
	{
		if (this.samples.Count > 0 && serverTime <= this.samples.Newest.ServerTime)
		{
			return false;
		}
		this.sampleInterval = (float)tickRateDivisor * tickInterval;
		this.extraLag = this.sampleInterval - tickInterval;
		this.velocityLeadFraction = 0.5f / (float)Mathf.Max(1, (int)tickRateDivisor);
		SynchronizedObjectSampleRing synchronizedObjectSampleRing = this.samples;
		SynchronizedObjectSample synchronizedObjectSample = default(SynchronizedObjectSample);
		synchronizedObjectSample.ServerTime = serverTime;
		synchronizedObjectSample.Position = position;
		synchronizedObjectSample.Rotation = rotation;
		synchronizedObjectSample.LinearVelocity = linearVelocity;
		synchronizedObjectSample.AngularVelocity = angularVelocity;
		synchronizedObjectSampleRing.Append(synchronizedObjectSample);
		this.MarkReceived(serverTime);
		return true;
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x00018539 File Offset: 0x00016739
	public void SetAsleep()
	{
		this.isAsleep = true;
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x00018544 File Offset: 0x00016744
	public void Reset()
	{
		this.hasReceivedData = false;
		this.lastReceivedServerTime = 0.0;
		this.samples.Reset();
		this.sampleInterval = 0f;
		this.extraLag = 0f;
		this.velocityLeadFraction = 0.5f;
		this.isAsleep = false;
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x0001859C File Offset: 0x0001679C
	public SynchronizedObjectPose Evaluate(double sampleTime, float projectionTime, float maxPositionExtrapolationTicks, float maxRotationExtrapolationTicks, out SynchronizedObjectPlayback playback)
	{
		SynchronizedObjectSample newest = this.samples.Newest;
		if (sampleTime >= newest.ServerTime)
		{
			return this.EvaluateAfterNewestSample(newest, sampleTime, projectionTime, maxPositionExtrapolationTicks, maxRotationExtrapolationTicks, out playback);
		}
		playback = SynchronizedObjectPlayback.Interpolated;
		SynchronizedObjectSample oldest = this.samples.Oldest;
		if (sampleTime <= oldest.ServerTime)
		{
			return SynchronizedObjectInterpolator.GetSamplePose(oldest);
		}
		return this.EvaluateBetweenSamples(newest, sampleTime, projectionTime);
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x000185FC File Offset: 0x000167FC
	private SynchronizedObjectPose EvaluateAfterNewestSample(in SynchronizedObjectSample newestSample, double sampleTime, float projectionTime, float maxPositionExtrapolationTicks, float maxRotationExtrapolationTicks, out SynchronizedObjectPlayback playback)
	{
		if (this.isAsleep)
		{
			playback = SynchronizedObjectPlayback.None;
			return new SynchronizedObjectPose
			{
				Position = newestSample.Position,
				Rotation = newestSample.Rotation
			};
		}
		SynchronizedObjectPose samplePose = SynchronizedObjectInterpolator.GetSamplePose(newestSample);
		samplePose.LinearVelocity = this.GetInstantaneousLinearVelocity(this.samples.Count - 1);
		float pastTime = (float)(sampleTime - newestSample.ServerTime);
		playback = SynchronizedObjectPlayback.Extrapolated;
		SynchronizedObjectInterpolator.ApplyPositionExtrapolation(ref samplePose, projectionTime);
		SynchronizedObjectInterpolator.ApplyRotationExtrapolation(ref samplePose, projectionTime);
		SynchronizedObjectInterpolator.ExtrapolatePositionWithFalloff(ref samplePose, pastTime, maxPositionExtrapolationTicks * this.sampleInterval);
		SynchronizedObjectInterpolator.ExtrapolateRotationWithFalloff(ref samplePose, pastTime, maxRotationExtrapolationTicks * this.sampleInterval);
		return samplePose;
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x0001869C File Offset: 0x0001689C
	private SynchronizedObjectPose EvaluateBetweenSamples(in SynchronizedObjectSample newestSample, double sampleTime, float projectionTime)
	{
		SynchronizedObjectPose samplePose = SynchronizedObjectInterpolator.GetSamplePose(newestSample);
		for (int i = this.samples.Count - 1; i > 0; i--)
		{
			SynchronizedObjectSample synchronizedObjectSample = this.samples[i - 1];
			if (sampleTime >= synchronizedObjectSample.ServerTime)
			{
				SynchronizedObjectSample synchronizedObjectSample2 = this.samples[i];
				double num = synchronizedObjectSample2.ServerTime - synchronizedObjectSample.ServerTime;
				float t = (float)((sampleTime - synchronizedObjectSample.ServerTime) / num);
				float sampleDeltaTime = (float)num;
				Vector3 instantaneousLinearVelocity = this.GetInstantaneousLinearVelocity(i - 1);
				Vector3 instantaneousLinearVelocity2 = this.GetInstantaneousLinearVelocity(i);
				samplePose.Position = SynchronizedObjectInterpolator.GetHermitePosition(synchronizedObjectSample.Position, instantaneousLinearVelocity, synchronizedObjectSample2.Position, instantaneousLinearVelocity2, t, sampleDeltaTime);
				samplePose.Rotation = Quaternion.Slerp(synchronizedObjectSample.Rotation, synchronizedObjectSample2.Rotation, t);
				samplePose.LinearVelocity = SynchronizedObjectInterpolator.GetHermiteLinearVelocity(synchronizedObjectSample.Position, instantaneousLinearVelocity, synchronizedObjectSample2.Position, instantaneousLinearVelocity2, t, sampleDeltaTime);
				samplePose.AngularVelocity = Vector3.Lerp(synchronizedObjectSample.AngularVelocity, synchronizedObjectSample2.AngularVelocity, t);
				SynchronizedObjectInterpolator.ApplyPositionExtrapolation(ref samplePose, projectionTime);
				SynchronizedObjectInterpolator.ApplyRotationExtrapolation(ref samplePose, projectionTime);
				return samplePose;
			}
		}
		return samplePose;
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x000187B4 File Offset: 0x000169B4
	private Vector3 GetInstantaneousLinearVelocity(int index)
	{
		if (index + 1 < this.samples.Count)
		{
			return Vector3.LerpUnclamped(this.samples[index].LinearVelocity, this.samples[index + 1].LinearVelocity, this.velocityLeadFraction);
		}
		if (index > 0)
		{
			return Vector3.LerpUnclamped(this.samples[index - 1].LinearVelocity, this.samples[index].LinearVelocity, 1f + this.velocityLeadFraction);
		}
		return this.samples[index].LinearVelocity;
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x0001884C File Offset: 0x00016A4C
	private static Vector3 GetHermitePosition(Vector3 fromPosition, Vector3 fromLinearVelocity, Vector3 toPosition, Vector3 toLinearVelocity, float t, float sampleDeltaTime)
	{
		float num = t * t;
		float num2 = num * t;
		return (2f * num2 - 3f * num + 1f) * fromPosition + (num2 - 2f * num + t) * sampleDeltaTime * fromLinearVelocity + (-2f * num2 + 3f * num) * toPosition + (num2 - num) * sampleDeltaTime * toLinearVelocity;
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x000188C4 File Offset: 0x00016AC4
	private static Vector3 GetHermiteLinearVelocity(Vector3 fromPosition, Vector3 fromLinearVelocity, Vector3 toPosition, Vector3 toLinearVelocity, float t, float sampleDeltaTime)
	{
		float num = t * t;
		return (6f * t - 6f * num) * (toPosition - fromPosition) / sampleDeltaTime + (3f * num - 4f * t + 1f) * fromLinearVelocity + (3f * num - 2f * t) * toLinearVelocity;
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x00018938 File Offset: 0x00016B38
	private static SynchronizedObjectPose GetSamplePose(in SynchronizedObjectSample sample)
	{
		return new SynchronizedObjectPose
		{
			Position = sample.Position,
			Rotation = sample.Rotation,
			LinearVelocity = sample.LinearVelocity,
			AngularVelocity = sample.AngularVelocity
		};
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00018984 File Offset: 0x00016B84
	private static void ExtrapolatePositionWithFalloff(ref SynchronizedObjectPose pose, float pastTime, float maxExtrapolationTime)
	{
		float velocityFalloff = SynchronizedObjectInterpolator.GetVelocityFalloff(pastTime, maxExtrapolationTime);
		SynchronizedObjectInterpolator.ApplyPositionExtrapolation(ref pose, maxExtrapolationTime * (1f - velocityFalloff));
		pose.LinearVelocity *= velocityFalloff;
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x000189C0 File Offset: 0x00016BC0
	private static void ExtrapolateRotationWithFalloff(ref SynchronizedObjectPose pose, float pastTime, float maxExtrapolationTime)
	{
		float velocityFalloff = SynchronizedObjectInterpolator.GetVelocityFalloff(pastTime, maxExtrapolationTime);
		SynchronizedObjectInterpolator.ApplyRotationExtrapolation(ref pose, maxExtrapolationTime * (1f - velocityFalloff));
		pose.AngularVelocity *= velocityFalloff;
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x000189FB File Offset: 0x00016BFB
	private static float GetVelocityFalloff(float pastTime, float maxExtrapolationTime)
	{
		if (pastTime <= 0f || maxExtrapolationTime <= 0f)
		{
			return 1f;
		}
		return Mathf.Exp(-pastTime / maxExtrapolationTime);
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x00018A1C File Offset: 0x00016C1C
	private static void ApplyPositionExtrapolation(ref SynchronizedObjectPose pose, float pastTime)
	{
		if (pastTime <= 0f)
		{
			return;
		}
		pose.Position += pose.LinearVelocity * pastTime;
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x00018A4C File Offset: 0x00016C4C
	private static void ApplyRotationExtrapolation(ref SynchronizedObjectPose pose, float pastTime)
	{
		if (pastTime <= 0f)
		{
			return;
		}
		float magnitude = pose.AngularVelocity.magnitude;
		if (magnitude <= 0f)
		{
			return;
		}
		pose.Rotation = Quaternion.AngleAxis(magnitude * pastTime * 57.29578f, pose.AngularVelocity / magnitude) * pose.Rotation;
	}

	// Token: 0x0400031B RID: 795
	private SynchronizedObjectSampleRing samples;

	// Token: 0x0400031C RID: 796
	private bool hasReceivedData;

	// Token: 0x0400031D RID: 797
	private double lastReceivedServerTime;

	// Token: 0x0400031E RID: 798
	private float sampleInterval;

	// Token: 0x0400031F RID: 799
	private float extraLag;

	// Token: 0x04000320 RID: 800
	private float velocityLeadFraction = 0.5f;

	// Token: 0x04000321 RID: 801
	private bool isAsleep;
}
