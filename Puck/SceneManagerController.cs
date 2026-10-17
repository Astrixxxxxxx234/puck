using System;
using System.Collections.Generic;

// Token: 0x02000144 RID: 324
public static class SceneManagerController
{
	// Token: 0x06000958 RID: 2392 RVA: 0x0002DD58 File Offset: 0x0002BF58
	public static void Initialize()
	{
		EventManager.AddEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(SceneManagerController.Event_Server_OnServerStarted));
		EventManager.AddEventListener("Event_Server_OnReady", new Action<Dictionary<string, object>>(SceneManagerController.Event_Server_OnReady));
		EventManager.AddEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(SceneManagerController.Event_Server_OnServerStopped));
		EventManager.AddEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(SceneManagerController.Event_OnClientStopped));
		if (!ApplicationManager.IsDedicatedGameServer)
		{
			SceneManager.LoadScene("locker_room");
		}
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x0002DDD0 File Offset: 0x0002BFD0
	public static void Dispose()
	{
		EventManager.RemoveEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(SceneManagerController.Event_Server_OnServerStarted));
		EventManager.RemoveEventListener("Event_Server_OnReady", new Action<Dictionary<string, object>>(SceneManagerController.Event_Server_OnReady));
		EventManager.RemoveEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(SceneManagerController.Event_Server_OnServerStopped));
		EventManager.RemoveEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(SceneManagerController.Event_OnClientStopped));
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x0002DE35 File Offset: 0x0002C035
	private static void Event_Server_OnServerStarted(Dictionary<string, object> message)
	{
		SceneManager.InitializeServer();
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x0002DE3C File Offset: 0x0002C03C
	private static void Event_Server_OnReady(Dictionary<string, object> message)
	{
		if (((ServerConfig)message["serverConfig"]).level == "default")
		{
			SceneManager.LoadScene("level_default");
		}
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0002DE69 File Offset: 0x0002C069
	private static void Event_Server_OnServerStopped(Dictionary<string, object> message)
	{
		SceneManager.DisposeServer();
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0002DE70 File Offset: 0x0002C070
	private static void Event_OnClientStopped(Dictionary<string, object> message)
	{
		SceneManager.LoadScene("locker_room");
	}
}
