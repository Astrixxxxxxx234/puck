using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000096 RID: 150
public class MatchableGameMode<TConfig> : StandardGameMode<TConfig> where TConfig : StandardGameModeConfig, new()
{
	// Token: 0x1700007C RID: 124
	// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0001A795 File Offset: 0x00018995
	protected MatchData matchData
	{
		get
		{
			return BackendManager.ServerState.MatchData;
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x060004D6 RID: 1238 RVA: 0x0001A7A1 File Offset: 0x000189A1
	protected bool isMatch
	{
		get
		{
			return this.matchData != null;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0001A7AC File Offset: 0x000189AC
	protected IEnumerable<string> abandonedSteamIds
	{
		get
		{
			return from m in this.steamIdMatchAbandonmentMap.Values
			where m.HasAbandoned
			select m.SteamId;
		}
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x0001A80C File Offset: 0x00018A0C
	public MatchableGameMode(string defaultConfigFilePath, string configFilePathCliArgument = null, string configCliArgument = null, string configEnvVariable = null) : base(defaultConfigFilePath, configFilePathCliArgument, configCliArgument, configEnvVariable)
	{
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x0001A824 File Offset: 0x00018A24
	public override bool Initialize(Level level, ServerManager serverManager, GameManager gameManager, PlayerManager playerManager, PuckManager puckManager, ChatManager chatManager, ReplayManager replayManager, VoteManager voteManager)
	{
		if (!base.Initialize(level, serverManager, gameManager, playerManager, puckManager, chatManager, replayManager, voteManager))
		{
			return false;
		}
		if (this.isMatch)
		{
			this.StartMatch();
		}
		return true;
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x0001A857 File Offset: 0x00018A57
	protected override void SubscribeEvents()
	{
		base.SubscribeEvents();
		EventManager.AddEventListener("Event_OnServerMatchDataChanged", new Action<Dictionary<string, object>>(this.Event_OnServerMatchDataChanged));
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x0001A875 File Offset: 0x00018A75
	protected override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();
		EventManager.RemoveEventListener("Event_OnServerMatchDataChanged", new Action<Dictionary<string, object>>(this.Event_OnServerMatchDataChanged));
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x0001A893 File Offset: 0x00018A93
	private void StartMatch()
	{
		if (this.isMatchStarted)
		{
			return;
		}
		this.isMatchStarted = true;
		this.OnMatchStarted();
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x0001A8AC File Offset: 0x00018AAC
	private void AddMatchAbandonment(string steamId)
	{
		if (this.steamIdMatchAbandonmentMap.ContainsKey(steamId))
		{
			return;
		}
		MatchAbandonment matchAbandonment = new MatchAbandonment(steamId, 120f);
		matchAbandonment.Abandoned += this.OnMatchAbandoned;
		this.steamIdMatchAbandonmentMap.Add(steamId, matchAbandonment);
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x0001A8F4 File Offset: 0x00018AF4
	private void ClearMatchAbandonments()
	{
		foreach (MatchAbandonment matchAbandonment in this.steamIdMatchAbandonmentMap.Values)
		{
			matchAbandonment.Dispose();
		}
		this.steamIdMatchAbandonmentMap.Clear();
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x0001A954 File Offset: 0x00018B54
	private MatchAbandonment GetMatchAbandonment(Player player)
	{
		string key = player.SteamId.Value.ToString();
		if (!this.steamIdMatchAbandonmentMap.ContainsKey(key))
		{
			return null;
		}
		return this.steamIdMatchAbandonmentMap[key];
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x0001A997 File Offset: 0x00018B97
	private void EndMatch()
	{
		if (!this.isMatchStarted)
		{
			return;
		}
		this.isMatchStarted = false;
		this.OnMatchEnded(this.gameResult);
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x0001A9B5 File Offset: 0x00018BB5
	private void CancelMatch()
	{
		if (!this.isMatchStarted)
		{
			return;
		}
		this.isMatchStarted = false;
		this.OnMatchCancelled();
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x0001A9D0 File Offset: 0x00018BD0
	private void IssueCooldown(string steamId)
	{
		if (!this.isMatchStarted)
		{
			return;
		}
		WebSocketManager.Emit("serverMatchIssueCooldown", new Dictionary<string, object>
		{
			{
				"steamId",
				steamId
			}
		}, null);
		MatchPlayer matchPlayerBySteamId = this.matchData.GetMatchPlayerBySteamId(steamId);
		PlayerTeam? teamBySteamId = this.matchData.GetTeamBySteamId(steamId);
		if (matchPlayerBySteamId != null && teamBySteamId != null)
		{
			this.ChatManager.Server_BroadcastChatMessage(StringUtils.WrapInTeamColor(matchPlayerBySteamId.username, teamBySteamId.Value) + " has been issued a matchmaking cooldown", "#b8b8b8");
		}
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x0001AA54 File Offset: 0x00018C54
	private bool BroadcastForfeitWarning(Player player)
	{
		PlayerTeam? teamBySteamId = this.matchData.GetTeamBySteamId(player.SteamId.Value.ToString());
		if (teamBySteamId == null)
		{
			return false;
		}
		MatchAbandonment[] array = (from m in (from id in this.matchData.GetTeamSteamIds(teamBySteamId.Value)
		where this.steamIdMatchAbandonmentMap.ContainsKey(id)
		select this.steamIdMatchAbandonmentMap[id]).ToArray<MatchAbandonment>()
		where !m.HasAbandoned
		select m).ToArray<MatchAbandonment>();
		if (array.Length != 0)
		{
			if (!array.Any((MatchAbandonment m) => m.IsPaused))
			{
				int num = (int)array.Max((MatchAbandonment m) => m.RemainingSeconds);
				this.ChatManager.Server_BroadcastChatMessage(string.Format("{0} team will auto-forfeit in {1} seconds, once all of its players have received a matchmaking cooldown", StringUtils.WrapInTeamColor(Utils.GetNameFromTeam(teamBySteamId.Value), teamBySteamId.Value), num), "#b8b8b8");
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x0001AB84 File Offset: 0x00018D84
	private void BroadcastAbandonmentWarning(Player player, MatchAbandonment matchAbandonment)
	{
		this.ChatManager.Server_BroadcastChatMessage(string.Format("{0} has {1} seconds to reconnect before receiving a matchmaking cooldown", StringUtils.WrapInTeamColor(player.Username.Value.ToString(), player.Team), (int)matchAbandonment.RemainingSeconds), "#b8b8b8");
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x0001ABDB File Offset: 0x00018DDB
	protected virtual void OnMatchStarted()
	{
		WebSocketManager.Emit("serverMatchStart", null, null);
		this.UpdatePlayerResult(this.matchData.SteamIds);
		this.ServerManager.WhitelistManager.AddWhitelistedSteamIds(this.matchData.SteamIds);
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x0001AC15 File Offset: 0x00018E15
	protected virtual void OnMatchEnded(GameResult gameResult)
	{
		WebSocketManager.Emit("serverMatchEnd", new Dictionary<string, object>
		{
			{
				"gameResult",
				gameResult
			}
		}, null);
		this.ClearMatchAbandonments();
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x0001AC39 File Offset: 0x00018E39
	protected virtual void OnMatchCancelled()
	{
		WebSocketManager.Emit("serverMatchCancel", null, null);
		this.ClearMatchAbandonments();
		this.GameManager.Server_StopTicking();
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x0001AC58 File Offset: 0x00018E58
	protected virtual void OnMatchAbandoned(MatchAbandonment matchAbandonment)
	{
		if (!this.isMatchStarted)
		{
			return;
		}
		this.IssueCooldown(matchAbandonment.SteamId);
		string[] array = this.matchData.HomeSteamIds.Except(this.abandonedSteamIds).ToArray<string>();
		string[] array2 = this.matchData.AwaySteamIds.Except(this.abandonedSteamIds).ToArray<string>();
		if (array.Length == 0)
		{
			this.ForfeitGame(PlayerTeam.Blue);
			return;
		}
		if (array2.Length == 0)
		{
			this.ForfeitGame(PlayerTeam.Red);
		}
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x0001ACC8 File Offset: 0x00018EC8
	protected override void OnWarmupTimedOut()
	{
		if (this.isMatchStarted)
		{
			string[] second = (from p in this.PlayerManager.GetPlayers(false)
			select p.SteamId.Value.ToString()).ToArray<string>();
			string[] array = this.matchData.SteamIds.Except(second).ToArray<string>();
			if (array.Length != 0)
			{
				this.ChatManager.Server_BroadcastChatMessage("Cancelling match due to players failing to join in time", "#b8b8b8");
				if (array.Length < this.matchData.SteamIds.Length)
				{
					foreach (string steamId in array)
					{
						this.IssueCooldown(steamId);
					}
				}
				this.CancelMatch();
				return;
			}
			foreach (string text in this.matchData.SteamIds)
			{
				this.AddMatchAbandonment(text);
				this.steamIdMatchAbandonmentMap[text].Stop();
			}
		}
		base.OnWarmupTimedOut();
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x0001ADBB File Offset: 0x00018FBB
	protected override void OnGameOverStarted()
	{
		base.OnGameOverStarted();
		this.ClearMatchAbandonments();
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x0001ADC9 File Offset: 0x00018FC9
	protected override void OnGameOverEnded()
	{
		base.OnGameOverEnded();
		if (this.isMatch)
		{
			this.EndMatch();
		}
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x0001ADDF File Offset: 0x00018FDF
	protected override void OnPlayerJoined(Player player)
	{
		base.OnPlayerJoined(player);
		MatchAbandonment matchAbandonment = this.GetMatchAbandonment(player);
		if (matchAbandonment == null)
		{
			return;
		}
		matchAbandonment.Stop();
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x0001ADFC File Offset: 0x00018FFC
	protected override void OnPlayerLeft(Player player)
	{
		base.OnPlayerLeft(player);
		MatchAbandonment matchAbandonment = this.GetMatchAbandonment(player);
		if (matchAbandonment == null)
		{
			return;
		}
		matchAbandonment.Start();
		if (matchAbandonment.HasAbandoned)
		{
			return;
		}
		if (this.BroadcastForfeitWarning(player))
		{
			return;
		}
		this.BroadcastAbandonmentWarning(player, matchAbandonment);
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x0001AE40 File Offset: 0x00019040
	private void Event_OnServerMatchDataChanged(Dictionary<string, object> message)
	{
		bool flag = (MatchData)message["oldServerMatchData"] != null;
		MatchData matchData = (MatchData)message["newServerMatchData"];
		if (!flag && matchData != null)
		{
			this.StartMatch();
		}
	}

	// Token: 0x0400035D RID: 861
	protected bool isMatchStarted;

	// Token: 0x0400035E RID: 862
	private Dictionary<string, MatchAbandonment> steamIdMatchAbandonmentMap = new Dictionary<string, MatchAbandonment>();
}
