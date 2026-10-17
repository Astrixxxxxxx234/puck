using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020000C1 RID: 193
public class ChatManagerController : MonoBehaviour
{
	// Token: 0x06000622 RID: 1570 RVA: 0x00021F4C File Offset: 0x0002014C
	private void Awake()
	{
		this.chatManager = base.GetComponent<ChatManager>();
		InputManager.QuickChat1Action.performed += this.OnQuickChatAction1Performed;
		InputManager.QuickChat2Action.performed += this.OnQuickChatAction2Performed;
		InputManager.QuickChat3Action.performed += this.OnQuickChatAction3Performed;
		InputManager.QuickChat4Action.performed += this.OnQuickChatAction4Performed;
		InputManager.QuickChat5Action.performed += this.OnQuickChatAction5Performed;
		EventManager.AddEventListener("Event_OnChatSubmitMessage", new Action<Dictionary<string, object>>(this.Event_OnChatSubmitMessage));
		EventManager.AddEventListener("Event_OnQuickChatCancel", new Action<Dictionary<string, object>>(this.Event_OnQuickChatCancel));
		EventManager.AddEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
		EventManager.AddEventListener("Event_Server_OnChatMessageReceived", new Action<Dictionary<string, object>>(this.Event_Server_OnChatMessageReceived));
		WebSocketManager.AddMessageListener("playerAnnouncement", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerAnnouncement));
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x00022044 File Offset: 0x00020244
	private void OnDestroy()
	{
		InputManager.QuickChat1Action.performed -= this.OnQuickChatAction1Performed;
		InputManager.QuickChat2Action.performed -= this.OnQuickChatAction2Performed;
		InputManager.QuickChat3Action.performed -= this.OnQuickChatAction3Performed;
		InputManager.QuickChat4Action.performed -= this.OnQuickChatAction4Performed;
		InputManager.QuickChat5Action.performed -= this.OnQuickChatAction5Performed;
		EventManager.RemoveEventListener("Event_OnChatSubmitMessage", new Action<Dictionary<string, object>>(this.Event_OnChatSubmitMessage));
		EventManager.RemoveEventListener("Event_OnQuickChatCancel", new Action<Dictionary<string, object>>(this.Event_OnQuickChatCancel));
		EventManager.RemoveEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
		EventManager.RemoveEventListener("Event_Server_OnChatMessageReceived", new Action<Dictionary<string, object>>(this.Event_Server_OnChatMessageReceived));
		WebSocketManager.RemoveMessageListener("playerAnnouncement", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerAnnouncement));
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x00022130 File Offset: 0x00020330
	private void OnQuickChatAction1Performed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.Phase != UIPhase.Playing)
		{
			return;
		}
		if (GlobalStateManager.UIState.IsInteracting)
		{
			return;
		}
		this.chatManager.Client_QuickChatAction(0);
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x00022168 File Offset: 0x00020368
	private void OnQuickChatAction2Performed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.Phase != UIPhase.Playing)
		{
			return;
		}
		if (GlobalStateManager.UIState.IsInteracting)
		{
			return;
		}
		this.chatManager.Client_QuickChatAction(1);
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x000221A0 File Offset: 0x000203A0
	private void OnQuickChatAction3Performed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.Phase != UIPhase.Playing)
		{
			return;
		}
		if (GlobalStateManager.UIState.IsInteracting)
		{
			return;
		}
		this.chatManager.Client_QuickChatAction(2);
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x000221D8 File Offset: 0x000203D8
	private void OnQuickChatAction4Performed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.Phase != UIPhase.Playing)
		{
			return;
		}
		if (GlobalStateManager.UIState.IsInteracting)
		{
			return;
		}
		this.chatManager.Client_QuickChatAction(3);
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x00022210 File Offset: 0x00020410
	private void OnQuickChatAction5Performed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.Phase != UIPhase.Playing)
		{
			return;
		}
		if (GlobalStateManager.UIState.IsInteracting)
		{
			return;
		}
		this.chatManager.Client_QuickChatAction(4);
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x00022248 File Offset: 0x00020448
	private void Event_OnChatSubmitMessage(Dictionary<string, object> message)
	{
		string content = (string)message["content"];
		bool isTeamChat = (bool)message["isTeamChat"];
		this.chatManager.Client_SendChatMessage(content, false, isTeamChat);
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x00022288 File Offset: 0x00020488
	private void Event_OnQuickChatCancel(Dictionary<string, object> message)
	{
		this.chatManager.SetQuickChatEnabled(false, null);
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x000222AA File Offset: 0x000204AA
	private void Event_OnClientStopped(Dictionary<string, object> message)
	{
		this.chatManager.ClearChatMessages();
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x000222B8 File Offset: 0x000204B8
	private void Event_Server_OnChatMessageReceived(Dictionary<string, object> message)
	{
		ChatMessage chatMessage = (ChatMessage)message["chatMessage"];
		if (!chatMessage.IsTeamChat)
		{
			this.chatManager.Server_BroadcastChatMessage(chatMessage);
			return;
		}
		if (chatMessage.Team == null)
		{
			return;
		}
		List<ulong> list = new List<ulong>();
		if (chatMessage.Team.Value == PlayerTeam.Spectator || chatMessage.Team.Value == PlayerTeam.None)
		{
			list.AddRange(MonoBehaviourSingleton<PlayerManager>.Instance.GetPlayersByTeams(new PlayerTeam[]
			{
				PlayerTeam.None,
				PlayerTeam.Spectator
			}, false).ConvertAll<ulong>((Player player) => player.OwnerClientId));
		}
		else
		{
			list.AddRange(MonoBehaviourSingleton<PlayerManager>.Instance.GetPlayersByTeam(chatMessage.Team.Value, false).ConvertAll<ulong>((Player player) => player.OwnerClientId));
		}
		this.chatManager.Server_SendChatMessage(chatMessage, list.ToArray());
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x000223B0 File Offset: 0x000205B0
	private void WebSocket_Event_OnPlayerAnnouncement(Dictionary<string, object> message)
	{
		InMessage inMessage = (InMessage)message["inMessage"];
		ChatMessage chatMessage = new ChatMessage
		{
			SteamID = null,
			Username = null,
			Team = null,
			Content = inMessage.GetData<PlayerAnnouncementMessage>().message,
			Timestamp = Utils.GetTimestamp(),
			IsQuickChat = false,
			IsTeamChat = false,
			IsSystem = true
		};
		this.chatManager.AddChatMessage(chatMessage);
	}

	// Token: 0x040003FE RID: 1022
	private ChatManager chatManager;
}
