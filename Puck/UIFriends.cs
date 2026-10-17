using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x020001D0 RID: 464
public class UIFriends : UIView
{
	// Token: 0x06000E23 RID: 3619 RVA: 0x000434BC File Offset: 0x000416BC
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("FriendsView", null);
		this.friends = base.View.Query("Friends", null);
		this.friendsList = this.friends.Query("FriendsList", null);
		this.closeIconButton = this.friends.Query("CloseIconButtonContainer", null).First().Query(null, null);
		this.closeIconButton.clicked += this.OnCloseIconButtonClicked;
		this.friendsList.Clear();
		this.friendsMap.Clear();
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x00043570 File Offset: 0x00041770
	public void AddFriend(string steamId, string username, Texture2D avatar)
	{
		if (this.friendsMap.ContainsKey(steamId))
		{
			return;
		}
		TemplateContainer templateContainer = this.CreateFriend(steamId, username, avatar);
		this.friendsList.Add(templateContainer);
		this.friendsMap.Add(steamId, templateContainer);
		this.SortFriends();
	}

	// Token: 0x06000E25 RID: 3621 RVA: 0x000435B8 File Offset: 0x000417B8
	public void UpdateFriend(string steamId, string username, Texture2D texture)
	{
		if (!this.friendsMap.ContainsKey(steamId))
		{
			return;
		}
		Friend friend = this.friendsMap[steamId].Query("Friend", null);
		friend.Texture = texture;
		friend.Username = username;
		friend.InviteButtonClicked = delegate()
		{
			this.OnFriendInviteButtonClicked(steamId);
		};
		this.SortFriends();
	}

	// Token: 0x06000E26 RID: 3622 RVA: 0x00043634 File Offset: 0x00041834
	public void RemoveFriend(string steamId)
	{
		if (!this.friendsMap.ContainsKey(steamId))
		{
			return;
		}
		TemplateContainer element = this.friendsMap[steamId];
		this.friendsList.Remove(element);
		this.friendsMap.Remove(steamId);
	}

	// Token: 0x06000E27 RID: 3623 RVA: 0x00043676 File Offset: 0x00041876
	public bool IsFriendListed(string steamId)
	{
		return this.friendsMap.ContainsKey(steamId);
	}

	// Token: 0x06000E28 RID: 3624 RVA: 0x00043684 File Offset: 0x00041884
	private TemplateContainer CreateFriend(string steamId, string username, Texture2D texture)
	{
		TemplateContainer templateContainer = this.friendAsset.Instantiate();
		Friend friend = templateContainer.Query("Friend", null);
		friend.Texture = texture;
		friend.Username = username;
		friend.InviteButtonClicked = delegate()
		{
			this.OnFriendInviteButtonClicked(steamId);
		};
		return templateContainer;
	}

	// Token: 0x06000E29 RID: 3625 RVA: 0x000436E0 File Offset: 0x000418E0
	private void SortFriends()
	{
		this.friendsList.Sort(delegate(VisualElement a, VisualElement b)
		{
			Friend friend = a.Query("Friend", null).First();
			Friend friend2 = b.Query("Friend", null).First();
			return string.Compare(friend.Username, friend2.Username);
		});
	}

	// Token: 0x06000E2A RID: 3626 RVA: 0x0004370C File Offset: 0x0004190C
	private void OnCloseIconButtonClicked()
	{
		EventManager.TriggerEvent("Event_OnFriendsClickClose", null);
	}

	// Token: 0x06000E2B RID: 3627 RVA: 0x00043719 File Offset: 0x00041919
	private void OnFriendInviteButtonClicked(string steamId)
	{
		EventManager.TriggerEvent("Event_OnFriendInviteButtonClicked", new Dictionary<string, object>
		{
			{
				"steamId",
				steamId
			}
		});
	}

	// Token: 0x0400090C RID: 2316
	[Header("References")]
	public VisualTreeAsset friendAsset;

	// Token: 0x0400090D RID: 2317
	private VisualElement friends;

	// Token: 0x0400090E RID: 2318
	private VisualElement friendsList;

	// Token: 0x0400090F RID: 2319
	private IconButton closeIconButton;

	// Token: 0x04000910 RID: 2320
	private Dictionary<string, TemplateContainer> friendsMap = new Dictionary<string, TemplateContainer>();
}
