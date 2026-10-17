using System;
using System.Collections.Generic;

// Token: 0x02000217 RID: 535
public class UITeamSelectController : UIViewController<UITeamSelect>
{
	// Token: 0x06001101 RID: 4353 RVA: 0x00050C08 File Offset: 0x0004EE08
	public override void Awake()
	{
		base.Awake();
		this.uiTeamSelect = base.GetComponent<UITeamSelect>();
		EventManager.AddEventListener("Event_OnTeamSelectShow", new Action<Dictionary<string, object>>(this.Event_OnTeamSelectShow));
		EventManager.AddEventListener("Event_Everyone_OnPlayerAdded", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerAdded));
		EventManager.AddEventListener("Event_Everyone_OnPlayerRemoved", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerRemoved));
		EventManager.AddEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
	}

	// Token: 0x06001102 RID: 4354 RVA: 0x00050C80 File Offset: 0x0004EE80
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnTeamSelectShow", new Action<Dictionary<string, object>>(this.Event_OnTeamSelectShow));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerAdded", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerAdded));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerRemoved", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerRemoved));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		base.OnDestroy();
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x00050CEB File Offset: 0x0004EEEB
	private void StyleTeamCounts()
	{
		this.uiTeamSelect.StyleTeamCounts(MonoBehaviourSingleton<PlayerManager>.Instance.GetPlayersByTeam(PlayerTeam.Blue, false).Count, MonoBehaviourSingleton<PlayerManager>.Instance.GetPlayersByTeam(PlayerTeam.Red, false).Count);
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x00050D1A File Offset: 0x0004EF1A
	private void Event_OnTeamSelectShow(Dictionary<string, object> message)
	{
		this.StyleTeamCounts();
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x00050D22 File Offset: 0x0004EF22
	private void Event_Everyone_OnPlayerAdded(Dictionary<string, object> message)
	{
		if (((Player)message["player"]).IsReplay.Value)
		{
			return;
		}
		this.StyleTeamCounts();
	}

	// Token: 0x06001106 RID: 4358 RVA: 0x00050D22 File Offset: 0x0004EF22
	private void Event_Everyone_OnPlayerRemoved(Dictionary<string, object> message)
	{
		if (((Player)message["player"]).IsReplay.Value)
		{
			return;
		}
		this.StyleTeamCounts();
	}

	// Token: 0x06001107 RID: 4359 RVA: 0x00050D1A File Offset: 0x0004EF1A
	private void Event_Everyone_OnPlayerGameStateChanged(Dictionary<string, object> message)
	{
		this.StyleTeamCounts();
	}

	// Token: 0x04000AAD RID: 2733
	private UITeamSelect uiTeamSelect;
}
