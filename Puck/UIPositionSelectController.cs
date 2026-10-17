using System;
using System.Collections.Generic;

// Token: 0x02000202 RID: 514
internal class UIPositionSelectController : UIViewController<UIPositionSelect>
{
	// Token: 0x06000FEE RID: 4078 RVA: 0x0004B500 File Offset: 0x00049700
	public override void Awake()
	{
		base.Awake();
		this.uiPositionSelect = base.GetComponent<UIPositionSelect>();
		EventManager.AddEventListener("Event_Everyone_OnPlayerPositionSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPositionSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerPositionDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPositionDespawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerPositionClaimedByPlayerChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPositionClaimedByPlayerChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
	}

	// Token: 0x06000FEF RID: 4079 RVA: 0x0004B590 File Offset: 0x00049790
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerPositionSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPositionSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerPositionDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPositionDespawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerPositionClaimedByPlayerChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPositionClaimedByPlayerChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		base.OnDestroy();
	}

	// Token: 0x06000FF0 RID: 4080 RVA: 0x0004B614 File Offset: 0x00049814
	private void HandlePlayerGameState(Player player)
	{
		PlayerGameState value = player.GameState.Value;
		this.uiPositionSelect.Team = value.Team;
	}

	// Token: 0x06000FF1 RID: 4081 RVA: 0x0004B640 File Offset: 0x00049840
	private void Event_Everyone_OnPlayerPositionSpawned(Dictionary<string, object> message)
	{
		PlayerPosition playerPosition = (PlayerPosition)message["playerPosition"];
		this.uiPositionSelect.AddPosition(playerPosition);
	}

	// Token: 0x06000FF2 RID: 4082 RVA: 0x0004B66C File Offset: 0x0004986C
	private void Event_Everyone_OnPlayerPositionDespawned(Dictionary<string, object> message)
	{
		PlayerPosition playerPosition = (PlayerPosition)message["playerPosition"];
		this.uiPositionSelect.RemovePosition(playerPosition);
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x0004B698 File Offset: 0x00049898
	private void Event_Everyone_OnPlayerPositionClaimedByPlayerChanged(Dictionary<string, object> message)
	{
		PlayerPosition playerPosition = (PlayerPosition)message["playerPosition"];
		this.uiPositionSelect.StylePosition(playerPosition);
	}

	// Token: 0x06000FF4 RID: 4084 RVA: 0x0004B6C4 File Offset: 0x000498C4
	private void Event_Everyone_OnPlayerSpawned(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (!player.IsLocalPlayer)
		{
			return;
		}
		this.HandlePlayerGameState(player);
	}

	// Token: 0x06000FF5 RID: 4085 RVA: 0x0004B6F4 File Offset: 0x000498F4
	private void Event_Everyone_OnPlayerGameStateChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		PlayerGameState playerGameState = (PlayerGameState)message["oldGameState"];
		PlayerGameState playerGameState2 = (PlayerGameState)message["newGameState"];
		if (!player.IsLocalPlayer)
		{
			return;
		}
		if (playerGameState.Team == playerGameState2.Team)
		{
			return;
		}
		this.HandlePlayerGameState(player);
	}

	// Token: 0x04000A15 RID: 2581
	private UIPositionSelect uiPositionSelect;
}
