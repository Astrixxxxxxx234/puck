using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000176 RID: 374
public static class SteamWorkshopManagerController
{
	// Token: 0x06000B91 RID: 2961 RVA: 0x000370C4 File Offset: 0x000352C4
	public static void Initialize()
	{
		EventManager.AddEventListener("Event_OnSteamConnected", new Action<Dictionary<string, object>>(SteamWorkshopManagerController.Event_OnSteamConnected));
		EventManager.AddEventListener("Event_OnSteamWorkshopSubscribedItemsListChanged", new Action<Dictionary<string, object>>(SteamWorkshopManagerController.Event_OnSteamWorkshopSubscribedItemsListChanged));
		EventManager.AddEventListener("Event_OnSteamWorkshopItemDownloaded", new Action<Dictionary<string, object>>(SteamWorkshopManagerController.Event_OnSteamWorkshopItemDownloaded));
		EventManager.AddEventListener("Event_OnModsClickRefresh", new Action<Dictionary<string, object>>(SteamWorkshopManagerController.Event_OnModsClickRefresh));
		EventManager.AddEventListener("Event_OnReconnectionStateChanged", new Action<Dictionary<string, object>>(SteamWorkshopManagerController.Event_OnReconnectionStateChanged));
		EventManager.AddEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(SteamWorkshopManagerController.Event_Server_OnServerStarted));
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x00037158 File Offset: 0x00035358
	public static void Dispose()
	{
		EventManager.RemoveEventListener("Event_OnSteamConnected", new Action<Dictionary<string, object>>(SteamWorkshopManagerController.Event_OnSteamConnected));
		EventManager.RemoveEventListener("Event_OnSteamWorkshopSubscribedItemsListChanged", new Action<Dictionary<string, object>>(SteamWorkshopManagerController.Event_OnSteamWorkshopSubscribedItemsListChanged));
		EventManager.RemoveEventListener("Event_OnSteamWorkshopItemDownloaded", new Action<Dictionary<string, object>>(SteamWorkshopManagerController.Event_OnSteamWorkshopItemDownloaded));
		EventManager.RemoveEventListener("Event_OnModsClickRefresh", new Action<Dictionary<string, object>>(SteamWorkshopManagerController.Event_OnModsClickRefresh));
		EventManager.RemoveEventListener("Event_OnReconnectionStateChanged", new Action<Dictionary<string, object>>(SteamWorkshopManagerController.Event_OnReconnectionStateChanged));
		EventManager.RemoveEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(SteamWorkshopManagerController.Event_Server_OnServerStarted));
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x000371E9 File Offset: 0x000353E9
	private static void Event_OnSteamConnected(Dictionary<string, object> message)
	{
		if (!ApplicationManager.IsDedicatedGameServer)
		{
			SteamWorkshopManager.VerifyIntegrity();
		}
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x000371F7 File Offset: 0x000353F7
	private static void Event_OnSteamWorkshopSubscribedItemsListChanged(Dictionary<string, object> message)
	{
		SteamWorkshopManager.VerifyIntegrity();
	}

	// Token: 0x06000B95 RID: 2965 RVA: 0x000371FE File Offset: 0x000353FE
	private static void Event_OnSteamWorkshopItemDownloaded(Dictionary<string, object> message)
	{
		SteamWorkshopManager.VerifyItemIntegrity((string)message["itemId"]);
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x000371F7 File Offset: 0x000353F7
	private static void Event_OnModsClickRefresh(Dictionary<string, object> message)
	{
		SteamWorkshopManager.VerifyIntegrity();
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x00037218 File Offset: 0x00035418
	private static void Event_OnReconnectionStateChanged(Dictionary<string, object> message)
	{
		ReconnectionState reconnectionState = (ReconnectionState)message["oldReconnectionState"];
		ReconnectionState reconnectionState2 = (ReconnectionState)message["newReconnectionState"];
		if (reconnectionState2.Phase == ReconnectionPhase.AwaitingMods && !reconnectionState.PendingReadinessModIds.SequenceEqual(reconnectionState2.PendingReadinessModIds))
		{
			foreach (string text in reconnectionState2.PendingReadinessModIds.Except(reconnectionState.PendingReadinessModIds).ToArray<string>())
			{
				if (ModManager.GetModById(text) == null)
				{
					SteamWorkshopManager.SubscribeItem(text);
				}
			}
		}
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x000372A0 File Offset: 0x000354A0
	private static void Event_Server_OnServerStarted(Dictionary<string, object> message)
	{
		ServerConfig serverConfig = (ServerConfig)message["serverConfig"];
		if (ApplicationManager.IsDedicatedGameServer)
		{
			string[] enabledModIds = serverConfig.EnabledModIds;
			for (int i = 0; i < enabledModIds.Length; i++)
			{
				SteamWorkshopManager.DownloadItem(enabledModIds[i]);
			}
		}
	}

	// Token: 0x040006E0 RID: 1760
	private static readonly Logger Logger = new Logger("SteamWorkshopManagerController");
}
