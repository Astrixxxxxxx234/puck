using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class LevelController : MonoBehaviour
{
	// Token: 0x0600005E RID: 94 RVA: 0x00002FF0 File Offset: 0x000011F0
	public virtual void Awake()
	{
		this.level = base.GetComponent<Level>();
		EventManager.AddEventListener("Event_Everyone_OnGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameStateChanged));
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00003014 File Offset: 0x00001214
	public virtual void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameStateChanged));
	}

	// Token: 0x06000060 RID: 96 RVA: 0x0000302C File Offset: 0x0000122C
	private void Event_Everyone_OnGameStateChanged(Dictionary<string, object> eventParams)
	{
		ref GameState ptr = (GameState)eventParams["oldGameState"];
		GameState gameState = (GameState)eventParams["newGameState"];
		if (ptr.Phase == gameState.Phase)
		{
			return;
		}
		switch (gameState.Phase)
		{
		case GamePhase.Warmup:
		case GamePhase.PreGame:
		case GamePhase.FaceOff:
			this.level.SetBlueGoalLightEnabled(false);
			this.level.SetRedGoalLightEnabled(false);
			return;
		case GamePhase.Play:
			break;
		case GamePhase.BlueScore:
			this.level.SetRedGoalLightEnabled(true);
			return;
		case GamePhase.RedScore:
			this.level.SetBlueGoalLightEnabled(true);
			break;
		default:
			return;
		}
	}

	// Token: 0x04000039 RID: 57
	private Level level;
}
