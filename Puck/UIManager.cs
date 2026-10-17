using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

// Token: 0x02000193 RID: 403
public class UIManager : MonoBehaviourSingleton<UIManager>
{
	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000C65 RID: 3173 RVA: 0x0003ABEB File Offset: 0x00038DEB
	[HideInInspector]
	public PanelSettings PanelSettings
	{
		get
		{
			return this.UIDocument.panelSettings;
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000C66 RID: 3174 RVA: 0x0003ABF8 File Offset: 0x00038DF8
	[HideInInspector]
	public VisualElement RootVisualElement
	{
		get
		{
			return this.UIDocument.rootVisualElement;
		}
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x0003AC08 File Offset: 0x00038E08
	public override void Awake()
	{
		base.Awake();
		this.UIDocument = base.GetComponent<UIDocument>();
		this.AudioSource = base.GetComponent<AudioSource>();
		this.RootVisualElement.style.display = (ApplicationManager.IsDedicatedGameServer ? DisplayStyle.None : DisplayStyle.Flex);
		this.RootVisualElement.RegisterCallback<PointerEnterEvent>(delegate(PointerEnterEvent e)
		{
			VisualElement visualElement = e.target as VisualElement;
			bool flag = visualElement != null && visualElement is Button && visualElement.enabledInHierarchy;
			bool flag2 = visualElement != null && visualElement.name == "unity-tab__header";
			if (flag || flag2)
			{
				this.PlaySelectSound();
			}
		}, TrickleDown.TrickleDown);
		this.RootVisualElement.RegisterCallback<PointerCaptureOutEvent>(delegate(PointerCaptureOutEvent e)
		{
			VisualElement visualElement = e.target as VisualElement;
			if (visualElement != null && visualElement is Button && visualElement.enabledInHierarchy)
			{
				this.PlayClickSound();
			}
		}, TrickleDown.TrickleDown);
		this.RootVisualElement.RegisterCallback<PointerDownEvent>(delegate(PointerDownEvent e)
		{
			VisualElement visualElement = e.target as VisualElement;
			if (visualElement != null && visualElement.name.Contains("unity-tab__header"))
			{
				this.PlayClickSound();
			}
		}, TrickleDown.TrickleDown);
		this.MainMenu = base.gameObject.GetComponent<UIMainMenu>();
		this.MainMenu.Initialize(this.RootVisualElement);
		this.views.Add(this.MainMenu);
		this.PauseMenu = base.gameObject.GetComponent<UIPauseMenu>();
		this.PauseMenu.Initialize(this.RootVisualElement);
		this.views.Add(this.PauseMenu);
		this.ServerBrowser = base.gameObject.GetComponent<UIServerBrowser>();
		this.ServerBrowser.Initialize(this.RootVisualElement);
		this.views.Add(this.ServerBrowser);
		this.GameState = base.gameObject.GetComponent<UIGameState>();
		this.GameState.Initialize(this.RootVisualElement);
		this.views.Add(this.GameState);
		this.Chat = base.gameObject.GetComponent<UIChat>();
		this.Chat.Initialize(this.RootVisualElement);
		this.views.Add(this.Chat);
		this.TeamSelect = base.gameObject.GetComponent<UITeamSelect>();
		this.TeamSelect.Initialize(this.RootVisualElement);
		this.views.Add(this.TeamSelect);
		this.PositionSelect = base.gameObject.GetComponent<UIPositionSelect>();
		this.PositionSelect.Initialize(this.RootVisualElement);
		this.views.Add(this.PositionSelect);
		this.Scoreboard = base.gameObject.GetComponent<UIScoreboard>();
		this.Scoreboard.Initialize(this.RootVisualElement);
		this.views.Add(this.Scoreboard);
		this.Settings = base.gameObject.GetComponent<UISettings>();
		this.Settings.Initialize(this.RootVisualElement);
		this.views.Add(this.Settings);
		this.Hud = base.gameObject.GetComponent<UIHUD>();
		this.Hud.Initialize(this.RootVisualElement);
		this.views.Add(this.Hud);
		this.Announcements = base.gameObject.GetComponent<UIAnnouncements>();
		this.Announcements.Initialize(this.RootVisualElement);
		this.views.Add(this.Announcements);
		this.Minimap = base.gameObject.GetComponent<UIMinimap>();
		this.Minimap.Initialize(this.RootVisualElement);
		this.views.Add(this.Minimap);
		this.NewServer = base.gameObject.GetComponent<UINewServer>();
		this.NewServer.Initialize(this.RootVisualElement);
		this.views.Add(this.NewServer);
		this.DirectConnect = base.gameObject.GetComponent<UIDirectConnect>();
		this.DirectConnect.Initialize(this.RootVisualElement);
		this.views.Add(this.DirectConnect);
		this.ToastManager = base.gameObject.GetComponent<UIToastManager>();
		this.ToastManager.Initialize(this.RootVisualElement);
		this.views.Add(this.ToastManager);
		this.OverlayManager = base.gameObject.GetComponent<UIOverlayManager>();
		this.OverlayManager.Initialize(this.RootVisualElement);
		this.views.Add(this.OverlayManager);
		this.PlayerMenu = base.gameObject.GetComponent<UIPlayerMenu>();
		this.PlayerMenu.Initialize(this.RootVisualElement);
		this.views.Add(this.PlayerMenu);
		this.Identity = base.gameObject.GetComponent<UIIdentity>();
		this.Identity.Initialize(this.RootVisualElement);
		this.views.Add(this.Identity);
		this.Appearance = base.gameObject.GetComponent<UIAppearance>();
		this.Appearance.Initialize(this.RootVisualElement);
		this.views.Add(this.Appearance);
		this.PopupManager = base.gameObject.GetComponent<UIPopupManager>();
		this.PopupManager.Initialize(this.RootVisualElement);
		this.views.Add(this.PopupManager);
		this.Usernames = base.gameObject.GetComponent<UIUsernames>();
		this.Usernames.Initialize(this.RootVisualElement);
		this.views.Add(this.Usernames);
		this.Debug = base.gameObject.GetComponent<UIDebug>();
		this.Debug.Initialize(this.RootVisualElement);
		this.views.Add(this.Debug);
		this.Mods = base.gameObject.GetComponent<UIMods>();
		this.Mods.Initialize(this.RootVisualElement);
		this.views.Add(this.Mods);
		this.Footer = base.gameObject.GetComponent<UIFooter>();
		this.Footer.Initialize(this.RootVisualElement);
		this.views.Add(this.Footer);
		this.Friends = base.gameObject.GetComponent<UIFriends>();
		this.Friends.Initialize(this.RootVisualElement);
		this.views.Add(this.Friends);
		this.Play = base.gameObject.GetComponent<UIPlay>();
		this.Play.Initialize(this.RootVisualElement);
		this.views.Add(this.Play);
		this.Matchmaking = base.gameObject.GetComponent<UIMatchmaking>();
		this.Matchmaking.Initialize(this.RootVisualElement);
		this.views.Add(this.Matchmaking);
		foreach (UIView uiview in this.views)
		{
			uiview.OnVisibility = (Action<UIView>)Delegate.Combine(uiview.OnVisibility, new Action<UIView>(this.OnViewVisibilityChanged));
			uiview.OnFocus = (Action<UIView>)Delegate.Combine(uiview.OnFocus, new Action<UIView>(this.OnViewFocusChanged));
		}
		InputManager.PauseAction.performed += this.OnPauseActionPerformed;
		InputManager.AllChatAction.performed += this.OnAllChatActionPerformed;
		InputManager.TeamChatAction.performed += this.OnTeamChatActionPerformed;
		InputManager.ScoreboardAction.started += this.OnScoreboardActionStarted;
		InputManager.ScoreboardAction.canceled += this.OnScoreboardActionCanceled;
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x0003B2EC File Offset: 0x000394EC
	private void OnDestroy()
	{
		foreach (UIView uiview in this.views)
		{
			uiview.OnVisibility = (Action<UIView>)Delegate.Remove(uiview.OnVisibility, new Action<UIView>(this.OnViewVisibilityChanged));
			uiview.OnFocus = (Action<UIView>)Delegate.Remove(uiview.OnFocus, new Action<UIView>(this.OnViewFocusChanged));
		}
		InputManager.PauseAction.performed -= this.OnPauseActionPerformed;
		InputManager.AllChatAction.performed -= this.OnAllChatActionPerformed;
		InputManager.TeamChatAction.performed -= this.OnTeamChatActionPerformed;
		InputManager.ScoreboardAction.started -= this.OnScoreboardActionStarted;
		InputManager.ScoreboardAction.canceled -= this.OnScoreboardActionCanceled;
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x0003B3E8 File Offset: 0x000395E8
	private void Update()
	{
		if (!ApplicationManager.IsDedicatedGameServer && GlobalStateManager.UIState.IsMouseRequired)
		{
			this.CheckMouseOverUI();
		}
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x0003B404 File Offset: 0x00039604
	private void HideAllViews()
	{
		foreach (UIView uiview in this.views)
		{
			uiview.Hide();
		}
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x0003B458 File Offset: 0x00039658
	public void ShowPhaseViews(UIPhase phase)
	{
		this.HideAllViews();
		switch (phase)
		{
		case UIPhase.None:
			break;
		case UIPhase.LockerRoom:
			this.Chat.Show();
			this.MainMenu.Show();
			this.Footer.Show();
			return;
		case UIPhase.Playing:
			this.Chat.Show();
			this.GameState.Show();
			this.Announcements.Show();
			this.Usernames.Show();
			break;
		default:
			return;
		}
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x0003B4D3 File Offset: 0x000396D3
	public void SetUIScale(float value)
	{
		this.PanelSettings.scale = value;
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x0003B4E1 File Offset: 0x000396E1
	public void PlaySelectSound()
	{
		if (this.selectAudioClip != null)
		{
			this.AudioSource.PlayOneShot(this.selectAudioClip);
		}
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x0003B502 File Offset: 0x00039702
	public void PlayClickSound()
	{
		if (this.clickAudioClip != null)
		{
			this.AudioSource.PlayOneShot(this.clickAudioClip);
		}
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x0003B523 File Offset: 0x00039723
	public void PlayNotificationSound()
	{
		if (this.notificationAudioClip != null)
		{
			this.AudioSource.PlayOneShot(this.notificationAudioClip);
		}
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x0003B544 File Offset: 0x00039744
	public void PlayWhooshSound()
	{
		if (this.whooshAudioClip != null)
		{
			this.AudioSource.PlayOneShot(this.whooshAudioClip);
		}
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x0003B565 File Offset: 0x00039765
	public void PlayTickSound()
	{
		if (this.tickAudioClip != null)
		{
			this.AudioSource.PlayOneShot(this.tickAudioClip);
		}
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x0003B588 File Offset: 0x00039788
	private void CheckMouseRequirement()
	{
		foreach (UIView uiview in this.views)
		{
			if ((uiview.VisibilityRequiresMouse && uiview.IsVisible) || (uiview.FocusRequiresMouse && uiview.IsFocused))
			{
				GlobalStateManager.SetUIState(new Dictionary<string, object>
				{
					{
						"isMouseRequired",
						true
					}
				});
				return;
			}
		}
		GlobalStateManager.SetUIState(new Dictionary<string, object>
		{
			{
				"isMouseRequired",
				false
			}
		});
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x0003B62C File Offset: 0x0003982C
	private void CheckInteraction()
	{
		List<UIView> list = new List<UIView>();
		foreach (UIView uiview in this.views)
		{
			if ((uiview.VisibilityIsInteractive && uiview.IsVisible) || (uiview.FocusIsInteractive && uiview.IsFocused))
			{
				list.Add(uiview);
			}
		}
		GlobalStateManager.SetUIState(new Dictionary<string, object>
		{
			{
				"interactingViews",
				list
			}
		});
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x0003B6BC File Offset: 0x000398BC
	private void CheckMouseOverUI()
	{
		if (this.RootVisualElement == null)
		{
			return;
		}
		IPanel panel = this.RootVisualElement.panel;
		if (panel == null)
		{
			return;
		}
		Vector2 vector = InputManager.PointAction.ReadValue<Vector2>();
		vector.y = (float)Screen.height - vector.y;
		if (vector == this.lastPointerPosition)
		{
			return;
		}
		this.lastPointerPosition = vector;
		Vector2 point = RuntimePanelUtils.ScreenToPanel(panel, vector);
		bool flag = panel.Pick(point) != null;
		if (flag != GlobalStateManager.UIState.IsMouseOverUI)
		{
			GlobalStateManager.SetUIState(new Dictionary<string, object>
			{
				{
					"isMouseOverUI",
					flag
				}
			});
		}
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x0003B752 File Offset: 0x00039952
	private void OnViewVisibilityChanged(UIView uiView)
	{
		if (uiView.VisibilityRequiresMouse)
		{
			this.CheckMouseRequirement();
		}
		if (uiView.VisibilityIsInteractive)
		{
			this.CheckInteraction();
		}
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x0003B770 File Offset: 0x00039970
	private void OnViewFocusChanged(UIView uiView)
	{
		if (uiView.FocusRequiresMouse)
		{
			this.CheckMouseRequirement();
		}
		if (uiView.FocusIsInteractive)
		{
			this.CheckInteraction();
		}
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x0003B790 File Offset: 0x00039990
	private UIView GetTopmostBlockingInteractingView()
	{
		List<UIView> interactingViews = GlobalStateManager.UIState.InteractingViews;
		for (int i = interactingViews.Count - 1; i >= 0; i--)
		{
			UIView uiview = interactingViews[i];
			if (!(uiview == this.TeamSelect) && !(uiview == this.PositionSelect) && !(uiview == this.Footer))
			{
				return uiview;
			}
		}
		return null;
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x0003B7F0 File Offset: 0x000399F0
	private void OnPauseActionPerformed(InputAction.CallbackContext context)
	{
		UIPhase phase = GlobalStateManager.UIState.Phase;
		if (phase == UIPhase.Playing)
		{
			if (this.Chat.IsFocused)
			{
				this.Chat.StopInput();
				return;
			}
			if (this.Chat.IsQuickChatVisible)
			{
				EventManager.TriggerEvent("Event_OnQuickChatCancel", null);
				return;
			}
		}
		if (this.Friends.IsVisible)
		{
			EventManager.TriggerEvent("Event_OnFriendsClickClose", null);
			return;
		}
		UIView topmostBlockingInteractingView = this.GetTopmostBlockingInteractingView();
		if (this.TryCloseSecondaryView(topmostBlockingInteractingView))
		{
			return;
		}
		if (phase == UIPhase.Playing)
		{
			if (this.PauseMenu.IsVisible && topmostBlockingInteractingView is UIPauseMenu)
			{
				this.PauseMenu.Hide();
				return;
			}
			if (!this.PauseMenu.IsVisible)
			{
				this.PauseMenu.Show();
			}
		}
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x0003B8A8 File Offset: 0x00039AA8
	private bool TryCloseSecondaryView(UIView view)
	{
		if (view is UISettings)
		{
			EventManager.TriggerEvent("Event_OnSettingsClickClose", null);
			return true;
		}
		if (view is UINewServer)
		{
			EventManager.TriggerEvent("Event_OnNewServerClickClose", null);
			return true;
		}
		if (view is UIDirectConnect)
		{
			EventManager.TriggerEvent("Event_OnDirectConnectClickClose", null);
			return true;
		}
		if (view is UIServerBrowser)
		{
			EventManager.TriggerEvent("Event_OnServerBrowserClickClose", null);
			return true;
		}
		if (view is UIPlay)
		{
			EventManager.TriggerEvent("Event_OnPlayClickClose", null);
			return true;
		}
		if (view is UIPlayerMenu)
		{
			EventManager.TriggerEvent("Event_OnPlayerMenuClickBack", null);
			return true;
		}
		if (view is UIIdentity)
		{
			EventManager.TriggerEvent("Event_OnIdentityClickClose", null);
			return true;
		}
		if (view is UIAppearance)
		{
			EventManager.TriggerEvent("Event_OnAppearanceClickClose", null);
			return true;
		}
		if (!(view is UIMods))
		{
			return false;
		}
		EventManager.TriggerEvent("Event_OnModsClickClose", null);
		return true;
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x0003B975 File Offset: 0x00039B75
	private void OnAllChatActionPerformed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.Phase != UIPhase.Playing)
		{
			return;
		}
		if (this.GetTopmostBlockingInteractingView() != null)
		{
			return;
		}
		this.Chat.StartInput(false, UIManager.GetChatOpenCharacter(context));
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x0003B9A6 File Offset: 0x00039BA6
	private void OnTeamChatActionPerformed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.Phase != UIPhase.Playing)
		{
			return;
		}
		if (this.GetTopmostBlockingInteractingView() != null)
		{
			return;
		}
		this.Chat.StartInput(true, UIManager.GetChatOpenCharacter(context));
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x0003B9D7 File Offset: 0x00039BD7
	private static string GetChatOpenCharacter(InputAction.CallbackContext context)
	{
		if (context.control == null || !context.ReadValueAsButton())
		{
			return null;
		}
		return context.control.displayName;
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x0003B9F9 File Offset: 0x00039BF9
	private void OnScoreboardActionStarted(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.Phase != UIPhase.Playing)
		{
			return;
		}
		if (this.GetTopmostBlockingInteractingView() != null)
		{
			return;
		}
		this.Scoreboard.Show();
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x0003BA24 File Offset: 0x00039C24
	private void OnScoreboardActionCanceled(InputAction.CallbackContext context)
	{
		this.Scoreboard.Hide();
	}

	// Token: 0x040007B4 RID: 1972
	[Header("References")]
	[SerializeField]
	private AudioClip selectAudioClip;

	// Token: 0x040007B5 RID: 1973
	[SerializeField]
	private AudioClip clickAudioClip;

	// Token: 0x040007B6 RID: 1974
	[SerializeField]
	private AudioClip notificationAudioClip;

	// Token: 0x040007B7 RID: 1975
	[SerializeField]
	private AudioClip whooshAudioClip;

	// Token: 0x040007B8 RID: 1976
	[SerializeField]
	private AudioClip tickAudioClip;

	// Token: 0x040007B9 RID: 1977
	[HideInInspector]
	public UIDocument UIDocument;

	// Token: 0x040007BA RID: 1978
	[HideInInspector]
	public AudioSource AudioSource;

	// Token: 0x040007BB RID: 1979
	[HideInInspector]
	public UIMainMenu MainMenu;

	// Token: 0x040007BC RID: 1980
	[HideInInspector]
	public UIPauseMenu PauseMenu;

	// Token: 0x040007BD RID: 1981
	[HideInInspector]
	public UIServerBrowser ServerBrowser;

	// Token: 0x040007BE RID: 1982
	[HideInInspector]
	public UIGameState GameState;

	// Token: 0x040007BF RID: 1983
	[HideInInspector]
	public UIChat Chat;

	// Token: 0x040007C0 RID: 1984
	[HideInInspector]
	public UITeamSelect TeamSelect;

	// Token: 0x040007C1 RID: 1985
	[HideInInspector]
	public UIPositionSelect PositionSelect;

	// Token: 0x040007C2 RID: 1986
	[HideInInspector]
	public UIScoreboard Scoreboard;

	// Token: 0x040007C3 RID: 1987
	[HideInInspector]
	public UISettings Settings;

	// Token: 0x040007C4 RID: 1988
	[HideInInspector]
	public UIHUD Hud;

	// Token: 0x040007C5 RID: 1989
	[HideInInspector]
	public UIAnnouncements Announcements;

	// Token: 0x040007C6 RID: 1990
	[HideInInspector]
	public UIMinimap Minimap;

	// Token: 0x040007C7 RID: 1991
	[HideInInspector]
	public UINewServer NewServer;

	// Token: 0x040007C8 RID: 1992
	[HideInInspector]
	public UIDirectConnect DirectConnect;

	// Token: 0x040007C9 RID: 1993
	[HideInInspector]
	public UIToastManager ToastManager;

	// Token: 0x040007CA RID: 1994
	[HideInInspector]
	public UIOverlayManager OverlayManager;

	// Token: 0x040007CB RID: 1995
	[HideInInspector]
	public UIPlayerMenu PlayerMenu;

	// Token: 0x040007CC RID: 1996
	[HideInInspector]
	public UIIdentity Identity;

	// Token: 0x040007CD RID: 1997
	[HideInInspector]
	public UIAppearance Appearance;

	// Token: 0x040007CE RID: 1998
	[HideInInspector]
	public UIPopupManager PopupManager;

	// Token: 0x040007CF RID: 1999
	[HideInInspector]
	public UIUsernames Usernames;

	// Token: 0x040007D0 RID: 2000
	[HideInInspector]
	public UIDebug Debug;

	// Token: 0x040007D1 RID: 2001
	[HideInInspector]
	public UIMods Mods;

	// Token: 0x040007D2 RID: 2002
	[HideInInspector]
	public UIFooter Footer;

	// Token: 0x040007D3 RID: 2003
	[HideInInspector]
	public UIFriends Friends;

	// Token: 0x040007D4 RID: 2004
	[HideInInspector]
	public UIPlay Play;

	// Token: 0x040007D5 RID: 2005
	[HideInInspector]
	public UIMatchmaking Matchmaking;

	// Token: 0x040007D6 RID: 2006
	private List<UIView> views = new List<UIView>();

	// Token: 0x040007D7 RID: 2007
	private Vector2 lastPointerPosition = Vector2.zero;
}
