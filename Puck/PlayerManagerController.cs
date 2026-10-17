using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class PlayerManagerController : MonoBehaviour
{
	// Token: 0x06000889 RID: 2185 RVA: 0x0002A714 File Offset: 0x00028914
	private void Awake()
	{
		this.playerManager = base.GetComponent<PlayerManager>();
		EventManager.AddEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerDespawned));
		EventManager.AddEventListener("Event_Server_OnApprovedClientConnected", new Action<Dictionary<string, object>>(this.Event_Server_OnApprovedClientConnected));
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x000023BA File Offset: 0x000005BA
	private void Start()
	{
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x0002A770 File Offset: 0x00028970
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerDespawned));
		EventManager.RemoveEventListener("Event_Server_OnApprovedClientConnected", new Action<Dictionary<string, object>>(this.Event_Server_OnApprovedClientConnected));
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x0002A7C0 File Offset: 0x000289C0
	private void Event_Everyone_OnPlayerSpawned(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		this.playerManager.AddPlayer(player);
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0002A7EC File Offset: 0x000289EC
	private void Event_Everyone_OnPlayerDespawned(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		this.playerManager.RemovePlayer(player);
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x0002A818 File Offset: 0x00028A18
	private void Event_Server_OnApprovedClientConnected(Dictionary<string, object> message)
	{
		ulong clientId = (ulong)message["clientId"];
		ConnectionApproval connectionApproval = (ConnectionApproval)message["connectionApproval"];
		ConnectionData connectionData = connectionApproval.ConnectionData;
		PlayerData playerData = connectionApproval.PlayerData;
		PlayerGameState gameState = new PlayerGameState
		{
			Phase = PlayerPhase.TeamSelect,
			Team = PlayerTeam.None,
			Role = PlayerRole.None
		};
		PlayerCustomizationState playerCustomizationState = default(PlayerCustomizationState);
		playerCustomizationState.FlagID = ItemManager.ValidateEquippedItem(connectionData.FlagID, -1, (Item item) => item.IsFlag, playerData);
		playerCustomizationState.HeadgearIDBlueAttacker = ItemManager.ValidateEquippedItem(connectionData.HeadgearIDBlueAttacker, 513, (Item item) => item.IsHeadgear && item.IsAttackerItem, playerData);
		playerCustomizationState.HeadgearIDRedAttacker = ItemManager.ValidateEquippedItem(connectionData.HeadgearIDRedAttacker, 513, (Item item) => item.IsHeadgear && item.IsAttackerItem, playerData);
		playerCustomizationState.HeadgearIDBlueGoalie = ItemManager.ValidateEquippedItem(connectionData.HeadgearIDBlueGoalie, 527, (Item item) => item.IsHeadgear && item.IsGoalieItem, playerData);
		playerCustomizationState.HeadgearIDRedGoalie = ItemManager.ValidateEquippedItem(connectionData.HeadgearIDRedGoalie, 527, (Item item) => item.IsHeadgear && item.IsGoalieItem, playerData);
		playerCustomizationState.MustacheID = ItemManager.ValidateEquippedItem(connectionData.MustacheID, -1, (Item item) => item.IsMustache, playerData);
		playerCustomizationState.BeardID = ItemManager.ValidateEquippedItem(connectionData.BeardID, -1, (Item item) => item.IsBeard, playerData);
		playerCustomizationState.JerseyIDBlueAttacker = ItemManager.ValidateEquippedItem(connectionData.JerseyIDBlueAttacker, 2048, (Item item) => item.IsJersey && item.IsAttackerItem, playerData);
		playerCustomizationState.JerseyIDRedAttacker = ItemManager.ValidateEquippedItem(connectionData.JerseyIDRedAttacker, 2048, (Item item) => item.IsJersey && item.IsAttackerItem, playerData);
		playerCustomizationState.JerseyIDBlueGoalie = ItemManager.ValidateEquippedItem(connectionData.JerseyIDBlueGoalie, 2048, (Item item) => item.IsJersey && item.IsGoalieItem, playerData);
		playerCustomizationState.JerseyIDRedGoalie = ItemManager.ValidateEquippedItem(connectionData.JerseyIDRedGoalie, 2048, (Item item) => item.IsJersey && item.IsGoalieItem, playerData);
		playerCustomizationState.StickSkinIDBlueAttacker = ItemManager.ValidateEquippedItem(connectionData.StickSkinIDBlueAttacker, 2621, (Item item) => item.IsStickSkin && item.IsAttackerItem, playerData);
		playerCustomizationState.StickSkinIDRedAttacker = ItemManager.ValidateEquippedItem(connectionData.StickSkinIDRedAttacker, 2621, (Item item) => item.IsStickSkin && item.IsAttackerItem, playerData);
		playerCustomizationState.StickSkinIDBlueGoalie = ItemManager.ValidateEquippedItem(connectionData.StickSkinIDBlueGoalie, 2621, (Item item) => item.IsStickSkin && item.IsGoalieItem, playerData);
		playerCustomizationState.StickSkinIDRedGoalie = ItemManager.ValidateEquippedItem(connectionData.StickSkinIDRedGoalie, 2621, (Item item) => item.IsStickSkin && item.IsGoalieItem, playerData);
		playerCustomizationState.StickShaftTapeIDBlueAttacker = ItemManager.ValidateEquippedItem(connectionData.StickShaftTapeIDBlueAttacker, -1, (Item item) => item.IsStickShaftTape && item.IsAttackerItem, playerData);
		playerCustomizationState.StickShaftTapeIDRedAttacker = ItemManager.ValidateEquippedItem(connectionData.StickShaftTapeIDRedAttacker, -1, (Item item) => item.IsStickShaftTape && item.IsAttackerItem, playerData);
		playerCustomizationState.StickShaftTapeIDBlueGoalie = ItemManager.ValidateEquippedItem(connectionData.StickShaftTapeIDBlueGoalie, -1, (Item item) => item.IsStickShaftTape && item.IsGoalieItem, playerData);
		playerCustomizationState.StickShaftTapeIDRedGoalie = ItemManager.ValidateEquippedItem(connectionData.StickShaftTapeIDRedGoalie, -1, (Item item) => item.IsStickShaftTape && item.IsGoalieItem, playerData);
		playerCustomizationState.StickBladeTapeIDBlueAttacker = ItemManager.ValidateEquippedItem(connectionData.StickBladeTapeIDBlueAttacker, -1, (Item item) => item.IsStickBladeTape && item.IsAttackerItem, playerData);
		playerCustomizationState.StickBladeTapeIDRedAttacker = ItemManager.ValidateEquippedItem(connectionData.StickBladeTapeIDRedAttacker, -1, (Item item) => item.IsStickBladeTape && item.IsAttackerItem, playerData);
		playerCustomizationState.StickBladeTapeIDBlueGoalie = ItemManager.ValidateEquippedItem(connectionData.StickBladeTapeIDBlueGoalie, -1, (Item item) => item.IsStickBladeTape && item.IsGoalieItem, playerData);
		playerCustomizationState.StickBladeTapeIDRedGoalie = ItemManager.ValidateEquippedItem(connectionData.StickBladeTapeIDRedGoalie, -1, (Item item) => item.IsStickBladeTape && item.IsGoalieItem, playerData);
		PlayerCustomizationState customizationState = playerCustomizationState;
		bool isMuted = BackendUtils.GetActivePlayerDataMute(playerData) != null;
		this.playerManager.Server_SpawnPlayer(clientId, gameState, customizationState, connectionData.Handedness, playerData.steamId, playerData.username, playerData.number, playerData.patreonLevel, playerData.adminLevel, isMuted, false);
	}

	// Token: 0x0400054D RID: 1357
	private PlayerManager playerManager;
}
