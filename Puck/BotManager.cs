using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000028 RID: 40
public static class BotManager
{
	// Token: 0x060000DA RID: 218 RVA: 0x00004C3C File Offset: 0x00002E3C
	public static BotBrain CreateBrain(BotManager.BotBehavior behavior)
	{
		if (behavior == BotManager.BotBehavior.Rookie)
		{
			return new RookieBotBrain();
		}
		if (behavior != BotManager.BotBehavior.Destroy)
		{
			return new ChaseBotBrain();
		}
		return new DestroyBotBrain();
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00004C5C File Offset: 0x00002E5C
	public static void Server_SetBotBehavior(BotManager.BotBehavior behavior)
	{
		BotManager.DefaultBehavior = behavior;
		BotInputDriver[] array = Object.FindObjectsByType<BotInputDriver>(FindObjectsSortMode.None);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetBrain(BotManager.CreateBrain(behavior));
		}
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00004C94 File Offset: 0x00002E94
	public static int Server_SpawnBots(int count, PlayerTeam team = PlayerTeam.None)
	{
		PlayerManager instance = MonoBehaviourSingleton<PlayerManager>.Instance;
		if (!instance)
		{
			return 0;
		}
		List<PlayerPosition> list = (from position in Object.FindObjectsByType<PlayerPosition>(FindObjectsSortMode.None)
		where !position.IsClaimed && (team == PlayerTeam.None || position.Team == team)
		select position).OrderBy(delegate(PlayerPosition position)
		{
			if (position.Role != PlayerRole.Goalie)
			{
				return 0;
			}
			return 1;
		}).ToList<PlayerPosition>();
		int num = 0;
		foreach (PlayerPosition playerPosition in list)
		{
			if (num >= count)
			{
				break;
			}
			ulong num2 = BotManager.nextBotClientId;
			BotManager.nextBotClientId = num2 + 1UL;
			ulong num3 = num2;
			int num4 = (int)(num3 - 100000UL + 1UL);
			instance.Server_SpawnPlayer(num3, new PlayerGameState
			{
				Phase = PlayerPhase.TeamSelect,
				Team = PlayerTeam.None,
				Role = PlayerRole.None
			}, BotManager.CreateRandomCustomizationState(), PlayerHandedness.Right, string.Format("BOT{0}", num3), BotManager.GetRandomBotUsername(instance, num4), num4 % 100, 0, 0, false, false);
			Player playerByClientId = instance.GetPlayerByClientId(num3);
			if (playerByClientId)
			{
				playerByClientId.gameObject.AddComponent<BotInputDriver>();
				Player player = playerByClientId;
				PlayerTeam? team2 = new PlayerTeam?(playerPosition.Team);
				player.Server_SetGameState(null, team2, null, null);
				playerPosition.Server_Claim(playerByClientId);
				num++;
			}
		}
		return num;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00004E20 File Offset: 0x00003020
	private static string GetRandomBotUsername(PlayerManager playerManager, int botNumber)
	{
		int num = Random.Range(0, BotManager.BotNames.Length);
		for (int i = 0; i < BotManager.BotNames.Length; i++)
		{
			string text = "Bot" + BotManager.BotNames[(num + i) % BotManager.BotNames.Length];
			if (!playerManager.GetPlayerByUsername(text, false))
			{
				return text;
			}
		}
		return string.Format("Bot{0}", botNumber);
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00004E90 File Offset: 0x00003090
	private static int GetRandomItemId(string category, PlayerRole role, float noneChance = 0f)
	{
		if (noneChance > 0f && Random.value < noneChance)
		{
			return -1;
		}
		List<Item> list = ItemManager.GetItemsByCategories(new string[]
		{
			category
		}).FindAll(delegate(Item item)
		{
			if (item.IsUnlisted)
			{
				return false;
			}
			if (role != PlayerRole.Goalie)
			{
				return item.IsAttackerItem;
			}
			return item.IsGoalieItem;
		});
		if (list.Count == 0)
		{
			return -1;
		}
		return list[Random.Range(0, list.Count)].id;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00004F00 File Offset: 0x00003100
	private static PlayerCustomizationState CreateRandomCustomizationState()
	{
		return new PlayerCustomizationState
		{
			FlagID = BotManager.GetRandomItemId("flag", PlayerRole.Attacker, 0f),
			HeadgearIDBlueAttacker = BotManager.GetRandomItemId("headgear", PlayerRole.Attacker, 0f),
			HeadgearIDRedAttacker = BotManager.GetRandomItemId("headgear", PlayerRole.Attacker, 0f),
			HeadgearIDBlueGoalie = BotManager.GetRandomItemId("headgear", PlayerRole.Goalie, 0f),
			HeadgearIDRedGoalie = BotManager.GetRandomItemId("headgear", PlayerRole.Goalie, 0f),
			MustacheID = BotManager.GetRandomItemId("mustache", PlayerRole.Attacker, 0.5f),
			BeardID = BotManager.GetRandomItemId("beard", PlayerRole.Attacker, 0.5f),
			JerseyIDBlueAttacker = BotManager.GetRandomItemId("jersey", PlayerRole.Attacker, 0f),
			JerseyIDRedAttacker = BotManager.GetRandomItemId("jersey", PlayerRole.Attacker, 0f),
			JerseyIDBlueGoalie = BotManager.GetRandomItemId("jersey", PlayerRole.Goalie, 0f),
			JerseyIDRedGoalie = BotManager.GetRandomItemId("jersey", PlayerRole.Goalie, 0f),
			StickSkinIDBlueAttacker = BotManager.GetRandomItemId("stickSkin", PlayerRole.Attacker, 0f),
			StickSkinIDRedAttacker = BotManager.GetRandomItemId("stickSkin", PlayerRole.Attacker, 0f),
			StickSkinIDBlueGoalie = BotManager.GetRandomItemId("stickSkin", PlayerRole.Goalie, 0f),
			StickSkinIDRedGoalie = BotManager.GetRandomItemId("stickSkin", PlayerRole.Goalie, 0f),
			StickShaftTapeIDBlueAttacker = BotManager.GetRandomItemId("stickShaftTape", PlayerRole.Attacker, 0.3f),
			StickShaftTapeIDRedAttacker = BotManager.GetRandomItemId("stickShaftTape", PlayerRole.Attacker, 0.3f),
			StickShaftTapeIDBlueGoalie = BotManager.GetRandomItemId("stickShaftTape", PlayerRole.Goalie, 0.3f),
			StickShaftTapeIDRedGoalie = BotManager.GetRandomItemId("stickShaftTape", PlayerRole.Goalie, 0.3f),
			StickBladeTapeIDBlueAttacker = BotManager.GetRandomItemId("stickBladeTape", PlayerRole.Attacker, 0.3f),
			StickBladeTapeIDRedAttacker = BotManager.GetRandomItemId("stickBladeTape", PlayerRole.Attacker, 0.3f),
			StickBladeTapeIDBlueGoalie = BotManager.GetRandomItemId("stickBladeTape", PlayerRole.Goalie, 0.3f),
			StickBladeTapeIDRedGoalie = BotManager.GetRandomItemId("stickBladeTape", PlayerRole.Goalie, 0.3f)
		};
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00005128 File Offset: 0x00003328
	public static int Server_DespawnBots()
	{
		PlayerManager instance = MonoBehaviourSingleton<PlayerManager>.Instance;
		if (!instance)
		{
			return 0;
		}
		List<Player> list = (from botPlayer in instance.GetPlayers(false)
		where botPlayer.OwnerClientId >= 100000UL
		select botPlayer).ToList<Player>();
		foreach (Player player in list)
		{
			ulong ownerClientId = player.OwnerClientId;
			if (player.PlayerPosition != null)
			{
				player.PlayerPosition.Server_Unclaim();
			}
			if (player.IsCharacterSpawned)
			{
				player.Server_DespawnCharacter();
			}
			player.NetworkObject.Despawn(true);
			if (NetworkManager.Singleton.ConnectedClients.ContainsKey(ownerClientId))
			{
				NetworkManager.Singleton.DisconnectClient(ownerClientId);
			}
		}
		return list.Count;
	}

	// Token: 0x04000076 RID: 118
	public static BotManager.BotBehavior DefaultBehavior = BotManager.BotBehavior.Chase;

	// Token: 0x04000077 RID: 119
	public const ulong BotClientIdBase = 100000UL;

	// Token: 0x04000078 RID: 120
	private static ulong nextBotClientId = 100000UL;

	// Token: 0x04000079 RID: 121
	private static readonly string[] BotNames = new string[]
	{
		"Sauce",
		"Dangles",
		"Wheels",
		"Mitts",
		"Biscuit",
		"Tendy",
		"Pylon",
		"Duster",
		"Grinder",
		"Sniper",
		"Celly",
		"Flow",
		"Chirp",
		"Hoser",
		"Clapper",
		"Stretch",
		"Bardown",
		"Chiclets",
		"Lettuce",
		"Twig",
		"Bucket",
		"Snipes",
		"Apple",
		"Ferda",
		"Wheelhouse",
		"Breakaway",
		"Deke",
		"Saucer",
		"Slapshot",
		"Crease",
		"Chel",
		"Gongshow",
		"Beauty"
	};

	// Token: 0x02000029 RID: 41
	public enum BotBehavior
	{
		// Token: 0x0400007B RID: 123
		Chase,
		// Token: 0x0400007C RID: 124
		Rookie,
		// Token: 0x0400007D RID: 125
		Destroy
	}
}
