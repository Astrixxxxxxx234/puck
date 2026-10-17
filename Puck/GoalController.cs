using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class GoalController : MonoBehaviour
{
	// Token: 0x06000045 RID: 69 RVA: 0x00002B82 File Offset: 0x00000D82
	private void Awake()
	{
		this.goal = base.GetComponent<Goal>();
		EventManager.AddEventListener("Event_Everyone_OnPuckSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPuckDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckDespawned));
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00002BBC File Offset: 0x00000DBC
	public void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnPuckSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPuckDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckDespawned));
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00002BEC File Offset: 0x00000DEC
	private void Event_Everyone_OnPuckSpawned(Dictionary<string, object> message)
	{
		Puck puck = (Puck)message["puck"];
		if (!NetworkManager.Singleton.IsClient)
		{
			return;
		}
		this.goal.Client_AddNetClothSphereCollider(puck.NetSphereCollider);
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00002C28 File Offset: 0x00000E28
	private void Event_Everyone_OnPuckDespawned(Dictionary<string, object> message)
	{
		Puck puck = (Puck)message["puck"];
		if (!NetworkManager.Singleton.IsClient)
		{
			return;
		}
		this.goal.Client_RemoveNetClothSphereCollider(puck.NetSphereCollider);
	}

	// Token: 0x0400002A RID: 42
	private Goal goal;
}
