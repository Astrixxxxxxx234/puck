using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Humanizer;

// Token: 0x020001FF RID: 511
internal class UIPopupManagerController : UIViewController<UIPopupManager>
{
	// Token: 0x06000FD1 RID: 4049 RVA: 0x0004A8A8 File Offset: 0x00048AA8
	public override void Awake()
	{
		base.Awake();
		this.uiPopupManager = base.GetComponent<UIPopupManager>();
		EventManager.AddEventListener("Event_OnReconnectionStateChanged", new Action<Dictionary<string, object>>(this.Event_OnReconnectionStateChanged));
		EventManager.AddEventListener("Event_OnIdentityClickConfirm", new Action<Dictionary<string, object>>(this.Event_OnIdentityClickConfirm));
		EventManager.AddEventListener("Event_OnMainMenuClickExitGame", new Action<Dictionary<string, object>>(this.Event_OnMainMenuClickExitGame));
		EventManager.AddEventListener("Event_OnPauseMenuClickExitGame", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickExitGame));
		EventManager.AddEventListener("Event_OnPlayerBanned", new Action<Dictionary<string, object>>(this.Event_OnPlayerBanned));
		EventManager.AddEventListener("Event_OnPlayerMuted", new Action<Dictionary<string, object>>(this.Event_OnPlayerMuted));
		EventManager.AddEventListener("Event_OnPlayerCooldown", new Action<Dictionary<string, object>>(this.Event_OnPlayerCooldown));
		EventManager.AddEventListener("Event_OnSettingsClickResetToDefault", new Action<Dictionary<string, object>>(this.Event_OnSettingsClickResetToDefault));
		EventManager.AddEventListener("Event_OnPopupClickOk", new Action<Dictionary<string, object>>(this.Event_OnPopupClickOk));
		EventManager.AddEventListener("Event_OnPopupClickClose", new Action<Dictionary<string, object>>(this.Event_OnPopupClickClose));
		EventManager.AddEventListener("Event_OnKeyBindRebindStart", new Action<Dictionary<string, object>>(this.Event_OnKeyBindRebindStart));
		EventManager.AddEventListener("Event_OnKeyBindRebindComplete", new Action<Dictionary<string, object>>(this.Event_OnKeyBindRebindComplete));
		EventManager.AddEventListener("Event_OnKeyBindRebindCancel", new Action<Dictionary<string, object>>(this.Event_OnKeyBindRebindCancel));
	}

	// Token: 0x06000FD2 RID: 4050 RVA: 0x0004A9E8 File Offset: 0x00048BE8
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnReconnectionStateChanged", new Action<Dictionary<string, object>>(this.Event_OnReconnectionStateChanged));
		EventManager.RemoveEventListener("Event_OnIdentityClickConfirm", new Action<Dictionary<string, object>>(this.Event_OnIdentityClickConfirm));
		EventManager.RemoveEventListener("Event_OnMainMenuClickExitGame", new Action<Dictionary<string, object>>(this.Event_OnMainMenuClickExitGame));
		EventManager.RemoveEventListener("Event_OnPauseMenuClickExitGame", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickExitGame));
		EventManager.RemoveEventListener("Event_OnPlayerBanned", new Action<Dictionary<string, object>>(this.Event_OnPlayerBanned));
		EventManager.RemoveEventListener("Event_OnPlayerMuted", new Action<Dictionary<string, object>>(this.Event_OnPlayerMuted));
		EventManager.RemoveEventListener("Event_OnPlayerCooldown", new Action<Dictionary<string, object>>(this.Event_OnPlayerCooldown));
		EventManager.RemoveEventListener("Event_OnSettingsClickResetToDefault", new Action<Dictionary<string, object>>(this.Event_OnSettingsClickResetToDefault));
		EventManager.RemoveEventListener("Event_OnPopupClickOk", new Action<Dictionary<string, object>>(this.Event_OnPopupClickOk));
		EventManager.RemoveEventListener("Event_OnPopupClickClose", new Action<Dictionary<string, object>>(this.Event_OnPopupClickClose));
		EventManager.RemoveEventListener("Event_OnKeyBindRebindStart", new Action<Dictionary<string, object>>(this.Event_OnKeyBindRebindStart));
		EventManager.RemoveEventListener("Event_OnKeyBindRebindComplete", new Action<Dictionary<string, object>>(this.Event_OnKeyBindRebindComplete));
		EventManager.RemoveEventListener("Event_OnKeyBindRebindCancel", new Action<Dictionary<string, object>>(this.Event_OnKeyBindRebindCancel));
		base.OnDestroy();
	}

	// Token: 0x06000FD3 RID: 4051 RVA: 0x0004AB1C File Offset: 0x00048D1C
	private void Event_OnReconnectionStateChanged(Dictionary<string, object> message)
	{
		ReconnectionState reconnectionState = (ReconnectionState)message["oldReconnectionState"];
		ReconnectionState reconnectionState2 = (ReconnectionState)message["newReconnectionState"];
		switch (reconnectionState2.Phase)
		{
		case ReconnectionPhase.None:
			if (reconnectionState.Phase == ReconnectionPhase.AwaitingPassword)
			{
				this.uiPopupManager.HidePopup("missingPassword");
				return;
			}
			if (reconnectionState.Phase == ReconnectionPhase.AwaitingMods)
			{
				this.uiPopupManager.HidePopup("missingMods");
				this.uiPopupManager.HidePopup("downloadingMods");
				return;
			}
			break;
		case ReconnectionPhase.AwaitingPassword:
			if (reconnectionState2.Password == null)
			{
				PopupMissingPasswordContent content = this.uiPopupManager.CreateMissingPasswordContent();
				this.uiPopupManager.ShowPopup("missingPassword", "PASSWORD REQUIRED", content, true, true, null);
				return;
			}
			break;
		case ReconnectionPhase.AwaitingMods:
		{
			bool flag = !reconnectionState.ClientRequiredModIds.SequenceEqual(reconnectionState2.ClientRequiredModIds);
			bool flag2 = !reconnectionState.PendingEnablingModIds.SequenceEqual(reconnectionState2.PendingEnablingModIds);
			bool flag3 = !reconnectionState.PendingReadinessModIds.SequenceEqual(reconnectionState2.PendingReadinessModIds);
			if (flag && reconnectionState2.PendingModIds.Length == 0)
			{
				string text = "This server requires the following mods to be installed and enabled in order to join:";
				string text2 = "\ud83d\udccc Safety Notice<br>";
				text2 += "This server requires mods with executable code (.dll files) that run directly on your computer. Steam does not audit mod code, and neither do we. Only proceed if you trust this server's host and its required mods.";
				PopupMissingModsPopupContent content2 = this.uiPopupManager.CreateMissingModsContent(text, text2, reconnectionState2.ClientRequiredModIds);
				this.uiPopupManager.ShowPopup("missingMods", "MODS REQUIRED", content2, true, true, null);
				return;
			}
			if ((flag3 || flag2) && reconnectionState2.PendingModIds.Length != 0)
			{
				string text3 = string.Format("<align=center>Downloading & installing {0} mods...", reconnectionState2.PendingModIds.Length);
				PopupNotificationContent content3 = this.uiPopupManager.CreateNotificationContent(text3);
				this.uiPopupManager.ShowPopup("downloadingMods", "MODS REQUIRED", content3, false, false, null);
			}
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x06000FD4 RID: 4052 RVA: 0x0004ACD0 File Offset: 0x00048ED0
	private void Event_OnIdentityClickConfirm(Dictionary<string, object> message)
	{
		string value = (string)message["username"];
		int num = (int)message["number"];
		PopupNotificationContent content = this.uiPopupManager.CreateNotificationContent("<align=center>Identity can be changed once every 24 hours.<br>Are you sure you want to continue?");
		this.uiPopupManager.ShowPopup("identity", "IDENTITY", content, true, true, new Dictionary<string, object>
		{
			{
				"username",
				value
			},
			{
				"number",
				num
			}
		});
	}

	// Token: 0x06000FD5 RID: 4053 RVA: 0x0004AD4C File Offset: 0x00048F4C
	private void Event_OnMainMenuClickExitGame(Dictionary<string, object> message)
	{
		PopupNotificationContent content = this.uiPopupManager.CreateNotificationContent("<align=center>Are you sure you want to exit the game?");
		this.uiPopupManager.ShowPopup("mainMenuExitGame", "EXIT GAME", content, true, true, null);
	}

	// Token: 0x06000FD6 RID: 4054 RVA: 0x0004AD84 File Offset: 0x00048F84
	private void Event_OnPauseMenuClickExitGame(Dictionary<string, object> message)
	{
		PopupNotificationContent content = this.uiPopupManager.CreateNotificationContent("<align=center>Are you sure you want to exit the game?");
		this.uiPopupManager.ShowPopup("pauseMenuExitGame", "EXIT GAME", content, true, true, null);
	}

	// Token: 0x06000FD7 RID: 4055 RVA: 0x0004ADBC File Offset: 0x00048FBC
	private void Event_OnPlayerBanned(Dictionary<string, object> message)
	{
		string text = (string)message["reason"];
		long num = (long)((double)message["expiresAt"]);
		DateTime utcNow = DateTime.UtcNow;
		TimeSpan timeSpan = DateTimeOffset.FromUnixTimeMilliseconds(num).DateTime.Subtract(utcNow);
		string text2 = "<align=center>Your account has been banned<br>Banned for " + timeSpan.Humanize(2, CultureInfo.InvariantCulture, TimeUnit.Week, TimeUnit.Millisecond, ", ", false);
		if (!string.IsNullOrEmpty(text))
		{
			text2 = text2 + "<br><br><align=left>" + text;
		}
		PopupNotificationContent content = this.uiPopupManager.CreateNotificationContent(text2);
		this.uiPopupManager.ShowPopup("banned", "BANNED", content, true, true, null);
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x0004AE6C File Offset: 0x0004906C
	private void Event_OnPlayerMuted(Dictionary<string, object> message)
	{
		string text = (string)message["reason"];
		long num = (long)((double)message["expiresAt"]);
		DateTime utcNow = DateTime.UtcNow;
		TimeSpan timeSpan = DateTimeOffset.FromUnixTimeMilliseconds(num).DateTime.Subtract(utcNow);
		string text2 = "<align=center>Your account has been muted<br>Muted for " + timeSpan.Humanize(2, CultureInfo.InvariantCulture, TimeUnit.Week, TimeUnit.Millisecond, ", ", false);
		if (!string.IsNullOrEmpty(text))
		{
			text2 = text2 + "<br><br><align=left>" + text;
		}
		PopupNotificationContent content = this.uiPopupManager.CreateNotificationContent(text2);
		this.uiPopupManager.ShowPopup("muted", "MUTED", content, true, true, null);
	}

	// Token: 0x06000FD9 RID: 4057 RVA: 0x0004AF1C File Offset: 0x0004911C
	private void Event_OnPlayerCooldown(Dictionary<string, object> message)
	{
		long num = (long)((double)message["expiresAt"]);
		DateTime utcNow = DateTime.UtcNow;
		TimeSpan timeSpan = DateTimeOffset.FromUnixTimeMilliseconds(num).DateTime.Subtract(utcNow);
		string text = "<align=center>Your account has received a matchmaking cooldown<br>Expires in " + timeSpan.Humanize(2, CultureInfo.InvariantCulture, TimeUnit.Week, TimeUnit.Millisecond, ", ", false);
		PopupNotificationContent content = this.uiPopupManager.CreateNotificationContent(text);
		this.uiPopupManager.ShowPopup("cooldown", "COOLDOWN", content, true, true, null);
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x0004AFA0 File Offset: 0x000491A0
	private void Event_OnSettingsClickResetToDefault(Dictionary<string, object> message)
	{
		PopupNotificationContent content = this.uiPopupManager.CreateNotificationContent("<align=center>This will reset all settings to their default values, including key binds. Are you sure you want to continue?");
		this.uiPopupManager.ShowPopup("settingsResetToDefault", "RESET SETTINGS", content, true, true, null);
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x0004AFD8 File Offset: 0x000491D8
	private void Event_OnPopupClickOk(Dictionary<string, object> message)
	{
		Popup popup = (Popup)message["popup"];
		this.uiPopupManager.HidePopup(popup.Name);
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x0004B008 File Offset: 0x00049208
	private void Event_OnPopupClickClose(Dictionary<string, object> message)
	{
		Popup popup = (Popup)message["popup"];
		this.uiPopupManager.HidePopup(popup.Name);
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x0004B038 File Offset: 0x00049238
	private void Event_OnKeyBindRebindStart(Dictionary<string, object> message)
	{
		if ((bool)message["isComposite"])
		{
			PopupNotificationContent content = this.uiPopupManager.CreateNotificationContent("<align=center>Press a <b>key</b> or combination of <b>modifier + key</b> to rebind");
			this.uiPopupManager.ShowPopup("keyBindRebind", "KEY REBIND", content, false, false, null);
			return;
		}
		PopupNotificationContent content2 = this.uiPopupManager.CreateNotificationContent("<align=center>Press a <b>key</b> to rebind");
		this.uiPopupManager.ShowPopup("keyBindRebind", "KEY REBIND", content2, false, false, null);
	}

	// Token: 0x06000FDE RID: 4062 RVA: 0x0004B0AC File Offset: 0x000492AC
	private void Event_OnKeyBindRebindComplete(Dictionary<string, object> message)
	{
		this.uiPopupManager.HidePopup("keyBindRebind");
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x0004B0AC File Offset: 0x000492AC
	private void Event_OnKeyBindRebindCancel(Dictionary<string, object> message)
	{
		this.uiPopupManager.HidePopup("keyBindRebind");
	}

	// Token: 0x04000A0C RID: 2572
	private UIPopupManager uiPopupManager;
}
