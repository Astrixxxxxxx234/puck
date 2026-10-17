using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000128 RID: 296
public class PuckManagerController : MonoBehaviour
{
	// Token: 0x060008C4 RID: 2244 RVA: 0x0002B280 File Offset: 0x00029480
	private void Awake()
	{
		this.puckManager = base.GetComponent<PuckManager>();
		EventManager.AddEventListener("Event_Everyone_OnPuckPositionSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckPositionSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPuckPositionDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckPositionDespawned));
		EventManager.AddEventListener("Event_Everyone_OnPuckSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPuckDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckDespawned));
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x0002B2F4 File Offset: 0x000294F4
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnPuckPositionSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckPositionSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPuckPositionDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckPositionDespawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPuckSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPuckDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckDespawned));
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0002B35C File Offset: 0x0002955C
	private void Event_Everyone_OnPuckPositionSpawned(Dictionary<string, object> message)
	{
		PuckPosition puckPosition = (PuckPosition)message["puckPosition"];
		this.puckManager.AddPuckPosition(puckPosition);
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x0002B388 File Offset: 0x00029588
	private void Event_Everyone_OnPuckPositionDespawned(Dictionary<string, object> message)
	{
		PuckPosition puckPosition = (PuckPosition)message["puckPosition"];
		this.puckManager.RemovePuckPosition(puckPosition);
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x0002B3B4 File Offset: 0x000295B4
	private void Event_Everyone_OnPuckSpawned(Dictionary<string, object> message)
	{
		Puck puck = (Puck)message["puck"];
		this.puckManager.AddPuck(puck);
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0002B3E0 File Offset: 0x000295E0
	private void Event_Everyone_OnPuckDespawned(Dictionary<string, object> message)
	{
		Puck puck = (Puck)message["puck"];
		this.puckManager.RemovePuck(puck);
	}

	// Token: 0x04000572 RID: 1394
	private PuckManager puckManager;
}
