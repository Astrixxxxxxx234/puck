using System;

// Token: 0x02000289 RID: 649
public class MatchPlayer
{
	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x060013A9 RID: 5033 RVA: 0x000571DE File Offset: 0x000553DE
	// (set) Token: 0x060013AA RID: 5034 RVA: 0x000571E6 File Offset: 0x000553E6
	public string steamId { get; set; }

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x060013AB RID: 5035 RVA: 0x000571EF File Offset: 0x000553EF
	// (set) Token: 0x060013AC RID: 5036 RVA: 0x000571F7 File Offset: 0x000553F7
	public string username { get; set; }

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x060013AD RID: 5037 RVA: 0x00057200 File Offset: 0x00055400
	// (set) Token: 0x060013AE RID: 5038 RVA: 0x00057208 File Offset: 0x00055408
	public int mmr { get; set; }
}
