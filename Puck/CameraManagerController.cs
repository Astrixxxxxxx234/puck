using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// Token: 0x020000BC RID: 188
public static class CameraManagerController
{
	// Token: 0x06000602 RID: 1538 RVA: 0x00020FDC File Offset: 0x0001F1DC
	public static void Initialize()
	{
		EventManager.AddEventListener("Event_OnBaseCameraStarted", new Action<Dictionary<string, object>>(CameraManagerController.Event_OnBaseCameraStarted));
		EventManager.AddEventListener("Event_OnBaseCameraDestroyed", new Action<Dictionary<string, object>>(CameraManagerController.Event_OnBaseCameraDestroyed));
		EventManager.AddEventListener("Event_OnSceneLoaded", new Action<Dictionary<string, object>>(CameraManagerController.Event_OnSceneLoaded));
		EventManager.AddEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(CameraManagerController.Event_Everyone_OnPlayerSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(CameraManagerController.Event_Everyone_OnPlayerGameStateChanged));
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x00021058 File Offset: 0x0001F258
	public static void Dispose()
	{
		EventManager.RemoveEventListener("Event_OnBaseCameraStarted", new Action<Dictionary<string, object>>(CameraManagerController.Event_OnBaseCameraStarted));
		EventManager.RemoveEventListener("Event_OnBaseCameraDestroyed", new Action<Dictionary<string, object>>(CameraManagerController.Event_OnBaseCameraDestroyed));
		EventManager.RemoveEventListener("Event_OnSceneLoaded", new Action<Dictionary<string, object>>(CameraManagerController.Event_OnSceneLoaded));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(CameraManagerController.Event_Everyone_OnPlayerSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(CameraManagerController.Event_Everyone_OnPlayerGameStateChanged));
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x000210D4 File Offset: 0x0001F2D4
	private static void HandlePlayerGameState(Player player)
	{
		PlayerGameState value = player.GameState.Value;
		switch (value.Phase)
		{
		case PlayerPhase.TeamSelect:
			CameraManager.SetActiveCamera(CameraType.Cinematic, null);
			return;
		case PlayerPhase.PositionSelect:
			if (value.Team == PlayerTeam.Blue)
			{
				CameraManager.SetActiveCamera(CameraType.BluePositionSelection, null);
				return;
			}
			if (value.Team == PlayerTeam.Red)
			{
				CameraManager.SetActiveCamera(CameraType.RedPositionSelection, null);
				return;
			}
			CameraManager.SetActiveCamera(CameraType.Cinematic, null);
			return;
		case PlayerPhase.Play:
			CameraManager.SetActiveCamera(CameraType.Player, new ulong?(player.OwnerClientId));
			return;
		case PlayerPhase.Replay:
			CameraManager.SetActiveCamera(CameraType.Replay, null);
			return;
		case PlayerPhase.Spectate:
			CameraManager.SetActiveCamera(CameraType.Spectator, new ulong?(player.OwnerClientId));
			return;
		default:
			CameraManager.SetActiveCamera(CameraType.Cinematic, null);
			return;
		}
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x000211AA File Offset: 0x0001F3AA
	private static void Event_OnBaseCameraStarted(Dictionary<string, object> eventParams)
	{
		CameraManager.RegisterCamera((BaseCamera)eventParams["baseCamera"]);
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x000211C1 File Offset: 0x0001F3C1
	private static void Event_OnBaseCameraDestroyed(Dictionary<string, object> eventParams)
	{
		CameraManager.UnregisterCamera((BaseCamera)eventParams["baseCamera"]);
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x000211D8 File Offset: 0x0001F3D8
	private static void Event_OnSceneLoaded(Dictionary<string, object> eventParams)
	{
		if (((Scene)eventParams["scene"]).name == "locker_room")
		{
			CameraManager.SetActiveCamera(CameraType.LockerRoom, null);
		}
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x00021218 File Offset: 0x0001F418
	private static void Event_Everyone_OnPlayerSpawned(Dictionary<string, object> eventParams)
	{
		Player player = (Player)eventParams["player"];
		if (!player.IsLocalPlayer)
		{
			return;
		}
		CameraManagerController.HandlePlayerGameState(player);
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x00021248 File Offset: 0x0001F448
	private static void Event_Everyone_OnPlayerGameStateChanged(Dictionary<string, object> eventParams)
	{
		Player player = (Player)eventParams["player"];
		PlayerGameState playerGameState = (PlayerGameState)eventParams["oldGameState"];
		PlayerGameState playerGameState2 = (PlayerGameState)eventParams["newGameState"];
		if (!player.IsLocalPlayer)
		{
			return;
		}
		if (playerGameState.Phase == playerGameState2.Phase && playerGameState.Team == playerGameState2.Team)
		{
			return;
		}
		CameraManagerController.HandlePlayerGameState(player);
	}
}
