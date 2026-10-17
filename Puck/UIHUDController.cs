using System;
using System.Collections.Generic;

// Token: 0x020001D8 RID: 472
public class UIHUDController : UIViewController<UIHUD>
{
	// Token: 0x06000E52 RID: 3666 RVA: 0x00043F2C File Offset: 0x0004212C
	public override void Awake()
	{
		base.Awake();
		this.uiHud = base.GetComponent<UIHUD>();
		EventManager.AddEventListener("Event_Everyone_OnPlayerBodySpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodySpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerBodyDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodyDespawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerVoiceStarted", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerVoiceStarted));
		EventManager.AddEventListener("Event_Everyone_OnPlayerVoiceStopped", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerVoiceStopped));
		EventManager.AddEventListener("Event_OnUnitsChanged", new Action<Dictionary<string, object>>(this.Event_OnUnitsChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerPositionChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPositionChanged));
		EventManager.AddEventListener("Event_OnShowTeamColorBarChanged", new Action<Dictionary<string, object>>(this.Event_OnShowTeamColorBarChanged));
		EventManager.AddEventListener("Event_OnShowGameUserInterfaceChanged", new Action<Dictionary<string, object>>(this.Event_OnShowGameUserInterfaceChanged));
	}

	// Token: 0x06000E53 RID: 3667 RVA: 0x00044014 File Offset: 0x00042214
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerBodySpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodySpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerBodyDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodyDespawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerVoiceStarted", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerVoiceStarted));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerVoiceStopped", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerVoiceStopped));
		EventManager.RemoveEventListener("Event_OnUnitsChanged", new Action<Dictionary<string, object>>(this.Event_OnUnitsChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerPositionChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPositionChanged));
		EventManager.RemoveEventListener("Event_OnShowTeamColorBarChanged", new Action<Dictionary<string, object>>(this.Event_OnShowTeamColorBarChanged));
		EventManager.RemoveEventListener("Event_OnShowGameUserInterfaceChanged", new Action<Dictionary<string, object>>(this.Event_OnShowGameUserInterfaceChanged));
		this.UnsubscribeFromLocalPlayerBody();
		this.UnsubscribeFromLocalVoiceRecorder();
		base.OnDestroy();
	}

	// Token: 0x06000E54 RID: 3668 RVA: 0x000440FC File Offset: 0x000422FC
	private void SubscribeToLocalPlayerBody(PlayerBody playerBody)
	{
		this.UnsubscribeFromLocalPlayerBody();
		this.localPlayerBody = playerBody;
		this.localPlayerBody.Stamina.OnRawValueChanged += this.OnLocalPlayerBodyStaminaChanged;
		this.localPlayerBody.Speed.OnRawValueChanged += this.OnLocalPlayerBodySpeedChanged;
	}

	// Token: 0x06000E55 RID: 3669 RVA: 0x00044150 File Offset: 0x00042350
	private void UnsubscribeFromLocalPlayerBody()
	{
		if (!this.localPlayerBody)
		{
			return;
		}
		this.localPlayerBody.Stamina.OnRawValueChanged -= this.OnLocalPlayerBodyStaminaChanged;
		this.localPlayerBody.Speed.OnRawValueChanged -= this.OnLocalPlayerBodySpeedChanged;
		this.localPlayerBody = null;
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x000441AA File Offset: 0x000423AA
	private void UpdatePosition(PlayerPosition playerPosition)
	{
		this.uiHud.SetPosition(playerPosition ? playerPosition.Name : string.Empty);
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x000441CC File Offset: 0x000423CC
	private void RefreshTeamColorBar()
	{
		bool flag = SettingsManager.ShowTeamColorBar && SettingsManager.ShowGameUserInterface;
		this.uiHud.SetTeam(flag ? this.localTeam : PlayerTeam.None);
	}

	// Token: 0x06000E58 RID: 3672 RVA: 0x00044200 File Offset: 0x00042400
	private void Event_Everyone_OnPlayerBodySpawned(Dictionary<string, object> message)
	{
		PlayerBody playerBody = (PlayerBody)message["playerBody"];
		if (!playerBody.Player.IsLocalPlayer)
		{
			return;
		}
		this.SubscribeToLocalPlayerBody(playerBody);
		this.uiHud.Show();
		this.uiHud.SetStamina(playerBody.Stamina.Value);
		this.localTeam = playerBody.Player.Team;
		this.RefreshTeamColorBar();
		this.UpdatePosition(playerBody.Player.PlayerPosition);
	}

	// Token: 0x06000E59 RID: 3673 RVA: 0x0004427D File Offset: 0x0004247D
	private void Event_Everyone_OnPlayerBodyDespawned(Dictionary<string, object> message)
	{
		if ((PlayerBody)message["playerBody"] != this.localPlayerBody)
		{
			return;
		}
		this.UnsubscribeFromLocalPlayerBody();
	}

	// Token: 0x06000E5A RID: 3674 RVA: 0x000442A3 File Offset: 0x000424A3
	private void SubscribeToLocalVoiceRecorder(PlayerVoiceRecorder voiceRecorder)
	{
		this.UnsubscribeFromLocalVoiceRecorder();
		this.localVoiceRecorder = voiceRecorder;
		this.localVoiceRecorder.OnTransmitLevelChanged += this.OnLocalVoiceTransmitLevelChanged;
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x000442C9 File Offset: 0x000424C9
	private void UnsubscribeFromLocalVoiceRecorder()
	{
		if (this.localVoiceRecorder == null)
		{
			return;
		}
		this.localVoiceRecorder.OnTransmitLevelChanged -= this.OnLocalVoiceTransmitLevelChanged;
		this.localVoiceRecorder = null;
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x000442F8 File Offset: 0x000424F8
	private void OnLocalPlayerBodyStaminaChanged(float oldStamina, float newStamina)
	{
		this.uiHud.SetStamina(newStamina);
	}

	// Token: 0x06000E5D RID: 3677 RVA: 0x00044306 File Offset: 0x00042506
	private void OnLocalVoiceTransmitLevelChanged(float level)
	{
		this.uiHud.SetVoiceLevel(level);
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x00044314 File Offset: 0x00042514
	private void Event_Everyone_OnPlayerVoiceStarted(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (!player.IsLocalPlayer)
		{
			return;
		}
		this.SubscribeToLocalVoiceRecorder(player.GetComponent<PlayerVoiceRecorder>());
		this.uiHud.SetVoiceLevel(0f);
		this.uiHud.SetVoiceTransmitting(true);
	}

	// Token: 0x06000E5F RID: 3679 RVA: 0x00044363 File Offset: 0x00042563
	private void Event_Everyone_OnPlayerVoiceStopped(Dictionary<string, object> message)
	{
		if (!((Player)message["player"]).IsLocalPlayer)
		{
			return;
		}
		this.UnsubscribeFromLocalVoiceRecorder();
		this.uiHud.SetVoiceTransmitting(false);
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x0004438F File Offset: 0x0004258F
	private void OnLocalPlayerBodySpeedChanged(float oldSpeed, float newSpeed)
	{
		this.uiHud.SetSpeed(newSpeed);
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x000443A0 File Offset: 0x000425A0
	private void Event_OnUnitsChanged(Dictionary<string, object> message)
	{
		Units units = (Units)message["value"];
		if (units == Units.Metric)
		{
			this.uiHud.SetUnits("KPH");
			return;
		}
		if (units != Units.Imperial)
		{
			return;
		}
		this.uiHud.SetUnits("MPH");
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x000443E8 File Offset: 0x000425E8
	private void Event_Everyone_OnPlayerGameStateChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (!player.IsLocalPlayer)
		{
			return;
		}
		this.localTeam = player.Team;
		this.RefreshTeamColorBar();
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x00044421 File Offset: 0x00042621
	private void Event_Everyone_OnPlayerPositionChanged(Dictionary<string, object> message)
	{
		if (!((Player)message["player"]).IsLocalPlayer)
		{
			return;
		}
		this.UpdatePosition((PlayerPosition)message["newPlayerPosition"]);
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x00044451 File Offset: 0x00042651
	private void Event_OnShowTeamColorBarChanged(Dictionary<string, object> message)
	{
		this.RefreshTeamColorBar();
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x00044451 File Offset: 0x00042651
	private void Event_OnShowGameUserInterfaceChanged(Dictionary<string, object> message)
	{
		this.RefreshTeamColorBar();
	}

	// Token: 0x04000928 RID: 2344
	private UIHUD uiHud;

	// Token: 0x04000929 RID: 2345
	private PlayerBody localPlayerBody;

	// Token: 0x0400092A RID: 2346
	private PlayerTeam localTeam;

	// Token: 0x0400092B RID: 2347
	private PlayerVoiceRecorder localVoiceRecorder;
}
