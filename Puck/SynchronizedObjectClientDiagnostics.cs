using System;

// Token: 0x02000179 RID: 377
public class SynchronizedObjectClientDiagnostics
{
	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000B9A RID: 2970 RVA: 0x000372F4 File Offset: 0x000354F4
	public bool IsEnabled
	{
		get
		{
			return this.reportInterval > 0f;
		}
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x00037303 File Offset: 0x00035503
	public void Configure(float reportInterval)
	{
		this.reportInterval = reportInterval;
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x0003730C File Offset: 0x0003550C
	public void RecordTick(float tickDeltaTime, int lostTickCount)
	{
		if (!this.IsEnabled)
		{
			return;
		}
		this.tickCount++;
		this.lostTickCount += lostTickCount;
		if (tickDeltaTime <= 0f)
		{
			return;
		}
		this.tickDeltaTimeHistogram.Add(tickDeltaTime);
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x00037348 File Offset: 0x00035548
	public void RecordOutOfOrderTick()
	{
		if (!this.IsEnabled)
		{
			return;
		}
		this.outOfOrderTickCount++;
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x00037361 File Offset: 0x00035561
	public void RecordTimeline(double timelinePosition, double timelineTimescale)
	{
		if (!this.IsEnabled)
		{
			return;
		}
		this.timelinePositionSum += timelinePosition;
		this.timelineTimescaleSum += timelineTimescale;
		this.timelineSampleCount++;
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x00037396 File Offset: 0x00035596
	public void RecordSnap()
	{
		if (!this.IsEnabled)
		{
			return;
		}
		this.snapCount++;
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x000373AF File Offset: 0x000355AF
	public void RecordPlayback(SynchronizedObjectPlayback playback)
	{
		if (!this.IsEnabled || playback == SynchronizedObjectPlayback.None)
		{
			return;
		}
		this.playbackSampleCount++;
		if (playback == SynchronizedObjectPlayback.Extrapolated)
		{
			this.extrapolatedPlaybackCount++;
		}
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x000373E0 File Offset: 0x000355E0
	public bool TryGetData(float deltaTime, out SynchronizedObjectClientDiagnosticsData data)
	{
		data = default(SynchronizedObjectClientDiagnosticsData);
		if (!this.IsEnabled)
		{
			return false;
		}
		this.reportTimer += deltaTime;
		if (this.reportTimer < this.reportInterval)
		{
			return false;
		}
		if (this.tickCount == 0)
		{
			this.Reset();
			return false;
		}
		data = this.BuildData();
		this.Reset();
		return true;
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x00037440 File Offset: 0x00035640
	public void Reset()
	{
		this.reportTimer = 0f;
		this.tickCount = 0;
		this.lostTickCount = 0;
		this.outOfOrderTickCount = 0;
		this.tickDeltaTimeHistogram.Reset();
		this.timelinePositionSum = 0.0;
		this.timelineTimescaleSum = 0.0;
		this.timelineSampleCount = 0;
		this.snapCount = 0;
		this.playbackSampleCount = 0;
		this.extrapolatedPlaybackCount = 0;
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x000374B4 File Offset: 0x000356B4
	private SynchronizedObjectClientDiagnosticsData BuildData()
	{
		int num = Math.Max(0, this.lostTickCount - this.outOfOrderTickCount);
		int num2 = this.tickCount + num;
		return new SynchronizedObjectClientDiagnosticsData
		{
			WindowTime = this.reportTimer,
			TickCount = this.tickCount,
			AverageTickDeltaTime = this.tickDeltaTimeHistogram.Average,
			P95TickDeltaTime = this.tickDeltaTimeHistogram.GetPercentile(0.95),
			MaxTickDeltaTime = this.tickDeltaTimeHistogram.Max,
			TickLossPercentage = ((num2 > 0) ? (100f * (float)num / (float)num2) : 0f),
			OutOfOrderTickCount = this.outOfOrderTickCount,
			AverageTimelinePosition = ((this.timelineSampleCount > 0) ? (this.timelinePositionSum / (double)this.timelineSampleCount) : 0.0),
			AverageTimelineTimescale = ((this.timelineSampleCount > 0) ? (this.timelineTimescaleSum / (double)this.timelineSampleCount) : 1.0),
			SnapCount = this.snapCount,
			ExtrapolationPercentage = ((this.playbackSampleCount > 0) ? (100f * (float)this.extrapolatedPlaybackCount / (float)this.playbackSampleCount) : 0f)
		};
	}

	// Token: 0x040006F0 RID: 1776
	private float reportInterval;

	// Token: 0x040006F1 RID: 1777
	private float reportTimer;

	// Token: 0x040006F2 RID: 1778
	private int tickCount;

	// Token: 0x040006F3 RID: 1779
	private int lostTickCount;

	// Token: 0x040006F4 RID: 1780
	private int outOfOrderTickCount;

	// Token: 0x040006F5 RID: 1781
	private TimeHistogram tickDeltaTimeHistogram = new TimeHistogram(0.0005f, 101);

	// Token: 0x040006F6 RID: 1782
	private double timelinePositionSum;

	// Token: 0x040006F7 RID: 1783
	private double timelineTimescaleSum;

	// Token: 0x040006F8 RID: 1784
	private int timelineSampleCount;

	// Token: 0x040006F9 RID: 1785
	private int snapCount;

	// Token: 0x040006FA RID: 1786
	private int playbackSampleCount;

	// Token: 0x040006FB RID: 1787
	private int extrapolatedPlaybackCount;
}
