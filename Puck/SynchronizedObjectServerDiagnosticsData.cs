using System;

// Token: 0x02000189 RID: 393
public struct SynchronizedObjectServerDiagnosticsData
{
	// Token: 0x06000C2C RID: 3116 RVA: 0x0003A22C File Offset: 0x0003842C
	public string DescribeHeader()
	{
		string str = "player".PadRight(24) + SynchronizedObjectPlayerDiagnosticsData.Column("default");
		for (int i = 0; i < this.BandMinDistances.Length; i++)
		{
			str += SynchronizedObjectPlayerDiagnosticsData.Column(string.Format("{0}m", this.BandMinDistances[i]));
		}
		return str + SynchronizedObjectPlayerDiagnosticsData.Column("culled") + SynchronizedObjectPlayerDiagnosticsData.Column("noOrigin");
	}

	// Token: 0x04000789 RID: 1929
	public float[] BandMinDistances;

	// Token: 0x0400078A RID: 1930
	public SynchronizedObjectPlayerDiagnosticsData[] Players;
}
