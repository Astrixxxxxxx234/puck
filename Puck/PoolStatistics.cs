using System;

// Token: 0x0200027C RID: 636
public class PoolStatistics
{
	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x0600133D RID: 4925 RVA: 0x00056CD1 File Offset: 0x00054ED1
	// (set) Token: 0x0600133E RID: 4926 RVA: 0x00056CD9 File Offset: 0x00054ED9
	public string id { get; set; }

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x0600133F RID: 4927 RVA: 0x00056CE2 File Offset: 0x00054EE2
	// (set) Token: 0x06001340 RID: 4928 RVA: 0x00056CEA File Offset: 0x00054EEA
	public double? averageGroupLifetime { get; set; }

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06001341 RID: 4929 RVA: 0x00056CF3 File Offset: 0x00054EF3
	// (set) Token: 0x06001342 RID: 4930 RVA: 0x00056CFB File Offset: 0x00054EFB
	public int groupPlayerCount { get; set; }
}
