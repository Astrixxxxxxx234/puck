using System;

// Token: 0x02000225 RID: 549
public class Country
{
	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06001170 RID: 4464 RVA: 0x00052A53 File Offset: 0x00050C53
	// (set) Token: 0x06001171 RID: 4465 RVA: 0x00052A5B File Offset: 0x00050C5B
	public string capital { get; set; }

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x06001172 RID: 4466 RVA: 0x00052A64 File Offset: 0x00050C64
	// (set) Token: 0x06001173 RID: 4467 RVA: 0x00052A6C File Offset: 0x00050C6C
	public string code { get; set; }

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06001174 RID: 4468 RVA: 0x00052A75 File Offset: 0x00050C75
	// (set) Token: 0x06001175 RID: 4469 RVA: 0x00052A7D File Offset: 0x00050C7D
	public string continent { get; set; }

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06001176 RID: 4470 RVA: 0x00052A86 File Offset: 0x00050C86
	// (set) Token: 0x06001177 RID: 4471 RVA: 0x00052A8E File Offset: 0x00050C8E
	public string name { get; set; }

	// Token: 0x04000B67 RID: 2919
	private static readonly Logger Logger = new Logger("Country");
}
