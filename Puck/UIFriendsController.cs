using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

// Token: 0x020001D4 RID: 468
public class UIFriendsController : UIViewController<UIFriends>
{
	// Token: 0x06000E34 RID: 3636 RVA: 0x000437C4 File Offset: 0x000419C4
	public override void Awake()
	{
		base.Awake();
		this.uiFriends = base.GetComponent<UIFriends>();
		EventManager.AddEventListener("Event_OnSteamConnected", new Action<Dictionary<string, object>>(this.Event_OnSteamConnected));
		EventManager.AddEventListener("Event_OnPersonaStateChange", new Action<Dictionary<string, object>>(this.Event_OnPersonaStateChange));
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x00043804 File Offset: 0x00041A04
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnSteamConnected", new Action<Dictionary<string, object>>(this.Event_OnSteamConnected));
		EventManager.RemoveEventListener("Event_OnPersonaStateChange", new Action<Dictionary<string, object>>(this.Event_OnPersonaStateChange));
		base.OnDestroy();
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x00043838 File Offset: 0x00041A38
	private void ParseSteamId(string steamId)
	{
		bool flag = this.uiFriends.IsFriendListed(steamId);
		bool flag2 = SteamIntegrationManager.IsFriend(steamId);
		bool flag3 = SteamIntegrationManager.IsFriendOnline(steamId);
		bool flag4 = !flag && flag2 && flag3;
		bool flag5 = flag && flag2 && flag3;
		bool flag6 = flag && (!flag2 || !flag3);
		if (flag4)
		{
			string username = SteamIntegrationManager.GetUsername(steamId);
			Texture2D avatar = SteamIntegrationManager.GetAvatar(steamId, AvatarSize.Medium);
			this.uiFriends.AddFriend(steamId, username, avatar);
			return;
		}
		if (flag5)
		{
			string username2 = SteamIntegrationManager.GetUsername(steamId);
			Texture2D avatar2 = SteamIntegrationManager.GetAvatar(steamId, AvatarSize.Medium);
			this.uiFriends.UpdateFriend(steamId, username2, avatar2);
			return;
		}
		if (flag6)
		{
			this.uiFriends.RemoveFriend(steamId);
		}
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x000438D9 File Offset: 0x00041AD9
	private void Event_OnSteamConnected(Dictionary<string, object> message)
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		SteamIntegrationManager.GetFriendSteamIds(false).ForEach(delegate(string steamId)
		{
			this.ParseSteamId(steamId);
		});
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x000438FC File Offset: 0x00041AFC
	private void Event_OnPersonaStateChange(Dictionary<string, object> message)
	{
		string steamId = (string)message["steamId"];
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		this.ParseSteamId(steamId);
	}

	// Token: 0x04000917 RID: 2327
	private UIFriends uiFriends;
}
