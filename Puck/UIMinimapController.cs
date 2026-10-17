using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001E0 RID: 480
internal class UIMinimapController : UIViewController<UIMinimap>
{
	// Token: 0x06000EBF RID: 3775 RVA: 0x0004605C File Offset: 0x0004425C
	public override void Awake()
	{
		base.Awake();
		this.uiMinimap = base.GetComponent<UIMinimap>();
		EventManager.AddEventListener("Event_Everyone_OnLevelSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnLevelSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerBodySpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodySpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerNumberChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerNumberChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerBodyDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodyDespawned));
		EventManager.AddEventListener("Event_Everyone_OnPuckSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPuckDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckDespawned));
		EventManager.AddEventListener("Event_Everyone_OnStickSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnStickSpawned));
		EventManager.AddEventListener("Event_Everyone_OnStickDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnStickDespawned));
		EventManager.AddEventListener("Event_OnShowMinimapChanged", new Action<Dictionary<string, object>>(this.Event_OnShowMinimapChanged));
		EventManager.AddEventListener("Event_OnMinimapOpacityChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapOpacityChanged));
		EventManager.AddEventListener("Event_OnShowGameUserInterfaceChanged", new Action<Dictionary<string, object>>(this.Event_OnShowGameUserInterfaceChanged));
		EventManager.AddEventListener("Event_OnMinimapHorizontalPositionChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapHorizontalPositionChanged));
		EventManager.AddEventListener("Event_OnMinimapVerticalPositionChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapVerticalPositionChanged));
		EventManager.AddEventListener("Event_OnMinimapBackgroundOpacityChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapBackgroundOpacityChanged));
		EventManager.AddEventListener("Event_OnMinimapScaleChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapScaleChanged));
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x000461F4 File Offset: 0x000443F4
	private void Start()
	{
		this.uiMinimap.SetOpacity(SettingsManager.MinimapOpacity);
		this.uiMinimap.SetBackgroundOpacity(SettingsManager.MinimapBackgroundOpacity);
		this.uiMinimap.SetPosition(new Vector2(SettingsManager.MinimapHorizontalPosition, SettingsManager.MinimapVerticalPosition));
		this.uiMinimap.SetScale(SettingsManager.MinimapScale);
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x0004624C File Offset: 0x0004444C
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnLevelSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnLevelSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerBodySpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodySpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerNumberChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerNumberChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerBodyDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodyDespawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPuckSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPuckDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckDespawned));
		EventManager.RemoveEventListener("Event_Everyone_OnStickSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnStickSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnStickDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnStickDespawned));
		EventManager.RemoveEventListener("Event_OnShowMinimapChanged", new Action<Dictionary<string, object>>(this.Event_OnShowMinimapChanged));
		EventManager.RemoveEventListener("Event_OnMinimapOpacityChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapOpacityChanged));
		EventManager.RemoveEventListener("Event_OnShowGameUserInterfaceChanged", new Action<Dictionary<string, object>>(this.Event_OnShowGameUserInterfaceChanged));
		EventManager.RemoveEventListener("Event_OnMinimapHorizontalPositionChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapHorizontalPositionChanged));
		EventManager.RemoveEventListener("Event_OnMinimapVerticalPositionChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapVerticalPositionChanged));
		EventManager.RemoveEventListener("Event_OnMinimapBackgroundOpacityChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapBackgroundOpacityChanged));
		EventManager.RemoveEventListener("Event_OnMinimapScaleChanged", new Action<Dictionary<string, object>>(this.Event_OnMinimapScaleChanged));
		base.OnDestroy();
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x000463D8 File Offset: 0x000445D8
	private void HandlePlayerGameState(Player player)
	{
		PlayerGameState value = player.GameState.Value;
		this.uiMinimap.Team = value.Team;
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x00046404 File Offset: 0x00044604
	private void Event_Everyone_OnLevelSpawned(Dictionary<string, object> message)
	{
		Level level = (Level)message["level"];
		this.uiMinimap.Bounds = level.Bounds;
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x00046434 File Offset: 0x00044634
	private void Event_Everyone_OnPlayerBodySpawned(Dictionary<string, object> message)
	{
		PlayerBody playerBody = (PlayerBody)message["playerBody"];
		this.uiMinimap.AddPlayerBody(playerBody);
	}

	// Token: 0x06000EC5 RID: 3781 RVA: 0x00046460 File Offset: 0x00044660
	private void Event_Everyone_OnPlayerSpawned(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (!player.IsLocalPlayer)
		{
			return;
		}
		this.HandlePlayerGameState(player);
	}

	// Token: 0x06000EC6 RID: 3782 RVA: 0x00046490 File Offset: 0x00044690
	private void Event_Everyone_OnPlayerGameStateChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (player.IsLocalPlayer)
		{
			this.HandlePlayerGameState(player);
		}
		this.uiMinimap.StylePlayer(player.PlayerBody);
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x000464D0 File Offset: 0x000446D0
	private void Event_Everyone_OnPlayerNumberChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		this.uiMinimap.StylePlayer(player.PlayerBody);
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x00046500 File Offset: 0x00044700
	private void Event_Everyone_OnPlayerBodyDespawned(Dictionary<string, object> message)
	{
		PlayerBody playerBody = (PlayerBody)message["playerBody"];
		this.uiMinimap.RemovePlayerBody(playerBody);
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x0004652C File Offset: 0x0004472C
	private void Event_Everyone_OnPuckSpawned(Dictionary<string, object> message)
	{
		Puck puck = (Puck)message["puck"];
		this.uiMinimap.AddPuck(puck);
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x00046558 File Offset: 0x00044758
	private void Event_Everyone_OnPuckDespawned(Dictionary<string, object> message)
	{
		Puck puck = (Puck)message["puck"];
		this.uiMinimap.RemovePuck(puck);
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x00046584 File Offset: 0x00044784
	private void Event_Everyone_OnStickSpawned(Dictionary<string, object> message)
	{
		Stick stick = (Stick)message["stick"];
		this.uiMinimap.AddStick(stick);
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x000465B0 File Offset: 0x000447B0
	private void Event_Everyone_OnStickDespawned(Dictionary<string, object> message)
	{
		Stick stick = (Stick)message["stick"];
		this.uiMinimap.RemoveStick(stick);
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x000465DC File Offset: 0x000447DC
	private void Event_OnMinimapOpacityChanged(Dictionary<string, object> message)
	{
		float opacity = (float)message["value"];
		this.uiMinimap.SetOpacity(opacity);
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x00046606 File Offset: 0x00044806
	private void Event_OnShowMinimapChanged(Dictionary<string, object> message)
	{
		if (GlobalStateManager.UIState.Phase == UIPhase.LockerRoom)
		{
			return;
		}
		if ((bool)message["value"])
		{
			this.uiMinimap.Show();
			return;
		}
		this.uiMinimap.Hide();
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x00046606 File Offset: 0x00044806
	private void Event_OnShowGameUserInterfaceChanged(Dictionary<string, object> message)
	{
		if (GlobalStateManager.UIState.Phase == UIPhase.LockerRoom)
		{
			return;
		}
		if ((bool)message["value"])
		{
			this.uiMinimap.Show();
			return;
		}
		this.uiMinimap.Hide();
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x00046644 File Offset: 0x00044844
	private void Event_OnMinimapHorizontalPositionChanged(Dictionary<string, object> message)
	{
		float x = (float)message["value"];
		this.uiMinimap.SetPosition(new Vector2(x, this.uiMinimap.Position.y));
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x00046684 File Offset: 0x00044884
	private void Event_OnMinimapVerticalPositionChanged(Dictionary<string, object> message)
	{
		float y = (float)message["value"];
		this.uiMinimap.SetPosition(new Vector2(this.uiMinimap.Position.x, y));
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x000466C4 File Offset: 0x000448C4
	private void Event_OnMinimapBackgroundOpacityChanged(Dictionary<string, object> message)
	{
		float backgroundOpacity = (float)message["value"];
		this.uiMinimap.SetBackgroundOpacity(backgroundOpacity);
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x000466F0 File Offset: 0x000448F0
	private void Event_OnMinimapScaleChanged(Dictionary<string, object> message)
	{
		float scale = (float)message["value"];
		this.uiMinimap.SetScale(scale);
	}

	// Token: 0x0400095E RID: 2398
	private UIMinimap uiMinimap;
}
