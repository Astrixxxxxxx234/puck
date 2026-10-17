using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class ReplayRecorderController : MonoBehaviour
{
	// Token: 0x0600091E RID: 2334 RVA: 0x0002D20C File Offset: 0x0002B40C
	private void Awake()
	{
		this.replayRecorder = base.GetComponent<ReplayRecorder>();
		EventManager.AddEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerDespawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerBodySpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodySpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerBodyDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodyDespawned));
		EventManager.AddEventListener("Event_Everyone_OnStickSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnStickSpawned));
		EventManager.AddEventListener("Event_Everyone_OnStickDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnStickDespawned));
		EventManager.AddEventListener("Event_Everyone_OnPuckSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPuckDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckDespawned));
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x000023BA File Offset: 0x000005BA
	private void Start()
	{
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x0002D2D8 File Offset: 0x0002B4D8
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerDespawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerBodySpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodySpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerBodyDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodyDespawned));
		EventManager.RemoveEventListener("Event_Everyone_OnStickSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnStickSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnStickDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnStickDespawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPuckSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPuckDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckDespawned));
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x0002D398 File Offset: 0x0002B598
	private void Event_Everyone_OnPlayerSpawned(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (player.IsReplay.Value)
		{
			return;
		}
		this.replayRecorder.Server_AddPlayerSpawnedEvent(player);
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x0002D3E0 File Offset: 0x0002B5E0
	private void Event_Everyone_OnPlayerDespawned(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (player.IsReplay.Value)
		{
			return;
		}
		this.replayRecorder.Server_AddPlayerDespawnedEvent(player);
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x0002D428 File Offset: 0x0002B628
	private void Event_Everyone_OnPlayerBodySpawned(Dictionary<string, object> message)
	{
		PlayerBody playerBody = (PlayerBody)message["playerBody"];
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (playerBody.Player.IsReplay.Value)
		{
			return;
		}
		this.replayRecorder.Server_AddPlayerBodySpawnedEvent(playerBody);
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x0002D474 File Offset: 0x0002B674
	private void Event_Everyone_OnPlayerBodyDespawned(Dictionary<string, object> message)
	{
		PlayerBody playerBody = (PlayerBody)message["playerBody"];
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (playerBody.Player.IsReplay.Value)
		{
			return;
		}
		this.replayRecorder.Server_AddPlayerBodyDespawnedEvent(playerBody);
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x0002D4C0 File Offset: 0x0002B6C0
	private void Event_Everyone_OnStickSpawned(Dictionary<string, object> message)
	{
		Stick stick = (Stick)message["stick"];
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (stick.Player.IsReplay.Value)
		{
			return;
		}
		this.replayRecorder.Server_AddStickSpawnedEvent(stick);
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x0002D50C File Offset: 0x0002B70C
	private void Event_Everyone_OnStickDespawned(Dictionary<string, object> message)
	{
		Stick stick = (Stick)message["stick"];
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (stick.Player.IsReplay.Value)
		{
			return;
		}
		this.replayRecorder.Server_AddStickDespawnedEvent(stick);
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x0002D558 File Offset: 0x0002B758
	private void Event_Everyone_OnPuckSpawned(Dictionary<string, object> message)
	{
		Puck puck = (Puck)message["puck"];
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (puck.IsReplay.Value)
		{
			return;
		}
		this.replayRecorder.Server_AddPuckSpawnedEvent(puck);
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x0002D5A0 File Offset: 0x0002B7A0
	private void Event_Everyone_OnPuckDespawned(Dictionary<string, object> message)
	{
		Puck puck = (Puck)message["puck"];
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (puck.IsReplay.Value)
		{
			return;
		}
		this.replayRecorder.Server_AddPuckDespawnedEvent(puck);
	}

	// Token: 0x040005C9 RID: 1481
	private ReplayRecorder replayRecorder;
}
