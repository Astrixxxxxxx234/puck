using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000215 RID: 533
public class UISettingsController : UIViewController<UISettings>
{
	// Token: 0x060010C6 RID: 4294 RVA: 0x0004F944 File Offset: 0x0004DB44
	public override void Awake()
	{
		base.Awake();
		this.uiSettings = base.GetComponent<UISettings>();
		this.micTester = base.gameObject.AddComponent<MicTester>();
		this.micTester.OnLevelChanged += this.OnMicTestLevelChanged;
		UISettings uisettings = this.uiSettings;
		uisettings.OnVisibility = (Action<UIView>)Delegate.Combine(uisettings.OnVisibility, new Action<UIView>(this.OnSettingsVisibilityChanged));
		UISettings uisettings2 = this.uiSettings;
		uisettings2.OnActiveTabChanged = (Action)Delegate.Combine(uisettings2.OnActiveTabChanged, new Action(this.RefreshMicTest));
		EventManager.AddEventListener("Event_OnCameraAngleChanged", new Action<Dictionary<string, object>>(this.Event_OnCameraAngleChanged));
		EventManager.AddEventListener("Event_OnHandednessChanged", new Action<Dictionary<string, object>>(this.Event_OnHandednessChanged));
		EventManager.AddEventListener("Event_OnShowPuckSilhouetteChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPuckSilhouetteChanged));
		EventManager.AddEventListener("Event_OnShowTeamColorBarChanged", new Action<Dictionary<string, object>>(this.Event_OnShowTeamColorBarChanged));
		EventManager.AddEventListener("Event_OnShowPuckOutlineChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPuckOutlineChanged));
		EventManager.AddEventListener("Event_OnShowPuckElevationChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPuckElevationChanged));
		EventManager.AddEventListener("Event_OnShowMinimapSticksChanged", new Action<Dictionary<string, object>>(this.Event_OnShowMinimapSticksChanged));
		EventManager.AddEventListener("Event_OnShowMinimapPuckElevationChanged", new Action<Dictionary<string, object>>(this.Event_OnShowMinimapPuckElevationChanged));
		EventManager.AddEventListener("Event_OnShowMinimapFallenIndicatorChanged", new Action<Dictionary<string, object>>(this.Event_OnShowMinimapFallenIndicatorChanged));
		EventManager.AddEventListener("Event_OnShowPlayerUsernamesChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPlayerUsernamesChanged));
		EventManager.AddEventListener("Event_OnPlayerUsernamesFadeThresholdChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerUsernamesFadeThresholdChanged));
		EventManager.AddEventListener("Event_OnMaxMatchmakingPingChanged", new Action<Dictionary<string, object>>(this.Event_OnMaxMatchmakingPingChanged));
		EventManager.AddEventListener("Event_OnNetworkBufferingChanged", new Action<Dictionary<string, object>>(this.Event_OnNetworkBufferingChanged));
		EventManager.AddEventListener("Event_OnFilterChatProfanityChanged", new Action<Dictionary<string, object>>(this.Event_OnFilterChatProfanityChanged));
		EventManager.AddEventListener("Event_OnUnitsChanged", new Action<Dictionary<string, object>>(this.Event_OnUnitsChanged));
		EventManager.AddEventListener("Event_OnShowGameUserInterfaceChanged", new Action<Dictionary<string, object>>(this.Event_OnShowGameUserInterfaceChanged));
		EventManager.AddEventListener("Event_OnUserInterfaceScaleChanged", new Action<Dictionary<string, object>>(this.Event_OnUserInterfaceScaleChanged));
		EventManager.AddEventListener("Event_OnChatOpacityChanged", new Action<Dictionary<string, object>>(this.Event_OnChatOpacityChanged));
		EventManager.AddEventListener("Event_OnChatScaleChanged", new Action<Dictionary<string, object>>(this.Event_OnChatScaleChanged));
		EventManager.AddEventListener("Event_OnMinimapOpacityChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapOpacityChanged));
		EventManager.AddEventListener("Event_OnMinimapBackgroundOpacityChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapBackgroundOpacityChanged));
		EventManager.AddEventListener("Event_OnMinimapHorizontalPositionChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapHorizontalPositionChanged));
		EventManager.AddEventListener("Event_OnMinimapVerticalPositionChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapVerticalPositionChanged));
		EventManager.AddEventListener("Event_OnMinimapScaleChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapScaleChanged));
		EventManager.AddEventListener("Event_OnGlobalStickSensitivityChanged", new Action<Dictionary<string, object>>(this.Event_OnGlobalStickSensitivityChanged));
		EventManager.AddEventListener("Event_OnHorizontalStickSensitivityChanged", new Action<Dictionary<string, object>>(this.Event_OnHorizontalStickSensitivityChanged));
		EventManager.AddEventListener("Event_OnVerticalStickSensitivityChanged", new Action<Dictionary<string, object>>(this.Event_OnVerticalStickSensitivityChanged));
		EventManager.AddEventListener("Event_OnLookSensitivityChanged", new Action<Dictionary<string, object>>(this.Event_OnLookSensitivityChanged));
		EventManager.AddEventListener("Event_OnKeyBindsLoaded", new Action<Dictionary<string, object>>(this.Event_OnKeyBindsLoaded));
		EventManager.AddEventListener("Event_OnKeyBindsSaved", new Action<Dictionary<string, object>>(this.Event_OnKeyBindsSaved));
		EventManager.AddEventListener("Event_OnGlobalVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnGlobalVolumeChanged));
		EventManager.AddEventListener("Event_OnAmbientVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnAmbientVolumeChanged));
		EventManager.AddEventListener("Event_OnGameVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnGameVolumeChanged));
		EventManager.AddEventListener("Event_OnVoiceVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnVoiceVolumeChanged));
		EventManager.AddEventListener("Event_OnUIVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnUIVolumeChanged));
		EventManager.AddEventListener("Event_OnFullScreenModeChanged", new Action<Dictionary<string, object>>(this.Event_OnFullScreenModeChanged));
		EventManager.AddEventListener("Event_OnDisplayIndexChanged", new Action<Dictionary<string, object>>(this.Event_OnDisplayIndexChanged));
		EventManager.AddEventListener("Event_OnResolutionIndexChanged", new Action<Dictionary<string, object>>(this.Event_OnResolutionIndexChanged));
		EventManager.AddEventListener("Event_OnVSyncChanged", new Action<Dictionary<string, object>>(this.Event_OnVSyncChanged));
		EventManager.AddEventListener("Event_OnFpsLimitChanged", new Action<Dictionary<string, object>>(this.Event_OnFpsLimitChanged));
		EventManager.AddEventListener("Event_OnFovChanged", new Action<Dictionary<string, object>>(this.Event_OnFovChanged));
		EventManager.AddEventListener("Event_OnQualityChanged", new Action<Dictionary<string, object>>(this.Event_OnQualityChanged));
		EventManager.AddEventListener("Event_OnShadowQualityChanged", new Action<Dictionary<string, object>>(this.Event_OnShadowQualityChanged));
		EventManager.AddEventListener("Event_OnMotionBlurChanged", new Action<Dictionary<string, object>>(this.Event_OnMotionBlurChanged));
		EventManager.AddEventListener("Event_OnIsDisplayChangeInProgressChanged", new Action<Dictionary<string, object>>(this.Event_OnIsDisplayChangeInProgressChanged));
	}

	// Token: 0x060010C7 RID: 4295 RVA: 0x0004FDB8 File Offset: 0x0004DFB8
	public override void OnDestroy()
	{
		this.micTester.OnLevelChanged -= this.OnMicTestLevelChanged;
		UISettings uisettings = this.uiSettings;
		uisettings.OnVisibility = (Action<UIView>)Delegate.Remove(uisettings.OnVisibility, new Action<UIView>(this.OnSettingsVisibilityChanged));
		UISettings uisettings2 = this.uiSettings;
		uisettings2.OnActiveTabChanged = (Action)Delegate.Remove(uisettings2.OnActiveTabChanged, new Action(this.RefreshMicTest));
		EventManager.RemoveEventListener("Event_OnCameraAngleChanged", new Action<Dictionary<string, object>>(this.Event_OnCameraAngleChanged));
		EventManager.RemoveEventListener("Event_OnHandednessChanged", new Action<Dictionary<string, object>>(this.Event_OnHandednessChanged));
		EventManager.RemoveEventListener("Event_OnShowPuckSilhouetteChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPuckSilhouetteChanged));
		EventManager.RemoveEventListener("Event_OnShowTeamColorBarChanged", new Action<Dictionary<string, object>>(this.Event_OnShowTeamColorBarChanged));
		EventManager.RemoveEventListener("Event_OnShowPuckOutlineChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPuckOutlineChanged));
		EventManager.RemoveEventListener("Event_OnShowPuckElevationChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPuckElevationChanged));
		EventManager.RemoveEventListener("Event_OnShowMinimapSticksChanged", new Action<Dictionary<string, object>>(this.Event_OnShowMinimapSticksChanged));
		EventManager.RemoveEventListener("Event_OnShowMinimapPuckElevationChanged", new Action<Dictionary<string, object>>(this.Event_OnShowMinimapPuckElevationChanged));
		EventManager.RemoveEventListener("Event_OnShowMinimapFallenIndicatorChanged", new Action<Dictionary<string, object>>(this.Event_OnShowMinimapFallenIndicatorChanged));
		EventManager.RemoveEventListener("Event_OnShowPlayerUsernamesChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPlayerUsernamesChanged));
		EventManager.RemoveEventListener("Event_OnPlayerUsernamesFadeThresholdChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerUsernamesFadeThresholdChanged));
		EventManager.RemoveEventListener("Event_OnMaxMatchmakingPingChanged", new Action<Dictionary<string, object>>(this.Event_OnMaxMatchmakingPingChanged));
		EventManager.RemoveEventListener("Event_OnNetworkBufferingChanged", new Action<Dictionary<string, object>>(this.Event_OnNetworkBufferingChanged));
		EventManager.RemoveEventListener("Event_OnFilterChatProfanityChanged", new Action<Dictionary<string, object>>(this.Event_OnFilterChatProfanityChanged));
		EventManager.RemoveEventListener("Event_OnUnitsChanged", new Action<Dictionary<string, object>>(this.Event_OnUnitsChanged));
		EventManager.RemoveEventListener("Event_OnShowGameUserInterfaceChanged", new Action<Dictionary<string, object>>(this.Event_OnShowGameUserInterfaceChanged));
		EventManager.RemoveEventListener("Event_OnUserInterfaceScaleChanged", new Action<Dictionary<string, object>>(this.Event_OnUserInterfaceScaleChanged));
		EventManager.RemoveEventListener("Event_OnChatOpacityChanged", new Action<Dictionary<string, object>>(this.Event_OnChatOpacityChanged));
		EventManager.RemoveEventListener("Event_OnChatScaleChanged", new Action<Dictionary<string, object>>(this.Event_OnChatScaleChanged));
		EventManager.RemoveEventListener("Event_OnMinimapOpacityChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapOpacityChanged));
		EventManager.RemoveEventListener("Event_OnMinimapBackgroundOpacityChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapBackgroundOpacityChanged));
		EventManager.RemoveEventListener("Event_OnMinimapHorizontalPositionChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapHorizontalPositionChanged));
		EventManager.RemoveEventListener("Event_OnMinimapVerticalPositionChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapVerticalPositionChanged));
		EventManager.RemoveEventListener("Event_OnMinimapScaleChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapScaleChanged));
		EventManager.RemoveEventListener("Event_OnGlobalStickSensitivityChanged", new Action<Dictionary<string, object>>(this.Event_OnGlobalStickSensitivityChanged));
		EventManager.RemoveEventListener("Event_OnHorizontalStickSensitivityChanged", new Action<Dictionary<string, object>>(this.Event_OnHorizontalStickSensitivityChanged));
		EventManager.RemoveEventListener("Event_OnVerticalStickSensitivityChanged", new Action<Dictionary<string, object>>(this.Event_OnVerticalStickSensitivityChanged));
		EventManager.RemoveEventListener("Event_OnLookSensitivityChanged", new Action<Dictionary<string, object>>(this.Event_OnLookSensitivityChanged));
		EventManager.RemoveEventListener("Event_OnKeyBindsLoaded", new Action<Dictionary<string, object>>(this.Event_OnKeyBindsLoaded));
		EventManager.RemoveEventListener("Event_OnKeyBindsSaved", new Action<Dictionary<string, object>>(this.Event_OnKeyBindsSaved));
		EventManager.RemoveEventListener("Event_OnGlobalVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnGlobalVolumeChanged));
		EventManager.RemoveEventListener("Event_OnAmbientVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnAmbientVolumeChanged));
		EventManager.RemoveEventListener("Event_OnGameVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnGameVolumeChanged));
		EventManager.RemoveEventListener("Event_OnVoiceVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnVoiceVolumeChanged));
		EventManager.RemoveEventListener("Event_OnUIVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnUIVolumeChanged));
		EventManager.RemoveEventListener("Event_OnFullScreenModeChanged", new Action<Dictionary<string, object>>(this.Event_OnFullScreenModeChanged));
		EventManager.RemoveEventListener("Event_OnDisplayIndexChanged", new Action<Dictionary<string, object>>(this.Event_OnDisplayIndexChanged));
		EventManager.RemoveEventListener("Event_OnResolutionIndexChanged", new Action<Dictionary<string, object>>(this.Event_OnResolutionIndexChanged));
		EventManager.RemoveEventListener("Event_OnVSyncChanged", new Action<Dictionary<string, object>>(this.Event_OnVSyncChanged));
		EventManager.RemoveEventListener("Event_OnFpsLimitChanged", new Action<Dictionary<string, object>>(this.Event_OnFpsLimitChanged));
		EventManager.RemoveEventListener("Event_OnFovChanged", new Action<Dictionary<string, object>>(this.Event_OnFovChanged));
		EventManager.RemoveEventListener("Event_OnQualityChanged", new Action<Dictionary<string, object>>(this.Event_OnQualityChanged));
		EventManager.RemoveEventListener("Event_OnShadowQualityChanged", new Action<Dictionary<string, object>>(this.Event_OnShadowQualityChanged));
		EventManager.RemoveEventListener("Event_OnMotionBlurChanged", new Action<Dictionary<string, object>>(this.Event_OnMotionBlurChanged));
		EventManager.RemoveEventListener("Event_OnIsDisplayChangeInProgressChanged", new Action<Dictionary<string, object>>(this.Event_OnIsDisplayChangeInProgressChanged));
		base.OnDestroy();
	}

	// Token: 0x060010C8 RID: 4296 RVA: 0x00050210 File Offset: 0x0004E410
	private void Event_OnCameraAngleChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateCameraAngle(value);
	}

	// Token: 0x060010C9 RID: 4297 RVA: 0x0005023C File Offset: 0x0004E43C
	private void Event_OnHandednessChanged(Dictionary<string, object> message)
	{
		string nameFromHandedness = Utils.GetNameFromHandedness((PlayerHandedness)message["value"]);
		this.uiSettings.UpdateHandedness(nameFromHandedness);
	}

	// Token: 0x060010CA RID: 4298 RVA: 0x0005026C File Offset: 0x0004E46C
	private void Event_OnShowPuckSilhouetteChanged(Dictionary<string, object> message)
	{
		bool value = (bool)message["value"];
		this.uiSettings.UpdateShowPuckSilhouette(value);
	}

	// Token: 0x060010CB RID: 4299 RVA: 0x00050298 File Offset: 0x0004E498
	private void Event_OnShowTeamColorBarChanged(Dictionary<string, object> message)
	{
		bool value = (bool)message["value"];
		this.uiSettings.UpdateShowTeamColorBar(value);
	}

	// Token: 0x060010CC RID: 4300 RVA: 0x000502C4 File Offset: 0x0004E4C4
	private void Event_OnShowPuckOutlineChanged(Dictionary<string, object> message)
	{
		bool value = (bool)message["value"];
		this.uiSettings.UpdateShowPuckOutline(value);
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x000502F0 File Offset: 0x0004E4F0
	private void Event_OnShowPuckElevationChanged(Dictionary<string, object> message)
	{
		bool value = (bool)message["value"];
		this.uiSettings.UpdateShowPuckElevation(value);
	}

	// Token: 0x060010CE RID: 4302 RVA: 0x0005031C File Offset: 0x0004E51C
	private void Event_OnShowMinimapSticksChanged(Dictionary<string, object> message)
	{
		bool value = (bool)message["value"];
		this.uiSettings.UpdateShowMinimapSticks(value);
	}

	// Token: 0x060010CF RID: 4303 RVA: 0x00050348 File Offset: 0x0004E548
	private void Event_OnShowMinimapPuckElevationChanged(Dictionary<string, object> message)
	{
		bool value = (bool)message["value"];
		this.uiSettings.UpdateShowMinimapPuckElevation(value);
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x00050374 File Offset: 0x0004E574
	private void Event_OnShowMinimapFallenIndicatorChanged(Dictionary<string, object> message)
	{
		bool value = (bool)message["value"];
		this.uiSettings.UpdateShowMinimapFallenIndicator(value);
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x000503A0 File Offset: 0x0004E5A0
	private void Event_OnShowPlayerUsernamesChanged(Dictionary<string, object> message)
	{
		bool value = (bool)message["value"];
		this.uiSettings.UpdateShowPlayerUsernames(value);
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x000503CC File Offset: 0x0004E5CC
	private void Event_OnPlayerUsernamesFadeThresholdChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdatePlayerUsernamesFadeThreshold(value);
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x000503F8 File Offset: 0x0004E5F8
	private void Event_OnMaxMatchmakingPingChanged(Dictionary<string, object> message)
	{
		int value = (int)message["value"];
		this.uiSettings.UpdateMaxMatchmakingPing(value);
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x00050424 File Offset: 0x0004E624
	private void Event_OnNetworkBufferingChanged(Dictionary<string, object> message)
	{
		string nameFromNetworkBuffering = Utils.GetNameFromNetworkBuffering((NetworkBuffering)message["value"]);
		this.uiSettings.UpdateNetworkBuffering(nameFromNetworkBuffering);
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x00050454 File Offset: 0x0004E654
	private void Event_OnFilterChatProfanityChanged(Dictionary<string, object> message)
	{
		bool value = (bool)message["value"];
		this.uiSettings.UpdateFilterChatProfanity(value);
	}

	// Token: 0x060010D6 RID: 4310 RVA: 0x00050480 File Offset: 0x0004E680
	private void Event_OnUnitsChanged(Dictionary<string, object> message)
	{
		string nameFromUnits = Utils.GetNameFromUnits((Units)message["value"]);
		this.uiSettings.UpdateUnits(nameFromUnits);
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x000504B0 File Offset: 0x0004E6B0
	private void Event_OnShowGameUserInterfaceChanged(Dictionary<string, object> message)
	{
		bool value = (bool)message["value"];
		this.uiSettings.UpdateShowGameUserInterface(value);
	}

	// Token: 0x060010D8 RID: 4312 RVA: 0x000504DC File Offset: 0x0004E6DC
	private void Event_OnUserInterfaceScaleChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateUserInterfaceScale(value);
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x00050508 File Offset: 0x0004E708
	private void Event_OnChatOpacityChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateChatOpacity(value);
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x00050534 File Offset: 0x0004E734
	private void Event_OnChatScaleChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateChatScale(value);
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x00050560 File Offset: 0x0004E760
	private void Event_OnMinimapOpacityChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateMinimapOpacity(value);
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x0005058C File Offset: 0x0004E78C
	private void Event_OnMinimapBackgroundOpacityChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateMinimapBackgroundOpacity(value);
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x000505B8 File Offset: 0x0004E7B8
	private void Event_OnMinimapHorizontalPositionChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateMinimapHorizontalPosition(value);
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x000505E4 File Offset: 0x0004E7E4
	private void Event_OnMinimapVerticalPositionChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateMinimapVerticalPosition(value);
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x00050610 File Offset: 0x0004E810
	private void Event_OnMinimapScaleChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateMinimapScale(value);
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x0005063C File Offset: 0x0004E83C
	private void Event_OnGlobalStickSensitivityChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateGlobalStickSensitivity(value);
	}

	// Token: 0x060010E1 RID: 4321 RVA: 0x00050668 File Offset: 0x0004E868
	private void Event_OnHorizontalStickSensitivityChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateHorizontalStickSensitivity(value);
	}

	// Token: 0x060010E2 RID: 4322 RVA: 0x00050694 File Offset: 0x0004E894
	private void Event_OnVerticalStickSensitivityChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateVerticalStickSensitivity(value);
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x000506C0 File Offset: 0x0004E8C0
	private void Event_OnLookSensitivityChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateLookSensitivity(value);
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x000506EC File Offset: 0x0004E8EC
	private void Event_OnKeyBindsLoaded(Dictionary<string, object> message)
	{
		Dictionary<string, KeyBind> keyBinds = (Dictionary<string, KeyBind>)message["keyBinds"];
		this.uiSettings.UpdateKeyBindInputs(keyBinds);
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x00050718 File Offset: 0x0004E918
	private void Event_OnKeyBindsSaved(Dictionary<string, object> message)
	{
		Dictionary<string, KeyBind> keyBinds = (Dictionary<string, KeyBind>)message["keyBinds"];
		this.uiSettings.UpdateKeyBindInputs(keyBinds);
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x00050744 File Offset: 0x0004E944
	private void Event_OnGlobalVolumeChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateGlobalVolume(value);
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x00050770 File Offset: 0x0004E970
	private void Event_OnAmbientVolumeChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateAmbientVolume(value);
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x0005079C File Offset: 0x0004E99C
	private void Event_OnGameVolumeChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateGameVolume(value);
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x000507C8 File Offset: 0x0004E9C8
	private void Event_OnVoiceVolumeChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateVoiceVolume(value);
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x000507F4 File Offset: 0x0004E9F4
	private void Event_OnUIVolumeChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateUIVolume(value);
	}

	// Token: 0x060010EB RID: 4331 RVA: 0x0005081E File Offset: 0x0004EA1E
	private void OnSettingsVisibilityChanged(UIView view)
	{
		this.RefreshMicTest();
	}

	// Token: 0x060010EC RID: 4332 RVA: 0x00050826 File Offset: 0x0004EA26
	private void RefreshMicTest()
	{
		this.micTester.SetActive(this.uiSettings.IsVisible && this.uiSettings.IsAudioTabActive);
	}

	// Token: 0x060010ED RID: 4333 RVA: 0x0005084E File Offset: 0x0004EA4E
	private void OnMicTestLevelChanged(float level)
	{
		this.uiSettings.SetMicTestLevel(level);
	}

	// Token: 0x060010EE RID: 4334 RVA: 0x0005085C File Offset: 0x0004EA5C
	private void Event_OnFullScreenModeChanged(Dictionary<string, object> message)
	{
		string nameFromFullScreenMode = Utils.GetNameFromFullScreenMode((FullScreenMode)message["value"]);
		this.uiSettings.UpdateFullScreenMode(nameFromFullScreenMode);
	}

	// Token: 0x060010EF RID: 4335 RVA: 0x0005088C File Offset: 0x0004EA8C
	private void Event_OnDisplayIndexChanged(Dictionary<string, object> message)
	{
		string displayNameFromIndex = Utils.GetDisplayNameFromIndex((int)message["value"]);
		this.uiSettings.UpdateDisplay(displayNameFromIndex);
	}

	// Token: 0x060010F0 RID: 4336 RVA: 0x000508BC File Offset: 0x0004EABC
	private void Event_OnResolutionIndexChanged(Dictionary<string, object> message)
	{
		string resolutionNameFromIndex = Utils.GetResolutionNameFromIndex((int)message["value"]);
		this.uiSettings.UpdateResolution(resolutionNameFromIndex);
	}

	// Token: 0x060010F1 RID: 4337 RVA: 0x000508EC File Offset: 0x0004EAEC
	private void Event_OnVSyncChanged(Dictionary<string, object> message)
	{
		bool value = (bool)message["value"];
		this.uiSettings.UpdateVSync(value);
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x00050918 File Offset: 0x0004EB18
	private void Event_OnFpsLimitChanged(Dictionary<string, object> message)
	{
		int value = (int)message["value"];
		this.uiSettings.UpdateFpsLimit(value);
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x00050944 File Offset: 0x0004EB44
	private void Event_OnFovChanged(Dictionary<string, object> message)
	{
		float value = (float)message["value"];
		this.uiSettings.UpdateFov(value);
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x00050970 File Offset: 0x0004EB70
	private void Event_OnQualityChanged(Dictionary<string, object> message)
	{
		string nameFromApplicationQuality = Utils.GetNameFromApplicationQuality((ApplicationQuality)message["value"]);
		this.uiSettings.UpdateQuality(nameFromApplicationQuality);
	}

	// Token: 0x060010F5 RID: 4341 RVA: 0x000509A0 File Offset: 0x0004EBA0
	private void Event_OnShadowQualityChanged(Dictionary<string, object> message)
	{
		string nameFromShadowQuality = Utils.GetNameFromShadowQuality((global::ShadowQuality)message["value"]);
		this.uiSettings.UpdateShadowQuality(nameFromShadowQuality);
	}

	// Token: 0x060010F6 RID: 4342 RVA: 0x000509D0 File Offset: 0x0004EBD0
	private void Event_OnMotionBlurChanged(Dictionary<string, object> message)
	{
		bool value = (bool)message["value"];
		this.uiSettings.UpdateMotionBlur(value);
	}

	// Token: 0x060010F7 RID: 4343 RVA: 0x000509FC File Offset: 0x0004EBFC
	private void Event_OnIsDisplayChangeInProgressChanged(Dictionary<string, object> message)
	{
		if ((bool)message["isDisplayChangeInProgress"])
		{
			return;
		}
		List<string> resolutionNames = Utils.GetResolutionNames();
		this.uiSettings.UpdateResolutionChoices(resolutionNames);
	}

	// Token: 0x04000AA5 RID: 2725
	private UISettings uiSettings;

	// Token: 0x04000AA6 RID: 2726
	private MicTester micTester;
}
