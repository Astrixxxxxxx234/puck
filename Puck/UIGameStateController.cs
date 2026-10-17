using System;
using System.Collections.Generic;

// Token: 0x020001D6 RID: 470
public class UIGameStateController : UIViewController<UIGameState>
{
	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000E41 RID: 3649 RVA: 0x00043AD5 File Offset: 0x00041CD5
	private int RegulationPeriods
	{
		get
		{
			if (!(NetworkBehaviourSingleton<GameModeManager>.Instance != null) || NetworkBehaviourSingleton<GameModeManager>.Instance.ClientConfig.Value.MaxPeriods <= 0)
			{
				return 3;
			}
			return NetworkBehaviourSingleton<GameModeManager>.Instance.ClientConfig.Value.MaxPeriods;
		}
	}

	// Token: 0x06000E42 RID: 3650 RVA: 0x00043B14 File Offset: 0x00041D14
	public override void Awake()
	{
		base.Awake();
		this.uiGameState = base.GetComponent<UIGameState>();
		EventManager.AddEventListener("Event_Everyone_OnGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameStateChanged));
		EventManager.AddEventListener("Event_Everyone_OnGameModeClientConfigChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameModeClientConfigChanged));
		EventManager.AddEventListener("Event_OnShowGameUserInterfaceChanged", new Action<Dictionary<string, object>>(this.Event_OnShowGameUserInterfaceChanged));
	}

	// Token: 0x06000E43 RID: 3651 RVA: 0x00043B78 File Offset: 0x00041D78
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameStateChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnGameModeClientConfigChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameModeClientConfigChanged));
		EventManager.RemoveEventListener("Event_OnShowGameUserInterfaceChanged", new Action<Dictionary<string, object>>(this.Event_OnShowGameUserInterfaceChanged));
		base.OnDestroy();
	}

	// Token: 0x06000E44 RID: 3652 RVA: 0x00043BCD File Offset: 0x00041DCD
	private void SetPhase(GameState gameState)
	{
		this.uiGameState.SetPhase(Utils.GetHumanizedGamePhase(gameState.Phase, gameState.Period, gameState.IsOvertime, this.RegulationPeriods));
	}

	// Token: 0x06000E45 RID: 3653 RVA: 0x00043BF8 File Offset: 0x00041DF8
	private void Event_Everyone_OnGameStateChanged(Dictionary<string, object> message)
	{
		GameState gameState = (GameState)message["newGameState"];
		this.SetPhase(gameState);
		this.uiGameState.SetTick(gameState.Tick);
		this.uiGameState.SetScore(PlayerTeam.Blue, gameState.BlueScore);
		this.uiGameState.SetScore(PlayerTeam.Red, gameState.RedScore);
	}

	// Token: 0x06000E46 RID: 3654 RVA: 0x00043C52 File Offset: 0x00041E52
	private void Event_Everyone_OnGameModeClientConfigChanged(Dictionary<string, object> message)
	{
		if (NetworkBehaviourSingleton<GameManager>.Instance == null || NetworkBehaviourSingleton<GameManager>.Instance.GameState == null)
		{
			return;
		}
		this.SetPhase(NetworkBehaviourSingleton<GameManager>.Instance.GameState.Value);
	}

	// Token: 0x06000E47 RID: 3655 RVA: 0x00043C83 File Offset: 0x00041E83
	private void Event_OnShowGameUserInterfaceChanged(Dictionary<string, object> message)
	{
		if (GlobalStateManager.UIState.Phase == UIPhase.LockerRoom)
		{
			return;
		}
		if ((bool)message["value"])
		{
			this.uiGameState.Show();
			return;
		}
		this.uiGameState.Hide();
	}

	// Token: 0x0400091D RID: 2333
	private UIGameState uiGameState;
}
