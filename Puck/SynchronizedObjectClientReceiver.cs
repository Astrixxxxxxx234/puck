using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class SynchronizedObjectClientReceiver
{
	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x0003760F File Offset: 0x0003580F
	private double StallTime
	{
		get
		{
			if (!this.hasReceivedFirstTick)
			{
				return 0.0;
			}
			return Time.realtimeSinceStartupAsDouble - this.lastReceivedLocalTime;
		}
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x0003762F File Offset: 0x0003582F
	private float TickInterval
	{
		get
		{
			if (this.receivedTickInterval <= 0f)
			{
				return this.fallbackTickInterval;
			}
			return this.receivedTickInterval;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x0003764B File Offset: 0x0003584B
	private float TargetTimelinePosition
	{
		get
		{
			return this.smoothingSettings.TargetTimelinePosition * this.TickInterval;
		}
	}

	// Token: 0x06000BA8 RID: 2984 RVA: 0x00037660 File Offset: 0x00035860
	public SynchronizedObjectClientReceiver(SynchronizedObjectRegistry registry)
	{
		this.registry = registry;
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x000376B1 File Offset: 0x000358B1
	public void Configure(SynchronizedObjectNetworkSmoothingSettings smoothingSettings, float diagnosticsInterval)
	{
		this.smoothingSettings = smoothingSettings;
		this.diagnostics.Configure(diagnosticsInterval);
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x000376C6 File Offset: 0x000358C6
	public void SetTickInterval(float tickInterval)
	{
		this.fallbackTickInterval = tickInterval;
		this.ConfigureTimeline();
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x000376D5 File Offset: 0x000358D5
	public void Forget(ulong networkObjectId)
	{
		this.receivedStates.Remove(networkObjectId);
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x000376E4 File Offset: 0x000358E4
	public void ReceiveTick(SynchronizedObjectTickHeader tickHeader, SynchronizedObjectData[] synchronizedObjectsData)
	{
		double serverTime = tickHeader.ServerTime;
		if (this.hasReceivedFirstTick && serverTime <= this.lastReceivedServerTime)
		{
			this.diagnostics.RecordOutOfOrderTick();
			return;
		}
		this.ApplyTickHeader(tickHeader);
		float serverDeltaTime;
		this.RecordTickDiagnostics(tickHeader.SequenceNumber, serverTime, out serverDeltaTime);
		this.hasPendingTimelineAdjustment = true;
		this.BufferObjects(synchronizedObjectsData, serverTime);
		this.TriggerSynchronizeEvents(serverDeltaTime);
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x00037744 File Offset: 0x00035944
	public void ReceiveReliable(SynchronizedObjectTickHeader tickHeader, SynchronizedObjectData[] synchronizedObjectsData)
	{
		this.ApplyTickHeader(tickHeader);
		double serverTime = tickHeader.ServerTime;
		float tickInterval = this.TickInterval;
		foreach (SynchronizedObjectData synchronizedObjectData in synchronizedObjectsData)
		{
			SynchronizedObject synchronizedObject;
			SynchronizedObjectData synchronizedObjectData2;
			if (this.registry.TryGet((ulong)synchronizedObjectData.NetworkObjectId, out synchronizedObject) && !synchronizedObject.Client_HasNewerDataThan(serverTime) && this.TryMergeReceivedData(synchronizedObjectData, serverTime, out synchronizedObjectData2))
			{
				Vector3 position = synchronizedObjectData2.GetPosition();
				Quaternion rotation = synchronizedObjectData2.Rotation;
				if (this.timeline.IsInitialized)
				{
					synchronizedObject.Client_BufferSynchronizedObjectData(position, rotation, synchronizedObjectData2.LinearVelocity, synchronizedObjectData2.AngularVelocity, serverTime, tickInterval, synchronizedObjectData2.TickRateDivisor);
				}
				else
				{
					synchronizedObject.Client_ApplyReliableSynchronizedObjectData(position, rotation, serverTime);
				}
				if (synchronizedObjectData.IsAsleep)
				{
					synchronizedObject.Client_SetAsleep();
				}
			}
		}
		this.TriggerSynchronizeEvents(0f);
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x00037820 File Offset: 0x00035A20
	public void Update(float deltaTime)
	{
		float num = deltaTime * this.timeScale;
		this.AdjustTimeline();
		if (this.hasReceivedFirstTick && this.diagnostics.IsEnabled)
		{
			this.diagnostics.RecordTimeline(this.GetTimelinePosition() / (double)this.TickInterval, this.timeline.Timescale);
		}
		this.TriggerDiagnosticsEvents(deltaTime);
		if (!this.timeline.IsInitialized)
		{
			return;
		}
		this.timeline.Step((double)num);
		float targetTimelinePosition = this.TargetTimelinePosition;
		float maxPositionExtrapolationTicks = this.smoothingSettings.MaxPositionExtrapolationTicks;
		float maxRotationExtrapolationTicks = this.smoothingSettings.MaxRotationExtrapolationTicks;
		foreach (SynchronizedObject synchronizedObject in this.registry.Objects)
		{
			if (synchronizedObject)
			{
				this.diagnostics.RecordPlayback(synchronizedObject.Client_Interpolate(this.timeline.Time, num, targetTimelinePosition, maxPositionExtrapolationTicks, maxRotationExtrapolationTicks));
			}
		}
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x00037928 File Offset: 0x00035B28
	public void Dispose()
	{
		this.receivedStates.Clear();
		this.lastReceivedSequenceNumber = 0;
		this.lastReceivedServerTime = 0.0;
		this.lastReceivedLocalTime = 0.0;
		this.hasReceivedFirstTick = false;
		this.hasPendingTimelineAdjustment = false;
		this.receivedTickInterval = 0f;
		this.timeScale = 1f;
		this.diagnostics.Reset();
		this.ConfigureTimeline();
		this.ResetInterpolationState();
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x000379A0 File Offset: 0x00035BA0
	private void AdjustTimeline()
	{
		if (!this.hasPendingTimelineAdjustment)
		{
			return;
		}
		this.hasPendingTimelineAdjustment = false;
		if (this.timeline.Adjust(this.lastReceivedServerTime))
		{
			this.diagnostics.RecordSnap();
		}
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x000379D0 File Offset: 0x00035BD0
	private void ApplyTickHeader(SynchronizedObjectTickHeader tickHeader)
	{
		if (tickHeader.HasTimeScale)
		{
			this.timeScale = tickHeader.TimeScale;
		}
		if (tickHeader.HasTickInterval && tickHeader.TickInterval != this.receivedTickInterval)
		{
			this.receivedTickInterval = tickHeader.TickInterval;
			this.ConfigureTimeline();
		}
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x00037A10 File Offset: 0x00035C10
	private void RecordTickDiagnostics(ushort sequenceNumber, double serverTime, out float serverDeltaTime)
	{
		int lostTickCount = (int)(this.hasReceivedFirstTick ? (sequenceNumber - this.lastReceivedSequenceNumber - 1) : 0);
		double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
		serverDeltaTime = (this.hasReceivedFirstTick ? ((float)(serverTime - this.lastReceivedServerTime)) : 0f);
		float tickDeltaTime = this.hasReceivedFirstTick ? ((float)(realtimeSinceStartupAsDouble - this.lastReceivedLocalTime)) : 0f;
		this.diagnostics.RecordTick(tickDeltaTime, lostTickCount);
		this.lastReceivedSequenceNumber = sequenceNumber;
		this.lastReceivedServerTime = serverTime;
		this.lastReceivedLocalTime = realtimeSinceStartupAsDouble;
		this.hasReceivedFirstTick = true;
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x00037A98 File Offset: 0x00035C98
	private double GetTimelinePosition()
	{
		double num = this.lastReceivedServerTime + this.StallTime * (double)this.timeScale;
		if (!this.timeline.IsInitialized)
		{
			return this.lastReceivedServerTime - num;
		}
		return this.timeline.Time + (double)this.TargetTimelinePosition - num;
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x00037AE8 File Offset: 0x00035CE8
	private void BufferObjects(SynchronizedObjectData[] synchronizedObjectsData, double serverTime)
	{
		float tickInterval = this.TickInterval;
		foreach (SynchronizedObjectData synchronizedObjectData in synchronizedObjectsData)
		{
			SynchronizedObject synchronizedObject;
			SynchronizedObjectData synchronizedObjectData2;
			if (this.registry.TryGet((ulong)synchronizedObjectData.NetworkObjectId, out synchronizedObject) && this.TryMergeReceivedData(synchronizedObjectData, serverTime, out synchronizedObjectData2))
			{
				synchronizedObject.Client_BufferSynchronizedObjectData(synchronizedObjectData2.GetPosition(), synchronizedObjectData2.Rotation, synchronizedObjectData2.LinearVelocity, synchronizedObjectData2.AngularVelocity, serverTime, tickInterval, synchronizedObjectData2.TickRateDivisor);
			}
		}
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x00037B64 File Offset: 0x00035D64
	private bool TryMergeReceivedData(SynchronizedObjectData synchronizedObjectData, double serverTime, out SynchronizedObjectData mergedData)
	{
		ulong key = (ulong)synchronizedObjectData.NetworkObjectId;
		mergedData = default(SynchronizedObjectData);
		SynchronizedObjectReceivedState synchronizedObjectReceivedState;
		if (this.receivedStates.TryGetValue(key, out synchronizedObjectReceivedState))
		{
			if (serverTime <= synchronizedObjectReceivedState.ServerTime)
			{
				return false;
			}
			mergedData = synchronizedObjectReceivedState.Data.Merge(synchronizedObjectData);
		}
		else
		{
			if (!synchronizedObjectData.HasAllComponents)
			{
				return false;
			}
			mergedData = synchronizedObjectData;
		}
		this.receivedStates[key] = new SynchronizedObjectReceivedState
		{
			Data = mergedData,
			ServerTime = serverTime
		};
		return true;
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00037BF0 File Offset: 0x00035DF0
	private void ConfigureTimeline()
	{
		this.timeline.Configure(Mathf.RoundToInt(1f / this.TickInterval), this.smoothingSettings.TimescaleCorrectionGain, this.smoothingSettings.MaxTimescaleCorrection, this.smoothingSettings.DriftEmaDuration, this.smoothingSettings.MaxTimelineDriftTime);
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x00037C48 File Offset: 0x00035E48
	private void ResetInterpolationState()
	{
		this.timeline.Reset();
		foreach (SynchronizedObject synchronizedObject in this.registry.Objects)
		{
			if (synchronizedObject)
			{
				synchronizedObject.Client_ResetInterpolation();
			}
		}
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x00037CB4 File Offset: 0x00035EB4
	private void TriggerDiagnosticsEvents(float deltaTime)
	{
		SynchronizedObjectClientDiagnosticsData synchronizedObjectClientDiagnosticsData;
		if (!this.diagnostics.TryGetData(deltaTime, out synchronizedObjectClientDiagnosticsData))
		{
			return;
		}
		if (!EventManager.HasEventListeners("Event_Client_OnObjectSynchronizationDiagnostics"))
		{
			return;
		}
		synchronizedObjectClientDiagnosticsData.TickInterval = this.TickInterval;
		synchronizedObjectClientDiagnosticsData.TargetTimelinePosition = (double)this.smoothingSettings.TargetTimelinePosition;
		EventManager.TriggerEvent("Event_Client_OnObjectSynchronizationDiagnostics", new Dictionary<string, object>
		{
			{
				"diagnosticsData",
				synchronizedObjectClientDiagnosticsData
			}
		});
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x00037D1F File Offset: 0x00035F1F
	private void TriggerSynchronizeEvents(float serverDeltaTime)
	{
		if (EventManager.HasEventListeners("Event_Client_OnSynchronizeObjects"))
		{
			EventManager.TriggerEvent("Event_Client_OnSynchronizeObjects", new Dictionary<string, object>
			{
				{
					"serverDeltaTime",
					serverDeltaTime
				}
			});
		}
	}

	// Token: 0x040006FC RID: 1788
	private SynchronizedObjectRegistry registry;

	// Token: 0x040006FD RID: 1789
	private SynchronizedObjectNetworkSmoothingSettings smoothingSettings = SynchronizedObjectNetworkSmoothingSettings.Default;

	// Token: 0x040006FE RID: 1790
	private float fallbackTickInterval;

	// Token: 0x040006FF RID: 1791
	private Dictionary<ulong, SynchronizedObjectReceivedState> receivedStates = new Dictionary<ulong, SynchronizedObjectReceivedState>();

	// Token: 0x04000700 RID: 1792
	private ushort lastReceivedSequenceNumber;

	// Token: 0x04000701 RID: 1793
	private double lastReceivedServerTime;

	// Token: 0x04000702 RID: 1794
	private double lastReceivedLocalTime;

	// Token: 0x04000703 RID: 1795
	private bool hasReceivedFirstTick;

	// Token: 0x04000704 RID: 1796
	private bool hasPendingTimelineAdjustment;

	// Token: 0x04000705 RID: 1797
	private float receivedTickInterval;

	// Token: 0x04000706 RID: 1798
	private float timeScale = 1f;

	// Token: 0x04000707 RID: 1799
	private SynchronizedObjectClientDiagnostics diagnostics = new SynchronizedObjectClientDiagnostics();

	// Token: 0x04000708 RID: 1800
	private SynchronizedObjectTimeline timeline = new SynchronizedObjectTimeline();
}
