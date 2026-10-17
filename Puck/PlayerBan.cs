using System;

// Token: 0x02000280 RID: 640
public class PlayerBan
{
	// Token: 0x170001BF RID: 447
	// (get) Token: 0x06001357 RID: 4951 RVA: 0x00056D8C File Offset: 0x00054F8C
	// (set) Token: 0x06001358 RID: 4952 RVA: 0x00056D94 File Offset: 0x00054F94
	public int id { get; set; }

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x06001359 RID: 4953 RVA: 0x00056D9D File Offset: 0x00054F9D
	// (set) Token: 0x0600135A RID: 4954 RVA: 0x00056DA5 File Offset: 0x00054FA5
	public double issuedAt { get; set; }

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x0600135B RID: 4955 RVA: 0x00056DAE File Offset: 0x00054FAE
	// (set) Token: 0x0600135C RID: 4956 RVA: 0x00056DB6 File Offset: 0x00054FB6
	public double expiresAt { get; set; }

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x0600135D RID: 4957 RVA: 0x00056DBF File Offset: 0x00054FBF
	// (set) Token: 0x0600135E RID: 4958 RVA: 0x00056DC7 File Offset: 0x00054FC7
	public string reason { get; set; }
}
