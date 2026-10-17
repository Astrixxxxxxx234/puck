using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000181 RID: 385
public class SynchronizedObjectManagerController : MonoBehaviour
{
	// Token: 0x06000C03 RID: 3075 RVA: 0x00039980 File Offset: 0x00037B80
	private void Awake()
	{
		this.synchronizedObjectManager = base.GetComponent<SynchronizedObjectManager>();
		EventManager.AddEventListener("Event_Everyone_OnSynchronizedObjectSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnSynchronizedObjectSpawned));
		EventManager.AddEventListener("Event_Everyone_OnSynchronizedObjectDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnSynchronizedObjectDespawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerDespawned));
		EventManager.AddEventListener("Event_OnPhysicsManagerInitialized", new Action<Dictionary<string, object>>(this.Event_OnPhysicsManagerInitialized));
		EventManager.AddEventListener("Event_OnPhysicsManagerTickRateChanged", new Action<Dictionary<string, object>>(this.Event_OnPhysicsManagerTickRateChanged));
		EventManager.AddEventListener("Event_OnNetworkBufferingChanged", new Action<Dictionary<string, object>>(this.Event_OnNetworkBufferingChanged));
		EventManager.AddEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
		EventManager.AddEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
		EventManager.AddEventListener("Event_Server_OnClientSceneSynchronizeComplete", new Action<Dictionary<string, object>>(this.Event_Server_OnClientSceneSynchronizeComplete));
	}

	// Token: 0x06000C04 RID: 3076 RVA: 0x00039A75 File Offset: 0x00037C75
	private void Start()
	{
		this.synchronizedObjectManager.Client_SetTargetTimelinePosition(Utils.GetTargetTimelinePositionFromNetworkBuffering(SettingsManager.NetworkBuffering));
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x00039A8C File Offset: 0x00037C8C
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnSynchronizedObjectSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnSynchronizedObjectSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnSynchronizedObjectDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnSynchronizedObjectDespawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerDespawned));
		EventManager.RemoveEventListener("Event_OnPhysicsManagerInitialized", new Action<Dictionary<string, object>>(this.Event_OnPhysicsManagerInitialized));
		EventManager.RemoveEventListener("Event_OnPhysicsManagerTickRateChanged", new Action<Dictionary<string, object>>(this.Event_OnPhysicsManagerTickRateChanged));
		EventManager.RemoveEventListener("Event_OnNetworkBufferingChanged", new Action<Dictionary<string, object>>(this.Event_OnNetworkBufferingChanged));
		EventManager.RemoveEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
		EventManager.RemoveEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
		EventManager.RemoveEventListener("Event_Server_OnClientSceneSynchronizeComplete", new Action<Dictionary<string, object>>(this.Event_Server_OnClientSceneSynchronizeComplete));
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x00039B78 File Offset: 0x00037D78
	private void Event_Everyone_OnSynchronizedObjectSpawned(Dictionary<string, object> message)
	{
		SynchronizedObject synchronizedObject = (SynchronizedObject)message["synchronizedObject"];
		this.synchronizedObjectManager.AddSynchronizedObject(synchronizedObject);
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x00039BA4 File Offset: 0x00037DA4
	private void Event_Everyone_OnSynchronizedObjectDespawned(Dictionary<string, object> message)
	{
		SynchronizedObject synchronizedObject = (SynchronizedObject)message["synchronizedObject"];
		this.synchronizedObjectManager.RemoveSynchronizedObject(synchronizedObject);
	}

	// Token: 0x06000C08 RID: 3080 RVA: 0x00039BD0 File Offset: 0x00037DD0
	private void Event_Everyone_OnPlayerSpawned(Dictionary<string, object> message)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player player = (Player)message["player"];
		if (player.IsReplay.Value)
		{
			return;
		}
		if (player.OwnerClientId == 0UL)
		{
			return;
		}
		this.synchronizedObjectManager.Server_AddSynchronizedPlayer(player);
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x00039C20 File Offset: 0x00037E20
	private void Event_Everyone_OnPlayerDespawned(Dictionary<string, object> message)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player player = (Player)message["player"];
		if (player.IsReplay.Value)
		{
			return;
		}
		if (player.OwnerClientId == 0UL)
		{
			return;
		}
		this.synchronizedObjectManager.Server_RemoveSynchronizedPlayer(player);
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x00039C70 File Offset: 0x00037E70
	private void Event_OnPhysicsManagerInitialized(Dictionary<string, object> message)
	{
		int tickRate = (int)message["tickRate"];
		this.synchronizedObjectManager.TickRate = tickRate;
	}

	// Token: 0x06000C0B RID: 3083 RVA: 0x00039C9C File Offset: 0x00037E9C
	private void Event_OnPhysicsManagerTickRateChanged(Dictionary<string, object> message)
	{
		int tickRate = (int)message["value"];
		this.synchronizedObjectManager.TickRate = tickRate;
	}

	// Token: 0x06000C0C RID: 3084 RVA: 0x00039CC8 File Offset: 0x00037EC8
	private void Event_OnNetworkBufferingChanged(Dictionary<string, object> message)
	{
		NetworkBuffering networkBuffering = (NetworkBuffering)message["value"];
		this.synchronizedObjectManager.Client_SetTargetTimelinePosition(Utils.GetTargetTimelinePositionFromNetworkBuffering(networkBuffering));
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x00039CF7 File Offset: 0x00037EF7
	private void Event_OnClientStopped(Dictionary<string, object> message)
	{
		this.synchronizedObjectManager.Client_Dispose();
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x00039D04 File Offset: 0x00037F04
	private void Event_Server_OnServerStopped(Dictionary<string, object> message)
	{
		this.synchronizedObjectManager.Server_Dispose();
	}

	// Token: 0x06000C0F RID: 3087 RVA: 0x00039D14 File Offset: 0x00037F14
	private void Event_Server_OnClientSceneSynchronizeComplete(Dictionary<string, object> message)
	{
		ulong num = (ulong)message["clientId"];
		if (num == 0UL)
		{
			return;
		}
		this.synchronizedObjectManager.Server_ForceSynchronizeClientId(num);
	}

	// Token: 0x04000769 RID: 1897
	private SynchronizedObjectManager synchronizedObjectManager;
}
