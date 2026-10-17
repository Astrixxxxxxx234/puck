using System;
using System.Collections.Generic;
using System.Text.Json;

// Token: 0x0200019C RID: 412
public class OutMessage
{
	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0003CD70 File Offset: 0x0003AF70
	public bool IsRequestMessage
	{
		get
		{
			return this.ResponseMessageName != null;
		}
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x0003CD7B File Offset: 0x0003AF7B
	public OutMessage(string messageName, Dictionary<string, object> data = null, string responseMessageName = null)
	{
		this.MessageName = messageName;
		this.Data = data;
		this.ResponseMessageName = responseMessageName;
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x0003CD98 File Offset: 0x0003AF98
	public override string ToString()
	{
		string result;
		try
		{
			result = JsonSerializer.Serialize<Dictionary<string, object>>(this.Data, WebSocketManager.JsonOptions);
		}
		catch
		{
			result = null;
		}
		return result;
	}

	// Token: 0x040007F2 RID: 2034
	private static readonly Logger Logger = new Logger("WebsocketManager");

	// Token: 0x040007F3 RID: 2035
	public readonly string MessageName;

	// Token: 0x040007F4 RID: 2036
	public readonly Dictionary<string, object> Data;

	// Token: 0x040007F5 RID: 2037
	public readonly string ResponseMessageName;
}
