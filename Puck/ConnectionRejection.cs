using System;

// Token: 0x020000FE RID: 254
public class ConnectionRejection
{
	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x060007EB RID: 2027 RVA: 0x00028885 File Offset: 0x00026A85
	// (set) Token: 0x060007EC RID: 2028 RVA: 0x0002888D File Offset: 0x00026A8D
	public ConnectionRejectionCode code { get; set; }

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x060007ED RID: 2029 RVA: 0x00028896 File Offset: 0x00026A96
	// (set) Token: 0x060007EE RID: 2030 RVA: 0x0002889E File Offset: 0x00026A9E
	public string message { get; set; }

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x060007EF RID: 2031 RVA: 0x000288A7 File Offset: 0x00026AA7
	// (set) Token: 0x060007F0 RID: 2032 RVA: 0x000288AF File Offset: 0x00026AAF
	public ConnectionRejectionData data { get; set; }
}
