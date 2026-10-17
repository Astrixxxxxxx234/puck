using System;
using System.Collections.Generic;

// Token: 0x020001A8 RID: 424
public static class WebSocketManagerController
{
	// Token: 0x06000D07 RID: 3335 RVA: 0x0003E06E File Offset: 0x0003C26E
	public static void Initialize()
	{
		EventManager.AddEventListener("Event_OnSteamConnected", new Action<Dictionary<string, object>>(WebSocketManagerController.Event_OnSteamConnected));
	}

	// Token: 0x06000D08 RID: 3336 RVA: 0x0003E086 File Offset: 0x0003C286
	public static void Dispose()
	{
		WebSocketManager.Disconnect();
		EventManager.RemoveEventListener("Event_OnSteamConnected", new Action<Dictionary<string, object>>(WebSocketManagerController.Event_OnSteamConnected));
	}

	// Token: 0x06000D09 RID: 3337 RVA: 0x0003E0A4 File Offset: 0x0003C2A4
	private static void Event_OnSteamConnected(Dictionary<string, object> message)
	{
		if (WebSocketManager.IsConnected || WebSocketManager.IsConnectionInProgress || WebSocketManager.IsReconnecting)
		{
			return;
		}
		WebSocketManager.Connect(WebSocketManager.BackendUrl);
	}
}
