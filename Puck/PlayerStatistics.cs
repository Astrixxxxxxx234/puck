using System;

// Token: 0x0200027E RID: 638
public class PlayerStatistics
{
	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x06001347 RID: 4935 RVA: 0x00056D15 File Offset: 0x00054F15
	// (set) Token: 0x06001348 RID: 4936 RVA: 0x00056D1D File Offset: 0x00054F1D
	public PlayerManagerStatistics playerManager { get; set; }

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x06001349 RID: 4937 RVA: 0x00056D26 File Offset: 0x00054F26
	// (set) Token: 0x0600134A RID: 4938 RVA: 0x00056D2E File Offset: 0x00054F2E
	public ServerManagerStatistics serverManager { get; set; }

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x0600134B RID: 4939 RVA: 0x00056D37 File Offset: 0x00054F37
	// (set) Token: 0x0600134C RID: 4940 RVA: 0x00056D3F File Offset: 0x00054F3F
	public MatchmakingManagerStatistics matchmakingManager { get; set; }
}
