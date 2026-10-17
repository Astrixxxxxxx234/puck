using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class ConnectionManagerController : MonoBehaviour
{
	// Token: 0x06000809 RID: 2057 RVA: 0x00028CE0 File Offset: 0x00026EE0
	private void Awake()
	{
		this.connectionManager = base.GetComponent<ConnectionManager>();
		EventManager.AddEventListener("Event_Server_OnConnectionRejected", new Action<Dictionary<string, object>>(this.Event_Server_OnConnectionRejected));
		EventManager.AddEventListener("Event_OnClientStarted", new Action<Dictionary<string, object>>(this.Event_OnClientStarted));
		EventManager.AddEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
		EventManager.AddEventListener("Event_OnClientConnected", new Action<Dictionary<string, object>>(this.Event_OnClientConnected));
		EventManager.AddEventListener("Event_OnMainMenuClickJoinServer", new Action<Dictionary<string, object>>(this.Event_OnMainMenuClickJoinServer));
		EventManager.AddEventListener("Event_OnPauseMenuClickDisconnect", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickDisconnect));
		EventManager.AddEventListener("Event_OnDebugChanged", new Action<Dictionary<string, object>>(this.Event_OnDebugChanged));
		EventManager.AddEventListener("Event_OnGotLaunchCommandLine", new Action<Dictionary<string, object>>(this.Event_OnGotLaunchCommandLine));
		EventManager.AddEventListener("Event_OnGameRichPresenceJoinRequested", new Action<Dictionary<string, object>>(this.Event_OnGameRichPresenceJoinRequested));
		EventManager.AddEventListener("Event_OnServerBrowserClickEndPoint", new Action<Dictionary<string, object>>(this.Event_OnServerBrowserClickEndPoint));
		EventManager.AddEventListener("Event_OnDirectConnectClickConnect", new Action<Dictionary<string, object>>(this.Event_OnDirectConnectClickConnect));
		EventManager.AddEventListener("Event_OnMatchmakingMatchingClickConnect", new Action<Dictionary<string, object>>(this.Event_OnMatchmakingMatchingClickConnect));
		EventManager.AddEventListener("Event_OnReconnectionStateChanged", new Action<Dictionary<string, object>>(this.Event_OnReconnectionStateChanged));
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x00028E18 File Offset: 0x00027018
	private void Start()
	{
		bool networkProfilingMetrics = SettingsManager.Debug != DebugMode.Off && !ApplicationManager.IsDedicatedGameServer;
		NetworkManager.Singleton.NetworkConfig.NetworkProfilingMetrics = networkProfilingMetrics;
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x00028E48 File Offset: 0x00027048
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Server_OnConnectionRejected", new Action<Dictionary<string, object>>(this.Event_Server_OnConnectionRejected));
		EventManager.RemoveEventListener("Event_OnClientStarted", new Action<Dictionary<string, object>>(this.Event_OnClientStarted));
		EventManager.RemoveEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
		EventManager.RemoveEventListener("Event_OnClientConnected", new Action<Dictionary<string, object>>(this.Event_OnClientConnected));
		EventManager.RemoveEventListener("Event_OnMainMenuClickJoinServer", new Action<Dictionary<string, object>>(this.Event_OnMainMenuClickJoinServer));
		EventManager.RemoveEventListener("Event_OnPauseMenuClickDisconnect", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickDisconnect));
		EventManager.RemoveEventListener("Event_OnDebugChanged", new Action<Dictionary<string, object>>(this.Event_OnDebugChanged));
		EventManager.RemoveEventListener("Event_OnGotLaunchCommandLine", new Action<Dictionary<string, object>>(this.Event_OnGotLaunchCommandLine));
		EventManager.RemoveEventListener("Event_OnGameRichPresenceJoinRequested", new Action<Dictionary<string, object>>(this.Event_OnGameRichPresenceJoinRequested));
		EventManager.RemoveEventListener("Event_OnServerBrowserClickEndPoint", new Action<Dictionary<string, object>>(this.Event_OnServerBrowserClickEndPoint));
		EventManager.RemoveEventListener("Event_OnDirectConnectClickConnect", new Action<Dictionary<string, object>>(this.Event_OnDirectConnectClickConnect));
		EventManager.RemoveEventListener("Event_OnMatchmakingMatchingClickConnect", new Action<Dictionary<string, object>>(this.Event_OnMatchmakingMatchingClickConnect));
		EventManager.RemoveEventListener("Event_OnReconnectionStateChanged", new Action<Dictionary<string, object>>(this.Event_OnReconnectionStateChanged));
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x00028F74 File Offset: 0x00027174
	private void HandleConnectionRejection(string reason)
	{
		ConnectionRejection connectionRejection;
		try
		{
			connectionRejection = JsonSerializer.Deserialize<ConnectionRejection>(reason, null);
		}
		catch
		{
			connectionRejection = new ConnectionRejection
			{
				code = ConnectionRejectionCode.Unreachable
			};
		}
		GlobalStateManager.SetConnectionState(new Dictionary<string, object>
		{
			{
				"connection",
				null
			},
			{
				"lastConnection",
				GlobalStateManager.ConnectionState.Connection
			},
			{
				"connectionRejection",
				connectionRejection
			},
			{
				"disconnection",
				null
			},
			{
				"phase",
				ConnectionPhase.Disconnected
			}
		});
		ConnectionRejectionCode code = connectionRejection.code;
		if (code - ConnectionRejectionCode.MissingPassword > 1)
		{
			if (code == ConnectionRejectionCode.MissingMods)
			{
				GlobalStateManager.SetReconnectionState(new Dictionary<string, object>
				{
					{
						"phase",
						ReconnectionPhase.AwaitingMods
					},
					{
						"clientRequiredModIds",
						connectionRejection.data.clientRequiredModIds
					},
					{
						"pendingReadinessModIds",
						new string[0]
					},
					{
						"pendingEnablingModIds",
						new string[0]
					}
				});
			}
		}
		else
		{
			GlobalStateManager.SetReconnectionState(new Dictionary<string, object>
			{
				{
					"phase",
					ReconnectionPhase.AwaitingPassword
				},
				{
					"password",
					null
				}
			});
		}
		EventManager.TriggerEvent("Event_OnConnectionRejected", new Dictionary<string, object>
		{
			{
				"connectionRejection",
				connectionRejection
			}
		});
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x000290AC File Offset: 0x000272AC
	private void HandleDisconnection(string reason)
	{
		Disconnection value;
		try
		{
			value = JsonSerializer.Deserialize<Disconnection>(reason, null);
		}
		catch
		{
			if (NetworkManager.Singleton.DisconnectEvent == NetworkTransport.DisconnectEvents.TransportShutdown)
			{
				value = new Disconnection
				{
					code = DisconnectionCode.Disconnected
				};
			}
			else
			{
				value = new Disconnection
				{
					code = DisconnectionCode.ConnectionLost
				};
			}
		}
		GlobalStateManager.SetConnectionState(new Dictionary<string, object>
		{
			{
				"connection",
				null
			},
			{
				"lastConnection",
				GlobalStateManager.ConnectionState.Connection
			},
			{
				"connectionRejection",
				null
			},
			{
				"disconnection",
				value
			},
			{
				"phase",
				ConnectionPhase.Disconnected
			}
		});
		EventManager.TriggerEvent("Event_OnDisconnected", new Dictionary<string, object>
		{
			{
				"disconnection",
				value
			}
		});
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x00029170 File Offset: 0x00027370
	private void Event_Server_OnConnectionRejected(Dictionary<string, object> message)
	{
		ConnectionApproval connectionApproval = (ConnectionApproval)message["connectionApproval"];
		if (connectionApproval.IsHost)
		{
			this.connectionManager.Client_Disconnect();
			this.HandleConnectionRejection(connectionApproval.Response.Reason);
		}
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x000291B2 File Offset: 0x000273B2
	private void Event_OnClientStarted(Dictionary<string, object> message)
	{
		GlobalStateManager.SetConnectionState(new Dictionary<string, object>
		{
			{
				"phase",
				ConnectionPhase.Connecting
			}
		});
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x000291D0 File Offset: 0x000273D0
	private void Event_OnClientStopped(Dictionary<string, object> message)
	{
		if (GlobalStateManager.ConnectionState.Phase == ConnectionPhase.Connecting)
		{
			this.HandleConnectionRejection(NetworkManager.Singleton.DisconnectReason);
		}
		else
		{
			this.HandleDisconnection(NetworkManager.Singleton.DisconnectReason);
		}
		Connection pendingConnection = GlobalStateManager.ConnectionState.PendingConnection;
		if (pendingConnection != null)
		{
			this.connectionManager.Client_StartClient(pendingConnection.EndPoint.ipAddress, pendingConnection.EndPoint.port, pendingConnection.Password);
		}
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x00029243 File Offset: 0x00027443
	private void Event_OnClientConnected(Dictionary<string, object> message)
	{
		GlobalStateManager.SetConnectionState(new Dictionary<string, object>
		{
			{
				"phase",
				ConnectionPhase.Connected
			}
		});
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x00029260 File Offset: 0x00027460
	private void Event_OnMainMenuClickJoinServer(Dictionary<string, object> message)
	{
		string ipAddress = (string)message["ipAddress"];
		ushort port = (ushort)message["port"];
		string password = (string)message["password"];
		this.connectionManager.Client_StartClient(ipAddress, port, password);
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x000292AE File Offset: 0x000274AE
	private void Event_OnPauseMenuClickDisconnect(Dictionary<string, object> message)
	{
		this.connectionManager.Client_Disconnect();
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x000292BC File Offset: 0x000274BC
	private void Event_OnDebugChanged(Dictionary<string, object> message)
	{
		bool networkProfilingMetrics = (DebugMode)message["value"] != DebugMode.Off && !ApplicationManager.IsDedicatedGameServer;
		NetworkManager.Singleton.NetworkConfig.NetworkProfilingMetrics = networkProfilingMetrics;
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x000292F8 File Offset: 0x000274F8
	private void Event_OnGotLaunchCommandLine(Dictionary<string, object> message)
	{
		string[] args = (string[])message["args"];
		string commandLineArgument = Utils.GetCommandLineArgument("+ipAddress", args);
		ushort num;
		ushort port = ushort.TryParse(Utils.GetCommandLineArgument("+port", args), out num) ? num : 30609;
		string commandLineArgument2 = Utils.GetCommandLineArgument("+password", args);
		if (string.IsNullOrEmpty(commandLineArgument))
		{
			return;
		}
		this.connectionManager.Client_StartClient(commandLineArgument, port, commandLineArgument2);
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x00029364 File Offset: 0x00027564
	private void Event_OnGameRichPresenceJoinRequested(Dictionary<string, object> message)
	{
		string[] args = (string[])message["args"];
		string commandLineArgument = Utils.GetCommandLineArgument("+ipAddress", args);
		ushort num;
		ushort port = ushort.TryParse(Utils.GetCommandLineArgument("+port", args), out num) ? num : 30609;
		string commandLineArgument2 = Utils.GetCommandLineArgument("+password", args);
		if (string.IsNullOrEmpty(commandLineArgument))
		{
			return;
		}
		this.connectionManager.Client_StartClient(commandLineArgument, port, commandLineArgument2);
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x000293D0 File Offset: 0x000275D0
	private void Event_OnServerBrowserClickEndPoint(Dictionary<string, object> message)
	{
		EndPoint endPoint = (EndPoint)message["endPoint"];
		this.connectionManager.Client_StartClient(endPoint.ipAddress, endPoint.port, null);
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x00029408 File Offset: 0x00027608
	private void Event_OnDirectConnectClickConnect(Dictionary<string, object> message)
	{
		string ipAddress = (string)message["ipAddress"];
		ushort port = (ushort)((int)message["port"]);
		this.connectionManager.Client_StartClient(ipAddress, port, null);
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x00029448 File Offset: 0x00027648
	private void Event_OnMatchmakingMatchingClickConnect(Dictionary<string, object> message)
	{
		EndPoint endPoint = BackendManager.PlayerState.MatchData.endPoint;
		if (endPoint == null)
		{
			return;
		}
		this.connectionManager.Client_StartClient(endPoint.ipAddress, endPoint.port, null);
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x00029488 File Offset: 0x00027688
	private void Event_OnReconnectionStateChanged(Dictionary<string, object> message)
	{
		ReconnectionState reconnectionState = (ReconnectionState)message["newReconnectionState"];
		ReconnectionState reconnectionState2 = (ReconnectionState)message["oldReconnectionState"];
		ReconnectionPhase phase = reconnectionState.Phase;
		if (phase != ReconnectionPhase.AwaitingPassword)
		{
			if (phase != ReconnectionPhase.AwaitingMods)
			{
				return;
			}
			bool flag = !reconnectionState2.PendingEnablingModIds.SequenceEqual(reconnectionState.PendingEnablingModIds);
			bool flag2 = !reconnectionState2.PendingReadinessModIds.SequenceEqual(reconnectionState.PendingReadinessModIds);
			if ((flag || flag2) && reconnectionState.PendingModIds.Length == 0)
			{
				this.connectionManager.Client_StartClient(GlobalStateManager.ConnectionState.LastConnection.EndPoint.ipAddress, GlobalStateManager.ConnectionState.LastConnection.EndPoint.port, GlobalStateManager.ConnectionState.LastConnection.Password);
			}
		}
		else if (reconnectionState2.Password != reconnectionState.Password && reconnectionState.Password != null)
		{
			this.connectionManager.Client_StartClient(GlobalStateManager.ConnectionState.LastConnection.EndPoint.ipAddress, GlobalStateManager.ConnectionState.LastConnection.EndPoint.port, reconnectionState.Password);
			return;
		}
	}

	// Token: 0x04000512 RID: 1298
	private static readonly global::Logger Logger = new global::Logger("ConnectionManagerController");

	// Token: 0x04000513 RID: 1299
	private ConnectionManager connectionManager;
}
