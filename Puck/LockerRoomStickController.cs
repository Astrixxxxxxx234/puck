using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class LockerRoomStickController : MonoBehaviour
{
	// Token: 0x06000091 RID: 145 RVA: 0x00003AB0 File Offset: 0x00001CB0
	private void Awake()
	{
		this.lockerRoomStick = base.GetComponent<LockerRoomStick>();
		EventManager.AddEventListener("Event_OnTeamChanged", new Action<Dictionary<string, object>>(this.Event_OnTeamChanged));
		EventManager.AddEventListener("Event_OnRoleChanged", new Action<Dictionary<string, object>>(this.Event_OnRoleChanged));
		EventManager.AddEventListener("Event_OnAppearanceClickItem", new Action<Dictionary<string, object>>(this.Event_OnAppearanceClickItem));
		EventManager.AddEventListener("Event_OnAppearanceHide", new Action<Dictionary<string, object>>(this.Event_OnAppearanceHide));
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00003B21 File Offset: 0x00001D21
	private void Start()
	{
		this.ApplySettings();
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00003B2C File Offset: 0x00001D2C
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnTeamChanged", new Action<Dictionary<string, object>>(this.Event_OnTeamChanged));
		EventManager.RemoveEventListener("Event_OnRoleChanged", new Action<Dictionary<string, object>>(this.Event_OnRoleChanged));
		EventManager.RemoveEventListener("Event_OnAppearanceClickItem", new Action<Dictionary<string, object>>(this.Event_OnAppearanceClickItem));
		EventManager.RemoveEventListener("Event_OnAppearanceHide", new Action<Dictionary<string, object>>(this.Event_OnAppearanceHide));
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00003B94 File Offset: 0x00001D94
	private void ApplySettings()
	{
		int stickSkinID = SettingsManager.GetStickSkinID(SettingsManager.Team, SettingsManager.Role);
		int stickShaftTapeID = SettingsManager.GetStickShaftTapeID(SettingsManager.Team, SettingsManager.Role);
		int stickBladeTapeID = SettingsManager.GetStickBladeTapeID(SettingsManager.Team, SettingsManager.Role);
		this.lockerRoomStick.ShowRoleStick(SettingsManager.Role);
		this.lockerRoomStick.SetSkinID(stickSkinID, SettingsManager.Team, SettingsManager.Role);
		this.lockerRoomStick.SetShaftTapeID(stickShaftTapeID, SettingsManager.Role);
		this.lockerRoomStick.SetBladeTapeID(stickBladeTapeID, SettingsManager.Role);
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00003B21 File Offset: 0x00001D21
	private void Event_OnTeamChanged(Dictionary<string, object> message)
	{
		this.ApplySettings();
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00003B21 File Offset: 0x00001D21
	private void Event_OnRoleChanged(Dictionary<string, object> message)
	{
		this.ApplySettings();
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00003C1C File Offset: 0x00001E1C
	private void Event_OnAppearanceClickItem(Dictionary<string, object> message)
	{
		Item item = message["item"] as Item;
		AppearanceCategory appearanceCategory = (AppearanceCategory)message["category"];
		AppearanceSubcategory appearanceSubcategory = (AppearanceSubcategory)message["subcategory"];
		PlayerTeam team = (PlayerTeam)message["team"];
		PlayerRole role = (PlayerRole)message["role"];
		switch (appearanceSubcategory)
		{
		case AppearanceSubcategory.StickSkins:
			this.lockerRoomStick.SetSkinID(item.id, team, role);
			return;
		case AppearanceSubcategory.StickShaftTapes:
			this.lockerRoomStick.SetShaftTapeID(item.id, role);
			return;
		case AppearanceSubcategory.StickBladeTapes:
			this.lockerRoomStick.SetBladeTapeID(item.id, role);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00003B21 File Offset: 0x00001D21
	private void Event_OnAppearanceHide(Dictionary<string, object> message)
	{
		this.ApplySettings();
	}

	// Token: 0x0400004B RID: 75
	private LockerRoomStick lockerRoomStick;
}
