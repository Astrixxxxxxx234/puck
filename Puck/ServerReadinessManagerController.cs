using System;
using System.Collections.Generic;

// Token: 0x02000160 RID: 352
public static class ServerReadinessManagerController
{
	// Token: 0x06000A65 RID: 2661 RVA: 0x00031730 File Offset: 0x0002F930
	public static void Initialize()
	{
		EventManager.AddEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(ServerReadinessManagerController.Event_Server_OnServerStarted));
		EventManager.AddEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(ServerReadinessManagerController.Event_Server_OnServerStopped));
		EventManager.AddEventListener("Event_OnModStateChanged", new Action<Dictionary<string, object>>(ServerReadinessManagerController.Event_OnModStateChanged));
		EventManager.AddEventListener("Event_OnServerStateChanged", new Action<Dictionary<string, object>>(ServerReadinessManagerController.Event_OnServerStateChanged));
		EventManager.AddEventListener("Event_Server_OnLoadSceneEventCompleted", new Action<Dictionary<string, object>>(ServerReadinessManagerController.Event_Server_OnLoadSceneEventCompleted));
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x000317AC File Offset: 0x0002F9AC
	public static void Dispose()
	{
		EventManager.RemoveEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(ServerReadinessManagerController.Event_Server_OnServerStarted));
		EventManager.RemoveEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(ServerReadinessManagerController.Event_Server_OnServerStopped));
		EventManager.RemoveEventListener("Event_OnModStateChanged", new Action<Dictionary<string, object>>(ServerReadinessManagerController.Event_OnModStateChanged));
		EventManager.RemoveEventListener("Event_OnServerStateChanged", new Action<Dictionary<string, object>>(ServerReadinessManagerController.Event_OnServerStateChanged));
		EventManager.RemoveEventListener("Event_Server_OnLoadSceneEventCompleted", new Action<Dictionary<string, object>>(ServerReadinessManagerController.Event_Server_OnLoadSceneEventCompleted));
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x00031827 File Offset: 0x0002FA27
	private static void Event_Server_OnServerStarted(Dictionary<string, object> message)
	{
		ServerReadinessManager.BeginTracking((ServerConfig)message["serverConfig"]);
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x0003183E File Offset: 0x0002FA3E
	private static void Event_Server_OnServerStopped(Dictionary<string, object> message)
	{
		ServerReadinessManager.Reset();
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x00031845 File Offset: 0x0002FA45
	private static void Event_OnModStateChanged(Dictionary<string, object> message)
	{
		if (ServerReadinessManager.Phase != ServerReadinessPhase.Starting)
		{
			return;
		}
		ServerReadinessManager.Evaluate();
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x00031845 File Offset: 0x0002FA45
	private static void Event_OnServerStateChanged(Dictionary<string, object> message)
	{
		if (ServerReadinessManager.Phase != ServerReadinessPhase.Starting)
		{
			return;
		}
		ServerReadinessManager.Evaluate();
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x00031856 File Offset: 0x0002FA56
	private static void Event_Server_OnLoadSceneEventCompleted(Dictionary<string, object> message)
	{
		if ((bool)message["isInitialScene"])
		{
			ServerReadinessManager.AnnounceReady();
		}
	}
}
