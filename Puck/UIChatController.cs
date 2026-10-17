using System;
using System.Collections.Generic;

// Token: 0x020001CA RID: 458
public class UIChatController : UIViewController<UIChat>
{
	// Token: 0x06000DC1 RID: 3521 RVA: 0x0004194C File Offset: 0x0003FB4C
	public override void Awake()
	{
		base.Awake();
		this.uiChat = base.GetComponent<UIChat>();
		EventManager.AddEventListener("Event_OnChatMessageAdded", new Action<Dictionary<string, object>>(this.Event_OnChatMessageAdded));
		EventManager.AddEventListener("Event_OnChatMessageRemoved", new Action<Dictionary<string, object>>(this.Event_OnChatMessageRemoved));
		EventManager.AddEventListener("Event_OnChatMessagesCleared", new Action<Dictionary<string, object>>(this.Event_OnChatMessagesCleared));
		EventManager.AddEventListener("Event_OnQuickChatEnabled", new Action<Dictionary<string, object>>(this.Event_OnQuickChatEnabled));
		EventManager.AddEventListener("Event_OnQuickChatDisabled", new Action<Dictionary<string, object>>(this.Event_OnQuickChatDisabled));
		EventManager.AddEventListener("Event_OnShowGameUserInterfaceChanged", new Action<Dictionary<string, object>>(this.Event_OnShowGameUserInterfaceChanged));
		EventManager.AddEventListener("Event_OnChatOpacityChanged", new Action<Dictionary<string, object>>(this.Event_OnChatOpacityChanged));
		EventManager.AddEventListener("Event_OnChatScaleChanged", new Action<Dictionary<string, object>>(this.Event_OnChatScaleChanged));
		EventManager.AddEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
	}

	// Token: 0x06000DC2 RID: 3522 RVA: 0x00041A31 File Offset: 0x0003FC31
	private void Start()
	{
		this.uiChat.SetOpacity(SettingsManager.ChatOpacity);
		this.uiChat.SetScale(SettingsManager.ChatScale);
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x00041A54 File Offset: 0x0003FC54
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnChatMessageAdded", new Action<Dictionary<string, object>>(this.Event_OnChatMessageAdded));
		EventManager.RemoveEventListener("Event_OnChatMessageRemoved", new Action<Dictionary<string, object>>(this.Event_OnChatMessageRemoved));
		EventManager.RemoveEventListener("Event_OnChatMessagesCleared", new Action<Dictionary<string, object>>(this.Event_OnChatMessagesCleared));
		EventManager.RemoveEventListener("Event_OnQuickChatEnabled", new Action<Dictionary<string, object>>(this.Event_OnQuickChatEnabled));
		EventManager.RemoveEventListener("Event_OnQuickChatDisabled", new Action<Dictionary<string, object>>(this.Event_OnQuickChatDisabled));
		EventManager.RemoveEventListener("Event_OnShowGameUserInterfaceChanged", new Action<Dictionary<string, object>>(this.Event_OnShowGameUserInterfaceChanged));
		EventManager.RemoveEventListener("Event_OnChatOpacityChanged", new Action<Dictionary<string, object>>(this.Event_OnChatOpacityChanged));
		EventManager.RemoveEventListener("Event_OnChatScaleChanged", new Action<Dictionary<string, object>>(this.Event_OnChatScaleChanged));
		EventManager.RemoveEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
		base.OnDestroy();
	}

	// Token: 0x06000DC4 RID: 3524 RVA: 0x00041B30 File Offset: 0x0003FD30
	private void Event_OnChatMessageAdded(Dictionary<string, object> message)
	{
		ChatMessage chatMessage = (ChatMessage)message["chatMessage"];
		this.uiChat.AddChatMessage(chatMessage, SettingsManager.Units, SettingsManager.FilterChatProfanity);
	}

	// Token: 0x06000DC5 RID: 3525 RVA: 0x00041B64 File Offset: 0x0003FD64
	private void Event_OnChatMessageRemoved(Dictionary<string, object> message)
	{
		ChatMessage chatMessage = (ChatMessage)message["chatMessage"];
		this.uiChat.RemoveChatMessage(chatMessage);
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x00041B8E File Offset: 0x0003FD8E
	private void Event_OnChatMessagesCleared(Dictionary<string, object> message)
	{
		this.uiChat.ClearChatMessages();
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x00041B9C File Offset: 0x0003FD9C
	private void Event_OnQuickChatEnabled(Dictionary<string, object> message)
	{
		QuickChatCategory category = (QuickChatCategory)message["category"];
		QuickChat[] quickChats = (QuickChat[])message["quickChats"];
		this.uiChat.ShowQuickChat(category, quickChats);
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x00041BD8 File Offset: 0x0003FDD8
	private void Event_OnQuickChatDisabled(Dictionary<string, object> message)
	{
		this.uiChat.HideQuickChat();
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x00041BE5 File Offset: 0x0003FDE5
	private void Event_OnShowGameUserInterfaceChanged(Dictionary<string, object> message)
	{
		if (GlobalStateManager.UIState.Phase == UIPhase.LockerRoom)
		{
			return;
		}
		if ((bool)message["value"])
		{
			this.uiChat.Show();
			return;
		}
		this.uiChat.Hide();
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x00041C20 File Offset: 0x0003FE20
	private void Event_OnChatOpacityChanged(Dictionary<string, object> message)
	{
		float opacity = (float)message["value"];
		this.uiChat.SetOpacity(opacity);
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x00041C4C File Offset: 0x0003FE4C
	private void Event_OnChatScaleChanged(Dictionary<string, object> message)
	{
		float scale = (float)message["value"];
		this.uiChat.SetScale(scale);
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x00041BD8 File Offset: 0x0003FDD8
	private void Event_OnClientStopped(Dictionary<string, object> message)
	{
		this.uiChat.HideQuickChat();
	}

	// Token: 0x040008BF RID: 2239
	private UIChat uiChat;
}
