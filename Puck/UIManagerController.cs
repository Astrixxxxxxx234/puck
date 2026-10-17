using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class UIManagerController : MonoBehaviour
{
	// Token: 0x06000C83 RID: 3203 RVA: 0x0003BB10 File Offset: 0x00039D10
	public void Awake()
	{
		this.uiManager = base.GetComponent<UIManager>();
		EventManager.AddEventListener("Event_OnUserInterfaceScaleChanged", new Action<Dictionary<string, object>>(this.Event_OnUserInterfaceScaleChanged));
		EventManager.AddEventListener("Event_OnUIStateChanged", new Action<Dictionary<string, object>>(this.Event_OnUIStateChanged));
		EventManager.AddEventListener("Event_OnMainMenuClickPlay", new Action<Dictionary<string, object>>(this.Event_OnMainMenuClickPlay));
		EventManager.AddEventListener("Event_OnMainMenuClickPlayer", new Action<Dictionary<string, object>>(this.Event_OnMainMenuClickPlayer));
		EventManager.AddEventListener("Event_OnMainMenuClickSettings", new Action<Dictionary<string, object>>(this.Event_OnMainMenuClickSettings));
		EventManager.AddEventListener("Event_OnMainMenuClickMods", new Action<Dictionary<string, object>>(this.Event_OnMainMenuClickMods));
		EventManager.AddEventListener("Event_OnPlayerMenuClickBack", new Action<Dictionary<string, object>>(this.Event_OnPlayerMenuClickBack));
		EventManager.AddEventListener("Event_OnPlayerMenuClickIdentity", new Action<Dictionary<string, object>>(this.Event_OnPlayerMenuClickIdentity));
		EventManager.AddEventListener("Event_OnPlayerMenuClickAppearance", new Action<Dictionary<string, object>>(this.Event_OnPlayerMenuClickAppearance));
		EventManager.AddEventListener("Event_OnIdentityClickClose", new Action<Dictionary<string, object>>(this.Event_OnIdentityClickClose));
		EventManager.AddEventListener("Event_OnAppearanceClickClose", new Action<Dictionary<string, object>>(this.Event_OnAppearanceClickClose));
		EventManager.AddEventListener("Event_OnPauseMenuClickReturnToGame", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickReturnToGame));
		EventManager.AddEventListener("Event_OnPauseMenuClickSettings", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickSettings));
		EventManager.AddEventListener("Event_OnPauseMenuClickSelectTeam", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickSelectTeam));
		EventManager.AddEventListener("Event_OnPauseMenuClickSelectPosition", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickSelectPosition));
		EventManager.AddEventListener("Event_OnPauseMenuClickServerBrowser", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickServerBrowser));
		EventManager.AddEventListener("Event_OnServerBrowserClickClose", new Action<Dictionary<string, object>>(this.Event_OnServerBrowserClickClose));
		EventManager.AddEventListener("Event_OnServerBrowserClickEndPoint", new Action<Dictionary<string, object>>(this.Event_OnServerBrowserClickEndPoint));
		EventManager.AddEventListener("Event_OnServerBrowserClickNewServer", new Action<Dictionary<string, object>>(this.Event_OnServerBrowserClickNewServer));
		EventManager.AddEventListener("Event_OnServerBrowserClickDirectConnect", new Action<Dictionary<string, object>>(this.Event_OnServerBrowserClickDirectConnect));
		EventManager.AddEventListener("Event_OnDirectConnectClickClose", new Action<Dictionary<string, object>>(this.Event_OnDirectConnectClickClose));
		EventManager.AddEventListener("Event_OnDirectConnectClickConnect", new Action<Dictionary<string, object>>(this.Event_OnDirectConnectClickConnect));
		EventManager.AddEventListener("Event_OnSettingsClickClose", new Action<Dictionary<string, object>>(this.Event_OnSettingsClickClose));
		EventManager.AddEventListener("Event_OnNewServerClickStart", new Action<Dictionary<string, object>>(this.Event_OnNewServerClickStart));
		EventManager.AddEventListener("Event_OnNewServerClickClose", new Action<Dictionary<string, object>>(this.Event_OnNewServerClickClose));
		EventManager.AddEventListener("Event_OnModsClickClose", new Action<Dictionary<string, object>>(this.Event_OnModsClickClose));
		EventManager.AddEventListener("Event_OnFooterClickInvite", new Action<Dictionary<string, object>>(this.Event_OnFooterClickInvite));
		EventManager.AddEventListener("Event_OnFriendsClickClose", new Action<Dictionary<string, object>>(this.Event_OnFriendsClickClose));
		EventManager.AddEventListener("Event_OnPlayClickServerBrowser", new Action<Dictionary<string, object>>(this.Event_OnPlayClickServerBrowser));
		EventManager.AddEventListener("Event_OnPlayClickClose", new Action<Dictionary<string, object>>(this.Event_OnPlayClickClose));
		EventManager.AddEventListener("Event_OnChatMessageAdded", new Action<Dictionary<string, object>>(this.Event_OnChatMessageAdded));
		EventManager.AddEventListener("Event_OnMatchJoinTimeoutTickerStarted", new Action<Dictionary<string, object>>(this.Event_OnMatchJoinTimeoutTickerStarted));
		EventManager.AddEventListener("Event_OnMatchJoinTimeoutTickerTick", new Action<Dictionary<string, object>>(this.Event_OnMatchJoinTimeoutTickerTick));
		EventManager.AddEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x0003BE2B File Offset: 0x0003A02B
	private void Start()
	{
		this.uiManager.SetUIScale(SettingsManager.UserInterfaceScale);
		this.uiManager.ShowPhaseViews(GlobalStateManager.UIState.Phase);
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x0003BE54 File Offset: 0x0003A054
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnUserInterfaceScaleChanged", new Action<Dictionary<string, object>>(this.Event_OnUserInterfaceScaleChanged));
		EventManager.RemoveEventListener("Event_OnUIStateChanged", new Action<Dictionary<string, object>>(this.Event_OnUIStateChanged));
		EventManager.RemoveEventListener("Event_OnMainMenuClickPlay", new Action<Dictionary<string, object>>(this.Event_OnMainMenuClickPlay));
		EventManager.RemoveEventListener("Event_OnMainMenuClickPlayer", new Action<Dictionary<string, object>>(this.Event_OnMainMenuClickPlayer));
		EventManager.RemoveEventListener("Event_OnMainMenuClickSettings", new Action<Dictionary<string, object>>(this.Event_OnMainMenuClickSettings));
		EventManager.RemoveEventListener("Event_OnMainMenuClickMods", new Action<Dictionary<string, object>>(this.Event_OnMainMenuClickMods));
		EventManager.RemoveEventListener("Event_OnPlayerMenuClickBack", new Action<Dictionary<string, object>>(this.Event_OnPlayerMenuClickBack));
		EventManager.RemoveEventListener("Event_OnPlayerMenuClickIdentity", new Action<Dictionary<string, object>>(this.Event_OnPlayerMenuClickIdentity));
		EventManager.RemoveEventListener("Event_OnPlayerMenuClickAppearance", new Action<Dictionary<string, object>>(this.Event_OnPlayerMenuClickAppearance));
		EventManager.RemoveEventListener("Event_OnIdentityClickClose", new Action<Dictionary<string, object>>(this.Event_OnIdentityClickClose));
		EventManager.RemoveEventListener("Event_OnAppearanceClickClose", new Action<Dictionary<string, object>>(this.Event_OnAppearanceClickClose));
		EventManager.RemoveEventListener("Event_OnPauseMenuClickReturnToGame", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickReturnToGame));
		EventManager.RemoveEventListener("Event_OnPauseMenuClickSettings", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickSettings));
		EventManager.RemoveEventListener("Event_OnPauseMenuClickSelectTeam", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickSelectTeam));
		EventManager.RemoveEventListener("Event_OnPauseMenuClickSelectPosition", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickSelectPosition));
		EventManager.RemoveEventListener("Event_OnPauseMenuClickServerBrowser", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickServerBrowser));
		EventManager.RemoveEventListener("Event_OnServerBrowserClickClose", new Action<Dictionary<string, object>>(this.Event_OnServerBrowserClickClose));
		EventManager.RemoveEventListener("Event_OnServerBrowserClickEndPoint", new Action<Dictionary<string, object>>(this.Event_OnServerBrowserClickEndPoint));
		EventManager.RemoveEventListener("Event_OnServerBrowserClickNewServer", new Action<Dictionary<string, object>>(this.Event_OnServerBrowserClickNewServer));
		EventManager.RemoveEventListener("Event_OnServerBrowserClickDirectConnect", new Action<Dictionary<string, object>>(this.Event_OnServerBrowserClickDirectConnect));
		EventManager.RemoveEventListener("Event_OnDirectConnectClickClose", new Action<Dictionary<string, object>>(this.Event_OnDirectConnectClickClose));
		EventManager.RemoveEventListener("Event_OnDirectConnectClickConnect", new Action<Dictionary<string, object>>(this.Event_OnDirectConnectClickConnect));
		EventManager.RemoveEventListener("Event_OnSettingsClickClose", new Action<Dictionary<string, object>>(this.Event_OnSettingsClickClose));
		EventManager.RemoveEventListener("Event_OnNewServerClickStart", new Action<Dictionary<string, object>>(this.Event_OnNewServerClickStart));
		EventManager.RemoveEventListener("Event_OnNewServerClickClose", new Action<Dictionary<string, object>>(this.Event_OnNewServerClickClose));
		EventManager.RemoveEventListener("Event_OnModsClickClose", new Action<Dictionary<string, object>>(this.Event_OnModsClickClose));
		EventManager.RemoveEventListener("Event_OnFooterClickInvite", new Action<Dictionary<string, object>>(this.Event_OnFooterClickInvite));
		EventManager.RemoveEventListener("Event_OnFriendsClickClose", new Action<Dictionary<string, object>>(this.Event_OnFriendsClickClose));
		EventManager.RemoveEventListener("Event_OnPlayClickServerBrowser", new Action<Dictionary<string, object>>(this.Event_OnPlayClickServerBrowser));
		EventManager.RemoveEventListener("Event_OnPlayClickClose", new Action<Dictionary<string, object>>(this.Event_OnPlayClickClose));
		EventManager.RemoveEventListener("Event_OnChatMessageAdded", new Action<Dictionary<string, object>>(this.Event_OnChatMessageAdded));
		EventManager.RemoveEventListener("Event_OnMatchJoinTimeoutTickerStarted", new Action<Dictionary<string, object>>(this.Event_OnMatchJoinTimeoutTickerStarted));
		EventManager.RemoveEventListener("Event_OnMatchJoinTimeoutTickerTick", new Action<Dictionary<string, object>>(this.Event_OnMatchJoinTimeoutTickerTick));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x0003C164 File Offset: 0x0003A364
	private void HandlePlayerGameState(Player player)
	{
		ref PlayerGameState value = player.GameState.Value;
		this.uiManager.ShowPhaseViews(GlobalStateManager.UIState.Phase);
		switch (value.Phase)
		{
		case PlayerPhase.TeamSelect:
			this.uiManager.TeamSelect.Show();
			return;
		case PlayerPhase.PositionSelect:
			this.uiManager.PositionSelect.Show();
			return;
		case PlayerPhase.Play:
			this.uiManager.Hud.Show();
			this.uiManager.Minimap.Show();
			return;
		case PlayerPhase.Replay:
		case PlayerPhase.Spectate:
			this.uiManager.Minimap.Show();
			return;
		default:
			return;
		}
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x0003C20C File Offset: 0x0003A40C
	private void Event_OnUserInterfaceScaleChanged(Dictionary<string, object> message)
	{
		float uiscale = (float)message["value"];
		this.uiManager.SetUIScale(uiscale);
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x0003C238 File Offset: 0x0003A438
	private void Event_OnUIStateChanged(Dictionary<string, object> message)
	{
		ref UIState ptr = (UIState)message["oldUIState"];
		UIState uistate = (UIState)message["newUIState"];
		if (ptr.Phase == uistate.Phase)
		{
			return;
		}
		this.uiManager.ShowPhaseViews(uistate.Phase);
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x0003C285 File Offset: 0x0003A485
	private void Event_OnMainMenuClickPlay(Dictionary<string, object> message)
	{
		this.uiManager.Play.Show();
		this.uiManager.MainMenu.Hide();
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x0003C2A9 File Offset: 0x0003A4A9
	private void Event_OnMainMenuClickPlayer(Dictionary<string, object> message)
	{
		this.uiManager.PlayerMenu.Show();
		this.uiManager.MainMenu.Hide();
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x0003C2CD File Offset: 0x0003A4CD
	private void Event_OnMainMenuClickSettings(Dictionary<string, object> message)
	{
		this.uiManager.Settings.Show();
		this.uiManager.MainMenu.Hide();
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x0003C2F1 File Offset: 0x0003A4F1
	private void Event_OnMainMenuClickMods(Dictionary<string, object> message)
	{
		this.uiManager.Mods.Show();
		this.uiManager.MainMenu.Hide();
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x0003C315 File Offset: 0x0003A515
	private void Event_OnPlayerMenuClickBack(Dictionary<string, object> message)
	{
		this.uiManager.MainMenu.Show();
		this.uiManager.PlayerMenu.Hide();
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x0003C339 File Offset: 0x0003A539
	private void Event_OnPlayerMenuClickIdentity(Dictionary<string, object> message)
	{
		this.uiManager.Identity.Show();
		this.uiManager.PlayerMenu.Hide();
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x0003C35D File Offset: 0x0003A55D
	private void Event_OnPlayerMenuClickAppearance(Dictionary<string, object> message)
	{
		this.uiManager.Appearance.Show();
		this.uiManager.PlayerMenu.Hide();
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x0003C381 File Offset: 0x0003A581
	private void Event_OnIdentityClickClose(Dictionary<string, object> message)
	{
		this.uiManager.PlayerMenu.Show();
		this.uiManager.Identity.Hide();
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x0003C3A5 File Offset: 0x0003A5A5
	private void Event_OnAppearanceClickClose(Dictionary<string, object> message)
	{
		this.uiManager.PlayerMenu.Show();
		this.uiManager.Appearance.Hide();
	}

	// Token: 0x06000C92 RID: 3218 RVA: 0x0003C3C9 File Offset: 0x0003A5C9
	private void Event_OnPauseMenuClickReturnToGame(Dictionary<string, object> message)
	{
		this.uiManager.PauseMenu.Hide();
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x0003C3DC File Offset: 0x0003A5DC
	private void Event_OnPauseMenuClickSettings(Dictionary<string, object> message)
	{
		this.uiManager.Settings.Show();
		this.uiManager.PauseMenu.Hide();
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x0003C3C9 File Offset: 0x0003A5C9
	private void Event_OnPauseMenuClickSelectTeam(Dictionary<string, object> message)
	{
		this.uiManager.PauseMenu.Hide();
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x0003C3C9 File Offset: 0x0003A5C9
	private void Event_OnPauseMenuClickSelectPosition(Dictionary<string, object> message)
	{
		this.uiManager.PauseMenu.Hide();
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x0003C400 File Offset: 0x0003A600
	private void Event_OnPauseMenuClickServerBrowser(Dictionary<string, object> message)
	{
		this.uiManager.ServerBrowser.Show();
		this.uiManager.PauseMenu.Hide();
	}

	// Token: 0x06000C97 RID: 3223 RVA: 0x0003C424 File Offset: 0x0003A624
	private void Event_OnServerBrowserClickClose(Dictionary<string, object> message)
	{
		UIPhase phase = GlobalStateManager.UIState.Phase;
		if (phase != UIPhase.LockerRoom)
		{
			if (phase == UIPhase.Playing)
			{
				this.uiManager.PauseMenu.Show();
			}
		}
		else
		{
			this.uiManager.Play.Show();
		}
		this.uiManager.ServerBrowser.Hide();
	}

	// Token: 0x06000C98 RID: 3224 RVA: 0x000023BA File Offset: 0x000005BA
	private void Event_OnServerBrowserClickEndPoint(Dictionary<string, object> message)
	{
	}

	// Token: 0x06000C99 RID: 3225 RVA: 0x0003C47B File Offset: 0x0003A67B
	private void Event_OnServerBrowserClickNewServer(Dictionary<string, object> message)
	{
		this.uiManager.NewServer.Show();
		this.uiManager.ServerBrowser.Hide();
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x0003C49F File Offset: 0x0003A69F
	private void Event_OnServerBrowserClickDirectConnect(Dictionary<string, object> message)
	{
		this.uiManager.DirectConnect.Show();
		this.uiManager.ServerBrowser.Hide();
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x0003C4C3 File Offset: 0x0003A6C3
	private void Event_OnDirectConnectClickClose(Dictionary<string, object> message)
	{
		this.uiManager.ServerBrowser.Show();
		this.uiManager.DirectConnect.Hide();
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x000023BA File Offset: 0x000005BA
	private void Event_OnDirectConnectClickConnect(Dictionary<string, object> message)
	{
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x0003C4E8 File Offset: 0x0003A6E8
	private void Event_OnSettingsClickClose(Dictionary<string, object> message)
	{
		UIPhase phase = GlobalStateManager.UIState.Phase;
		if (phase != UIPhase.LockerRoom)
		{
			if (phase == UIPhase.Playing)
			{
				this.uiManager.PauseMenu.Show();
			}
		}
		else
		{
			this.uiManager.MainMenu.Show();
		}
		this.uiManager.Settings.Hide();
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x0003C53F File Offset: 0x0003A73F
	private void Event_OnNewServerClickStart(Dictionary<string, object> message)
	{
		this.uiManager.ServerBrowser.Show();
		this.uiManager.NewServer.Hide();
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x0003C53F File Offset: 0x0003A73F
	private void Event_OnNewServerClickClose(Dictionary<string, object> message)
	{
		this.uiManager.ServerBrowser.Show();
		this.uiManager.NewServer.Hide();
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x0003C563 File Offset: 0x0003A763
	private void Event_OnModsClickClose(Dictionary<string, object> message)
	{
		this.uiManager.MainMenu.Show();
		this.uiManager.Mods.Hide();
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x0003C587 File Offset: 0x0003A787
	private void Event_OnFooterClickInvite(Dictionary<string, object> message)
	{
		this.uiManager.Friends.Show();
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x0003C59A File Offset: 0x0003A79A
	private void Event_OnFriendsClickClose(Dictionary<string, object> message)
	{
		this.uiManager.Friends.Hide();
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x0003C5AD File Offset: 0x0003A7AD
	private void Event_OnPlayClickServerBrowser(Dictionary<string, object> message)
	{
		this.uiManager.ServerBrowser.Show();
		this.uiManager.Play.Hide();
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x0003C5D1 File Offset: 0x0003A7D1
	private void Event_OnPlayClickClose(Dictionary<string, object> message)
	{
		this.uiManager.MainMenu.Show();
		this.uiManager.Play.Hide();
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x0003C5F5 File Offset: 0x0003A7F5
	private void Event_OnChatMessageAdded(Dictionary<string, object> message)
	{
		this.uiManager.PlayNotificationSound();
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x0003C602 File Offset: 0x0003A802
	private void Event_OnMatchJoinTimeoutTickerStarted(Dictionary<string, object> message)
	{
		if (!BackendUtils.IsConnectedToMatchEndPoint())
		{
			this.uiManager.PlayWhooshSound();
		}
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x0003C616 File Offset: 0x0003A816
	private void Event_OnMatchJoinTimeoutTickerTick(Dictionary<string, object> message)
	{
		if (!BackendUtils.IsConnectedToMatchEndPoint())
		{
			this.uiManager.PlayTickSound();
		}
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x0003C62C File Offset: 0x0003A82C
	private void Event_Everyone_OnPlayerSpawned(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (!player.IsLocalPlayer)
		{
			return;
		}
		this.HandlePlayerGameState(player);
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x0003C65C File Offset: 0x0003A85C
	private void Event_Everyone_OnPlayerGameStateChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		PlayerGameState playerGameState = (PlayerGameState)message["oldGameState"];
		PlayerGameState playerGameState2 = (PlayerGameState)message["newGameState"];
		if (!player.IsLocalPlayer)
		{
			return;
		}
		if (playerGameState.Phase == playerGameState2.Phase)
		{
			return;
		}
		this.HandlePlayerGameState(player);
	}

	// Token: 0x040007D8 RID: 2008
	private UIManager uiManager;
}
