using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.SceneManagement;

// Token: 0x02000143 RID: 323
public static class SceneManager
{
	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06000949 RID: 2377 RVA: 0x0002DA14 File Offset: 0x0002BC14
	public static bool IsNetworkSceneManagerAvailable
	{
		get
		{
			return NetworkManager.Singleton != null && NetworkManager.Singleton.SceneManager != null && NetworkManager.Singleton.IsServer;
		}
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x0002DA3B File Offset: 0x0002BC3B
	public static void Initialize()
	{
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += global::SceneManager.OnSceneLoaded;
		UnityEngine.SceneManagement.SceneManager.sceneUnloaded += global::SceneManager.OnSceneUnloaded;
		SceneManagerController.Initialize();
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x0002DA64 File Offset: 0x0002BC64
	public static void Dispose()
	{
		UnityEngine.SceneManagement.SceneManager.sceneLoaded -= global::SceneManager.OnSceneLoaded;
		UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= global::SceneManager.OnSceneUnloaded;
		SceneManagerController.Dispose();
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x0002DA8D File Offset: 0x0002BC8D
	public static void InitializeServer()
	{
		if (!global::SceneManager.IsNetworkSceneManagerAvailable)
		{
			return;
		}
		global::SceneManager.IsSceneLoadInProgress = false;
		global::SceneManager.IsInitialSceneLoaded = false;
		NetworkManager.Singleton.SceneManager.OnSceneEvent += global::SceneManager.Server_OnSceneEvent;
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0002DABE File Offset: 0x0002BCBE
	public static void DisposeServer()
	{
		if (!global::SceneManager.IsNetworkSceneManagerAvailable)
		{
			return;
		}
		NetworkManager.Singleton.SceneManager.OnSceneEvent -= global::SceneManager.Server_OnSceneEvent;
		global::SceneManager.IsSceneLoadInProgress = false;
		global::SceneManager.IsInitialSceneLoaded = false;
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x0002DAF0 File Offset: 0x0002BCF0
	public static void LoadScene(string sceneName)
	{
		if (global::SceneManager.IsNetworkSceneManagerAvailable)
		{
			global::SceneManager.Logger.Info("Loading server scene " + sceneName);
			NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
			return;
		}
		global::SceneManager.Logger.Info("Loading scene " + sceneName);
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0002DB47 File Offset: 0x0002BD47
	private static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		EventManager.TriggerEvent("Event_OnSceneLoaded", new Dictionary<string, object>
		{
			{
				"scene",
				scene
			}
		});
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0002DB69 File Offset: 0x0002BD69
	private static void OnSceneUnloaded(Scene scene)
	{
		EventManager.TriggerEvent("Event_OnSceneUnloaded", new Dictionary<string, object>
		{
			{
				"scene",
				scene
			}
		});
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0002DB8C File Offset: 0x0002BD8C
	private static void Server_OnSceneEvent(SceneEvent sceneEvent)
	{
		switch (sceneEvent.SceneEventType)
		{
		case SceneEventType.Load:
			global::SceneManager.Server_OnLoadScene();
			return;
		case SceneEventType.Unload:
		case SceneEventType.Synchronize:
		case SceneEventType.ReSynchronize:
		case SceneEventType.UnloadEventCompleted:
			break;
		case SceneEventType.LoadEventCompleted:
			global::SceneManager.Server_OnLoadSceneEventCompleted(sceneEvent.ClientsThatCompleted, sceneEvent.ClientsThatTimedOut);
			break;
		case SceneEventType.LoadComplete:
			global::SceneManager.Server_OnClientSceneLoadComplete(sceneEvent.ClientId);
			return;
		case SceneEventType.UnloadComplete:
			global::SceneManager.Server_OnClientSceneUnloadComplete(sceneEvent.ClientId);
			return;
		case SceneEventType.SynchronizeComplete:
			global::SceneManager.Server_OnClientSceneSynchronizeComplete(sceneEvent.ClientId);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x0002DC06 File Offset: 0x0002BE06
	private static void Server_OnLoadScene()
	{
		global::SceneManager.IsSceneLoadInProgress = true;
		global::SceneManager.Logger.Info("Server started loading scene");
		EventManager.TriggerEvent("Event_Server_OnLoadScene", null);
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x0002DC28 File Offset: 0x0002BE28
	private static void Server_OnClientSceneLoadComplete(ulong clientId)
	{
		global::SceneManager.Logger.Info(string.Format("Client {0} completed scene load", clientId));
		EventManager.TriggerEvent("Event_Server_OnClientSceneLoadComplete", new Dictionary<string, object>
		{
			{
				"clientId",
				clientId
			}
		});
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x0002DC64 File Offset: 0x0002BE64
	private static void Server_OnClientSceneUnloadComplete(ulong clientId)
	{
		global::SceneManager.Logger.Info(string.Format("Client {0} completed scene unload", clientId));
		EventManager.TriggerEvent("Event_Server_OnClientSceneUnloadComplete", new Dictionary<string, object>
		{
			{
				"clientId",
				clientId
			}
		});
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0002DCA0 File Offset: 0x0002BEA0
	private static void Server_OnClientSceneSynchronizeComplete(ulong clientId)
	{
		global::SceneManager.Logger.Info(string.Format("Client {0} completed scene synchronization", clientId));
		EventManager.TriggerEvent("Event_Server_OnClientSceneSynchronizeComplete", new Dictionary<string, object>
		{
			{
				"clientId",
				clientId
			}
		});
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x0002DCDC File Offset: 0x0002BEDC
	private static void Server_OnLoadSceneEventCompleted(List<ulong> clientsThatCompleted, List<ulong> clientsThatTimedOut)
	{
		global::SceneManager.Logger.Info("Scene load event completed on server");
		global::SceneManager.IsSceneLoadInProgress = false;
		bool flag = !global::SceneManager.IsInitialSceneLoaded;
		global::SceneManager.IsInitialSceneLoaded = true;
		EventManager.TriggerEvent("Event_Server_OnLoadSceneEventCompleted", new Dictionary<string, object>
		{
			{
				"clientsThatCompleted",
				clientsThatCompleted
			},
			{
				"clientsThatTimedOut",
				clientsThatTimedOut
			},
			{
				"isInitialScene",
				flag
			}
		});
	}

	// Token: 0x040005D4 RID: 1492
	private static readonly Logger Logger = new Logger("SceneManager");

	// Token: 0x040005D5 RID: 1493
	public static bool IsSceneLoadInProgress;

	// Token: 0x040005D6 RID: 1494
	public static bool IsInitialSceneLoaded;
}
