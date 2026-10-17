using System;
using System.Collections.Generic;

// Token: 0x0200018A RID: 394
public class SynchronizedObjectServerDiagnostics
{
	// Token: 0x1700013E RID: 318
	// (get) Token: 0x06000C2D RID: 3117 RVA: 0x0003A2A5 File Offset: 0x000384A5
	private bool IsEnabled
	{
		get
		{
			return this.reportInterval > 0f;
		}
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x0003A2B4 File Offset: 0x000384B4
	public void Configure(float reportInterval, SynchronizedObjectBandSettings[] lodBands)
	{
		this.reportInterval = reportInterval;
		this.lodBands = lodBands;
		this.lodCounts.Configure((lodBands != null) ? lodBands.Length : 0);
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x0003A2D8 File Offset: 0x000384D8
	public void Step(float deltaTime)
	{
		this.isReportDue = false;
		if (!this.IsEnabled)
		{
			return;
		}
		this.reportTimer += deltaTime;
		if (this.reportTimer < this.reportInterval)
		{
			return;
		}
		this.reportTimer = 0f;
		this.isReportDue = true;
		this.playersData.Clear();
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x0003A32F File Offset: 0x0003852F
	public SynchronizedObjectLodCounts BeginPlayer()
	{
		if (!this.isReportDue)
		{
			return null;
		}
		this.lodCounts.Clear();
		return this.lodCounts;
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x0003A34C File Offset: 0x0003854C
	public void EndPlayer(SynchronizedPlayerState playerState)
	{
		if (!this.isReportDue)
		{
			return;
		}
		this.playersData.Add(this.BuildPlayerData(playerState.Player));
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x0003A370 File Offset: 0x00038570
	public bool TryGetData(out SynchronizedObjectServerDiagnosticsData data)
	{
		data = default(SynchronizedObjectServerDiagnosticsData);
		if (!this.isReportDue || this.playersData.Count == 0)
		{
			return false;
		}
		data = new SynchronizedObjectServerDiagnosticsData
		{
			BandMinDistances = this.TakeBandMinDistances(),
			Players = this.playersData.ToArray()
		};
		return true;
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x0003A3CA File Offset: 0x000385CA
	public void Reset()
	{
		this.reportTimer = 0f;
		this.isReportDue = false;
		this.playersData.Clear();
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x0003A3EC File Offset: 0x000385EC
	private float[] TakeBandMinDistances()
	{
		float[] array = new float[(this.lodBands != null) ? this.lodBands.Length : 0];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.lodBands[i].MinDistance;
		}
		return array;
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x0003A438 File Offset: 0x00038638
	private SynchronizedObjectPlayerDiagnosticsData BuildPlayerData(Player player)
	{
		return new SynchronizedObjectPlayerDiagnosticsData
		{
			ClientId = (player ? player.OwnerClientId : 0UL),
			Username = (player ? player.Username.Value.ToString() : "unknown"),
			DefaultCount = this.lodCounts.DefaultCount,
			CulledCount = this.lodCounts.CulledCount,
			NoOriginCount = this.lodCounts.NoOriginCount,
			BandCounts = this.lodCounts.TakeBandCounts()
		};
	}

	// Token: 0x0400078B RID: 1931
	private SynchronizedObjectLodCounts lodCounts = new SynchronizedObjectLodCounts();

	// Token: 0x0400078C RID: 1932
	private List<SynchronizedObjectPlayerDiagnosticsData> playersData = new List<SynchronizedObjectPlayerDiagnosticsData>();

	// Token: 0x0400078D RID: 1933
	private SynchronizedObjectBandSettings[] lodBands;

	// Token: 0x0400078E RID: 1934
	private float reportInterval;

	// Token: 0x0400078F RID: 1935
	private float reportTimer;

	// Token: 0x04000790 RID: 1936
	private bool isReportDue;
}
