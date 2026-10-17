using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Token: 0x020000A1 RID: 161
public static class ApplicationManager
{
	// Token: 0x1700008F RID: 143
	// (get) Token: 0x0600055C RID: 1372 RVA: 0x0001DC60 File Offset: 0x0001BE60
	public static bool IsDedicatedGameServer
	{
		get
		{
			return Application.isBatchMode;
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x0600055D RID: 1373 RVA: 0x0001DC68 File Offset: 0x0001BE68
	public static ushort Version
	{
		get
		{
			ushort result;
			if (!ushort.TryParse(Application.version, out result))
			{
				return 0;
			}
			return result;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x0600055E RID: 1374 RVA: 0x0001DC86 File Offset: 0x0001BE86
	// (set) Token: 0x0600055F RID: 1375 RVA: 0x0001DC8D File Offset: 0x0001BE8D
	private static bool IsDisplayChangeInProgress
	{
		get
		{
			return ApplicationManager.isDisplayChangeInProgress;
		}
		set
		{
			if (ApplicationManager.isDisplayChangeInProgress == value)
			{
				return;
			}
			ApplicationManager.isDisplayChangeInProgress = value;
			ApplicationManager.OnIsDisplayChangeInProgressChanged();
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x06000560 RID: 1376 RVA: 0x0001DCA3 File Offset: 0x0001BEA3
	private static float DiagnosticsInterval
	{
		get
		{
			if (!ApplicationManager.IsDedicatedGameServer)
			{
				return 1f;
			}
			return 0f;
		}
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x0001DCB7 File Offset: 0x0001BEB7
	public static void Initialize()
	{
		ApplicationManagerController.Initialize();
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x0001DCBF File Offset: 0x0001BEBF
	public static void Dispose()
	{
		ApplicationManagerController.Dispose();
		Tween tween = ApplicationManager.mouseVisibilityDebounceTween;
		if (tween == null)
		{
			return;
		}
		tween.Kill(false);
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x0001DCD8 File Offset: 0x0001BED8
	public static void Update(float deltaTime)
	{
		float diagnosticsInterval = ApplicationManager.DiagnosticsInterval;
		if (diagnosticsInterval <= 0f)
		{
			return;
		}
		ApplicationManager.RecordFrame(deltaTime);
		ApplicationManager.diagnosticsTime += deltaTime;
		if (ApplicationManager.diagnosticsTime < diagnosticsInterval)
		{
			return;
		}
		ApplicationManager.TakeDiagnostics();
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0001DD14 File Offset: 0x0001BF14
	private static void RecordFrame(float frameTime)
	{
		if (frameTime <= 0f)
		{
			return;
		}
		ApplicationManager.frameTimeHistogram.Add(frameTime);
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x0001DD2A File Offset: 0x0001BF2A
	private static void TakeDiagnostics()
	{
		ApplicationManager.TriggerDiagnosticsEvent(ApplicationManager.GetApplicationDiagnostics());
		ApplicationManager.diagnosticsTime = 0f;
		ApplicationManager.frameTimeHistogram.Reset();
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x0001DD4C File Offset: 0x0001BF4C
	private static ApplicationDiagnostics GetApplicationDiagnostics()
	{
		return new ApplicationDiagnostics
		{
			WindowTime = ApplicationManager.diagnosticsTime,
			FrameCount = ApplicationManager.frameTimeHistogram.Count,
			AverageFrameTime = ApplicationManager.frameTimeHistogram.Average,
			P95FrameTime = ApplicationManager.frameTimeHistogram.GetPercentile(0.95),
			MaxFrameTime = ApplicationManager.frameTimeHistogram.Max
		};
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x0001DDBB File Offset: 0x0001BFBB
	private static void TriggerDiagnosticsEvent(ApplicationDiagnostics applicationDiagnostics)
	{
		if (!EventManager.HasEventListeners("Event_OnApplicationDiagnostics"))
		{
			return;
		}
		EventManager.TriggerEvent("Event_OnApplicationDiagnostics", new Dictionary<string, object>
		{
			{
				"applicationDiagnostics",
				applicationDiagnostics
			}
		});
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x0001DDEC File Offset: 0x0001BFEC
	public static Task SetDisplay(int index)
	{
		ApplicationManager.<SetDisplay>d__24 <SetDisplay>d__;
		<SetDisplay>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SetDisplay>d__.index = index;
		<SetDisplay>d__.<>1__state = -1;
		<SetDisplay>d__.<>t__builder.Start<ApplicationManager.<SetDisplay>d__24>(ref <SetDisplay>d__);
		return <SetDisplay>d__.<>t__builder.Task;
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x0001DE30 File Offset: 0x0001C030
	public static void SetResolution(int index, FullScreenMode mode)
	{
		ApplicationManager.Logger.Info(string.Format("Setting resolution to {0} with full screen mode {1}", index, mode));
		if (ApplicationManager.IsDisplayChangeInProgress)
		{
			return;
		}
		List<Resolution> resolutions = Utils.GetResolutions();
		Resolution resolution = (index < 0 || index >= resolutions.Count) ? Screen.currentResolution : resolutions[index];
		Screen.SetResolution(resolution.width, resolution.height, mode, resolution.refreshRateRatio);
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x0001DEAA File Offset: 0x0001C0AA
	public static void SetVSync(bool isEnabled)
	{
		ApplicationManager.Logger.Info(string.Format("Setting vSync to {0}", isEnabled));
		QualitySettings.vSyncCount = (isEnabled ? 1 : 0);
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x0001DED2 File Offset: 0x0001C0D2
	public static void SetTargetFrameRate(int targetFrameRate)
	{
		ApplicationManager.Logger.Info(string.Format("Setting target frame rate to {0}", targetFrameRate));
		Application.targetFrameRate = targetFrameRate;
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x0001DEF4 File Offset: 0x0001C0F4
	public static void SetQuality(ApplicationQuality quality)
	{
		ApplicationManager.Logger.Info(string.Format("Setting quality to {0}", quality));
		int vSyncCount = QualitySettings.vSyncCount;
		switch (quality)
		{
		case ApplicationQuality.Low:
			QualitySettings.SetQualityLevel(0, true);
			break;
		case ApplicationQuality.Medium:
			QualitySettings.SetQualityLevel(2, true);
			break;
		case ApplicationQuality.High:
			QualitySettings.SetQualityLevel(4, true);
			break;
		case ApplicationQuality.Ultra:
			QualitySettings.SetQualityLevel(5, true);
			break;
		default:
			QualitySettings.SetQualityLevel(4, true);
			break;
		}
		QualitySettings.vSyncCount = vSyncCount;
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x0001DF6C File Offset: 0x0001C16C
	public static void SetShadowQuality(global::ShadowQuality shadowQuality)
	{
		ApplicationManager.Logger.Info(string.Format("Setting shadow quality to {0}", shadowQuality));
		UniversalRenderPipelineAsset asset = UniversalRenderPipeline.asset;
		if (asset == null)
		{
			ApplicationManager.Logger.Error("Render pipeline asset is null");
			return;
		}
		ValueTuple<int, float> valueTuple;
		switch (shadowQuality)
		{
		case global::ShadowQuality.Low:
			valueTuple = new ValueTuple<int, float>(1024, 25f);
			break;
		case global::ShadowQuality.Medium:
			valueTuple = new ValueTuple<int, float>(2048, 40f);
			break;
		case global::ShadowQuality.High:
			valueTuple = new ValueTuple<int, float>(4096, 50f);
			break;
		case global::ShadowQuality.Ultra:
			valueTuple = new ValueTuple<int, float>(8192, 50f);
			break;
		default:
			valueTuple = new ValueTuple<int, float>(4096, 50f);
			break;
		}
		ValueTuple<int, float> valueTuple2 = valueTuple;
		int item = valueTuple2.Item1;
		float item2 = valueTuple2.Item2;
		asset.mainLightShadowmapResolution = item;
		asset.shadowDistance = item2;
		asset.shadowCascadeCount = 4;
		asset.cascade4Split = Constants.SHADOW_CASCADE_RANGES / item2;
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x0001E058 File Offset: 0x0001C258
	public static void SetMouseVisibility(bool isVisible)
	{
		Tween tween = ApplicationManager.mouseVisibilityDebounceTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		ApplicationManager.mouseVisibilityDebounceTween = DOVirtual.DelayedCall(0f, delegate
		{
			ApplicationManager.Logger.Info(string.Format("Setting mouse visibility to {0}", isVisible));
			if (isVisible)
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				return;
			}
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}, true);
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x0001E0A0 File Offset: 0x0001C2A0
	private static void OnIsDisplayChangeInProgressChanged()
	{
		ApplicationManager.Logger.Info(string.Format("Display change in progress: {0}", ApplicationManager.isDisplayChangeInProgress));
		EventManager.TriggerEvent("Event_OnIsDisplayChangeInProgressChanged", new Dictionary<string, object>
		{
			{
				"isDisplayChangeInProgress",
				ApplicationManager.isDisplayChangeInProgress
			}
		});
	}

	// Token: 0x04000385 RID: 901
	private static readonly global::Logger Logger = new global::Logger("ApplicationManager");

	// Token: 0x04000386 RID: 902
	private static bool isDisplayChangeInProgress = false;

	// Token: 0x04000387 RID: 903
	private static Tween mouseVisibilityDebounceTween;

	// Token: 0x04000388 RID: 904
	private const float SERVER_DIAGNOSTICS_INTERVAL = 0f;

	// Token: 0x04000389 RID: 905
	private const float CLIENT_DIAGNOSTICS_INTERVAL = 1f;

	// Token: 0x0400038A RID: 906
	private const int FRAME_TIME_BUCKET_COUNT = 201;

	// Token: 0x0400038B RID: 907
	private static TimeHistogram frameTimeHistogram = new TimeHistogram(0.0005f, 201);

	// Token: 0x0400038C RID: 908
	private static float diagnosticsTime = 0f;
}
