using System;
using System.Collections.Generic;

// Token: 0x02000205 RID: 517
internal class UIScoreboardController : UIViewController<UIScoreboard>
{
	// Token: 0x06001004 RID: 4100 RVA: 0x0004BCE4 File Offset: 0x00049EE4
	public override void Awake()
	{
		base.Awake();
		this.uiScoreboard = base.GetComponent<UIScoreboard>();
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		EventManager.AddEventListener("Event_Everyone_OnPlayerAdded", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerAdded));
		EventManager.AddEventListener("Event_Everyone_OnPlayerRemoved", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerRemoved));
		EventManager.AddEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerUsernameChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerUsernameChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerGoalsChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGoalsChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerAssistsChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerAssistsChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerPingChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPingChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerPositionChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPositionChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerPatreonLevelChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPatreonLevelChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerAdminLevelChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerAdminLevelChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerSteamIdChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSteamIdChanged));
		EventManager.AddEventListener("Event_Everyone_OnServerChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnServerChanged));
	}

	// Token: 0x06001005 RID: 4101 RVA: 0x0004BE14 File Offset: 0x0004A014
	public override void OnDestroy()
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			base.OnDestroy();
			return;
		}
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerAdded", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerAdded));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerRemoved", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerRemoved));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerUsernameChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerUsernameChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerGoalsChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGoalsChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerAssistsChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerAssistsChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerPingChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPingChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerPositionChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPositionChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerPatreonLevelChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPatreonLevelChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerAdminLevelChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerAdminLevelChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerSteamIdChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerSteamIdChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnServerChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnServerChanged));
		base.OnDestroy();
	}

	// Token: 0x06001006 RID: 4102 RVA: 0x0004BF40 File Offset: 0x0004A140
	private void Event_Everyone_OnPlayerAdded(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (player.IsReplay.Value)
		{
			return;
		}
		this.uiScoreboard.AddPlayer(player);
		this.uiScoreboard.StyleServer(NetworkBehaviourSingleton<ServerManager>.Instance.Server.Value, MonoBehaviourSingleton<PlayerManager>.Instance.GetPlayers(false).Count);
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x0004BFA4 File Offset: 0x0004A1A4
	private void Event_Everyone_OnPlayerRemoved(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (player.IsReplay.Value)
		{
			return;
		}
		this.uiScoreboard.RemovePlayer(player);
		this.uiScoreboard.StyleServer(NetworkBehaviourSingleton<ServerManager>.Instance.Server.Value, MonoBehaviourSingleton<PlayerManager>.Instance.GetPlayers(false).Count);
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x0004C008 File Offset: 0x0004A208
	private void Event_Everyone_OnPlayerGameStateChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		this.uiScoreboard.StylePlayer(player);
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x0004C034 File Offset: 0x0004A234
	private void Event_Everyone_OnPlayerUsernameChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		this.uiScoreboard.StylePlayer(player);
	}

	// Token: 0x0600100A RID: 4106 RVA: 0x0004C060 File Offset: 0x0004A260
	private void Event_Everyone_OnPlayerGoalsChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		this.uiScoreboard.StylePlayer(player);
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x0004C08C File Offset: 0x0004A28C
	private void Event_Everyone_OnPlayerAssistsChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		this.uiScoreboard.StylePlayer(player);
	}

	// Token: 0x0600100C RID: 4108 RVA: 0x0004C0B8 File Offset: 0x0004A2B8
	private void Event_Everyone_OnPlayerPingChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		this.uiScoreboard.UpdatePlayerPing(player);
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x0004C0E4 File Offset: 0x0004A2E4
	private void Event_Everyone_OnPlayerPositionChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		this.uiScoreboard.StylePlayer(player);
	}

	// Token: 0x0600100E RID: 4110 RVA: 0x0004C110 File Offset: 0x0004A310
	private void Event_Everyone_OnPlayerPatreonLevelChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		this.uiScoreboard.StylePlayer(player);
	}

	// Token: 0x0600100F RID: 4111 RVA: 0x0004C13C File Offset: 0x0004A33C
	private void Event_Everyone_OnPlayerAdminLevelChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		this.uiScoreboard.StylePlayer(player);
	}

	// Token: 0x06001010 RID: 4112 RVA: 0x0004C168 File Offset: 0x0004A368
	private void Event_Everyone_OnPlayerSteamIdChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		this.uiScoreboard.StylePlayer(player);
	}

	// Token: 0x06001011 RID: 4113 RVA: 0x0004C192 File Offset: 0x0004A392
	private void Event_Everyone_OnServerChanged(Dictionary<string, object> message)
	{
		this.uiScoreboard.StyleServer(NetworkBehaviourSingleton<ServerManager>.Instance.Server.Value, MonoBehaviourSingleton<PlayerManager>.Instance.GetPlayers(false).Count);
	}

	// Token: 0x04000A1F RID: 2591
	private UIScoreboard uiScoreboard;
}
