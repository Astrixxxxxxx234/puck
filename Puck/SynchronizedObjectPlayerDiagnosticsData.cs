using System;

// Token: 0x02000188 RID: 392
public struct SynchronizedObjectPlayerDiagnosticsData
{
	// Token: 0x06000C28 RID: 3112 RVA: 0x0003A164 File Offset: 0x00038364
	public string Describe()
	{
		string str = this.DescribePlayer() + SynchronizedObjectPlayerDiagnosticsData.Column(this.DefaultCount);
		for (int i = 0; i < this.BandCounts.Length; i++)
		{
			str += SynchronizedObjectPlayerDiagnosticsData.Column(this.BandCounts[i]);
		}
		return str + SynchronizedObjectPlayerDiagnosticsData.Column(this.CulledCount) + SynchronizedObjectPlayerDiagnosticsData.Column(this.NoOriginCount);
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x0003A1CB File Offset: 0x000383CB
	public static string Column(string text)
	{
		return text.PadLeft(10);
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x0003A1D5 File Offset: 0x000383D5
	private static string Column(int value)
	{
		return SynchronizedObjectPlayerDiagnosticsData.Column(value.ToString());
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x0003A1E4 File Offset: 0x000383E4
	private string DescribePlayer()
	{
		string text = string.Format("{0} ({1})", this.Username, this.ClientId);
		if (text.Length >= 24)
		{
			text = text.Substring(0, 23);
		}
		return text.PadRight(24);
	}

	// Token: 0x04000781 RID: 1921
	public const int PLAYER_COLUMN_WIDTH = 24;

	// Token: 0x04000782 RID: 1922
	public const int COLUMN_WIDTH = 10;

	// Token: 0x04000783 RID: 1923
	public ulong ClientId;

	// Token: 0x04000784 RID: 1924
	public string Username;

	// Token: 0x04000785 RID: 1925
	public int DefaultCount;

	// Token: 0x04000786 RID: 1926
	public int CulledCount;

	// Token: 0x04000787 RID: 1927
	public int NoOriginCount;

	// Token: 0x04000788 RID: 1928
	public int[] BandCounts;
}
