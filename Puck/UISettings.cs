using System;
using System.Collections.Generic;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x02000212 RID: 530
public class UISettings : UIView
{
	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06001061 RID: 4193 RVA: 0x0004DA08 File Offset: 0x0004BC08
	public bool IsAudioTabActive
	{
		get
		{
			return this.tabView != null && this.tabView.activeTab == this.audioTab;
		}
	}

	// Token: 0x06001062 RID: 4194 RVA: 0x0004DA28 File Offset: 0x0004BC28
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("SettingsView", null);
		this.settings = base.View.Query("Settings", null);
		this.tabView = this.settings.Query(null, null);
		this.audioTab = this.settings.Query("AudioTab", null);
		this.tabView.activeTabChanged += this.OnActiveTabChangedInternal;
		this.micTestFill = this.settings.Query("MicTestFill", null);
		this.closeIconButton = this.settings.Query("CloseIconButtonContainer", null).First().Query(null, null);
		this.closeIconButton.clicked += this.OnClickClose;
		this.cameraAngleSlider = this.settings.Query("CameraAngleSlider", null).First().Query(null, null);
		this.cameraAngleSlider.value = SettingsManager.CameraAngle;
		this.cameraAngleSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnCameraAngleChanged));
		this.handednessDropdown = this.settings.Query("HandednessDropdown", null).First().Query(null, null);
		this.handednessDropdown.value = Utils.GetNameFromHandedness(SettingsManager.Handedness);
		this.handednessDropdown.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnHandednessChanged));
		this.showPuckSilhouetteToggle = this.settings.Query("ShowPuckSilhouetteToggle", null).First().Query(null, null);
		this.showPuckSilhouetteToggle.value = SettingsManager.ShowPuckSilhouette;
		this.showPuckSilhouetteToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnShowPuckSilhouetteChanged));
		this.showTeamColorBarToggle = this.settings.Query("ShowTeamColorBarToggle", null).First().Query(null, null);
		this.showTeamColorBarToggle.value = SettingsManager.ShowTeamColorBar;
		this.showTeamColorBarToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnShowTeamColorBarChanged));
		this.showPuckOutlineToggle = this.settings.Query("ShowPuckOutlineToggle", null).First().Query(null, null);
		this.showPuckOutlineToggle.value = SettingsManager.ShowPuckOutline;
		this.showPuckOutlineToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnShowPuckOutlineChanged));
		this.showPuckElevationToggle = this.settings.Query("ShowPuckElevationToggle", null).First().Query(null, null);
		this.showPuckElevationToggle.value = SettingsManager.ShowPuckElevation;
		this.showPuckElevationToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnShowPuckEleveationChanged));
		this.showPlayerUsernamesToggle = this.settings.Query("ShowPlayerUsernamesToggle", null).First().Query(null, null);
		this.showPlayerUsernamesToggle.value = SettingsManager.ShowPlayerUsernames;
		this.showPlayerUsernamesToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnShowPlayerUsernamesChanged));
		this.playerUsernamesFadeThresholdSlider = this.settings.Query("PlayerUsernamesFadeThresholdSlider", null).First().Query(null, null);
		this.playerUsernamesFadeThresholdSlider.value = SettingsManager.PlayerUsernamesFadeThreshold;
		this.playerUsernamesFadeThresholdSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnPlayerUsernamesFadeThresholdChanged));
		this.maxMatchmakingPingSliderInt = this.settings.Query("MaxMatchmakingPingSliderInt", null).First().Query(null, null);
		this.maxMatchmakingPingSliderInt.value = SettingsManager.MaxMatchmakingPing;
		this.maxMatchmakingPingSliderInt.RegisterValueChangedCallback(new EventCallback<ChangeEvent<int>>(this.OnMaxMatchmakingPingChanged));
		this.networkBufferingDropdown = this.settings.Query("NetworkBufferingDropdown", null).First().Query(null, null);
		this.networkBufferingDropdown.value = Utils.GetNameFromNetworkBuffering(SettingsManager.NetworkBuffering);
		this.networkBufferingDropdown.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnNetworkBufferingChanged));
		this.filterChatProfanityToggle = this.settings.Query("FilterChatProfanityToggle", null).First().Query(null, null);
		this.filterChatProfanityToggle.value = SettingsManager.FilterChatProfanity;
		this.filterChatProfanityToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnFilterChatProfanityChanged));
		this.unitsDropdown = this.settings.Query("UnitsDropdown", null).First().Query(null, null);
		this.unitsDropdown.value = Utils.GetNameFromUnits(SettingsManager.Units);
		this.unitsDropdown.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnUnitsChanged));
		this.showGameUserInterfaceToggle = this.settings.Query("ShowGameUserInterfaceToggle", null).First().Query(null, null);
		this.showGameUserInterfaceToggle.value = SettingsManager.ShowGameUserInterface;
		this.showGameUserInterfaceToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnShowGameUserInterfaceChanged));
		this.userInterfaceScaleSlider = this.settings.Query("UserInterfaceScaleSlider", null).First().Query(null, null);
		this.userInterfaceScaleSlider.value = SettingsManager.UserInterfaceScale;
		this.userInterfaceScaleSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnUserInterfaceScaleChanged));
		this.chatOpacitySlider = this.settings.Query("ChatOpacitySlider", null).First().Query(null, null);
		this.chatOpacitySlider.value = SettingsManager.ChatOpacity;
		this.chatOpacitySlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnChatOpacityChanged));
		this.chatScaleSlider = this.settings.Query("ChatScaleSlider", null).First().Query(null, null);
		this.chatScaleSlider.value = SettingsManager.ChatScale;
		this.chatScaleSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnChatScaleChanged));
		this.minimapOpacitySlider = this.settings.Query("MinimapOpacitySlider", null).First().Query(null, null);
		this.minimapOpacitySlider.value = SettingsManager.MinimapOpacity;
		this.minimapOpacitySlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnMinimapOpacityChanged));
		this.minimapBackgroundOpacitySlider = this.settings.Query("MinimapBackgroundOpacitySlider", null).First().Query(null, null);
		this.minimapBackgroundOpacitySlider.value = SettingsManager.MinimapBackgroundOpacity;
		this.minimapBackgroundOpacitySlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnMinimapBackgroundOpacityChanged));
		this.minimapHorizontalPositionSlider = this.settings.Query("MinimapHorizontalPositionSlider", null).First().Query(null, null);
		this.minimapHorizontalPositionSlider.value = SettingsManager.MinimapHorizontalPosition;
		this.minimapHorizontalPositionSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnMinimapHorizontalPositionChanged));
		this.minimapVerticalPositionSlider = this.settings.Query("MinimapVerticalPositionSlider", null).First().Query(null, null);
		this.minimapVerticalPositionSlider.value = SettingsManager.MinimapVerticalPosition;
		this.minimapVerticalPositionSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnMinimapVerticalPositionChanged));
		this.minimapScaleSlider = this.settings.Query("MinimapScaleSlider", null).First().Query(null, null);
		this.minimapScaleSlider.value = SettingsManager.MinimapScale;
		this.minimapScaleSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnMinimapScaleChanged));
		this.showMinimapSticksToggle = this.settings.Query("ShowMinimapSticksToggle", null).First().Query(null, null);
		this.showMinimapSticksToggle.value = SettingsManager.ShowMinimapSticks;
		this.showMinimapSticksToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnShowMinimapSticksChanged));
		this.showMinimapPuckElevationToggle = this.settings.Query("ShowMinimapPuckElevationToggle", null).First().Query(null, null);
		this.showMinimapPuckElevationToggle.value = SettingsManager.ShowMinimapPuckElevation;
		this.showMinimapPuckElevationToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnShowMinimapPuckElevationChanged));
		this.showMinimapFallenIndicatorToggle = this.settings.Query("ShowMinimapFallenIndicatorToggle", null).First().Query(null, null);
		this.showMinimapFallenIndicatorToggle.value = SettingsManager.ShowMinimapFallenIndicator;
		this.showMinimapFallenIndicatorToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnShowMinimapFallenIndicatorChanged));
		this.globalStickSensitivitySlider = this.settings.Query("GlobalStickSensitivitySlider", null).First().Query(null, null);
		this.globalStickSensitivitySlider.value = SettingsManager.GlobalStickSensitivity;
		this.globalStickSensitivitySlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnGlobalStickSensitivityChanged));
		this.horizontalStickSensitivitySlider = this.settings.Query("HorizontalStickSensitivitySlider", null).First().Query(null, null);
		this.horizontalStickSensitivitySlider.value = SettingsManager.HorizontalStickSensitivity;
		this.horizontalStickSensitivitySlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnHorizontalStickSensitivityChanged));
		this.verticalStickSensitivitySlider = this.settings.Query("VerticalStickSensitivitySlider", null).First().Query(null, null);
		this.verticalStickSensitivitySlider.value = SettingsManager.VerticalStickSensitivity;
		this.verticalStickSensitivitySlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnVerticalStickSensitivityChanged));
		this.lookSensitivitySlider = this.settings.Query("LookSensitivitySlider", null).First().Query(null, null);
		this.lookSensitivitySlider.value = SettingsManager.LookSensitivity;
		this.lookSensitivitySlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnLookSensitivityChanged));
		this.actionNameKeyBindFieldMap = new Dictionary<string, KeyBindField>
		{
			{
				"Move Forward",
				this.settings.Query("MoveForwardKeyBindInput", null).First().Query(null, null)
			},
			{
				"Move Backward",
				this.settings.Query("MoveBackwardKeyBindInput", null).First().Query(null, null)
			},
			{
				"Turn Left",
				this.settings.Query("TurnLeftKeyBindInput", null).First().Query(null, null)
			},
			{
				"Turn Right",
				this.settings.Query("TurnRightKeyBindInput", null).First().Query(null, null)
			},
			{
				"Blade Angle Up",
				this.settings.Query("BladeAngleUpKeyBindInput", null).First().Query(null, null)
			},
			{
				"Blade Angle Down",
				this.settings.Query("BladeAngleDownKeyBindInput", null).First().Query(null, null)
			},
			{
				"Slide",
				this.settings.Query("SlideKeyBindInput", null).First().Query(null, null)
			},
			{
				"Sprint",
				this.settings.Query("SprintKeyBindInput", null).First().Query(null, null)
			},
			{
				"Track",
				this.settings.Query("TrackKeyBindInput", null).First().Query(null, null)
			},
			{
				"Look",
				this.settings.Query("LookKeyBindInput", null).First().Query(null, null)
			},
			{
				"Jump",
				this.settings.Query("JumpKeyBindInput", null).First().Query(null, null)
			},
			{
				"Stop",
				this.settings.Query("StopKeyBindInput", null).First().Query(null, null)
			},
			{
				"Twist Left",
				this.settings.Query("TwistLeftKeyBindInput", null).First().Query(null, null)
			},
			{
				"Twist Right",
				this.settings.Query("TwistRightKeyBindInput", null).First().Query(null, null)
			},
			{
				"Dash Left",
				this.settings.Query("DashLeftKeyBindInput", null).First().Query(null, null)
			},
			{
				"Dash Right",
				this.settings.Query("DashRightKeyBindInput", null).First().Query(null, null)
			},
			{
				"Extend Left",
				this.settings.Query("ExtendLeftKeyBindInput", null).First().Query(null, null)
			},
			{
				"Extend Right",
				this.settings.Query("ExtendRightKeyBindInput", null).First().Query(null, null)
			},
			{
				"Lateral Left",
				this.settings.Query("LateralLeftKeyBindInput", null).First().Query(null, null)
			},
			{
				"Lateral Right",
				this.settings.Query("LateralRightKeyBindInput", null).First().Query(null, null)
			},
			{
				"Talk",
				this.settings.Query("TalkKeyBindInput", null).First().Query(null, null)
			},
			{
				"All Chat",
				this.settings.Query("AllChatKeyBindInput", null).First().Query(null, null)
			},
			{
				"Team Chat",
				this.settings.Query("TeamChatKeyBindInput", null).First().Query(null, null)
			},
			{
				"Position Select",
				this.settings.Query("PositionSelectKeyBindInput", null).First().Query(null, null)
			},
			{
				"Scoreboard",
				this.settings.Query("ScoreboardKeyBindInput", null).First().Query(null, null)
			},
			{
				"Debug 1",
				this.settings.Query("DebugOverlayKeyBindInput", null).First().Query(null, null)
			}
		};
		this.UpdateKeyBindInputs(InputManager.KeyBinds);
		foreach (KeyValuePair<string, KeyBindField> keyValuePair in this.actionNameKeyBindFieldMap)
		{
			string actionName = keyValuePair.Key;
			KeyBindField value = keyValuePair.Value;
			value.Click = delegate()
			{
				this.OnKeyBindInputClicked(actionName);
			};
			value.InteractionChange = delegate(KeyBindInteraction interaction)
			{
				this.OnKeyBindInputInteractionChanged(actionName, interaction);
			};
		}
		this.globalVolumeSlider = this.settings.Query("GlobalVolumeSlider", null).First().Query(null, null);
		this.globalVolumeSlider.value = SettingsManager.GlobalVolume;
		this.globalVolumeSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnGlobalVolumeChanged));
		this.ambientVolumeSlider = this.settings.Query("AmbientVolumeSlider", null).First().Query(null, null);
		this.ambientVolumeSlider.value = SettingsManager.AmbientVolume;
		this.ambientVolumeSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnAmbientVolumeChanged));
		this.gameVolumeSlider = this.settings.Query("GameVolumeSlider", null).First().Query(null, null);
		this.gameVolumeSlider.value = SettingsManager.GameVolume;
		this.gameVolumeSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnGameVolumeChanged));
		this.voiceVolumeSlider = this.settings.Query("VoiceVolumeSlider", null).First().Query(null, null);
		this.voiceVolumeSlider.value = SettingsManager.VoiceVolume;
		this.voiceVolumeSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnVoiceVolumeChanged));
		this.uiVolumeSlider = this.settings.Query("UIVolumeSlider", null).First().Query(null, null);
		this.uiVolumeSlider.value = SettingsManager.UIVolume;
		this.uiVolumeSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnUIVolumeChanged));
		this.fullScreenModeDropdown = this.settings.Query("FullScreenModeDropdown", null).First().Query(null, null);
		this.fullScreenModeDropdown.choices = Utils.GetFullScreenModeNames();
		this.fullScreenModeDropdown.value = Utils.GetNameFromFullScreenMode(SettingsManager.FullScreenMode);
		this.fullScreenModeDropdown.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnFullScreenModeChanged));
		this.displayDropdown = this.settings.Query("DisplayDropdown", null).First().Query(null, null);
		this.displayDropdown.choices = Utils.GetDisplayNames();
		this.displayDropdown.value = Utils.GetDisplayNameFromIndex(SettingsManager.DisplayIndex);
		this.displayDropdown.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnDisplayChanged));
		this.resolutionDropdown = this.settings.Query("ResolutionDropdown", null).First().Query(null, null);
		this.resolutionDropdown.choices = Utils.GetResolutionNames();
		this.resolutionDropdown.value = Utils.GetResolutionNameFromIndex(SettingsManager.ResolutionIndex);
		this.resolutionDropdown.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnResolutionChanged));
		this.vSyncToggle = this.settings.Query("VSyncToggle", null).First().Query(null, null);
		this.vSyncToggle.value = SettingsManager.VSync;
		this.vSyncToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnVSyncChanged));
		this.fpsLimitSlider = this.settings.Query("FPSLimitSlider", null).First().Query(null, null);
		this.fpsLimitSlider.value = (float)SettingsManager.FpsLimit;
		this.fpsLimitSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnFpsLimitChanged));
		this.fovSlider = this.settings.Query("FOVSlider", null).First().Query(null, null);
		this.fovSlider.value = SettingsManager.Fov;
		this.fovSlider.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnFovChanged));
		this.qualityDropdown = this.settings.Query("QualityDropdown", null).First().Query(null, null);
		this.qualityDropdown.choices = Utils.GetApplicationQualityNames();
		this.qualityDropdown.value = Utils.GetNameFromApplicationQuality(SettingsManager.Quality);
		this.qualityDropdown.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnQualityChanged));
		this.shadowQualityDropdown = this.settings.Query("ShadowQualityDropdown", null).First().Query(null, null);
		this.shadowQualityDropdown.choices = Utils.GetShadowQualityNames();
		this.shadowQualityDropdown.value = Utils.GetNameFromShadowQuality(SettingsManager.ShadowQuality);
		this.shadowQualityDropdown.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnShadowQualityChanged));
		this.motionBlurToggle = this.settings.Query("MotionBlurToggle", null).First().Query(null, null);
		this.motionBlurToggle.value = SettingsManager.MotionBlur;
		this.motionBlurToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnMotionBlurChanged));
		this.resetToDefaultButton = this.settings.Query("ResetToDefaultButton", null);
		this.resetToDefaultButton.clicked += this.OnClickResetToDefault;
	}

	// Token: 0x06001063 RID: 4195 RVA: 0x0004EEDC File Offset: 0x0004D0DC
	private void OnClickClose()
	{
		EventManager.TriggerEvent("Event_OnSettingsClickClose", null);
	}

	// Token: 0x06001064 RID: 4196 RVA: 0x0004EEE9 File Offset: 0x0004D0E9
	private void OnClickResetToDefault()
	{
		EventManager.TriggerEvent("Event_OnSettingsClickResetToDefault", null);
	}

	// Token: 0x06001065 RID: 4197 RVA: 0x0004EEF8 File Offset: 0x0004D0F8
	public void UpdateKeyBindInputs(Dictionary<string, KeyBind> keyBinds)
	{
		foreach (KeyValuePair<string, KeyBind> keyValuePair in keyBinds)
		{
			string key = keyValuePair.Key;
			KeyBind value = keyValuePair.Value;
			if (this.actionNameKeyBindFieldMap.ContainsKey(key))
			{
				KeyBindField keyBindField = this.actionNameKeyBindFieldMap[key];
				keyBindField.Path = InputManager.GetKeyBindDisplayString(value);
				KeyBindInteraction keyBindInteractionFromInteraction = Utils.GetKeyBindInteractionFromInteraction(value.Interactions, keyBindField.InteractionType);
				keyBindField.Interaction = keyBindInteractionFromInteraction;
			}
		}
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x0004EF98 File Offset: 0x0004D198
	private void OnCameraAngleChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsCameraAngleChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x0004EFBF File Offset: 0x0004D1BF
	public void UpdateCameraAngle(float value)
	{
		this.cameraAngleSlider.value = value;
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x0004EFCD File Offset: 0x0004D1CD
	private void OnHandednessChanged(ChangeEvent<string> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsHandednessChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001069 RID: 4201 RVA: 0x0004EFEF File Offset: 0x0004D1EF
	public void UpdateHandedness(string value)
	{
		this.handednessDropdown.value = value;
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x0004EFFD File Offset: 0x0004D1FD
	private void OnShowPuckSilhouetteChanged(ChangeEvent<bool> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsShowPuckSilhouetteChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x0004F024 File Offset: 0x0004D224
	public void UpdateShowPuckSilhouette(bool value)
	{
		this.showPuckSilhouetteToggle.value = value;
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x0004F032 File Offset: 0x0004D232
	private void OnShowTeamColorBarChanged(ChangeEvent<bool> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsShowTeamColorBarChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x0600106D RID: 4205 RVA: 0x0004F059 File Offset: 0x0004D259
	public void UpdateShowTeamColorBar(bool value)
	{
		this.showTeamColorBarToggle.value = value;
	}

	// Token: 0x0600106E RID: 4206 RVA: 0x0004F067 File Offset: 0x0004D267
	private void OnShowPuckOutlineChanged(ChangeEvent<bool> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsShowPuckOutlineChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x0600106F RID: 4207 RVA: 0x0004F08E File Offset: 0x0004D28E
	public void UpdateShowPuckOutline(bool value)
	{
		this.showPuckOutlineToggle.value = value;
	}

	// Token: 0x06001070 RID: 4208 RVA: 0x0004F09C File Offset: 0x0004D29C
	private void OnShowPuckEleveationChanged(ChangeEvent<bool> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsShowPuckElevationChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001071 RID: 4209 RVA: 0x0004F0C3 File Offset: 0x0004D2C3
	public void UpdateShowPuckElevation(bool value)
	{
		this.showPuckElevationToggle.value = value;
	}

	// Token: 0x06001072 RID: 4210 RVA: 0x0004F0D1 File Offset: 0x0004D2D1
	private void OnShowMinimapSticksChanged(ChangeEvent<bool> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsShowMinimapSticksChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001073 RID: 4211 RVA: 0x0004F0F8 File Offset: 0x0004D2F8
	public void UpdateShowMinimapSticks(bool value)
	{
		this.showMinimapSticksToggle.value = value;
	}

	// Token: 0x06001074 RID: 4212 RVA: 0x0004F106 File Offset: 0x0004D306
	private void OnShowMinimapPuckElevationChanged(ChangeEvent<bool> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsShowMinimapPuckElevationChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x0004F12D File Offset: 0x0004D32D
	public void UpdateShowMinimapPuckElevation(bool value)
	{
		this.showMinimapPuckElevationToggle.value = value;
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x0004F13B File Offset: 0x0004D33B
	private void OnShowMinimapFallenIndicatorChanged(ChangeEvent<bool> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsShowMinimapFallenIndicatorChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x0004F162 File Offset: 0x0004D362
	public void UpdateShowMinimapFallenIndicator(bool value)
	{
		this.showMinimapFallenIndicatorToggle.value = value;
	}

	// Token: 0x06001078 RID: 4216 RVA: 0x0004F170 File Offset: 0x0004D370
	private void OnShowPlayerUsernamesChanged(ChangeEvent<bool> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsShowPlayerUsernamesChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001079 RID: 4217 RVA: 0x0004F197 File Offset: 0x0004D397
	public void UpdateShowPlayerUsernames(bool value)
	{
		this.showPlayerUsernamesToggle.value = value;
	}

	// Token: 0x0600107A RID: 4218 RVA: 0x0004F1A5 File Offset: 0x0004D3A5
	private void OnPlayerUsernamesFadeThresholdChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsPlayerUsernamesFadeThresholdChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x0600107B RID: 4219 RVA: 0x0004F1CC File Offset: 0x0004D3CC
	public void UpdatePlayerUsernamesFadeThreshold(float value)
	{
		this.playerUsernamesFadeThresholdSlider.value = value;
	}

	// Token: 0x0600107C RID: 4220 RVA: 0x0004F1DA File Offset: 0x0004D3DA
	private void OnMaxMatchmakingPingChanged(ChangeEvent<int> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsMaxMatchmakingPingChanged", new Dictionary<string, object>
		{
			{
				"value",
				this.maxMatchmakingPingSliderInt.value
			}
		});
	}

	// Token: 0x0600107D RID: 4221 RVA: 0x0004F206 File Offset: 0x0004D406
	public void UpdateMaxMatchmakingPing(int value)
	{
		this.maxMatchmakingPingSliderInt.value = value;
	}

	// Token: 0x0600107E RID: 4222 RVA: 0x0004F214 File Offset: 0x0004D414
	private void OnNetworkBufferingChanged(ChangeEvent<string> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsNetworkBufferingChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x0600107F RID: 4223 RVA: 0x0004F236 File Offset: 0x0004D436
	public void UpdateNetworkBuffering(string value)
	{
		this.networkBufferingDropdown.value = value;
	}

	// Token: 0x06001080 RID: 4224 RVA: 0x0004F244 File Offset: 0x0004D444
	private void OnFilterChatProfanityChanged(ChangeEvent<bool> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsFilterChatProfanityChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001081 RID: 4225 RVA: 0x0004F26B File Offset: 0x0004D46B
	public void UpdateFilterChatProfanity(bool value)
	{
		this.filterChatProfanityToggle.value = value;
	}

	// Token: 0x06001082 RID: 4226 RVA: 0x0004F279 File Offset: 0x0004D479
	private void OnUnitsChanged(ChangeEvent<string> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsUnitsChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001083 RID: 4227 RVA: 0x0004F29B File Offset: 0x0004D49B
	public void UpdateUnits(string value)
	{
		this.unitsDropdown.value = value;
	}

	// Token: 0x06001084 RID: 4228 RVA: 0x0004F2A9 File Offset: 0x0004D4A9
	private void OnShowGameUserInterfaceChanged(ChangeEvent<bool> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsShowGameUserInterfaceChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001085 RID: 4229 RVA: 0x0004F2D0 File Offset: 0x0004D4D0
	public void UpdateShowGameUserInterface(bool value)
	{
		this.showGameUserInterfaceToggle.value = value;
	}

	// Token: 0x06001086 RID: 4230 RVA: 0x0004F2E0 File Offset: 0x0004D4E0
	private void OnUserInterfaceScaleChanged(ChangeEvent<float> changeEvent)
	{
		float newValue = changeEvent.newValue;
		Tween tween = this.debounceTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.debounceTween = DOVirtual.DelayedCall(1f, delegate
		{
			EventManager.TriggerEvent("Event_OnSettingsUserInterfaceScaleChanged", new Dictionary<string, object>
			{
				{
					"value",
					newValue
				}
			});
		}, true);
	}

	// Token: 0x06001087 RID: 4231 RVA: 0x0004F32E File Offset: 0x0004D52E
	public void UpdateUserInterfaceScale(float value)
	{
		this.userInterfaceScaleSlider.value = value;
	}

	// Token: 0x06001088 RID: 4232 RVA: 0x0004F33C File Offset: 0x0004D53C
	private void OnChatOpacityChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsChatOpacityChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001089 RID: 4233 RVA: 0x0004F363 File Offset: 0x0004D563
	public void UpdateChatOpacity(float value)
	{
		this.chatOpacitySlider.value = value;
	}

	// Token: 0x0600108A RID: 4234 RVA: 0x0004F371 File Offset: 0x0004D571
	private void OnChatScaleChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsChatScaleChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x0600108B RID: 4235 RVA: 0x0004F398 File Offset: 0x0004D598
	public void UpdateChatScale(float value)
	{
		this.chatScaleSlider.value = value;
	}

	// Token: 0x0600108C RID: 4236 RVA: 0x0004F3A6 File Offset: 0x0004D5A6
	private void OnMinimapOpacityChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsMinimapOpacityChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x0600108D RID: 4237 RVA: 0x0004F3CD File Offset: 0x0004D5CD
	public void UpdateMinimapOpacity(float value)
	{
		this.minimapOpacitySlider.value = value;
	}

	// Token: 0x0600108E RID: 4238 RVA: 0x0004F3DB File Offset: 0x0004D5DB
	private void OnMinimapBackgroundOpacityChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsMinimapBackgroundOpacityChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x0600108F RID: 4239 RVA: 0x0004F402 File Offset: 0x0004D602
	public void UpdateMinimapBackgroundOpacity(float value)
	{
		this.minimapBackgroundOpacitySlider.value = value;
	}

	// Token: 0x06001090 RID: 4240 RVA: 0x0004F410 File Offset: 0x0004D610
	private void OnMinimapHorizontalPositionChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsMinimapHorizontalPositionChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001091 RID: 4241 RVA: 0x0004F437 File Offset: 0x0004D637
	public void UpdateMinimapHorizontalPosition(float value)
	{
		this.minimapHorizontalPositionSlider.value = value;
	}

	// Token: 0x06001092 RID: 4242 RVA: 0x0004F445 File Offset: 0x0004D645
	private void OnMinimapVerticalPositionChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsMinimapVerticalPositionChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001093 RID: 4243 RVA: 0x0004F46C File Offset: 0x0004D66C
	public void UpdateMinimapVerticalPosition(float value)
	{
		this.minimapVerticalPositionSlider.value = value;
	}

	// Token: 0x06001094 RID: 4244 RVA: 0x0004F47A File Offset: 0x0004D67A
	private void OnMinimapScaleChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsMinimapScaleChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x0004F4A1 File Offset: 0x0004D6A1
	public void UpdateMinimapScale(float value)
	{
		this.minimapScaleSlider.value = value;
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x0004F4AF File Offset: 0x0004D6AF
	private void OnGlobalStickSensitivityChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsGlobalStickSensitivityChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x0004F4D6 File Offset: 0x0004D6D6
	public void UpdateGlobalStickSensitivity(float value)
	{
		this.globalStickSensitivitySlider.value = value;
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x0004F4E4 File Offset: 0x0004D6E4
	private void OnHorizontalStickSensitivityChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsHorizontalStickSensitivityChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x0004F50B File Offset: 0x0004D70B
	public void UpdateHorizontalStickSensitivity(float value)
	{
		this.horizontalStickSensitivitySlider.value = value;
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x0004F519 File Offset: 0x0004D719
	private void OnVerticalStickSensitivityChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsVerticalStickSensitivityChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x0004F540 File Offset: 0x0004D740
	public void UpdateVerticalStickSensitivity(float value)
	{
		this.verticalStickSensitivitySlider.value = value;
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x0004F54E File Offset: 0x0004D74E
	private void OnKeyBindInputClicked(string actionName)
	{
		EventManager.TriggerEvent("Event_OnSettingsKeyBindInputClicked", new Dictionary<string, object>
		{
			{
				"actionName",
				actionName
			}
		});
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x0004F56B File Offset: 0x0004D76B
	private void OnKeyBindInputInteractionChanged(string actionName, KeyBindInteraction interaction)
	{
		EventManager.TriggerEvent("Event_OnSettingsKeyBindInputInteractionChanged", new Dictionary<string, object>
		{
			{
				"actionName",
				actionName
			},
			{
				"interaction",
				interaction
			}
		});
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x0004F599 File Offset: 0x0004D799
	private void OnLookSensitivityChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsLookSensitivityChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x0004F5C0 File Offset: 0x0004D7C0
	public void UpdateLookSensitivity(float value)
	{
		this.lookSensitivitySlider.value = value;
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x0004F5CE File Offset: 0x0004D7CE
	private void OnGlobalVolumeChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsGlobalVolumeChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x0004F5F5 File Offset: 0x0004D7F5
	public void UpdateGlobalVolume(float value)
	{
		this.globalVolumeSlider.value = value;
	}

	// Token: 0x060010A2 RID: 4258 RVA: 0x0004F603 File Offset: 0x0004D803
	private void OnAmbientVolumeChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsAmbientVolumeChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x0004F62A File Offset: 0x0004D82A
	public void UpdateAmbientVolume(float value)
	{
		this.ambientVolumeSlider.value = value;
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x0004F638 File Offset: 0x0004D838
	private void OnGameVolumeChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsGameVolumeChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x0004F65F File Offset: 0x0004D85F
	public void UpdateGameVolume(float value)
	{
		this.gameVolumeSlider.value = value;
	}

	// Token: 0x060010A6 RID: 4262 RVA: 0x0004F66D File Offset: 0x0004D86D
	private void OnVoiceVolumeChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsVoiceVolumeChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x060010A7 RID: 4263 RVA: 0x0004F694 File Offset: 0x0004D894
	public void UpdateVoiceVolume(float value)
	{
		this.voiceVolumeSlider.value = value;
	}

	// Token: 0x060010A8 RID: 4264 RVA: 0x0004F6A2 File Offset: 0x0004D8A2
	private void OnUIVolumeChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsUIVolumeChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x060010A9 RID: 4265 RVA: 0x0004F6C9 File Offset: 0x0004D8C9
	public void UpdateUIVolume(float value)
	{
		this.uiVolumeSlider.value = value;
	}

	// Token: 0x060010AA RID: 4266 RVA: 0x0004F6D7 File Offset: 0x0004D8D7
	public void SetMicTestLevel(float level)
	{
		this.micTestFill.style.width = Length.Percent(Mathf.Clamp01(level) * 100f);
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x0004F6FF File Offset: 0x0004D8FF
	private void OnActiveTabChangedInternal(Tab previous, Tab current)
	{
		Action onActiveTabChanged = this.OnActiveTabChanged;
		if (onActiveTabChanged == null)
		{
			return;
		}
		onActiveTabChanged();
	}

	// Token: 0x060010AC RID: 4268 RVA: 0x0004F711 File Offset: 0x0004D911
	private void OnFullScreenModeChanged(ChangeEvent<string> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsFullScreenModeChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x060010AD RID: 4269 RVA: 0x0004F733 File Offset: 0x0004D933
	public void UpdateFullScreenMode(string value)
	{
		this.fullScreenModeDropdown.value = value;
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x0004F741 File Offset: 0x0004D941
	private void OnDisplayChanged(ChangeEvent<string> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsDisplayChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x0004F763 File Offset: 0x0004D963
	public void UpdateDisplay(string value)
	{
		this.displayDropdown.value = value;
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x0004F771 File Offset: 0x0004D971
	public void UpdateDisplayChoices(List<string> choices)
	{
		this.displayDropdown.choices = choices;
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x0004F77F File Offset: 0x0004D97F
	private void OnResolutionChanged(ChangeEvent<string> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsResolutionChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x0004F7A1 File Offset: 0x0004D9A1
	public void UpdateResolution(string value)
	{
		this.resolutionDropdown.value = value;
	}

	// Token: 0x060010B3 RID: 4275 RVA: 0x0004F7AF File Offset: 0x0004D9AF
	public void UpdateResolutionChoices(List<string> choices)
	{
		this.resolutionDropdown.choices = choices;
	}

	// Token: 0x060010B4 RID: 4276 RVA: 0x0004F7BD File Offset: 0x0004D9BD
	private void OnVSyncChanged(ChangeEvent<bool> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsVSyncChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x060010B5 RID: 4277 RVA: 0x0004F7E4 File Offset: 0x0004D9E4
	public void UpdateVSync(bool value)
	{
		this.vSyncToggle.value = value;
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x0004F7F2 File Offset: 0x0004D9F2
	private void OnFpsLimitChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsFpsLimitChanged", new Dictionary<string, object>
		{
			{
				"value",
				(int)changeEvent.newValue
			}
		});
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x0004F81A File Offset: 0x0004DA1A
	public void UpdateFpsLimit(int value)
	{
		this.fpsLimitSlider.value = (float)value;
	}

	// Token: 0x060010B8 RID: 4280 RVA: 0x0004F829 File Offset: 0x0004DA29
	private void OnFovChanged(ChangeEvent<float> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsFovChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x060010B9 RID: 4281 RVA: 0x0004F850 File Offset: 0x0004DA50
	public void UpdateFov(float value)
	{
		this.fovSlider.value = value;
	}

	// Token: 0x060010BA RID: 4282 RVA: 0x0004F85E File Offset: 0x0004DA5E
	private void OnQualityChanged(ChangeEvent<string> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsQualityChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x060010BB RID: 4283 RVA: 0x0004F880 File Offset: 0x0004DA80
	public void UpdateQuality(string value)
	{
		this.qualityDropdown.value = value;
	}

	// Token: 0x060010BC RID: 4284 RVA: 0x0004F88E File Offset: 0x0004DA8E
	private void OnShadowQualityChanged(ChangeEvent<string> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsShadowQualityChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x060010BD RID: 4285 RVA: 0x0004F8B0 File Offset: 0x0004DAB0
	public void UpdateShadowQuality(string value)
	{
		this.shadowQualityDropdown.value = value;
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x0004F8BE File Offset: 0x0004DABE
	private void OnMotionBlurChanged(ChangeEvent<bool> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnSettingsMotionBlurChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x0004F8E5 File Offset: 0x0004DAE5
	public void UpdateMotionBlur(bool value)
	{
		this.motionBlurToggle.value = value;
	}

	// Token: 0x04000A6F RID: 2671
	private VisualElement settings;

	// Token: 0x04000A70 RID: 2672
	private TabView tabView;

	// Token: 0x04000A71 RID: 2673
	private Tab audioTab;

	// Token: 0x04000A72 RID: 2674
	public Action OnActiveTabChanged;

	// Token: 0x04000A73 RID: 2675
	private VisualElement micTestFill;

	// Token: 0x04000A74 RID: 2676
	private IconButton closeIconButton;

	// Token: 0x04000A75 RID: 2677
	private Slider cameraAngleSlider;

	// Token: 0x04000A76 RID: 2678
	private DropdownField handednessDropdown;

	// Token: 0x04000A77 RID: 2679
	private Toggle showPuckSilhouetteToggle;

	// Token: 0x04000A78 RID: 2680
	private Toggle showTeamColorBarToggle;

	// Token: 0x04000A79 RID: 2681
	private Toggle showPuckOutlineToggle;

	// Token: 0x04000A7A RID: 2682
	private Toggle showPuckElevationToggle;

	// Token: 0x04000A7B RID: 2683
	private Toggle showPlayerUsernamesToggle;

	// Token: 0x04000A7C RID: 2684
	private Slider playerUsernamesFadeThresholdSlider;

	// Token: 0x04000A7D RID: 2685
	private SliderInt maxMatchmakingPingSliderInt;

	// Token: 0x04000A7E RID: 2686
	private DropdownField networkBufferingDropdown;

	// Token: 0x04000A7F RID: 2687
	private Toggle filterChatProfanityToggle;

	// Token: 0x04000A80 RID: 2688
	private DropdownField unitsDropdown;

	// Token: 0x04000A81 RID: 2689
	private Toggle showGameUserInterfaceToggle;

	// Token: 0x04000A82 RID: 2690
	private Slider userInterfaceScaleSlider;

	// Token: 0x04000A83 RID: 2691
	private Slider chatOpacitySlider;

	// Token: 0x04000A84 RID: 2692
	private Slider chatScaleSlider;

	// Token: 0x04000A85 RID: 2693
	private Slider minimapOpacitySlider;

	// Token: 0x04000A86 RID: 2694
	private Slider minimapBackgroundOpacitySlider;

	// Token: 0x04000A87 RID: 2695
	private Slider minimapHorizontalPositionSlider;

	// Token: 0x04000A88 RID: 2696
	private Slider minimapVerticalPositionSlider;

	// Token: 0x04000A89 RID: 2697
	private Slider minimapScaleSlider;

	// Token: 0x04000A8A RID: 2698
	private Toggle showMinimapSticksToggle;

	// Token: 0x04000A8B RID: 2699
	private Toggle showMinimapPuckElevationToggle;

	// Token: 0x04000A8C RID: 2700
	private Toggle showMinimapFallenIndicatorToggle;

	// Token: 0x04000A8D RID: 2701
	private Slider globalStickSensitivitySlider;

	// Token: 0x04000A8E RID: 2702
	private Slider horizontalStickSensitivitySlider;

	// Token: 0x04000A8F RID: 2703
	private Slider verticalStickSensitivitySlider;

	// Token: 0x04000A90 RID: 2704
	private Slider lookSensitivitySlider;

	// Token: 0x04000A91 RID: 2705
	private Dictionary<string, KeyBindField> actionNameKeyBindFieldMap;

	// Token: 0x04000A92 RID: 2706
	private Slider globalVolumeSlider;

	// Token: 0x04000A93 RID: 2707
	private Slider ambientVolumeSlider;

	// Token: 0x04000A94 RID: 2708
	private Slider gameVolumeSlider;

	// Token: 0x04000A95 RID: 2709
	private Slider voiceVolumeSlider;

	// Token: 0x04000A96 RID: 2710
	private Slider uiVolumeSlider;

	// Token: 0x04000A97 RID: 2711
	private DropdownField fullScreenModeDropdown;

	// Token: 0x04000A98 RID: 2712
	private DropdownField displayDropdown;

	// Token: 0x04000A99 RID: 2713
	private DropdownField resolutionDropdown;

	// Token: 0x04000A9A RID: 2714
	private Toggle vSyncToggle;

	// Token: 0x04000A9B RID: 2715
	private Slider fpsLimitSlider;

	// Token: 0x04000A9C RID: 2716
	private Slider fovSlider;

	// Token: 0x04000A9D RID: 2717
	private DropdownField qualityDropdown;

	// Token: 0x04000A9E RID: 2718
	private DropdownField shadowQualityDropdown;

	// Token: 0x04000A9F RID: 2719
	private Toggle motionBlurToggle;

	// Token: 0x04000AA0 RID: 2720
	private Button resetToDefaultButton;

	// Token: 0x04000AA1 RID: 2721
	private Tween debounceTween;
}
