using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

// Token: 0x02000286 RID: 646
public class MatchData
{
	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06001391 RID: 5009 RVA: 0x00057010 File Offset: 0x00055210
	// (set) Token: 0x06001392 RID: 5010 RVA: 0x00057018 File Offset: 0x00055218
	public MatchPlayer[] homePlayers { get; set; }

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x06001393 RID: 5011 RVA: 0x00057021 File Offset: 0x00055221
	// (set) Token: 0x06001394 RID: 5012 RVA: 0x00057029 File Offset: 0x00055229
	public MatchPlayer[] awayPlayers { get; set; }

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x06001395 RID: 5013 RVA: 0x00057032 File Offset: 0x00055232
	// (set) Token: 0x06001396 RID: 5014 RVA: 0x0005703A File Offset: 0x0005523A
	public double? startedAt { get; set; }

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x06001397 RID: 5015 RVA: 0x00057043 File Offset: 0x00055243
	// (set) Token: 0x06001398 RID: 5016 RVA: 0x0005704B File Offset: 0x0005524B
	public EndPoint endPoint { get; set; }

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x06001399 RID: 5017 RVA: 0x00057054 File Offset: 0x00055254
	[JsonIgnore]
	public Dictionary<PlayerTeam, string[]> TeamAssignments
	{
		get
		{
			return new Dictionary<PlayerTeam, string[]>
			{
				{
					PlayerTeam.Blue,
					this.HomeSteamIds
				},
				{
					PlayerTeam.Red,
					this.AwaySteamIds
				}
			};
		}
	}

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x0600139A RID: 5018 RVA: 0x00057075 File Offset: 0x00055275
	[JsonIgnore]
	public MatchPlayer[] Players
	{
		get
		{
			return this.homePlayers.Concat(this.awayPlayers).ToArray<MatchPlayer>();
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x0600139B RID: 5019 RVA: 0x0005708D File Offset: 0x0005528D
	[JsonIgnore]
	public string[] SteamIds
	{
		get
		{
			return (from p in this.Players
			select p.steamId).ToArray<string>();
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x0600139C RID: 5020 RVA: 0x000570BE File Offset: 0x000552BE
	[JsonIgnore]
	public string[] HomeSteamIds
	{
		get
		{
			return (from p in this.homePlayers
			select p.steamId).ToArray<string>();
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x0600139D RID: 5021 RVA: 0x000570EF File Offset: 0x000552EF
	[JsonIgnore]
	public string[] AwaySteamIds
	{
		get
		{
			return (from p in this.awayPlayers
			select p.steamId).ToArray<string>();
		}
	}

	// Token: 0x0600139E RID: 5022 RVA: 0x00057120 File Offset: 0x00055320
	public MatchPlayer GetMatchPlayerBySteamId(string steamId)
	{
		return this.Players.FirstOrDefault((MatchPlayer p) => p.steamId == steamId);
	}

	// Token: 0x0600139F RID: 5023 RVA: 0x00057154 File Offset: 0x00055354
	public PlayerTeam? GetTeamBySteamId(string steamId)
	{
		if (this.HomeSteamIds.Contains(steamId))
		{
			return new PlayerTeam?(PlayerTeam.Blue);
		}
		if (this.AwaySteamIds.Contains(steamId))
		{
			return new PlayerTeam?(PlayerTeam.Red);
		}
		return null;
	}

	// Token: 0x060013A0 RID: 5024 RVA: 0x00057194 File Offset: 0x00055394
	public string[] GetTeamSteamIds(PlayerTeam team)
	{
		if (!this.TeamAssignments.ContainsKey(team))
		{
			return new string[0];
		}
		return this.TeamAssignments[team];
	}
}
