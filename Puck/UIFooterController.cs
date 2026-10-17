using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class UIFooterController : UIViewController<UIFooter>
{
	// Token: 0x06000E1C RID: 3612 RVA: 0x00043208 File Offset: 0x00041408
	public override void Awake()
	{
		base.Awake();
		this.uiFooter = base.GetComponent<UIFooter>();
		EventManager.AddEventListener("Event_OnSteamConnected", new Action<Dictionary<string, object>>(this.Event_OnSteamConnected));
		EventManager.AddEventListener("Event_OnPlayerDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerDataChanged));
		EventManager.AddEventListener("Event_OnPlayerPartyDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerPartyDataChanged));
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x0004326C File Offset: 0x0004146C
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnSteamConnected", new Action<Dictionary<string, object>>(this.Event_OnSteamConnected));
		EventManager.RemoveEventListener("Event_OnPlayerDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerDataChanged));
		EventManager.RemoveEventListener("Event_OnPlayerPartyDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerPartyDataChanged));
		base.OnDestroy();
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x000432C4 File Offset: 0x000414C4
	private void Event_OnSteamConnected(Dictionary<string, object> message)
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		this.uiFooter.SetCreatePartyButtonVisibility(true);
		string steamId = SteamIntegrationManager.GetSteamId();
		if (string.IsNullOrEmpty(steamId))
		{
			return;
		}
		this.localAvatar = SteamIntegrationManager.GetAvatar(steamId, AvatarSize.Medium);
		this.RefreshLocalUser();
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x00043308 File Offset: 0x00041508
	private void Event_OnPlayerDataChanged(Dictionary<string, object> message)
	{
		PlayerData playerData = (PlayerData)message["oldPlayerData"];
		PlayerData playerData2 = (PlayerData)message["newPlayerData"];
		if (playerData2 == null)
		{
			return;
		}
		this.uiFooter.SetMmr(playerData2.mmr);
		this.RefreshLocalUser();
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x00043354 File Offset: 0x00041554
	private void RefreshLocalUser()
	{
		string steamId = SteamIntegrationManager.GetSteamId();
		if (string.IsNullOrEmpty(steamId))
		{
			return;
		}
		PlayerData playerData = BackendManager.PlayerState.PlayerData;
		string username = (playerData != null && !string.IsNullOrEmpty(playerData.username)) ? playerData.username : SteamIntegrationManager.GetUsername(steamId);
		this.uiFooter.SetLocalUser(username, this.localAvatar);
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x000433AC File Offset: 0x000415AC
	private void Event_OnPlayerPartyDataChanged(Dictionary<string, object> message)
	{
		PlayerPartyData playerPartyData = (PlayerPartyData)message["oldPlayerPartyData"];
		PlayerPartyData playerPartyData2 = (PlayerPartyData)message["newPlayerPartyData"];
		if (playerPartyData2 == null)
		{
			this.uiFooter.ClearPartyUsers();
			this.uiFooter.SetCreatePartyButtonVisibility(true);
			this.uiFooter.SetInviteButtonVisibility(false);
			this.uiFooter.SetLeavePartyButtonVisibility(false);
			this.uiFooter.SetDisbandPartyButtonVisibility(false);
			return;
		}
		bool flag = BackendManager.PlayerState.PlayerData.steamId == playerPartyData2.ownerSteamId;
		this.uiFooter.ClearPartyUsers();
		foreach (string steamId in playerPartyData2.memberSteamIds)
		{
			string username = SteamIntegrationManager.GetUsername(steamId);
			Texture2D avatar = SteamIntegrationManager.GetAvatar(steamId, AvatarSize.Medium);
			this.uiFooter.AddPartyUser(steamId, username, avatar);
		}
		this.uiFooter.SetCreatePartyButtonVisibility(false);
		this.uiFooter.SetInviteButtonVisibility(true);
		this.uiFooter.SetLeavePartyButtonVisibility(!flag);
		this.uiFooter.SetDisbandPartyButtonVisibility(flag);
	}

	// Token: 0x0400090A RID: 2314
	private UIFooter uiFooter;

	// Token: 0x0400090B RID: 2315
	private Texture2D localAvatar;
}
