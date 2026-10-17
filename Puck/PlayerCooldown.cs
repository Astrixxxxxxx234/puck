using System;

// Token: 0x02000281 RID: 641
public class PlayerCooldown
{
	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06001360 RID: 4960 RVA: 0x00056DD0 File Offset: 0x00054FD0
	// (set) Token: 0x06001361 RID: 4961 RVA: 0x00056DD8 File Offset: 0x00054FD8
	public int id { get; set; }

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x06001362 RID: 4962 RVA: 0x00056DE1 File Offset: 0x00054FE1
	// (set) Token: 0x06001363 RID: 4963 RVA: 0x00056DE9 File Offset: 0x00054FE9
	public string matchId { get; set; }

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06001364 RID: 4964 RVA: 0x00056DF2 File Offset: 0x00054FF2
	// (set) Token: 0x06001365 RID: 4965 RVA: 0x00056DFA File Offset: 0x00054FFA
	public double issuedAt { get; set; }

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06001366 RID: 4966 RVA: 0x00056E03 File Offset: 0x00055003
	// (set) Token: 0x06001367 RID: 4967 RVA: 0x00056E0B File Offset: 0x0005500B
	public double expiresAt { get; set; }
}
