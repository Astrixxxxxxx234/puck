using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x020000CD RID: 205
public class GameModeManager : NetworkBehaviourSingleton<GameModeManager>
{
	// Token: 0x0600067F RID: 1663 RVA: 0x00023F00 File Offset: 0x00022100
	protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
	{
		if (this.ClientConfig == null)
		{
			this.ClientConfig = new NetworkVariable<GameModeClientConfig>(default(GameModeClientConfig), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		}
		if (networkManager.IsServer)
		{
			this.ClientConfig.Value = default(GameModeClientConfig);
		}
		base.OnNetworkPreSpawn(ref networkManager);
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x00023F4F File Offset: 0x0002214F
	public override void OnNetworkSpawn()
	{
		NetworkVariable<GameModeClientConfig> clientConfig = this.ClientConfig;
		clientConfig.OnValueChanged = (NetworkVariable<GameModeClientConfig>.OnValueChangedDelegate)Delegate.Combine(clientConfig.OnValueChanged, new NetworkVariable<GameModeClientConfig>.OnValueChangedDelegate(this.OnClientConfigChanged));
		base.OnNetworkSpawn();
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x00023F7E File Offset: 0x0002217E
	protected override void OnNetworkPostSpawn()
	{
		if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsConnectedClient)
		{
			this.ProcessInitialNetworkVariableValues();
		}
		base.OnNetworkPostSpawn();
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x00023FA4 File Offset: 0x000221A4
	protected override void OnNetworkSessionSynchronized()
	{
		this.ProcessInitialNetworkVariableValues();
		base.OnNetworkSessionSynchronized();
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x00023FB2 File Offset: 0x000221B2
	public override void OnNetworkDespawn()
	{
		NetworkVariable<GameModeClientConfig> clientConfig = this.ClientConfig;
		clientConfig.OnValueChanged = (NetworkVariable<GameModeClientConfig>.OnValueChangedDelegate)Delegate.Remove(clientConfig.OnValueChanged, new NetworkVariable<GameModeClientConfig>.OnValueChangedDelegate(this.OnClientConfigChanged));
		base.OnNetworkDespawn();
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x00023FE4 File Offset: 0x000221E4
	private void ProcessInitialNetworkVariableValues()
	{
		this.OnClientConfigChanged(default(GameModeClientConfig), this.ClientConfig.Value);
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x0002400B File Offset: 0x0002220B
	private void OnClientConfigChanged(GameModeClientConfig oldClientConfig, GameModeClientConfig newClientConfig)
	{
		EventManager.TriggerEvent("Event_Everyone_OnGameModeClientConfigChanged", new Dictionary<string, object>
		{
			{
				"oldClientConfig",
				oldClientConfig
			},
			{
				"newClientConfig",
				newClientConfig
			}
		});
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x0002403E File Offset: 0x0002223E
	public override void Awake()
	{
		base.Awake();
		this.RegisterGameModes();
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x0002404C File Offset: 0x0002224C
	private void RegisterGameModes()
	{
		PublicGameMode<PublicGameModeConfig> value = new PublicGameMode<PublicGameModeConfig>("./public_game_mode_config.json", "--publicGameModeConfigPath", "--publicGameModeConfig", "PUCK_PUBLIC_GAME_MODE_CONFIG");
		CompetitiveGameMode<CompetitiveGameModeConfig> value2 = new CompetitiveGameMode<CompetitiveGameModeConfig>("./competitive_game_mode_config.json", "--competitiveGameModeConfigPath", "--competitiveGameModeConfig", "PUCK_COMPETITIVE_GAME_MODE_CONFIG");
		this.gameModeMap.Add("public", value);
		this.gameModeMap.Add("competitive", value2);
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x000240B0 File Offset: 0x000222B0
	public void SelectGameMode(string name)
	{
		if (!this.gameModeMap.ContainsKey(name))
		{
			return;
		}
		this.selectedGameMode = this.gameModeMap[name];
		GameModeManager.Logger.Info("Selected game mode " + Utils.GetReadableTypeName(this.selectedGameMode.GetType()));
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x00024102 File Offset: 0x00022302
	public void DeselectGameMode()
	{
		this.selectedGameMode = null;
		GameModeManager.Logger.Info("Deselected game mode");
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x0002411C File Offset: 0x0002231C
	public void EnableSelectedGameMode()
	{
		if (this.selectedGameMode == null)
		{
			return;
		}
		if (this.selectedGameMode.IsInitialized)
		{
			return;
		}
		GameModeManager.Logger.Info("Enabling game mode " + Utils.GetReadableTypeName(this.selectedGameMode.GetType()));
		this.selectedGameMode.Initialize(this.Level, this.serverManager, this.gameManager, this.playerManager, this.puckManager, this.chatManager, this.replayManager, this.voteManager);
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x000241A0 File Offset: 0x000223A0
	public void DisableSelectedGameMode()
	{
		if (this.selectedGameMode == null)
		{
			return;
		}
		if (!this.selectedGameMode.IsInitialized)
		{
			return;
		}
		GameModeManager.Logger.Info("Disabling game mode " + Utils.GetReadableTypeName(this.selectedGameMode.GetType()));
		this.selectedGameMode.Dispose();
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x00024218 File Offset: 0x00022418
	protected override void __initializeVariables()
	{
		bool flag = this.ClientConfig == null;
		if (flag)
		{
			throw new Exception("GameModeManager.ClientConfig cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.ClientConfig.Initialize(this);
		base.__nameNetworkVariable(this.ClientConfig, "ClientConfig");
		this.NetworkVariableFields.Add(this.ClientConfig);
		base.__initializeVariables();
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x0002427B File Offset: 0x0002247B
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x00024285 File Offset: 0x00022485
	protected internal override string __getTypeName()
	{
		return "GameModeManager";
	}

	// Token: 0x0400042E RID: 1070
	private static readonly global::Logger Logger = new global::Logger("GameModeManager");

	// Token: 0x0400042F RID: 1071
	[Header("References")]
	[SerializeField]
	private ServerManager serverManager;

	// Token: 0x04000430 RID: 1072
	[SerializeField]
	private GameManager gameManager;

	// Token: 0x04000431 RID: 1073
	[SerializeField]
	private PlayerManager playerManager;

	// Token: 0x04000432 RID: 1074
	[SerializeField]
	private PuckManager puckManager;

	// Token: 0x04000433 RID: 1075
	[SerializeField]
	private ChatManager chatManager;

	// Token: 0x04000434 RID: 1076
	[SerializeField]
	private ReplayManager replayManager;

	// Token: 0x04000435 RID: 1077
	[SerializeField]
	private VoteManager voteManager;

	// Token: 0x04000436 RID: 1078
	[HideInInspector]
	public Level Level;

	// Token: 0x04000437 RID: 1079
	private Dictionary<string, IGameMode> gameModeMap = new Dictionary<string, IGameMode>();

	// Token: 0x04000438 RID: 1080
	private IGameMode selectedGameMode;

	// Token: 0x04000439 RID: 1081
	[HideInInspector]
	public NetworkVariable<GameModeClientConfig> ClientConfig;
}
