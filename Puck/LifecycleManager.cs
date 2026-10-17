using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

// Token: 0x020000EA RID: 234
public static class LifecycleManager
{
	// Token: 0x06000737 RID: 1847 RVA: 0x00026D0B File Offset: 0x00024F0B
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void SubsystemRegistration()
	{
		LogManager.Initialize();
		Application.quitting += LifecycleManager.Dispose;
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x00026D23 File Offset: 0x00024F23
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void BeforeSceneLoad()
	{
		PatchManager.Initialize();
		EventManager.Initialize();
		GlobalStateManager.Initialize();
		SaveManager.Initialize();
		InputManager.Initialize();
		SettingsManager.Initialize();
		ApplicationManager.Initialize();
		BackendManager.Initialize();
		ItemManager.Initialize();
		CameraManager.Initialize();
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x00026D57 File Offset: 0x00024F57
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void AfterSceneLoad()
	{
		SceneManager.Initialize();
		WebSocketManager.Initialize();
		SteamManager.Initialize();
		ModManager.Initialize();
		ServerReadinessManager.Initialize();
		LifecycleManager.RegisterUpdate();
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x00026D77 File Offset: 0x00024F77
	private static void Update()
	{
		ApplicationManager.Update(Time.unscaledDeltaTime);
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x00026D84 File Offset: 0x00024F84
	public static void Dispose()
	{
		LifecycleManager.UnregisterUpdate();
		ServerReadinessManager.Dispose();
		ModManager.Dispose();
		SteamManager.Dispose();
		WebSocketManager.Dispose();
		SceneManager.Dispose();
		CameraManager.Dispose();
		ItemManager.Dispose();
		BackendManager.Dispose();
		ApplicationManager.Dispose();
		SettingsManager.Dispose();
		InputManager.Dispose();
		SaveManager.Dispose();
		GlobalStateManager.Dispose();
		EventManager.Dispose();
		PatchManager.Dispose();
		LogManager.Dispose();
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x00026DE8 File Offset: 0x00024FE8
	private static void RegisterUpdate()
	{
		PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
		int num;
		if (!LifecycleManager.TryGetUpdateIndex(currentPlayerLoop, out num))
		{
			return;
		}
		List<PlayerLoopSystem> list = new List<PlayerLoopSystem>(currentPlayerLoop.subSystemList[num].subSystemList)
		{
			new PlayerLoopSystem
			{
				type = typeof(LifecycleManager),
				updateDelegate = new PlayerLoopSystem.UpdateFunction(LifecycleManager.Update)
			}
		};
		currentPlayerLoop.subSystemList[num].subSystemList = list.ToArray();
		PlayerLoop.SetPlayerLoop(currentPlayerLoop);
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x00026E70 File Offset: 0x00025070
	private static void UnregisterUpdate()
	{
		PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
		int num;
		if (!LifecycleManager.TryGetUpdateIndex(currentPlayerLoop, out num))
		{
			return;
		}
		currentPlayerLoop.subSystemList[num].subSystemList = Array.FindAll<PlayerLoopSystem>(currentPlayerLoop.subSystemList[num].subSystemList, (PlayerLoopSystem subSystem) => subSystem.type != typeof(LifecycleManager));
		PlayerLoop.SetPlayerLoop(currentPlayerLoop);
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x00026EDA File Offset: 0x000250DA
	private static bool TryGetUpdateIndex(PlayerLoopSystem playerLoop, out int index)
	{
		index = Array.FindIndex<PlayerLoopSystem>(playerLoop.subSystemList, (PlayerLoopSystem subSystem) => subSystem.type == typeof(Update));
		return index >= 0;
	}
}
