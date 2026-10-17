using System;

// Token: 0x02000278 RID: 632
public class PlayerGroupData
{
	// Token: 0x170001AD RID: 429
	// (get) Token: 0x0600132C RID: 4908 RVA: 0x00056BE4 File Offset: 0x00054DE4
	// (set) Token: 0x0600132D RID: 4909 RVA: 0x00056BEC File Offset: 0x00054DEC
	public string id { get; set; }

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x0600132E RID: 4910 RVA: 0x00056BF5 File Offset: 0x00054DF5
	// (set) Token: 0x0600132F RID: 4911 RVA: 0x00056BFD File Offset: 0x00054DFD
	public string[] poolIds { get; set; }

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06001330 RID: 4912 RVA: 0x00056C06 File Offset: 0x00054E06
	// (set) Token: 0x06001331 RID: 4913 RVA: 0x00056C0E File Offset: 0x00054E0E
	public string ownerSteamId { get; set; }

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06001332 RID: 4914 RVA: 0x00056C17 File Offset: 0x00054E17
	// (set) Token: 0x06001333 RID: 4915 RVA: 0x00056C1F File Offset: 0x00054E1F
	public string[] memberSteamIds { get; set; }
}
