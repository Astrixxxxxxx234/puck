using System;
using System.Collections.Generic;
using System.Net;
using DG.Tweening;
using Steamworks;

// Token: 0x0200016A RID: 362
public static class SteamManager
{
	// Token: 0x06000B25 RID: 2853 RVA: 0x00035741 File Offset: 0x00033941
	public static void Initialize()
	{
		SteamManagerController.Initialize();
		SteamManager.InitializeSteam();
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x0003574D File Offset: 0x0003394D
	public static void Dispose()
	{
		SteamManager.DisposeSteam();
		SteamManagerController.Dispose();
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x0003575C File Offset: 0x0003395C
	private static void InitializeSteam()
	{
		if (SteamManager.IsInitialized)
		{
			return;
		}
		SteamManager.Logger.Info("Initializing Steam");
		EventManager.TriggerEvent("Event_OnSteamInitializationStarted", null);
		if (ApplicationManager.IsDedicatedGameServer)
		{
			SteamManager.IsInitialized = GameServer.Init(BitConverter.ToUInt32(IPAddress.Any.GetAddressBytes(), 0), 0, 0, EServerMode.eServerModeNoAuthentication, null);
		}
		else
		{
			SteamManager.IsInitialized = SteamAPI.Init();
		}
		if (!SteamManager.IsInitialized)
		{
			SteamManager.steamInitializationAttempts++;
			if (ApplicationManager.IsDedicatedGameServer)
			{
				SteamManager.Logger.Info(string.Format("Failed to initialize as game server (attempt {0})", SteamManager.steamInitializationAttempts));
			}
			else
			{
				SteamManager.Logger.Info(string.Format("Failed to initialize as client (attempt {0})", SteamManager.steamInitializationAttempts));
			}
			EventManager.TriggerEvent("Event_OnSteamInitializationFailed", new Dictionary<string, object>
			{
				{
					"attempts",
					SteamManager.steamInitializationAttempts
				}
			});
			Tween tween = SteamManager.steamInitializationRetryTween;
			if (tween != null)
			{
				tween.Kill(false);
			}
			SteamManager.steamInitializationRetryTween = DOVirtual.DelayedCall(5f, delegate
			{
				SteamManager.Logger.Info("Retrying Steam initialization");
				SteamManager.InitializeSteam();
			}, true);
			return;
		}
		SteamManager.steamInitializationAttempts = 0;
		SteamManager.RegisterCallbacks();
		SteamManager.StartCallbackLoop();
		if (ApplicationManager.IsDedicatedGameServer)
		{
			SteamManager.Logger.Info("Initialized as game server");
			EventManager.TriggerEvent("Event_OnSteamInitialized", null);
			SteamGameServer.LogOnAnonymous();
			return;
		}
		SteamManager.Logger.Info("Initialized as client");
		EventManager.TriggerEvent("Event_OnSteamInitialized", null);
		SteamManager.OnSteamServersConnected(default(SteamServersConnected_t));
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x000358D8 File Offset: 0x00033AD8
	private static void DisposeSteam()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		Tween tween = SteamManager.steamInitializationRetryTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		if (ApplicationManager.IsDedicatedGameServer)
		{
			GameServer.Shutdown();
		}
		else
		{
			SteamAPI.Shutdown();
		}
		SteamManager.StopCallbackLoop();
		SteamManager.UnregisterCallbacks();
		SteamManager.IsInitialized = false;
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x00035918 File Offset: 0x00033B18
	private static void RegisterCallbacks()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		if (ApplicationManager.IsDedicatedGameServer)
		{
			SteamManager.steamServersConnectedCallback = Callback<SteamServersConnected_t>.CreateGameServer(new Callback<SteamServersConnected_t>.DispatchDelegate(SteamManager.OnSteamServersConnected));
			SteamManager.steamServerConnectFailureCallback = Callback<SteamServerConnectFailure_t>.CreateGameServer(new Callback<SteamServerConnectFailure_t>.DispatchDelegate(SteamManager.OnSteamServerConnectFailure));
			SteamManager.steamServersDisconnectedCallback = Callback<SteamServersDisconnected_t>.CreateGameServer(new Callback<SteamServersDisconnected_t>.DispatchDelegate(SteamManager.OnSteamServersDisconnected));
			return;
		}
		SteamManager.steamServersConnectedCallback = Callback<SteamServersConnected_t>.Create(new Callback<SteamServersConnected_t>.DispatchDelegate(SteamManager.OnSteamServersConnected));
		SteamManager.steamServerConnectFailureCallback = Callback<SteamServerConnectFailure_t>.Create(new Callback<SteamServerConnectFailure_t>.DispatchDelegate(SteamManager.OnSteamServerConnectFailure));
		SteamManager.steamServersDisconnectedCallback = Callback<SteamServersDisconnected_t>.Create(new Callback<SteamServersDisconnected_t>.DispatchDelegate(SteamManager.OnSteamServersDisconnected));
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x000359B9 File Offset: 0x00033BB9
	private static void UnregisterCallbacks()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		SteamManager.steamServersConnectedCallback.Unregister();
		SteamManager.steamServerConnectFailureCallback.Unregister();
		SteamManager.steamServersDisconnectedCallback.Unregister();
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x000359E4 File Offset: 0x00033BE4
	private static void StartCallbackLoop()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		Tween tween = SteamManager.callbackTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		SteamManager.callbackTween = DOVirtual.DelayedCall(SteamManager.RunCallbackInterval, delegate
		{
			if (ApplicationManager.IsDedicatedGameServer)
			{
				GameServer.RunCallbacks();
			}
			else
			{
				SteamAPI.RunCallbacks();
			}
			SteamManager.StartCallbackLoop();
		}, true);
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x00035A39 File Offset: 0x00033C39
	private static void StopCallbackLoop()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		Tween tween = SteamManager.callbackTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		SteamManager.callbackTween = null;
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x00035A5A File Offset: 0x00033C5A
	private static void OnSteamServersConnected(SteamServersConnected_t callback)
	{
		SteamManager.Logger.Info("Connected to Steam");
		SteamIntegrationManager.Initialize();
		SteamWorkshopManager.Initialize();
		SteamManager.IsConnected = true;
		EventManager.TriggerEvent("Event_OnSteamConnected", null);
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x00035A86 File Offset: 0x00033C86
	private static void OnSteamServerConnectFailure(SteamServerConnectFailure_t callback)
	{
		SteamManager.Logger.Error(string.Format("Failed to connect to Steam: {0}", callback.m_eResult));
		EventManager.TriggerEvent("Event_OnSteamConnectionFailed", null);
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x00035AB2 File Offset: 0x00033CB2
	private static void OnSteamServersDisconnected(SteamServersDisconnected_t callback)
	{
		SteamManager.Logger.Warning(string.Format("Disconnected from Steam: {0}", callback.m_eResult));
		SteamManager.IsConnected = false;
		SteamWorkshopManager.Dispose();
		SteamIntegrationManager.Dispose();
		EventManager.TriggerEvent("Event_OnSteamDisconnected", null);
	}

	// Token: 0x040006A8 RID: 1704
	private static readonly Logger Logger = new Logger("SteamManager");

	// Token: 0x040006A9 RID: 1705
	public static bool IsInitialized = false;

	// Token: 0x040006AA RID: 1706
	public static float RunCallbackInterval = 0.033333335f;

	// Token: 0x040006AB RID: 1707
	public static bool IsConnected = false;

	// Token: 0x040006AC RID: 1708
	private static Callback<SteamServersConnected_t> steamServersConnectedCallback;

	// Token: 0x040006AD RID: 1709
	private static Callback<SteamServerConnectFailure_t> steamServerConnectFailureCallback;

	// Token: 0x040006AE RID: 1710
	private static Callback<SteamServersDisconnected_t> steamServersDisconnectedCallback;

	// Token: 0x040006AF RID: 1711
	private static Tween callbackTween;

	// Token: 0x040006B0 RID: 1712
	private static Tween steamInitializationRetryTween;

	// Token: 0x040006B1 RID: 1713
	private static int steamInitializationAttempts = 0;
}
