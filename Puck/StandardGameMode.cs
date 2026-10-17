using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class StandardGameMode<TConfig> : BaseGameMode<TConfig> where TConfig : StandardGameModeConfig, new()
{
	// Token: 0x1700008B RID: 139
	// (get) Token: 0x0600051D RID: 1309 RVA: 0x0001B52C File Offset: 0x0001972C
	protected bool isGameInProgress
	{
		get
		{
			return Utils.IsGameInProgress(this.GameManager.Phase);
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x0600051E RID: 1310 RVA: 0x0001B53E File Offset: 0x0001973E
	protected bool isReplayable
	{
		get
		{
			return this.GameManager.Phase == GamePhase.FaceOff || this.GameManager.Phase == GamePhase.Play || this.GameManager.Phase == GamePhase.BlueScore || this.GameManager.Phase == GamePhase.RedScore;
		}
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x0001B57A File Offset: 0x0001977A
	public StandardGameMode(string defaultConfigFilePath, string configFilePathCliArgument = null, string configCliArgument = null, string configEnvVariable = null) : base(defaultConfigFilePath, configFilePathCliArgument, configCliArgument, configEnvVariable)
	{
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x0001B594 File Offset: 0x00019794
	public override bool Initialize(Level level, ServerManager serverManager, GameManager gameManager, PlayerManager playerManager, PuckManager puckManager, ChatManager chatManager, ReplayManager replayManager, VoteManager voteManager)
	{
		if (!base.Initialize(level, serverManager, gameManager, playerManager, puckManager, chatManager, replayManager, voteManager))
		{
			return false;
		}
		NetworkBehaviourSingleton<GameModeManager>.Instance.ClientConfig.Value = new GameModeClientConfig
		{
			GoalieCreaseProtection = base.Config.goalieCreaseProtection,
			MaxPeriods = base.Config.maxPeriods
		};
		if (base.Config.goalieCreaseProtection)
		{
			this.creaseProtection = new CreaseProtection();
		}
		this.StartGame(GamePhase.Warmup);
		this.GameManager.Server_StartTicking();
		return true;
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x0001B62E File Offset: 0x0001982E
	public override bool Dispose()
	{
		if (!base.Dispose())
		{
			return false;
		}
		CreaseProtection creaseProtection = this.creaseProtection;
		if (creaseProtection != null)
		{
			creaseProtection.Dispose();
		}
		this.creaseProtection = null;
		return true;
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x0001B653 File Offset: 0x00019853
	protected override void OnPlayerEnterZone(Zone zone, PlayerBody playerBody)
	{
		base.OnPlayerEnterZone(zone, playerBody);
		CreaseProtection creaseProtection = this.creaseProtection;
		if (creaseProtection == null)
		{
			return;
		}
		creaseProtection.OnPlayerEnterZone(zone, playerBody);
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x0001B66F File Offset: 0x0001986F
	protected override void OnPlayerExitZone(Zone zone, PlayerBody playerBody)
	{
		base.OnPlayerExitZone(zone, playerBody);
		CreaseProtection creaseProtection = this.creaseProtection;
		if (creaseProtection == null)
		{
			return;
		}
		creaseProtection.OnPlayerExitZone(zone, playerBody);
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x0001B68C File Offset: 0x0001988C
	protected bool CanPlayerEnterPhase(Player player, PlayerPhase phase)
	{
		switch (phase)
		{
		case PlayerPhase.TeamSelect:
			return true;
		case PlayerPhase.PositionSelect:
			return player.Team == PlayerTeam.Blue || player.Team == PlayerTeam.Red;
		case PlayerPhase.Play:
		case PlayerPhase.Replay:
			return (player.Team == PlayerTeam.Blue || player.Team == PlayerTeam.Red) && (player.Role == PlayerRole.Attacker || player.Role == PlayerRole.Goalie) && player.PlayerPosition != null;
		case PlayerPhase.Spectate:
			return player.Team == PlayerTeam.Spectator;
		default:
			return false;
		}
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x0001B70C File Offset: 0x0001990C
	protected virtual void PreparePlayersForGamePhase(GamePhase gamePhase)
	{
		foreach (Player player in this.PlayerManager.GetPlayers(false))
		{
			this.PreparePlayerForGamePhase(player, gamePhase);
		}
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x0001B768 File Offset: 0x00019968
	protected virtual void PreparePlayerForGamePhase(Player player, GamePhase gamePhase)
	{
		player.Server_CancelDelayedGameState();
		switch (gamePhase)
		{
		case GamePhase.Warmup:
			if (this.CanPlayerEnterPhase(player, PlayerPhase.Play))
			{
				PlayerPhase? phase = new PlayerPhase?(PlayerPhase.Play);
				PlayerTeam? team = null;
				PlayerRole? role = null;
				float? delay = null;
				player.Server_SetGameState(phase, team, role, delay);
			}
			break;
		case GamePhase.PreGame:
			if (this.CanPlayerEnterPhase(player, PlayerPhase.PositionSelect))
			{
				PlayerPhase? phase2 = new PlayerPhase?(PlayerPhase.PositionSelect);
				PlayerTeam? team2 = null;
				PlayerRole? role2 = null;
				float? delay = null;
				player.Server_SetGameState(phase2, team2, role2, delay);
			}
			break;
		case GamePhase.FaceOff:
			if (player.Phase == PlayerPhase.Play)
			{
				player.Server_SpawnCharacter(player.PlayerPosition.transform.position, player.PlayerPosition.transform.rotation, player.Role);
			}
			else if (this.CanPlayerEnterPhase(player, PlayerPhase.Play))
			{
				PlayerPhase? phase3 = new PlayerPhase?(PlayerPhase.Play);
				PlayerTeam? team3 = null;
				PlayerRole? role3 = null;
				float? delay = null;
				player.Server_SetGameState(phase3, team3, role3, delay);
			}
			break;
		case GamePhase.Play:
			if (player.Phase != PlayerPhase.Play && this.CanPlayerEnterPhase(player, PlayerPhase.Play))
			{
				PlayerPhase? phase4 = new PlayerPhase?(PlayerPhase.Play);
				float? delay = new float?(base.Config.spawnDelay);
				player.Server_SetGameState(phase4, null, null, delay);
				this.ChatManager.Server_SendChatMessage(string.Format("Spawning in {0} seconds...", base.Config.spawnDelay), "#ffe97f", new ulong[]
				{
					player.OwnerClientId
				});
			}
			break;
		case GamePhase.Replay:
			if (this.CanPlayerEnterPhase(player, PlayerPhase.Replay))
			{
				PlayerPhase? phase5 = new PlayerPhase?(PlayerPhase.Replay);
				PlayerTeam? team4 = null;
				PlayerRole? role4 = null;
				float? delay = null;
				player.Server_SetGameState(phase5, team4, role4, delay);
			}
			break;
		}
		if (player.IsCharacterSpawned)
		{
			if (gamePhase == GamePhase.FaceOff)
			{
				player.PlayerBody.Server_Freeze((RigidbodyConstraints)10);
				return;
			}
			player.PlayerBody.Server_Unfreeze();
		}
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x0001B96E File Offset: 0x00019B6E
	protected virtual void ClearGameResult()
	{
		this.PlayerManager.GetPlayers(false).ForEach(delegate(Player player)
		{
			player.Server_ResetPoints();
			if (this.CanPlayerEnterPhase(player, PlayerPhase.PositionSelect))
			{
				player.Server_SetGameState(new PlayerPhase?(PlayerPhase.PositionSelect), null, null, null);
				if (player.PlayerPosition != null)
				{
					player.PlayerPosition.Server_Unclaim();
				}
			}
		});
		this.gameResult = new GameResult();
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x0001B998 File Offset: 0x00019B98
	protected virtual void UpdateGameResult(PlayerTeam? forfeitingTeam = null)
	{
		PlayerTeam winningTeam = (forfeitingTeam == null) ? ((this.GameManager.BlueScore > this.GameManager.RedScore) ? PlayerTeam.Blue : ((this.GameManager.BlueScore < this.GameManager.RedScore) ? PlayerTeam.Red : PlayerTeam.None)) : Utils.GetOpposingTeam(forfeitingTeam.Value).GetValueOrDefault();
		this.gameResult.winningTeam = winningTeam;
		this.gameResult.blueScore = this.GameManager.BlueScore;
		this.gameResult.redScore = this.GameManager.RedScore;
		this.gameResult.forfeit = (forfeitingTeam != null);
		foreach (Player player in this.PlayerManager.GetPlayers(false))
		{
			string key = player.SteamId.Value.ToString();
			if (!this.gameResult.playerResults.ContainsKey(key))
			{
				this.gameResult.playerResults[key] = new PlayerResult();
			}
			this.gameResult.playerResults[key].goals = player.Goals.Value;
			this.gameResult.playerResults[key].assists = player.Assists.Value;
		}
		this.UpdatePlayerResult(Array.Empty<string>());
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x0001BB28 File Offset: 0x00019D28
	protected virtual void UpdatePlayerResult(params string[] steamIds)
	{
		foreach (string key in steamIds)
		{
			if (!this.gameResult.playerResults.ContainsKey(key))
			{
				this.gameResult.playerResults[key] = new PlayerResult();
			}
		}
		foreach (Player player in this.PlayerManager.GetPlayers(false))
		{
			string key2 = player.SteamId.Value.ToString();
			if (!this.gameResult.playerResults.ContainsKey(key2))
			{
				this.gameResult.playerResults[key2] = new PlayerResult();
			}
			this.gameResult.playerResults[key2].goals = player.Goals.Value;
			this.gameResult.playerResults[key2].assists = player.Assists.Value;
		}
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x0001BC4C File Offset: 0x00019E4C
	protected virtual void StartGame(GamePhase phase = GamePhase.Warmup)
	{
		this.ClearGameResult();
		this.UpdateGameResult(null);
		this.GameManager.Server_SetGameState(new GamePhase?(phase), new int?(base.Config.phaseDurationMap[phase]), new int?(1), new int?(0), new int?(0), new bool?(false));
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x0001BCB4 File Offset: 0x00019EB4
	protected virtual void EndGame()
	{
		this.UpdateGameResult(null);
		this.GameManager.Server_SetGameState(new GamePhase?(GamePhase.GameOver), new int?(base.Config.phaseDurationMap[GamePhase.GameOver]), null, null, null, null);
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x0001BD24 File Offset: 0x00019F24
	protected virtual void ForfeitGame(PlayerTeam forfeitingTeam)
	{
		this.UpdateGameResult(new PlayerTeam?(forfeitingTeam));
		this.GameManager.Server_SetGameState(new GamePhase?(GamePhase.GameOver), new int?(base.Config.phaseDurationMap[GamePhase.GameOver]), null, null, null, null);
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x0001BD90 File Offset: 0x00019F90
	protected override void OnWarmupTimedOut()
	{
		base.OnWarmupTimedOut();
		this.GameManager.Server_SetGameState(new GamePhase?(GamePhase.PreGame), new int?(base.Config.phaseDurationMap[GamePhase.PreGame]), null, null, null, null);
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x0001BDF4 File Offset: 0x00019FF4
	protected override void OnPreGameTimedOut()
	{
		base.OnPreGameTimedOut();
		this.GameManager.Server_SetGameState(new GamePhase?(GamePhase.FaceOff), new int?(base.Config.phaseDurationMap[GamePhase.FaceOff]), null, null, null, null);
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x0001BE58 File Offset: 0x0001A058
	protected override void OnFaceOffTimedOut()
	{
		base.OnFaceOffTimedOut();
		this.GameManager.Server_SetGameState(new GamePhase?(GamePhase.Play), new int?(this.tickRemainder), null, null, null, null);
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x0001BEAC File Offset: 0x0001A0AC
	protected override void OnPlayTimedOut()
	{
		base.OnPlayTimedOut();
		if (this.GameManager.Period < base.Config.maxPeriods)
		{
			this.tickRemainder = base.Config.phaseDurationMap[GamePhase.Play];
			GameManager gameManager = this.GameManager;
			GamePhase? phase = new GamePhase?(GamePhase.Intermission);
			int? tick = new int?(base.Config.phaseDurationMap[GamePhase.Intermission]);
			int? period = new int?(this.GameManager.Period + 1);
			int? blueScore = null;
			int? redScore = null;
			bool? isOvertime = null;
			gameManager.Server_SetGameState(phase, tick, period, blueScore, redScore, isOvertime);
			return;
		}
		if (this.GameManager.BlueScore == this.GameManager.RedScore)
		{
			this.tickRemainder = base.Config.phaseDurationMap[GamePhase.Play];
			GameManager gameManager2 = this.GameManager;
			GamePhase? phase2 = new GamePhase?(GamePhase.Intermission);
			int? tick2 = new int?(base.Config.phaseDurationMap[GamePhase.Intermission]);
			int? period2 = new int?(this.GameManager.Period + 1);
			bool? isOvertime = new bool?(true);
			gameManager2.Server_SetGameState(phase2, tick2, period2, null, null, isOvertime);
			return;
		}
		this.EndGame();
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x0001BFE8 File Offset: 0x0001A1E8
	protected override void OnBlueScoreTimedOut()
	{
		base.OnBlueScoreTimedOut();
		this.GameManager.Server_SetGameState(new GamePhase?(GamePhase.Replay), new int?(base.Config.phaseDurationMap[GamePhase.Replay]), null, null, null, null);
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x0001C04C File Offset: 0x0001A24C
	protected override void OnRedScoreTimedOut()
	{
		base.OnRedScoreTimedOut();
		this.GameManager.Server_SetGameState(new GamePhase?(GamePhase.Replay), new int?(base.Config.phaseDurationMap[GamePhase.Replay]), null, null, null, null);
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x0001C0B0 File Offset: 0x0001A2B0
	protected override void OnReplayTimedOut()
	{
		base.OnReplayTimedOut();
		if (this.GameManager.IsOvertime)
		{
			this.EndGame();
			return;
		}
		this.GameManager.Server_SetGameState(new GamePhase?(GamePhase.FaceOff), new int?(base.Config.phaseDurationMap[GamePhase.FaceOff]), null, null, null, null);
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x0001C128 File Offset: 0x0001A328
	protected override void OnIntermissionTimedOut()
	{
		base.OnIntermissionTimedOut();
		this.GameManager.Server_SetGameState(new GamePhase?(GamePhase.FaceOff), new int?(base.Config.phaseDurationMap[GamePhase.FaceOff]), null, null, null, null);
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x0001C18C File Offset: 0x0001A38C
	protected override void OnGameOverTimedOut()
	{
		base.OnGameOverTimedOut();
		this.GameManager.Server_SetGameState(new GamePhase?(GamePhase.PostGame), new int?(base.Config.phaseDurationMap[GamePhase.PostGame]), null, null, null, null);
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0001C1F1 File Offset: 0x0001A3F1
	protected override void OnPostGameTimedOut()
	{
		base.OnPostGameTimedOut();
		this.StartGame(GamePhase.Warmup);
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x0001C200 File Offset: 0x0001A400
	protected override void OnGamePhaseStarted(GamePhase gamePhase)
	{
		base.OnGamePhaseStarted(gamePhase);
		if (this.isReplayable)
		{
			this.ReplayManager.Server_StartRecording();
		}
		else
		{
			this.ReplayManager.Server_StopRecording();
		}
		if (!this.isGameInProgress)
		{
			foreach (Vote vote in this.VoteManager.Server_GetVotesByName("forfeit"))
			{
				this.VoteManager.Server_RemoveVote(vote);
			}
		}
		this.PreparePlayersForGamePhase(gamePhase);
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x0001C272 File Offset: 0x0001A472
	protected override void OnWarmupStarted()
	{
		base.OnWarmupStarted();
		this.PuckManager.Server_DespawnPucks(false);
		this.PuckManager.Server_SpawnPucksForPhase(GamePhase.Warmup);
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x0001C292 File Offset: 0x0001A492
	protected override void OnPreGameStarted()
	{
		base.OnPreGameStarted();
		this.PuckManager.Server_DespawnPucks(false);
		this.tickRemainder = base.Config.phaseDurationMap[GamePhase.Play];
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x0001C2C2 File Offset: 0x0001A4C2
	protected override void OnFaceOffStarted()
	{
		base.OnFaceOffStarted();
		this.PuckManager.Server_DespawnPucks(false);
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x0001C2D6 File Offset: 0x0001A4D6
	protected override void OnPlayStarted()
	{
		base.OnPlayStarted();
		this.PuckManager.Server_SpawnPucksForPhase(GamePhase.Play);
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x0001C2EC File Offset: 0x0001A4EC
	protected override void OnBlueScoreStarted()
	{
		base.OnBlueScoreStarted();
		this.Level.Server_PlayerCheerSound((float)(base.Config.phaseDurationMap[GamePhase.BlueScore] + base.Config.phaseDurationMap[GamePhase.Replay]));
		this.Level.Server_PlayRedGoalSound();
		this.Server_StartGoalSlowMotion();
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x0001C34C File Offset: 0x0001A54C
	protected override void OnRedScoreStarted()
	{
		base.OnRedScoreStarted();
		this.Level.Server_PlayerCheerSound((float)(base.Config.phaseDurationMap[GamePhase.RedScore] + base.Config.phaseDurationMap[GamePhase.Replay]));
		this.Level.Server_PlayBlueGoalSound();
		this.Server_StartGoalSlowMotion();
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x0001C3A9 File Offset: 0x0001A5A9
	protected override void OnIntermissionStarted()
	{
		base.OnIntermissionStarted();
		this.Level.Server_PlayHornSound();
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x0001C3BC File Offset: 0x0001A5BC
	protected virtual void Server_StartGoalSlowMotion()
	{
		if (!base.Config.goalSlowMotion)
		{
			return;
		}
		this.GameManager.Server_StartSlowMotion(0.35f, 0.08f, 0.28f, 0.42f, 1f, 0f);
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x0001C3FC File Offset: 0x0001A5FC
	protected virtual void Server_ScheduleReplaySlowMotion(Player goalPlayer, Puck puck)
	{
		if (!base.Config.goalSlowMotion)
		{
			return;
		}
		if (!puck)
		{
			return;
		}
		float? num = this.Server_GetReplaySlowMotionTouchTime(goalPlayer, puck);
		if (num != null)
		{
			this.Server_ScheduleReplayScorerTouchSlowMotion(num.Value);
		}
		this.Server_ScheduleReplayGoalEntrySlowMotion(Time.time);
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x0001C44F File Offset: 0x0001A64F
	protected virtual void Server_ScheduleReplayScorerTouchSlowMotion(float touchTime)
	{
		this.ReplayManager.Server_ScheduleSlowMotionAtWorldTime(touchTime - 0.6f, 0.3f, 0.5f, 0.45f, 0.5f, 0.95f, 12f);
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x0001C481 File Offset: 0x0001A681
	protected virtual void Server_ScheduleReplayGoalEntrySlowMotion(float goalTime)
	{
		this.ReplayManager.Server_ScheduleSlowMotionAtWorldTime(goalTime - 0.4f, 0.3f, 0.4f, 0.4f, 0.5f, 0.95f, 12f);
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x0001C4B4 File Offset: 0x0001A6B4
	protected virtual float? Server_GetReplaySlowMotionTouchTime(Player goalPlayer, Puck puck)
	{
		IReadOnlyDictionary<Player, float> lastContactTimeByPlayer = puck.ReplayTouchTracker.LastContactTimeByPlayer;
		if (lastContactTimeByPlayer.Count == 0)
		{
			return null;
		}
		if (puck.IsActivelyTouchedBy(goalPlayer))
		{
			return new float?(Time.time);
		}
		float num;
		if (goalPlayer != null && lastContactTimeByPlayer.TryGetValue(goalPlayer, out num) && Time.time - num <= 3f)
		{
			return new float?(num);
		}
		Player player = null;
		float num2 = float.MinValue;
		foreach (KeyValuePair<Player, float> keyValuePair in lastContactTimeByPlayer)
		{
			if (keyValuePair.Value > num2)
			{
				num2 = keyValuePair.Value;
				player = keyValuePair.Key;
			}
		}
		if (puck.IsActivelyTouchedBy(player))
		{
			return new float?(Time.time);
		}
		return new float?(num2);
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0001C594 File Offset: 0x0001A794
	protected override void OnReplayStarted()
	{
		base.OnReplayStarted();
		this.GameManager.Server_StopSlowMotion(true);
		this.PuckManager.Server_DespawnPucks(false);
		this.ReplayManager.Server_StartReplaying((float)base.Config.phaseDurationMap[GamePhase.Replay]);
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0001C5E4 File Offset: 0x0001A7E4
	protected override void OnGameOverStarted()
	{
		base.OnGameOverStarted();
		PlayerTeam winningTeam = this.gameResult.winningTeam;
		if (this.gameResult.forfeit)
		{
			string str = StringUtils.WrapInTeamColor(winningTeam.ToString(), winningTeam);
			this.ChatManager.Server_BroadcastChatMessage("Game Over! " + str + " team wins by forfeit!", "#ffe97f");
		}
		else
		{
			PlayerTeam team = (winningTeam == PlayerTeam.Blue) ? PlayerTeam.Red : PlayerTeam.Blue;
			int num = Mathf.Max(this.gameResult.blueScore, this.gameResult.redScore);
			int num2 = Mathf.Min(this.gameResult.blueScore, this.gameResult.redScore);
			string content = string.Concat(new string[]
			{
				"Game Over! ",
				StringUtils.WrapInTeamColor(winningTeam.ToString(), winningTeam),
				" team wins with a score of ",
				StringUtils.WrapInTeamColor(num.ToString(), winningTeam),
				" to ",
				StringUtils.WrapInTeamColor(num2.ToString(), team),
				"!"
			});
			this.ChatManager.Server_BroadcastChatMessage(content, "#ffe97f");
		}
		this.Level.Server_PlayBlueGoalSound();
		this.Level.Server_PlayRedGoalSound();
		this.Level.Server_PlayHornSound();
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x0001C721 File Offset: 0x0001A921
	protected override void OnReplayEnded()
	{
		base.OnReplayEnded();
		this.ReplayManager.Server_StopReplaying();
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x0001C734 File Offset: 0x0001A934
	protected override void OnPlayerJoined(Player player)
	{
		base.OnPlayerJoined(player);
		string text = player.SteamId.Value.ToString();
		this.UpdatePlayerResult(new string[]
		{
			text
		});
		if (this.gameResult.playerResults.ContainsKey(text))
		{
			PlayerResult playerResult = this.gameResult.playerResults[text];
			player.Goals.Value = playerResult.goals;
			player.Assists.Value = playerResult.assists;
		}
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0001C7BC File Offset: 0x0001A9BC
	protected override void OnPlayerLeft(Player player)
	{
		base.OnPlayerLeft(player);
		string text = player.SteamId.Value.ToString();
		this.UpdatePlayerResult(new string[]
		{
			text
		});
		if (player.PlayerPosition != null)
		{
			player.PlayerPosition.Server_Unclaim();
		}
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x0001C814 File Offset: 0x0001AA14
	protected override void OnPlayerPhaseChanged(Player player, PlayerPhase oldPhase, PlayerPhase newPhase)
	{
		base.OnPlayerPhaseChanged(player, oldPhase, newPhase);
		switch (newPhase)
		{
		case PlayerPhase.PositionSelect:
			if (player.IsCharacterSpawned)
			{
				player.Server_DespawnCharacter();
				return;
			}
			break;
		case PlayerPhase.Play:
			player.Server_SpawnCharacter(player.PlayerPosition.transform.position, player.PlayerPosition.transform.rotation, player.Role);
			return;
		case PlayerPhase.Replay:
			if (player.IsCharacterSpawned)
			{
				player.Server_DespawnCharacter();
				return;
			}
			break;
		case PlayerPhase.Spectate:
			player.Server_SpawnSpectatorCamera(Vector3.zero, Quaternion.identity);
			return;
		default:
			if (player.IsCharacterSpawned)
			{
				player.Server_DespawnCharacter();
			}
			if (player.PlayerPosition != null)
			{
				player.PlayerPosition.Server_Unclaim();
			}
			break;
		}
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0001C8C8 File Offset: 0x0001AAC8
	protected override void OnPlayerPositionChanged(Player player, PlayerPosition oldPlayerPosition, PlayerPosition newPlayerPosition)
	{
		base.OnPlayerPositionChanged(player, oldPlayerPosition, newPlayerPosition);
		PlayerRole? role;
		if (newPlayerPosition == null)
		{
			role = new PlayerRole?(PlayerRole.None);
			player.Server_SetGameState(null, null, role, null);
			return;
		}
		role = new PlayerRole?(newPlayerPosition.Role);
		player.Server_SetGameState(null, null, role, null);
		this.PreparePlayerForGamePhase(player, this.GameManager.Phase);
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x0001C954 File Offset: 0x0001AB54
	protected override void OnGoalScored(PlayerTeam byTeam, Player goalPlayer, Player assistPlayer, Player secondAssistPlayer, Puck puck)
	{
		base.OnGoalScored(byTeam, goalPlayer, assistPlayer, secondAssistPlayer, puck);
		this.tickRemainder = this.GameManager.Tick;
		this.Server_ScheduleReplaySlowMotion(goalPlayer, puck);
		if (goalPlayer)
		{
			this.ReplayManager.Server_ScheduleCellyCam(goalPlayer.OwnerClientId, 0.75f);
		}
		if (goalPlayer)
		{
			goalPlayer.Server_GoalScored();
		}
		if (assistPlayer)
		{
			assistPlayer.Server_AssistScored();
		}
		if (secondAssistPlayer)
		{
			secondAssistPlayer.Server_AssistScored();
		}
		if (byTeam != PlayerTeam.Blue)
		{
			if (byTeam == PlayerTeam.Red)
			{
				GameManager gameManager = this.GameManager;
				GamePhase? phase = new GamePhase?(GamePhase.RedScore);
				int? tick = new int?(base.Config.phaseDurationMap[GamePhase.RedScore]);
				int? num = new int?(this.GameManager.GameState.Value.RedScore + 1);
				gameManager.Server_SetGameState(phase, tick, null, null, num, null);
			}
		}
		else
		{
			GameManager gameManager2 = this.GameManager;
			GamePhase? phase2 = new GamePhase?(GamePhase.BlueScore);
			int? tick2 = new int?(base.Config.phaseDurationMap[GamePhase.BlueScore]);
			int? num = new int?(this.GameManager.GameState.Value.BlueScore + 1);
			gameManager2.Server_SetGameState(phase2, tick2, null, num, null, null);
		}
		if (puck)
		{
			string text = puck.Speed.ToString(CultureInfo.InvariantCulture);
			string text2 = puck.ShotSpeed.ToString(CultureInfo.InvariantCulture);
			this.ChatManager.Server_BroadcastChatMessage(string.Concat(new string[]
			{
				"Goal scored! <united>",
				text,
				"</united> &units across the line, <united>",
				text2,
				"</united> &units from the stick."
			}), "#ffe97f");
		}
		else
		{
			this.ChatManager.Server_BroadcastChatMessage("Goal scored!", "#ffe97f");
		}
		this.UpdateGameResult(null);
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x0001CB44 File Offset: 0x0001AD44
	protected override void OnPuckEnterGoal(PlayerTeam team, Puck puck)
	{
		base.OnPuckEnterGoal(team, puck);
		if (this.GameManager.Phase != GamePhase.Play)
		{
			return;
		}
		PlayerTeam? opposingTeam = Utils.GetOpposingTeam(team);
		if (opposingTeam == null)
		{
			return;
		}
		List<KeyValuePair<Player, float>> playerCollisionsByTeam = puck.GetPlayerCollisionsByTeam(opposingTeam.Value);
		Player goalPlayer = null;
		Player assistPlayer = null;
		Player secondAssistPlayer = null;
		if (playerCollisionsByTeam.Count >= 1)
		{
			List<KeyValuePair<Player, float>> list = playerCollisionsByTeam;
			goalPlayer = list[list.Count - 1].Key;
			if (playerCollisionsByTeam.Count >= 2)
			{
				List<KeyValuePair<Player, float>> list2 = playerCollisionsByTeam;
				assistPlayer = list2[list2.Count - 2].Key;
			}
			if (playerCollisionsByTeam.Count >= 3)
			{
				List<KeyValuePair<Player, float>> list3 = playerCollisionsByTeam;
				secondAssistPlayer = list3[list3.Count - 3].Key;
			}
		}
		this.ScoreGoal(opposingTeam.Value, goalPlayer, assistPlayer, secondAssistPlayer, puck);
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x0001CC04 File Offset: 0x0001AE04
	protected override void OnPlayerRequestPositionSelect(Player player)
	{
		base.OnPlayerRequestPositionSelect(player);
		if (this.CanPlayerEnterPhase(player, PlayerPhase.PositionSelect))
		{
			player.Server_SetGameState(new PlayerPhase?(PlayerPhase.PositionSelect), null, null, null);
			if (player.PlayerPosition != null)
			{
				player.PlayerPosition.Server_Unclaim();
			}
		}
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0001CC64 File Offset: 0x0001AE64
	protected override void OnPlayerRequestPosition(Player player, PlayerPosition position)
	{
		base.OnPlayerRequestPosition(player, position);
		if (position.IsClaimed)
		{
			if (player == position.ClaimedByPlayer)
			{
				position.Server_Unclaim();
				if (this.CanPlayerEnterPhase(player, PlayerPhase.PositionSelect))
				{
					player.Server_SetGameState(new PlayerPhase?(PlayerPhase.PositionSelect), null, null, null);
					return;
				}
			}
		}
		else if (position.Team == player.Team)
		{
			PlayerPosition playerPosition = player.PlayerPosition;
			position.Server_Claim(player);
			if (playerPosition != null)
			{
				playerPosition.Server_Unclaim();
			}
		}
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0001CCF2 File Offset: 0x0001AEF2
	protected override void OnPlayerRequestForfeit(Player player)
	{
		base.OnPlayerRequestForfeit(player);
		this.StartForfeitVote(player);
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0001CD04 File Offset: 0x0001AF04
	protected virtual void StartForfeitVote(Player player)
	{
		if (player.Team != PlayerTeam.Blue && player.Team != PlayerTeam.Red)
		{
			this.ChatManager.Server_SendChatMessage("You must be on a team to start this vote", "#e74c3c", new ulong[]
			{
				player.OwnerClientId
			});
			return;
		}
		if (!this.isGameInProgress)
		{
			this.ChatManager.Server_SendChatMessage("You can not forfeit right now", "#e74c3c", new ulong[]
			{
				player.OwnerClientId
			});
			return;
		}
		Vote vote = this.VoteManager.Server_GetTeamVoteByName("forfeit", player.Team);
		if (vote != null)
		{
			vote.CastVote(player.SteamId.Value.ToString(), true);
			return;
		}
		this.VoteManager.Server_AddVote("forfeit", "Forfeit", "use /ff or /forfeit", new PlayerTeam[]
		{
			player.Team
		}, 30f, player.SteamId.Value.ToString(), Utils.GetVoteMajority(this.PlayerManager.GetPlayersByTeam(player.Team, false).Count), player.Team);
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x0001CE20 File Offset: 0x0001B020
	protected override void OnVoteRemoved(Vote vote)
	{
		base.OnVoteRemoved(vote);
		if (!vote.Passed)
		{
			return;
		}
		string name = vote.Name;
		if (!(name == "kick"))
		{
			if (!(name == "forfeit"))
			{
				return;
			}
			PlayerTeam forfeitingTeam = (PlayerTeam)vote.Data;
			this.ForfeitGame(forfeitingTeam);
			return;
		}
		else
		{
			string text = (string)vote.Data;
			Player playerBySteamId = this.PlayerManager.GetPlayerBySteamId(text);
			if (playerBySteamId)
			{
				this.ServerManager.Server_KickPlayer(playerBySteamId, DisconnectionCode.Kicked, null, true);
				return;
			}
			this.ServerManager.TimeoutManager.AddSteamIdTimeout(text, 60f);
			return;
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000552 RID: 1362 RVA: 0x0001CEBF File Offset: 0x0001B0BF
	protected virtual bool AllowVotekick
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0001CEC4 File Offset: 0x0001B0C4
	protected override List<ChatCommandInfo> GetChatCommands()
	{
		List<ChatCommandInfo> chatCommands = base.GetChatCommands();
		if (this.AllowVotekick)
		{
			chatCommands.Add(new ChatCommandInfo("/vk, /votekick <player>", "Start a vote to kick a player", 0));
		}
		chatCommands.Add(new ChatCommandInfo("/ff, /forfeit", "Start a vote to forfeit the game", 0));
		chatCommands.Add(new ChatCommandInfo("/kick <player>", "Kick a player", 1));
		chatCommands.Add(new ChatCommandInfo("/ban <player|steamId>", "Ban a player", 1));
		chatCommands.Add(new ChatCommandInfo("/unban <steamId>", "Unban a Steam ID", 1));
		chatCommands.Add(new ChatCommandInfo("/start", "Start the game", 1));
		chatCommands.Add(new ChatCommandInfo("/warmup", "Return to warmup", 1));
		chatCommands.Add(new ChatCommandInfo("/pause", "Pause the game", 1));
		chatCommands.Add(new ChatCommandInfo("/resume, /unpause", "Resume the game", 1));
		chatCommands.Add(new ChatCommandInfo("/skip", "Skip to the next phase", 1));
		chatCommands.Add(new ChatCommandInfo("/bots", "Spawn and manage bots", 3));
		return chatCommands;
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x0001CFD4 File Offset: 0x0001B1D4
	protected override void OnChatCommand(Player player, string command, string[] args)
	{
		bool flag = player.AdminLevel.Value > 0 || this.ServerManager.AdminManager.IsSteamIdAdmin(player.SteamId.Value.ToString());
		uint num = <PrivateImplementationDetails>.ComputeStringHash(command);
		if (num > 2545781614U)
		{
			if (num <= 3277915388U)
			{
				if (num != 2919672153U)
				{
					if (num != 3194771123U)
					{
						if (num != 3277915388U)
						{
							goto IL_B4E;
						}
						if (!(command == "/votekick"))
						{
							goto IL_B4E;
						}
					}
					else
					{
						if (!(command == "/ban"))
						{
							goto IL_B4E;
						}
						if (!flag)
						{
							this.ChatManager.Server_SendChatMessage("You do not have permissions to use this command", "#e74c3c", new ulong[]
							{
								player.OwnerClientId
							});
							return;
						}
						string text = (args.Length != 0) ? args[0] : string.Empty;
						Player playerByNeedle = this.PlayerManager.GetPlayerByNeedle(text, true);
						if (playerByNeedle)
						{
							this.ServerManager.Server_BanPlayer(playerByNeedle);
							this.ChatManager.Server_SendChatMessage(string.Format("Banned {0}", playerByNeedle.Username.Value), "#b8b8b8", new ulong[]
							{
								player.OwnerClientId
							});
							return;
						}
						if (Utils.IsValidSteamId64(text))
						{
							this.ServerManager.Server_BanSteamId(text);
							this.ChatManager.Server_SendChatMessage("Banned Steam ID " + text, "#b8b8b8", new ulong[]
							{
								player.OwnerClientId
							});
							return;
						}
						this.ChatManager.Server_SendChatMessage("Player not found", "#e74c3c", new ulong[]
						{
							player.OwnerClientId
						});
						return;
					}
				}
				else
				{
					if (!(command == "/forfeit"))
					{
						goto IL_B4E;
					}
					goto IL_405;
				}
			}
			else if (num <= 3777082446U)
			{
				if (num != 3607949496U)
				{
					if (num != 3777082446U)
					{
						goto IL_B4E;
					}
					if (!(command == "/unban"))
					{
						goto IL_B4E;
					}
					if (!flag)
					{
						this.ChatManager.Server_SendChatMessage("You do not have permissions to use this command", "#e74c3c", new ulong[]
						{
							player.OwnerClientId
						});
						return;
					}
					string text2 = (args.Length != 0) ? args[0] : string.Empty;
					if (!Utils.IsValidSteamId64(text2))
					{
						this.ChatManager.Server_SendChatMessage("Provide a SteamID64 to unban", "#e74c3c", new ulong[]
						{
							player.OwnerClientId
						});
						return;
					}
					if (!this.ServerManager.Server_IsSteamIdBanned(text2))
					{
						this.ChatManager.Server_SendChatMessage("Steam ID " + text2 + " is not banned", "#e74c3c", new ulong[]
						{
							player.OwnerClientId
						});
						return;
					}
					this.ServerManager.Server_UnbanSteamId(text2);
					this.ChatManager.Server_SendChatMessage("Unbanned Steam ID " + text2, "#b8b8b8", new ulong[]
					{
						player.OwnerClientId
					});
					return;
				}
				else
				{
					if (!(command == "/pause"))
					{
						goto IL_B4E;
					}
					if (!flag)
					{
						this.ChatManager.Server_SendChatMessage("You do not have permissions to use this command", "#e74c3c", new ulong[]
						{
							player.OwnerClientId
						});
						return;
					}
					if (!this.GameManager.IsTicking)
					{
						this.ChatManager.Server_SendChatMessage("The game is already paused", "#e74c3c", new ulong[]
						{
							player.OwnerClientId
						});
						return;
					}
					this.GameManager.Server_StopTicking();
					this.ChatManager.Server_BroadcastChatMessage(StringUtils.WrapInTeamColor(player.Username.Value.ToString(), player.Team) + " paused the game", "#b8b8b8");
					return;
				}
			}
			else if (num != 3819631050U)
			{
				if (num != 4169298025U)
				{
					goto IL_B4E;
				}
				if (!(command == "/vk"))
				{
					goto IL_B4E;
				}
			}
			else
			{
				if (!(command == "/ff"))
				{
					goto IL_B4E;
				}
				goto IL_405;
			}
			if (!this.AllowVotekick)
			{
				this.ChatManager.Server_SendChatMessage("Votekick is disabled in this game mode", "#e74c3c", new ulong[]
				{
					player.OwnerClientId
				});
				return;
			}
			Vote vote = this.VoteManager.Server_GetTeamVoteByName("kick", player.Team);
			if (vote != null)
			{
				vote.CastVote(player.SteamId.Value.ToString(), true);
				return;
			}
			string needle = (args.Length != 0) ? args[0] : string.Empty;
			Player playerByNeedle2 = this.PlayerManager.GetPlayerByNeedle(needle, true);
			if (!playerByNeedle2)
			{
				this.ChatManager.Server_SendChatMessage("Player not found", "#e74c3c", new ulong[]
				{
					player.OwnerClientId
				});
				return;
			}
			if (playerByNeedle2.AdminLevel.Value > 0 || this.ServerManager.AdminManager.IsSteamIdAdmin(playerByNeedle2.SteamId.Value.ToString()))
			{
				this.ChatManager.Server_SendChatMessage("You can't votekick an admin", "#e74c3c", new ulong[]
				{
					player.OwnerClientId
				});
				return;
			}
			string str = StringUtils.WrapInTeamColor(playerByNeedle2.Username.Value.ToString(), playerByNeedle2.Team);
			PlayerTeam[] array = new PlayerTeam[]
			{
				PlayerTeam.None,
				PlayerTeam.Blue,
				PlayerTeam.Red,
				PlayerTeam.Spectator
			};
			this.VoteManager.Server_AddVote("kick", "Kick " + str, "use /vk or /votekick", array, 30f, player.SteamId.Value.ToString(), Utils.GetVoteMajority(this.PlayerManager.GetPlayersByTeams(array, false).Count), playerByNeedle2.SteamId.Value.ToString());
			return;
			IL_405:
			this.StartForfeitVote(player);
			return;
		}
		if (num <= 709441620U)
		{
			if (num != 261473465U)
			{
				if (num != 580386680U)
				{
					if (num != 709441620U)
					{
						goto IL_B4E;
					}
					if (!(command == "/bots"))
					{
						goto IL_B4E;
					}
					if (player.AdminLevel.Value < 3)
					{
						this.ChatManager.Server_SendChatMessage("You do not have permissions to use this command", "#e74c3c", new ulong[]
						{
							player.OwnerClientId
						});
						return;
					}
					if (args.Length != 0 && args[0].ToLower() == "mode")
					{
						if (args.Length > 1)
						{
							string a = args[1].ToLower();
							if (!(a == "chase"))
							{
								if (!(a == "rookie"))
								{
									if (!(a == "destroy"))
									{
										this.ChatManager.Server_SendChatMessage("Usage: /bots mode <chase|rookie|destroy>", "#e74c3c", new ulong[]
										{
											player.OwnerClientId
										});
										return;
									}
									BotManager.Server_SetBotBehavior(BotManager.BotBehavior.Destroy);
								}
								else
								{
									BotManager.Server_SetBotBehavior(BotManager.BotBehavior.Rookie);
								}
							}
							else
							{
								BotManager.Server_SetBotBehavior(BotManager.BotBehavior.Chase);
							}
						}
						this.ChatManager.Server_SendChatMessage("Bot mode: " + BotManager.DefaultBehavior.ToString().ToLower(), "#ffe97f", new ulong[]
						{
							player.OwnerClientId
						});
						return;
					}
					if (args.Length != 0 && args[0].ToLower() == "clear")
					{
						int num2 = BotManager.Server_DespawnBots();
						this.ChatManager.Server_SendChatMessage(string.Format("Despawned {0} bots", num2), "#ffe97f", new ulong[]
						{
							player.OwnerClientId
						});
						return;
					}
					PlayerTeam playerTeam = PlayerTeam.None;
					int num3 = 0;
					if (args.Length != 0 && args[0].ToLower() == "red")
					{
						playerTeam = PlayerTeam.Red;
						num3 = 1;
					}
					else if (args.Length != 0 && args[0].ToLower() == "blue")
					{
						playerTeam = PlayerTeam.Blue;
						num3 = 1;
					}
					int count = 1;
					if (args.Length > num3 && !int.TryParse(args[num3], out count))
					{
						this.ChatManager.Server_SendChatMessage("Usage: /bots [red|blue] <count> | clear | mode <chase|rookie|destroy>", "#e74c3c", new ulong[]
						{
							player.OwnerClientId
						});
						return;
					}
					int num4 = BotManager.Server_SpawnBots(count, playerTeam);
					string arg = (playerTeam == PlayerTeam.None) ? "" : (" on " + playerTeam.ToString().ToLower());
					this.ChatManager.Server_SendChatMessage(string.Format("Spawned {0} bots{1} (use /bots clear to remove)", num4, arg), "#ffe97f", new ulong[]
					{
						player.OwnerClientId
					});
					return;
				}
				else
				{
					if (!(command == "/kick"))
					{
						goto IL_B4E;
					}
					if (!flag)
					{
						this.ChatManager.Server_SendChatMessage("You do not have permissions to use this command", "#e74c3c", new ulong[]
						{
							player.OwnerClientId
						});
						return;
					}
					string needle2 = (args.Length != 0) ? args[0] : string.Empty;
					Player playerByNeedle3 = this.PlayerManager.GetPlayerByNeedle(needle2, true);
					if (!playerByNeedle3)
					{
						this.ChatManager.Server_SendChatMessage("Player not found", "#e74c3c", new ulong[]
						{
							player.OwnerClientId
						});
						return;
					}
					this.ServerManager.Server_KickPlayer(playerByNeedle3, DisconnectionCode.Kicked, null, true);
					return;
				}
			}
			else if (!(command == "/resume"))
			{
				goto IL_B4E;
			}
		}
		else if (num <= 1918686277U)
		{
			if (num != 1760276126U)
			{
				if (num != 1918686277U)
				{
					goto IL_B4E;
				}
				if (!(command == "/unpause"))
				{
					goto IL_B4E;
				}
			}
			else
			{
				if (!(command == "/warmup"))
				{
					goto IL_B4E;
				}
				if (!flag)
				{
					this.ChatManager.Server_SendChatMessage("You do not have permissions to use this command", "#e74c3c", new ulong[]
					{
						player.OwnerClientId
					});
					return;
				}
				this.StartGame(GamePhase.Warmup);
				this.ChatManager.Server_BroadcastChatMessage(StringUtils.WrapInTeamColor(player.Username.Value.ToString(), player.Team) + " returned to warmup", "#b8b8b8");
				return;
			}
		}
		else if (num != 2459559413U)
		{
			if (num != 2545781614U)
			{
				goto IL_B4E;
			}
			if (!(command == "/start"))
			{
				goto IL_B4E;
			}
			if (!flag)
			{
				this.ChatManager.Server_SendChatMessage("You do not have permissions to use this command", "#e74c3c", new ulong[]
				{
					player.OwnerClientId
				});
				return;
			}
			this.StartGame(GamePhase.PreGame);
			this.ChatManager.Server_BroadcastChatMessage(StringUtils.WrapInTeamColor(player.Username.Value.ToString(), player.Team) + " started the game", "#b8b8b8");
			return;
		}
		else
		{
			if (!(command == "/skip"))
			{
				goto IL_B4E;
			}
			if (!flag)
			{
				this.ChatManager.Server_SendChatMessage("You do not have permissions to use this command", "#e74c3c", new ulong[]
				{
					player.OwnerClientId
				});
				return;
			}
			this.GameManager.Server_ExpirePhase();
			this.ChatManager.Server_BroadcastChatMessage(StringUtils.WrapInTeamColor(player.Username.Value.ToString(), player.Team) + " skipped to the next phase", "#b8b8b8");
			return;
		}
		if (!flag)
		{
			this.ChatManager.Server_SendChatMessage("You do not have permissions to use this command", "#e74c3c", new ulong[]
			{
				player.OwnerClientId
			});
			return;
		}
		if (this.GameManager.IsTicking)
		{
			this.ChatManager.Server_SendChatMessage("The game is not paused", "#e74c3c", new ulong[]
			{
				player.OwnerClientId
			});
			return;
		}
		this.GameManager.Server_StartTicking();
		this.ChatManager.Server_BroadcastChatMessage(StringUtils.WrapInTeamColor(player.Username.Value.ToString(), player.Team) + " resumed the game", "#b8b8b8");
		return;
		IL_B4E:
		base.OnChatCommand(player, command, args);
	}

	// Token: 0x04000372 RID: 882
	protected int tickRemainder;

	// Token: 0x04000373 RID: 883
	protected GameResult gameResult = new GameResult();

	// Token: 0x04000374 RID: 884
	protected CreaseProtection creaseProtection;
}
