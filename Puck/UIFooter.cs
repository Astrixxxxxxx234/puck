using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x020001CE RID: 462
public class UIFooter : UIView
{
	// Token: 0x06000E0B RID: 3595 RVA: 0x00042D64 File Offset: 0x00040F64
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("FooterView", null);
		this.footer = base.View.Query("Footer", null);
		this.left = this.footer.Query("Left", null);
		this.center = this.footer.Query("Center", null);
		this.right = this.footer.Query("Right", null);
		this.localUserContainer = this.left.Query("LocalUserContainer", null);
		this.mmr = this.left.Query("LocalUserMmr", null).First().Query(null, null);
		this.party = this.right.Query("Party", null);
		this.partyUsers = this.party.Query("Users", null);
		this.createPartyIconButtonInstance = this.party.Query("CreatePartyIconButtonContainer", null);
		this.createPartyIconButton = this.createPartyIconButtonInstance.Query(null, null);
		this.createPartyIconButton.clicked += this.OnClickCreateParty;
		this.inviteIconButtonInstance = this.party.Query("InviteIconButtonContainer", null);
		this.inviteIconButton = this.inviteIconButtonInstance.Query(null, null);
		this.inviteIconButton.clicked += this.OnClickInvite;
		this.leavePartyIconButtonInstance = this.party.Query("LeavePartyIconButtonContainer", null);
		this.leavePartyIconButton = this.leavePartyIconButtonInstance.Query(null, null);
		this.leavePartyIconButton.clicked += this.OnClickLeaveParty;
		this.disbandPartyIconButtonInstance = this.party.Query("DisbandPartyIconButtonContainer", null);
		this.disbandPartyIconButton = this.disbandPartyIconButtonInstance.Query(null, null);
		this.disbandPartyIconButton.clicked += this.OnClickDisbandParty;
		this.ClearLocalUser();
		this.ClearPartyUsers();
		this.SetCreatePartyButtonVisibility(false);
		this.SetInviteButtonVisibility(false);
		this.SetDisbandPartyButtonVisibility(false);
		this.SetLeavePartyButtonVisibility(false);
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x00042FCC File Offset: 0x000411CC
	public void SetLocalUser(string username, Texture2D avatar)
	{
		this.ClearLocalUser();
		TemplateContainer child = this.CreateUser(username, avatar, false, false);
		this.localUserContainer.Add(child);
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x00042FF6 File Offset: 0x000411F6
	public void ClearLocalUser()
	{
		this.localUserContainer.Clear();
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x00043003 File Offset: 0x00041203
	public void SetMmr(int value)
	{
		this.mmr.TargetValue = new int?(value);
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x00043018 File Offset: 0x00041218
	public void AddPartyUser(string steamId, string username, Texture2D texture)
	{
		if (this.partyUserMap.ContainsKey(steamId))
		{
			return;
		}
		TemplateContainer templateContainer = this.CreateUser(username, texture, true, true);
		this.partyUsers.Add(templateContainer);
		this.partyUsers.style.display = DisplayStyle.Flex;
		this.partyUserMap.Add(steamId, templateContainer);
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x00043070 File Offset: 0x00041270
	public void RemovePartyUser(string steamId)
	{
		if (!this.partyUserMap.ContainsKey(steamId))
		{
			return;
		}
		VisualElement element = this.partyUserMap[steamId];
		this.partyUsers.Remove(element);
		this.partyUsers.style.display = ((this.partyUsers.childCount > 0) ? DisplayStyle.Flex : DisplayStyle.None);
		this.partyUserMap.Remove(steamId);
	}

	// Token: 0x06000E11 RID: 3601 RVA: 0x000430D9 File Offset: 0x000412D9
	public void ClearPartyUsers()
	{
		this.partyUsers.Clear();
		this.partyUserMap.Clear();
		this.partyUsers.style.display = DisplayStyle.None;
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x00043107 File Offset: 0x00041307
	private TemplateContainer CreateUser(string username, Texture2D texture, bool small = false, bool hideUsername = false)
	{
		TemplateContainer templateContainer = this.userAsset.Instantiate();
		User user = templateContainer.Query(null, null);
		user.AvatarTexture = texture;
		user.Username = username;
		templateContainer.EnableInClassList("small", small);
		templateContainer.EnableInClassList("hideUsername", hideUsername);
		return templateContainer;
	}

	// Token: 0x06000E13 RID: 3603 RVA: 0x00043147 File Offset: 0x00041347
	public void SetCreatePartyButtonVisibility(bool show)
	{
		this.createPartyIconButtonInstance.style.display = (show ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06000E14 RID: 3604 RVA: 0x00043165 File Offset: 0x00041365
	public void SetInviteButtonVisibility(bool show)
	{
		this.inviteIconButtonInstance.style.display = (show ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06000E15 RID: 3605 RVA: 0x00043183 File Offset: 0x00041383
	public void SetLeavePartyButtonVisibility(bool show)
	{
		this.leavePartyIconButtonInstance.style.display = (show ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06000E16 RID: 3606 RVA: 0x000431A1 File Offset: 0x000413A1
	public void SetDisbandPartyButtonVisibility(bool show)
	{
		this.disbandPartyIconButtonInstance.style.display = (show ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06000E17 RID: 3607 RVA: 0x000431BF File Offset: 0x000413BF
	private void OnClickCreateParty()
	{
		EventManager.TriggerEvent("Event_OnFooterClickCreateParty", null);
	}

	// Token: 0x06000E18 RID: 3608 RVA: 0x000431CC File Offset: 0x000413CC
	private void OnClickInvite()
	{
		EventManager.TriggerEvent("Event_OnFooterClickInvite", null);
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x000431D9 File Offset: 0x000413D9
	private void OnClickLeaveParty()
	{
		EventManager.TriggerEvent("Event_OnFooterClickLeaveParty", null);
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x000431E6 File Offset: 0x000413E6
	private void OnClickDisbandParty()
	{
		EventManager.TriggerEvent("Event_OnFooterClickDisbandParty", null);
	}

	// Token: 0x040008F8 RID: 2296
	[Header("References")]
	public VisualTreeAsset userAsset;

	// Token: 0x040008F9 RID: 2297
	private VisualElement footer;

	// Token: 0x040008FA RID: 2298
	private VisualElement left;

	// Token: 0x040008FB RID: 2299
	private VisualElement center;

	// Token: 0x040008FC RID: 2300
	private VisualElement right;

	// Token: 0x040008FD RID: 2301
	private VisualElement localUserContainer;

	// Token: 0x040008FE RID: 2302
	private Mmr mmr;

	// Token: 0x040008FF RID: 2303
	private VisualElement party;

	// Token: 0x04000900 RID: 2304
	private VisualElement partyUsers;

	// Token: 0x04000901 RID: 2305
	private TemplateContainer createPartyIconButtonInstance;

	// Token: 0x04000902 RID: 2306
	private IconButton createPartyIconButton;

	// Token: 0x04000903 RID: 2307
	private TemplateContainer inviteIconButtonInstance;

	// Token: 0x04000904 RID: 2308
	private IconButton inviteIconButton;

	// Token: 0x04000905 RID: 2309
	private TemplateContainer leavePartyIconButtonInstance;

	// Token: 0x04000906 RID: 2310
	private IconButton leavePartyIconButton;

	// Token: 0x04000907 RID: 2311
	private TemplateContainer disbandPartyIconButtonInstance;

	// Token: 0x04000908 RID: 2312
	private IconButton disbandPartyIconButton;

	// Token: 0x04000909 RID: 2313
	private Dictionary<string, TemplateContainer> partyUserMap = new Dictionary<string, TemplateContainer>();
}
