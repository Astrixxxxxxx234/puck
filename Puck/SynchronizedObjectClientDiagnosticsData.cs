using System;

// Token: 0x02000178 RID: 376
public struct SynchronizedObjectClientDiagnosticsData
{
	// Token: 0x040006E3 RID: 1763
	public float WindowTime;

	// Token: 0x040006E4 RID: 1764
	public int TickCount;

	// Token: 0x040006E5 RID: 1765
	public double AverageTickDeltaTime;

	// Token: 0x040006E6 RID: 1766
	public double P95TickDeltaTime;

	// Token: 0x040006E7 RID: 1767
	public double MaxTickDeltaTime;

	// Token: 0x040006E8 RID: 1768
	public float TickLossPercentage;

	// Token: 0x040006E9 RID: 1769
	public int OutOfOrderTickCount;

	// Token: 0x040006EA RID: 1770
	public float TickInterval;

	// Token: 0x040006EB RID: 1771
	public double TargetTimelinePosition;

	// Token: 0x040006EC RID: 1772
	public double AverageTimelinePosition;

	// Token: 0x040006ED RID: 1773
	public double AverageTimelineTimescale;

	// Token: 0x040006EE RID: 1774
	public int SnapCount;

	// Token: 0x040006EF RID: 1775
	public float ExtrapolationPercentage;
}
