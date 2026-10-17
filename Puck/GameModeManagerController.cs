using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class GameModeManagerController : MonoBehaviour
{
	// Token: 0x06000691 RID: 1681 RVA: 0x0002428C File Offset: 0x0002248C
	private void Awake()
	{
		this.gameModeManager = base.GetComponent<GameModeManager>();
		EventManager.AddEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStarted));
		EventManager.AddEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
		EventManager.AddEventListener("Event_Server_OnLoadSceneEventCompleted", new Action<Dictionary<string, object>>(this.Event_Server_OnLoadSceneEventCompleted));
		EventManager.AddEventListener("Event_Everyone_OnLevelSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnLevelSpawned));
		EventManager.AddEventListener("Event_Everyone_OnLevelDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnLevelDespawned));
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x00024314 File Offset: 0x00022514
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStarted));
		EventManager.RemoveEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
		EventManager.RemoveEventListener("Event_Server_OnLoadSceneEventCompleted", new Action<Dictionary<string, object>>(this.Event_Server_OnLoadSceneEventCompleted));
		EventManager.RemoveEventListener("Event_Everyone_OnLevelSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnLevelSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnLevelDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnLevelDespawned));
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x00024390 File Offset: 0x00022590
	private void Event_Server_OnServerStarted(Dictionary<string, object> message)
	{
		ServerConfig serverConfig = (ServerConfig)message["serverConfig"];
		this.gameModeManager.SelectGameMode(serverConfig.gameMode);
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x000243BF File Offset: 0x000225BF
	private void Event_Server_OnServerStopped(Dictionary<string, object> message)
	{
		this.gameModeManager.DisableSelectedGameMode();
		this.gameModeManager.DeselectGameMode();
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x000243D7 File Offset: 0x000225D7
	private void Event_Server_OnLoadSceneEventCompleted(Dictionary<string, object> message)
	{
		this.gameModeManager.EnableSelectedGameMode();
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x000243E4 File Offset: 0x000225E4
	private void Event_Everyone_OnLevelSpawned(Dictionary<string, object> message)
	{
		Level level = (Level)message["level"];
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.gameModeManager.Level = level;
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x0002441B File Offset: 0x0002261B
	private void Event_Everyone_OnLevelDespawned(Dictionary<string, object> message)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.gameModeManager.Level = null;
	}

	// Token: 0x0400043A RID: 1082
	private GameModeManager gameModeManager;
}
