using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Steamworks;
using UnityEngine;

// Token: 0x02000167 RID: 359
public static class SteamIntegrationManager
{
	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x000341FF File Offset: 0x000323FF
	public static bool IsOverlayEnabled
	{
		get
		{
			return SteamManager.IsInitialized && SteamUtils.IsOverlayEnabled();
		}
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x0003420F File Offset: 0x0003240F
	public static void Initialize()
	{
		SteamIntegrationManager.RegisterCallbacks();
		SteamIntegrationManagerController.Initialize();
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x0003421B File Offset: 0x0003241B
	public static void Dispose()
	{
		SteamIntegrationManagerController.Dispose();
		SteamIntegrationManager.StopTicketForWebApi();
		SteamIntegrationManager.UnregisterCallbacks();
		SteamIntegrationManager.joinedLobbyIds.Clear();
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x00034238 File Offset: 0x00032438
	private static void RegisterCallbacks()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		SteamIntegrationManager.GetTicketForWebApiCallback = Callback<GetTicketForWebApiResponse_t>.Create(new Callback<GetTicketForWebApiResponse_t>.DispatchDelegate(SteamIntegrationManager.OnGetTicketForWebApiResponse));
		SteamIntegrationManager.MicroTxnAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(new Callback<MicroTxnAuthorizationResponse_t>.DispatchDelegate(SteamIntegrationManager.OnMicroTxnAuthorizationResponse));
		SteamIntegrationManager.GameRichPresenceJoinRequested = Callback<GameRichPresenceJoinRequested_t>.Create(new Callback<GameRichPresenceJoinRequested_t>.DispatchDelegate(SteamIntegrationManager.OnGameRichPresenceJoinRequested));
		SteamIntegrationManager.NewUrlLaunchParameters = Callback<NewUrlLaunchParameters_t>.Create(new Callback<NewUrlLaunchParameters_t>.DispatchDelegate(SteamIntegrationManager.OnNewUrlLaunchParameters));
		SteamIntegrationManager.LobbyCreatedCallback = Callback<LobbyCreated_t>.Create(new Callback<LobbyCreated_t>.DispatchDelegate(SteamIntegrationManager.OnLobbyCreated));
		SteamIntegrationManager.LobbyEnterCallback = Callback<LobbyEnter_t>.Create(new Callback<LobbyEnter_t>.DispatchDelegate(SteamIntegrationManager.OnLobbyEntered));
		SteamIntegrationManager.LobbyChatUpdateCallback = Callback<LobbyChatUpdate_t>.Create(new Callback<LobbyChatUpdate_t>.DispatchDelegate(SteamIntegrationManager.OnLobbyChatUpdate));
		SteamIntegrationManager.GameLobbyJoinRequestedCallback = Callback<GameLobbyJoinRequested_t>.Create(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(SteamIntegrationManager.OnGameLobbyJoinRequested));
		SteamIntegrationManager.PersonaStateChangeCallback = Callback<PersonaStateChange_t>.Create(new Callback<PersonaStateChange_t>.DispatchDelegate(SteamIntegrationManager.OnPersonaStateChange));
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x00034314 File Offset: 0x00032514
	private static void UnregisterCallbacks()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		SteamIntegrationManager.GetTicketForWebApiCallback.Unregister();
		SteamIntegrationManager.MicroTxnAuthorizationResponse.Unregister();
		SteamIntegrationManager.GameRichPresenceJoinRequested.Unregister();
		SteamIntegrationManager.NewUrlLaunchParameters.Unregister();
		SteamIntegrationManager.LobbyCreatedCallback.Unregister();
		SteamIntegrationManager.LobbyEnterCallback.Unregister();
		SteamIntegrationManager.LobbyChatUpdateCallback.Unregister();
		SteamIntegrationManager.GameLobbyJoinRequestedCallback.Unregister();
		SteamIntegrationManager.PersonaStateChangeCallback.Unregister();
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x00034383 File Offset: 0x00032583
	public static void SetRichPresenceMainMenu()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		SteamFriends.ClearRichPresence();
		SteamFriends.SetRichPresence("steam_display", "#Status_MainMenu");
		SteamFriends.SetRichPresence("status", "In the changing room");
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x000343B4 File Offset: 0x000325B4
	public static void SetRichPresenceSpectating(Server server, int playerCount)
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		SteamFriends.SetRichPresence("steam_player_group", string.Format("{0}:{1}", server.IpAddress, server.Port));
		SteamFriends.SetRichPresence("steam_player_group_size", string.Format("{0}", playerCount));
		SteamFriends.SetRichPresence("steam_display", "#Status_Spectating");
		SteamFriends.SetRichPresence("status", "Spectating");
		SteamFriends.SetRichPresence("connect", string.Format("+ipAddress {0} +port {1}", server.IpAddress, server.Port));
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x0003445C File Offset: 0x0003265C
	public static void SetRichPresencePlaying(Server server, int playerCount)
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		SteamFriends.SetRichPresence("steam_player_group", string.Format("{0}:{1}", server.IpAddress, server.Port));
		SteamFriends.SetRichPresence("steam_player_group_size", string.Format("{0}", playerCount));
		SteamFriends.SetRichPresence("steam_display", "#Status_Playing");
		SteamFriends.SetRichPresence("status", "Playing");
		SteamFriends.SetRichPresence("connect", string.Format("+ipAddress {0} +port {1}", server.IpAddress, server.Port));
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x00034504 File Offset: 0x00032704
	public static void UpdateRichPresenceScore(bool show, int period, int blueScore, int redScore)
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		string pchValue = show ? string.Format(" | P{0} {1} - {2}", period, blueScore, redScore) : " ";
		SteamFriends.SetRichPresence("score", pchValue);
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x0003454C File Offset: 0x0003274C
	public static void UpdateRichPresenceRole(PlayerRole role)
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		string pchValue = role.ToString().Replace("Attacker", "Skater");
		SteamFriends.SetRichPresence("role", pchValue);
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x0003458C File Offset: 0x0003278C
	public static void UpdateRichPresenceTeam(PlayerTeam team)
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		string pchValue = team.ToString().Replace("Blue", "Team Blue").Replace("Red", "Team Red");
		SteamFriends.SetRichPresence("team", pchValue);
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x000345DC File Offset: 0x000327DC
	public static void UpdateRichPresencePhase(GamePhase phase)
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		string pchValue;
		if (phase == GamePhase.Warmup)
		{
			pchValue = "Warming up";
		}
		else
		{
			pchValue = "Playing";
		}
		SteamFriends.SetRichPresence("phase", pchValue);
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x0003460F File Offset: 0x0003280F
	public static void CreateLobby()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		SteamIntegrationManager.Logger.Info("Creating lobby");
		SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 128);
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x00034634 File Offset: 0x00032834
	public static void JoinLobby(string lobbyId)
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		if (SteamIntegrationManager.IsInLobby(lobbyId))
		{
			return;
		}
		SteamIntegrationManager.Logger.Info("Joining lobby " + lobbyId);
		SteamMatchmaking.JoinLobby(new CSteamID(ulong.Parse(lobbyId)));
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x00034670 File Offset: 0x00032870
	public static void LeaveLobby(string lobbyId)
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		if (!SteamIntegrationManager.IsInLobby(lobbyId))
		{
			return;
		}
		SteamIntegrationManager.Logger.Info("Leaving lobby " + lobbyId);
		SteamMatchmaking.LeaveLobby(new CSteamID(ulong.Parse(lobbyId)));
		SteamIntegrationManager.joinedLobbyIds.Remove(lobbyId);
		EventManager.TriggerEvent("Event_OnLobbyLeft", new Dictionary<string, object>
		{
			{
				"lobbyId",
				lobbyId
			}
		});
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x000346DC File Offset: 0x000328DC
	public static void LeaveAllLobbies()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		SteamIntegrationManager.Logger.Info("Leaving all lobbies");
		foreach (string lobbyId in SteamIntegrationManager.joinedLobbyIds.ToList<string>())
		{
			SteamIntegrationManager.LeaveLobby(lobbyId);
		}
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x00034748 File Offset: 0x00032948
	public static bool IsInLobby(string lobbyId)
	{
		return SteamIntegrationManager.joinedLobbyIds.Contains(lobbyId);
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x00034758 File Offset: 0x00032958
	public static string GetSteamId()
	{
		if (!SteamManager.IsInitialized)
		{
			return null;
		}
		return SteamUser.GetSteamID().ToString();
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x00034781 File Offset: 0x00032981
	public static void GetTicketForWebApi()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		SteamIntegrationManager.RequestTicketForWebApi();
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x00034790 File Offset: 0x00032990
	public static void StopTicketForWebApi()
	{
		Tween tween = SteamIntegrationManager.ticketTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		SteamIntegrationManager.ticketTween = null;
		SteamIntegrationManager.CancelPendingTicket();
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x000347B0 File Offset: 0x000329B0
	private static void RequestTicketForWebApi()
	{
		Tween tween = SteamIntegrationManager.ticketTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		SteamIntegrationManager.ticketTween = null;
		if (!SteamUser.BLoggedOn())
		{
			SteamIntegrationManager.Logger.Warning("Steam client is not logged on to Steam servers; cannot fetch web API auth ticket");
			EventManager.TriggerEvent("Event_OnSteamAuthenticationStalled", null);
			SteamIntegrationManager.ScheduleTicketRetry();
			return;
		}
		SteamIntegrationManager.CancelPendingTicket();
		SteamIntegrationManager.Logger.Info("Requesting web API auth ticket");
		EventManager.TriggerEvent("Event_OnSteamAuthenticationStarted", null);
		SteamIntegrationManager.pendingTicketHandle = SteamUser.GetAuthTicketForWebApi("*");
		SteamIntegrationManager.ticketTween = DOVirtual.DelayedCall(10f, delegate
		{
			SteamIntegrationManager.Logger.Warning(string.Format("Web API auth ticket request did not respond within {0}s; retrying", 10f));
			EventManager.TriggerEvent("Event_OnSteamAuthenticationStalled", null);
			SteamIntegrationManager.RequestTicketForWebApi();
		}, true);
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x00034858 File Offset: 0x00032A58
	private static void ScheduleTicketRetry()
	{
		Tween tween = SteamIntegrationManager.ticketTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		SteamIntegrationManager.ticketTween = DOVirtual.DelayedCall(3f, new TweenCallback(SteamIntegrationManager.RequestTicketForWebApi), true);
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x00034887 File Offset: 0x00032A87
	private static void CancelPendingTicket()
	{
		if (SteamIntegrationManager.pendingTicketHandle == HAuthTicket.Invalid)
		{
			return;
		}
		SteamUser.CancelAuthTicket(SteamIntegrationManager.pendingTicketHandle);
		SteamIntegrationManager.pendingTicketHandle = HAuthTicket.Invalid;
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x000348B0 File Offset: 0x00032AB0
	public static void GetLaunchCommandLine()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		string text;
		SteamApps.GetLaunchCommandLine(out text, 256);
		string[] array = text.Split(" ", StringSplitOptions.None);
		if (array.Length != 0)
		{
			SteamIntegrationManager.Logger.Info(string.Format("GotLaunchCommandLine: {0} ({1})", text, array.Length));
			EventManager.TriggerEvent("Event_OnGotLaunchCommandLine", new Dictionary<string, object>
			{
				{
					"args",
					array
				}
			});
		}
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x0003491C File Offset: 0x00032B1C
	public static Texture2D GetAvatar(string steamId, AvatarSize size)
	{
		if (!SteamManager.IsInitialized)
		{
			return null;
		}
		CSteamID steamIDFriend = new CSteamID(ulong.Parse(steamId));
		int num;
		switch (size)
		{
		case AvatarSize.Small:
			num = SteamFriends.GetSmallFriendAvatar(steamIDFriend);
			break;
		case AvatarSize.Medium:
			num = SteamFriends.GetMediumFriendAvatar(steamIDFriend);
			break;
		case AvatarSize.Large:
			num = SteamFriends.GetLargeFriendAvatar(steamIDFriend);
			break;
		default:
			num = SteamFriends.GetMediumFriendAvatar(steamIDFriend);
			break;
		}
		int iImage = num;
		uint num2;
		uint num3;
		SteamUtils.GetImageSize(iImage, out num2, out num3);
		byte[] array = new byte[num2 * num3 * 4U];
		bool imageRGBA = SteamUtils.GetImageRGBA(iImage, array, array.Length);
		byte[] array2 = new byte[array.Length];
		int num4 = (int)(num2 * 4U);
		int num5 = 0;
		while ((long)num5 < (long)((ulong)num3))
		{
			Buffer.BlockCopy(array, num5 * num4, array2, (int)((num3 - 1U - (uint)num5) * (uint)num4), num4);
			num5++;
		}
		if (imageRGBA)
		{
			Texture2D texture2D = new Texture2D((int)num2, (int)num3, TextureFormat.RGBA32, false);
			texture2D.LoadRawTextureData(array2);
			texture2D.Apply();
			return texture2D;
		}
		return null;
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x000349F4 File Offset: 0x00032BF4
	public static string GetUsername(string steamId)
	{
		if (!SteamManager.IsInitialized)
		{
			return null;
		}
		CSteamID csteamID = new CSteamID(ulong.Parse(steamId));
		if (csteamID == SteamUser.GetSteamID())
		{
			return SteamFriends.GetPersonaName();
		}
		return SteamFriends.GetFriendPersonaName(csteamID);
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x00034A30 File Offset: 0x00032C30
	public static string[] GetFriendSteamIds(bool includeOffline = false)
	{
		if (!SteamManager.IsInitialized)
		{
			return new string[0];
		}
		int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
		List<string> list = new List<string>();
		for (int i = 0; i < friendCount; i++)
		{
			string text = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate).ToString();
			if (SteamIntegrationManager.IsFriendOnline(text) || includeOffline)
			{
				list.Add(text);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x00034A98 File Offset: 0x00032C98
	public static string GetLobbyOwnerSteamId(string lobbyId)
	{
		if (!SteamManager.IsInitialized)
		{
			return null;
		}
		return SteamMatchmaking.GetLobbyOwner(new CSteamID(ulong.Parse(lobbyId))).ToString();
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x00034ACC File Offset: 0x00032CCC
	public static string[] GetLobbyMemberSteamIds(string lobbyId)
	{
		if (!SteamManager.IsInitialized)
		{
			return new string[0];
		}
		CSteamID steamIDLobby = new CSteamID(ulong.Parse(lobbyId));
		int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(steamIDLobby);
		List<string> list = new List<string>();
		for (int i = 0; i < numLobbyMembers; i++)
		{
			string item = SteamMatchmaking.GetLobbyMemberByIndex(steamIDLobby, i).ToString();
			list.Add(item);
		}
		return list.ToArray();
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x00034B33 File Offset: 0x00032D33
	public static bool IsFriend(string steamId)
	{
		return SteamManager.IsInitialized && SteamFriends.GetFriendRelationship(new CSteamID(ulong.Parse(steamId))) == EFriendRelationship.k_EFriendRelationshipFriend;
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x00034B51 File Offset: 0x00032D51
	public static bool IsFriendOnline(string steamId)
	{
		return SteamManager.IsInitialized && SteamFriends.GetFriendPersonaState(new CSteamID(ulong.Parse(steamId))) > EPersonaState.k_EPersonaStateOffline;
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x00034B70 File Offset: 0x00032D70
	public static void InviteToLobby(string lobbyId, string invitedSteamId)
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		CSteamID steamIDLobby = new CSteamID(ulong.Parse(lobbyId));
		CSteamID steamIDInvitee = new CSteamID(ulong.Parse(invitedSteamId));
		SteamMatchmaking.InviteUserToLobby(steamIDLobby, steamIDInvitee);
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x00034BA4 File Offset: 0x00032DA4
	private static void OnGetTicketForWebApiResponse(GetTicketForWebApiResponse_t response)
	{
		Tween tween = SteamIntegrationManager.ticketTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		SteamIntegrationManager.ticketTween = null;
		SteamIntegrationManager.pendingTicketHandle = HAuthTicket.Invalid;
		if (response.m_eResult != EResult.k_EResultOK)
		{
			SteamIntegrationManager.Logger.Warning(string.Format("Web API auth ticket request failed ({0}); retrying", response.m_eResult));
			EventManager.TriggerEvent("Event_OnSteamAuthenticationStalled", null);
			SteamIntegrationManager.ScheduleTicketRetry();
			return;
		}
		byte[] rgubTicket = response.m_rgubTicket;
		string value = BitConverter.ToString(rgubTicket, 0, rgubTicket.Length).Replace("-", string.Empty);
		EventManager.TriggerEvent("Event_OnGetTicketForWebApiResponse", new Dictionary<string, object>
		{
			{
				"ticket",
				value
			}
		});
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x00034C48 File Offset: 0x00032E48
	private static void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t response)
	{
		EventManager.TriggerEvent("Event_OnMicroTxnAuthorizationResponse", new Dictionary<string, object>
		{
			{
				"orderId",
				response.m_ulOrderID
			},
			{
				"authorized",
				Convert.ToBoolean(response.m_bAuthorized)
			}
		});
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x00034C95 File Offset: 0x00032E95
	private static void OnGameRichPresenceJoinRequested(GameRichPresenceJoinRequested_t response)
	{
		EventManager.TriggerEvent("Event_OnGameRichPresenceJoinRequested", new Dictionary<string, object>
		{
			{
				"args",
				response.m_rgchConnect.Split(" ", StringSplitOptions.None)
			}
		});
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x00034CC3 File Offset: 0x00032EC3
	private static void OnNewUrlLaunchParameters(NewUrlLaunchParameters_t response)
	{
		SteamIntegrationManager.GetLaunchCommandLine();
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x00034CCC File Offset: 0x00032ECC
	private static void OnLobbyCreated(LobbyCreated_t result)
	{
		if (result.m_eResult != EResult.k_EResultOK)
		{
			return;
		}
		string text = result.m_ulSteamIDLobby.ToString();
		SteamIntegrationManager.Logger.Info("Lobby " + text + " created");
		if (!SteamIntegrationManager.joinedLobbyIds.Contains(text))
		{
			SteamIntegrationManager.joinedLobbyIds.Add(text);
		}
		EventManager.TriggerEvent("Event_OnLobbyCreated", new Dictionary<string, object>
		{
			{
				"lobbyId",
				text
			}
		});
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x00034D40 File Offset: 0x00032F40
	private static void OnLobbyEntered(LobbyEnter_t result)
	{
		string text = result.m_ulSteamIDLobby.ToString();
		SteamIntegrationManager.Logger.Info("Lobby " + text + " entered");
		if (!SteamIntegrationManager.joinedLobbyIds.Contains(text))
		{
			SteamIntegrationManager.joinedLobbyIds.Add(text);
		}
		EventManager.TriggerEvent("Event_OnLobbyEntered", new Dictionary<string, object>
		{
			{
				"lobbyId",
				text
			},
			{
				"ownerSteamId",
				SteamIntegrationManager.GetLobbyOwnerSteamId(text)
			},
			{
				"memberSteamIds",
				SteamIntegrationManager.GetLobbyMemberSteamIds(text)
			}
		});
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x00034DCC File Offset: 0x00032FCC
	private static void OnLobbyChatUpdate(LobbyChatUpdate_t result)
	{
		string text = result.m_ulSteamIDLobby.ToString();
		SteamIntegrationManager.Logger.Info("Lobby " + text + " updated");
		EventManager.TriggerEvent("Event_OnLobbyChatUpdate", new Dictionary<string, object>
		{
			{
				"lobbyId",
				text
			},
			{
				"ownerSteamId",
				SteamIntegrationManager.GetLobbyOwnerSteamId(text)
			},
			{
				"memberSteamIds",
				SteamIntegrationManager.GetLobbyMemberSteamIds(text)
			}
		});
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x00034E40 File Offset: 0x00033040
	private static void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t result)
	{
		CSteamID steamIDLobby = result.m_steamIDLobby;
		string text = steamIDLobby.ToString();
		SteamIntegrationManager.Logger.Info("Lobby " + text + " join requested");
		EventManager.TriggerEvent("Event_OnGameLobbyJoinRequested", new Dictionary<string, object>
		{
			{
				"lobbyId",
				text
			}
		});
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x00034E98 File Offset: 0x00033098
	private static void OnPersonaStateChange(PersonaStateChange_t result)
	{
		string value = result.m_ulSteamID.ToString();
		if ((result.m_nChangeFlags & (EPersonaChange.k_EPersonaChangeName | EPersonaChange.k_EPersonaChangeStatus | EPersonaChange.k_EPersonaChangeAvatar | EPersonaChange.k_EPersonaChangeRelationshipChanged | EPersonaChange.k_EPersonaChangeNickname)) != (EPersonaChange)0)
		{
			EventManager.TriggerEvent("Event_OnPersonaStateChange", new Dictionary<string, object>
			{
				{
					"steamId",
					value
				}
			});
		}
	}

	// Token: 0x04000699 RID: 1689
	private static readonly global::Logger Logger = new global::Logger("SteamIntegrationManager");

	// Token: 0x0400069A RID: 1690
	private static List<string> joinedLobbyIds = new List<string>();

	// Token: 0x0400069B RID: 1691
	private static Tween ticketTween;

	// Token: 0x0400069C RID: 1692
	private static HAuthTicket pendingTicketHandle = HAuthTicket.Invalid;

	// Token: 0x0400069D RID: 1693
	private static Callback<GetTicketForWebApiResponse_t> GetTicketForWebApiCallback;

	// Token: 0x0400069E RID: 1694
	private static Callback<MicroTxnAuthorizationResponse_t> MicroTxnAuthorizationResponse;

	// Token: 0x0400069F RID: 1695
	private static Callback<GameRichPresenceJoinRequested_t> GameRichPresenceJoinRequested;

	// Token: 0x040006A0 RID: 1696
	private static Callback<NewUrlLaunchParameters_t> NewUrlLaunchParameters;

	// Token: 0x040006A1 RID: 1697
	private static Callback<LobbyCreated_t> LobbyCreatedCallback;

	// Token: 0x040006A2 RID: 1698
	private static Callback<LobbyEnter_t> LobbyEnterCallback;

	// Token: 0x040006A3 RID: 1699
	private static Callback<LobbyChatUpdate_t> LobbyChatUpdateCallback;

	// Token: 0x040006A4 RID: 1700
	private static Callback<GameLobbyJoinRequested_t> GameLobbyJoinRequestedCallback;

	// Token: 0x040006A5 RID: 1701
	private static Callback<PersonaStateChange_t> PersonaStateChangeCallback;
}
