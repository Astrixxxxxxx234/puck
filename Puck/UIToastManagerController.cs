using System;
using System.Collections.Generic;
using Unity.Netcode;

// Token: 0x0200021B RID: 539
public class UIToastManagerController : UIViewController<UIToastManager>
{
	// Token: 0x06001119 RID: 4377 RVA: 0x00050FE0 File Offset: 0x0004F1E0
	public override void Awake()
	{
		base.Awake();
		this.uiToastManager = base.GetComponent<UIToastManager>();
		EventManager.AddEventListener("Event_Everyone_OnClientConnected", new Action<Dictionary<string, object>>(this.Event_Everyone_OnClientConnected));
		EventManager.AddEventListener("Event_OnSteamInitializationStarted", new Action<Dictionary<string, object>>(this.Event_OnSteamInitializationStarted));
		EventManager.AddEventListener("Event_OnSteamInitializationFailed", new Action<Dictionary<string, object>>(this.Event_OnSteamInitializationFailed));
		EventManager.AddEventListener("Event_OnSteamInitialized", new Action<Dictionary<string, object>>(this.Event_OnSteamInitialized));
		EventManager.AddEventListener("Event_OnSteamConnectionFailed", new Action<Dictionary<string, object>>(this.Event_OnSteamConnectionFailed));
		EventManager.AddEventListener("Event_OnSteamConnected", new Action<Dictionary<string, object>>(this.Event_OnSteamConnected));
		EventManager.AddEventListener("Event_OnSteamDisconnected", new Action<Dictionary<string, object>>(this.Event_OnSteamDisconnected));
		EventManager.AddEventListener("Event_OnSteamAuthenticationStarted", new Action<Dictionary<string, object>>(this.Event_OnSteamAuthenticationStarted));
		EventManager.AddEventListener("Event_OnSteamAuthenticationStalled", new Action<Dictionary<string, object>>(this.Event_OnSteamAuthenticationStalled));
		EventManager.AddEventListener("Event_OnPlayerStateChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerStateChanged));
		EventManager.AddEventListener("Event_OnTransportFailure", new Action<Dictionary<string, object>>(this.Event_OnTransportFailure));
		EventManager.AddEventListener("Event_OnServerStartFailed", new Action<Dictionary<string, object>>(this.Event_OnServerStartFailed));
		EventManager.AddEventListener("Event_OnClientStarted", new Action<Dictionary<string, object>>(this.Event_OnClientStarted));
		EventManager.AddEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
		EventManager.AddEventListener("Event_OnConnectionRejected", new Action<Dictionary<string, object>>(this.Event_OnConnectionRejected));
		EventManager.AddEventListener("Event_OnDisconnected", new Action<Dictionary<string, object>>(this.Event_OnDisconnected));
		EventManager.AddEventListener("Event_OnPluginEnableFailed", new Action<Dictionary<string, object>>(this.Event_OnPluginEnableFailed));
		EventManager.AddEventListener("Event_OnPluginDisableFailed", new Action<Dictionary<string, object>>(this.Event_OnPluginDisableFailed));
		EventManager.AddEventListener("Event_OnModEnableFailed", new Action<Dictionary<string, object>>(this.Event_OnModEnableFailed));
		EventManager.AddEventListener("Event_OnModDisableFailed", new Action<Dictionary<string, object>>(this.Event_OnModDisableFailed));
		WebSocketManager.AddMessageListener("emit", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnEmit));
		WebSocketManager.AddMessageListener("connecting", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnConnecting));
		WebSocketManager.AddMessageListener("connected", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnConnected));
		WebSocketManager.AddMessageListener("disconnected", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnDisconnected));
		WebSocketManager.AddMessageListener("PlayerStartTransactionResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerStartTransactionResponse));
		WebSocketManager.AddMessageListener("playerDeployServerResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerDeployServerResponse));
		WebSocketManager.AddMessageListener("playerSetIdentityResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerSetIdentityResponse));
		WebSocketManager.AddMessageListener("playerJoinPartyResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerJoinPartyResponse));
		WebSocketManager.AddMessageListener("playerStartMatchmakingResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerStartMatchmakingResponse));
	}

	// Token: 0x0600111A RID: 4378 RVA: 0x00051280 File Offset: 0x0004F480
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnClientConnected", new Action<Dictionary<string, object>>(this.Event_Everyone_OnClientConnected));
		EventManager.RemoveEventListener("Event_OnSteamInitializationStarted", new Action<Dictionary<string, object>>(this.Event_OnSteamInitializationStarted));
		EventManager.RemoveEventListener("Event_OnSteamInitializationFailed", new Action<Dictionary<string, object>>(this.Event_OnSteamInitializationFailed));
		EventManager.RemoveEventListener("Event_OnSteamInitialized", new Action<Dictionary<string, object>>(this.Event_OnSteamInitialized));
		EventManager.RemoveEventListener("Event_OnSteamConnectionFailed", new Action<Dictionary<string, object>>(this.Event_OnSteamConnectionFailed));
		EventManager.RemoveEventListener("Event_OnSteamConnected", new Action<Dictionary<string, object>>(this.Event_OnSteamConnected));
		EventManager.RemoveEventListener("Event_OnSteamDisconnected", new Action<Dictionary<string, object>>(this.Event_OnSteamDisconnected));
		EventManager.RemoveEventListener("Event_OnSteamAuthenticationStarted", new Action<Dictionary<string, object>>(this.Event_OnSteamAuthenticationStarted));
		EventManager.RemoveEventListener("Event_OnSteamAuthenticationStalled", new Action<Dictionary<string, object>>(this.Event_OnSteamAuthenticationStalled));
		EventManager.RemoveEventListener("Event_OnPlayerStateChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerStateChanged));
		EventManager.RemoveEventListener("Event_OnTransportFailure", new Action<Dictionary<string, object>>(this.Event_OnTransportFailure));
		EventManager.RemoveEventListener("Event_OnServerStartFailed", new Action<Dictionary<string, object>>(this.Event_OnServerStartFailed));
		EventManager.RemoveEventListener("Event_OnClientStarted", new Action<Dictionary<string, object>>(this.Event_OnClientStarted));
		EventManager.RemoveEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
		EventManager.RemoveEventListener("Event_OnConnectionRejected", new Action<Dictionary<string, object>>(this.Event_OnConnectionRejected));
		EventManager.RemoveEventListener("Event_OnDisconnected", new Action<Dictionary<string, object>>(this.Event_OnDisconnected));
		EventManager.RemoveEventListener("Event_OnPluginEnableFailed", new Action<Dictionary<string, object>>(this.Event_OnPluginEnableFailed));
		EventManager.RemoveEventListener("Event_OnPluginDisableFailed", new Action<Dictionary<string, object>>(this.Event_OnPluginDisableFailed));
		EventManager.RemoveEventListener("Event_OnModEnableFailed", new Action<Dictionary<string, object>>(this.Event_OnModEnableFailed));
		EventManager.RemoveEventListener("Event_OnModDisableFailed", new Action<Dictionary<string, object>>(this.Event_OnModDisableFailed));
		WebSocketManager.RemoveMessageListener("emit", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnEmit));
		WebSocketManager.RemoveMessageListener("connecting", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnConnecting));
		WebSocketManager.RemoveMessageListener("connected", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnConnected));
		WebSocketManager.RemoveMessageListener("disconnected", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnDisconnected));
		WebSocketManager.RemoveMessageListener("PlayerStartTransactionResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerStartTransactionResponse));
		WebSocketManager.RemoveMessageListener("playerDeployServerResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerDeployServerResponse));
		WebSocketManager.RemoveMessageListener("playerSetIdentityResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerSetIdentityResponse));
		WebSocketManager.RemoveMessageListener("playerJoinPartyResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerJoinPartyResponse));
		WebSocketManager.RemoveMessageListener("playerStartMatchmakingResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerStartMatchmakingResponse));
		base.OnDestroy();
	}

	// Token: 0x0600111B RID: 4379 RVA: 0x00051514 File Offset: 0x0004F714
	private void Event_Everyone_OnClientConnected(Dictionary<string, object> message)
	{
		ulong num = (ulong)message["clientId"];
		if (NetworkManager.Singleton.LocalClientId != num)
		{
			return;
		}
		this.uiToastManager.HideToast("serverConnection");
	}

	// Token: 0x0600111C RID: 4380 RVA: 0x00051550 File Offset: 0x0004F750
	private void Event_OnSteamInitializationStarted(Dictionary<string, object> message)
	{
		this.uiToastManager.ShowToast("steamInitialization", "Initializing Steam...", float.PositiveInfinity);
	}

	// Token: 0x0600111D RID: 4381 RVA: 0x0005156C File Offset: 0x0004F76C
	private void Event_OnSteamInitializationFailed(Dictionary<string, object> message)
	{
		string content = (((message != null && message.ContainsKey("attempts")) ? ((int)message["attempts"]) : 0) >= 3) ? "Can't connect to Steam. Make sure the Steam client is running and signed in, or try restarting it." : "Failed to initialize Steam, retrying...";
		this.uiToastManager.ShowToast("steamInitialization", content, float.PositiveInfinity);
	}

	// Token: 0x0600111E RID: 4382 RVA: 0x000515C2 File Offset: 0x0004F7C2
	private void Event_OnSteamInitialized(Dictionary<string, object> message)
	{
		this.uiToastManager.HideToast("steamInitialization");
		this.uiToastManager.ShowToast("steamConnection", "Connecting to Steam...", float.PositiveInfinity);
	}

	// Token: 0x0600111F RID: 4383 RVA: 0x000515EE File Offset: 0x0004F7EE
	private void Event_OnSteamConnectionFailed(Dictionary<string, object> message)
	{
		this.uiToastManager.ShowToast("steamConnection", "Failed to connect to Steam, retrying...", float.PositiveInfinity);
	}

	// Token: 0x06001120 RID: 4384 RVA: 0x0005160A File Offset: 0x0004F80A
	private void Event_OnSteamConnected(Dictionary<string, object> message)
	{
		this.uiToastManager.HideToast("steamConnection");
	}

	// Token: 0x06001121 RID: 4385 RVA: 0x0005161C File Offset: 0x0004F81C
	private void Event_OnSteamDisconnected(Dictionary<string, object> message)
	{
		this.uiToastManager.ShowToast("steamConnection", "Disconnected from Steam, reconnecting...", float.PositiveInfinity);
	}

	// Token: 0x06001122 RID: 4386 RVA: 0x00051638 File Offset: 0x0004F838
	private void Event_OnSteamAuthenticationStarted(Dictionary<string, object> message)
	{
		this.uiToastManager.ShowToast("steamAuthentication", "Signing in to Steam...", float.PositiveInfinity);
	}

	// Token: 0x06001123 RID: 4387 RVA: 0x00051654 File Offset: 0x0004F854
	private void Event_OnSteamAuthenticationStalled(Dictionary<string, object> message)
	{
		this.uiToastManager.ShowToast("steamAuthentication", "Can't reach Steam. Try restarting the Steam client.", float.PositiveInfinity);
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x00051670 File Offset: 0x0004F870
	private void Event_OnPlayerStateChanged(Dictionary<string, object> message)
	{
		if (((PlayerState)message["newPlayerState"]).AuthenticationPhase == AuthenticationPhase.Authenticating)
		{
			this.uiToastManager.HideToast("steamAuthentication");
			this.uiToastManager.ShowToast("playerAuthentication", "Authenticating...", float.PositiveInfinity);
			return;
		}
		this.uiToastManager.HideToast("playerAuthentication");
	}

	// Token: 0x06001125 RID: 4389 RVA: 0x000516D0 File Offset: 0x0004F8D0
	private void Event_OnTransportFailure(Dictionary<string, object> message)
	{
		this.uiToastManager.ShowToast("transportFailure", "Network transport failure", 3f);
	}

	// Token: 0x06001126 RID: 4390 RVA: 0x000516EC File Offset: 0x0004F8EC
	private void Event_OnServerStartFailed(Dictionary<string, object> message)
	{
		ushort num = (ushort)message["port"];
		this.uiToastManager.ShowToast("serverStartFailed", string.Format("Failed to start server (is port {0} already in use?)", num), 6f);
	}

	// Token: 0x06001127 RID: 4391 RVA: 0x0005172F File Offset: 0x0004F92F
	private void Event_OnClientStarted(Dictionary<string, object> message)
	{
		this.uiToastManager.ShowToast("serverConnection", "Connecting to server...", float.PositiveInfinity);
	}

	// Token: 0x06001128 RID: 4392 RVA: 0x0005174B File Offset: 0x0004F94B
	private void Event_OnClientStopped(Dictionary<string, object> message)
	{
		this.uiToastManager.HideToast("serverConnection");
	}

	// Token: 0x06001129 RID: 4393 RVA: 0x00051760 File Offset: 0x0004F960
	private void Event_OnConnectionRejected(Dictionary<string, object> message)
	{
		ConnectionRejection connectionRejection = (ConnectionRejection)message["connectionRejection"];
		if (connectionRejection.code == ConnectionRejectionCode.MissingPassword || connectionRejection.code == ConnectionRejectionCode.MissingMods)
		{
			return;
		}
		this.uiToastManager.ShowToast("connectionRejected", "Connection rejected: " + Utils.GetConnectionRejectionMessage(connectionRejection.code, connectionRejection.message), 3f);
	}

	// Token: 0x0600112A RID: 4394 RVA: 0x000517C4 File Offset: 0x0004F9C4
	private void Event_OnDisconnected(Dictionary<string, object> message)
	{
		Disconnection disconnection = (Disconnection)message["disconnection"];
		if (disconnection.code != DisconnectionCode.Disconnected)
		{
			this.uiToastManager.ShowToast("disconnected", "Disconnected: " + Utils.GetDisconnectionMessage(disconnection.code, disconnection.message), 3f);
		}
	}

	// Token: 0x0600112B RID: 4395 RVA: 0x0005181C File Offset: 0x0004FA1C
	private void Event_OnPluginEnableFailed(Dictionary<string, object> message)
	{
		Plugin plugin = (Plugin)message["plugin"];
		this.uiToastManager.ShowToast("pluginEnableFailed_" + plugin.Id, "Failed to enable plugin " + plugin.Id, 3f);
	}

	// Token: 0x0600112C RID: 4396 RVA: 0x0005186C File Offset: 0x0004FA6C
	private void Event_OnPluginDisableFailed(Dictionary<string, object> message)
	{
		Plugin plugin = (Plugin)message["plugin"];
		this.uiToastManager.ShowToast("pluginDisableFailed_" + plugin.Id, "Failed to disable plugin " + plugin.Id, 3f);
	}

	// Token: 0x0600112D RID: 4397 RVA: 0x000518BC File Offset: 0x0004FABC
	private void Event_OnModEnableFailed(Dictionary<string, object> message)
	{
		Mod mod = (Mod)message["mod"];
		SteamWorkshopItemDetails details = mod.SteamWorkshopItem.Details;
		string str = ((details != null) ? details.Title : null) ?? mod.Id;
		this.uiToastManager.ShowToast("modEnableFailed_" + mod.Id, "Failed to enable mod " + str, 3f);
	}

	// Token: 0x0600112E RID: 4398 RVA: 0x00051928 File Offset: 0x0004FB28
	private void Event_OnModDisableFailed(Dictionary<string, object> message)
	{
		Mod mod = (Mod)message["mod"];
		SteamWorkshopItemDetails details = mod.SteamWorkshopItem.Details;
		string str = ((details != null) ? details.Title : null) ?? mod.Id;
		this.uiToastManager.ShowToast("modDisableFailed_" + mod.Id, "Failed to disable mod " + str, 3f);
	}

	// Token: 0x0600112F RID: 4399 RVA: 0x00051993 File Offset: 0x0004FB93
	private void WebSocket_Event_OnEmit(Dictionary<string, object> message)
	{
		if ((string)message["messageName"] == "playerDeployServerRequest")
		{
			this.uiToastManager.ShowToast("playerDeployServer", "Deploying server...", float.PositiveInfinity);
		}
	}

	// Token: 0x06001130 RID: 4400 RVA: 0x000519CB File Offset: 0x0004FBCB
	private void WebSocket_Event_OnConnecting(Dictionary<string, object> message)
	{
		this.uiToastManager.ShowToast("webSocketConnection", "Connecting to Puck backend...", float.PositiveInfinity);
	}

	// Token: 0x06001131 RID: 4401 RVA: 0x000519E7 File Offset: 0x0004FBE7
	private void WebSocket_Event_OnConnected(Dictionary<string, object> message)
	{
		this.uiToastManager.HideToast("webSocketConnection");
	}

	// Token: 0x06001132 RID: 4402 RVA: 0x000519F9 File Offset: 0x0004FBF9
	private void WebSocket_Event_OnDisconnected(Dictionary<string, object> message)
	{
		this.uiToastManager.HideToast("steamAuthentication");
		this.uiToastManager.ShowToast("webSocketConnection", "Disconnected from Puck backend, reconnecting...", float.PositiveInfinity);
	}

	// Token: 0x06001133 RID: 4403 RVA: 0x00051A28 File Offset: 0x0004FC28
	private void WebSocket_Event_OnPlayerStartTransactionResponse(Dictionary<string, object> message)
	{
		PlayerStartTransactionResponse data = ((InMessage)message["inMessage"]).GetData<PlayerStartTransactionResponse>();
		if (!data.success)
		{
			this.uiToastManager.ShowToast("playerStartTransaction", "Failed to start transaction: " + data.errorData.message, 3f);
		}
	}

	// Token: 0x06001134 RID: 4404 RVA: 0x00051A80 File Offset: 0x0004FC80
	private void WebSocket_Event_OnPlayerDeployServerResponse(Dictionary<string, object> message)
	{
		PlayerDeployServerResponse data = ((InMessage)message["inMessage"]).GetData<PlayerDeployServerResponse>();
		if (data.success)
		{
			this.uiToastManager.ShowToast("playerDeployServer", "Server deployed!", 3f);
			return;
		}
		this.uiToastManager.ShowToast("playerDeployServer", "Failed to deploy server: " + data.errorData.message, 3f);
	}

	// Token: 0x06001135 RID: 4405 RVA: 0x00051AF0 File Offset: 0x0004FCF0
	private void WebSocket_Event_OnPlayerSetIdentityResponse(Dictionary<string, object> message)
	{
		PlayerSetIdentityResponse data = ((InMessage)message["inMessage"]).GetData<PlayerSetIdentityResponse>();
		if (!data.success)
		{
			this.uiToastManager.ShowToast("playerSetIdentity", "Failed to set identity: " + data.errorData.message, 3f);
		}
	}

	// Token: 0x06001136 RID: 4406 RVA: 0x00051B48 File Offset: 0x0004FD48
	private void WebSocket_Event_OnPlayerJoinPartyResponse(Dictionary<string, object> message)
	{
		PlayerJoinPartyResponse data = ((InMessage)message["inMessage"]).GetData<PlayerJoinPartyResponse>();
		if (!data.success)
		{
			this.uiToastManager.ShowToast("playerJoinParty", "Failed to join party: " + data.errorData.message, 3f);
		}
	}

	// Token: 0x06001137 RID: 4407 RVA: 0x00051BA0 File Offset: 0x0004FDA0
	private void WebSocket_Event_OnPlayerStartMatchmakingResponse(Dictionary<string, object> message)
	{
		PlayerStartMatchmakingResponse data = ((InMessage)message["inMessage"]).GetData<PlayerStartMatchmakingResponse>();
		if (!data.success)
		{
			this.uiToastManager.ShowToast("playerStartMatchmaking", "Failed to start matchmaking: " + data.errorData.message, 3f);
		}
	}

	// Token: 0x04000ABB RID: 2747
	private UIToastManager uiToastManager;
}
