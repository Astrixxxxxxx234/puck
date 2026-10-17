using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

// Token: 0x02000155 RID: 341
[RequireComponent(typeof(DeploymentManager))]
[RequireComponent(typeof(ConnectionApprovalManager))]
[RequireComponent(typeof(TimeoutManager))]
[RequireComponent(typeof(BanManager))]
[RequireComponent(typeof(AdminManager))]
public class ServerManager : NetworkBehaviourSingleton<ServerManager>
{
	// Token: 0x17000119 RID: 281
	// (get) Token: 0x060009F3 RID: 2547 RVA: 0x0002FADD File Offset: 0x0002DCDD
	public ushort PublicPort
	{
		get
		{
			if (!this.DeploymentManager.IsDeployment)
			{
				return this.ServerConfig.port;
			}
			return this.DeploymentManager.DeploymentPort;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x060009F4 RID: 2548 RVA: 0x0002FB03 File Offset: 0x0002DD03
	public ushort BindPort
	{
		get
		{
			if (!this.DeploymentManager.BindsDeploymentPort)
			{
				return this.ServerConfig.port;
			}
			return this.DeploymentManager.DeploymentPort;
		}
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x0002FB2C File Offset: 0x0002DD2C
	public override void Awake()
	{
		base.Awake();
		if (ApplicationManager.IsDedicatedGameServer)
		{
			this.LoadConfig("./server_config.json", "--serverConfigPath", "--serverConfig", "PUCK_SERVER_CONFIG");
		}
		this.DeploymentManager = base.GetComponent<DeploymentManager>();
		this.ConnectionApprovalManager = base.GetComponent<ConnectionApprovalManager>();
		this.TimeoutManager = base.GetComponent<TimeoutManager>();
		this.BanManager = base.GetComponent<BanManager>();
		this.AdminManager = base.GetComponent<AdminManager>();
		this.WhitelistManager = base.GetComponent<WhitelistManager>();
		uPnPHelper.DebugMode = true;
		uPnPHelper.LogErrors = true;
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x0002FBB4 File Offset: 0x0002DDB4
	private void Start()
	{
		this.UnityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
		NetworkManager.Singleton.OnServerStarted += this.Server_OnServerStarted;
		NetworkManager.Singleton.OnServerStopped += this.Server_OnServerStopped;
		NetworkManager.Singleton.OnClientConnectedCallback += this.OnClientConnected;
		NetworkManager.Singleton.OnClientDisconnectCallback += this.OnClientDisconnected;
		NetworkManager.Singleton.OnTransportFailure += this.OnTransportFailure;
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x0002FC40 File Offset: 0x0002DE40
	protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
	{
		if (this.Server == null)
		{
			this.Server = new NetworkVariable<Server>(default(Server), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		}
		if (networkManager.IsServer)
		{
			this.Server.Value = default(Server);
		}
		base.OnNetworkPreSpawn(ref networkManager);
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x0002FC8F File Offset: 0x0002DE8F
	public override void OnNetworkSpawn()
	{
		NetworkVariable<Server> server = this.Server;
		server.OnValueChanged = (NetworkVariable<Server>.OnValueChangedDelegate)Delegate.Combine(server.OnValueChanged, new NetworkVariable<Server>.OnValueChangedDelegate(this.OnServerChanged));
		base.OnNetworkSpawn();
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x0002FCBE File Offset: 0x0002DEBE
	protected override void OnNetworkPostSpawn()
	{
		if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsConnectedClient)
		{
			this.ProcessInitialNetworkVariableValues();
		}
		base.OnNetworkPostSpawn();
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x0002FCE4 File Offset: 0x0002DEE4
	protected override void OnNetworkSessionSynchronized()
	{
		this.ProcessInitialNetworkVariableValues();
		base.OnNetworkSessionSynchronized();
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x0002FCF2 File Offset: 0x0002DEF2
	public override void OnNetworkDespawn()
	{
		NetworkVariable<Server> server = this.Server;
		server.OnValueChanged = (NetworkVariable<Server>.OnValueChangedDelegate)Delegate.Remove(server.OnValueChanged, new NetworkVariable<Server>.OnValueChangedDelegate(this.OnServerChanged));
		base.OnNetworkDespawn();
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x0002FD24 File Offset: 0x0002DF24
	public override void OnDestroy()
	{
		if (NetworkManager.Singleton != null)
		{
			NetworkManager.Singleton.OnServerStarted -= this.Server_OnServerStarted;
			NetworkManager.Singleton.OnServerStopped -= this.Server_OnServerStopped;
			NetworkManager.Singleton.OnClientConnectedCallback -= this.OnClientConnected;
			NetworkManager.Singleton.OnClientDisconnectCallback -= this.OnClientDisconnected;
			NetworkManager.Singleton.OnTransportFailure -= this.OnTransportFailure;
		}
		uPnPHelper.CloseAll();
		Utils.PrintUPnPLogs();
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x0002FDB8 File Offset: 0x0002DFB8
	private void ProcessInitialNetworkVariableValues()
	{
		this.OnServerChanged(default(Server), this.Server.Value);
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x0002FDE0 File Offset: 0x0002DFE0
	private void LoadConfig(string defaultFilePath, string filePathCliArgument = null, string cliArgument = null, string envVariable = null)
	{
		string environmentVariable = Environment.GetEnvironmentVariable(envVariable);
		string commandLineArgument = Utils.GetCommandLineArgument(cliArgument, null);
		if (!string.IsNullOrEmpty(environmentVariable))
		{
			ServerManager.Logger.Info("Deserializing server config from environment variable (" + envVariable + ")");
			this.ServerConfig = ConfigUtils.LoadConfigFromSerializedString<ServerConfig>(environmentVariable, "environment variable " + envVariable);
		}
		else if (!string.IsNullOrEmpty(commandLineArgument))
		{
			ServerManager.Logger.Info("Deserializing server config from CLI argument (" + cliArgument + ")");
			this.ServerConfig = ConfigUtils.LoadConfigFromSerializedString<ServerConfig>(commandLineArgument, "CLI argument " + cliArgument);
		}
		else
		{
			string text = Utils.GetCommandLineArgument(filePathCliArgument, null) ?? defaultFilePath;
			ServerManager.Logger.Info("Deserializing server config from file (" + text + ")");
			this.ServerConfig = ConfigUtils.LoadConfigFromFile<ServerConfig>(text, true);
		}
		if (this.ServerConfig == null)
		{
			ServerManager.Logger.Error("Server config is invalid (see the error above); refusing to start. Fix the config and restart.");
			Application.Quit(1);
		}
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x0002FEC8 File Offset: 0x0002E0C8
	private bool StartListener(ushort port, bool forwardPorts = false)
	{
		if (NetworkManager.Singleton.IsListening)
		{
			return false;
		}
		ServerManager.Logger.Info(string.Format("Puck B{0}", ApplicationManager.Version));
		if (forwardPorts)
		{
			this.StartPortForwarding(port);
		}
		this.UnityTransport.SetConnectionData("0.0.0.0", port, null);
		ServerManager.Logger.Info(string.Format("Started UDP listener on {0}:{1}", "0.0.0.0", port));
		return true;
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x0002FF40 File Offset: 0x0002E140
	public void StartHost(ushort port, string name, int maxPlayers, string password, bool isPublic, bool useVoip, bool forwardPorts = false)
	{
		if (!this.StartListener(port, forwardPorts))
		{
			return;
		}
		this.ServerConfig = new ServerConfig
		{
			port = port,
			name = name,
			maxPlayers = maxPlayers,
			password = password,
			isPublic = isPublic,
			useVoip = useVoip
		};
		ConnectionData connectionData = new ConnectionData();
		connectionData.SteamId = BackendManager.PlayerState.PlayerData.steamId;
		connectionData.Key = BackendManager.PlayerState.Key;
		connectionData.Password = password;
		connectionData.EnabledModIds = (from mod in ModManager.EnabledMods
		select mod.Id).ToArray<string>();
		connectionData.Handedness = SettingsManager.Handedness;
		connectionData.FlagID = SettingsManager.FlagID;
		connectionData.HeadgearIDBlueAttacker = SettingsManager.HeadgearIDBlueAttacker;
		connectionData.HeadgearIDRedAttacker = SettingsManager.HeadgearIDRedAttacker;
		connectionData.HeadgearIDBlueGoalie = SettingsManager.HeadgearIDBlueGoalie;
		connectionData.HeadgearIDRedGoalie = SettingsManager.HeadgearIDRedGoalie;
		connectionData.MustacheID = SettingsManager.MustacheID;
		connectionData.BeardID = SettingsManager.BeardID;
		connectionData.JerseyIDBlueAttacker = SettingsManager.JerseyIDBlueAttacker;
		connectionData.JerseyIDRedAttacker = SettingsManager.JerseyIDRedAttacker;
		connectionData.JerseyIDBlueGoalie = SettingsManager.JerseyIDBlueGoalie;
		connectionData.JerseyIDRedGoalie = SettingsManager.JerseyIDRedGoalie;
		connectionData.StickSkinIDBlueAttacker = SettingsManager.StickSkinIDBlueAttacker;
		connectionData.StickSkinIDRedAttacker = SettingsManager.StickSkinIDRedAttacker;
		connectionData.StickSkinIDBlueGoalie = SettingsManager.StickSkinIDBlueGoalie;
		connectionData.StickSkinIDRedGoalie = SettingsManager.StickSkinIDRedGoalie;
		connectionData.StickShaftTapeIDBlueAttacker = SettingsManager.StickShaftTapeIDBlueAttacker;
		connectionData.StickShaftTapeIDRedAttacker = SettingsManager.StickShaftTapeIDRedAttacker;
		connectionData.StickShaftTapeIDBlueGoalie = SettingsManager.StickShaftTapeIDBlueGoalie;
		connectionData.StickShaftTapeIDRedGoalie = SettingsManager.StickShaftTapeIDRedGoalie;
		connectionData.StickBladeTapeIDBlueAttacker = SettingsManager.StickBladeTapeIDBlueAttacker;
		connectionData.StickBladeTapeIDRedAttacker = SettingsManager.StickBladeTapeIDRedAttacker;
		connectionData.StickBladeTapeIDBlueGoalie = SettingsManager.StickBladeTapeIDBlueGoalie;
		connectionData.StickBladeTapeIDRedGoalie = SettingsManager.StickBladeTapeIDRedGoalie;
		string s = JsonSerializer.Serialize<ConnectionData>(connectionData, null);
		NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(s);
		this.IsHostStartInProgress = true;
		this.Authenticate();
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x0003012D File Offset: 0x0002E32D
	public void StartServer(ushort port, bool forwardPorts = false)
	{
		if (!this.StartListener(port, forwardPorts))
		{
			return;
		}
		if (!NetworkManager.Singleton.StartServer())
		{
			this.HandleStartFailure();
			return;
		}
		this.Authenticate();
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x00030154 File Offset: 0x0002E354
	public void HandleStartFailure()
	{
		ServerManager.Logger.Error(string.Format("Failed to start server on port {0} (is the port already in use?)", this.ServerConfig.port));
		this.IsHostStartInProgress = false;
		this.StopPortForwarding();
		this.Unauthenticate();
		EventManager.TriggerEvent("Event_OnServerStartFailed", new Dictionary<string, object>
		{
			{
				"port",
				this.ServerConfig.port
			}
		});
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x000301C2 File Offset: 0x0002E3C2
	public void StartPortForwarding(ushort port)
	{
		ServerManager.Logger.Info(string.Format("Starting uPnP port forwarding for TCP & UDP port {0}", port));
		uPnPHelper.Start(uPnPHelper.Protocol.UDP, (int)port, 0, "Puck");
		Utils.PrintUPnPLogs();
		uPnPHelper.Start(uPnPHelper.Protocol.TCP, (int)port, 0, "Puck");
		Utils.PrintUPnPLogs();
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x00030202 File Offset: 0x0002E402
	public void StopPortForwarding()
	{
		ServerManager.Logger.Info("Stopping uPnP port forwarding");
		uPnPHelper.CloseAll();
		Utils.PrintUPnPLogs();
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x00030220 File Offset: 0x0002E420
	public void Authenticate()
	{
		WebSocketManager.Emit("serverAuthenticateRequest", new Dictionary<string, object>
		{
			{
				"port",
				this.PublicPort
			},
			{
				"isPublic",
				this.ServerConfig.isPublic
			},
			{
				"deploymentId",
				this.DeploymentManager.DeploymentId
			}
		}, "serverAuthenticateResponse");
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x00030288 File Offset: 0x0002E488
	public void Unauthenticate()
	{
		WebSocketManager.Emit("serverUnauthenticateRequest", null, "serverUnauthenticateResponse");
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x0003029A File Offset: 0x0002E49A
	private void OnServerChanged(Server oldServer, Server newServer)
	{
		EventManager.TriggerEvent("Event_Everyone_OnServerChanged", new Dictionary<string, object>
		{
			{
				"oldServer",
				oldServer
			},
			{
				"newServer",
				newServer
			}
		});
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x000302CD File Offset: 0x0002E4CD
	public void StartTcpServer(ushort port)
	{
		this.TcpServer = new TCPServer(port);
		this.TcpServer.OnMessageReceived += delegate(string ipPort, string message)
		{
			try
			{
				if (JsonSerializer.Deserialize<TCPServerMessage>(message, null).type == TCPServerMessageType.PreviewRequest)
				{
					JsonSerializer.Deserialize<TCPServerPreviewRequest>(message, null);
					string message2 = JsonSerializer.Serialize<TCPServerPreviewResponse>(new TCPServerPreviewResponse
					{
						name = this.ServerConfig.name,
						players = NetworkManager.Singleton.ConnectedClientsList.Count,
						maxPlayers = this.ServerConfig.maxPlayers,
						isPasswordProtected = !string.IsNullOrEmpty(this.ServerConfig.password),
						clientRequiredModIds = this.ServerConfig.ClientRequiredModIds
					}, null);
					this.TcpServer.SendMessageAsync(ipPort, message2);
				}
			}
			catch (Exception ex)
			{
				ServerManager.Logger.Error("Error parsing message from " + ipPort + ": " + ex.Message);
			}
		};
		this.TcpServer.StartAsync();
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x000302FD File Offset: 0x0002E4FD
	public void StopTcpServer()
	{
		this.TcpServer.StopAsync();
		this.TcpServer = null;
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x00030311 File Offset: 0x0002E511
	private void Server_OnServerStarted()
	{
		EventManager.TriggerEvent("Event_Server_OnServerStarted", new Dictionary<string, object>
		{
			{
				"serverConfig",
				this.ServerConfig
			}
		});
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x00030333 File Offset: 0x0002E533
	private void Server_OnServerStopped(bool wasHost)
	{
		EventManager.TriggerEvent("Event_Server_OnServerStopped", new Dictionary<string, object>
		{
			{
				"wasHost",
				wasHost
			}
		});
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x00030355 File Offset: 0x0002E555
	private void OnClientConnected(ulong clientId)
	{
		if (NetworkManager.Singleton.LocalClientId == clientId)
		{
			EventManager.TriggerEvent("Event_OnClientConnected", null);
		}
		EventManager.TriggerEvent("Event_Everyone_OnClientConnected", new Dictionary<string, object>
		{
			{
				"clientId",
				clientId
			}
		});
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x0003038F File Offset: 0x0002E58F
	private void OnClientDisconnected(ulong clientId)
	{
		EventManager.TriggerEvent("Event_Everyone_OnClientDisconnected", new Dictionary<string, object>
		{
			{
				"clientId",
				clientId
			}
		});
		if (NetworkManager.Singleton.LocalClientId == clientId)
		{
			EventManager.TriggerEvent("Event_OnClientDisconnected", null);
		}
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x000303C9 File Offset: 0x0002E5C9
	private void OnTransportFailure()
	{
		EventManager.TriggerEvent("Event_OnTransportFailure", null);
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x000303D8 File Offset: 0x0002E5D8
	public void Server_KickPlayer(Player player, DisconnectionCode disconnectionCode = DisconnectionCode.Kicked, string message = null, bool applyTimeout = true)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		string steamId = player.SteamId.Value.ToString();
		if (player.OwnerClientId == 0UL)
		{
			NetworkManager.Singleton.Shutdown(true);
			return;
		}
		if (applyTimeout)
		{
			this.TimeoutManager.AddSteamIdTimeout(steamId, 60f);
		}
		string reason = JsonSerializer.Serialize<Disconnection>(new Disconnection
		{
			code = disconnectionCode,
			message = message
		}, null);
		NetworkManager.Singleton.DisconnectClient(player.OwnerClientId, reason);
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x00030460 File Offset: 0x0002E660
	public void Server_BanPlayer(Player player)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		string steamId = player.SteamId.Value.ToString();
		this.Server_BanSteamId(steamId);
		this.Server_KickPlayer(player, DisconnectionCode.Banned, null, false);
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x000304A5 File Offset: 0x0002E6A5
	public void Server_BanSteamId(string steamId)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.BanManager.AddBannedSteamId(steamId);
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x000304C0 File Offset: 0x0002E6C0
	public void Server_UnbanSteamId(string steamId)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.BanManager.RemoveBannedSteamId(steamId);
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x000304DB File Offset: 0x0002E6DB
	public bool Server_IsSteamIdBanned(string steamId)
	{
		return this.BanManager.IsSteamIdBanned(steamId);
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x000305DC File Offset: 0x0002E7DC
	protected override void __initializeVariables()
	{
		bool flag = this.Server == null;
		if (flag)
		{
			throw new Exception("ServerManager.Server cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.Server.Initialize(this);
		base.__nameNetworkVariable(this.Server, "Server");
		this.NetworkVariableFields.Add(this.Server);
		base.__initializeVariables();
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x0003063F File Offset: 0x0002E83F
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x00030649 File Offset: 0x0002E849
	protected internal override string __getTypeName()
	{
		return "ServerManager";
	}

	// Token: 0x0400061C RID: 1564
	private static readonly global::Logger Logger = new global::Logger("ServerManager");

	// Token: 0x0400061D RID: 1565
	[HideInInspector]
	public UnityTransport UnityTransport;

	// Token: 0x0400061E RID: 1566
	[HideInInspector]
	public DeploymentManager DeploymentManager;

	// Token: 0x0400061F RID: 1567
	[HideInInspector]
	public ConnectionApprovalManager ConnectionApprovalManager;

	// Token: 0x04000620 RID: 1568
	[HideInInspector]
	public TimeoutManager TimeoutManager;

	// Token: 0x04000621 RID: 1569
	[HideInInspector]
	public BanManager BanManager;

	// Token: 0x04000622 RID: 1570
	[HideInInspector]
	public AdminManager AdminManager;

	// Token: 0x04000623 RID: 1571
	[HideInInspector]
	public WhitelistManager WhitelistManager;

	// Token: 0x04000624 RID: 1572
	[HideInInspector]
	public NetworkVariable<Server> Server;

	// Token: 0x04000625 RID: 1573
	[HideInInspector]
	public bool IsHostStartInProgress;

	// Token: 0x04000626 RID: 1574
	[HideInInspector]
	public TCPServer TcpServer;

	// Token: 0x04000627 RID: 1575
	[HideInInspector]
	public string IpAddress;

	// Token: 0x04000628 RID: 1576
	[HideInInspector]
	public ServerConfig ServerConfig;
}
