using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x020001E5 RID: 485
public class UINewServer : UIView
{
	// Token: 0x06000F02 RID: 3842 RVA: 0x0004747C File Offset: 0x0004567C
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("NewServerView", null);
		this.newServer = base.View.Query("NewServer", null);
		this.closeIconButton = this.newServer.Query("CloseIconButtonContainer", null).First().Query(null, null);
		this.closeIconButton.clicked += this.OnClickClose;
		this.startButton = this.newServer.Query("StartButton", null);
		this.startButton.clicked += this.OnClickStart;
		this.tabView = this.newServer.Query("TabView", null);
		this.dedicatedTab = this.newServer.Query("DedicatedTab", null);
		this.dedicatedNameTextField = this.dedicatedTab.Query("NameTextFieldInput", null).First().Query(null, null);
		this.dedicatedNameTextField.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnDedicatedNameChanged));
		this.dedicatedNameTextField.RegisterCallback<FocusOutEvent>(new EventCallback<FocusOutEvent>(this.OnDedicatedNameFocusOut), TrickleDown.NoTrickleDown);
		this.dedicatedNameTextField.value = this.dedicatedName;
		this.dedicatedProbeDropdown = this.dedicatedTab.Query("ProbeDropdownInput", null).First().Query(null, null);
		this.dedicatedProbeDropdown.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnDedicatedProbeChanged));
		this.dedicatedMaxPlayerSlider = this.dedicatedTab.Query("MaxPlayersSliderInput", null).First().Query(null, null);
		this.dedicatedMaxPlayerSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnDedicatedMaxPlayersChanged));
		this.dedicatedMaxPlayerSlider.value = (float)this.dedicatedMaxPlayers;
		this.dedicatedPasswordTextField = this.dedicatedTab.Query("PasswordTextFieldInput", null).First().Query(null, null);
		this.dedicatedPasswordTextField.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnDedicatedPasswordChanged));
		this.dedicatedPasswordTextField.value = this.dedicatedPassword;
		this.dedicatedPasswordProtectedToggle = this.dedicatedTab.Query("PasswordProtectedToggleInput", null).First().Query(null, null);
		this.dedicatedPasswordProtectedToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnDedicatedPasswordProtectedChanged));
		this.dedicatedPasswordProtectedToggle.value = !string.IsNullOrEmpty(this.dedicatedPassword);
		this.dedicatedForm = this.dedicatedTab.Query("DedicatedForm", null);
		this.cloudBannerIcon = this.dedicatedTab.Query("CloudBannerIcon", null);
		this.cloudBannerLabel = this.dedicatedTab.Query("CloudBannerLabel", null);
		this.cloudPatreonButton = this.dedicatedTab.Query("CloudPatreonButton", null);
		this.cloudPatreonButton.clicked += this.OnClickCloudPatreon;
		this.tabView.activeTabChanged += this.OnActiveTabChanged;
		this.selfHostedTab = this.newServer.Query("SelfHostedTab", null);
		this.selfHostedNameTextField = this.selfHostedTab.Query("NameTextFieldInput", null).First().Query(null, null);
		this.selfHostedNameTextField.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnSelfHostedNameChanged));
		this.selfHostedNameTextField.RegisterCallback<FocusOutEvent>(new EventCallback<FocusOutEvent>(this.OnSelfHostedNameFocusOut), TrickleDown.NoTrickleDown);
		this.selfHostedNameTextField.value = this.selfHostedName;
		this.selfHostedPortIntegerField = this.selfHostedTab.Query("PortIntegerFieldInput", null).First().Query(null, null);
		this.selfHostedPortIntegerField.RegisterValueChangedCallback(new EventCallback<ChangeEvent<int>>(this.OnSelfHostedPortChanged));
		this.selfHostedPortIntegerField.value = this.selfHostedPort;
		this.selfHostedMaxPlayerSlider = this.selfHostedTab.Query("MaxPlayersSliderInput", null).First().Query(null, null);
		this.selfHostedMaxPlayerSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnSelfHostedMaxPlayersChanged));
		this.selfHostedMaxPlayerSlider.value = (float)this.selfHostedMaxPlayers;
		this.selfHostedPasswordTextField = this.selfHostedTab.Query("PasswordTextFieldInput", null).First().Query(null, null);
		this.selfHostedPasswordTextField.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnSelfHostedPasswordChanged));
		this.selfHostedPasswordTextField.value = this.selfHostedPassword;
		this.selfHostedPasswordProtectedToggle = this.selfHostedTab.Query("PasswordProtectedToggleInput", null).First().Query(null, null);
		this.selfHostedPasswordProtectedToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnSelfHostedPasswordProtectedChanged));
		this.selfHostedPasswordProtectedToggle.value = !string.IsNullOrEmpty(this.selfHostedPassword);
		this.selfHostedVoipToggle = this.selfHostedTab.Query("VOIPToggleInput", null).First().Query(null, null);
		this.selfHostedVoipToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnSelfHostedVoipChanged));
		this.selfHostedVoipToggle.value = this.selfHostedUseVoip;
		this.AddTabHeaderIcons();
	}

	// Token: 0x06000F03 RID: 3843 RVA: 0x000479FC File Offset: 0x00045BFC
	private void AddTabHeaderIcons()
	{
		List<VisualElement> list = this.tabView.Query(null, "unity-tab__header").ToList();
		string[] array = new string[]
		{
			"computer",
			"cloud"
		};
		int num = 0;
		while (num < list.Count && num < array.Length)
		{
			VisualElement visualElement = new VisualElement
			{
				name = "TabHeaderIcon",
				pickingMode = PickingMode.Ignore
			};
			visualElement.AddToClassList("tabHeaderIcon");
			visualElement.AddToClassList(array[num]);
			list[num].Insert(0, visualElement);
			num++;
		}
	}

	// Token: 0x06000F04 RID: 3844 RVA: 0x00047A8F File Offset: 0x00045C8F
	public override bool Show()
	{
		if (!base.IsVisible)
		{
			this.Refresh();
		}
		return base.Show();
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x00047AA8 File Offset: 0x00045CA8
	public void Refresh()
	{
		PlayerData playerData = BackendManager.PlayerState.PlayerData;
		this.SetDefaultServerName((playerData != null) ? playerData.username : null);
		PlayerData playerData2 = BackendManager.PlayerState.PlayerData;
		this.SetCloudAvailable(playerData2 != null && playerData2.patreonLevel >= 1);
		this.dedicatedProbesLoaded = false;
		this.RefreshStartButton();
		this.dedicatedLauncherProbes = new Probe[0];
		this.dedicatedProbeDropdown.choices = new List<string>();
		this.dedicatedProbeDropdown.value = null;
		this.dedicatedProbeDropdown.SetEnabled(false);
		WebSocketManager.Emit("playerGetProbesRequest", null, "playerGetProbesResponse");
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x00047B44 File Offset: 0x00045D44
	private void SetDefaultServerName(string username)
	{
		if (string.IsNullOrEmpty(username))
		{
			return;
		}
		string valueWithoutNotify = username + "'s server";
		if (this.selfHostedName == "MY PUCK SERVER")
		{
			this.selfHostedName = valueWithoutNotify;
			this.selfHostedNameTextField.SetValueWithoutNotify(valueWithoutNotify);
		}
		if (this.dedicatedName == "MY PUCK SERVER")
		{
			this.dedicatedName = valueWithoutNotify;
			this.dedicatedNameTextField.SetValueWithoutNotify(valueWithoutNotify);
		}
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x00047BB0 File Offset: 0x00045DB0
	public void SetCloudAvailable(bool patreon)
	{
		this.isPatreon = patreon;
		this.dedicatedForm.SetEnabled(patreon);
		this.cloudPatreonButton.style.display = (patreon ? DisplayStyle.None : DisplayStyle.Flex);
		this.cloudBannerIcon.EnableInClassList("patreon", patreon);
		this.cloudBannerLabel.EnableInClassList("patreon", patreon);
		this.cloudBannerLabel.text = (patreon ? "Thanks for being a Patreon supporter!" : "Cloud servers are only available to Patreon supporters.\nYou can still self-host a server.");
		this.RefreshStartButton();
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x00047C30 File Offset: 0x00045E30
	private void RefreshStartButton()
	{
		bool flag = this.tabView.activeTab == this.dedicatedTab && !this.isPatreon;
		this.startButton.SetEnabled(this.dedicatedProbesLoaded && !flag);
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x00047C78 File Offset: 0x00045E78
	public void SetDedicatedProbes(Probe[] probes)
	{
		this.dedicatedLauncherProbes = (from probe in probes
		orderby probe.ToLabel()
		select probe).ToArray<Probe>();
		this.dedicatedProbeDropdown.choices = (from probe in this.dedicatedLauncherProbes
		select probe.ToLabel()).ToList<string>();
		this.dedicatedProbeDropdown.index = 0;
		this.dedicatedProbeDropdown.SetEnabled(true);
		this.dedicatedProbesLoaded = true;
		this.RefreshStartButton();
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x00047D14 File Offset: 0x00045F14
	private void ResetDedicatedName()
	{
		this.dedicatedName = "MY PUCK SERVER";
		this.dedicatedNameTextField.value = this.dedicatedName;
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x00047D32 File Offset: 0x00045F32
	private void ResetSelfHostedName()
	{
		this.selfHostedName = "MY PUCK SERVER";
		this.selfHostedNameTextField.value = this.selfHostedName;
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x00047D50 File Offset: 0x00045F50
	private void OnClickClose()
	{
		EventManager.TriggerEvent("Event_OnNewServerClickClose", null);
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x00044C7A File Offset: 0x00042E7A
	private void OnClickCloudPatreon()
	{
		EventManager.TriggerEvent("Event_OnSocialClickPatreon", null);
	}

	// Token: 0x06000F0E RID: 3854 RVA: 0x00047D5D File Offset: 0x00045F5D
	private void OnActiveTabChanged(Tab previousTab, Tab currentTab)
	{
		this.RefreshStartButton();
	}

	// Token: 0x06000F0F RID: 3855 RVA: 0x00047D68 File Offset: 0x00045F68
	private void OnClickStart()
	{
		if (this.tabView.activeTab != this.dedicatedTab)
		{
			if (this.tabView.activeTab == this.selfHostedTab)
			{
				EventManager.TriggerEvent("Event_OnNewServerClickStart", new Dictionary<string, object>
				{
					{
						"type",
						"selfHosted"
					},
					{
						"port",
						this.selfHostedPort
					},
					{
						"name",
						this.selfHostedName
					},
					{
						"maxPlayers",
						this.selfHostedMaxPlayers
					},
					{
						"password",
						this.selfHostedPassword
					},
					{
						"useVoip",
						this.selfHostedUseVoip
					}
				});
			}
			return;
		}
		if (!this.isPatreon)
		{
			return;
		}
		EventManager.TriggerEvent("Event_OnNewServerClickStart", new Dictionary<string, object>
		{
			{
				"type",
				"dedicated"
			},
			{
				"name",
				this.dedicatedName
			},
			{
				"maxPlayers",
				this.dedicatedMaxPlayers
			},
			{
				"password",
				this.dedicatedPassword
			},
			{
				"probeId",
				this.dedicatedProbe.id
			}
		});
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x00047E98 File Offset: 0x00046098
	private void OnDedicatedNameChanged(ChangeEvent<string> changeEvent)
	{
		this.dedicatedName = StringUtils.FilterStringSpecialCharacters(changeEvent.newValue, null, null);
		this.dedicatedNameTextField.value = this.dedicatedName;
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x00047EBE File Offset: 0x000460BE
	private void OnDedicatedNameFocusOut(FocusOutEvent focusOutEvent)
	{
		this.dedicatedName = StringUtils.FilterStringProfanity(this.dedicatedName, false);
		if (string.IsNullOrEmpty(this.dedicatedName))
		{
			this.ResetDedicatedName();
			return;
		}
		this.dedicatedNameTextField.value = this.dedicatedName;
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x00047EF8 File Offset: 0x000460F8
	private void OnDedicatedProbeChanged(ChangeEvent<string> changeEvent)
	{
		int index = this.dedicatedProbeDropdown.index;
		if (index < 0 || index >= this.dedicatedLauncherProbes.Length)
		{
			return;
		}
		this.dedicatedProbe = this.dedicatedLauncherProbes[index];
	}

	// Token: 0x06000F13 RID: 3859 RVA: 0x00047F2F File Offset: 0x0004612F
	private void OnDedicatedMaxPlayersChanged(ChangeEvent<float> changeEvent)
	{
		this.dedicatedMaxPlayers = Mathf.RoundToInt(changeEvent.newValue);
		this.dedicatedMaxPlayerSlider.value = (float)this.dedicatedMaxPlayers;
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x00047F54 File Offset: 0x00046154
	private void OnDedicatedPasswordProtectedChanged(ChangeEvent<bool> changeEvent)
	{
		if (changeEvent.newValue)
		{
			this.dedicatedPasswordTextField.SetEnabled(true);
			return;
		}
		this.dedicatedPasswordTextField.SetEnabled(false);
		this.dedicatedPasswordTextField.value = string.Empty;
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x00047F87 File Offset: 0x00046187
	private void OnDedicatedPasswordChanged(ChangeEvent<string> changeEvent)
	{
		this.dedicatedPassword = changeEvent.newValue;
		this.dedicatedPasswordTextField.value = this.dedicatedPassword;
	}

	// Token: 0x06000F16 RID: 3862 RVA: 0x00047FA6 File Offset: 0x000461A6
	private void OnSelfHostedNameChanged(ChangeEvent<string> changeEvent)
	{
		this.selfHostedName = StringUtils.FilterStringSpecialCharacters(changeEvent.newValue, null, null);
		this.selfHostedNameTextField.value = this.selfHostedName;
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x00047FCC File Offset: 0x000461CC
	private void OnSelfHostedNameFocusOut(FocusOutEvent focusOutEvent)
	{
		this.selfHostedName = StringUtils.FilterStringProfanity(this.selfHostedName, false);
		if (string.IsNullOrEmpty(this.selfHostedName))
		{
			this.ResetSelfHostedName();
			return;
		}
		this.selfHostedNameTextField.value = this.selfHostedName;
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x00048005 File Offset: 0x00046205
	private void OnSelfHostedPortChanged(ChangeEvent<int> changeEvent)
	{
		this.selfHostedPort = Mathf.Clamp(changeEvent.newValue, 1, 65535);
		this.selfHostedPortIntegerField.SetValueWithoutNotify(this.selfHostedPort);
	}

	// Token: 0x06000F19 RID: 3865 RVA: 0x0004802F File Offset: 0x0004622F
	private void OnSelfHostedMaxPlayersChanged(ChangeEvent<float> changeEvent)
	{
		this.selfHostedMaxPlayers = Mathf.RoundToInt(changeEvent.newValue);
		this.selfHostedMaxPlayerSlider.value = (float)this.selfHostedMaxPlayers;
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x00048054 File Offset: 0x00046254
	private void OnSelfHostedPasswordProtectedChanged(ChangeEvent<bool> changeEvent)
	{
		if (changeEvent.newValue)
		{
			this.selfHostedPasswordTextField.SetEnabled(true);
			return;
		}
		this.selfHostedPasswordTextField.SetEnabled(false);
		this.selfHostedPasswordTextField.value = string.Empty;
	}

	// Token: 0x06000F1B RID: 3867 RVA: 0x00048087 File Offset: 0x00046287
	private void OnSelfHostedPasswordChanged(ChangeEvent<string> changeEvent)
	{
		this.selfHostedPassword = changeEvent.newValue;
		this.selfHostedPasswordTextField.value = this.selfHostedPassword;
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x000480A6 File Offset: 0x000462A6
	private void OnSelfHostedVoipChanged(ChangeEvent<bool> changeEvent)
	{
		this.selfHostedUseVoip = changeEvent.newValue;
	}

	// Token: 0x04000974 RID: 2420
	private static readonly global::Logger Logger = new global::Logger("UINewServer");

	// Token: 0x04000975 RID: 2421
	private VisualElement newServer;

	// Token: 0x04000976 RID: 2422
	private IconButton closeIconButton;

	// Token: 0x04000977 RID: 2423
	private Button startButton;

	// Token: 0x04000978 RID: 2424
	private TabView tabView;

	// Token: 0x04000979 RID: 2425
	private Tab dedicatedTab;

	// Token: 0x0400097A RID: 2426
	private Tab selfHostedTab;

	// Token: 0x0400097B RID: 2427
	private TextField dedicatedNameTextField;

	// Token: 0x0400097C RID: 2428
	private DropdownField dedicatedProbeDropdown;

	// Token: 0x0400097D RID: 2429
	private Slider dedicatedMaxPlayerSlider;

	// Token: 0x0400097E RID: 2430
	private Toggle dedicatedPasswordProtectedToggle;

	// Token: 0x0400097F RID: 2431
	private TextField dedicatedPasswordTextField;

	// Token: 0x04000980 RID: 2432
	private TextField selfHostedNameTextField;

	// Token: 0x04000981 RID: 2433
	private IntegerField selfHostedPortIntegerField;

	// Token: 0x04000982 RID: 2434
	private Slider selfHostedMaxPlayerSlider;

	// Token: 0x04000983 RID: 2435
	private Toggle selfHostedPasswordProtectedToggle;

	// Token: 0x04000984 RID: 2436
	private TextField selfHostedPasswordTextField;

	// Token: 0x04000985 RID: 2437
	private Toggle selfHostedVoipToggle;

	// Token: 0x04000986 RID: 2438
	private VisualElement dedicatedForm;

	// Token: 0x04000987 RID: 2439
	private VisualElement cloudBannerIcon;

	// Token: 0x04000988 RID: 2440
	private Label cloudBannerLabel;

	// Token: 0x04000989 RID: 2441
	private Button cloudPatreonButton;

	// Token: 0x0400098A RID: 2442
	private bool isPatreon;

	// Token: 0x0400098B RID: 2443
	private bool dedicatedProbesLoaded;

	// Token: 0x0400098C RID: 2444
	private Probe[] dedicatedLauncherProbes = new Probe[0];

	// Token: 0x0400098D RID: 2445
	private string dedicatedName = "MY PUCK SERVER";

	// Token: 0x0400098E RID: 2446
	private Probe dedicatedProbe;

	// Token: 0x0400098F RID: 2447
	private int dedicatedMaxPlayers = 6;

	// Token: 0x04000990 RID: 2448
	private string dedicatedPassword;

	// Token: 0x04000991 RID: 2449
	private int selfHostedPort = 30609;

	// Token: 0x04000992 RID: 2450
	private string selfHostedName = "MY PUCK SERVER";

	// Token: 0x04000993 RID: 2451
	private int selfHostedMaxPlayers = 12;

	// Token: 0x04000994 RID: 2452
	private string selfHostedPassword;

	// Token: 0x04000995 RID: 2453
	private bool selfHostedUseVoip;
}
