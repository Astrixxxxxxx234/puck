using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public static class ApplicationManagerController
{
	// Token: 0x06000575 RID: 1397 RVA: 0x0001E2A8 File Offset: 0x0001C4A8
	public static Task Initialize()
	{
		ApplicationManagerController.<Initialize>d__0 <Initialize>d__;
		<Initialize>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<Initialize>d__.<>1__state = -1;
		<Initialize>d__.<>t__builder.Start<ApplicationManagerController.<Initialize>d__0>(ref <Initialize>d__);
		return <Initialize>d__.<>t__builder.Task;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0001E2E4 File Offset: 0x0001C4E4
	public static void Dispose()
	{
		EventManager.RemoveEventListener("Event_OnFullScreenModeChanged", new Action<Dictionary<string, object>>(ApplicationManagerController.Event_OnFullScreenModeChanged));
		EventManager.RemoveEventListener("Event_OnDisplayIndexChanged", new Action<Dictionary<string, object>>(ApplicationManagerController.Event_OnDisplayIndexChanged));
		EventManager.RemoveEventListener("Event_OnResolutionIndexChanged", new Action<Dictionary<string, object>>(ApplicationManagerController.Event_OnResolutionIndexChanged));
		EventManager.RemoveEventListener("Event_OnVSyncChanged", new Action<Dictionary<string, object>>(ApplicationManagerController.Event_OnVSyncChanged));
		EventManager.RemoveEventListener("Event_OnFpsLimitChanged", new Action<Dictionary<string, object>>(ApplicationManagerController.Event_OnFpsLimitChanged));
		EventManager.RemoveEventListener("Event_OnQualityChanged", new Action<Dictionary<string, object>>(ApplicationManagerController.Event_OnQualityChanged));
		EventManager.RemoveEventListener("Event_OnShadowQualityChanged", new Action<Dictionary<string, object>>(ApplicationManagerController.Event_OnShadowQualityChanged));
		EventManager.RemoveEventListener("Event_OnUIStateChanged", new Action<Dictionary<string, object>>(ApplicationManagerController.Event_OnUIStateChanged));
		EventManager.RemoveEventListener("Event_OnSocialClickDiscord", new Action<Dictionary<string, object>>(ApplicationManagerController.Event_OnSocialClickDiscord));
		EventManager.RemoveEventListener("Event_OnSocialClickPatreon", new Action<Dictionary<string, object>>(ApplicationManagerController.Event_OnSocialClickPatreon));
		EventManager.RemoveEventListener("Event_OnPopupClickOk", new Action<Dictionary<string, object>>(ApplicationManagerController.Event_OnPopupClickOk));
		EventManager.RemoveEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(ApplicationManagerController.Event_Server_OnServerStarted));
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0001E3FC File Offset: 0x0001C5FC
	private static void Event_OnFullScreenModeChanged(Dictionary<string, object> message)
	{
		FullScreenMode mode = (FullScreenMode)message["value"];
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		ApplicationManager.SetResolution(SettingsManager.ResolutionIndex, mode);
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x0001E430 File Offset: 0x0001C630
	private static void Event_OnDisplayIndexChanged(Dictionary<string, object> message)
	{
		ApplicationManagerController.<Event_OnDisplayIndexChanged>d__3 <Event_OnDisplayIndexChanged>d__;
		<Event_OnDisplayIndexChanged>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Event_OnDisplayIndexChanged>d__.message = message;
		<Event_OnDisplayIndexChanged>d__.<>1__state = -1;
		<Event_OnDisplayIndexChanged>d__.<>t__builder.Start<ApplicationManagerController.<Event_OnDisplayIndexChanged>d__3>(ref <Event_OnDisplayIndexChanged>d__);
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x0001E468 File Offset: 0x0001C668
	private static void Event_OnResolutionIndexChanged(Dictionary<string, object> message)
	{
		int index = (int)message["value"];
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		ApplicationManager.SetResolution(index, SettingsManager.FullScreenMode);
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0001E49C File Offset: 0x0001C69C
	private static void Event_OnVSyncChanged(Dictionary<string, object> message)
	{
		bool vsync = (bool)message["value"];
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		ApplicationManager.SetVSync(vsync);
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x0001E4C8 File Offset: 0x0001C6C8
	private static void Event_OnFpsLimitChanged(Dictionary<string, object> message)
	{
		int targetFrameRate = (int)message["value"];
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		ApplicationManager.SetTargetFrameRate(targetFrameRate);
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0001E4F4 File Offset: 0x0001C6F4
	private static void Event_OnQualityChanged(Dictionary<string, object> message)
	{
		ApplicationQuality quality = (ApplicationQuality)message["value"];
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		ApplicationManager.SetQuality(quality);
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x0001E520 File Offset: 0x0001C720
	private static void Event_OnShadowQualityChanged(Dictionary<string, object> message)
	{
		global::ShadowQuality shadowQuality = (global::ShadowQuality)message["value"];
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		ApplicationManager.SetShadowQuality(shadowQuality);
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x0001E54C File Offset: 0x0001C74C
	private static void Event_OnUIStateChanged(Dictionary<string, object> message)
	{
		UIState uistate = (UIState)message["oldUIState"];
		UIState uistate2 = (UIState)message["newUIState"];
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		if (uistate.IsMouseRequired == uistate2.IsMouseRequired)
		{
			return;
		}
		ApplicationManager.SetMouseVisibility(uistate2.IsMouseRequired);
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0001E59D File Offset: 0x0001C79D
	private static void Event_OnSocialClickDiscord(Dictionary<string, object> message)
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		Application.OpenURL("https://discord.gg/AZDBj6XsGg");
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0001E5B1 File Offset: 0x0001C7B1
	private static void Event_OnSocialClickPatreon(Dictionary<string, object> message)
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		Application.OpenURL("https://www.patreon.com/c/PuckGame");
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x0001E5C8 File Offset: 0x0001C7C8
	private static void Event_OnPopupClickOk(Dictionary<string, object> message)
	{
		string name = ((Popup)message["popup"]).Name;
		if (name == "mainMenuExitGame")
		{
			Application.Quit();
			return;
		}
		if (!(name == "pauseMenuExitGame"))
		{
			return;
		}
		Application.Quit();
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x0001E614 File Offset: 0x0001C814
	private static void Event_Server_OnServerStarted(Dictionary<string, object> message)
	{
		ServerConfig serverConfig = (ServerConfig)message["serverConfig"];
		if (ApplicationManager.IsDedicatedGameServer)
		{
			ApplicationManager.SetTargetFrameRate(serverConfig.tickRate);
		}
	}
}
