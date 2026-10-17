using System;
using System.Collections.Generic;
using System.Text.Json;
using SocketIOClient;
using SocketIOClient.Common.Messages;

// Token: 0x0200019D RID: 413
public class InMessage
{
	// Token: 0x06000CD2 RID: 3282 RVA: 0x0003CDE1 File Offset: 0x0003AFE1
	public InMessage(string messageName, IEventContext eventContext = null, IDataMessage dataMessage = null)
	{
		this.MessageName = messageName;
		this.EventContext = eventContext;
		this.DataMessage = dataMessage;
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x0003CE00 File Offset: 0x0003B000
	public T GetData<T>()
	{
		if (this.EventContext != null)
		{
			return this.EventContext.GetValue<T>(0);
		}
		if (this.DataMessage != null)
		{
			return this.DataMessage.GetValue<T>(0);
		}
		return default(T);
	}

	// Token: 0x06000CD4 RID: 3284 RVA: 0x0003CE40 File Offset: 0x0003B040
	public void Respond(Dictionary<string, object> data)
	{
		try
		{
			if (this.EventContext == null)
			{
				throw new Exception("Cannot send response to message without event context");
			}
			string text = JsonSerializer.Serialize<Dictionary<string, object>>(data, WebSocketManager.JsonOptions);
			InMessage.Logger.Info(string.Concat(new string[]
			{
				"WebSocket sending response to message ",
				this.MessageName,
				" (",
				text,
				")"
			}));
			this.EventContext.SendAckDataAsync(new object[]
			{
				data
			});
		}
		catch (Exception ex)
		{
			InMessage.Logger.Error("WebSocket failed to send response to message " + this.MessageName + ": " + ex.Message);
		}
	}

	// Token: 0x06000CD5 RID: 3285 RVA: 0x0003CEF8 File Offset: 0x0003B0F8
	public override string ToString()
	{
		if (this.EventContext != null)
		{
			return this.EventContext.RawText;
		}
		if (this.DataMessage != null)
		{
			return this.DataMessage.RawText;
		}
		return null;
	}

	// Token: 0x040007F6 RID: 2038
	private static readonly Logger Logger = new Logger("WebsocketManager");

	// Token: 0x040007F7 RID: 2039
	public readonly string MessageName;

	// Token: 0x040007F8 RID: 2040
	public readonly IEventContext EventContext;

	// Token: 0x040007F9 RID: 2041
	public readonly IDataMessage DataMessage;
}
