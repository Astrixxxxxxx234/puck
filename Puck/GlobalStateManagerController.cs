using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

// Token: 0x020000D5 RID: 213
public static class GlobalStateManagerController
{
	// Token: 0x060006C0 RID: 1728 RVA: 0x00024E24 File Offset: 0x00023024
	public static void Initialize()
	{
		EventManager.AddEventListener("Event_OnSceneLoaded", new Action<Dictionary<string, object>>(GlobalStateManagerController.Event_OnSceneLoaded));
		EventManager.AddEventListener("Event_OnPopupClickOk", new Action<Dictionary<string, object>>(GlobalStateManagerController.Event_OnPopupClickOk));
		EventManager.AddEventListener("Event_OnPopupClickClose", new Action<Dictionary<string, object>>(GlobalStateManagerController.Event_OnPopupClickClose));
		EventManager.AddEventListener("Event_OnModStateChanged", new Action<Dictionary<string, object>>(GlobalStateManagerController.Event_OnModStateChanged));
		EventManager.AddEventListener("Event_OnModEnableFailed", new Action<Dictionary<string, object>>(GlobalStateManagerController.Event_OnModEnableFailed));
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x00024EA0 File Offset: 0x000230A0
	public static void Dispose()
	{
		EventManager.RemoveEventListener("Event_OnSceneLoaded", new Action<Dictionary<string, object>>(GlobalStateManagerController.Event_OnSceneLoaded));
		EventManager.RemoveEventListener("Event_OnPopupClickOk", new Action<Dictionary<string, object>>(GlobalStateManagerController.Event_OnPopupClickOk));
		EventManager.RemoveEventListener("Event_OnPopupClickClose", new Action<Dictionary<string, object>>(GlobalStateManagerController.Event_OnPopupClickClose));
		EventManager.RemoveEventListener("Event_OnModStateChanged", new Action<Dictionary<string, object>>(GlobalStateManagerController.Event_OnModStateChanged));
		EventManager.RemoveEventListener("Event_OnModEnableFailed", new Action<Dictionary<string, object>>(GlobalStateManagerController.Event_OnModEnableFailed));
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x00024F1C File Offset: 0x0002311C
	private static void Event_OnSceneLoaded(Dictionary<string, object> message)
	{
		if (((Scene)message["scene"]).name == "level_default")
		{
			GlobalStateManager.SetUIState(new Dictionary<string, object>
			{
				{
					"phase",
					UIPhase.Playing
				}
			});
			return;
		}
		GlobalStateManager.SetUIState(new Dictionary<string, object>
		{
			{
				"phase",
				UIPhase.LockerRoom
			}
		});
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x00024F84 File Offset: 0x00023184
	private static void Event_OnPopupClickOk(Dictionary<string, object> message)
	{
		Popup popup = (Popup)message["popup"];
		string name = popup.Name;
		if (!(name == "missingPassword"))
		{
			if (!(name == "missingMods"))
			{
				return;
			}
			if (GlobalStateManager.ReconnectionState.Phase == ReconnectionPhase.AwaitingMods)
			{
				string[] clientRequiredModIds = GlobalStateManager.ReconnectionState.ClientRequiredModIds;
				string[] second = (from mod in ModManager.EnabledMods
				select mod.Id).ToArray<string>();
				string[] second2 = (from mod in ModManager.ReadyMods
				select mod.Id).ToArray<string>();
				string[] array = clientRequiredModIds.Except(second2).ToArray<string>();
				string[] value = clientRequiredModIds.Except(array).Except(second).ToArray<string>();
				GlobalStateManager.SetReconnectionState(new Dictionary<string, object>
				{
					{
						"pendingReadinessModIds",
						array
					},
					{
						"pendingEnablingModIds",
						value
					}
				});
			}
		}
		else if (GlobalStateManager.ReconnectionState.Phase == ReconnectionPhase.AwaitingPassword)
		{
			PopupMissingPasswordContent popupMissingPasswordContent = (PopupMissingPasswordContent)popup.Content;
			if (string.IsNullOrEmpty(popupMissingPasswordContent.Password))
			{
				GlobalStateManager.ClearReconnectionState();
				return;
			}
			GlobalStateManager.SetReconnectionState(new Dictionary<string, object>
			{
				{
					"password",
					popupMissingPasswordContent.Password
				}
			});
			return;
		}
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x000250D0 File Offset: 0x000232D0
	private static void Event_OnPopupClickClose(Dictionary<string, object> message)
	{
		string name = ((Popup)message["popup"]).Name;
		if ((name == "missingPassword" || name == "missingMods") && (GlobalStateManager.ReconnectionState.Phase == ReconnectionPhase.AwaitingPassword || GlobalStateManager.ReconnectionState.Phase == ReconnectionPhase.AwaitingMods))
		{
			GlobalStateManager.ClearReconnectionState();
		}
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x0002512C File Offset: 0x0002332C
	private static void Event_OnModStateChanged(Dictionary<string, object> message)
	{
		Mod mod = (Mod)message["mod"];
		BasePluginState basePluginState = (BasePluginState)message["oldState"];
		BasePluginState basePluginState2 = (BasePluginState)message["newState"];
		if (GlobalStateManager.ReconnectionState.Phase == ReconnectionPhase.AwaitingMods)
		{
			bool flag = GlobalStateManager.ReconnectionState.PendingReadinessModIds.Contains(mod.Id);
			bool flag2 = GlobalStateManager.ReconnectionState.PendingEnablingModIds.Contains(mod.Id);
			bool flag3 = basePluginState.IsEnabled != basePluginState2.IsEnabled;
			bool flag4 = basePluginState.IsReady != basePluginState2.IsReady;
			if (flag && flag4 && basePluginState2.IsReady)
			{
				GlobalStateManager.SetReconnectionState(new Dictionary<string, object>
				{
					{
						"pendingReadinessModIds",
						(from id in GlobalStateManager.ReconnectionState.PendingReadinessModIds
						where id != mod.Id
						select id).ToArray<string>()
					},
					{
						"pendingEnablingModIds",
						GlobalStateManager.ReconnectionState.PendingEnablingModIds.Append(mod.Id).ToArray<string>()
					}
				});
				return;
			}
			if (flag2 && flag3 && basePluginState2.IsEnabled)
			{
				GlobalStateManager.SetReconnectionState(new Dictionary<string, object>
				{
					{
						"pendingEnablingModIds",
						(from id in GlobalStateManager.ReconnectionState.PendingEnablingModIds
						where id != mod.Id
						select id).ToArray<string>()
					}
				});
			}
		}
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x00025292 File Offset: 0x00023492
	private static void Event_OnModEnableFailed(Dictionary<string, object> message)
	{
		if (GlobalStateManager.ReconnectionState.Phase == ReconnectionPhase.AwaitingMods)
		{
			GlobalStateManager.ClearReconnectionState();
		}
	}

	// Token: 0x04000456 RID: 1110
	private static readonly Logger Logger = new Logger("GlobalStateManagerController");
}
