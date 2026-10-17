using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200007B RID: 123
public class ScoreboardController : MonoBehaviour
{
	// Token: 0x060003D0 RID: 976 RVA: 0x00016A3B File Offset: 0x00014C3B
	private void Awake()
	{
		this.scoreboard = base.GetComponent<Scoreboard>();
		EventManager.AddEventListener("Event_Everyone_OnGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameStateChanged));
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x00016A5F File Offset: 0x00014C5F
	private void Start()
	{
		this.scoreboard.TurnOff();
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00016A6C File Offset: 0x00014C6C
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameStateChanged));
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x00016A84 File Offset: 0x00014C84
	private void Event_Everyone_OnGameStateChanged(Dictionary<string, object> message)
	{
		GameState gameState = (GameState)message["newGameState"];
		GamePhase phase = gameState.Phase;
		if (phase - GamePhase.PreGame <= 8)
		{
			this.scoreboard.TurnOn();
		}
		else
		{
			this.scoreboard.TurnOff();
		}
		this.scoreboard.SetTick(gameState.Tick);
		this.scoreboard.SetPeriod(gameState.Period);
		this.scoreboard.SetBlueScore(gameState.BlueScore);
		this.scoreboard.SetRedScore(gameState.RedScore);
	}

	// Token: 0x040002D7 RID: 727
	private Scoreboard scoreboard;
}
