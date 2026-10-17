using System;

// Token: 0x02000277 RID: 631
public class PlayerPartyData
{
	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06001323 RID: 4899 RVA: 0x00056BA0 File Offset: 0x00054DA0
	// (set) Token: 0x06001324 RID: 4900 RVA: 0x00056BA8 File Offset: 0x00054DA8
	public string id { get; set; }

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06001325 RID: 4901 RVA: 0x00056BB1 File Offset: 0x00054DB1
	// (set) Token: 0x06001326 RID: 4902 RVA: 0x00056BB9 File Offset: 0x00054DB9
	public string steamLobbyId { get; set; }

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06001327 RID: 4903 RVA: 0x00056BC2 File Offset: 0x00054DC2
	// (set) Token: 0x06001328 RID: 4904 RVA: 0x00056BCA File Offset: 0x00054DCA
	public string ownerSteamId { get; set; }

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06001329 RID: 4905 RVA: 0x00056BD3 File Offset: 0x00054DD3
	// (set) Token: 0x0600132A RID: 4906 RVA: 0x00056BDB File Offset: 0x00054DDB
	public string[] memberSteamIds { get; set; }
}
