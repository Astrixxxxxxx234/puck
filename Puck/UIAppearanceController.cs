using System;
using System.Collections.Generic;

// Token: 0x020001C3 RID: 451
public class UIAppearanceController : UIViewController<UIAppearance>
{
	// Token: 0x06000D7B RID: 3451 RVA: 0x0004058C File Offset: 0x0003E78C
	public override void Awake()
	{
		base.Awake();
		this.uiAppearance = base.GetComponent<UIAppearance>();
		EventManager.AddEventListener("Event_OnTeamChanged", new Action<Dictionary<string, object>>(this.Event_OnTeamChanged));
		EventManager.AddEventListener("Event_OnRoleChanged", new Action<Dictionary<string, object>>(this.Event_OnRoleChanged));
		EventManager.AddEventListener("Event_OnApplyForBothTeamsChanged", new Action<Dictionary<string, object>>(this.Event_OnApplyForBothTeamsChanged));
		EventManager.AddEventListener("Event_OnFlagIDChanged", new Action<Dictionary<string, object>>(this.Event_OnFlagIDChanged));
		EventManager.AddEventListener("Event_OnHeadgearIDChanged", new Action<Dictionary<string, object>>(this.Event_OnHeadgearIDChanged));
		EventManager.AddEventListener("Event_OnMustacheIDChanged", new Action<Dictionary<string, object>>(this.Event_OnMustacheIDChanged));
		EventManager.AddEventListener("Event_OnBeardIDChanged", new Action<Dictionary<string, object>>(this.Event_OnBeardIDChanged));
		EventManager.AddEventListener("Event_OnJerseyIDChanged", new Action<Dictionary<string, object>>(this.Event_OnJerseyIDChanged));
		EventManager.AddEventListener("Event_OnStickSkinIDChanged", new Action<Dictionary<string, object>>(this.Event_OnStickSkinIDChanged));
		EventManager.AddEventListener("Event_OnStickShaftTapeIDChanged", new Action<Dictionary<string, object>>(this.Event_OnStickShaftTapeIDChanged));
		EventManager.AddEventListener("Event_OnStickBladeTapeIDChanged", new Action<Dictionary<string, object>>(this.Event_OnStickBladeTapeIDChanged));
		EventManager.AddEventListener("Event_OnPlayerDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerDataChanged));
		EventManager.AddEventListener("Event_OnAppearanceHide", new Action<Dictionary<string, object>>(this.Event_OnAppearanceHide));
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x000406CC File Offset: 0x0003E8CC
	private void Start()
	{
		this.uiAppearance.SetTeam(SettingsManager.Team);
		this.uiAppearance.SetRole(SettingsManager.Role);
		this.uiAppearance.SetApplyForBothTeams(SettingsManager.ApplyForBothTeams);
		this.uiAppearance.SetFlagID(SettingsManager.FlagID);
		this.uiAppearance.SetHeadgearID(PlayerTeam.Blue, PlayerRole.Attacker, SettingsManager.HeadgearIDBlueAttacker);
		this.uiAppearance.SetHeadgearID(PlayerTeam.Blue, PlayerRole.Goalie, SettingsManager.HeadgearIDBlueGoalie);
		this.uiAppearance.SetHeadgearID(PlayerTeam.Red, PlayerRole.Attacker, SettingsManager.HeadgearIDRedAttacker);
		this.uiAppearance.SetHeadgearID(PlayerTeam.Red, PlayerRole.Goalie, SettingsManager.HeadgearIDRedGoalie);
		this.uiAppearance.SetMustacheID(SettingsManager.MustacheID);
		this.uiAppearance.SetBeardID(SettingsManager.BeardID);
		this.uiAppearance.SetJerseyID(PlayerTeam.Blue, PlayerRole.Attacker, SettingsManager.JerseyIDBlueAttacker);
		this.uiAppearance.SetJerseyID(PlayerTeam.Blue, PlayerRole.Goalie, SettingsManager.JerseyIDBlueGoalie);
		this.uiAppearance.SetJerseyID(PlayerTeam.Red, PlayerRole.Attacker, SettingsManager.JerseyIDRedAttacker);
		this.uiAppearance.SetJerseyID(PlayerTeam.Red, PlayerRole.Goalie, SettingsManager.JerseyIDRedGoalie);
		this.uiAppearance.SetStickSkinID(PlayerTeam.Blue, PlayerRole.Attacker, SettingsManager.StickSkinIDBlueAttacker);
		this.uiAppearance.SetStickSkinID(PlayerTeam.Blue, PlayerRole.Goalie, SettingsManager.StickSkinIDBlueGoalie);
		this.uiAppearance.SetStickSkinID(PlayerTeam.Red, PlayerRole.Attacker, SettingsManager.StickSkinIDRedAttacker);
		this.uiAppearance.SetStickSkinID(PlayerTeam.Red, PlayerRole.Goalie, SettingsManager.StickSkinIDRedGoalie);
		this.uiAppearance.SetStickShaftTapeID(PlayerTeam.Blue, PlayerRole.Attacker, SettingsManager.StickShaftTapeIDBlueAttacker);
		this.uiAppearance.SetStickShaftTapeID(PlayerTeam.Blue, PlayerRole.Goalie, SettingsManager.StickShaftTapeIDBlueGoalie);
		this.uiAppearance.SetStickShaftTapeID(PlayerTeam.Red, PlayerRole.Attacker, SettingsManager.StickShaftTapeIDRedAttacker);
		this.uiAppearance.SetStickShaftTapeID(PlayerTeam.Red, PlayerRole.Goalie, SettingsManager.StickShaftTapeIDRedGoalie);
		this.uiAppearance.SetStickBladeTapeID(PlayerTeam.Blue, PlayerRole.Attacker, SettingsManager.StickBladeTapeIDBlueAttacker);
		this.uiAppearance.SetStickBladeTapeID(PlayerTeam.Blue, PlayerRole.Goalie, SettingsManager.StickBladeTapeIDBlueGoalie);
		this.uiAppearance.SetStickBladeTapeID(PlayerTeam.Red, PlayerRole.Attacker, SettingsManager.StickBladeTapeIDRedAttacker);
		this.uiAppearance.SetStickBladeTapeID(PlayerTeam.Red, PlayerRole.Goalie, SettingsManager.StickBladeTapeIDRedGoalie);
	}

	// Token: 0x06000D7D RID: 3453 RVA: 0x000408A4 File Offset: 0x0003EAA4
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnTeamChanged", new Action<Dictionary<string, object>>(this.Event_OnTeamChanged));
		EventManager.RemoveEventListener("Event_OnRoleChanged", new Action<Dictionary<string, object>>(this.Event_OnRoleChanged));
		EventManager.RemoveEventListener("Event_OnApplyForBothTeamsChanged", new Action<Dictionary<string, object>>(this.Event_OnApplyForBothTeamsChanged));
		EventManager.RemoveEventListener("Event_OnFlagIDChanged", new Action<Dictionary<string, object>>(this.Event_OnFlagIDChanged));
		EventManager.RemoveEventListener("Event_OnHeadgearIDChanged", new Action<Dictionary<string, object>>(this.Event_OnHeadgearIDChanged));
		EventManager.RemoveEventListener("Event_OnMustacheIDChanged", new Action<Dictionary<string, object>>(this.Event_OnMustacheIDChanged));
		EventManager.RemoveEventListener("Event_OnBeardIDChanged", new Action<Dictionary<string, object>>(this.Event_OnBeardIDChanged));
		EventManager.RemoveEventListener("Event_OnJerseyIDChanged", new Action<Dictionary<string, object>>(this.Event_OnJerseyIDChanged));
		EventManager.RemoveEventListener("Event_OnStickSkinIDChanged", new Action<Dictionary<string, object>>(this.Event_OnStickSkinIDChanged));
		EventManager.RemoveEventListener("Event_OnStickShaftTapeIDChanged", new Action<Dictionary<string, object>>(this.Event_OnStickShaftTapeIDChanged));
		EventManager.RemoveEventListener("Event_OnStickBladeTapeIDChanged", new Action<Dictionary<string, object>>(this.Event_OnStickBladeTapeIDChanged));
		EventManager.RemoveEventListener("Event_OnPlayerDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerDataChanged));
		EventManager.RemoveEventListener("Event_OnAppearanceHide", new Action<Dictionary<string, object>>(this.Event_OnAppearanceHide));
		base.OnDestroy();
	}

	// Token: 0x06000D7E RID: 3454 RVA: 0x000409D8 File Offset: 0x0003EBD8
	private void Event_OnTeamChanged(Dictionary<string, object> message)
	{
		PlayerTeam team = (PlayerTeam)message["value"];
		this.uiAppearance.SetTeam(team);
	}

	// Token: 0x06000D7F RID: 3455 RVA: 0x00040A04 File Offset: 0x0003EC04
	private void Event_OnRoleChanged(Dictionary<string, object> message)
	{
		PlayerRole role = (PlayerRole)message["value"];
		this.uiAppearance.SetRole(role);
	}

	// Token: 0x06000D80 RID: 3456 RVA: 0x00040A30 File Offset: 0x0003EC30
	private void Event_OnApplyForBothTeamsChanged(Dictionary<string, object> message)
	{
		bool applyForBothTeams = (bool)message["value"];
		this.uiAppearance.SetApplyForBothTeams(applyForBothTeams);
	}

	// Token: 0x06000D81 RID: 3457 RVA: 0x00040A5C File Offset: 0x0003EC5C
	private void Event_OnFlagIDChanged(Dictionary<string, object> message)
	{
		int flagID = (int)message["value"];
		this.uiAppearance.SetFlagID(flagID);
	}

	// Token: 0x06000D82 RID: 3458 RVA: 0x00040A88 File Offset: 0x0003EC88
	private void Event_OnHeadgearIDChanged(Dictionary<string, object> message)
	{
		PlayerTeam team = (PlayerTeam)message["team"];
		PlayerRole role = (PlayerRole)message["role"];
		int value = (int)message["value"];
		this.uiAppearance.SetHeadgearID(team, role, value);
	}

	// Token: 0x06000D83 RID: 3459 RVA: 0x00040AD8 File Offset: 0x0003ECD8
	private void Event_OnMustacheIDChanged(Dictionary<string, object> message)
	{
		int mustacheID = (int)message["value"];
		this.uiAppearance.SetMustacheID(mustacheID);
	}

	// Token: 0x06000D84 RID: 3460 RVA: 0x00040B04 File Offset: 0x0003ED04
	private void Event_OnBeardIDChanged(Dictionary<string, object> message)
	{
		int beardID = (int)message["value"];
		this.uiAppearance.SetBeardID(beardID);
	}

	// Token: 0x06000D85 RID: 3461 RVA: 0x00040B30 File Offset: 0x0003ED30
	private void Event_OnJerseyIDChanged(Dictionary<string, object> message)
	{
		PlayerTeam team = (PlayerTeam)message["team"];
		PlayerRole role = (PlayerRole)message["role"];
		int value = (int)message["value"];
		this.uiAppearance.SetJerseyID(team, role, value);
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x00040B80 File Offset: 0x0003ED80
	private void Event_OnStickSkinIDChanged(Dictionary<string, object> message)
	{
		PlayerTeam team = (PlayerTeam)message["team"];
		PlayerRole role = (PlayerRole)message["role"];
		int value = (int)message["value"];
		this.uiAppearance.SetStickSkinID(team, role, value);
	}

	// Token: 0x06000D87 RID: 3463 RVA: 0x00040BD0 File Offset: 0x0003EDD0
	private void Event_OnStickShaftTapeIDChanged(Dictionary<string, object> message)
	{
		PlayerTeam team = (PlayerTeam)message["team"];
		PlayerRole role = (PlayerRole)message["role"];
		int value = (int)message["value"];
		this.uiAppearance.SetStickShaftTapeID(team, role, value);
	}

	// Token: 0x06000D88 RID: 3464 RVA: 0x00040C20 File Offset: 0x0003EE20
	private void Event_OnStickBladeTapeIDChanged(Dictionary<string, object> message)
	{
		PlayerTeam team = (PlayerTeam)message["team"];
		PlayerRole role = (PlayerRole)message["role"];
		int value = (int)message["value"];
		this.uiAppearance.SetStickBladeTapeID(team, role, value);
	}

	// Token: 0x06000D89 RID: 3465 RVA: 0x00040C6E File Offset: 0x0003EE6E
	private void Event_OnPlayerDataChanged(Dictionary<string, object> message)
	{
		if ((PlayerData)message["newPlayerData"] == null)
		{
			return;
		}
		this.uiAppearance.StyleRadioButtonGroups();
	}

	// Token: 0x06000D8A RID: 3466 RVA: 0x00040C8E File Offset: 0x0003EE8E
	private void Event_OnAppearanceHide(Dictionary<string, object> message)
	{
		this.uiAppearance.UpdateRadioButtons();
	}

	// Token: 0x0400089E RID: 2206
	private UIAppearance uiAppearance;
}
