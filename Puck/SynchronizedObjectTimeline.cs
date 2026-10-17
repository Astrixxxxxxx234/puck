using System;

// Token: 0x0200018D RID: 397
public class SynchronizedObjectTimeline
{
	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06000C48 RID: 3144 RVA: 0x0003A877 File Offset: 0x00038A77
	// (set) Token: 0x06000C49 RID: 3145 RVA: 0x0003A87F File Offset: 0x00038A7F
	public bool IsInitialized { get; private set; }

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06000C4A RID: 3146 RVA: 0x0003A888 File Offset: 0x00038A88
	public double Time
	{
		get
		{
			return this.localTimeline;
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000C4B RID: 3147 RVA: 0x0003A890 File Offset: 0x00038A90
	public double Timescale
	{
		get
		{
			return this.localTimescale;
		}
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0003A898 File Offset: 0x00038A98
	public void Configure(int sampleRate, float timescaleCorrectionGain, float maxTimescaleCorrection, int driftEmaDuration, float maxDriftTime)
	{
		this.timescaleCorrectionGain = timescaleCorrectionGain;
		this.maxTimescaleCorrection = maxTimescaleCorrection;
		this.maxDriftTime = maxDriftTime;
		this.driftEma = new ExponentialMovingAverage(Math.Max(1, sampleRate * driftEmaDuration));
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x0003A8C8 File Offset: 0x00038AC8
	public bool Adjust(double serverTime)
	{
		if (!this.IsInitialized)
		{
			this.localTimeline = serverTime;
			this.IsInitialized = true;
		}
		double num = serverTime - this.localTimeline;
		if (this.IsBeyondSnapThreshold(num))
		{
			this.Snap(serverTime);
			return true;
		}
		this.driftEma.Add(num);
		double num2 = Math.Clamp(this.driftEma.Value * (double)this.timescaleCorrectionGain, (double)(-(double)this.maxTimescaleCorrection), (double)this.maxTimescaleCorrection);
		this.localTimescale = 1.0 + num2;
		return false;
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x0003A94C File Offset: 0x00038B4C
	private bool IsBeyondSnapThreshold(double driftTime)
	{
		return this.maxDriftTime > 0f && Math.Abs(driftTime) > (double)this.maxDriftTime;
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x0003A96C File Offset: 0x00038B6C
	private void Snap(double serverTime)
	{
		this.localTimeline = serverTime;
		this.localTimescale = 1.0;
		this.driftEma.Reset();
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x0003A98F File Offset: 0x00038B8F
	public void Step(double deltaTime)
	{
		NetworkingUtils.StepTime(deltaTime, ref this.localTimeline, this.localTimescale);
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x0003A9A3 File Offset: 0x00038BA3
	public void Reset()
	{
		this.localTimeline = 0.0;
		this.localTimescale = 1.0;
		this.IsInitialized = false;
		this.driftEma.Reset();
	}

	// Token: 0x0400079D RID: 1949
	private double localTimeline;

	// Token: 0x0400079E RID: 1950
	private double localTimescale = 1.0;

	// Token: 0x0400079F RID: 1951
	private float timescaleCorrectionGain = 2f;

	// Token: 0x040007A0 RID: 1952
	private float maxTimescaleCorrection = 0.04f;

	// Token: 0x040007A1 RID: 1953
	private float maxDriftTime;

	// Token: 0x040007A2 RID: 1954
	private ExponentialMovingAverage driftEma;
}
