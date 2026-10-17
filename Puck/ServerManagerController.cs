using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000157 RID: 343
public class ServerManagerController : MonoBehaviour
{
	// Token: 0x06000A1D RID: 2589 RVA: 0x0003065C File Offset: 0x0002E85C
	private void Awake()
	{
		this.serverManager = base.GetComponent<ServerManager>();
		EventManager.AddEventListener("Event_Everyone_OnClientConnected", new Action<Dictionary<string, object>>(this.Event_Everyone_OnClientConnected));
		EventManager.AddEventListener("Event_Everyone_OnClientDisconnected", new Action<Dictionary<string, object>>(this.Event_Everyone_OnClientDisconnected));
		EventManager.AddEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStarted));
		EventManager.AddEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
		EventManager.AddEventListener("Event_Server_OnNetworkDiagnostics", new Action<Dictionary<string, object>>(this.Event_Server_OnNetworkDiagnostics));
		EventManager.AddEventListener("Event_Server_OnObjectSynchronizationDiagnostics", new Action<Dictionary<string, object>>(this.Event_Server_OnObjectSynchronizationDiagnostics));
		EventManager.AddEventListener("Event_OnServerStateChanged", new Action<Dictionary<string, object>>(this.Event_OnServerStateChanged));
		EventManager.AddEventListener("Event_OnTransportFailure", new Action<Dictionary<string, object>>(this.Event_OnTransportFailure));
		EventManager.AddEventListener("Event_OnMainMenuClickHostServer", new Action<Dictionary<string, object>>(this.Event_OnMainMenuClickHostServer));
		EventManager.AddEventListener("Event_OnNewServerClickStart", new Action<Dictionary<string, object>>(this.Event_OnNewServerClickStart));
		EventManager.AddEventListener("Event_OnPlayClickPractice", new Action<Dictionary<string, object>>(this.Event_OnPlayClickPractice));
		WebSocketManager.AddMessageListener("connected", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnConnected));
		WebSocketManager.AddMessageListener("serverKickPlayer", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnServerKickPlayer));
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x000023BA File Offset: 0x000005BA
	private void Start()
	{
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x00030794 File Offset: 0x0002E994
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnClientConnected", new Action<Dictionary<string, object>>(this.Event_Everyone_OnClientConnected));
		EventManager.RemoveEventListener("Event_Everyone_OnClientDisconnected", new Action<Dictionary<string, object>>(this.Event_Everyone_OnClientDisconnected));
		EventManager.RemoveEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStarted));
		EventManager.RemoveEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
		EventManager.RemoveEventListener("Event_Server_OnNetworkDiagnostics", new Action<Dictionary<string, object>>(this.Event_Server_OnNetworkDiagnostics));
		EventManager.RemoveEventListener("Event_Server_OnObjectSynchronizationDiagnostics", new Action<Dictionary<string, object>>(this.Event_Server_OnObjectSynchronizationDiagnostics));
		EventManager.RemoveEventListener("Event_OnServerStateChanged", new Action<Dictionary<string, object>>(this.Event_OnServerStateChanged));
		EventManager.RemoveEventListener("Event_OnTransportFailure", new Action<Dictionary<string, object>>(this.Event_OnTransportFailure));
		EventManager.RemoveEventListener("Event_OnMainMenuClickHostServer", new Action<Dictionary<string, object>>(this.Event_OnMainMenuClickHostServer));
		EventManager.RemoveEventListener("Event_OnNewServerClickStart", new Action<Dictionary<string, object>>(this.Event_OnNewServerClickStart));
		EventManager.RemoveEventListener("Event_OnPlayClickPractice", new Action<Dictionary<string, object>>(this.Event_OnPlayClickPractice));
		WebSocketManager.RemoveMessageListener("connected", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnConnected));
		WebSocketManager.RemoveMessageListener("serverKickPlayer", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnServerKickPlayer));
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x000308C0 File Offset: 0x0002EAC0
	private void Event_Everyone_OnClientConnected(Dictionary<string, object> message)
	{
		ulong num = (ulong)message["clientId"];
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		ServerManagerController.Logger.Info(string.Format("Client connected ({0}) {1}/{2}", num, NetworkManager.Singleton.ConnectedClientsList.Count, this.serverManager.ServerConfig.maxPlayers));
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x00030930 File Offset: 0x0002EB30
	private void Event_Everyone_OnClientDisconnected(Dictionary<string, object> message)
	{
		ulong num = (ulong)message["clientId"];
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		ServerManagerController.Logger.Info(string.Format("Client disconnected ({0}) {1}/{2}", num, NetworkManager.Singleton.ConnectedClientsList.Count, this.serverManager.ServerConfig.maxPlayers));
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x000309A0 File Offset: 0x0002EBA0
	private void Event_OnServerStateChanged(Dictionary<string, object> message)
	{
		ref ServerState ptr = (ServerState)message["oldServerState"];
		ServerState serverState = (ServerState)message["newServerState"];
		if (ptr.AuthenticationPhase == AuthenticationPhase.None && serverState.AuthenticationPhase == AuthenticationPhase.Authenticated && this.serverManager.IsHostStartInProgress)
		{
			this.serverManager.IsHostStartInProgress = false;
			if (!NetworkManager.Singleton.StartHost())
			{
				this.serverManager.HandleStartFailure();
			}
		}
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x00030A0E File Offset: 0x0002EC0E
	private void Event_OnTransportFailure(Dictionary<string, object> message)
	{
		if (this.serverManager.IsHostStartInProgress)
		{
			this.serverManager.IsHostStartInProgress = false;
		}
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x00030A2C File Offset: 0x0002EC2C
	private void Event_Server_OnServerStarted(Dictionary<string, object> message)
	{
		this.serverManager.Server.Value = new Server
		{
			IpAddress = this.serverManager.IpAddress,
			Port = this.serverManager.PublicPort,
			Name = this.serverManager.ServerConfig.name,
			MaxPlayers = this.serverManager.ServerConfig.maxPlayers,
			TickRate = this.serverManager.ServerConfig.tickRate,
			UseVoip = this.serverManager.ServerConfig.useVoip,
			GameMode = this.serverManager.ServerConfig.gameMode
		};
		this.serverManager.StartTcpServer(this.serverManager.BindPort);
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x00030B0E File Offset: 0x0002ED0E
	private void Event_Server_OnServerStopped(Dictionary<string, object> message)
	{
		this.serverManager.StopTcpServer();
		this.serverManager.StopPortForwarding();
		this.serverManager.Unauthenticate();
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x00030B34 File Offset: 0x0002ED34
	private void Event_Server_OnNetworkDiagnostics(Dictionary<string, object> message)
	{
		NetworkDiagnostics networkDiagnostics = (NetworkDiagnostics)message["networkDiagnostics"];
		ServerManagerController.Logger.Info(NetworkDiagnostics.DescribeHeader());
		ServerManagerController.Logger.Info(networkDiagnostics.Describe());
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x00030B74 File Offset: 0x0002ED74
	private void Event_Server_OnObjectSynchronizationDiagnostics(Dictionary<string, object> message)
	{
		SynchronizedObjectServerDiagnosticsData synchronizedObjectServerDiagnosticsData = (SynchronizedObjectServerDiagnosticsData)message["diagnosticsData"];
		ServerManagerController.Logger.Info(synchronizedObjectServerDiagnosticsData.DescribeHeader());
		foreach (SynchronizedObjectPlayerDiagnosticsData synchronizedObjectPlayerDiagnosticsData in synchronizedObjectServerDiagnosticsData.Players)
		{
			ServerManagerController.Logger.Info(synchronizedObjectPlayerDiagnosticsData.Describe());
		}
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x00030BD4 File Offset: 0x0002EDD4
	private void Event_OnMainMenuClickHostServer(Dictionary<string, object> message)
	{
		ushort port = (ushort)message["port"];
		string password = (string)message["password"];
		this.serverManager.StartHost(port, "MY PUCK SERVER", 12, password, true, true, true);
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x00030C1C File Offset: 0x0002EE1C
	private void Event_OnNewServerClickStart(Dictionary<string, object> message)
	{
		if ((string)message["type"] != "selfHosted")
		{
			return;
		}
		int num = (int)message["port"];
		string name = (string)message["name"];
		int maxPlayers = (int)message["maxPlayers"];
		string password = (string)message["password"];
		bool useVoip = (bool)message["useVoip"];
		this.serverManager.StartHost((ushort)num, name, maxPlayers, password, true, useVoip, true);
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x00030CB0 File Offset: 0x0002EEB0
	private void Event_OnPlayClickPractice(Dictionary<string, object> message)
	{
		this.serverManager.StartHost(30609, "PRACTICE", 1, null, false, false, false);
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x00030CCC File Offset: 0x0002EECC
	private void WebSocket_Event_OnConnected(Dictionary<string, object> message)
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			if (NetworkManager.Singleton.IsServer)
			{
				this.serverManager.Authenticate();
				return;
			}
			this.serverManager.StartServer(this.serverManager.BindPort, true);
		}
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x00030D04 File Offset: 0x0002EF04
	private void WebSocket_Event_OnServerKickPlayer(Dictionary<string, object> message)
	{
		ServerKickPlayerMessage data = ((InMessage)message["inMessage"]).GetData<ServerKickPlayerMessage>();
		Player playerBySteamId = MonoBehaviourSingleton<PlayerManager>.Instance.GetPlayerBySteamId(data.steamId);
		if (!playerBySteamId)
		{
			return;
		}
		this.serverManager.Server_KickPlayer(playerBySteamId, DisconnectionCode.Kicked, null, false);
	}

	// Token: 0x0400062B RID: 1579
	private static readonly global::Logger Logger = new global::Logger("ServerManagerController");

	// Token: 0x0400062C RID: 1580
	private ServerManager serverManager;
}
