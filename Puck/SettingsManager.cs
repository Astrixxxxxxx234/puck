using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000164 RID: 356
public static class SettingsManager
{
	// Token: 0x06000A6C RID: 2668 RVA: 0x00031870 File Offset: 0x0002FA70
	public static void Initialize()
	{
		SettingsManager.Debug = SaveManager.GetEnum<DebugMode>("debug", DebugMode.Off);
		SettingsManager.CameraAngle = SaveManager.GetFloat("cameraAngle", 30f);
		SettingsManager.Handedness = SaveManager.GetEnum<PlayerHandedness>("handedness", PlayerHandedness.Right);
		SettingsManager.ShowPuckSilhouette = SaveManager.GetBool("showPuckSilhouette", true);
		SettingsManager.ShowPuckOutline = SaveManager.GetBool("showPuckOutline", false);
		SettingsManager.ShowPuckElevation = SaveManager.GetBool("showPuckElevation", true);
		SettingsManager.ShowPlayerUsernames = SaveManager.GetBool("showPlayerUsernames", false);
		SettingsManager.PlayerUsernamesFadeThreshold = SaveManager.GetFloat("playerUsernamesFadeThreshold", 1f);
		SettingsManager.MaxMatchmakingPing = SaveManager.GetInt("maxMatchmakingPing", 50);
		SettingsManager.NetworkBuffering = SaveManager.GetEnum<NetworkBuffering>("networkBuffering", NetworkBuffering.Responsive);
		SettingsManager.FilterChatProfanity = SaveManager.GetBool("filterChatProfanity", true);
		SettingsManager.Units = SaveManager.GetEnum<Units>("units", Units.Metric);
		SettingsManager.ShowGameUserInterface = SaveManager.GetBool("showGameUserInterface", true);
		SettingsManager.ShowTeamColorBar = SaveManager.GetBool("showTeamColorBar", true);
		SettingsManager.UserInterfaceScale = SaveManager.GetFloat("userInterfaceScale", 1f);
		SettingsManager.ChatOpacity = SaveManager.GetFloat("chatOpacity", 1f);
		SettingsManager.ChatScale = SaveManager.GetFloat("chatScale", 1f);
		SettingsManager.MinimapOpacity = SaveManager.GetFloat("minimapOpacity", 1f);
		SettingsManager.MinimapBackgroundOpacity = SaveManager.GetFloat("minimapBackgroundOpacity", 1f);
		SettingsManager.MinimapHorizontalPosition = SaveManager.GetFloat("minimapHorizontalPosition", 100f);
		SettingsManager.MinimapVerticalPosition = SaveManager.GetFloat("minimapVerticalPosition", 0f);
		SettingsManager.MinimapScale = SaveManager.GetFloat("minimapScale", 1f);
		SettingsManager.ShowMinimapSticks = SaveManager.GetBool("showMinimapSticks", true);
		SettingsManager.ShowMinimapPuckElevation = SaveManager.GetBool("showMinimapPuckElevation", true);
		SettingsManager.ShowMinimapFallenIndicator = SaveManager.GetBool("showMinimapFallenIndicator", true);
		SettingsManager.GlobalStickSensitivity = SaveManager.GetFloat("globalStickSensitivity", 0.2f);
		SettingsManager.HorizontalStickSensitivity = SaveManager.GetFloat("horizontalStickSensitivity", 1f);
		SettingsManager.VerticalStickSensitivity = SaveManager.GetFloat("verticalStickSensitivity", 1f);
		SettingsManager.LookSensitivity = SaveManager.GetFloat("lookSensitivity", 0.2f);
		SettingsManager.GlobalVolume = SaveManager.GetFloat("globalVolume", 0.5f);
		SettingsManager.AmbientVolume = SaveManager.GetFloat("ambientVolume", 1f);
		SettingsManager.GameVolume = SaveManager.GetFloat("gameVolume", 1f);
		SettingsManager.VoiceVolume = SaveManager.GetFloat("voiceVolume", 1f);
		SettingsManager.UIVolume = SaveManager.GetFloat("uiVolume", 0.5f);
		SettingsManager.FullScreenMode = SaveManager.GetEnum<FullScreenMode>("fullScreenMode", FullScreenMode.FullScreenWindow);
		SettingsManager.DisplayIndex = SaveManager.GetInt("displayIndex", 0);
		SettingsManager.ResolutionIndex = SaveManager.GetInt("resolutionIndex", -1);
		SettingsManager.VSync = SaveManager.GetBool("vSync", false);
		SettingsManager.FpsLimit = SaveManager.GetInt("fpsLimit", 240);
		SettingsManager.Fov = SaveManager.GetFloat("fov", 90f);
		SettingsManager.Quality = SaveManager.GetEnum<ApplicationQuality>("quality", ApplicationQuality.High);
		SettingsManager.ShadowQuality = SaveManager.GetEnum<global::ShadowQuality>("shadowQuality", global::ShadowQuality.High);
		SettingsManager.MotionBlur = SaveManager.GetBool("motionBlur", true);
		SettingsManager.Team = SaveManager.GetEnum<PlayerTeam>("team", PlayerTeam.Blue);
		SettingsManager.Role = SaveManager.GetEnum<PlayerRole>("role", PlayerRole.Attacker);
		SettingsManager.ApplyForBothTeams = SaveManager.GetBool("applyForBothTeams", false);
		SettingsManager.FlagID = SaveManager.GetInt("flagID", -1);
		SettingsManager.HeadgearIDBlueAttacker = SaveManager.GetInt("headgearIDBlueAttacker", 513);
		SettingsManager.HeadgearIDRedAttacker = SaveManager.GetInt("headgearIDRedAttacker", 513);
		SettingsManager.HeadgearIDBlueGoalie = SaveManager.GetInt("headgearIDBlueGoalie", 527);
		SettingsManager.HeadgearIDRedGoalie = SaveManager.GetInt("headgearIDRedGoalie", 527);
		SettingsManager.MustacheID = SaveManager.GetInt("mustacheID", -1);
		SettingsManager.BeardID = SaveManager.GetInt("beardID", -1);
		SettingsManager.JerseyIDBlueAttacker = SaveManager.GetInt("jerseyIDBlueAttacker", 2048);
		SettingsManager.JerseyIDRedAttacker = SaveManager.GetInt("jerseyIDRedAttacker", 2048);
		SettingsManager.JerseyIDBlueGoalie = SaveManager.GetInt("jerseyIDBlueGoalie", 2048);
		SettingsManager.JerseyIDRedGoalie = SaveManager.GetInt("jerseyIDRedGoalie", 2048);
		SettingsManager.StickSkinIDBlueAttacker = SaveManager.GetInt("stickSkinIDBlueAttacker", 2621);
		SettingsManager.StickSkinIDRedAttacker = SaveManager.GetInt("stickSkinIDRedAttacker", 2621);
		SettingsManager.StickSkinIDBlueGoalie = SaveManager.GetInt("stickSkinIDBlueGoalie", 2621);
		SettingsManager.StickSkinIDRedGoalie = SaveManager.GetInt("stickSkinIDRedGoalie", 2621);
		SettingsManager.StickShaftTapeIDBlueAttacker = SaveManager.GetInt("stickShaftTapeIDBlueAttacker", -1);
		SettingsManager.StickShaftTapeIDRedAttacker = SaveManager.GetInt("stickShaftTapeIDRedAttacker", -1);
		SettingsManager.StickShaftTapeIDBlueGoalie = SaveManager.GetInt("stickShaftTapeIDBlueGoalie", -1);
		SettingsManager.StickShaftTapeIDRedGoalie = SaveManager.GetInt("stickShaftTapeIDRedGoalie", -1);
		SettingsManager.StickBladeTapeIDBlueAttacker = SaveManager.GetInt("stickBladeTapeIDBlueAttacker", -1);
		SettingsManager.StickBladeTapeIDRedAttacker = SaveManager.GetInt("stickBladeTapeIDRedAttacker", -1);
		SettingsManager.StickBladeTapeIDBlueGoalie = SaveManager.GetInt("stickBladeTapeIDBlueGoalie", -1);
		SettingsManager.StickBladeTapeIDRedGoalie = SaveManager.GetInt("stickBladeTapeIDRedGoalie", -1);
		SettingsManagerController.Initialize();
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x00031D57 File Offset: 0x0002FF57
	public static void Dispose()
	{
		SettingsManagerController.Dispose();
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x00031D60 File Offset: 0x0002FF60
	public static void ResetToDefault()
	{
		SettingsManager.UpdateDebug(DebugMode.Off);
		SettingsManager.UpdateCameraAngle(30f);
		SettingsManager.UpdateHandedness(PlayerHandedness.Right);
		SettingsManager.UpdateShowPuckSilhouette(true);
		SettingsManager.UpdateShowPuckOutline(false);
		SettingsManager.UpdateShowPuckElevation(true);
		SettingsManager.UpdateShowPlayerUsernames(false);
		SettingsManager.UpdatePlayerUsernamesFadeThreshold(1f);
		SettingsManager.UpdateMaxMatchmakingPing(50);
		SettingsManager.UpdateNetworkBuffering(NetworkBuffering.Responsive);
		SettingsManager.UpdateFilterChatProfanity(true);
		SettingsManager.UpdateUnits(Units.Metric);
		SettingsManager.UpdateShowGameUserInterface(true);
		SettingsManager.UpdateShowTeamColorBar(true);
		SettingsManager.UpdateUserInterfaceScale(1f);
		SettingsManager.UpdateChatOpacity(1f);
		SettingsManager.UpdateChatScale(1f);
		SettingsManager.UpdateMinimapOpacity(1f);
		SettingsManager.UpdateMinimapBackgroundOpacity(1f);
		SettingsManager.UpdateMinimapHorizontalPosition(100f);
		SettingsManager.UpdateMinimapVerticalPosition(0f);
		SettingsManager.UpdateMinimapScale(1f);
		SettingsManager.UpdateShowMinimapSticks(true);
		SettingsManager.UpdateShowMinimapPuckElevation(true);
		SettingsManager.UpdateShowMinimapFallenIndicator(true);
		SettingsManager.UpdateGlobalStickSensitivity(0.2f);
		SettingsManager.UpdateHorizontalStickSensitivity(1f);
		SettingsManager.UpdateVerticalStickSensitivity(1f);
		SettingsManager.UpdateLookSensitivity(0.2f);
		SettingsManager.UpdateGlobalVolume(0.5f);
		SettingsManager.UpdateAmbientVolume(1f);
		SettingsManager.UpdateGameVolume(1f);
		SettingsManager.UpdateVoiceVolume(1f);
		SettingsManager.UpdateUIVolume(0.5f);
		SettingsManager.UpdateFullScreenMode(FullScreenMode.FullScreenWindow);
		SettingsManager.UpdateDisplayIndex(0);
		SettingsManager.UpdateResolutionIndex(-1);
		SettingsManager.UpdateVSync(false);
		SettingsManager.UpdateFpsLimit(240);
		SettingsManager.UpdateFov(90f);
		SettingsManager.UpdateQuality(ApplicationQuality.High);
		SettingsManager.UpdateShadowQuality(global::ShadowQuality.High);
		SettingsManager.UpdateMotionBlur(true);
		SettingsManager.UpdateTeam(PlayerTeam.Blue);
		SettingsManager.UpdateRole(PlayerRole.Attacker);
		SettingsManager.UpdateApplyForBothTeams(false);
		SettingsManager.UpdateFlagID(-1);
		SettingsManager.UpdateHeadgearID(PlayerTeam.Blue, PlayerRole.Attacker, 513);
		SettingsManager.UpdateHeadgearID(PlayerTeam.Red, PlayerRole.Attacker, 513);
		SettingsManager.UpdateHeadgearID(PlayerTeam.Blue, PlayerRole.Goalie, 527);
		SettingsManager.UpdateHeadgearID(PlayerTeam.Red, PlayerRole.Goalie, 527);
		SettingsManager.UpdateMustacheID(-1);
		SettingsManager.UpdateBeardID(-1);
		SettingsManager.UpdateJerseyID(PlayerTeam.Blue, PlayerRole.Attacker, 2048);
		SettingsManager.UpdateJerseyID(PlayerTeam.Red, PlayerRole.Attacker, 2048);
		SettingsManager.UpdateJerseyID(PlayerTeam.Blue, PlayerRole.Goalie, 2048);
		SettingsManager.UpdateJerseyID(PlayerTeam.Red, PlayerRole.Goalie, 2048);
		SettingsManager.UpdateStickSkinID(PlayerTeam.Blue, PlayerRole.Attacker, 2621);
		SettingsManager.UpdateStickSkinID(PlayerTeam.Red, PlayerRole.Attacker, 2621);
		SettingsManager.UpdateStickSkinID(PlayerTeam.Blue, PlayerRole.Goalie, 2621);
		SettingsManager.UpdateStickSkinID(PlayerTeam.Red, PlayerRole.Goalie, 2621);
		SettingsManager.UpdateStickShaftTapeID(PlayerTeam.Blue, PlayerRole.Attacker, -1);
		SettingsManager.UpdateStickShaftTapeID(PlayerTeam.Red, PlayerRole.Attacker, -1);
		SettingsManager.UpdateStickShaftTapeID(PlayerTeam.Blue, PlayerRole.Goalie, -1);
		SettingsManager.UpdateStickShaftTapeID(PlayerTeam.Red, PlayerRole.Goalie, -1);
		SettingsManager.UpdateStickBladeTapeID(PlayerTeam.Blue, PlayerRole.Attacker, -1);
		SettingsManager.UpdateStickBladeTapeID(PlayerTeam.Red, PlayerRole.Attacker, -1);
		SettingsManager.UpdateStickBladeTapeID(PlayerTeam.Blue, PlayerRole.Goalie, -1);
		SettingsManager.UpdateStickBladeTapeID(PlayerTeam.Red, PlayerRole.Goalie, -1);
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x00031FB8 File Offset: 0x000301B8
	public static int GetHeadgearID(PlayerTeam team, PlayerRole role)
	{
		if (role != PlayerRole.Attacker)
		{
			if (role == PlayerRole.Goalie)
			{
				if (team == PlayerTeam.Blue)
				{
					return SettingsManager.HeadgearIDBlueGoalie;
				}
				if (team == PlayerTeam.Red)
				{
					return SettingsManager.HeadgearIDRedGoalie;
				}
			}
		}
		else
		{
			if (team == PlayerTeam.Blue)
			{
				return SettingsManager.HeadgearIDBlueAttacker;
			}
			if (team == PlayerTeam.Red)
			{
				return SettingsManager.HeadgearIDRedAttacker;
			}
		}
		return -1;
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x00031FF1 File Offset: 0x000301F1
	public static int GetJerseyID(PlayerTeam team, PlayerRole role)
	{
		if (role != PlayerRole.Attacker)
		{
			if (role == PlayerRole.Goalie)
			{
				if (team == PlayerTeam.Blue)
				{
					return SettingsManager.JerseyIDBlueGoalie;
				}
				if (team == PlayerTeam.Red)
				{
					return SettingsManager.JerseyIDRedGoalie;
				}
			}
		}
		else
		{
			if (team == PlayerTeam.Blue)
			{
				return SettingsManager.JerseyIDBlueAttacker;
			}
			if (team == PlayerTeam.Red)
			{
				return SettingsManager.JerseyIDRedAttacker;
			}
		}
		return 2048;
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0003202E File Offset: 0x0003022E
	public static int GetStickSkinID(PlayerTeam team, PlayerRole role)
	{
		if (role != PlayerRole.Attacker)
		{
			if (role == PlayerRole.Goalie)
			{
				if (team == PlayerTeam.Blue)
				{
					return SettingsManager.StickSkinIDBlueGoalie;
				}
				if (team == PlayerTeam.Red)
				{
					return SettingsManager.StickSkinIDRedGoalie;
				}
			}
		}
		else
		{
			if (team == PlayerTeam.Blue)
			{
				return SettingsManager.StickSkinIDBlueAttacker;
			}
			if (team == PlayerTeam.Red)
			{
				return SettingsManager.StickSkinIDRedAttacker;
			}
		}
		return 2621;
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x0003206B File Offset: 0x0003026B
	public static int GetStickShaftTapeID(PlayerTeam team, PlayerRole role)
	{
		if (role != PlayerRole.Attacker)
		{
			if (role == PlayerRole.Goalie)
			{
				if (team == PlayerTeam.Blue)
				{
					return SettingsManager.StickShaftTapeIDBlueGoalie;
				}
				if (team == PlayerTeam.Red)
				{
					return SettingsManager.StickShaftTapeIDRedGoalie;
				}
			}
		}
		else
		{
			if (team == PlayerTeam.Blue)
			{
				return SettingsManager.StickShaftTapeIDBlueAttacker;
			}
			if (team == PlayerTeam.Red)
			{
				return SettingsManager.StickShaftTapeIDRedAttacker;
			}
		}
		return -1;
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x000320A4 File Offset: 0x000302A4
	public static int GetStickBladeTapeID(PlayerTeam team, PlayerRole role)
	{
		if (role != PlayerRole.Attacker)
		{
			if (role == PlayerRole.Goalie)
			{
				if (team == PlayerTeam.Blue)
				{
					return SettingsManager.StickBladeTapeIDBlueGoalie;
				}
				if (team == PlayerTeam.Red)
				{
					return SettingsManager.StickBladeTapeIDRedGoalie;
				}
			}
		}
		else
		{
			if (team == PlayerTeam.Blue)
			{
				return SettingsManager.StickBladeTapeIDBlueAttacker;
			}
			if (team == PlayerTeam.Red)
			{
				return SettingsManager.StickBladeTapeIDRedAttacker;
			}
		}
		return -1;
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x000320E0 File Offset: 0x000302E0
	public static void UpdateDebug(DebugMode value)
	{
		if (SettingsManager.Debug == value)
		{
			return;
		}
		SettingsManager.Debug = value;
		SaveManager.SetEnum<DebugMode>("debug", SettingsManager.Debug);
		EventManager.TriggerEvent("Event_OnDebugChanged", new Dictionary<string, object>
		{
			{
				"value",
				SettingsManager.Debug
			}
		});
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x00032130 File Offset: 0x00030330
	public static void UpdateCameraAngle(float value)
	{
		if (SettingsManager.CameraAngle == value)
		{
			return;
		}
		SettingsManager.CameraAngle = value;
		SaveManager.SetFloat("cameraAngle", SettingsManager.CameraAngle);
		EventManager.TriggerEvent("Event_OnCameraAngleChanged", new Dictionary<string, object>
		{
			{
				"value",
				SettingsManager.CameraAngle
			}
		});
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x00032180 File Offset: 0x00030380
	public static void UpdateHandedness(PlayerHandedness value)
	{
		if (SettingsManager.Handedness == value)
		{
			return;
		}
		SettingsManager.Handedness = value;
		SaveManager.SetEnum<PlayerHandedness>("handedness", SettingsManager.Handedness);
		EventManager.TriggerEvent("Event_OnHandednessChanged", new Dictionary<string, object>
		{
			{
				"value",
				SettingsManager.Handedness
			}
		});
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x000321CF File Offset: 0x000303CF
	public static void UpdateShowPuckSilhouette(bool value)
	{
		if (SettingsManager.ShowPuckSilhouette == value)
		{
			return;
		}
		SettingsManager.ShowPuckSilhouette = value;
		SaveManager.SetBool("showPuckSilhouette", SettingsManager.ShowPuckSilhouette);
		EventManager.TriggerEvent("Event_OnShowPuckSilhouetteChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x0003220F File Offset: 0x0003040F
	public static void UpdateShowPuckOutline(bool value)
	{
		if (SettingsManager.ShowPuckOutline == value)
		{
			return;
		}
		SettingsManager.ShowPuckOutline = value;
		SaveManager.SetBool("showPuckOutline", SettingsManager.ShowPuckOutline);
		EventManager.TriggerEvent("Event_OnShowPuckOutlineChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x0003224F File Offset: 0x0003044F
	public static void UpdateShowPuckElevation(bool value)
	{
		if (SettingsManager.ShowPuckElevation == value)
		{
			return;
		}
		SettingsManager.ShowPuckElevation = value;
		SaveManager.SetBool("showPuckElevation", SettingsManager.ShowPuckElevation);
		EventManager.TriggerEvent("Event_OnShowPuckElevationChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0003228F File Offset: 0x0003048F
	public static void UpdateShowMinimapSticks(bool value)
	{
		if (SettingsManager.ShowMinimapSticks == value)
		{
			return;
		}
		SettingsManager.ShowMinimapSticks = value;
		SaveManager.SetBool("showMinimapSticks", SettingsManager.ShowMinimapSticks);
		EventManager.TriggerEvent("Event_OnShowMinimapSticksChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x000322CF File Offset: 0x000304CF
	public static void UpdateShowMinimapPuckElevation(bool value)
	{
		if (SettingsManager.ShowMinimapPuckElevation == value)
		{
			return;
		}
		SettingsManager.ShowMinimapPuckElevation = value;
		SaveManager.SetBool("showMinimapPuckElevation", SettingsManager.ShowMinimapPuckElevation);
		EventManager.TriggerEvent("Event_OnShowMinimapPuckElevationChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x0003230F File Offset: 0x0003050F
	public static void UpdateShowMinimapFallenIndicator(bool value)
	{
		if (SettingsManager.ShowMinimapFallenIndicator == value)
		{
			return;
		}
		SettingsManager.ShowMinimapFallenIndicator = value;
		SaveManager.SetBool("showMinimapFallenIndicator", SettingsManager.ShowMinimapFallenIndicator);
		EventManager.TriggerEvent("Event_OnShowMinimapFallenIndicatorChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x0003234F File Offset: 0x0003054F
	public static void UpdateShowPlayerUsernames(bool value)
	{
		if (SettingsManager.ShowPlayerUsernames == value)
		{
			return;
		}
		SettingsManager.ShowPlayerUsernames = value;
		SaveManager.SetBool("showPlayerUsernames", SettingsManager.ShowPlayerUsernames);
		EventManager.TriggerEvent("Event_OnShowPlayerUsernamesChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x0003238F File Offset: 0x0003058F
	public static void UpdatePlayerUsernamesFadeThreshold(float value)
	{
		if (SettingsManager.PlayerUsernamesFadeThreshold == value)
		{
			return;
		}
		SettingsManager.PlayerUsernamesFadeThreshold = value;
		SaveManager.SetFloat("playerUsernamesFadeThreshold", SettingsManager.PlayerUsernamesFadeThreshold);
		EventManager.TriggerEvent("Event_OnPlayerUsernamesFadeThresholdChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x000323CF File Offset: 0x000305CF
	public static void UpdateMaxMatchmakingPing(int value)
	{
		if (SettingsManager.MaxMatchmakingPing == value)
		{
			return;
		}
		SettingsManager.MaxMatchmakingPing = value;
		SaveManager.SetInt("maxMatchmakingPing", SettingsManager.MaxMatchmakingPing);
		EventManager.TriggerEvent("Event_OnMaxMatchmakingPingChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0003240F File Offset: 0x0003060F
	public static void UpdateNetworkBuffering(NetworkBuffering value)
	{
		if (SettingsManager.NetworkBuffering == value)
		{
			return;
		}
		SettingsManager.NetworkBuffering = value;
		SaveManager.SetEnum<NetworkBuffering>("networkBuffering", SettingsManager.NetworkBuffering);
		EventManager.TriggerEvent("Event_OnNetworkBufferingChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0003244F File Offset: 0x0003064F
	public static void UpdateFilterChatProfanity(bool value)
	{
		if (SettingsManager.FilterChatProfanity == value)
		{
			return;
		}
		SettingsManager.FilterChatProfanity = value;
		SaveManager.SetBool("filterChatProfanity", SettingsManager.FilterChatProfanity);
		EventManager.TriggerEvent("Event_OnFilterChatProfanityChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x00032490 File Offset: 0x00030690
	public static void UpdateUnits(Units value)
	{
		if (SettingsManager.Units == value)
		{
			return;
		}
		SettingsManager.Units = value;
		SaveManager.SetEnum<Units>("units", SettingsManager.Units);
		EventManager.TriggerEvent("Event_OnUnitsChanged", new Dictionary<string, object>
		{
			{
				"value",
				SettingsManager.Units
			}
		});
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x000324DF File Offset: 0x000306DF
	public static void UpdateShowGameUserInterface(bool value)
	{
		if (SettingsManager.ShowGameUserInterface == value)
		{
			return;
		}
		SettingsManager.ShowGameUserInterface = value;
		SaveManager.SetBool("showGameUserInterface", SettingsManager.ShowGameUserInterface);
		EventManager.TriggerEvent("Event_OnShowGameUserInterfaceChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0003251F File Offset: 0x0003071F
	public static void UpdateShowTeamColorBar(bool value)
	{
		if (SettingsManager.ShowTeamColorBar == value)
		{
			return;
		}
		SettingsManager.ShowTeamColorBar = value;
		SaveManager.SetBool("showTeamColorBar", SettingsManager.ShowTeamColorBar);
		EventManager.TriggerEvent("Event_OnShowTeamColorBarChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x0003255F File Offset: 0x0003075F
	public static void UpdateUserInterfaceScale(float value)
	{
		if (SettingsManager.UserInterfaceScale == value)
		{
			return;
		}
		SettingsManager.UserInterfaceScale = value;
		SaveManager.SetFloat("userInterfaceScale", SettingsManager.UserInterfaceScale);
		EventManager.TriggerEvent("Event_OnUserInterfaceScaleChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x0003259F File Offset: 0x0003079F
	public static void UpdateChatOpacity(float value)
	{
		if (SettingsManager.ChatOpacity == value)
		{
			return;
		}
		SettingsManager.ChatOpacity = value;
		SaveManager.SetFloat("chatOpacity", SettingsManager.ChatOpacity);
		EventManager.TriggerEvent("Event_OnChatOpacityChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x000325DF File Offset: 0x000307DF
	public static void UpdateChatScale(float value)
	{
		if (SettingsManager.ChatScale == value)
		{
			return;
		}
		SettingsManager.ChatScale = value;
		SaveManager.SetFloat("chatScale", SettingsManager.ChatScale);
		EventManager.TriggerEvent("Event_OnChatScaleChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x0003261F File Offset: 0x0003081F
	public static void UpdateMinimapOpacity(float value)
	{
		if (SettingsManager.MinimapOpacity == value)
		{
			return;
		}
		SettingsManager.MinimapOpacity = value;
		SaveManager.SetFloat("minimapOpacity", SettingsManager.MinimapOpacity);
		EventManager.TriggerEvent("Event_OnMinimapOpacityChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x0003265F File Offset: 0x0003085F
	public static void UpdateMinimapBackgroundOpacity(float value)
	{
		if (SettingsManager.MinimapBackgroundOpacity == value)
		{
			return;
		}
		SettingsManager.MinimapBackgroundOpacity = value;
		SaveManager.SetFloat("minimapBackgroundOpacity", SettingsManager.MinimapBackgroundOpacity);
		EventManager.TriggerEvent("Event_OnMinimapBackgroundOpacityChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x0003269F File Offset: 0x0003089F
	public static void UpdateMinimapHorizontalPosition(float value)
	{
		if (SettingsManager.MinimapHorizontalPosition == value)
		{
			return;
		}
		SettingsManager.MinimapHorizontalPosition = value;
		SaveManager.SetFloat("minimapHorizontalPosition", SettingsManager.MinimapHorizontalPosition);
		EventManager.TriggerEvent("Event_OnMinimapHorizontalPositionChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x000326DF File Offset: 0x000308DF
	public static void UpdateMinimapVerticalPosition(float value)
	{
		if (SettingsManager.MinimapVerticalPosition == value)
		{
			return;
		}
		SettingsManager.MinimapVerticalPosition = value;
		SaveManager.SetFloat("minimapVerticalPosition", SettingsManager.MinimapVerticalPosition);
		EventManager.TriggerEvent("Event_OnMinimapVerticalPositionChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x0003271F File Offset: 0x0003091F
	public static void UpdateMinimapScale(float value)
	{
		if (SettingsManager.MinimapScale == value)
		{
			return;
		}
		SettingsManager.MinimapScale = value;
		SaveManager.SetFloat("minimapScale", SettingsManager.MinimapScale);
		EventManager.TriggerEvent("Event_OnMinimapScaleChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x0003275F File Offset: 0x0003095F
	public static void UpdateGlobalStickSensitivity(float value)
	{
		if (SettingsManager.GlobalStickSensitivity == value)
		{
			return;
		}
		SettingsManager.GlobalStickSensitivity = value;
		SaveManager.SetFloat("globalStickSensitivity", SettingsManager.GlobalStickSensitivity);
		EventManager.TriggerEvent("Event_OnGlobalStickSensitivityChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x0003279F File Offset: 0x0003099F
	public static void UpdateHorizontalStickSensitivity(float value)
	{
		if (SettingsManager.HorizontalStickSensitivity == value)
		{
			return;
		}
		SettingsManager.HorizontalStickSensitivity = value;
		SaveManager.SetFloat("horizontalStickSensitivity", SettingsManager.HorizontalStickSensitivity);
		EventManager.TriggerEvent("Event_OnHorizontalStickSensitivityChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x000327DF File Offset: 0x000309DF
	public static void UpdateVerticalStickSensitivity(float value)
	{
		if (SettingsManager.VerticalStickSensitivity == value)
		{
			return;
		}
		SettingsManager.VerticalStickSensitivity = value;
		SaveManager.SetFloat("verticalStickSensitivity", SettingsManager.VerticalStickSensitivity);
		EventManager.TriggerEvent("Event_OnVerticalStickSensitivityChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x0003281F File Offset: 0x00030A1F
	public static void UpdateLookSensitivity(float value)
	{
		if (SettingsManager.LookSensitivity == value)
		{
			return;
		}
		SettingsManager.LookSensitivity = value;
		SaveManager.SetFloat("lookSensitivity", SettingsManager.LookSensitivity);
		EventManager.TriggerEvent("Event_OnLookSensitivityChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x0003285F File Offset: 0x00030A5F
	public static void UpdateGlobalVolume(float value)
	{
		if (SettingsManager.GlobalVolume == value)
		{
			return;
		}
		SettingsManager.GlobalVolume = value;
		SaveManager.SetFloat("globalVolume", SettingsManager.GlobalVolume);
		EventManager.TriggerEvent("Event_OnGlobalVolumeChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0003289F File Offset: 0x00030A9F
	public static void UpdateAmbientVolume(float value)
	{
		if (SettingsManager.AmbientVolume == value)
		{
			return;
		}
		SettingsManager.AmbientVolume = value;
		SaveManager.SetFloat("ambientVolume", SettingsManager.AmbientVolume);
		EventManager.TriggerEvent("Event_OnAmbientVolumeChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x000328DF File Offset: 0x00030ADF
	public static void UpdateGameVolume(float value)
	{
		if (SettingsManager.GameVolume == value)
		{
			return;
		}
		SettingsManager.GameVolume = value;
		SaveManager.SetFloat("gameVolume", SettingsManager.GameVolume);
		EventManager.TriggerEvent("Event_OnGameVolumeChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x0003291F File Offset: 0x00030B1F
	public static void UpdateVoiceVolume(float value)
	{
		if (SettingsManager.VoiceVolume == value)
		{
			return;
		}
		SettingsManager.VoiceVolume = value;
		SaveManager.SetFloat("voiceVolume", SettingsManager.VoiceVolume);
		EventManager.TriggerEvent("Event_OnVoiceVolumeChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x0003295F File Offset: 0x00030B5F
	public static void UpdateUIVolume(float value)
	{
		if (SettingsManager.UIVolume == value)
		{
			return;
		}
		SettingsManager.UIVolume = value;
		SaveManager.SetFloat("uiVolume", SettingsManager.UIVolume);
		EventManager.TriggerEvent("Event_OnUIVolumeChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x000329A0 File Offset: 0x00030BA0
	public static void UpdateFullScreenMode(FullScreenMode value)
	{
		if (SettingsManager.FullScreenMode == value)
		{
			return;
		}
		SettingsManager.FullScreenMode = value;
		SaveManager.SetEnum<FullScreenMode>("fullScreenMode", SettingsManager.FullScreenMode);
		EventManager.TriggerEvent("Event_OnFullScreenModeChanged", new Dictionary<string, object>
		{
			{
				"value",
				SettingsManager.FullScreenMode
			}
		});
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x000329F0 File Offset: 0x00030BF0
	public static void UpdateDisplayIndex(int value)
	{
		if (SettingsManager.DisplayIndex == value)
		{
			return;
		}
		SettingsManager.DisplayIndex = value;
		SaveManager.SetInt("displayIndex", SettingsManager.DisplayIndex);
		EventManager.TriggerEvent("Event_OnDisplayIndexChanged", new Dictionary<string, object>
		{
			{
				"value",
				SettingsManager.DisplayIndex
			}
		});
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x00032A40 File Offset: 0x00030C40
	public static void UpdateResolutionIndex(int value)
	{
		if (SettingsManager.ResolutionIndex == value)
		{
			return;
		}
		SettingsManager.ResolutionIndex = value;
		SaveManager.SetInt("resolutionIndex", SettingsManager.ResolutionIndex);
		EventManager.TriggerEvent("Event_OnResolutionIndexChanged", new Dictionary<string, object>
		{
			{
				"value",
				SettingsManager.ResolutionIndex
			}
		});
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x00032A90 File Offset: 0x00030C90
	public static void UpdateVSync(bool value)
	{
		if (SettingsManager.VSync == value)
		{
			return;
		}
		SettingsManager.VSync = value;
		SaveManager.SetBool("vSync", SettingsManager.VSync);
		EventManager.TriggerEvent("Event_OnVSyncChanged", new Dictionary<string, object>
		{
			{
				"value",
				SettingsManager.VSync
			}
		});
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x00032AE0 File Offset: 0x00030CE0
	public static void UpdateFpsLimit(int value)
	{
		if (SettingsManager.FpsLimit == value)
		{
			return;
		}
		SettingsManager.FpsLimit = value;
		SaveManager.SetInt("fpsLimit", SettingsManager.FpsLimit);
		EventManager.TriggerEvent("Event_OnFpsLimitChanged", new Dictionary<string, object>
		{
			{
				"value",
				SettingsManager.FpsLimit
			}
		});
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x00032B30 File Offset: 0x00030D30
	public static void UpdateFov(float value)
	{
		if (SettingsManager.Fov == value)
		{
			return;
		}
		SettingsManager.Fov = value;
		SaveManager.SetFloat("fov", SettingsManager.Fov);
		EventManager.TriggerEvent("Event_OnFovChanged", new Dictionary<string, object>
		{
			{
				"value",
				SettingsManager.Fov
			}
		});
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x00032B80 File Offset: 0x00030D80
	public static void UpdateQuality(ApplicationQuality value)
	{
		if (SettingsManager.Quality == value)
		{
			return;
		}
		SettingsManager.Quality = value;
		SaveManager.SetEnum<ApplicationQuality>("quality", SettingsManager.Quality);
		EventManager.TriggerEvent("Event_OnQualityChanged", new Dictionary<string, object>
		{
			{
				"value",
				SettingsManager.Quality
			}
		});
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00032BD0 File Offset: 0x00030DD0
	public static void UpdateShadowQuality(global::ShadowQuality value)
	{
		if (SettingsManager.ShadowQuality == value)
		{
			return;
		}
		SettingsManager.ShadowQuality = value;
		SaveManager.SetEnum<global::ShadowQuality>("shadowQuality", SettingsManager.ShadowQuality);
		EventManager.TriggerEvent("Event_OnShadowQualityChanged", new Dictionary<string, object>
		{
			{
				"value",
				SettingsManager.ShadowQuality
			}
		});
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x00032C1F File Offset: 0x00030E1F
	public static void UpdateMotionBlur(bool value)
	{
		if (SettingsManager.MotionBlur == value)
		{
			return;
		}
		SettingsManager.MotionBlur = value;
		SaveManager.SetBool("motionBlur", SettingsManager.MotionBlur);
		EventManager.TriggerEvent("Event_OnMotionBlurChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x00032C5F File Offset: 0x00030E5F
	public static void UpdateTeam(PlayerTeam team)
	{
		if (SettingsManager.Team == team)
		{
			return;
		}
		SettingsManager.Team = team;
		SaveManager.SetEnum<PlayerTeam>("team", SettingsManager.Team);
		EventManager.TriggerEvent("Event_OnTeamChanged", new Dictionary<string, object>
		{
			{
				"value",
				team
			}
		});
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x00032C9F File Offset: 0x00030E9F
	public static void UpdateRole(PlayerRole role)
	{
		if (SettingsManager.Role == role)
		{
			return;
		}
		SettingsManager.Role = role;
		SaveManager.SetEnum<PlayerRole>("role", SettingsManager.Role);
		EventManager.TriggerEvent("Event_OnRoleChanged", new Dictionary<string, object>
		{
			{
				"value",
				role
			}
		});
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00032CDF File Offset: 0x00030EDF
	public static void UpdateApplyForBothTeams(bool value)
	{
		if (SettingsManager.ApplyForBothTeams == value)
		{
			return;
		}
		SettingsManager.ApplyForBothTeams = value;
		SaveManager.SetBool("applyForBothTeams", SettingsManager.ApplyForBothTeams);
		EventManager.TriggerEvent("Event_OnApplyForBothTeamsChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x00032D1F File Offset: 0x00030F1F
	public static void UpdateFlagID(int value)
	{
		if (SettingsManager.FlagID == value)
		{
			return;
		}
		SettingsManager.FlagID = value;
		SaveManager.SetInt("flagID", SettingsManager.FlagID);
		EventManager.TriggerEvent("Event_OnFlagIDChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x00032D60 File Offset: 0x00030F60
	public static void UpdateHeadgearID(PlayerTeam team, PlayerRole role, int value)
	{
		if (SettingsManager.GetHeadgearID(team, role) == value)
		{
			return;
		}
		if (role != PlayerRole.Attacker)
		{
			if (role == PlayerRole.Goalie)
			{
				if (team != PlayerTeam.Blue)
				{
					if (team == PlayerTeam.Red)
					{
						SettingsManager.HeadgearIDRedGoalie = value;
						SaveManager.SetInt("headgearIDRedGoalie", SettingsManager.HeadgearIDRedGoalie);
					}
				}
				else
				{
					SettingsManager.HeadgearIDBlueGoalie = value;
					SaveManager.SetInt("headgearIDBlueGoalie", SettingsManager.HeadgearIDBlueGoalie);
				}
			}
		}
		else if (team != PlayerTeam.Blue)
		{
			if (team == PlayerTeam.Red)
			{
				SettingsManager.HeadgearIDRedAttacker = value;
				SaveManager.SetInt("headgearIDRedAttacker", SettingsManager.HeadgearIDRedAttacker);
			}
		}
		else
		{
			SettingsManager.HeadgearIDBlueAttacker = value;
			SaveManager.SetInt("headgearIDBlueAttacker", SettingsManager.HeadgearIDBlueAttacker);
		}
		EventManager.TriggerEvent("Event_OnHeadgearIDChanged", new Dictionary<string, object>
		{
			{
				"team",
				team
			},
			{
				"role",
				role
			},
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x00032E32 File Offset: 0x00031032
	public static void UpdateMustacheID(int value)
	{
		if (SettingsManager.MustacheID == value)
		{
			return;
		}
		SettingsManager.MustacheID = value;
		SaveManager.SetInt("mustacheID", SettingsManager.MustacheID);
		EventManager.TriggerEvent("Event_OnMustacheIDChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x00032E72 File Offset: 0x00031072
	public static void UpdateBeardID(int value)
	{
		if (SettingsManager.BeardID == value)
		{
			return;
		}
		SettingsManager.BeardID = value;
		SaveManager.SetInt("beardID", SettingsManager.BeardID);
		EventManager.TriggerEvent("Event_OnBeardIDChanged", new Dictionary<string, object>
		{
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x00032EB4 File Offset: 0x000310B4
	public static void UpdateJerseyID(PlayerTeam team, PlayerRole role, int value)
	{
		if (SettingsManager.GetJerseyID(team, role) == value)
		{
			return;
		}
		if (role != PlayerRole.Attacker)
		{
			if (role == PlayerRole.Goalie)
			{
				if (team != PlayerTeam.Blue)
				{
					if (team == PlayerTeam.Red)
					{
						SettingsManager.JerseyIDRedGoalie = value;
						SaveManager.SetInt("jerseyIDRedGoalie", SettingsManager.JerseyIDRedGoalie);
					}
				}
				else
				{
					SettingsManager.JerseyIDBlueGoalie = value;
					SaveManager.SetInt("jerseyIDBlueGoalie", SettingsManager.JerseyIDBlueGoalie);
				}
			}
		}
		else if (team != PlayerTeam.Blue)
		{
			if (team == PlayerTeam.Red)
			{
				SettingsManager.JerseyIDRedAttacker = value;
				SaveManager.SetInt("jerseyIDRedAttacker", SettingsManager.JerseyIDRedAttacker);
			}
		}
		else
		{
			SettingsManager.JerseyIDBlueAttacker = value;
			SaveManager.SetInt("jerseyIDBlueAttacker", SettingsManager.JerseyIDBlueAttacker);
		}
		EventManager.TriggerEvent("Event_OnJerseyIDChanged", new Dictionary<string, object>
		{
			{
				"team",
				team
			},
			{
				"role",
				role
			},
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x00032F88 File Offset: 0x00031188
	public static void UpdateStickSkinID(PlayerTeam team, PlayerRole role, int value)
	{
		if (SettingsManager.GetStickSkinID(team, role) == value)
		{
			return;
		}
		if (role != PlayerRole.Attacker)
		{
			if (role == PlayerRole.Goalie)
			{
				if (team != PlayerTeam.Blue)
				{
					if (team == PlayerTeam.Red)
					{
						SettingsManager.StickSkinIDRedGoalie = value;
						SaveManager.SetInt("stickSkinIDRedGoalie", SettingsManager.StickSkinIDRedGoalie);
					}
				}
				else
				{
					SettingsManager.StickSkinIDBlueGoalie = value;
					SaveManager.SetInt("stickSkinIDBlueGoalie", SettingsManager.StickSkinIDBlueGoalie);
				}
			}
		}
		else if (team != PlayerTeam.Blue)
		{
			if (team == PlayerTeam.Red)
			{
				SettingsManager.StickSkinIDRedAttacker = value;
				SaveManager.SetInt("stickSkinIDRedAttacker", SettingsManager.StickSkinIDRedAttacker);
			}
		}
		else
		{
			SettingsManager.StickSkinIDBlueAttacker = value;
			SaveManager.SetInt("stickSkinIDBlueAttacker", SettingsManager.StickSkinIDBlueAttacker);
		}
		EventManager.TriggerEvent("Event_OnStickSkinIDChanged", new Dictionary<string, object>
		{
			{
				"team",
				team
			},
			{
				"role",
				role
			},
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x0003305C File Offset: 0x0003125C
	public static void UpdateStickShaftTapeID(PlayerTeam team, PlayerRole role, int value)
	{
		if (SettingsManager.GetStickShaftTapeID(team, role) == value)
		{
			return;
		}
		if (role != PlayerRole.Attacker)
		{
			if (role == PlayerRole.Goalie)
			{
				if (team != PlayerTeam.Blue)
				{
					if (team == PlayerTeam.Red)
					{
						SettingsManager.StickShaftTapeIDRedGoalie = value;
						SaveManager.SetInt("stickShaftTapeIDRedGoalie", SettingsManager.StickShaftTapeIDRedGoalie);
					}
				}
				else
				{
					SettingsManager.StickShaftTapeIDBlueGoalie = value;
					SaveManager.SetInt("stickShaftTapeIDBlueGoalie", SettingsManager.StickShaftTapeIDBlueGoalie);
				}
			}
		}
		else if (team != PlayerTeam.Blue)
		{
			if (team == PlayerTeam.Red)
			{
				SettingsManager.StickShaftTapeIDRedAttacker = value;
				SaveManager.SetInt("stickShaftTapeIDRedAttacker", SettingsManager.StickShaftTapeIDRedAttacker);
			}
		}
		else
		{
			SettingsManager.StickShaftTapeIDBlueAttacker = value;
			SaveManager.SetInt("stickShaftTapeIDBlueAttacker", SettingsManager.StickShaftTapeIDBlueAttacker);
		}
		EventManager.TriggerEvent("Event_OnStickShaftTapeIDChanged", new Dictionary<string, object>
		{
			{
				"team",
				team
			},
			{
				"role",
				role
			},
			{
				"value",
				value
			}
		});
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x00033130 File Offset: 0x00031330
	public static void UpdateStickBladeTapeID(PlayerTeam team, PlayerRole role, int value)
	{
		if (SettingsManager.GetStickBladeTapeID(team, role) == value)
		{
			return;
		}
		if (role != PlayerRole.Attacker)
		{
			if (role == PlayerRole.Goalie)
			{
				if (team != PlayerTeam.Blue)
				{
					if (team == PlayerTeam.Red)
					{
						SettingsManager.StickBladeTapeIDRedGoalie = value;
						SaveManager.SetInt("stickBladeTapeIDRedGoalie", SettingsManager.StickBladeTapeIDRedGoalie);
					}
				}
				else
				{
					SettingsManager.StickBladeTapeIDBlueGoalie = value;
					SaveManager.SetInt("stickBladeTapeIDBlueGoalie", SettingsManager.StickBladeTapeIDBlueGoalie);
				}
			}
		}
		else if (team != PlayerTeam.Blue)
		{
			if (team == PlayerTeam.Red)
			{
				SettingsManager.StickBladeTapeIDRedAttacker = value;
				SaveManager.SetInt("stickBladeTapeIDRedAttacker", SettingsManager.StickBladeTapeIDRedAttacker);
			}
		}
		else
		{
			SettingsManager.StickBladeTapeIDBlueAttacker = value;
			SaveManager.SetInt("stickBladeTapeIDBlueAttacker", SettingsManager.StickBladeTapeIDBlueAttacker);
		}
		EventManager.TriggerEvent("Event_OnStickBladeTapeIDChanged", new Dictionary<string, object>
		{
			{
				"team",
				team
			},
			{
				"role",
				role
			},
			{
				"value",
				value
			}
		});
	}

	// Token: 0x0400064F RID: 1615
	public static DebugMode Debug;

	// Token: 0x04000650 RID: 1616
	public static float CameraAngle;

	// Token: 0x04000651 RID: 1617
	public static PlayerHandedness Handedness;

	// Token: 0x04000652 RID: 1618
	public static bool ShowPuckSilhouette;

	// Token: 0x04000653 RID: 1619
	public static bool ShowPuckOutline;

	// Token: 0x04000654 RID: 1620
	public static bool ShowPuckElevation;

	// Token: 0x04000655 RID: 1621
	public static bool ShowPlayerUsernames;

	// Token: 0x04000656 RID: 1622
	public static float PlayerUsernamesFadeThreshold;

	// Token: 0x04000657 RID: 1623
	public static int MaxMatchmakingPing;

	// Token: 0x04000658 RID: 1624
	public static NetworkBuffering NetworkBuffering;

	// Token: 0x04000659 RID: 1625
	public static bool FilterChatProfanity;

	// Token: 0x0400065A RID: 1626
	public static Units Units;

	// Token: 0x0400065B RID: 1627
	public static bool ShowGameUserInterface;

	// Token: 0x0400065C RID: 1628
	public static bool ShowTeamColorBar;

	// Token: 0x0400065D RID: 1629
	public static float UserInterfaceScale;

	// Token: 0x0400065E RID: 1630
	public static float ChatOpacity;

	// Token: 0x0400065F RID: 1631
	public static float ChatScale;

	// Token: 0x04000660 RID: 1632
	public static float MinimapOpacity;

	// Token: 0x04000661 RID: 1633
	public static float MinimapBackgroundOpacity;

	// Token: 0x04000662 RID: 1634
	public static float MinimapHorizontalPosition;

	// Token: 0x04000663 RID: 1635
	public static float MinimapVerticalPosition;

	// Token: 0x04000664 RID: 1636
	public static float MinimapScale;

	// Token: 0x04000665 RID: 1637
	public static bool ShowMinimapSticks;

	// Token: 0x04000666 RID: 1638
	public static bool ShowMinimapPuckElevation;

	// Token: 0x04000667 RID: 1639
	public static bool ShowMinimapFallenIndicator;

	// Token: 0x04000668 RID: 1640
	public static float GlobalStickSensitivity;

	// Token: 0x04000669 RID: 1641
	public static float HorizontalStickSensitivity;

	// Token: 0x0400066A RID: 1642
	public static float VerticalStickSensitivity;

	// Token: 0x0400066B RID: 1643
	public static float LookSensitivity;

	// Token: 0x0400066C RID: 1644
	public static float GlobalVolume;

	// Token: 0x0400066D RID: 1645
	public static float AmbientVolume;

	// Token: 0x0400066E RID: 1646
	public static float GameVolume;

	// Token: 0x0400066F RID: 1647
	public static float VoiceVolume;

	// Token: 0x04000670 RID: 1648
	public static float UIVolume;

	// Token: 0x04000671 RID: 1649
	public static FullScreenMode FullScreenMode;

	// Token: 0x04000672 RID: 1650
	public static int DisplayIndex;

	// Token: 0x04000673 RID: 1651
	public static int ResolutionIndex;

	// Token: 0x04000674 RID: 1652
	public static bool VSync;

	// Token: 0x04000675 RID: 1653
	public static int FpsLimit;

	// Token: 0x04000676 RID: 1654
	public static float Fov;

	// Token: 0x04000677 RID: 1655
	public static ApplicationQuality Quality;

	// Token: 0x04000678 RID: 1656
	public static global::ShadowQuality ShadowQuality;

	// Token: 0x04000679 RID: 1657
	public static bool MotionBlur;

	// Token: 0x0400067A RID: 1658
	public static PlayerTeam Team;

	// Token: 0x0400067B RID: 1659
	public static PlayerRole Role;

	// Token: 0x0400067C RID: 1660
	public static bool ApplyForBothTeams;

	// Token: 0x0400067D RID: 1661
	public static int FlagID;

	// Token: 0x0400067E RID: 1662
	public static int HeadgearIDBlueAttacker;

	// Token: 0x0400067F RID: 1663
	public static int HeadgearIDRedAttacker;

	// Token: 0x04000680 RID: 1664
	public static int HeadgearIDBlueGoalie;

	// Token: 0x04000681 RID: 1665
	public static int HeadgearIDRedGoalie;

	// Token: 0x04000682 RID: 1666
	public static int MustacheID;

	// Token: 0x04000683 RID: 1667
	public static int BeardID;

	// Token: 0x04000684 RID: 1668
	public static int JerseyIDBlueAttacker;

	// Token: 0x04000685 RID: 1669
	public static int JerseyIDRedAttacker;

	// Token: 0x04000686 RID: 1670
	public static int JerseyIDBlueGoalie;

	// Token: 0x04000687 RID: 1671
	public static int JerseyIDRedGoalie;

	// Token: 0x04000688 RID: 1672
	public static int StickSkinIDBlueAttacker;

	// Token: 0x04000689 RID: 1673
	public static int StickSkinIDRedAttacker;

	// Token: 0x0400068A RID: 1674
	public static int StickSkinIDBlueGoalie;

	// Token: 0x0400068B RID: 1675
	public static int StickSkinIDRedGoalie;

	// Token: 0x0400068C RID: 1676
	public static int StickShaftTapeIDBlueAttacker;

	// Token: 0x0400068D RID: 1677
	public static int StickShaftTapeIDRedAttacker;

	// Token: 0x0400068E RID: 1678
	public static int StickShaftTapeIDBlueGoalie;

	// Token: 0x0400068F RID: 1679
	public static int StickShaftTapeIDRedGoalie;

	// Token: 0x04000690 RID: 1680
	public static int StickBladeTapeIDBlueAttacker;

	// Token: 0x04000691 RID: 1681
	public static int StickBladeTapeIDRedAttacker;

	// Token: 0x04000692 RID: 1682
	public static int StickBladeTapeIDBlueGoalie;

	// Token: 0x04000693 RID: 1683
	public static int StickBladeTapeIDRedGoalie;
}
