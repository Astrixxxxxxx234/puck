using System;
using System.Collections.Generic;

// Token: 0x020000D4 RID: 212
public static class GlobalStateManager
{
	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x060006AE RID: 1710 RVA: 0x00024956 File Offset: 0x00022B56
	// (set) Token: 0x060006AF RID: 1711 RVA: 0x0002495D File Offset: 0x00022B5D
	public static UIState UIState
	{
		get
		{
			return GlobalStateManager.uiState;
		}
		set
		{
			if (GlobalStateManager.uiState.Equals(value))
			{
				return;
			}
			UIState oldUIState = GlobalStateManager.uiState;
			GlobalStateManager.uiState = value;
			GlobalStateManager.OnUIStateChanged(oldUIState, GlobalStateManager.uiState);
		}
	}

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00024982 File Offset: 0x00022B82
	// (set) Token: 0x060006B1 RID: 1713 RVA: 0x00024989 File Offset: 0x00022B89
	public static ConnectionState ConnectionState
	{
		get
		{
			return GlobalStateManager.connectionState;
		}
		set
		{
			if (GlobalStateManager.connectionState.Equals(value))
			{
				return;
			}
			ConnectionState oldConnectionState = GlobalStateManager.connectionState;
			GlobalStateManager.connectionState = value;
			GlobalStateManager.OnConnectionStateChanged(oldConnectionState, GlobalStateManager.connectionState);
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060006B2 RID: 1714 RVA: 0x000249AE File Offset: 0x00022BAE
	// (set) Token: 0x060006B3 RID: 1715 RVA: 0x000249B5 File Offset: 0x00022BB5
	public static ReconnectionState ReconnectionState
	{
		get
		{
			return GlobalStateManager.reconnectionState;
		}
		set
		{
			if (GlobalStateManager.reconnectionState.Equals(value))
			{
				return;
			}
			ReconnectionState oldReconnectionState = GlobalStateManager.reconnectionState;
			GlobalStateManager.reconnectionState = value;
			GlobalStateManager.OnReconnectionStateChanged(oldReconnectionState, GlobalStateManager.reconnectionState);
		}
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x000249DA File Offset: 0x00022BDA
	public static void Initialize()
	{
		GlobalStateManagerController.Initialize();
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x000249E1 File Offset: 0x00022BE1
	public static void Dispose()
	{
		GlobalStateManagerController.Dispose();
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x000249E8 File Offset: 0x00022BE8
	public static void SetUIState(Dictionary<string, object> updates)
	{
		GlobalStateManager.UIState = new UIState
		{
			Phase = (updates.ContainsKey("phase") ? ((UIPhase)updates["phase"]) : GlobalStateManager.UIState.Phase),
			IsMouseRequired = (updates.ContainsKey("isMouseRequired") ? ((bool)updates["isMouseRequired"]) : GlobalStateManager.UIState.IsMouseRequired),
			IsMouseOverUI = (updates.ContainsKey("isMouseOverUI") ? ((bool)updates["isMouseOverUI"]) : GlobalStateManager.UIState.IsMouseOverUI),
			InteractingViews = (updates.ContainsKey("interactingViews") ? ((List<UIView>)updates["interactingViews"]) : GlobalStateManager.UIState.InteractingViews)
		};
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x00024AC2 File Offset: 0x00022CC2
	public static void ClearUIState()
	{
		GlobalStateManager.UIState = new UIState();
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x00024AD0 File Offset: 0x00022CD0
	public static void SetConnectionState(Dictionary<string, object> updates)
	{
		GlobalStateManager.ConnectionState = new ConnectionState
		{
			Connection = (updates.ContainsKey("connection") ? ((Connection)updates["connection"]) : GlobalStateManager.ConnectionState.Connection),
			LastConnection = (updates.ContainsKey("lastConnection") ? ((Connection)updates["lastConnection"]) : GlobalStateManager.ConnectionState.LastConnection),
			ConnectionRejection = (updates.ContainsKey("connectionRejection") ? ((ConnectionRejection)updates["connectionRejection"]) : GlobalStateManager.ConnectionState.ConnectionRejection),
			Disconnection = (updates.ContainsKey("disconnection") ? ((Disconnection)updates["disconnection"]) : GlobalStateManager.ConnectionState.Disconnection),
			PendingConnection = (updates.ContainsKey("pendingConnection") ? ((Connection)updates["pendingConnection"]) : GlobalStateManager.ConnectionState.PendingConnection),
			Phase = (updates.ContainsKey("phase") ? ((ConnectionPhase)updates["phase"]) : GlobalStateManager.ConnectionState.Phase)
		};
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x00024C0A File Offset: 0x00022E0A
	public static void ClearConnectionState()
	{
		GlobalStateManager.ConnectionState = new ConnectionState();
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x00024C18 File Offset: 0x00022E18
	public static void SetReconnectionState(Dictionary<string, object> updates)
	{
		GlobalStateManager.ReconnectionState = new ReconnectionState
		{
			Phase = (updates.ContainsKey("phase") ? ((ReconnectionPhase)updates["phase"]) : GlobalStateManager.ReconnectionState.Phase),
			Password = (updates.ContainsKey("password") ? ((string)updates["password"]) : GlobalStateManager.ReconnectionState.Password),
			ClientRequiredModIds = (updates.ContainsKey("clientRequiredModIds") ? ((string[])updates["clientRequiredModIds"]) : GlobalStateManager.ReconnectionState.ClientRequiredModIds),
			PendingReadinessModIds = (updates.ContainsKey("pendingReadinessModIds") ? ((string[])updates["pendingReadinessModIds"]) : GlobalStateManager.ReconnectionState.PendingReadinessModIds),
			PendingEnablingModIds = (updates.ContainsKey("pendingEnablingModIds") ? ((string[])updates["pendingEnablingModIds"]) : GlobalStateManager.ReconnectionState.PendingEnablingModIds)
		};
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x00024D22 File Offset: 0x00022F22
	public static void ClearReconnectionState()
	{
		GlobalStateManager.ReconnectionState = new ReconnectionState();
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x00024D2E File Offset: 0x00022F2E
	private static void OnUIStateChanged(UIState oldUIState, UIState newUIState)
	{
		EventManager.TriggerEvent("Event_OnUIStateChanged", new Dictionary<string, object>
		{
			{
				"oldUIState",
				oldUIState
			},
			{
				"newUIState",
				newUIState
			}
		});
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x00024D61 File Offset: 0x00022F61
	private static void OnConnectionStateChanged(ConnectionState oldConnectionState, ConnectionState newConnectionState)
	{
		EventManager.TriggerEvent("Event_OnConnectionStateChanged", new Dictionary<string, object>
		{
			{
				"oldConnectionState",
				oldConnectionState
			},
			{
				"newConnectionState",
				newConnectionState
			}
		});
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x00024D94 File Offset: 0x00022F94
	private static void OnReconnectionStateChanged(ReconnectionState oldReconnectionState, ReconnectionState newReconnectionState)
	{
		GlobalStateManager.Logger.Info(string.Format("Reconnection state changed \nFrom:{{{0}}} \nTo:{{{1}}}", oldReconnectionState, newReconnectionState));
		EventManager.TriggerEvent("Event_OnReconnectionStateChanged", new Dictionary<string, object>
		{
			{
				"oldReconnectionState",
				oldReconnectionState
			},
			{
				"newReconnectionState",
				newReconnectionState
			}
		});
	}

	// Token: 0x04000452 RID: 1106
	private static readonly Logger Logger = new Logger("GlobalStateManager");

	// Token: 0x04000453 RID: 1107
	private static UIState uiState = new UIState();

	// Token: 0x04000454 RID: 1108
	private static ConnectionState connectionState = new ConnectionState();

	// Token: 0x04000455 RID: 1109
	private static ReconnectionState reconnectionState = new ReconnectionState();
}
