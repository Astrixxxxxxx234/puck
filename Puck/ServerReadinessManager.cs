using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

// Token: 0x0200015E RID: 350
public static class ServerReadinessManager
{
	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000A4F RID: 2639 RVA: 0x00031254 File Offset: 0x0002F454
	// (set) Token: 0x06000A50 RID: 2640 RVA: 0x0003125B File Offset: 0x0002F45B
	public static ServerReadinessPhase Phase { get; private set; } = ServerReadinessPhase.NotReady;

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000A51 RID: 2641 RVA: 0x00031263 File Offset: 0x0002F463
	public static bool IsReady
	{
		get
		{
			return ServerReadinessManager.Phase == ServerReadinessPhase.Ready;
		}
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x0003126D File Offset: 0x0002F46D
	public static void Initialize()
	{
		ServerReadinessManagerController.Initialize();
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x00031274 File Offset: 0x0002F474
	public static void Dispose()
	{
		ServerReadinessManager.Reset();
		ServerReadinessManagerController.Dispose();
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x00031280 File Offset: 0x0002F480
	public static void BeginTracking(ServerConfig config)
	{
		ServerReadinessManager.serverConfig = config;
		ServerReadinessManager.requiredModIds = config.EnabledModIds;
		ServerReadinessManager.modWaitTimedOut = false;
		ServerReadinessManager.SetPhase(ServerReadinessPhase.Starting);
		ServerReadinessManager.Logger.Info(string.Format("Waiting for backend authentication and {0} server mod(s) to download and enable", ServerReadinessManager.requiredModIds.Length));
		if (ServerReadinessManager.Evaluate())
		{
			return;
		}
		ServerReadinessManager.StartProgressPoll();
		ServerReadinessManager.StartTimeout();
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x000312DC File Offset: 0x0002F4DC
	public static bool Evaluate()
	{
		if (ServerReadinessManager.Phase == ServerReadinessPhase.Ready)
		{
			return true;
		}
		if (!ServerReadinessManager.IsAuthenticated())
		{
			return false;
		}
		if (ServerReadinessManager.modWaitTimedOut || ServerReadinessManager.requiredModIds.All(new Func<string, bool>(ServerReadinessManager.IsModReady)))
		{
			ServerReadinessManager.MarkReady();
			return true;
		}
		return false;
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x00031318 File Offset: 0x0002F518
	public static void Reset()
	{
		ServerReadinessManager.Stop();
		ServerReadinessManager.serverConfig = null;
		ServerReadinessManager.requiredModIds = new string[0];
		ServerReadinessManager.modWaitTimedOut = false;
		ServerReadinessManager.SetPhase(ServerReadinessPhase.NotReady);
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x0003133C File Offset: 0x0002F53C
	private static bool IsAuthenticated()
	{
		return BackendManager.ServerState.AuthenticationPhase == AuthenticationPhase.Authenticated;
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x0003134C File Offset: 0x0002F54C
	private static bool IsModReady(string modId)
	{
		Mod modById = ModManager.GetModById(modId);
		return modById != null && modById.IsReady && modById.IsEnabled;
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x00031374 File Offset: 0x0002F574
	private static void MarkReady()
	{
		if (ServerReadinessManager.Phase == ServerReadinessPhase.Ready)
		{
			return;
		}
		ServerReadinessManager.Stop();
		ServerReadinessManager.Logger.Info("Backend authenticated and all server mods ready; loading level");
		ServerReadinessManager.SetPhase(ServerReadinessPhase.Ready);
		EventManager.TriggerEvent("Event_Server_OnReady", new Dictionary<string, object>
		{
			{
				"serverConfig",
				ServerReadinessManager.serverConfig
			}
		});
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x000313C4 File Offset: 0x0002F5C4
	public static void AnnounceReady()
	{
		if (ServerReadinessManager.Phase != ServerReadinessPhase.Ready || ServerReadinessManager.serverConfig == null)
		{
			return;
		}
		ServerData serverData = BackendManager.ServerState.ServerData;
		string arg = ((serverData != null) ? serverData.ipAddress : null) ?? "0.0.0.0";
		ServerData serverData2 = BackendManager.ServerState.ServerData;
		ushort num = (serverData2 != null) ? serverData2.port : ServerReadinessManager.serverConfig.port;
		ServerReadinessManager.Logger.Info(string.Format("Puck B{0} is ready to accept clients on {1}:{2}!", ApplicationManager.Version, arg, num));
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x00031448 File Offset: 0x0002F648
	private static void SetPhase(ServerReadinessPhase phase)
	{
		if (ServerReadinessManager.Phase == phase)
		{
			return;
		}
		ServerReadinessPhase phase2 = ServerReadinessManager.Phase;
		ServerReadinessManager.Phase = phase;
		EventManager.TriggerEvent("Event_Server_OnReadinessChanged", new Dictionary<string, object>
		{
			{
				"oldPhase",
				phase2
			},
			{
				"newPhase",
				phase
			}
		});
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x0003149B File Offset: 0x0002F69B
	private static void Stop()
	{
		Tween tween = ServerReadinessManager.progressPollTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		ServerReadinessManager.progressPollTween = null;
		Tween tween2 = ServerReadinessManager.timeoutTween;
		if (tween2 != null)
		{
			tween2.Kill(false);
		}
		ServerReadinessManager.timeoutTween = null;
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x000314CB File Offset: 0x0002F6CB
	private static void StartProgressPoll()
	{
		Tween tween = ServerReadinessManager.progressPollTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		ServerReadinessManager.progressPollTween = DOVirtual.DelayedCall(2f, new TweenCallback(ServerReadinessManager.LogProgress), true).SetLoops(-1);
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x00031500 File Offset: 0x0002F700
	private static void StartTimeout()
	{
		Tween tween = ServerReadinessManager.timeoutTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		ServerReadinessManager.timeoutTween = DOVirtual.DelayedCall(300f, new TweenCallback(ServerReadinessManager.OnTimeout), true);
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x00031530 File Offset: 0x0002F730
	private static void OnTimeout()
	{
		ServerReadinessManager.modWaitTimedOut = true;
		string[] value = (from modId in ServerReadinessManager.requiredModIds
		where !ServerReadinessManager.IsModReady(modId)
		select modId).ToArray<string>();
		if (!ServerReadinessManager.IsAuthenticated())
		{
			ServerReadinessManager.Logger.Error(string.Format("Timed out after {0}s but the server is not authenticated with the backend; staying closed until it authenticates (pending mods: [{1}])", 300f, string.Join(", ", value)));
			return;
		}
		ServerReadinessManager.Logger.Error(string.Format("Timed out after {0}s waiting for server mod(s) [{1}]; accepting players anyway", 300f, string.Join(", ", value)));
		ServerReadinessManager.MarkReady();
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x000315D4 File Offset: 0x0002F7D4
	private static void LogProgress()
	{
		if (!ServerReadinessManager.IsAuthenticated())
		{
			ServerReadinessManager.Logger.Info("Waiting for backend authentication...");
		}
		int num = ServerReadinessManager.requiredModIds.Count(new Func<string, bool>(ServerReadinessManager.IsModReady));
		ServerReadinessManager.Logger.Info(string.Format("Server mods: {0}/{1} ready", num, ServerReadinessManager.requiredModIds.Length));
		foreach (string text in ServerReadinessManager.requiredModIds)
		{
			if (!ServerReadinessManager.IsModReady(text))
			{
				ulong num2;
				ulong num3;
				if (SteamWorkshopManager.GetItemDownloadInfo(text, out num2, out num3) && num3 > 0UL)
				{
					float num4 = num2 / 1048576f;
					float num5 = num3 / 1048576f;
					float num6 = num2 / num3 * 100f;
					ServerReadinessManager.Logger.Info(string.Format("Downloading {0}: {1:0.0}% ({2:0.0}/{3:0.0} MB)", new object[]
					{
						text,
						num6,
						num4,
						num5
					}));
				}
				else
				{
					ServerReadinessManager.Logger.Info("Downloading " + text + ": waiting for download to start");
				}
			}
		}
	}

	// Token: 0x0400063A RID: 1594
	private static readonly Logger Logger = new Logger("ServerReadinessManager");

	// Token: 0x0400063C RID: 1596
	private static ServerConfig serverConfig;

	// Token: 0x0400063D RID: 1597
	private static string[] requiredModIds = new string[0];

	// Token: 0x0400063E RID: 1598
	private static bool modWaitTimedOut = false;

	// Token: 0x0400063F RID: 1599
	private static Tween progressPollTween;

	// Token: 0x04000640 RID: 1600
	private static Tween timeoutTween;
}
