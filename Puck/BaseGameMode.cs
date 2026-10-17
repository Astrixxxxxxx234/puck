using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

// Token: 0x0200008F RID: 143
public abstract class BaseGameMode<TConfig> : IGameMode where TConfig : BaseGameModeConfig, new()
{
	// Token: 0x17000076 RID: 118
	// (get) Token: 0x06000460 RID: 1120 RVA: 0x00018E9A File Offset: 0x0001709A
	protected global::Logger Logger
	{
		get
		{
			return new global::Logger(Utils.GetReadableTypeName(base.GetType()));
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x06000461 RID: 1121 RVA: 0x00018EAC File Offset: 0x000170AC
	// (set) Token: 0x06000462 RID: 1122 RVA: 0x00018EB4 File Offset: 0x000170B4
	public bool IsInitialized { get; set; }

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06000463 RID: 1123 RVA: 0x00018EBD File Offset: 0x000170BD
	// (set) Token: 0x06000464 RID: 1124 RVA: 0x00018EC5 File Offset: 0x000170C5
	public TConfig Config { get; private set; }

	// Token: 0x06000465 RID: 1125 RVA: 0x00018ECE File Offset: 0x000170CE
	public BaseGameMode(string defaultConfigFilePath, string configFilePathCliArgument = null, string configCliArgument = null, string configEnvVariable = null)
	{
		this.defaultConfigFilePath = defaultConfigFilePath;
		this.configFilePathCliArgument = configFilePathCliArgument;
		this.configCliArgument = configCliArgument;
		this.configEnvVariable = configEnvVariable;
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x00018EF4 File Offset: 0x000170F4
	public virtual bool Initialize(Level level, ServerManager serverManager, GameManager gameManager, PlayerManager playerManager, PuckManager puckManager, ChatManager chatManager, ReplayManager replayManager, VoteManager voteManager)
	{
		if (this.IsInitialized)
		{
			return false;
		}
		this.IsInitialized = true;
		this.Level = level;
		this.ServerManager = serverManager;
		this.GameManager = gameManager;
		this.PlayerManager = playerManager;
		this.PuckManager = puckManager;
		this.ChatManager = chatManager;
		this.ReplayManager = replayManager;
		this.VoteManager = voteManager;
		this.LoadConfig(this.defaultConfigFilePath, this.configFilePathCliArgument, this.configCliArgument, this.configEnvVariable);
		this.SubscribeEvents();
		return true;
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x00018F74 File Offset: 0x00017174
	public virtual bool Dispose()
	{
		if (!this.IsInitialized)
		{
			return false;
		}
		this.IsInitialized = false;
		this.Level = null;
		this.GameManager = null;
		this.PlayerManager = null;
		this.PuckManager = null;
		this.ChatManager = null;
		this.ReplayManager = null;
		this.VoteManager = null;
		this.UnsubscribeEvents();
		return true;
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x00018FCC File Offset: 0x000171CC
	protected virtual void SubscribeEvents()
	{
		EventManager.AddEventListener("Event_Everyone_OnGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameStateChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerAdded", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerAdded));
		EventManager.AddEventListener("Event_Everyone_OnPlayerRemoved", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerRemoved));
		EventManager.AddEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerPositionChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPositionChanged));
		EventManager.AddEventListener("Event_Everyone_OnGoalScored", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGoalScored));
		EventManager.AddEventListener("Event_Everyone_OnPlayerEnterZone", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerEnterZone));
		EventManager.AddEventListener("Event_Everyone_OnPlayerExitZone", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerExitZone));
		EventManager.AddEventListener("Event_Server_OnPhaseExpired", new Action<Dictionary<string, object>>(this.Event_Server_OnPhaseExpired));
		EventManager.AddEventListener("Event_Server_OnPuckEnterGoal", new Action<Dictionary<string, object>>(this.Event_Server_OnPuckEnterGoal));
		EventManager.AddEventListener("Event_Server_OnPlayerRequestTeamSelect", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerRequestTeamSelect));
		EventManager.AddEventListener("Event_Server_OnPlayerRequestTeam", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerRequestTeam));
		EventManager.AddEventListener("Event_Server_OnPlayerRequestPositionSelect", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerRequestPositionSelect));
		EventManager.AddEventListener("Event_Server_OnPlayerRequestPosition", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerRequestPosition));
		EventManager.AddEventListener("Event_Server_OnPlayerRequestHandedness", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerRequestHandedness));
		EventManager.AddEventListener("Event_Server_OnPlayerRequestForfeit", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerRequestForfeit));
		EventManager.AddEventListener("Event_Server_OnVoteAdded", new Action<Dictionary<string, object>>(this.Event_Server_OnVoteAdded));
		EventManager.AddEventListener("Event_Server_OnVoteProgressed", new Action<Dictionary<string, object>>(this.Event_Server_OnVoteProgressed));
		EventManager.AddEventListener("Event_Server_OnVoteRemoved", new Action<Dictionary<string, object>>(this.Event_Server_OnVoteRemoved));
		EventManager.AddEventListener("Event_Server_OnChatCommand", new Action<Dictionary<string, object>>(this.Event_Server_OnChatCommand));
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x00019194 File Offset: 0x00017394
	protected virtual void UnsubscribeEvents()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameStateChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerAdded", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerAdded));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerRemoved", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerRemoved));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerPositionChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPositionChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnGoalScored", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGoalScored));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerEnterZone", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerEnterZone));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerExitZone", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerExitZone));
		EventManager.RemoveEventListener("Event_Server_OnPhaseExpired", new Action<Dictionary<string, object>>(this.Event_Server_OnPhaseExpired));
		EventManager.RemoveEventListener("Event_Server_OnPuckEnterGoal", new Action<Dictionary<string, object>>(this.Event_Server_OnPuckEnterGoal));
		EventManager.RemoveEventListener("Event_Server_OnPlayerRequestTeamSelect", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerRequestTeamSelect));
		EventManager.RemoveEventListener("Event_Server_OnPlayerRequestTeam", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerRequestTeam));
		EventManager.RemoveEventListener("Event_Server_OnPlayerRequestPositionSelect", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerRequestPositionSelect));
		EventManager.RemoveEventListener("Event_Server_OnPlayerRequestPosition", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerRequestPosition));
		EventManager.RemoveEventListener("Event_Server_OnPlayerRequestHandedness", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerRequestHandedness));
		EventManager.RemoveEventListener("Event_Server_OnPlayerRequestForfeit", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerRequestForfeit));
		EventManager.RemoveEventListener("Event_Server_OnVoteAdded", new Action<Dictionary<string, object>>(this.Event_Server_OnVoteAdded));
		EventManager.RemoveEventListener("Event_Server_OnVoteProgressed", new Action<Dictionary<string, object>>(this.Event_Server_OnVoteProgressed));
		EventManager.RemoveEventListener("Event_Server_OnVoteRemoved", new Action<Dictionary<string, object>>(this.Event_Server_OnVoteRemoved));
		EventManager.RemoveEventListener("Event_Server_OnChatCommand", new Action<Dictionary<string, object>>(this.Event_Server_OnChatCommand));
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x0001935C File Offset: 0x0001755C
	private void LoadConfig(string defaultFilePath, string filePathCliArgument = null, string cliArgument = null, string envVariable = null)
	{
		string environmentVariable = Environment.GetEnvironmentVariable(envVariable);
		string commandLineArgument = Utils.GetCommandLineArgument(cliArgument, null);
		if (!string.IsNullOrEmpty(commandLineArgument))
		{
			this.Logger.Info("Deserializing config from CLI argument (" + cliArgument + ")");
			this.Config = ConfigUtils.LoadConfigFromSerializedString<TConfig>(commandLineArgument, "serialized string");
		}
		else if (!string.IsNullOrEmpty(environmentVariable))
		{
			this.Logger.Info("Deserializing config from environment variable (" + envVariable + ")");
			this.Config = ConfigUtils.LoadConfigFromSerializedString<TConfig>(environmentVariable, "serialized string");
		}
		else
		{
			string text = Utils.GetCommandLineArgument(filePathCliArgument, null) ?? defaultFilePath;
			this.Logger.Info("Deserializing config from file (" + text + ")");
			this.Config = ConfigUtils.LoadConfigFromFile<TConfig>(text, true);
		}
		if (this.Config == null)
		{
			this.Logger.Error("Game mode config is invalid (see the error above); falling back to default " + typeof(TConfig).Name + ".");
			this.Config = Activator.CreateInstance<TConfig>();
		}
		this.OnConfigLoaded();
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x00019470 File Offset: 0x00017670
	protected virtual void ScoreGoal(PlayerTeam byTeam, Player goalPlayer, Player assistPlayer, Player secondAssistPlayer, Puck puck)
	{
		NetworkObjectReference goalPlayerNetworkObjectReference = new NetworkObjectReference((goalPlayer != null) ? goalPlayer.NetworkObject : null);
		NetworkObjectReference assistPlayerNetworkObjectReference = new NetworkObjectReference((assistPlayer != null) ? assistPlayer.NetworkObject : null);
		NetworkObjectReference secondAssistPlayerNetworkObjectReference = new NetworkObjectReference((secondAssistPlayer != null) ? secondAssistPlayer.NetworkObject : null);
		NetworkObjectReference puckNetworkObjectReference = new NetworkObjectReference((puck != null) ? puck.NetworkObject : null);
		this.GameManager.Server_NotifyGoalScoredRpc(byTeam, goalPlayerNetworkObjectReference, assistPlayerNetworkObjectReference, secondAssistPlayerNetworkObjectReference, puckNetworkObjectReference);
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnConfigLoaded()
	{
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x000194DD File Offset: 0x000176DD
	protected virtual void OnGameStateChanged(GameState oldGameState, GameState newGameState)
	{
		if (oldGameState.Phase != newGameState.Phase)
		{
			this.OnGamePhaseEnded(oldGameState.Phase);
			this.OnGamePhaseStarted(newGameState.Phase);
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00019508 File Offset: 0x00017708
	protected virtual void OnGamePhaseTimedOut(GamePhase gamePhase)
	{
		switch (gamePhase)
		{
		case GamePhase.Warmup:
			this.OnWarmupTimedOut();
			return;
		case GamePhase.PreGame:
			this.OnPreGameTimedOut();
			return;
		case GamePhase.FaceOff:
			this.OnFaceOffTimedOut();
			return;
		case GamePhase.Play:
			this.OnPlayTimedOut();
			return;
		case GamePhase.BlueScore:
			this.OnBlueScoreTimedOut();
			return;
		case GamePhase.RedScore:
			this.OnRedScoreTimedOut();
			return;
		case GamePhase.Replay:
			this.OnReplayTimedOut();
			return;
		case GamePhase.Intermission:
			this.OnIntermissionTimedOut();
			return;
		case GamePhase.GameOver:
			this.OnGameOverTimedOut();
			return;
		case GamePhase.PostGame:
			this.OnPostGameTimedOut();
			return;
		default:
			return;
		}
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnWarmupTimedOut()
	{
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPreGameTimedOut()
	{
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnFaceOffTimedOut()
	{
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnBlueScoreTimedOut()
	{
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnRedScoreTimedOut()
	{
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnReplayTimedOut()
	{
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnIntermissionTimedOut()
	{
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPlayTimedOut()
	{
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnGameOverTimedOut()
	{
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPostGameTimedOut()
	{
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0001958C File Offset: 0x0001778C
	protected virtual void OnGamePhaseStarted(GamePhase gamePhase)
	{
		switch (gamePhase)
		{
		case GamePhase.Warmup:
			this.OnWarmupStarted();
			return;
		case GamePhase.PreGame:
			this.OnPreGameStarted();
			return;
		case GamePhase.FaceOff:
			this.OnFaceOffStarted();
			return;
		case GamePhase.Play:
			this.OnPlayStarted();
			return;
		case GamePhase.BlueScore:
			this.OnBlueScoreStarted();
			return;
		case GamePhase.RedScore:
			this.OnRedScoreStarted();
			return;
		case GamePhase.Replay:
			this.OnReplayStarted();
			return;
		case GamePhase.Intermission:
			this.OnIntermissionStarted();
			return;
		case GamePhase.GameOver:
			this.OnGameOverStarted();
			return;
		case GamePhase.PostGame:
			this.OnPostGameStarted();
			return;
		default:
			return;
		}
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnWarmupStarted()
	{
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPreGameStarted()
	{
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnFaceOffStarted()
	{
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnBlueScoreStarted()
	{
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnRedScoreStarted()
	{
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnReplayStarted()
	{
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnIntermissionStarted()
	{
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPlayStarted()
	{
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnGameOverStarted()
	{
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPostGameStarted()
	{
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x00019610 File Offset: 0x00017810
	protected virtual void OnGamePhaseEnded(GamePhase gamePhase)
	{
		switch (gamePhase)
		{
		case GamePhase.Warmup:
			this.OnWarmupEnded();
			return;
		case GamePhase.PreGame:
			this.OnPreGameEnded();
			return;
		case GamePhase.FaceOff:
			this.OnFaceOffEnded();
			return;
		case GamePhase.Play:
			this.OnPlayEnded();
			return;
		case GamePhase.BlueScore:
			this.OnBlueScoreEnded();
			return;
		case GamePhase.RedScore:
			this.OnRedScoreEnded();
			return;
		case GamePhase.Replay:
			this.OnReplayEnded();
			return;
		case GamePhase.Intermission:
			this.OnIntermissionEnded();
			return;
		case GamePhase.GameOver:
			this.OnGameOverEnded();
			return;
		case GamePhase.PostGame:
			this.OnPostGameEnded();
			return;
		default:
			return;
		}
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnWarmupEnded()
	{
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPreGameEnded()
	{
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnFaceOffEnded()
	{
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnBlueScoreEnded()
	{
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnRedScoreEnded()
	{
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnReplayEnded()
	{
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnIntermissionEnded()
	{
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPlayEnded()
	{
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnGameOverEnded()
	{
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPostGameEnded()
	{
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x00019693 File Offset: 0x00017893
	protected virtual void OnPlayerJoined(Player player)
	{
		this.ChatManager.Server_BroadcastChatMessage(string.Format("{0} has joined the server", player.Username.Value), "#b8b8b8");
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x000196BF File Offset: 0x000178BF
	protected virtual void OnPlayerLeft(Player player)
	{
		this.ChatManager.Server_BroadcastChatMessage(string.Format("{0} has left the server", player.Username.Value), "#b8b8b8");
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x000196EC File Offset: 0x000178EC
	protected virtual void OnPlayerGameStateChanged(Player player, PlayerGameState oldGameState, PlayerGameState newGameState)
	{
		if (oldGameState.Phase != newGameState.Phase)
		{
			this.OnPlayerPhaseChanged(player, oldGameState.Phase, newGameState.Phase);
		}
		if (oldGameState.Team != newGameState.Team)
		{
			this.OnPlayerTeamChanged(player, oldGameState.Team, newGameState.Team);
		}
		if (oldGameState.Role != newGameState.Role)
		{
			this.OnPlayerRoleChanged(player, oldGameState.Role, newGameState.Role);
		}
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPlayerPhaseChanged(Player player, PlayerPhase oldPlayerPhase, PlayerPhase newPlayerPhase)
	{
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x0001975C File Offset: 0x0001795C
	protected virtual void OnPlayerTeamChanged(Player player, PlayerTeam oldPlayerTeam, PlayerTeam newPlayerTeam)
	{
		if (newPlayerTeam == PlayerTeam.None || newPlayerTeam == PlayerTeam.Spectator)
		{
			return;
		}
		string arg = StringUtils.WrapInTeamColor(newPlayerTeam.ToString(), newPlayerTeam);
		this.ChatManager.Server_BroadcastChatMessage(string.Format("{0} joined team {1}", player.Username.Value, arg), "#b8b8b8");
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPlayerRoleChanged(Player player, PlayerRole oldPlayerRole, PlayerRole newPlayerRole)
	{
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPlayerPositionChanged(Player player, PlayerPosition oldPlayerPosition, PlayerPosition newPlayerPosition)
	{
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnGoalScored(PlayerTeam byTeam, Player goalPlayer, Player assistPlayer, Player secondAssistPlayer, Puck puck)
	{
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPuckEnterGoal(PlayerTeam team, Puck puck)
	{
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPlayerEnterZone(Zone zone, PlayerBody playerBody)
	{
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPlayerExitZone(Zone zone, PlayerBody playerBody)
	{
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPlayerRequestTeamSelect(Player player)
	{
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPlayerRequestTeam(Player player, PlayerTeam team)
	{
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPlayerRequestPositionSelect(Player player)
	{
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPlayerRequestPosition(Player player, PlayerPosition position)
	{
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x000197B0 File Offset: 0x000179B0
	protected virtual void OnPlayerRequestHandedness(Player player, PlayerHandedness handedness)
	{
		player.Handedness.Value = handedness;
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x000023BA File Offset: 0x000005BA
	protected virtual void OnPlayerRequestForfeit(Player player)
	{
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x000197C0 File Offset: 0x000179C0
	protected virtual void OnVoteAdded(Vote vote)
	{
		Player playerBySteamId = this.PlayerManager.GetPlayerBySteamId(vote.SteamId);
		List<Player> playersByTeams = this.PlayerManager.GetPlayersByTeams(vote.Teams, false);
		if (playerBySteamId && playersByTeams.Count > 0)
		{
			string text = StringUtils.WrapInTeamColor(playerBySteamId.Username.Value.ToString(), playerBySteamId.Team);
			this.ChatManager.Server_SendChatMessage(string.Concat(new string[]
			{
				vote.Title,
				" vote started by ",
				text,
				" (",
				vote.Description,
				")"
			}), "#e67e22", playersByTeams.ConvertAll<ulong>((Player p) => p.OwnerClientId).ToArray());
		}
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x000198A8 File Offset: 0x00017AA8
	protected virtual void OnVoteProgressed(Vote vote, string steamId, bool inFavour)
	{
		if (inFavour)
		{
			List<Player> playersByTeams = this.PlayerManager.GetPlayersByTeams(vote.Teams, false);
			if (playersByTeams.Count > 0)
			{
				this.ChatManager.Server_SendChatMessage(string.Format("{0} vote progressed {1}/{2}", vote.Title, vote.InFavourVotes, vote.RequiredVotes), "#e67e22", playersByTeams.ConvertAll<ulong>((Player p) => p.OwnerClientId).ToArray());
			}
		}
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x00019934 File Offset: 0x00017B34
	protected virtual void OnVoteRemoved(Vote vote)
	{
		Object playerBySteamId = this.PlayerManager.GetPlayerBySteamId(vote.SteamId);
		List<Player> playersByTeams = this.PlayerManager.GetPlayersByTeams(vote.Teams, false);
		if (playerBySteamId && playersByTeams.Count > 0)
		{
			string str = vote.Passed ? "passed" : "failed";
			this.ChatManager.Server_SendChatMessage(vote.Title + " vote " + str, "#e67e22", playersByTeams.ConvertAll<ulong>((Player p) => p.OwnerClientId).ToArray());
		}
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x000199DA File Offset: 0x00017BDA
	protected virtual List<ChatCommandInfo> GetChatCommands()
	{
		return new List<ChatCommandInfo>
		{
			new ChatCommandInfo("/help", "List the commands available to you", 0)
		};
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x000199F7 File Offset: 0x00017BF7
	protected virtual void OnChatCommand(Player player, string command, string[] args)
	{
		if (command == "/help")
		{
			this.SendChatCommandHelp(player);
			return;
		}
		this.ChatManager.Server_SendChatMessage("Unknown command", "#e74c3c", new ulong[]
		{
			player.OwnerClientId
		});
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x00019A34 File Offset: 0x00017C34
	private void SendChatCommandHelp(Player player)
	{
		int num = player.AdminLevel.Value;
		if (num < 1 && this.ServerManager.AdminManager.IsSteamIdAdmin(player.SteamId.Value.ToString()))
		{
			num = 1;
		}
		List<string> list = new List<string>
		{
			"Available commands:"
		};
		foreach (ChatCommandInfo chatCommandInfo in this.GetChatCommands())
		{
			if (chatCommandInfo.RequiredAdminLevel <= num)
			{
				list.Add(chatCommandInfo.Usage + " - " + chatCommandInfo.Description);
			}
		}
		int byteCount = Encoding.UTF8.GetByteCount(StringUtils.WrapInColor(string.Empty, "#b8b8b8"));
		int maxBytes = 509 - byteCount;
		foreach (string content in StringUtils.ChunkLinesByByteBudget(list, maxBytes))
		{
			this.ChatManager.Server_SendChatMessage(content, "#b8b8b8", new ulong[]
			{
				player.OwnerClientId
			});
		}
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x00019B7C File Offset: 0x00017D7C
	private void Event_Everyone_OnGameStateChanged(Dictionary<string, object> message)
	{
		GameState oldGameState = (GameState)message["oldGameState"];
		GameState newGameState = (GameState)message["newGameState"];
		this.OnGameStateChanged(oldGameState, newGameState);
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x00019BB4 File Offset: 0x00017DB4
	private void Event_Everyone_OnPlayerAdded(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (player.IsReplay.Value)
		{
			return;
		}
		this.OnPlayerJoined(player);
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x00019BE8 File Offset: 0x00017DE8
	private void Event_Everyone_OnPlayerRemoved(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (player.IsReplay.Value)
		{
			return;
		}
		this.OnPlayerLeft(player);
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x00019C1C File Offset: 0x00017E1C
	private void Event_Everyone_OnPlayerGameStateChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		PlayerGameState oldGameState = (PlayerGameState)message["oldGameState"];
		PlayerGameState newGameState = (PlayerGameState)message["newGameState"];
		if (player.IsReplay.Value)
		{
			return;
		}
		this.OnPlayerGameStateChanged(player, oldGameState, newGameState);
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x00019C74 File Offset: 0x00017E74
	private void Event_Everyone_OnPlayerPositionChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		PlayerPosition oldPlayerPosition = (PlayerPosition)message["oldPlayerPosition"];
		PlayerPosition newPlayerPosition = (PlayerPosition)message["newPlayerPosition"];
		if (player.IsReplay.Value)
		{
			return;
		}
		this.OnPlayerPositionChanged(player, oldPlayerPosition, newPlayerPosition);
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x00019CCC File Offset: 0x00017ECC
	private void Event_Everyone_OnGoalScored(Dictionary<string, object> message)
	{
		PlayerTeam byTeam = (PlayerTeam)message["byTeam"];
		Player goalPlayer = (Player)message["goalPlayer"];
		Player assistPlayer = (Player)message["assistPlayer"];
		Player secondAssistPlayer = (Player)message["secondAssistPlayer"];
		Puck puck = (Puck)message["puck"];
		this.OnGoalScored(byTeam, goalPlayer, assistPlayer, secondAssistPlayer, puck);
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x00019D3C File Offset: 0x00017F3C
	private void Event_Server_OnPhaseExpired(Dictionary<string, object> message)
	{
		GamePhase gamePhase = (GamePhase)message["phase"];
		this.OnGamePhaseTimedOut(gamePhase);
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x00019D64 File Offset: 0x00017F64
	private void Event_Server_OnPuckEnterGoal(Dictionary<string, object> message)
	{
		PlayerTeam team = (PlayerTeam)message["team"];
		Puck puck = (Puck)message["puck"];
		this.OnPuckEnterGoal(team, puck);
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x00019D9C File Offset: 0x00017F9C
	private void Event_Everyone_OnPlayerEnterZone(Dictionary<string, object> message)
	{
		Zone zone = (Zone)message["zone"];
		PlayerBody playerBody = (PlayerBody)message["playerBody"];
		this.OnPlayerEnterZone(zone, playerBody);
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x00019DD4 File Offset: 0x00017FD4
	private void Event_Everyone_OnPlayerExitZone(Dictionary<string, object> message)
	{
		Zone zone = (Zone)message["zone"];
		PlayerBody playerBody = (PlayerBody)message["playerBody"];
		this.OnPlayerExitZone(zone, playerBody);
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x00019E0C File Offset: 0x0001800C
	private void Event_Server_OnPlayerRequestTeamSelect(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (player.IsReplay.Value)
		{
			return;
		}
		this.OnPlayerRequestTeamSelect(player);
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x00019E40 File Offset: 0x00018040
	private void Event_Server_OnPlayerRequestTeam(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		PlayerTeam team = (PlayerTeam)message["team"];
		if (player.IsReplay.Value)
		{
			return;
		}
		this.OnPlayerRequestTeam(player, team);
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x00019E88 File Offset: 0x00018088
	private void Event_Server_OnPlayerRequestPositionSelect(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (player.IsReplay.Value)
		{
			return;
		}
		this.OnPlayerRequestPositionSelect(player);
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x00019EBC File Offset: 0x000180BC
	private void Event_Server_OnPlayerRequestPosition(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		PlayerPosition position = (PlayerPosition)message["playerPosition"];
		if (player.IsReplay.Value)
		{
			return;
		}
		this.OnPlayerRequestPosition(player, position);
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x00019F04 File Offset: 0x00018104
	private void Event_Server_OnPlayerRequestHandedness(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		PlayerHandedness handedness = (PlayerHandedness)message["handedness"];
		if (player.IsReplay.Value)
		{
			return;
		}
		this.OnPlayerRequestHandedness(player, handedness);
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x00019F4C File Offset: 0x0001814C
	private void Event_Server_OnPlayerRequestForfeit(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (player.IsReplay.Value)
		{
			return;
		}
		this.OnPlayerRequestForfeit(player);
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x00019F80 File Offset: 0x00018180
	private void Event_Server_OnVoteAdded(Dictionary<string, object> message)
	{
		Vote vote = (Vote)message["vote"];
		this.OnVoteAdded(vote);
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x00019FA8 File Offset: 0x000181A8
	private void Event_Server_OnVoteProgressed(Dictionary<string, object> message)
	{
		Vote vote = (Vote)message["vote"];
		string steamId = (string)message["steamId"];
		bool inFavour = (bool)message["inFavour"];
		this.OnVoteProgressed(vote, steamId, inFavour);
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x00019FF4 File Offset: 0x000181F4
	private void Event_Server_OnVoteRemoved(Dictionary<string, object> message)
	{
		Vote vote = (Vote)message["vote"];
		this.OnVoteRemoved(vote);
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x0001A01C File Offset: 0x0001821C
	private void Event_Server_OnChatCommand(Dictionary<string, object> message)
	{
		ulong clientId = (ulong)message["clientId"];
		string command = (string)message["command"];
		string[] args = (string[])message["args"];
		Player playerByClientId = this.PlayerManager.GetPlayerByClientId(clientId);
		if (playerByClientId != null)
		{
			this.OnChatCommand(playerByClientId, command, args);
		}
	}

	// Token: 0x0400033E RID: 830
	public Level Level;

	// Token: 0x0400033F RID: 831
	public ServerManager ServerManager;

	// Token: 0x04000340 RID: 832
	public GameManager GameManager;

	// Token: 0x04000341 RID: 833
	public PlayerManager PlayerManager;

	// Token: 0x04000342 RID: 834
	public PuckManager PuckManager;

	// Token: 0x04000343 RID: 835
	public ChatManager ChatManager;

	// Token: 0x04000344 RID: 836
	public ReplayManager ReplayManager;

	// Token: 0x04000345 RID: 837
	public VoteManager VoteManager;

	// Token: 0x04000347 RID: 839
	private string defaultConfigFilePath;

	// Token: 0x04000348 RID: 840
	private string configFilePathCliArgument;

	// Token: 0x04000349 RID: 841
	private string configCliArgument;

	// Token: 0x0400034A RID: 842
	private string configEnvVariable;
}
