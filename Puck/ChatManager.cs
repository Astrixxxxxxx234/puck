using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class ChatManager : NetworkBehaviourSingleton<ChatManager>
{
	// Token: 0x0600060D RID: 1549 RVA: 0x0002150C File Offset: 0x0001F70C
	public void AddChatMessage(ChatMessage chatMessage)
	{
		if (this.chatMessages.Count >= this.maxChatMessages)
		{
			this.RemoveChatMessage(this.chatMessages[0]);
		}
		this.chatMessages.Add(chatMessage);
		EventManager.TriggerEvent("Event_OnChatMessageAdded", new Dictionary<string, object>
		{
			{
				"chatMessage",
				chatMessage
			}
		});
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x00021565 File Offset: 0x0001F765
	public void RemoveChatMessage(ChatMessage chatMessage)
	{
		this.chatMessages.Remove(chatMessage);
		EventManager.TriggerEvent("Event_OnChatMessageRemoved", new Dictionary<string, object>
		{
			{
				"chatMessage",
				chatMessage
			}
		});
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x0002158F File Offset: 0x0001F78F
	public void ClearChatMessages()
	{
		this.chatMessages.Clear();
		EventManager.TriggerEvent("Event_OnChatMessagesCleared", null);
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x000215A7 File Offset: 0x0001F7A7
	public string ParseChatMessageContent(string content)
	{
		content = content.Trim();
		if (content.Length > this.maxChatMessageLength)
		{
			content = content.Substring(0, this.maxChatMessageLength);
		}
		return content;
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x000215D0 File Offset: 0x0001F7D0
	public void SetQuickChatEnabled(bool isEnabled, QuickChatCategory? category = null)
	{
		this.isQuickChatEnabled = isEnabled;
		this.quickChatCategory = category;
		if (this.isQuickChatEnabled && this.quickChatCategory != null && this.quickChats.ContainsKey(this.quickChatCategory.Value))
		{
			EventManager.TriggerEvent("Event_OnQuickChatEnabled", new Dictionary<string, object>
			{
				{
					"category",
					this.quickChatCategory.Value
				},
				{
					"quickChats",
					this.quickChats[this.quickChatCategory.Value]
				}
			});
			return;
		}
		EventManager.TriggerEvent("Event_OnQuickChatDisabled", null);
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x00021670 File Offset: 0x0001F870
	public void Client_SendChatMessage(string content, bool isQuickChat, bool isTeamChat)
	{
		content = this.ParseChatMessageContent(content);
		if (string.IsNullOrEmpty(content))
		{
			return;
		}
		this.Client_SendChatMessageRpc(content, isQuickChat, isTeamChat, default(RpcParams));
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x000216A4 File Offset: 0x0001F8A4
	public void Client_QuickChatAction(int index)
	{
		if (this.isQuickChatEnabled)
		{
			if (this.quickChatCategory == null)
			{
				return;
			}
			if (!this.quickChats.ContainsKey(this.quickChatCategory.Value))
			{
				return;
			}
			if (index < 0 || index >= this.quickChats[this.quickChatCategory.Value].Length)
			{
				return;
			}
			QuickChat quickChat = this.quickChats[this.quickChatCategory.Value][index];
			this.SetQuickChatEnabled(false, null);
			this.Client_SendChatMessageRpc(quickChat.Content, true, quickChat.IsTeamChat, default(RpcParams));
			return;
		}
		else
		{
			if (!Enum.IsDefined(typeof(QuickChatCategory), index))
			{
				return;
			}
			this.SetQuickChatEnabled(true, new QuickChatCategory?((QuickChatCategory)index));
			Tween tween = this.quickChatTimeoutTween;
			if (tween != null)
			{
				tween.Kill(false);
			}
			this.quickChatTimeoutTween = DOVirtual.DelayedCall(5f, delegate
			{
				this.SetQuickChatEnabled(false, null);
			}, true);
			return;
		}
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x000217A4 File Offset: 0x0001F9A4
	[Rpc(SendTo.Server, DeferLocal = true)]
	private void Client_SendChatMessageRpc(string content, bool isQuickChat, bool isTeamChat, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 3638797367U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			bool flag = content != null;
			fastBufferWriter.WriteValueSafe<bool>(flag, default(FastBufferWriter.ForPrimitives));
			if (flag)
			{
				fastBufferWriter.WriteValueSafe(content, false);
			}
			fastBufferWriter.WriteValueSafe<bool>(isQuickChat, default(FastBufferWriter.ForPrimitives));
			fastBufferWriter.WriteValueSafe<bool>(isTeamChat, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 3638797367U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		content = this.ParseChatMessageContent(content.ToString());
		if (string.IsNullOrEmpty(content.ToString()))
		{
			return;
		}
		ulong senderClientId = rpcParams.Receive.SenderClientId;
		Player component = NetworkManager.Singleton.ConnectedClients[senderClientId].PlayerObject.GetComponent<Player>();
		if (!component)
		{
			return;
		}
		if (!component.IsChatAvailable)
		{
			this.Server_SendChatMessage("Chat timeout", "#e74c3c", new ulong[]
			{
				senderClientId
			});
			return;
		}
		component.Server_ConsumeChatTicket();
		ChatManager.Logger.Info(string.Format("Received chat message from player {0} ({1}): {2}", component.Username.Value, senderClientId, content));
		if (content[0] == '/')
		{
			string[] array = content.ToString().Split(" ", StringSplitOptions.None);
			string value = array[0].ToLower();
			string[] value2 = array.Skip(1).ToArray<string>();
			EventManager.TriggerEvent("Event_Server_OnChatCommand", new Dictionary<string, object>
			{
				{
					"clientId",
					senderClientId
				},
				{
					"command",
					value
				},
				{
					"args",
					value2
				}
			});
			return;
		}
		if (component.IsMuted.Value)
		{
			this.Server_SendChatMessage("Chat disabled", "#e74c3c", new ulong[]
			{
				senderClientId
			});
			return;
		}
		ChatMessage value3 = new ChatMessage
		{
			SteamID = new FixedString32Bytes?(component.SteamId.Value),
			Username = new FixedString32Bytes?(component.Username.Value),
			Team = new PlayerTeam?(component.Team),
			Content = content,
			Timestamp = Utils.GetTimestamp(),
			IsQuickChat = isQuickChat,
			IsTeamChat = isTeamChat,
			IsSystem = false
		};
		EventManager.TriggerEvent("Event_Server_OnChatMessageReceived", new Dictionary<string, object>
		{
			{
				"chatMessage",
				value3
			}
		});
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x00021AA3 File Offset: 0x0001FCA3
	public void Server_SendChatMessage(ChatMessage chatMessage, params ulong[] clientIds)
	{
		this.Server_SendChatMessageRpc(chatMessage, base.RpcTarget.Group(clientIds, RpcTargetUse.Temp));
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x00021AC0 File Offset: 0x0001FCC0
	public void Server_SendChatMessage(string content, string color, params ulong[] clientIds)
	{
		ChatMessage chatMessage = new ChatMessage
		{
			SteamID = null,
			Username = null,
			Team = null,
			Content = StringUtils.WrapInColor(content, color),
			Timestamp = Utils.GetTimestamp(),
			IsQuickChat = false,
			IsTeamChat = false,
			IsSystem = true
		};
		this.Server_SendChatMessage(chatMessage, clientIds);
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x00021B31 File Offset: 0x0001FD31
	public void Server_BroadcastChatMessage(ChatMessage chatMessage)
	{
		this.Server_SendChatMessageRpc(chatMessage, base.RpcTarget.Everyone);
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x00021B4C File Offset: 0x0001FD4C
	public void Server_BroadcastChatMessage(string content, string color = null)
	{
		ChatMessage chatMessage = new ChatMessage
		{
			SteamID = null,
			Username = null,
			Team = null,
			Content = ((color == null) ? content : StringUtils.WrapInColor(content, color)),
			Timestamp = Utils.GetTimestamp(),
			IsQuickChat = false,
			IsTeamChat = false,
			IsSystem = true
		};
		this.Server_BroadcastChatMessage(chatMessage);
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x00021BC4 File Offset: 0x0001FDC4
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	private void Server_SendChatMessageRpc(ChatMessage chatMessage, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 846499610U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			bool flag = chatMessage != null;
			fastBufferWriter.WriteValueSafe<bool>(flag, default(FastBufferWriter.ForPrimitives));
			if (flag)
			{
				fastBufferWriter.WriteValueSafe<ChatMessage>(chatMessage, default(FastBufferWriter.ForNetworkSerializable));
			}
			base.__endSendRpc(ref fastBufferWriter, 846499610U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		this.AddChatMessage(chatMessage);
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x00021D50 File Offset: 0x0001FF50
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x00021D68 File Offset: 0x0001FF68
	protected override void __initializeRpcs()
	{
		base.__registerRpc(3638797367U, new NetworkBehaviour.RpcReceiveHandler(ChatManager.__rpc_handler_3638797367), "Client_SendChatMessageRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(846499610U, new NetworkBehaviour.RpcReceiveHandler(ChatManager.__rpc_handler_846499610), "Server_SendChatMessageRpc", RpcInvokePermission.Server);
		base.__initializeRpcs();
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x00021DC0 File Offset: 0x0001FFC0
	private static void __rpc_handler_3638797367(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool flag;
		reader.ReadValueSafe<bool>(out flag, default(FastBufferWriter.ForPrimitives));
		string content = null;
		if (flag)
		{
			reader.ReadValueSafe(out content, false);
		}
		bool isQuickChat;
		reader.ReadValueSafe<bool>(out isQuickChat, default(FastBufferWriter.ForPrimitives));
		bool isTeamChat;
		reader.ReadValueSafe<bool>(out isTeamChat, default(FastBufferWriter.ForPrimitives));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((ChatManager)target).Client_SendChatMessageRpc(content, isQuickChat, isTeamChat, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x00021E9C File Offset: 0x0002009C
	private static void __rpc_handler_846499610(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool flag;
		reader.ReadValueSafe<bool>(out flag, default(FastBufferWriter.ForPrimitives));
		ChatMessage chatMessage = null;
		if (flag)
		{
			reader.ReadValueSafe<ChatMessage>(out chatMessage, default(FastBufferWriter.ForNetworkSerializable));
		}
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((ChatManager)target).Server_SendChatMessageRpc(chatMessage, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x00021F44 File Offset: 0x00020144
	protected internal override string __getTypeName()
	{
		return "ChatManager";
	}

	// Token: 0x040003F6 RID: 1014
	private static readonly global::Logger Logger = new global::Logger("ChatManager");

	// Token: 0x040003F7 RID: 1015
	[Header("Settings")]
	[SerializeField]
	private int maxChatMessageLength = 128;

	// Token: 0x040003F8 RID: 1016
	[SerializeField]
	private int maxChatMessages = 100;

	// Token: 0x040003F9 RID: 1017
	[SerializeField]
	private SerializedDictionary<QuickChatCategory, QuickChat[]> quickChats = new SerializedDictionary<QuickChatCategory, QuickChat[]>();

	// Token: 0x040003FA RID: 1018
	private List<ChatMessage> chatMessages = new List<ChatMessage>();

	// Token: 0x040003FB RID: 1019
	private bool isQuickChatEnabled;

	// Token: 0x040003FC RID: 1020
	private QuickChatCategory? quickChatCategory;

	// Token: 0x040003FD RID: 1021
	private Tween quickChatTimeoutTween;
}
