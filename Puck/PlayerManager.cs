using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class PlayerManager : MonoBehaviourSingleton<PlayerManager>
{
	// Token: 0x0600085B RID: 2139 RVA: 0x00029FA0 File Offset: 0x000281A0
	public void AddPlayer(Player player)
	{
		this.players.Add(player);
		if (!player.IsReplay.Value)
		{
			this.nonReplayPlayers.Add(player);
		}
		EventManager.TriggerEvent("Event_Everyone_OnPlayerAdded", new Dictionary<string, object>
		{
			{
				"player",
				player
			}
		});
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00029FF0 File Offset: 0x000281F0
	public void RemovePlayer(Player player)
	{
		this.players.Remove(player);
		this.nonReplayPlayers.Remove(player);
		if (this.localPlayer == player)
		{
			this.localPlayer = null;
		}
		EventManager.TriggerEvent("Event_Everyone_OnPlayerRemoved", new Dictionary<string, object>
		{
			{
				"player",
				player
			}
		});
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x0002A047 File Offset: 0x00028247
	public List<Player> GetPlayers(bool includeReplay = false)
	{
		this.RemoveStalePlayers();
		if (includeReplay)
		{
			return this.players;
		}
		return this.nonReplayPlayers;
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x0002A060 File Offset: 0x00028260
	private void RemoveStalePlayers()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			Player player = this.players[i];
			if (!player || !player.NetworkObject.IsSpawned)
			{
				this.RebuildPlayerLists();
				return;
			}
		}
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x0002A0AC File Offset: 0x000282AC
	private void RebuildPlayerLists()
	{
		List<Player> list = new List<Player>(this.players.Count);
		List<Player> list2 = new List<Player>(this.nonReplayPlayers.Count);
		foreach (Player player in this.players)
		{
			if (player && player.NetworkObject.IsSpawned)
			{
				list.Add(player);
				if (!player.IsReplay.Value)
				{
					list2.Add(player);
				}
			}
		}
		this.players = list;
		this.nonReplayPlayers = list2;
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x0002A158 File Offset: 0x00028358
	public List<Player> GetPlayersByPhase(PlayerPhase phase, bool includeReplay = false)
	{
		return (from player in this.GetPlayers(includeReplay)
		where player.Phase == phase
		select player).ToList<Player>();
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x0002A190 File Offset: 0x00028390
	public List<Player> GetPlayersByPhases(PlayerPhase[] phases, bool includeReplay = false)
	{
		return (from player in this.GetPlayers(includeReplay)
		where phases.Contains(player.Phase)
		select player).ToList<Player>();
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x0002A1C8 File Offset: 0x000283C8
	public List<Player> GetPlayersByTeam(PlayerTeam team, bool includeReplay = false)
	{
		return (from player in this.GetPlayers(includeReplay)
		where player.Team == team
		select player).ToList<Player>();
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x0002A200 File Offset: 0x00028400
	public List<Player> GetPlayersByTeams(PlayerTeam[] team, bool includeReplay = false)
	{
		return (from player in this.GetPlayers(includeReplay)
		where team.Contains(player.Team)
		select player).ToList<Player>();
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x0002A238 File Offset: 0x00028438
	public Player GetPlayerByClientId(ulong clientId)
	{
		return this.GetPlayers(false).Find((Player player) => player.OwnerClientId == clientId);
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x0002A26C File Offset: 0x0002846C
	public Player GetPlayerByUsername(FixedString32Bytes username, bool caseSensitive = false)
	{
		return this.GetPlayers(false).Find((Player player) => (caseSensitive ? player.Username.Value.ToString() : player.Username.Value.ToString().ToLower()) == (caseSensitive ? username.ToString() : username.ToString().ToLower()));
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x0002A2A8 File Offset: 0x000284A8
	public Player GetPlayerByNumber(int number)
	{
		return this.GetPlayers(false).Find((Player player) => player.Number.Value == number);
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0002A2DC File Offset: 0x000284DC
	public Player GetPlayerByNeedle(string needle, bool caseSensitive = true)
	{
		Player player = this.GetPlayerByUsername(needle, caseSensitive);
		int number;
		if (!player && int.TryParse(needle, out number))
		{
			player = this.GetPlayerByNumber(number);
		}
		if (!player && Utils.IsValidSteamId64(needle))
		{
			player = this.GetPlayerBySteamId(needle);
		}
		return player;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x0002A330 File Offset: 0x00028530
	public Player GetPlayerBySteamId(FixedString32Bytes steamId)
	{
		return this.GetPlayers(false).Find(delegate(Player player)
		{
			FixedString32Bytes value = player.SteamId.Value;
			return value == steamId;
		});
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x0002A362 File Offset: 0x00028562
	public List<Player> GetReplayPlayers()
	{
		return (from player in this.GetPlayers(true)
		where player.IsReplay.Value
		select player).ToList<Player>();
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x0002A394 File Offset: 0x00028594
	public Player GetReplayPlayerByClientId(ulong clientId)
	{
		return this.GetReplayPlayers().Find((Player player) => player.OwnerClientId == clientId + 1337UL);
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x0002A3C8 File Offset: 0x000285C8
	public Player GetLocalPlayer()
	{
		if (this.localPlayer && this.localPlayer.NetworkObject.IsSpawned)
		{
			return this.localPlayer;
		}
		this.localPlayer = null;
		foreach (Player player in this.GetPlayers(false))
		{
			if (player.IsLocalPlayer)
			{
				this.localPlayer = player;
				break;
			}
		}
		return this.localPlayer;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x0002A45C File Offset: 0x0002865C
	public List<Player> GetSpawnedPlayers(bool includeReplay = false)
	{
		return this.GetPlayers(includeReplay).FindAll((Player player) => player.IsCharacterSpawned);
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x0002A48C File Offset: 0x0002868C
	public List<Player> GetSpawnedPlayersByTeam(PlayerTeam team, bool includeReplay = false)
	{
		return (from player in this.GetSpawnedPlayers(includeReplay)
		where player.Team == team
		select player).ToList<Player>();
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x0002A4C4 File Offset: 0x000286C4
	public void Server_SpawnPlayer(ulong clientId, PlayerGameState gameState, PlayerCustomizationState customizationState, PlayerHandedness handedness, string steamID, string username, int number, int patreonLevel, int adminLevel, bool isMuted = false, bool isReplay = false)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player player = Object.Instantiate<Player>(this.playerPrefab);
		Player player2 = player;
		FixedString32Bytes steamID2 = steamID;
		FixedString32Bytes username2 = username;
		player2.InitializeNetworkVariables(gameState, customizationState, handedness, steamID2, username2, number, patreonLevel, adminLevel, 0, 0, 0UL, default(NetworkObjectReference), isMuted, isReplay);
		if (isReplay)
		{
			player.NetworkObject.SpawnWithOwnership(1337UL + clientId, false);
			PlayerManager.Logger.Info(string.Format("Spawned replay player ({0})", clientId));
			return;
		}
		player.NetworkObject.SpawnAsPlayerObject(clientId, false);
		PlayerManager.Logger.Info(string.Format("Spawned player ({0})", clientId));
	}

	// Token: 0x0400053A RID: 1338
	private static readonly global::Logger Logger = new global::Logger("PlayerManager");

	// Token: 0x0400053B RID: 1339
	[Header("Prefabs")]
	[SerializeField]
	private Player playerPrefab;

	// Token: 0x0400053C RID: 1340
	private List<Player> players = new List<Player>();

	// Token: 0x0400053D RID: 1341
	private List<Player> nonReplayPlayers = new List<Player>();

	// Token: 0x0400053E RID: 1342
	private Player localPlayer;
}
