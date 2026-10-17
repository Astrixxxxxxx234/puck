using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x02000203 RID: 515
public class UIScoreboard : UIView
{
	// Token: 0x06000FF7 RID: 4087 RVA: 0x0004B75C File Offset: 0x0004995C
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("ScoreboardView", null);
		this.scoreboard = base.View.Query("Scoreboard", null);
		this.header = this.scoreboard.Query("Header", null);
		this.players = this.scoreboard.Query("Players", null);
		this.nameLabel = this.header.Query("NameLabel", null);
		this.playersLabel = this.header.Query("PlayersLabel", null);
		this.players.Clear();
	}

	// Token: 0x06000FF8 RID: 4088 RVA: 0x0004B818 File Offset: 0x00049A18
	public void AddPlayer(Player player)
	{
		if (this.playerVisualElementMap.ContainsKey(player))
		{
			return;
		}
		VisualElement visualElement = this.playerAsset.Instantiate();
		visualElement.userData = player;
		this.players.Add(visualElement);
		visualElement.Query(null, null).RegisterCallback<ClickEvent, Player>(new EventCallback<ClickEvent, Player>(this.OnPlayerClicked), player, TrickleDown.NoTrickleDown);
		this.playerVisualElementMap.Add(player, visualElement);
		this.StylePlayer(player);
	}

	// Token: 0x06000FF9 RID: 4089 RVA: 0x0004B888 File Offset: 0x00049A88
	public void RemovePlayer(Player player)
	{
		if (!this.playerVisualElementMap.ContainsKey(player))
		{
			return;
		}
		this.playerVisualElementMap[player].Query(null, null).UnregisterCallback<ClickEvent, Player>(new EventCallback<ClickEvent, Player>(this.OnPlayerClicked), TrickleDown.NoTrickleDown);
		this.players.Remove(this.playerVisualElementMap[player]);
		this.playerVisualElementMap.Remove(player);
	}

	// Token: 0x06000FFA RID: 4090 RVA: 0x0004B8F4 File Offset: 0x00049AF4
	public void StylePlayer(Player player)
	{
		if (!this.playerVisualElementMap.ContainsKey(player))
		{
			return;
		}
		VisualElement visualElement = this.playerVisualElementMap[player].Query("Player", null);
		UIUtils.SetTeamClass(visualElement, player.Team);
		visualElement.EnableInClassList("patreon", player.PatreonLevel.Value > 0);
		visualElement.EnableInClassList("moderator", player.AdminLevel.Value == 1);
		visualElement.EnableInClassList("admin", player.AdminLevel.Value == 2);
		visualElement.EnableInClassList("developer", player.AdminLevel.Value == 3);
		Label label = visualElement.Query("PositionLabel", null);
		Label label2 = visualElement.Query("UsernameLabel", null);
		Label label3 = visualElement.Query("GoalsLabel", null);
		Label label4 = visualElement.Query("AssistsLabel", null);
		Label label5 = visualElement.Query("PointsLabel", null);
		TextElement textElement = visualElement.Query("PingLabel", null);
		bool flag = player.Team != PlayerTeam.Blue && player.Team != PlayerTeam.Red;
		label.text = (player.PlayerPosition ? player.PlayerPosition.Name.ToString() : string.Empty);
		label2.text = string.Format("#{0} {1}", player.Number.Value, player.Username.Value);
		label3.text = (flag ? string.Empty : player.Goals.Value.ToString());
		label4.text = (flag ? string.Empty : player.Assists.Value.ToString());
		label5.text = (flag ? string.Empty : (player.Goals.Value + player.Assists.Value).ToString());
		textElement.text = string.Format("{0}ms", player.Ping.Value);
		this.SortPlayers();
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x0004BB1C File Offset: 0x00049D1C
	public void UpdatePlayerPing(Player player)
	{
		VisualElement e;
		if (!this.playerVisualElementMap.TryGetValue(player, out e))
		{
			return;
		}
		e.Query("PingLabel", null).text = string.Format("{0}ms", player.Ping.Value);
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x0004BB6A File Offset: 0x00049D6A
	public void StyleServer(Server server, int playerCount)
	{
		this.nameLabel.text = server.Name.Value;
		this.playersLabel.text = string.Format("{0}/{1}", playerCount, server.MaxPlayers);
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x0004BBAC File Offset: 0x00049DAC
	public void SortPlayers()
	{
		this.players.hierarchy.Sort(delegate(VisualElement a, VisualElement b)
		{
			Player player = (Player)a.userData;
			Player player2 = (Player)b.userData;
			int num = UIScoreboard.<SortPlayers>g__GetTeamOrder|13_1(player.Team);
			int num2 = UIScoreboard.<SortPlayers>g__GetTeamOrder|13_1(player2.Team);
			int num3 = player.Goals.Value + player.Assists.Value;
			int num4 = player2.Goals.Value + player2.Assists.Value;
			if (num != num2)
			{
				return num.CompareTo(num2);
			}
			if (num3 != num4)
			{
				return num4.CompareTo(num3);
			}
			return player.Username.Value.CompareTo(player2.Username.Value);
		});
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x0004BBEB File Offset: 0x00049DEB
	private void OnPlayerClicked(ClickEvent clickEvent, Player player)
	{
		EventManager.TriggerEvent("Event_OnScoreboardClickPlayer", new Dictionary<string, object>
		{
			{
				"player",
				player
			}
		});
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x0004BC1B File Offset: 0x00049E1B
	[CompilerGenerated]
	internal static int <SortPlayers>g__GetTeamOrder|13_1(PlayerTeam team)
	{
		if (team == PlayerTeam.Blue)
		{
			return 0;
		}
		if (team != PlayerTeam.Red)
		{
			return 2;
		}
		return 1;
	}

	// Token: 0x04000A16 RID: 2582
	[Header("References")]
	public VisualTreeAsset playerAsset;

	// Token: 0x04000A17 RID: 2583
	private VisualElement scoreboard;

	// Token: 0x04000A18 RID: 2584
	private VisualElement header;

	// Token: 0x04000A19 RID: 2585
	private VisualElement players;

	// Token: 0x04000A1A RID: 2586
	private Label nameLabel;

	// Token: 0x04000A1B RID: 2587
	private Label playersLabel;

	// Token: 0x04000A1C RID: 2588
	private Dictionary<Player, VisualElement> playerVisualElementMap = new Dictionary<Player, VisualElement>();
}
