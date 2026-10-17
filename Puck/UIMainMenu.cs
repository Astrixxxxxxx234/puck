using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

// Token: 0x020001DB RID: 475
public class UIMainMenu : UIView
{
	// Token: 0x06000E76 RID: 3702 RVA: 0x000447D8 File Offset: 0x000429D8
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("MainMenuView", null);
		this.mainMenu = base.View.Query("MainMenu", null);
		this.debug = base.View.Query("Debug", null);
		this.social = base.View.Query("Social", null);
		this.playButton = this.mainMenu.Query("PlayButton", null);
		this.playButton.clicked += this.OnClickPlay;
		this.playerButton = this.mainMenu.Query("PlayerButton", null);
		this.playerButton.clicked += this.OnClickPlayer;
		this.settingsButton = this.mainMenu.Query("SettingsButton", null);
		this.settingsButton.clicked += this.OnClickSettings;
		this.modsButton = this.mainMenu.Query("ModsButton", null);
		this.modsButton.clicked += this.OnClickMods;
		this.exitGameButton = this.mainMenu.Query("ExitGameButton", null);
		this.exitGameButton.clicked += this.OnClickExitGame;
		this.discordButton = this.social.Query("DiscordButton", null);
		this.discordButton.clicked += this.OnClickDiscord;
		this.patreonButton = this.social.Query("PatreonButton", null);
		this.patreonButton.clicked += this.OnClickPatreon;
		this.ipAddressTextField = this.debug.Query("IpAddressTextField", null).First().Query(null, null);
		this.ipAddressTextField.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnIpAddressChanged));
		this.ipAddressTextField.value = this.ipAddress;
		this.portIntegerField = this.debug.Query("PortIntegerField", null).First().Query(null, null);
		this.portIntegerField.RegisterValueChangedCallback(new EventCallback<ChangeEvent<int>>(this.OnPortChanged));
		this.portIntegerField.value = (int)this.port;
		this.passwordTextField = this.debug.Query("PasswordTextField", null).First().Query(null, null);
		this.passwordTextField.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnPasswordChanged));
		this.passwordTextField.value = this.password;
		this.joinServerButton = this.debug.Query("JoinServerButton", null);
		this.joinServerButton.clicked += this.OnClickJoinServer;
		this.hostServerButton = this.debug.Query("HostServerButton", null);
		this.hostServerButton.clicked += this.OnClickHostServer;
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x00044B17 File Offset: 0x00042D17
	public override bool Show()
	{
		bool flag = base.Show();
		if (flag)
		{
			EventManager.TriggerEvent("Event_OnMainMenuShow", null);
		}
		return flag;
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x00044B2D File Offset: 0x00042D2D
	public override bool Hide()
	{
		bool flag = base.Hide();
		if (flag)
		{
			EventManager.TriggerEvent("Event_OnMainMenuHide", null);
		}
		return flag;
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x00044B43 File Offset: 0x00042D43
	public void ShowDebug()
	{
		this.debug.style.display = DisplayStyle.Flex;
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x00044B5B File Offset: 0x00042D5B
	public void HideDebug()
	{
		this.debug.style.display = DisplayStyle.None;
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x00044B73 File Offset: 0x00042D73
	private void OnIpAddressChanged(ChangeEvent<string> changeEvent)
	{
		this.ipAddress = changeEvent.newValue;
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x00044B81 File Offset: 0x00042D81
	private void OnPortChanged(ChangeEvent<int> changeEvent)
	{
		this.port = (ushort)changeEvent.newValue;
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x00044B90 File Offset: 0x00042D90
	private void OnPasswordChanged(ChangeEvent<string> changeEvent)
	{
		this.password = changeEvent.newValue;
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x00044BA0 File Offset: 0x00042DA0
	private void OnClickJoinServer()
	{
		EventManager.TriggerEvent("Event_OnMainMenuClickJoinServer", new Dictionary<string, object>
		{
			{
				"ipAddress",
				this.ipAddress
			},
			{
				"port",
				this.port
			},
			{
				"password",
				this.password
			}
		});
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x00044BF4 File Offset: 0x00042DF4
	private void OnClickHostServer()
	{
		EventManager.TriggerEvent("Event_OnMainMenuClickHostServer", new Dictionary<string, object>
		{
			{
				"port",
				this.port
			},
			{
				"password",
				this.password
			}
		});
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x00044C2C File Offset: 0x00042E2C
	private void OnClickPlay()
	{
		EventManager.TriggerEvent("Event_OnMainMenuClickPlay", null);
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x00044C39 File Offset: 0x00042E39
	private void OnClickPlayer()
	{
		EventManager.TriggerEvent("Event_OnMainMenuClickPlayer", null);
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x00044C46 File Offset: 0x00042E46
	private void OnClickSettings()
	{
		EventManager.TriggerEvent("Event_OnMainMenuClickSettings", null);
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x00044C53 File Offset: 0x00042E53
	private void OnClickMods()
	{
		EventManager.TriggerEvent("Event_OnMainMenuClickMods", null);
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x00044C60 File Offset: 0x00042E60
	private void OnClickExitGame()
	{
		EventManager.TriggerEvent("Event_OnMainMenuClickExitGame", null);
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x00044C6D File Offset: 0x00042E6D
	private void OnClickDiscord()
	{
		EventManager.TriggerEvent("Event_OnSocialClickDiscord", null);
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x00044C7A File Offset: 0x00042E7A
	private void OnClickPatreon()
	{
		EventManager.TriggerEvent("Event_OnSocialClickPatreon", null);
	}

	// Token: 0x04000934 RID: 2356
	private VisualElement mainMenu;

	// Token: 0x04000935 RID: 2357
	private VisualElement debug;

	// Token: 0x04000936 RID: 2358
	private TextField ipAddressTextField;

	// Token: 0x04000937 RID: 2359
	private IntegerField portIntegerField;

	// Token: 0x04000938 RID: 2360
	private TextField passwordTextField;

	// Token: 0x04000939 RID: 2361
	private Button joinServerButton;

	// Token: 0x0400093A RID: 2362
	private Button hostServerButton;

	// Token: 0x0400093B RID: 2363
	private Button playButton;

	// Token: 0x0400093C RID: 2364
	private Button playerButton;

	// Token: 0x0400093D RID: 2365
	private Button settingsButton;

	// Token: 0x0400093E RID: 2366
	private Button modsButton;

	// Token: 0x0400093F RID: 2367
	private Button exitGameButton;

	// Token: 0x04000940 RID: 2368
	private VisualElement social;

	// Token: 0x04000941 RID: 2369
	private Button discordButton;

	// Token: 0x04000942 RID: 2370
	private Button patreonButton;

	// Token: 0x04000943 RID: 2371
	private string ipAddress = "127.0.0.1";

	// Token: 0x04000944 RID: 2372
	private ushort port = 30609;

	// Token: 0x04000945 RID: 2373
	private string password;
}
