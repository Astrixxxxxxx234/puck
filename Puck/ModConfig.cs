using System;

// Token: 0x02000151 RID: 337
public class ModConfig
{
	// Token: 0x17000109 RID: 265
	// (get) Token: 0x060009CB RID: 2507 RVA: 0x0002F6C4 File Offset: 0x0002D8C4
	// (set) Token: 0x060009CC RID: 2508 RVA: 0x0002F6CC File Offset: 0x0002D8CC
	public string id { get; set; }

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x060009CD RID: 2509 RVA: 0x0002F6D5 File Offset: 0x0002D8D5
	// (set) Token: 0x060009CE RID: 2510 RVA: 0x0002F6DD File Offset: 0x0002D8DD
	public bool isEnabled { get; set; }

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x060009CF RID: 2511 RVA: 0x0002F6E6 File Offset: 0x0002D8E6
	// (set) Token: 0x060009D0 RID: 2512 RVA: 0x0002F6EE File Offset: 0x0002D8EE
	public bool isClientRequired { get; set; }
}
