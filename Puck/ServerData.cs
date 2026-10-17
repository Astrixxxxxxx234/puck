using System;

// Token: 0x02000283 RID: 643
public class ServerData
{
	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x0600136E RID: 4974 RVA: 0x00056E36 File Offset: 0x00055036
	// (set) Token: 0x0600136F RID: 4975 RVA: 0x00056E3E File Offset: 0x0005503E
	public string id { get; set; }

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x06001370 RID: 4976 RVA: 0x00056E47 File Offset: 0x00055047
	// (set) Token: 0x06001371 RID: 4977 RVA: 0x00056E4F File Offset: 0x0005504F
	public string ipAddress { get; set; }

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x06001372 RID: 4978 RVA: 0x00056E58 File Offset: 0x00055058
	// (set) Token: 0x06001373 RID: 4979 RVA: 0x00056E60 File Offset: 0x00055060
	public ushort port { get; set; }
}
