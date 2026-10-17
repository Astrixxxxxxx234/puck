using System;
using System.Collections.Generic;

// Token: 0x020001CC RID: 460
public class UIDebugController : UIViewController<UIDebug>
{
	// Token: 0x06000DEA RID: 3562 RVA: 0x00042374 File Offset: 0x00040574
	public override void Awake()
	{
		base.Awake();
		this.uiDebug = base.GetComponent<UIDebug>();
		EventManager.AddEventListener("Event_OnDebugChanged", new Action<Dictionary<string, object>>(this.Event_OnDebugChanged));
		EventManager.AddEventListener("Event_OnKeyBindsLoaded", new Action<Dictionary<string, object>>(this.Event_OnKeyBindsChanged));
		EventManager.AddEventListener("Event_OnKeyBindsSaved", new Action<Dictionary<string, object>>(this.Event_OnKeyBindsChanged));
		EventManager.AddEventListener("Event_Client_OnObjectSynchronizationDiagnostics", new Action<Dictionary<string, object>>(this.Event_Client_OnObjectSynchronizationDiagnostics));
		EventManager.AddEventListener("Event_OnApplicationDiagnostics", new Action<Dictionary<string, object>>(this.Event_OnApplicationDiagnostics));
		EventManager.AddEventListener("Event_Server_OnNetworkDiagnostics", new Action<Dictionary<string, object>>(this.Event_Server_OnNetworkDiagnostics));
		EventManager.AddEventListener("Event_Client_OnNetworkDiagnostics", new Action<Dictionary<string, object>>(this.Event_Client_OnNetworkDiagnostics));
		EventManager.AddEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
		EventManager.AddEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerDespawned));
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x00042470 File Offset: 0x00040670
	private void Start()
	{
		this.uiDebug.SetBuild(string.Format("PUCK B{0} {1:yyyy-MM-dd HH:mm:ss}", ApplicationManager.Version, DateTime.UtcNow));
		this.RefreshHint();
		this.uiDebug.SetMode(SettingsManager.Debug);
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x000424BC File Offset: 0x000406BC
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnDebugChanged", new Action<Dictionary<string, object>>(this.Event_OnDebugChanged));
		EventManager.RemoveEventListener("Event_OnKeyBindsLoaded", new Action<Dictionary<string, object>>(this.Event_OnKeyBindsChanged));
		EventManager.RemoveEventListener("Event_OnKeyBindsSaved", new Action<Dictionary<string, object>>(this.Event_OnKeyBindsChanged));
		EventManager.RemoveEventListener("Event_Client_OnObjectSynchronizationDiagnostics", new Action<Dictionary<string, object>>(this.Event_Client_OnObjectSynchronizationDiagnostics));
		EventManager.RemoveEventListener("Event_OnApplicationDiagnostics", new Action<Dictionary<string, object>>(this.Event_OnApplicationDiagnostics));
		EventManager.RemoveEventListener("Event_Server_OnNetworkDiagnostics", new Action<Dictionary<string, object>>(this.Event_Server_OnNetworkDiagnostics));
		EventManager.RemoveEventListener("Event_Client_OnNetworkDiagnostics", new Action<Dictionary<string, object>>(this.Event_Client_OnNetworkDiagnostics));
		EventManager.RemoveEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerDespawned));
		base.OnDestroy();
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x000425AB File Offset: 0x000407AB
	private void Event_OnKeyBindsChanged(Dictionary<string, object> message)
	{
		this.RefreshHint();
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x000425B4 File Offset: 0x000407B4
	private void RefreshHint()
	{
		KeyBind keyBind;
		string str = InputManager.KeyBinds.TryGetValue(InputManager.Debug1Action.name, out keyBind) ? InputManager.GetKeyBindDisplayString(keyBind) : "F5";
		this.uiDebug.SetHint("[" + str + "]");
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x00042604 File Offset: 0x00040804
	private void Event_OnDebugChanged(Dictionary<string, object> message)
	{
		DebugMode debugMode = (DebugMode)message["value"];
		this.uiDebug.SetMode(debugMode);
		if (debugMode != DebugMode.Off)
		{
			this.uiDebug.Show();
			return;
		}
		this.uiDebug.Hide();
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x0004264C File Offset: 0x0004084C
	private void Event_Everyone_OnPlayerSpawned(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (!player.IsLocalPlayer)
		{
			return;
		}
		this.localPlayer = player;
	}

	// Token: 0x06000DF1 RID: 3569 RVA: 0x0004267A File Offset: 0x0004087A
	private void Event_Everyone_OnPlayerDespawned(Dictionary<string, object> message)
	{
		if ((Player)message["player"] != this.localPlayer)
		{
			return;
		}
		this.localPlayer = null;
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x000426A1 File Offset: 0x000408A1
	private void Event_OnClientStopped(Dictionary<string, object> message)
	{
		this.uiDebug.SetNetworkVisible(false);
		this.uiDebug.SetObjectSynchronizationVisible(false);
		this.uiDebug.SetSimpleRoundTripTime(string.Empty);
		this.uiDebug.SetSimpleLoss(string.Empty);
	}

	// Token: 0x06000DF3 RID: 3571 RVA: 0x000426DC File Offset: 0x000408DC
	private void Event_OnApplicationDiagnostics(Dictionary<string, object> message)
	{
		ApplicationDiagnostics applicationDiagnostics = (ApplicationDiagnostics)message["applicationDiagnostics"];
		this.uiDebug.SetApplicationVisible(true);
		string text = string.Format("{0:0.0}/s", applicationDiagnostics.FramesPerSecond);
		string milliseconds = UIDebugController.GetMilliseconds(applicationDiagnostics.P95FrameTime);
		this.uiDebug.SetApplicationFrames(text);
		this.uiDebug.SetApplicationFrameTime(UIDebugController.GetMilliseconds(applicationDiagnostics.AverageFrameTime), milliseconds, UIDebugController.GetMilliseconds(applicationDiagnostics.MaxFrameTime));
		this.uiDebug.SetSimpleApplication(text, milliseconds);
	}

	// Token: 0x06000DF4 RID: 3572 RVA: 0x00042764 File Offset: 0x00040964
	private void Event_Server_OnNetworkDiagnostics(Dictionary<string, object> message)
	{
		this.RefreshNetworkDiagnostics(message);
	}

	// Token: 0x06000DF5 RID: 3573 RVA: 0x00042764 File Offset: 0x00040964
	private void Event_Client_OnNetworkDiagnostics(Dictionary<string, object> message)
	{
		this.RefreshNetworkDiagnostics(message);
	}

	// Token: 0x06000DF6 RID: 3574 RVA: 0x00042770 File Offset: 0x00040970
	private void RefreshNetworkDiagnostics(Dictionary<string, object> message)
	{
		NetworkDiagnostics networkDiagnostics = (NetworkDiagnostics)message["networkDiagnostics"];
		this.uiDebug.SetNetworkVisible(true);
		this.uiDebug.SetNetworkSent(UIDebugController.GetKilobytesPerSecond(networkDiagnostics.SentKilobytesPerSecond), UIDebugController.GetPacketsPerSecond(networkDiagnostics.SentPacketsPerSecond));
		this.uiDebug.SetNetworkReceived(UIDebugController.GetKilobytesPerSecond(networkDiagnostics.ReceivedKilobytesPerSecond), UIDebugController.GetPacketsPerSecond(networkDiagnostics.ReceivedPacketsPerSecond));
		string roundTripTimeText = this.GetRoundTripTimeText();
		this.uiDebug.SetNetworkRoundTripTime(roundTripTimeText);
		this.uiDebug.SetNetworkStall(UIDebugController.GetMilliseconds(networkDiagnostics.MaxStallTime));
		this.uiDebug.SetSimpleRoundTripTime(roundTripTimeText);
	}

	// Token: 0x06000DF7 RID: 3575 RVA: 0x00042815 File Offset: 0x00040A15
	private static string GetKilobytesPerSecond(float kilobytesPerSecond)
	{
		return string.Format("{0:0.0}KB/s", kilobytesPerSecond);
	}

	// Token: 0x06000DF8 RID: 3576 RVA: 0x00042827 File Offset: 0x00040A27
	private static string GetPacketsPerSecond(float packetsPerSecond)
	{
		return string.Format("{0:0}/s", packetsPerSecond);
	}

	// Token: 0x06000DF9 RID: 3577 RVA: 0x0004283C File Offset: 0x00040A3C
	private void Event_Client_OnObjectSynchronizationDiagnostics(Dictionary<string, object> message)
	{
		SynchronizedObjectClientDiagnosticsData synchronizedObjectClientDiagnosticsData = (SynchronizedObjectClientDiagnosticsData)message["diagnosticsData"];
		this.uiDebug.SetObjectSynchronizationVisible(true);
		this.uiDebug.SetObjectSynchronizationTicks(string.Format("{0:0.0}/s", UIDebugController.GetTicksPerSecond(synchronizedObjectClientDiagnosticsData)));
		this.uiDebug.SetObjectSynchronizationTickDelta(UIDebugController.GetMilliseconds(synchronizedObjectClientDiagnosticsData.AverageTickDeltaTime), UIDebugController.GetMilliseconds(synchronizedObjectClientDiagnosticsData.P95TickDeltaTime), UIDebugController.GetMilliseconds(synchronizedObjectClientDiagnosticsData.MaxTickDeltaTime));
		string lossText = UIDebugController.GetLossText(synchronizedObjectClientDiagnosticsData);
		this.uiDebug.SetObjectSynchronizationLoss(lossText);
		this.uiDebug.SetSimpleLoss(lossText);
		this.uiDebug.SetObjectSynchronizationReordered(UIDebugController.GetReorderedText(synchronizedObjectClientDiagnosticsData));
		this.uiDebug.SetObjectSynchronizationTimeline(UIDebugController.GetTimelineText(synchronizedObjectClientDiagnosticsData));
		this.uiDebug.SetObjectSynchronizationSpeed(string.Format("{0:0.000}", synchronizedObjectClientDiagnosticsData.AverageTimelineTimescale));
		this.uiDebug.SetObjectSynchronizationSnaps(UIDebugController.GetSnapsText(synchronizedObjectClientDiagnosticsData));
		this.uiDebug.SetObjectSynchronizationExtrapolation(string.Format("{0:0.0}%", synchronizedObjectClientDiagnosticsData.ExtrapolationPercentage));
	}

	// Token: 0x06000DFA RID: 3578 RVA: 0x00042944 File Offset: 0x00040B44
	private static string GetLossText(SynchronizedObjectClientDiagnosticsData diagnosticsData)
	{
		return UIDebugController.GetThresholdText(string.Format("{0:0.00}%", diagnosticsData.TickLossPercentage), (double)diagnosticsData.TickLossPercentage, 0.01, 0.02);
	}

	// Token: 0x06000DFB RID: 3579 RVA: 0x00042979 File Offset: 0x00040B79
	private static string GetReorderedText(SynchronizedObjectClientDiagnosticsData diagnosticsData)
	{
		return UIDebugController.GetThresholdText(string.Format("{0}", diagnosticsData.OutOfOrderTickCount), (double)diagnosticsData.OutOfOrderTickCount, 1.0, 2.0);
	}

	// Token: 0x06000DFC RID: 3580 RVA: 0x000429AE File Offset: 0x00040BAE
	private string GetRoundTripTimeText()
	{
		if (!this.localPlayer)
		{
			return string.Empty;
		}
		return string.Format("{0}ms", this.localPlayer.Ping.Value);
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x000429E2 File Offset: 0x00040BE2
	private static string GetSnapsText(SynchronizedObjectClientDiagnosticsData diagnosticsData)
	{
		return UIDebugController.GetThresholdText(string.Format("{0}", diagnosticsData.SnapCount), (double)diagnosticsData.SnapCount, 1.0, 2.0);
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x00042A17 File Offset: 0x00040C17
	private static string GetTimelineMilliseconds(SynchronizedObjectClientDiagnosticsData diagnosticsData, double timelinePosition)
	{
		return UIDebugController.GetMilliseconds(timelinePosition * (double)diagnosticsData.TickInterval);
	}

	// Token: 0x06000DFF RID: 3583 RVA: 0x00042A28 File Offset: 0x00040C28
	private static string GetTimelineText(SynchronizedObjectClientDiagnosticsData diagnosticsData)
	{
		double value = Math.Abs(diagnosticsData.AverageTimelinePosition - diagnosticsData.TargetTimelinePosition);
		string thresholdText = UIDebugController.GetThresholdText(string.Format("{0:0.00} ({1})", diagnosticsData.AverageTimelinePosition, UIDebugController.GetTimelineMilliseconds(diagnosticsData, diagnosticsData.AverageTimelinePosition)), value, 0.5, 1.0);
		string str = StringUtils.WrapInColor(string.Format("/ {0:0.00} ({1})", diagnosticsData.TargetTimelinePosition, UIDebugController.GetTimelineMilliseconds(diagnosticsData, diagnosticsData.TargetTimelinePosition)), "#b8b8b8");
		return thresholdText + " " + str;
	}

	// Token: 0x06000E00 RID: 3584 RVA: 0x00042AB8 File Offset: 0x00040CB8
	private static string GetThresholdText(string text, double value, double warningThreshold, double errorThreshold)
	{
		if (value >= errorThreshold)
		{
			return StringUtils.WrapInColor(text, "#e74c3c");
		}
		if (value >= warningThreshold)
		{
			return StringUtils.WrapInColor(text, "#ffe97f");
		}
		return text;
	}

	// Token: 0x06000E01 RID: 3585 RVA: 0x00042ADB File Offset: 0x00040CDB
	private static float GetTicksPerSecond(SynchronizedObjectClientDiagnosticsData diagnosticsData)
	{
		if (diagnosticsData.WindowTime <= 0f)
		{
			return 0f;
		}
		return (float)diagnosticsData.TickCount / diagnosticsData.WindowTime;
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x00042AFE File Offset: 0x00040CFE
	private static string GetMilliseconds(double seconds)
	{
		return string.Format("{0:0.0}ms", seconds * 1000.0);
	}

	// Token: 0x040008E7 RID: 2279
	private const double LOSS_WARNING_PERCENTAGE = 0.01;

	// Token: 0x040008E8 RID: 2280
	private const double LOSS_ERROR_PERCENTAGE = 0.02;

	// Token: 0x040008E9 RID: 2281
	private const double REORDERED_WARNING_COUNT = 1.0;

	// Token: 0x040008EA RID: 2282
	private const double REORDERED_ERROR_COUNT = 2.0;

	// Token: 0x040008EB RID: 2283
	private const double TIMELINE_WARNING_DRIFT_TICKS = 0.5;

	// Token: 0x040008EC RID: 2284
	private const double TIMELINE_ERROR_DRIFT_TICKS = 1.0;

	// Token: 0x040008ED RID: 2285
	private const double SNAP_WARNING_COUNT = 1.0;

	// Token: 0x040008EE RID: 2286
	private const double SNAP_ERROR_COUNT = 2.0;

	// Token: 0x040008EF RID: 2287
	private UIDebug uiDebug;

	// Token: 0x040008F0 RID: 2288
	private Player localPlayer;
}
