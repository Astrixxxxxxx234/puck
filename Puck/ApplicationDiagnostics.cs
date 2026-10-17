using System;

// Token: 0x0200009E RID: 158
public struct ApplicationDiagnostics
{
	// Token: 0x1700008E RID: 142
	// (get) Token: 0x06000556 RID: 1366 RVA: 0x0001DB95 File Offset: 0x0001BD95
	public float FramesPerSecond
	{
		get
		{
			if (this.WindowTime <= 0f)
			{
				return 0f;
			}
			return (float)this.FrameCount / this.WindowTime;
		}
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x0001DBB8 File Offset: 0x0001BDB8
	public static string DescribeHeader()
	{
		return ApplicationDiagnostics.Column("frames/s") + ApplicationDiagnostics.Column("frame time ms") + ApplicationDiagnostics.Column("p95 ms") + ApplicationDiagnostics.Column("max ms");
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x0001DBE8 File Offset: 0x0001BDE8
	public string Describe()
	{
		return ApplicationDiagnostics.Column(this.FramesPerSecond) + ApplicationDiagnostics.Column(ApplicationDiagnostics.GetMilliseconds(this.AverageFrameTime)) + ApplicationDiagnostics.Column(ApplicationDiagnostics.GetMilliseconds(this.P95FrameTime)) + ApplicationDiagnostics.Column(ApplicationDiagnostics.GetMilliseconds(this.MaxFrameTime));
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x0001DC35 File Offset: 0x0001BE35
	private static string Column(string text)
	{
		return text.PadLeft(20);
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0001DC3F File Offset: 0x0001BE3F
	private static string Column(float value)
	{
		return ApplicationDiagnostics.Column(value.ToString("F1"));
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x0001DC52 File Offset: 0x0001BE52
	private static float GetMilliseconds(double seconds)
	{
		return (float)(seconds * 1000.0);
	}

	// Token: 0x04000375 RID: 885
	private const int COLUMN_WIDTH = 20;

	// Token: 0x04000376 RID: 886
	public float WindowTime;

	// Token: 0x04000377 RID: 887
	public int FrameCount;

	// Token: 0x04000378 RID: 888
	public double AverageFrameTime;

	// Token: 0x04000379 RID: 889
	public double P95FrameTime;

	// Token: 0x0400037A RID: 890
	public double MaxFrameTime;
}
