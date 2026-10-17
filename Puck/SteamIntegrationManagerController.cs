using System;
using System.Collections.Generic;
using Steamworks;

// Token: 0x02000169 RID: 361
public static class SteamIntegrationManagerController
{
	// Token: 0x06000B12 RID: 2834 RVA: 0x00034F3C File Offset: 0x0003313C
	public static void Initialize()
	{
		EventManager.AddEventListener("Event_Everyone_OnGamePhaseChanged", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_Everyone_OnGamePhaseChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.AddEventListener("Event_Everyone_OnServerChanged", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_Everyone_OnServerChanged));
		EventManager.AddEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnClientStopped));
		EventManager.AddEventListener("Event_OnPlayerDataChanged", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnPlayerDataChanged));
		EventManager.AddEventListener("Event_OnPlayerPartyDataChanged", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnPlayerPartyDataChanged));
		EventManager.AddEventListener("Event_OnScoreboardClickPlayer", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnScoreboardClickPlayer));
		EventManager.AddEventListener("Event_OnModsClickFindMods", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnModsClickFindMods));
		EventManager.AddEventListener("Event_OnModPreviewLinkClicked", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnModPreviewLinkClicked));
		EventManager.AddEventListener("Event_OnFriendInviteButtonClicked", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnFriendInviteButtonClicked));
		EventManager.AddEventListener("Event_OnGameLobbyJoinRequested", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnGameLobbyJoinRequested));
		EventManager.AddEventListener("Event_OnAppearanceClickPurchaseItem", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnAppearanceClickPurchaseItem));
		EventManager.AddEventListener("Event_OnMicroTxnAuthorizationResponse", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnMicroTxnAuthorizationResponse));
		WebSocketManager.AddMessageListener("connected", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.WebSocket_Event_OnConnected));
		WebSocketManager.AddMessageListener("disconnected", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.WebSocket_Event_OnDisconnected));
		WebSocketManager.AddMessageListener("playerAuthenticateResponse", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.WebSocket_Event_OnPlayerAuthenticateResponse));
		WebSocketManager.AddMessageListener("playerJoinPartyResponse", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.WebSocket_Event_OnPlayerJoinPartyResponse));
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x000350C0 File Offset: 0x000332C0
	public static void Dispose()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnGamePhaseChanged", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_Everyone_OnGamePhaseChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnServerChanged", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_Everyone_OnServerChanged));
		EventManager.RemoveEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnClientStopped));
		EventManager.RemoveEventListener("Event_OnPlayerDataChanged", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnPlayerDataChanged));
		EventManager.RemoveEventListener("Event_OnPlayerPartyDataChanged", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnPlayerPartyDataChanged));
		EventManager.RemoveEventListener("Event_OnScoreboardClickPlayer", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnScoreboardClickPlayer));
		EventManager.RemoveEventListener("Event_OnModsClickFindMods", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnModsClickFindMods));
		EventManager.RemoveEventListener("Event_OnModPreviewLinkClicked", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnModPreviewLinkClicked));
		EventManager.RemoveEventListener("Event_OnFriendInviteButtonClicked", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnFriendInviteButtonClicked));
		EventManager.RemoveEventListener("Event_OnGameLobbyJoinRequested", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnGameLobbyJoinRequested));
		EventManager.RemoveEventListener("Event_OnAppearanceClickPurchaseItem", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnAppearanceClickPurchaseItem));
		EventManager.RemoveEventListener("Event_OnMicroTxnAuthorizationResponse", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.Event_OnMicroTxnAuthorizationResponse));
		WebSocketManager.RemoveMessageListener("connected", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.WebSocket_Event_OnConnected));
		WebSocketManager.RemoveMessageListener("disconnected", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.WebSocket_Event_OnDisconnected));
		WebSocketManager.RemoveMessageListener("playerAuthenticateResponse", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.WebSocket_Event_OnPlayerAuthenticateResponse));
		WebSocketManager.RemoveMessageListener("playerJoinPartyResponse", new Action<Dictionary<string, object>>(SteamIntegrationManagerController.WebSocket_Event_OnPlayerJoinPartyResponse));
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x00035244 File Offset: 0x00033444
	private static void Event_Everyone_OnGamePhaseChanged(Dictionary<string, object> message)
	{
		GameState gameState = (GameState)message["gameState"];
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		SteamIntegrationManager.UpdateRichPresencePhase(gameState.Phase);
		SteamIntegrationManager.UpdateRichPresenceScore(gameState.Phase != GamePhase.Warmup, gameState.Period, gameState.BlueScore, gameState.RedScore);
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x00035298 File Offset: 0x00033498
	private static void Event_Everyone_OnPlayerGameStateChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		Server value = NetworkBehaviourSingleton<ServerManager>.Instance.Server.Value;
		GameState value2 = NetworkBehaviourSingleton<GameManager>.Instance.GameState.Value;
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		if (!player.IsLocalPlayer)
		{
			return;
		}
		PlayerTeam team = player.Team;
		if (team - PlayerTeam.Blue <= 1)
		{
			SteamIntegrationManager.UpdateRichPresencePhase(value2.Phase);
			SteamIntegrationManager.UpdateRichPresenceTeam(player.Team);
			SteamIntegrationManager.UpdateRichPresenceRole(player.Role);
			SteamIntegrationManager.UpdateRichPresenceScore(value2.Phase != GamePhase.Warmup, value2.Period, value2.BlueScore, value2.RedScore);
			SteamIntegrationManager.SetRichPresencePlaying(value, MonoBehaviourSingleton<PlayerManager>.Instance.GetPlayers(false).Count);
			return;
		}
		SteamIntegrationManager.UpdateRichPresenceScore(value2.Phase != GamePhase.Warmup, value2.Period, value2.BlueScore, value2.RedScore);
		SteamIntegrationManager.SetRichPresenceSpectating(value, MonoBehaviourSingleton<PlayerManager>.Instance.GetPlayers(false).Count);
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x00035388 File Offset: 0x00033588
	private static void Event_Everyone_OnServerChanged(Dictionary<string, object> message)
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		SteamIntegrationManager.UpdateRichPresenceScore(false, 0, 0, 0);
		SteamIntegrationManager.SetRichPresenceSpectating(NetworkBehaviourSingleton<ServerManager>.Instance.Server.Value, 1);
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x000353B0 File Offset: 0x000335B0
	private static void Event_OnClientStopped(Dictionary<string, object> message)
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		SteamIntegrationManager.SetRichPresenceMainMenu();
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x000353C0 File Offset: 0x000335C0
	private static void Event_OnPlayerDataChanged(Dictionary<string, object> message)
	{
		bool flag = (PlayerData)message["oldPlayerData"] != null;
		PlayerData playerData = (PlayerData)message["newPlayerData"];
		if (!flag && playerData != null)
		{
			SteamIntegrationManager.GetLaunchCommandLine();
		}
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x000353F8 File Offset: 0x000335F8
	private static void Event_OnPlayerPartyDataChanged(Dictionary<string, object> message)
	{
		PlayerPartyData playerPartyData = (PlayerPartyData)message["oldPlayerPartyData"];
		PlayerPartyData playerPartyData2 = (PlayerPartyData)message["newPlayerPartyData"];
		bool flag = playerPartyData != null;
		bool flag2 = playerPartyData2 != null;
		bool flag3 = flag && playerPartyData.steamLobbyId != null;
		bool flag4 = flag2 && playerPartyData2.steamLobbyId != null;
		bool flag5 = flag2 && playerPartyData2.ownerSteamId == BackendManager.PlayerState.PlayerData.steamId;
		bool flag6 = ((playerPartyData != null) ? playerPartyData.steamLobbyId : null) != ((playerPartyData2 != null) ? playerPartyData2.steamLobbyId : null);
		bool flag7 = flag4 && flag6;
		bool flag8 = flag3 && flag6;
		bool flag9 = flag2 && !flag4 && flag5;
		if (flag8)
		{
			SteamIntegrationManager.LeaveLobby(playerPartyData.steamLobbyId);
		}
		if (flag9)
		{
			SteamIntegrationManager.CreateLobby();
		}
		if (flag7)
		{
			SteamIntegrationManager.JoinLobby(playerPartyData2.steamLobbyId);
		}
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x000354D0 File Offset: 0x000336D0
	private static void Event_OnScoreboardClickPlayer(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		SteamFriends.ActivateGameOverlayToUser("steamID", new CSteamID(ulong.Parse(player.SteamId.Value.ToString())));
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x0003551B File Offset: 0x0003371B
	private static void Event_OnModsClickFindMods(Dictionary<string, object> message)
	{
		SteamFriends.ActivateGameOverlayToWebPage("https://steamcommunity.com/app/2994020/workshop/", EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x00035528 File Offset: 0x00033728
	private static void Event_OnModPreviewLinkClicked(Dictionary<string, object> message)
	{
		string str = (string)message["id"];
		SteamFriends.ActivateGameOverlayToWebPage("https://steamcommunity.com/sharedfiles/filedetails/?id=" + str, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x00035558 File Offset: 0x00033758
	private static void Event_OnFriendInviteButtonClicked(Dictionary<string, object> message)
	{
		string invitedSteamId = (string)message["steamId"];
		PlayerPartyData partyData = BackendManager.PlayerState.PartyData;
		if (partyData == null)
		{
			return;
		}
		if (partyData.steamLobbyId != null)
		{
			SteamIntegrationManager.InviteToLobby(BackendManager.PlayerState.PartyData.steamLobbyId, invitedSteamId);
		}
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x000355A2 File Offset: 0x000337A2
	private static void Event_OnGameLobbyJoinRequested(Dictionary<string, object> message)
	{
		SteamIntegrationManager.JoinLobby((string)message["lobbyId"]);
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x000355BC File Offset: 0x000337BC
	private static void Event_OnAppearanceClickPurchaseItem(Dictionary<string, object> message)
	{
		Item item = (Item)message["item"];
		EventManager.TriggerEvent("Event_OnTransactionStarting", new Dictionary<string, object>
		{
			{
				"itemId",
				item.id
			}
		});
		if (!SteamUtils.IsOverlayEnabled())
		{
			EventManager.TriggerEvent("Event_OnTransactionStartFailed", new Dictionary<string, object>
			{
				{
					"error",
					"Steam overlay disabled"
				}
			});
			return;
		}
		WebSocketManager.Emit("playerStartPurchaseRequest", new Dictionary<string, object>
		{
			{
				"itemId",
				item.id
			}
		}, "playerStartPurchaseResponse");
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x00035650 File Offset: 0x00033850
	private static void Event_OnMicroTxnAuthorizationResponse(Dictionary<string, object> message)
	{
		bool flag = (bool)message["authorized"];
		ulong num = (ulong)message["orderId"];
		if (flag)
		{
			WebSocketManager.Emit("playerCompletePurchaseRequest", new Dictionary<string, object>
			{
				{
					"orderId",
					num
				}
			}, "playerCompletePurchaseResponse");
		}
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x000356A5 File Offset: 0x000338A5
	private static void WebSocket_Event_OnConnected(Dictionary<string, object> message)
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		SteamIntegrationManager.GetTicketForWebApi();
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x000356B4 File Offset: 0x000338B4
	private static void WebSocket_Event_OnDisconnected(Dictionary<string, object> message)
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		SteamIntegrationManager.StopTicketForWebApi();
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x000356C3 File Offset: 0x000338C3
	private static void WebSocket_Event_OnPlayerAuthenticateResponse(Dictionary<string, object> message)
	{
		if (((InMessage)message["inMessage"]).GetData<PlayerAuthenticateResponse>().success)
		{
			SteamIntegrationManager.SetRichPresenceMainMenu();
		}
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x000356E8 File Offset: 0x000338E8
	private static void WebSocket_Event_OnPlayerJoinPartyResponse(Dictionary<string, object> message)
	{
		OutMessage outMessage = (OutMessage)message["outMessage"];
		Response<object, BaseErrorResponseData> data = ((InMessage)message["inMessage"]).GetData<PlayerJoinPartyResponse>();
		string text = (string)outMessage.Data["steamLobbyId"];
		if (!data.success && text != null)
		{
			SteamIntegrationManager.LeaveLobby(text);
		}
	}
}
