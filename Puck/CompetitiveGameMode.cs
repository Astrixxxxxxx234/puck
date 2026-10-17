using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000093 RID: 147
public class CompetitiveGameMode<TConfig> : MatchableGameMode<TConfig> where TConfig : CompetitiveGameModeConfig, new()
{
	// Token: 0x1700007A RID: 122
	// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0001A0CA File Offset: 0x000182CA
	protected override bool AllowVotekick
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x0001A0CD File Offset: 0x000182CD
	public CompetitiveGameMode(string defaultConfigFilePath, string configFilePathCliArgument = null, string configCliArgument = null, string configEnvVariable = null) : base(defaultConfigFilePath, configFilePathCliArgument, configCliArgument, configEnvVariable)
	{
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x0001A0E8 File Offset: 0x000182E8
	private PlayerTeam GetAssignedPlayerTeam(Player player)
	{
		foreach (PlayerTeam playerTeam in this.teamAssignments.Keys)
		{
			if (this.teamAssignments[playerTeam].Contains(player.SteamId.Value.ToString()))
			{
				return playerTeam;
			}
		}
		return PlayerTeam.None;
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x0001A16C File Offset: 0x0001836C
	protected override void OnConfigLoaded()
	{
		base.OnConfigLoaded();
		this.teamAssignments = base.Config.teamAssignments;
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x0001A18C File Offset: 0x0001838C
	protected override void OnPlayerJoined(Player player)
	{
		base.OnPlayerJoined(player);
		PlayerTeam assignedPlayerTeam = this.GetAssignedPlayerTeam(player);
		switch (assignedPlayerTeam)
		{
		case PlayerTeam.None:
		case PlayerTeam.Spectator:
		{
			PlayerTeam? team = new PlayerTeam?(PlayerTeam.Spectator);
			player.Server_SetGameState(null, team, null, null);
			if (base.CanPlayerEnterPhase(player, PlayerPhase.Spectate))
			{
				PlayerPhase? phase = new PlayerPhase?(PlayerPhase.Spectate);
				team = null;
				player.Server_SetGameState(phase, team, null, null);
			}
			break;
		}
		case PlayerTeam.Blue:
		case PlayerTeam.Red:
		{
			PlayerTeam? team = new PlayerTeam?(assignedPlayerTeam);
			player.Server_SetGameState(null, team, null, null);
			if (base.CanPlayerEnterPhase(player, PlayerPhase.PositionSelect))
			{
				PlayerPhase? phase2 = new PlayerPhase?(PlayerPhase.PositionSelect);
				team = null;
				player.Server_SetGameState(phase2, team, null, null);
				return;
			}
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0001A27D File Offset: 0x0001847D
	protected override void OnMatchStarted()
	{
		base.OnMatchStarted();
		this.teamAssignments = new Dictionary<PlayerTeam, string[]>
		{
			{
				PlayerTeam.Blue,
				base.matchData.HomeSteamIds
			},
			{
				PlayerTeam.Red,
				base.matchData.AwaySteamIds
			}
		};
	}

	// Token: 0x04000353 RID: 851
	private Dictionary<PlayerTeam, string[]> teamAssignments = new Dictionary<PlayerTeam, string[]>();
}
