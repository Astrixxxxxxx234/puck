using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class LockerRoomPlayerController : MonoBehaviour
{
	// Token: 0x0600007F RID: 127 RVA: 0x0000364C File Offset: 0x0000184C
	private void Awake()
	{
		this.lockerRoomPlayer = base.GetComponent<LockerRoomPlayer>();
		EventManager.AddEventListener("Event_OnTeamChanged", new Action<Dictionary<string, object>>(this.Event_OnTeamChanged));
		EventManager.AddEventListener("Event_OnRoleChanged", new Action<Dictionary<string, object>>(this.Event_OnRoleChanged));
		EventManager.AddEventListener("Event_OnAppearanceClickItem", new Action<Dictionary<string, object>>(this.Event_OnAppearanceClickItem));
		EventManager.AddEventListener("Event_OnAppearanceShow", new Action<Dictionary<string, object>>(this.Event_OnAppearanceShow));
		EventManager.AddEventListener("Event_OnAppearanceHide", new Action<Dictionary<string, object>>(this.Event_OnAppearanceHide));
		EventManager.AddEventListener("Event_OnIdentityShow", new Action<Dictionary<string, object>>(this.Event_OnIdentityShow));
		EventManager.AddEventListener("Event_OnIdentityHide", new Action<Dictionary<string, object>>(this.Event_OnIdentityHide));
		EventManager.AddEventListener("Event_OnPlayerDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerDataChanged));
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00003715 File Offset: 0x00001915
	private void Start()
	{
		this.ApplySettings();
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00003720 File Offset: 0x00001920
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnTeamChanged", new Action<Dictionary<string, object>>(this.Event_OnTeamChanged));
		EventManager.RemoveEventListener("Event_OnRoleChanged", new Action<Dictionary<string, object>>(this.Event_OnRoleChanged));
		EventManager.RemoveEventListener("Event_OnAppearanceClickItem", new Action<Dictionary<string, object>>(this.Event_OnAppearanceClickItem));
		EventManager.RemoveEventListener("Event_OnAppearanceShow", new Action<Dictionary<string, object>>(this.Event_OnAppearanceShow));
		EventManager.RemoveEventListener("Event_OnAppearanceHide", new Action<Dictionary<string, object>>(this.Event_OnAppearanceHide));
		EventManager.RemoveEventListener("Event_OnIdentityShow", new Action<Dictionary<string, object>>(this.Event_OnIdentityShow));
		EventManager.RemoveEventListener("Event_OnIdentityHide", new Action<Dictionary<string, object>>(this.Event_OnIdentityHide));
		EventManager.RemoveEventListener("Event_OnPlayerDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerDataChanged));
	}

	// Token: 0x06000082 RID: 130 RVA: 0x000037E0 File Offset: 0x000019E0
	private void ApplySettings()
	{
		int flagID = SettingsManager.FlagID;
		int headgearID = SettingsManager.GetHeadgearID(SettingsManager.Team, SettingsManager.Role);
		int mustacheID = SettingsManager.MustacheID;
		int beardID = SettingsManager.BeardID;
		int jerseyID = SettingsManager.GetJerseyID(SettingsManager.Team, SettingsManager.Role);
		if (BackendManager.PlayerState.PlayerData != null)
		{
			this.lockerRoomPlayer.SetUsername(BackendManager.PlayerState.PlayerData.username);
			this.lockerRoomPlayer.SetNumber(BackendManager.PlayerState.PlayerData.number.ToString());
		}
		this.lockerRoomPlayer.SetLegsPadsActive(SettingsManager.Role == PlayerRole.Goalie);
		this.lockerRoomPlayer.SetFlagID(flagID);
		this.lockerRoomPlayer.SetHeadgearID(headgearID, SettingsManager.Role);
		this.lockerRoomPlayer.SetMustacheID(mustacheID);
		this.lockerRoomPlayer.SetBeardID(beardID);
		this.lockerRoomPlayer.SetJerseyID(jerseyID, SettingsManager.Team);
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00003715 File Offset: 0x00001915
	private void Event_OnTeamChanged(Dictionary<string, object> message)
	{
		this.ApplySettings();
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00003715 File Offset: 0x00001915
	private void Event_OnRoleChanged(Dictionary<string, object> message)
	{
		this.ApplySettings();
	}

	// Token: 0x06000085 RID: 133 RVA: 0x000038C4 File Offset: 0x00001AC4
	private void Event_OnAppearanceClickItem(Dictionary<string, object> message)
	{
		Item item = message["item"] as Item;
		AppearanceCategory appearanceCategory = (AppearanceCategory)message["category"];
		AppearanceSubcategory appearanceSubcategory = (AppearanceSubcategory)message["subcategory"];
		PlayerTeam team = (PlayerTeam)message["team"];
		PlayerRole role = (PlayerRole)message["role"];
		switch (appearanceSubcategory)
		{
		case AppearanceSubcategory.Headgear:
			this.lockerRoomPlayer.SetHeadgearID(item.id, role);
			return;
		case AppearanceSubcategory.Flags:
			this.lockerRoomPlayer.SetFlagID(item.id);
			return;
		case AppearanceSubcategory.Mustaches:
			this.lockerRoomPlayer.SetMustacheID(item.id);
			return;
		case AppearanceSubcategory.Beards:
			this.lockerRoomPlayer.SetBeardID(item.id);
			return;
		case AppearanceSubcategory.Jerseys:
			this.lockerRoomPlayer.SetJerseyID(item.id, team);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000086 RID: 134 RVA: 0x0000399C File Offset: 0x00001B9C
	private void Event_OnAppearanceShow(Dictionary<string, object> message)
	{
		this.lockerRoomPlayer.AllowRotation = true;
		this.lockerRoomPlayer.SetRotationFromPreset("front");
	}

	// Token: 0x06000087 RID: 135 RVA: 0x000039BA File Offset: 0x00001BBA
	private void Event_OnAppearanceHide(Dictionary<string, object> message)
	{
		this.ApplySettings();
		this.lockerRoomPlayer.AllowRotation = false;
		this.lockerRoomPlayer.SetRotationFromPreset("front");
	}

	// Token: 0x06000088 RID: 136 RVA: 0x000039DE File Offset: 0x00001BDE
	private void Event_OnIdentityShow(Dictionary<string, object> message)
	{
		this.lockerRoomPlayer.AllowRotation = true;
		this.lockerRoomPlayer.SetRotationFromPreset("back");
	}

	// Token: 0x06000089 RID: 137 RVA: 0x000039FC File Offset: 0x00001BFC
	private void Event_OnIdentityHide(Dictionary<string, object> message)
	{
		this.lockerRoomPlayer.AllowRotation = false;
		this.lockerRoomPlayer.SetRotationFromPreset("front");
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00003A1A File Offset: 0x00001C1A
	private void Event_OnPlayerDataChanged(Dictionary<string, object> message)
	{
		if ((PlayerData)message["newPlayerData"] == null)
		{
			return;
		}
		this.ApplySettings();
	}

	// Token: 0x04000048 RID: 72
	private LockerRoomPlayer lockerRoomPlayer;
}
