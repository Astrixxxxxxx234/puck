using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

// Token: 0x02000165 RID: 357
public static class SettingsManagerController
{
	// Token: 0x06000AAA RID: 2730 RVA: 0x00033204 File Offset: 0x00031404
	public static void Initialize()
	{
		EventManager.AddEventListener("Event_OnDisplayIndexChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnDisplayIndexChanged));
		EventManager.AddEventListener("Event_OnIsDisplayChangeInProgressChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnIsDisplayChangeInProgressChanged));
		EventManager.AddEventListener("Event_OnBaseCameraEnabled", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnBaseCameraEnabled));
		EventManager.AddEventListener("Event_OnPopupClickOk", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnPopupClickOk));
		SettingsManagerController.AddSettingsEventListeners();
		SettingsManagerController.AddAppearanceEventListeners();
		InputManager.Debug1Action.performed += SettingsManagerController.OnDebug1ActionPerformed;
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x0003328C File Offset: 0x0003148C
	public static void Dispose()
	{
		EventManager.RemoveEventListener("Event_OnDisplayIndexChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnDisplayIndexChanged));
		EventManager.RemoveEventListener("Event_OnIsDisplayChangeInProgressChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnIsDisplayChangeInProgressChanged));
		EventManager.RemoveEventListener("Event_OnBaseCameraEnabled", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnBaseCameraEnabled));
		EventManager.RemoveEventListener("Event_OnPopupClickOk", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnPopupClickOk));
		SettingsManagerController.RemoveSettingsEventListeners();
		SettingsManagerController.RemoveAppearanceEventListeners();
		InputManager.Debug1Action.performed -= SettingsManagerController.OnDebug1ActionPerformed;
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x00033314 File Offset: 0x00031514
	private static void AddSettingsEventListeners()
	{
		EventManager.AddEventListener("Event_OnSettingsCameraAngleChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsCameraAngleChanged));
		EventManager.AddEventListener("Event_OnSettingsHandednessChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsHandednessChanged));
		EventManager.AddEventListener("Event_OnSettingsShowPuckSilhouetteChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowPuckSilhouetteChanged));
		EventManager.AddEventListener("Event_OnSettingsShowTeamColorBarChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowTeamColorBarChanged));
		EventManager.AddEventListener("Event_OnSettingsShowPuckOutlineChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowPuckOutlineChanged));
		EventManager.AddEventListener("Event_OnSettingsShowPuckElevationChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowPuckElevationChanged));
		EventManager.AddEventListener("Event_OnSettingsShowMinimapSticksChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowMinimapSticksChanged));
		EventManager.AddEventListener("Event_OnSettingsShowMinimapPuckElevationChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowMinimapPuckElevationChanged));
		EventManager.AddEventListener("Event_OnSettingsShowMinimapFallenIndicatorChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowMinimapFallenIndicatorChanged));
		EventManager.AddEventListener("Event_OnSettingsShowPlayerUsernamesChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowPlayerUsernamesChanged));
		EventManager.AddEventListener("Event_OnSettingsPlayerUsernamesFadeThresholdChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsPlayerUsernamesFadeThresholdChanged));
		EventManager.AddEventListener("Event_OnSettingsMaxMatchmakingPingChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsMaxMatchmakingPingChanged));
		EventManager.AddEventListener("Event_OnSettingsNetworkBufferingChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsNetworkBufferingChanged));
		EventManager.AddEventListener("Event_OnSettingsFilterChatProfanityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsFilterChatProfanityChanged));
		EventManager.AddEventListener("Event_OnSettingsUnitsChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsUnitsChanged));
		EventManager.AddEventListener("Event_OnSettingsShowGameUserInterfaceChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowGameUserInterfaceChanged));
		EventManager.AddEventListener("Event_OnSettingsUserInterfaceScaleChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsUserInterfaceScaleChanged));
		EventManager.AddEventListener("Event_OnSettingsChatOpacityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsChatOpacityChanged));
		EventManager.AddEventListener("Event_OnSettingsChatScaleChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsChatScaleChanged));
		EventManager.AddEventListener("Event_OnSettingsMinimapOpacityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsMinimapOpacityChanged));
		EventManager.AddEventListener("Event_OnSettingsMinimapBackgroundOpacityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsMinimapBackgroundOpacityChanged));
		EventManager.AddEventListener("Event_OnSettingsMinimapHorizontalPositionChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsMinimapHorizontalPositionChanged));
		EventManager.AddEventListener("Event_OnSettingsMinimapVerticalPositionChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsMinimapVerticalPositionChanged));
		EventManager.AddEventListener("Event_OnSettingsMinimapScaleChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsMinimapScaleChanged));
		EventManager.AddEventListener("Event_OnSettingsGlobalStickSensitivityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsGlobalStickSensitivityChanged));
		EventManager.AddEventListener("Event_OnSettingsHorizontalStickSensitivityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsHorizontalStickSensitivityChanged));
		EventManager.AddEventListener("Event_OnSettingsVerticalStickSensitivityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsVerticalStickSensitivityChanged));
		EventManager.AddEventListener("Event_OnSettingsLookSensitivityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsLookSensitivityChanged));
		EventManager.AddEventListener("Event_OnSettingsGlobalVolumeChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsGlobalVolumeChanged));
		EventManager.AddEventListener("Event_OnSettingsAmbientVolumeChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsAmbientVolumeChanged));
		EventManager.AddEventListener("Event_OnSettingsGameVolumeChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsGameVolumeChanged));
		EventManager.AddEventListener("Event_OnSettingsVoiceVolumeChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsVoiceVolumeChanged));
		EventManager.AddEventListener("Event_OnSettingsUIVolumeChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsUIVolumeChanged));
		EventManager.AddEventListener("Event_OnSettingsFullScreenModeChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsFullScreenModeChanged));
		EventManager.AddEventListener("Event_OnSettingsDisplayChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsDisplayChanged));
		EventManager.AddEventListener("Event_OnSettingsResolutionChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsResolutionChanged));
		EventManager.AddEventListener("Event_OnSettingsVSyncChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsVSyncChanged));
		EventManager.AddEventListener("Event_OnSettingsFpsLimitChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsFpsLimitChanged));
		EventManager.AddEventListener("Event_OnSettingsFovChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsFovChanged));
		EventManager.AddEventListener("Event_OnSettingsQualityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsQualityChanged));
		EventManager.AddEventListener("Event_OnSettingsShadowQualityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShadowQualityChanged));
		EventManager.AddEventListener("Event_OnSettingsMotionBlurChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsMotionBlurChanged));
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x000336C0 File Offset: 0x000318C0
	private static void AddAppearanceEventListeners()
	{
		EventManager.AddEventListener("Event_OnAppearanceTeamChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnAppearanceTeamChanged));
		EventManager.AddEventListener("Event_OnAppearanceRoleChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnAppearanceRoleChanged));
		EventManager.AddEventListener("Event_OnAppearanceApplyForBothTeamsChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnAppearanceApplyForBothTeamsChanged));
		EventManager.AddEventListener("Event_OnAppearanceClickItem", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnAppearanceClickItem));
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x00033728 File Offset: 0x00031928
	private static void RemoveSettingsEventListeners()
	{
		EventManager.RemoveEventListener("Event_OnSettingsCameraAngleChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsCameraAngleChanged));
		EventManager.RemoveEventListener("Event_OnSettingsHandednessChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsHandednessChanged));
		EventManager.RemoveEventListener("Event_OnSettingsShowPuckSilhouetteChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowPuckSilhouetteChanged));
		EventManager.RemoveEventListener("Event_OnSettingsShowTeamColorBarChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowTeamColorBarChanged));
		EventManager.RemoveEventListener("Event_OnSettingsShowPuckOutlineChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowPuckOutlineChanged));
		EventManager.RemoveEventListener("Event_OnSettingsShowPuckElevationChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowPuckElevationChanged));
		EventManager.RemoveEventListener("Event_OnSettingsShowMinimapSticksChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowMinimapSticksChanged));
		EventManager.RemoveEventListener("Event_OnSettingsShowMinimapPuckElevationChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowMinimapPuckElevationChanged));
		EventManager.RemoveEventListener("Event_OnSettingsShowMinimapFallenIndicatorChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowMinimapFallenIndicatorChanged));
		EventManager.RemoveEventListener("Event_OnSettingsShowPlayerUsernamesChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowPlayerUsernamesChanged));
		EventManager.RemoveEventListener("Event_OnSettingsPlayerUsernamesFadeThresholdChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsPlayerUsernamesFadeThresholdChanged));
		EventManager.RemoveEventListener("Event_OnSettingsMaxMatchmakingPingChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsMaxMatchmakingPingChanged));
		EventManager.RemoveEventListener("Event_OnSettingsNetworkBufferingChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsNetworkBufferingChanged));
		EventManager.RemoveEventListener("Event_OnSettingsFilterChatProfanityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsFilterChatProfanityChanged));
		EventManager.RemoveEventListener("Event_OnSettingsUnitsChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsUnitsChanged));
		EventManager.RemoveEventListener("Event_OnSettingsShowGameUserInterfaceChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShowGameUserInterfaceChanged));
		EventManager.RemoveEventListener("Event_OnSettingsUserInterfaceScaleChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsUserInterfaceScaleChanged));
		EventManager.RemoveEventListener("Event_OnSettingsChatOpacityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsChatOpacityChanged));
		EventManager.RemoveEventListener("Event_OnSettingsChatScaleChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsChatScaleChanged));
		EventManager.RemoveEventListener("Event_OnSettingsMinimapOpacityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsMinimapOpacityChanged));
		EventManager.RemoveEventListener("Event_OnSettingsMinimapBackgroundOpacityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsMinimapBackgroundOpacityChanged));
		EventManager.RemoveEventListener("Event_OnSettingsMinimapHorizontalPositionChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsMinimapHorizontalPositionChanged));
		EventManager.RemoveEventListener("Event_OnSettingsMinimapVerticalPositionChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsMinimapVerticalPositionChanged));
		EventManager.RemoveEventListener("Event_OnSettingsMinimapScaleChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsMinimapScaleChanged));
		EventManager.RemoveEventListener("Event_OnSettingsGlobalStickSensitivityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsGlobalStickSensitivityChanged));
		EventManager.RemoveEventListener("Event_OnSettingsHorizontalStickSensitivityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsHorizontalStickSensitivityChanged));
		EventManager.RemoveEventListener("Event_OnSettingsVerticalStickSensitivityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsVerticalStickSensitivityChanged));
		EventManager.RemoveEventListener("Event_OnSettingsLookSensitivityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsLookSensitivityChanged));
		EventManager.RemoveEventListener("Event_OnSettingsGlobalVolumeChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsGlobalVolumeChanged));
		EventManager.RemoveEventListener("Event_OnSettingsAmbientVolumeChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsAmbientVolumeChanged));
		EventManager.RemoveEventListener("Event_OnSettingsGameVolumeChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsGameVolumeChanged));
		EventManager.RemoveEventListener("Event_OnSettingsVoiceVolumeChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsVoiceVolumeChanged));
		EventManager.RemoveEventListener("Event_OnSettingsUIVolumeChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsUIVolumeChanged));
		EventManager.RemoveEventListener("Event_OnSettingsFullScreenModeChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsFullScreenModeChanged));
		EventManager.RemoveEventListener("Event_OnSettingsDisplayChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsDisplayChanged));
		EventManager.RemoveEventListener("Event_OnSettingsResolutionChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsResolutionChanged));
		EventManager.RemoveEventListener("Event_OnSettingsVSyncChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsVSyncChanged));
		EventManager.RemoveEventListener("Event_OnSettingsFpsLimitChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsFpsLimitChanged));
		EventManager.RemoveEventListener("Event_OnSettingsFovChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsFovChanged));
		EventManager.RemoveEventListener("Event_OnSettingsQualityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsQualityChanged));
		EventManager.RemoveEventListener("Event_OnSettingsShadowQualityChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsShadowQualityChanged));
		EventManager.RemoveEventListener("Event_OnSettingsMotionBlurChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnSettingsMotionBlurChanged));
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x00033AD4 File Offset: 0x00031CD4
	private static void RemoveAppearanceEventListeners()
	{
		EventManager.RemoveEventListener("Event_OnAppearanceTeamChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnAppearanceTeamChanged));
		EventManager.RemoveEventListener("Event_OnAppearanceRoleChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnAppearanceRoleChanged));
		EventManager.RemoveEventListener("Event_OnAppearanceApplyForBothTeamsChanged", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnAppearanceApplyForBothTeamsChanged));
		EventManager.RemoveEventListener("Event_OnAppearanceClickItem", new Action<Dictionary<string, object>>(SettingsManagerController.Event_OnAppearanceClickItem));
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x00033B39 File Offset: 0x00031D39
	private static DebugMode GetNextDebugMode(DebugMode debugMode)
	{
		if (debugMode == DebugMode.Off)
		{
			return DebugMode.Simple;
		}
		if (debugMode != DebugMode.Simple)
		{
			return DebugMode.Off;
		}
		return DebugMode.Advanced;
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x00033B49 File Offset: 0x00031D49
	private static void OnDebug1ActionPerformed(InputAction.CallbackContext context)
	{
		SettingsManager.UpdateDebug(SettingsManagerController.GetNextDebugMode(SettingsManager.Debug));
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x00033B5A File Offset: 0x00031D5A
	private static void Event_OnSettingsCameraAngleChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateCameraAngle((float)message["value"]);
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x00033B71 File Offset: 0x00031D71
	private static void Event_OnSettingsHandednessChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateHandedness(Utils.GetHandednessFromName((string)message["value"]));
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x00033B8D File Offset: 0x00031D8D
	private static void Event_OnSettingsShowPuckSilhouetteChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateShowPuckSilhouette((bool)message["value"]);
	}

	// Token: 0x06000AB5 RID: 2741 RVA: 0x00033BA4 File Offset: 0x00031DA4
	private static void Event_OnSettingsShowTeamColorBarChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateShowTeamColorBar((bool)message["value"]);
	}

	// Token: 0x06000AB6 RID: 2742 RVA: 0x00033BBB File Offset: 0x00031DBB
	private static void Event_OnSettingsShowPuckOutlineChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateShowPuckOutline((bool)message["value"]);
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x00033BD2 File Offset: 0x00031DD2
	private static void Event_OnSettingsShowPuckElevationChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateShowPuckElevation((bool)message["value"]);
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x00033BE9 File Offset: 0x00031DE9
	private static void Event_OnSettingsShowMinimapSticksChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateShowMinimapSticks((bool)message["value"]);
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x00033C00 File Offset: 0x00031E00
	private static void Event_OnSettingsShowMinimapPuckElevationChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateShowMinimapPuckElevation((bool)message["value"]);
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x00033C17 File Offset: 0x00031E17
	private static void Event_OnSettingsShowMinimapFallenIndicatorChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateShowMinimapFallenIndicator((bool)message["value"]);
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x00033C2E File Offset: 0x00031E2E
	private static void Event_OnSettingsShowPlayerUsernamesChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateShowPlayerUsernames((bool)message["value"]);
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x00033C45 File Offset: 0x00031E45
	private static void Event_OnSettingsPlayerUsernamesFadeThresholdChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdatePlayerUsernamesFadeThreshold((float)message["value"]);
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x00033C5C File Offset: 0x00031E5C
	private static void Event_OnSettingsMaxMatchmakingPingChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateMaxMatchmakingPing((int)message["value"]);
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x00033C73 File Offset: 0x00031E73
	private static void Event_OnSettingsNetworkBufferingChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateNetworkBuffering(Utils.GetNetworkBufferingFromName((string)message["value"]));
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x00033C8F File Offset: 0x00031E8F
	private static void Event_OnSettingsFilterChatProfanityChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateFilterChatProfanity((bool)message["value"]);
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x00033CA6 File Offset: 0x00031EA6
	private static void Event_OnSettingsUnitsChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateUnits(Utils.GetUnitsFromName((string)message["value"]));
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x00033CC2 File Offset: 0x00031EC2
	private static void Event_OnSettingsShowGameUserInterfaceChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateShowGameUserInterface((bool)message["value"]);
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x00033CD9 File Offset: 0x00031ED9
	private static void Event_OnSettingsUserInterfaceScaleChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateUserInterfaceScale((float)message["value"]);
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x00033CF0 File Offset: 0x00031EF0
	private static void Event_OnSettingsChatOpacityChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateChatOpacity((float)message["value"]);
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x00033D07 File Offset: 0x00031F07
	private static void Event_OnSettingsChatScaleChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateChatScale((float)message["value"]);
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x00033D1E File Offset: 0x00031F1E
	private static void Event_OnSettingsMinimapOpacityChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateMinimapOpacity((float)message["value"]);
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x00033D35 File Offset: 0x00031F35
	private static void Event_OnSettingsMinimapBackgroundOpacityChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateMinimapBackgroundOpacity((float)message["value"]);
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x00033D4C File Offset: 0x00031F4C
	private static void Event_OnSettingsMinimapHorizontalPositionChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateMinimapHorizontalPosition((float)message["value"]);
	}

	// Token: 0x06000AC8 RID: 2760 RVA: 0x00033D63 File Offset: 0x00031F63
	private static void Event_OnSettingsMinimapVerticalPositionChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateMinimapVerticalPosition((float)message["value"]);
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x00033D7A File Offset: 0x00031F7A
	private static void Event_OnSettingsMinimapScaleChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateMinimapScale((float)message["value"]);
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x00033D91 File Offset: 0x00031F91
	private static void Event_OnSettingsGlobalStickSensitivityChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateGlobalStickSensitivity((float)message["value"]);
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x00033DA8 File Offset: 0x00031FA8
	private static void Event_OnSettingsHorizontalStickSensitivityChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateHorizontalStickSensitivity((float)message["value"]);
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x00033DBF File Offset: 0x00031FBF
	private static void Event_OnSettingsVerticalStickSensitivityChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateVerticalStickSensitivity((float)message["value"]);
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x00033DD6 File Offset: 0x00031FD6
	private static void Event_OnSettingsLookSensitivityChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateLookSensitivity((float)message["value"]);
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x00033DED File Offset: 0x00031FED
	private static void Event_OnSettingsGlobalVolumeChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateGlobalVolume((float)message["value"]);
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x00033E04 File Offset: 0x00032004
	private static void Event_OnSettingsAmbientVolumeChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateAmbientVolume((float)message["value"]);
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x00033E1B File Offset: 0x0003201B
	private static void Event_OnSettingsGameVolumeChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateGameVolume((float)message["value"]);
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x00033E32 File Offset: 0x00032032
	private static void Event_OnSettingsVoiceVolumeChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateVoiceVolume((float)message["value"]);
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x00033E49 File Offset: 0x00032049
	private static void Event_OnSettingsUIVolumeChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateUIVolume((float)message["value"]);
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x00033E60 File Offset: 0x00032060
	private static void Event_OnSettingsFullScreenModeChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateFullScreenMode(Utils.GetFullScreenModeFromName((string)message["value"]));
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x00033E7C File Offset: 0x0003207C
	private static void Event_OnSettingsDisplayChanged(Dictionary<string, object> message)
	{
		string text = (string)message["value"];
		int displayIndexFromName = Utils.GetDisplayIndexFromName(text);
		if (displayIndexFromName == -1)
		{
			SettingsManagerController.Logger.Warning("Could not find display index for display name " + text + ", skipping update");
			return;
		}
		SettingsManager.UpdateDisplayIndex(displayIndexFromName);
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x00033EC8 File Offset: 0x000320C8
	private static void Event_OnSettingsResolutionChanged(Dictionary<string, object> message)
	{
		string text = (string)message["value"];
		int resolutionIndexFromName = Utils.GetResolutionIndexFromName(text);
		if (resolutionIndexFromName == -1)
		{
			SettingsManagerController.Logger.Warning("Could not find resolution index for resolution name " + text + ", skipping update");
			return;
		}
		SettingsManager.UpdateResolutionIndex(resolutionIndexFromName);
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x00033F12 File Offset: 0x00032112
	private static void Event_OnSettingsVSyncChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateVSync((bool)message["value"]);
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x00033F29 File Offset: 0x00032129
	private static void Event_OnSettingsFpsLimitChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateFpsLimit((int)message["value"]);
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x00033F40 File Offset: 0x00032140
	private static void Event_OnSettingsFovChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateFov((float)message["value"]);
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x00033F57 File Offset: 0x00032157
	private static void Event_OnSettingsQualityChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateQuality(Utils.GetApplicationQualityFromName((string)message["value"]));
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x00033F73 File Offset: 0x00032173
	private static void Event_OnSettingsShadowQualityChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateShadowQuality(Utils.GetShadowQualityFromName((string)message["value"]));
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x00033F8F File Offset: 0x0003218F
	private static void Event_OnSettingsMotionBlurChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateMotionBlur((bool)message["value"]);
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x00033FA6 File Offset: 0x000321A6
	private static void Event_OnAppearanceTeamChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateTeam(Utils.GetTeamFromName((string)message["value"]));
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x00033FC2 File Offset: 0x000321C2
	private static void Event_OnAppearanceRoleChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateRole(Utils.GetRoleFromName((string)message["value"]));
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x00033FDE File Offset: 0x000321DE
	private static void Event_OnAppearanceApplyForBothTeamsChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateApplyForBothTeams((bool)message["value"]);
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x00033FF8 File Offset: 0x000321F8
	private static void Event_OnAppearanceClickItem(Dictionary<string, object> message)
	{
		Item item = (Item)message["item"];
		AppearanceCategory appearanceCategory = (AppearanceCategory)message["category"];
		AppearanceSubcategory appearanceSubcategory = (AppearanceSubcategory)message["subcategory"];
		PlayerTeam playerTeam = (PlayerTeam)message["team"];
		PlayerRole role = (PlayerRole)message["role"];
		if (!item.IsOwned)
		{
			return;
		}
		PlayerTeam team = (playerTeam == PlayerTeam.Blue) ? PlayerTeam.Red : PlayerTeam.Blue;
		switch (appearanceSubcategory)
		{
		case AppearanceSubcategory.Headgear:
			SettingsManager.UpdateHeadgearID(playerTeam, role, item.id);
			if (SettingsManager.ApplyForBothTeams)
			{
				SettingsManager.UpdateHeadgearID(team, role, item.id);
				return;
			}
			break;
		case AppearanceSubcategory.Flags:
			SettingsManager.UpdateFlagID(item.id);
			return;
		case AppearanceSubcategory.Mustaches:
			SettingsManager.UpdateMustacheID(item.id);
			return;
		case AppearanceSubcategory.Beards:
			SettingsManager.UpdateBeardID(item.id);
			return;
		case AppearanceSubcategory.Jerseys:
			SettingsManager.UpdateJerseyID(playerTeam, role, item.id);
			if (SettingsManager.ApplyForBothTeams)
			{
				SettingsManager.UpdateJerseyID(team, role, item.id);
				return;
			}
			break;
		case AppearanceSubcategory.StickSkins:
			SettingsManager.UpdateStickSkinID(playerTeam, role, item.id);
			if (SettingsManager.ApplyForBothTeams)
			{
				SettingsManager.UpdateStickSkinID(team, role, item.id);
				return;
			}
			break;
		case AppearanceSubcategory.StickShaftTapes:
			SettingsManager.UpdateStickShaftTapeID(playerTeam, role, item.id);
			if (SettingsManager.ApplyForBothTeams)
			{
				SettingsManager.UpdateStickShaftTapeID(team, role, item.id);
				return;
			}
			break;
		case AppearanceSubcategory.StickBladeTapes:
			SettingsManager.UpdateStickBladeTapeID(playerTeam, role, item.id);
			if (SettingsManager.ApplyForBothTeams)
			{
				SettingsManager.UpdateStickBladeTapeID(team, role, item.id);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x00034169 File Offset: 0x00032369
	private static void Event_OnDisplayIndexChanged(Dictionary<string, object> message)
	{
		SettingsManager.UpdateResolutionIndex(-1);
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x00034171 File Offset: 0x00032371
	private static void Event_OnIsDisplayChangeInProgressChanged(Dictionary<string, object> message)
	{
		if ((bool)message["isDisplayChangeInProgress"])
		{
			return;
		}
		if (SettingsManager.ResolutionIndex == -1)
		{
			SettingsManager.UpdateResolutionIndex(Utils.GetResolutions().Count - 1);
		}
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x0003419F File Offset: 0x0003239F
	private static void Event_OnBaseCameraEnabled(Dictionary<string, object> message)
	{
		((BaseCamera)message["baseCamera"]).SetFieldOfView(SettingsManager.Fov);
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x000341BB File Offset: 0x000323BB
	private static void Event_OnPopupClickOk(Dictionary<string, object> message)
	{
		if (((Popup)message["popup"]).Name == "settingsResetToDefault")
		{
			SettingsManager.ResetToDefault();
			EventManager.TriggerEvent("Event_OnSettingsResetToDefault", null);
		}
	}

	// Token: 0x04000694 RID: 1684
	private static readonly Logger Logger = new Logger("SettingsManagerController");
}
