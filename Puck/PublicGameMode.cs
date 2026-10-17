using System;
using System.Collections.Generic;

// Token: 0x02000099 RID: 153
public class PublicGameMode<TConfig> : StandardGameMode<TConfig> where TConfig : PublicGameModeConfig, new()
{
	// Token: 0x060004FA RID: 1274 RVA: 0x0001AEFA File Offset: 0x000190FA
	public PublicGameMode(string defaultConfigFilePath, string configFilePathCliArgument = null, string configCliArgument = null, string configEnvVariable = null) : base(defaultConfigFilePath, configFilePathCliArgument, configCliArgument, configEnvVariable)
	{
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x0001AF08 File Offset: 0x00019108
	protected override void OnWarmupTimedOut()
	{
		if (this.PlayerManager.GetPlayersByTeam(PlayerTeam.Blue, false).Count == 0 || this.PlayerManager.GetPlayersByTeam(PlayerTeam.Red, false).Count == 0)
		{
			this.GameManager.Server_SetGameState(new GamePhase?(GamePhase.Warmup), new int?(base.Config.phaseDurationMap[GamePhase.Warmup]), null, null, null, null);
			this.ChatManager.Server_BroadcastChatMessage("Not enough players to start the game. Extending warmup...", "#ffe97f");
			return;
		}
		base.OnWarmupTimedOut();
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x0001AFAC File Offset: 0x000191AC
	protected override void OnPlayerJoined(Player player)
	{
		base.OnPlayerJoined(player);
		if (base.CanPlayerEnterPhase(player, PlayerPhase.TeamSelect))
		{
			player.Server_SetGameState(new PlayerPhase?(PlayerPhase.TeamSelect), null, null, null);
		}
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x0001AFF4 File Offset: 0x000191F4
	protected override void OnPlayerRequestTeamSelect(Player player)
	{
		base.OnPlayerRequestTeamSelect(player);
		if (base.CanPlayerEnterPhase(player, PlayerPhase.TeamSelect))
		{
			player.Server_SetGameState(new PlayerPhase?(PlayerPhase.TeamSelect), new PlayerTeam?(PlayerTeam.None), null, null);
		}
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x0001B038 File Offset: 0x00019238
	protected override void OnPlayerRequestTeam(Player player, PlayerTeam team)
	{
		base.OnPlayerRequestTeam(player, team);
		if (team - PlayerTeam.Blue > 1)
		{
			if (team != PlayerTeam.Spectator)
			{
				return;
			}
			PlayerTeam? team2 = new PlayerTeam?(team);
			player.Server_SetGameState(null, team2, null, null);
			if (base.CanPlayerEnterPhase(player, PlayerPhase.Spectate))
			{
				PlayerPhase? phase = new PlayerPhase?(PlayerPhase.Spectate);
				team2 = null;
				player.Server_SetGameState(phase, team2, null, null);
			}
		}
		else
		{
			PlayerTeam? team2 = new PlayerTeam?(team);
			player.Server_SetGameState(null, team2, null, null);
			if (base.CanPlayerEnterPhase(player, PlayerPhase.PositionSelect))
			{
				PlayerPhase? phase2 = new PlayerPhase?(PlayerPhase.PositionSelect);
				team2 = null;
				player.Server_SetGameState(phase2, team2, null, null);
				return;
			}
		}
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x0001B114 File Offset: 0x00019314
	protected override void OnVoteRemoved(Vote vote)
	{
		base.OnVoteRemoved(vote);
		if (!vote.Passed)
		{
			return;
		}
		string name = vote.Name;
		if (name == "start")
		{
			this.StartGame(GamePhase.PreGame);
			return;
		}
		if (!(name == "warmup"))
		{
			return;
		}
		this.StartGame(GamePhase.Warmup);
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x0001B162 File Offset: 0x00019362
	protected override List<ChatCommandInfo> GetChatCommands()
	{
		List<ChatCommandInfo> chatCommands = base.GetChatCommands();
		chatCommands.Add(new ChatCommandInfo("/vs, /votestart", "Start a vote to start the game", 0));
		chatCommands.Add(new ChatCommandInfo("/vw, /votewarmup", "Start a vote to return to warmup", 0));
		return chatCommands;
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x0001B198 File Offset: 0x00019398
	protected override void OnChatCommand(Player player, string command, string[] args)
	{
		if (!(command == "/vs") && !(command == "/votestart"))
		{
			if (!(command == "/vw") && !(command == "/votewarmup"))
			{
				base.OnChatCommand(player, command, args);
				return;
			}
			if (player.Team != PlayerTeam.Blue && player.Team != PlayerTeam.Red)
			{
				this.ChatManager.Server_SendChatMessage("You must be on a team to start this vote", "#e74c3c", new ulong[]
				{
					player.OwnerClientId
				});
				return;
			}
			Vote vote = this.VoteManager.Server_GetTeamVoteByName("warmup", player.Team);
			if (vote != null)
			{
				vote.CastVote(player.SteamId.Value.ToString(), true);
				return;
			}
			this.VoteManager.Server_AddVote("warmup", "Warmup", "use /vw or /votewarmup", new PlayerTeam[]
			{
				PlayerTeam.Blue,
				PlayerTeam.Red
			}, 30f, player.SteamId.Value.ToString(), Utils.GetVoteMajority(this.PlayerManager.GetPlayersByTeams(new PlayerTeam[]
			{
				PlayerTeam.Blue,
				PlayerTeam.Red
			}, false).Count), null);
			return;
		}
		else
		{
			if (player.Team != PlayerTeam.Blue && player.Team != PlayerTeam.Red)
			{
				this.ChatManager.Server_SendChatMessage("You must be on a team to start this vote", "#e74c3c", new ulong[]
				{
					player.OwnerClientId
				});
				return;
			}
			Vote vote2 = this.VoteManager.Server_GetTeamVoteByName("start", player.Team);
			if (vote2 != null)
			{
				vote2.CastVote(player.SteamId.Value.ToString(), true);
				return;
			}
			this.VoteManager.Server_AddVote("start", "Start", "use /vs or /votestart", new PlayerTeam[]
			{
				PlayerTeam.Blue,
				PlayerTeam.Red
			}, 30f, player.SteamId.Value.ToString(), Utils.GetVoteMajority(this.PlayerManager.GetPlayersByTeams(new PlayerTeam[]
			{
				PlayerTeam.Blue,
				PlayerTeam.Red
			}, false).Count), null);
			return;
		}
	}
}
