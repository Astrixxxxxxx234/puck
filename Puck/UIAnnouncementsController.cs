using System;
using System.Collections.Generic;

// Token: 0x020001AC RID: 428
internal class UIAnnouncementsController : UIViewController<UIAnnouncements>
{
	// Token: 0x06000D20 RID: 3360 RVA: 0x0003E4EC File Offset: 0x0003C6EC
	public override void Awake()
	{
		base.Awake();
		this.uiAnnouncements = base.GetComponent<UIAnnouncements>();
		EventManager.AddEventListener("Event_Everyone_OnGoalScored", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGoalScored));
		EventManager.AddEventListener("Event_Everyone_OnGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameStateChanged));
		EventManager.AddEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x0003E550 File Offset: 0x0003C750
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnGoalScored", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGoalScored));
		EventManager.RemoveEventListener("Event_Everyone_OnGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameStateChanged));
		EventManager.RemoveEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
		base.OnDestroy();
	}

	// Token: 0x06000D22 RID: 3362 RVA: 0x0003E5A8 File Offset: 0x0003C7A8
	public void Event_Everyone_OnGoalScored(Dictionary<string, object> message)
	{
		PlayerTeam team = (PlayerTeam)message["byTeam"];
		Player goalPlayer = (Player)message["goalPlayer"];
		Player assistPlayer = (Player)message["assistPlayer"];
		Player secondAssistPlayer = (Player)message["secondAssistPlayer"];
		this.uiAnnouncements.ShowScore(team, goalPlayer, assistPlayer, secondAssistPlayer);
	}

	// Token: 0x06000D23 RID: 3363 RVA: 0x0003E608 File Offset: 0x0003C808
	public void Event_Everyone_OnGameStateChanged(Dictionary<string, object> message)
	{
		GameState gameState = (GameState)message["newGameState"];
		if (gameState.Phase != GamePhase.BlueScore && gameState.Phase != GamePhase.RedScore)
		{
			this.uiAnnouncements.HideScore();
		}
	}

	// Token: 0x06000D24 RID: 3364 RVA: 0x0003E648 File Offset: 0x0003C848
	private void Event_OnClientStopped(Dictionary<string, object> message)
	{
		this.uiAnnouncements.HideScore();
	}

	// Token: 0x04000831 RID: 2097
	private UIAnnouncements uiAnnouncements;
}
