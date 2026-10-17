using System;
using UnityEngine.UIElements;

// Token: 0x020001ED RID: 493
public class UIPauseMenu : UIView
{
	// Token: 0x06000F50 RID: 3920 RVA: 0x00048EF0 File Offset: 0x000470F0
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("PauseMenuView", null);
		this.pauseMenu = base.View.Query("PauseMenu", null);
		this.returnToGameButton = this.pauseMenu.Query("ReturnToGameButton", null);
		this.returnToGameButton.clicked += this.OnClickReturnToGame;
		this.selectTeamButton = this.pauseMenu.Query("SelectTeamButton", null);
		this.selectTeamButton.clicked += this.OnClickSelectTeam;
		this.selectPositionButton = this.pauseMenu.Query("SelectPositionButton", null);
		this.selectPositionButton.clicked += this.OnClickSelectPosition;
		this.selectPositionButton.SetEnabled(false);
		this.forfeitButton = this.pauseMenu.Query("ForfeitButton", null);
		this.forfeitButton.clicked += this.OnClickForfeit;
		this.forfeitButton.SetEnabled(false);
		this.serverBrowserButton = this.pauseMenu.Query("ServerBrowserButton", null);
		this.serverBrowserButton.clicked += this.OnClickServerBrowser;
		this.settingsButton = this.pauseMenu.Query("SettingsButton", null);
		this.settingsButton.clicked += this.OnClickSettings;
		this.disconnectButton = this.pauseMenu.Query("DisconnectButton", null);
		this.disconnectButton.clicked += this.OnClickDisconnect;
		this.exitGameButton = this.pauseMenu.Query("ExitGameButton", null);
		this.exitGameButton.clicked += this.OnClickExitGame;
		this.voiceStatus = base.View.Query("VoiceStatus", null);
		this.voiceStatusLabel = base.View.Query("VoiceStatusLabel", null);
		this.tickRateLabel = base.View.Query("TickRateLabel", null);
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x00049134 File Offset: 0x00047334
	public void SetVoiceEnabled(bool value)
	{
		this.voiceStatus.EnableInClassList("enabled", value);
		this.voiceStatus.EnableInClassList("disabled", !value);
		this.voiceStatusLabel.text = (value ? "Voice Enabled" : "Voice Disabled");
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x00049180 File Offset: 0x00047380
	public void SetTickRate(int tickRate)
	{
		this.tickRateLabel.text = string.Format("Tick Rate: {0} Hz", tickRate);
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x0004919D File Offset: 0x0004739D
	private void OnClickReturnToGame()
	{
		EventManager.TriggerEvent("Event_OnPauseMenuClickReturnToGame", null);
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x000491AA File Offset: 0x000473AA
	private void OnClickSelectTeam()
	{
		EventManager.TriggerEvent("Event_OnPauseMenuClickSelectTeam", null);
	}

	// Token: 0x06000F55 RID: 3925 RVA: 0x000491B7 File Offset: 0x000473B7
	private void OnClickSelectPosition()
	{
		EventManager.TriggerEvent("Event_OnPauseMenuClickSelectPosition", null);
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x000491C4 File Offset: 0x000473C4
	public void SetSelectTeamEnabled(bool value)
	{
		this.selectTeamButton.SetEnabled(value);
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x000491D2 File Offset: 0x000473D2
	public void SetSelectPositionEnabled(bool value)
	{
		this.selectPositionButton.SetEnabled(value);
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x000491E0 File Offset: 0x000473E0
	public void SetForfeitEnabled(bool value)
	{
		this.forfeitButton.SetEnabled(value);
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x000491EE File Offset: 0x000473EE
	public void SetForfeitVoted(bool value)
	{
		this.forfeitButton.text = (value ? "VOTED TO FORFEIT" : "VOTE TO FORFEIT");
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x0004920A File Offset: 0x0004740A
	private void OnClickForfeit()
	{
		EventManager.TriggerEvent("Event_OnPauseMenuClickForfeit", null);
	}

	// Token: 0x06000F5B RID: 3931 RVA: 0x00049217 File Offset: 0x00047417
	private void OnClickServerBrowser()
	{
		EventManager.TriggerEvent("Event_OnPauseMenuClickServerBrowser", null);
	}

	// Token: 0x06000F5C RID: 3932 RVA: 0x00049224 File Offset: 0x00047424
	private void OnClickSettings()
	{
		EventManager.TriggerEvent("Event_OnPauseMenuClickSettings", null);
	}

	// Token: 0x06000F5D RID: 3933 RVA: 0x00049231 File Offset: 0x00047431
	private void OnClickDisconnect()
	{
		EventManager.TriggerEvent("Event_OnPauseMenuClickDisconnect", null);
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x0004923E File Offset: 0x0004743E
	private void OnClickExitGame()
	{
		EventManager.TriggerEvent("Event_OnPauseMenuClickExitGame", null);
	}

	// Token: 0x040009B9 RID: 2489
	private VisualElement pauseMenu;

	// Token: 0x040009BA RID: 2490
	private Button returnToGameButton;

	// Token: 0x040009BB RID: 2491
	private Button selectTeamButton;

	// Token: 0x040009BC RID: 2492
	private Button selectPositionButton;

	// Token: 0x040009BD RID: 2493
	private Button forfeitButton;

	// Token: 0x040009BE RID: 2494
	private Button serverBrowserButton;

	// Token: 0x040009BF RID: 2495
	private Button disconnectButton;

	// Token: 0x040009C0 RID: 2496
	private Button settingsButton;

	// Token: 0x040009C1 RID: 2497
	private Button exitGameButton;

	// Token: 0x040009C2 RID: 2498
	private VisualElement voiceStatus;

	// Token: 0x040009C3 RID: 2499
	private Label voiceStatusLabel;

	// Token: 0x040009C4 RID: 2500
	private Label tickRateLabel;
}
