using System;

// Token: 0x02000250 RID: 592
public class TCPServerPreviewRequest : TCPServerMessage
{
	// Token: 0x060012AA RID: 4778 RVA: 0x00056814 File Offset: 0x00054A14
	public TCPServerPreviewRequest()
	{
		base.type = TCPServerMessageType.PreviewRequest;
	}
}
