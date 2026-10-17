using System;

// Token: 0x02000276 RID: 630
public class PlayerData
{
	// Token: 0x1700019E RID: 414
	// (get) Token: 0x0600130C RID: 4876 RVA: 0x00056AE5 File Offset: 0x00054CE5
	// (set) Token: 0x0600130D RID: 4877 RVA: 0x00056AED File Offset: 0x00054CED
	public string steamId { get; set; }

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x0600130E RID: 4878 RVA: 0x00056AF6 File Offset: 0x00054CF6
	// (set) Token: 0x0600130F RID: 4879 RVA: 0x00056AFE File Offset: 0x00054CFE
	public string username { get; set; }

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06001310 RID: 4880 RVA: 0x00056B07 File Offset: 0x00054D07
	// (set) Token: 0x06001311 RID: 4881 RVA: 0x00056B0F File Offset: 0x00054D0F
	public int number { get; set; }

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06001312 RID: 4882 RVA: 0x00056B18 File Offset: 0x00054D18
	// (set) Token: 0x06001313 RID: 4883 RVA: 0x00056B20 File Offset: 0x00054D20
	public double? usernameChangedAt { get; set; }

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06001314 RID: 4884 RVA: 0x00056B29 File Offset: 0x00054D29
	// (set) Token: 0x06001315 RID: 4885 RVA: 0x00056B31 File Offset: 0x00054D31
	public int patreonLevel { get; set; }

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06001316 RID: 4886 RVA: 0x00056B3A File Offset: 0x00054D3A
	// (set) Token: 0x06001317 RID: 4887 RVA: 0x00056B42 File Offset: 0x00054D42
	public int mmr { get; set; }

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06001318 RID: 4888 RVA: 0x00056B4B File Offset: 0x00054D4B
	// (set) Token: 0x06001319 RID: 4889 RVA: 0x00056B53 File Offset: 0x00054D53
	public int adminLevel { get; set; }

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x0600131A RID: 4890 RVA: 0x00056B5C File Offset: 0x00054D5C
	// (set) Token: 0x0600131B RID: 4891 RVA: 0x00056B64 File Offset: 0x00054D64
	public PlayerItem[] items { get; set; }

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x0600131C RID: 4892 RVA: 0x00056B6D File Offset: 0x00054D6D
	// (set) Token: 0x0600131D RID: 4893 RVA: 0x00056B75 File Offset: 0x00054D75
	public PlayerMute[] mutes { get; set; }

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x0600131E RID: 4894 RVA: 0x00056B7E File Offset: 0x00054D7E
	// (set) Token: 0x0600131F RID: 4895 RVA: 0x00056B86 File Offset: 0x00054D86
	public PlayerBan[] bans { get; set; }

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06001320 RID: 4896 RVA: 0x00056B8F File Offset: 0x00054D8F
	// (set) Token: 0x06001321 RID: 4897 RVA: 0x00056B97 File Offset: 0x00054D97
	public PlayerCooldown[] cooldowns { get; set; }
}
