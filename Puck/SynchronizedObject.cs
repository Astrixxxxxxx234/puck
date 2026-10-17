using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class SynchronizedObject : NetworkBehaviour
{
	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000413 RID: 1043 RVA: 0x000180BD File Offset: 0x000162BD
	public bool IsDrivenExternally
	{
		get
		{
			return this.Rigidbody.isKinematic;
		}
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x000180CA File Offset: 0x000162CA
	private void Awake()
	{
		this.Rigidbody = base.GetComponent<Rigidbody>();
		this.clientInterpolator = new SynchronizedObjectInterpolator(64);
		this.clientSmoothing = new SynchronizedObjectSmoothing();
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x000180F0 File Offset: 0x000162F0
	protected override void OnNetworkPostSpawn()
	{
		EventManager.TriggerEvent("Event_Everyone_OnSynchronizedObjectSpawned", new Dictionary<string, object>
		{
			{
				"synchronizedObject",
				this
			}
		});
		base.OnNetworkPostSpawn();
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x00018113 File Offset: 0x00016313
	public override void OnNetworkDespawn()
	{
		EventManager.TriggerEvent("Event_Everyone_OnSynchronizedObjectDespawned", new Dictionary<string, object>
		{
			{
				"synchronizedObject",
				this
			}
		});
		base.OnNetworkDespawn();
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x00018136 File Offset: 0x00016336
	public void DriveExternally(SynchronizedObjectDriveCadence cadence)
	{
		this.Rigidbody.isKinematic = true;
		this.Rigidbody.interpolation = SynchronizedObject.GetDrivenInterpolation(cadence);
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x00018155 File Offset: 0x00016355
	private static RigidbodyInterpolation GetDrivenInterpolation(SynchronizedObjectDriveCadence cadence)
	{
		if (cadence != SynchronizedObjectDriveCadence.PerTick || ApplicationManager.IsDedicatedGameServer)
		{
			return RigidbodyInterpolation.None;
		}
		return RigidbodyInterpolation.Interpolate;
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0001816B File Offset: 0x0001636B
	public void SetDrivenPose(in SynchronizedObjectPose pose)
	{
		this.drivenPose = pose;
		this.hasDrivenPose = true;
		this.Rigidbody.Move(pose.Position, pose.Rotation);
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x00018198 File Offset: 0x00016398
	public SynchronizedObjectPose GetPose()
	{
		if (this.hasDrivenPose)
		{
			return this.drivenPose;
		}
		return new SynchronizedObjectPose
		{
			Position = this.Rigidbody.position,
			Rotation = this.Rigidbody.rotation,
			LinearVelocity = this.GetLinearVelocityAtPosition(),
			AngularVelocity = this.Rigidbody.angularVelocity
		};
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x00018200 File Offset: 0x00016400
	public Vector3 GetLinearVelocityAtPosition()
	{
		return this.Rigidbody.GetPointVelocity(this.Rigidbody.position);
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x00018218 File Offset: 0x00016418
	public bool Client_HasNewerDataThan(double serverTime)
	{
		return this.clientInterpolator.HasDataAtLeastAsNewAs(serverTime);
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x00018226 File Offset: 0x00016426
	public void Client_ApplyReliableSynchronizedObjectData(Vector3 position, Quaternion rotation, double serverTime)
	{
		if (this.Client_HasNewerDataThan(serverTime))
		{
			return;
		}
		this.clientInterpolator.MarkReceived(serverTime);
		base.transform.SetPositionAndRotation(position, rotation);
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x0001824B File Offset: 0x0001644B
	public void Client_BufferSynchronizedObjectData(Vector3 position, Quaternion rotation, Vector3 linearVelocity, Vector3 angularVelocity, double serverTime, float tickInterval, byte tickRateDivisor)
	{
		if (this.clientInterpolator.Append(position, rotation, linearVelocity, angularVelocity, serverTime, tickInterval, tickRateDivisor))
		{
			this.clientSmoothing.MarkNewSample();
		}
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x00018270 File Offset: 0x00016470
	public void Client_SetAsleep()
	{
		this.clientInterpolator.SetAsleep();
		this.PredictedLinearVelocity = Vector3.zero;
		this.PredictedAngularVelocity = Vector3.zero;
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x00018293 File Offset: 0x00016493
	public void Client_ResetInterpolation()
	{
		this.clientInterpolator.Reset();
		this.clientSmoothing.Reset();
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x000182AC File Offset: 0x000164AC
	public SynchronizedObjectPlayback Client_Interpolate(double localTimeline, float deltaTime, float latencyCompensationTime, float maxPositionExtrapolationTicks, float maxRotationExtrapolationTicks)
	{
		if (!this.clientInterpolator.HasSamples)
		{
			return SynchronizedObjectPlayback.None;
		}
		double sampleTime = localTimeline + (double)latencyCompensationTime - (double)this.clientInterpolator.ExtraLag;
		float extraLag = this.clientInterpolator.ExtraLag;
		double sampleTime2;
		float projectionTime;
		if (this.clientSmoothing.TryBeginAbsorb(out sampleTime2, out projectionTime))
		{
			SynchronizedObjectSmoothing synchronizedObjectSmoothing = this.clientSmoothing;
			SynchronizedObjectPlayback synchronizedObjectPlayback;
			SynchronizedObjectPose synchronizedObjectPose = this.clientInterpolator.Evaluate(sampleTime2, projectionTime, maxPositionExtrapolationTicks, maxRotationExtrapolationTicks, out synchronizedObjectPlayback);
			synchronizedObjectSmoothing.Absorb(synchronizedObjectPose);
		}
		SynchronizedObjectPlayback result;
		SynchronizedObjectPose synchronizedObjectPose2 = this.clientSmoothing.Apply(this.clientInterpolator.Evaluate(sampleTime, extraLag, maxPositionExtrapolationTicks, maxRotationExtrapolationTicks, out result), deltaTime, this.clientInterpolator.SampleInterval);
		base.transform.SetPositionAndRotation(synchronizedObjectPose2.Position, synchronizedObjectPose2.Rotation);
		this.PredictedLinearVelocity = synchronizedObjectPose2.LinearVelocity;
		this.PredictedAngularVelocity = synchronizedObjectPose2.AngularVelocity;
		this.clientSmoothing.CaptureRenderedPose(synchronizedObjectPose2, sampleTime, extraLag);
		return result;
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x000183B8 File Offset: 0x000165B8
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x000183CE File Offset: 0x000165CE
	protected internal override string __getTypeName()
	{
		return "SynchronizedObject";
	}

	// Token: 0x0400030D RID: 781
	private const int CLIENT_SAMPLE_CAPACITY = 64;

	// Token: 0x0400030E RID: 782
	[Header("Settings")]
	[Tooltip("Lets Server Lod Bands on Synchronized Object Manager override the server defaults for this object by its distance from each player. Disabling it keeps the defaults at every distance.")]
	public bool UseLod = true;

	// Token: 0x0400030F RID: 783
	[Tooltip("Lets Server Culling on Synchronized Object Manager override the server defaults and any LOD band for this object while it is behind a player's camera. Disabling it keeps whatever the distance produced.")]
	public bool UseCulling = true;

	// Token: 0x04000310 RID: 784
	[HideInInspector]
	public Rigidbody Rigidbody;

	// Token: 0x04000311 RID: 785
	[HideInInspector]
	public Vector3 PredictedLinearVelocity = Vector3.zero;

	// Token: 0x04000312 RID: 786
	[HideInInspector]
	public Vector3 PredictedAngularVelocity = Vector3.zero;

	// Token: 0x04000313 RID: 787
	private SynchronizedObjectInterpolator clientInterpolator;

	// Token: 0x04000314 RID: 788
	private SynchronizedObjectSmoothing clientSmoothing;

	// Token: 0x04000315 RID: 789
	private bool hasDrivenPose;

	// Token: 0x04000316 RID: 790
	private SynchronizedObjectPose drivenPose;
}
