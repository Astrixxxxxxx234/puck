using System;
using System.Collections.Generic;
using DG.Tweening;

// Token: 0x020001EE RID: 494
public class UIPauseMenuController : UIViewController<UIPauseMenu>
{
	// Token: 0x06000F60 RID: 3936 RVA: 0x0004924C File Offset: 0x0004744C
	public override void Awake()
	{
		base.Awake();
		this.uiPauseMenu = base.GetComponent<UIPauseMenu>();
		EventManager.AddEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.AddEventListener("Event_Everyone_OnGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameStateChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.AddEventListener("Event_Everyone_OnServerChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnServerChanged));
		EventManager.AddEventListener("Event_OnPauseMenuClickForfeit", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickForfeit));
	}

	// Token: 0x06000F61 RID: 3937 RVA: 0x000492DC File Offset: 0x000474DC
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGameStateChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnServerChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnServerChanged));
		EventManager.RemoveEventListener("Event_OnPauseMenuClickForfeit", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickForfeit));
		Tween tween = this.forfeitVotedResetTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		base.OnDestroy();
	}

	// Token: 0x06000F62 RID: 3938 RVA: 0x00049370 File Offset: 0x00047570
	private void Event_Everyone_OnPlayerSpawned(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (!player.IsLocalPlayer)
		{
			return;
		}
		this.localPlayer = player;
		this.RefreshButtons();
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x000493A4 File Offset: 0x000475A4
	private void Event_Everyone_OnGameStateChanged(Dictionary<string, object> message)
	{
		GameState gameState = (GameState)message["newGameState"];
		this.phase = gameState.Phase;
		this.RefreshButtons();
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x000493D4 File Offset: 0x000475D4
	private void Event_Everyone_OnPlayerGameStateChanged(Dictionary<string, object> message)
	{
		if ((Player)message["player"] != this.localPlayer)
		{
			return;
		}
		this.RefreshButtons();
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x000493FA File Offset: 0x000475FA
	private void Event_Everyone_OnServerChanged(Dictionary<string, object> message)
	{
		this.RefreshButtons();
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x00049404 File Offset: 0x00047604
	private void Event_OnPauseMenuClickForfeit(Dictionary<string, object> message)
	{
		this.forfeitVoted = true;
		this.uiPauseMenu.SetForfeitVoted(true);
		this.RefreshButtons();
		Tween tween = this.forfeitVotedResetTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.forfeitVotedResetTween = DOVirtual.DelayedCall(5f, delegate
		{
			this.forfeitVoted = false;
			this.uiPauseMenu.SetForfeitVoted(false);
			this.RefreshButtons();
		}, true);
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x0004945C File Offset: 0x0004765C
	private void RefreshButtons()
	{
		bool flag = this.localPlayer != null && (this.localPlayer.Team == PlayerTeam.Blue || this.localPlayer.Team == PlayerTeam.Red);
		bool flag2 = NetworkBehaviourSingleton<ServerManager>.Instance != null && NetworkBehaviourSingleton<ServerManager>.Instance.Server.Value.GameMode == "competitive";
		bool voiceEnabled = NetworkBehaviourSingleton<ServerManager>.Instance != null && NetworkBehaviourSingleton<ServerManager>.Instance.Server.Value.UseVoip;
		int tickRate = (NetworkBehaviourSingleton<ServerManager>.Instance != null) ? NetworkBehaviourSingleton<ServerManager>.Instance.Server.Value.TickRate : 0;
		this.uiPauseMenu.SetSelectPositionEnabled(flag);
		this.uiPauseMenu.SetForfeitEnabled(Utils.IsGameInProgress(this.phase) && flag && !this.forfeitVoted);
		this.uiPauseMenu.SetSelectTeamEnabled(!flag2);
		this.uiPauseMenu.SetVoiceEnabled(voiceEnabled);
		this.uiPauseMenu.SetTickRate(tickRate);
	}

	// Token: 0x040009C5 RID: 2501
	private const float forfeitVotedResetDelay = 5f;

	// Token: 0x040009C6 RID: 2502
	private UIPauseMenu uiPauseMenu;

	// Token: 0x040009C7 RID: 2503
	private Player localPlayer;

	// Token: 0x040009C8 RID: 2504
	private GamePhase phase;

	// Token: 0x040009C9 RID: 2505
	private bool forfeitVoted;

	// Token: 0x040009CA RID: 2506
	private Tween forfeitVotedResetTween;
}
