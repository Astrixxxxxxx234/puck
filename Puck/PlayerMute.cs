using System;

// Token: 0x0200027F RID: 639
public class PlayerMute
{
	// Token: 0x170001BB RID: 443
	// (get) Token: 0x0600134E RID: 4942 RVA: 0x00056D48 File Offset: 0x00054F48
	// (set) Token: 0x0600134F RID: 4943 RVA: 0x00056D50 File Offset: 0x00054F50
	public int id { get; set; }

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06001350 RID: 4944 RVA: 0x00056D59 File Offset: 0x00054F59
	// (set) Token: 0x06001351 RID: 4945 RVA: 0x00056D61 File Offset: 0x00054F61
	public double issuedAt { get; set; }

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06001352 RID: 4946 RVA: 0x00056D6A File Offset: 0x00054F6A
	// (set) Token: 0x06001353 RID: 4947 RVA: 0x00056D72 File Offset: 0x00054F72
	public double expiresAt { get; set; }

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06001354 RID: 4948 RVA: 0x00056D7B File Offset: 0x00054F7B
	// (set) Token: 0x06001355 RID: 4949 RVA: 0x00056D83 File Offset: 0x00054F83
	public string reason { get; set; }
}
