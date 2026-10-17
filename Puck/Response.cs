using System;

// Token: 0x02000252 RID: 594
public class Response<TSuccessData, TErrorData>
{
	// Token: 0x17000188 RID: 392
	// (get) Token: 0x060012B6 RID: 4790 RVA: 0x00056887 File Offset: 0x00054A87
	// (set) Token: 0x060012B7 RID: 4791 RVA: 0x0005688F File Offset: 0x00054A8F
	public bool success { get; set; }

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x060012B8 RID: 4792 RVA: 0x00056898 File Offset: 0x00054A98
	// (set) Token: 0x060012B9 RID: 4793 RVA: 0x000568A0 File Offset: 0x00054AA0
	public TSuccessData data { get; set; }

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x060012BA RID: 4794 RVA: 0x000568A9 File Offset: 0x00054AA9
	// (set) Token: 0x060012BB RID: 4795 RVA: 0x000568B1 File Offset: 0x00054AB1
	public TErrorData errorData { get; set; }
}
