using System;
using UnityEngine;

// Token: 0x0200008B RID: 139
public class SynchronizedObjectSmoothing
{
	// Token: 0x06000449 RID: 1097 RVA: 0x00018B7C File Offset: 0x00016D7C
	public void MarkNewSample()
	{
		this.hasNewSample = true;
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x00018B85 File Offset: 0x00016D85
	public bool TryBeginAbsorb(out double sampleTime, out float projectionTime)
	{
		sampleTime = this.lastSampleTime;
		projectionTime = this.lastProjectionTime;
		if (!this.hasNewSample)
		{
			return false;
		}
		this.hasNewSample = false;
		return this.hasRenderedPose;
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x00018BAE File Offset: 0x00016DAE
	public void Absorb(in SynchronizedObjectPose poseAtLastRenderedTime)
	{
		this.positionCorrection = this.renderedPosition - poseAtLastRenderedTime.Position;
		this.rotationCorrection = this.renderedRotation * Quaternion.Inverse(poseAtLastRenderedTime.Rotation);
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x00018BE4 File Offset: 0x00016DE4
	public SynchronizedObjectPose Apply(SynchronizedObjectPose pose, float deltaTime, float correctionTimeConstant)
	{
		float num = Mathf.Exp(-deltaTime / correctionTimeConstant);
		this.positionCorrection *= num;
		this.rotationCorrection = Quaternion.Slerp(Quaternion.identity, this.rotationCorrection, num);
		pose.Position += this.positionCorrection;
		pose.Rotation = this.rotationCorrection * pose.Rotation;
		return pose;
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x00018C5A File Offset: 0x00016E5A
	public void CaptureRenderedPose(in SynchronizedObjectPose pose, double sampleTime, float projectionTime)
	{
		this.renderedPosition = pose.Position;
		this.renderedRotation = pose.Rotation;
		this.hasRenderedPose = true;
		this.lastSampleTime = sampleTime;
		this.lastProjectionTime = projectionTime;
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x00018C89 File Offset: 0x00016E89
	public void Reset()
	{
		this.hasNewSample = false;
		this.hasRenderedPose = false;
		this.lastSampleTime = 0.0;
		this.lastProjectionTime = 0f;
		this.positionCorrection = Vector3.zero;
		this.rotationCorrection = Quaternion.identity;
	}

	// Token: 0x04000332 RID: 818
	private bool hasNewSample;

	// Token: 0x04000333 RID: 819
	private bool hasRenderedPose;

	// Token: 0x04000334 RID: 820
	private double lastSampleTime;

	// Token: 0x04000335 RID: 821
	private float lastProjectionTime;

	// Token: 0x04000336 RID: 822
	private Vector3 renderedPosition = Vector3.zero;

	// Token: 0x04000337 RID: 823
	private Quaternion renderedRotation = Quaternion.identity;

	// Token: 0x04000338 RID: 824
	private Vector3 positionCorrection = Vector3.zero;

	// Token: 0x04000339 RID: 825
	private Quaternion rotationCorrection = Quaternion.identity;
}
