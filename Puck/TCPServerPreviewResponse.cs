using System;

// Token: 0x02000251 RID: 593
public class TCPServerPreviewResponse : TCPServerMessage
{
	// Token: 0x17000183 RID: 387
	// (get) Token: 0x060012AB RID: 4779 RVA: 0x00056823 File Offset: 0x00054A23
	// (set) Token: 0x060012AC RID: 4780 RVA: 0x0005682B File Offset: 0x00054A2B
	public string name { get; set; }

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x060012AD RID: 4781 RVA: 0x00056834 File Offset: 0x00054A34
	// (set) Token: 0x060012AE RID: 4782 RVA: 0x0005683C File Offset: 0x00054A3C
	public int players { get; set; }

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x060012AF RID: 4783 RVA: 0x00056845 File Offset: 0x00054A45
	// (set) Token: 0x060012B0 RID: 4784 RVA: 0x0005684D File Offset: 0x00054A4D
	public int maxPlayers { get; set; }

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x060012B1 RID: 4785 RVA: 0x00056856 File Offset: 0x00054A56
	// (set) Token: 0x060012B2 RID: 4786 RVA: 0x0005685E File Offset: 0x00054A5E
	public bool isPasswordProtected { get; set; }

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x060012B3 RID: 4787 RVA: 0x00056867 File Offset: 0x00054A67
	// (set) Token: 0x060012B4 RID: 4788 RVA: 0x0005686F File Offset: 0x00054A6F
	public string[] clientRequiredModIds { get; set; }

	// Token: 0x060012B5 RID: 4789 RVA: 0x00056878 File Offset: 0x00054A78
	public TCPServerPreviewResponse()
	{
		base.type = TCPServerMessageType.PreviewResponse;
	}
}
