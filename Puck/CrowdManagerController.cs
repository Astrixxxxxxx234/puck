using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C4 RID: 196
public class CrowdManagerController : MonoBehaviour
{
	// Token: 0x06000639 RID: 1593 RVA: 0x00022688 File Offset: 0x00020888
	private void Awake()
	{
		this.crowdManager = base.GetComponent<CrowdManager>();
		EventManager.AddEventListener("Event_OnCrowdPositionSpawned", new Action<Dictionary<string, object>>(this.Event_OnCrowdPositionSpawned));
		EventManager.AddEventListener("Event_OnCrowdPositionDespawned", new Action<Dictionary<string, object>>(this.Event_OnCrowdPositionDespawned));
		EventManager.AddEventListener("Event_Everyone_OnPuckSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckSpawned));
		EventManager.AddEventListener("Event_Everyone_OnGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameStateChanged));
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x000226FC File Offset: 0x000208FC
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnCrowdPositionSpawned", new Action<Dictionary<string, object>>(this.Event_OnCrowdPositionSpawned));
		EventManager.RemoveEventListener("Event_OnCrowdPositionDespawned", new Action<Dictionary<string, object>>(this.Event_OnCrowdPositionDespawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPuckSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameStateChanged));
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x00022764 File Offset: 0x00020964
	private void Event_OnCrowdPositionSpawned(Dictionary<string, object> message)
	{
		CrowdPosition position = (CrowdPosition)message["crowdPosition"];
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		this.crowdManager.RegisterCrowdPosition(position);
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x00022798 File Offset: 0x00020998
	private void Event_OnCrowdPositionDespawned(Dictionary<string, object> message)
	{
		CrowdPosition position = (CrowdPosition)message["crowdPosition"];
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		this.crowdManager.UnregisterCrowdPosition(position);
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x000227CC File Offset: 0x000209CC
	private void Event_Everyone_OnPuckSpawned(Dictionary<string, object> message)
	{
		Puck puck = (Puck)message["puck"];
		this.crowdManager.SetCrowdLookTarget(puck.transform);
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x000227FC File Offset: 0x000209FC
	private void Event_Everyone_OnGameStateChanged(Dictionary<string, object> message)
	{
		ref GameState ptr = (GameState)message["oldGameState"];
		GameState gameState = (GameState)message["newGameState"];
		if (ptr.Phase == gameState.Phase)
		{
			return;
		}
		GamePhase phase = gameState.Phase;
		if (phase - GamePhase.BlueScore <= 1)
		{
			this.crowdManager.SetCrowdAnimation("Cheering");
			return;
		}
		this.crowdManager.SetCrowdAnimation("Seated");
	}

	// Token: 0x0400040A RID: 1034
	private CrowdManager crowdManager;
}
