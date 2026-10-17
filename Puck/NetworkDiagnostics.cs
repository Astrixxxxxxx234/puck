using System;

// Token: 0x02000105 RID: 261
public struct NetworkDiagnostics
{
	// Token: 0x170000EF RID: 239
	// (get) Token: 0x0600081D RID: 2077 RVA: 0x000295B1 File Offset: 0x000277B1
	public float SentKilobytesPerSecond
	{
		get
		{
			return this.GetKilobytesPerSecond(this.SentWireByteCount);
		}
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x0600081E RID: 2078 RVA: 0x000295BF File Offset: 0x000277BF
	public float ReceivedKilobytesPerSecond
	{
		get
		{
			return this.GetKilobytesPerSecond(this.ReceivedWireByteCount);
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x0600081F RID: 2079 RVA: 0x000295CD File Offset: 0x000277CD
	public float SentPacketsPerSecond
	{
		get
		{
			return this.GetPerSecond(this.SentPacketCount);
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000820 RID: 2080 RVA: 0x000295DB File Offset: 0x000277DB
	public float ReceivedPacketsPerSecond
	{
		get
		{
			return this.GetPerSecond(this.ReceivedPacketCount);
		}
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x000295EC File Offset: 0x000277EC
	public static string DescribeHeader()
	{
		return string.Concat(new string[]
		{
			NetworkDiagnostics.Column("sent KB/s"),
			NetworkDiagnostics.Column("sent packets/s"),
			NetworkDiagnostics.Column("received KB/s"),
			NetworkDiagnostics.Column("received packets/s"),
			NetworkDiagnostics.Column("max stall ms")
		});
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00029648 File Offset: 0x00027848
	public string Describe()
	{
		return string.Concat(new string[]
		{
			NetworkDiagnostics.Column(this.SentKilobytesPerSecond),
			NetworkDiagnostics.Column(this.SentPacketsPerSecond),
			NetworkDiagnostics.Column(this.ReceivedKilobytesPerSecond),
			NetworkDiagnostics.Column(this.ReceivedPacketsPerSecond),
			NetworkDiagnostics.Column((float)(this.MaxStallTime * 1000.0))
		});
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x0001DC35 File Offset: 0x0001BE35
	private static string Column(string text)
	{
		return text.PadLeft(20);
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x000296B1 File Offset: 0x000278B1
	private static string Column(float value)
	{
		return NetworkDiagnostics.Column(value.ToString("F1"));
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x000296C4 File Offset: 0x000278C4
	private float GetKilobytesPerSecond(ulong byteCount)
	{
		return this.GetPerSecond(byteCount) / 1024f;
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x000296D3 File Offset: 0x000278D3
	private float GetPerSecond(ulong count)
	{
		if (this.WindowTime <= 0f)
		{
			return 0f;
		}
		return count / this.WindowTime;
	}

	// Token: 0x04000514 RID: 1300
	private const int COLUMN_WIDTH = 20;

	// Token: 0x04000515 RID: 1301
	public float WindowTime;

	// Token: 0x04000516 RID: 1302
	public double MaxStallTime;

	// Token: 0x04000517 RID: 1303
	public ulong SentWireByteCount;

	// Token: 0x04000518 RID: 1304
	public ulong ReceivedWireByteCount;

	// Token: 0x04000519 RID: 1305
	public ulong SentPacketCount;

	// Token: 0x0400051A RID: 1306
	public ulong ReceivedPacketCount;
}
