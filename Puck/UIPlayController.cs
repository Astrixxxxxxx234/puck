using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Humanizer;

// Token: 0x020001F0 RID: 496
public class UIPlayController : UIViewController<UIPlay>
{
	// Token: 0x06000F7A RID: 3962 RVA: 0x0004993C File Offset: 0x00047B3C
	public override void Awake()
	{
		base.Awake();
		this.uiPlay = base.GetComponent<UIPlay>();
		EventManager.AddEventListener("Event_OnPlayerDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerDataChanged));
		EventManager.AddEventListener("Event_OnPlayerPartyDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerPartyDataChanged));
		EventManager.AddEventListener("Event_OnPlayerGroupDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerGroupDataChanged));
		EventManager.AddEventListener("Event_OnPlayerMatchDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerMatchDataChanged));
		EventManager.AddEventListener("Event_OnPlayerStatisticsChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerStatisticsChanged));
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x000499C9 File Offset: 0x00047BC9
	private void Start()
	{
		this.UpdateMatchmakingPoolAvailability();
	}

	// Token: 0x06000F7C RID: 3964 RVA: 0x000499D4 File Offset: 0x00047BD4
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnPlayerDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerDataChanged));
		EventManager.RemoveEventListener("Event_OnPlayerPartyDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerPartyDataChanged));
		EventManager.RemoveEventListener("Event_OnPlayerGroupDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerGroupDataChanged));
		EventManager.RemoveEventListener("Event_OnPlayerMatchDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerMatchDataChanged));
		EventManager.RemoveEventListener("Event_OnPlayerStatisticsChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerStatisticsChanged));
		base.OnDestroy();
	}

	// Token: 0x06000F7D RID: 3965 RVA: 0x00049A58 File Offset: 0x00047C58
	private void UpdateMatchmakingPoolAvailability()
	{
		PlayerData playerData = BackendManager.PlayerState.PlayerData;
		PlayerGroupData groupData = BackendManager.PlayerState.GroupData;
		PlayerMatchData matchData = BackendManager.PlayerState.MatchData;
		PlayerPartyData partyData = BackendManager.PlayerState.PartyData;
		bool isMatchmakingAvailable = playerData != null && (partyData == null || (partyData != null && partyData.ownerSteamId == playerData.steamId)) && groupData == null && matchData == null;
		this.uiPlay.SetPoolsAvailable(isMatchmakingAvailable, this.DoesPartyFitPool(3), this.DoesPartyFitPool(5));
	}

	// Token: 0x06000F7E RID: 3966 RVA: 0x00049AD4 File Offset: 0x00047CD4
	private bool DoesPartyFitPool(int teamSize)
	{
		PlayerPartyData partyData = BackendManager.PlayerState.PartyData;
		return ((partyData != null) ? partyData.memberSteamIds.Length : 1) <= teamSize;
	}

	// Token: 0x06000F7F RID: 3967 RVA: 0x00049B00 File Offset: 0x00047D00
	private void Event_OnPlayerStatisticsChanged(Dictionary<string, object> message)
	{
		PlayerStatistics playerStatistics = (PlayerStatistics)message["newPlayerStatistics"];
		this.uiPlay.SetThreeVsThreePoolDescription(this.GetPoolDescription(playerStatistics, "3v3"));
		this.uiPlay.SetFiveVsFivePoolDescription(this.GetPoolDescription(playerStatistics, "5v5"));
		this.uiPlay.SetStatistics(playerStatistics.playerManager.playerCount);
	}

	// Token: 0x06000F80 RID: 3968 RVA: 0x00049B64 File Offset: 0x00047D64
	private string GetPoolDescription(PlayerStatistics playerStatistics, string poolId)
	{
		PoolStatistics poolStatistics = playerStatistics.matchmakingManager.pools.FirstOrDefault((PoolStatistics pool) => pool.id == poolId);
		if (poolStatistics == null)
		{
			return string.Empty;
		}
		return string.Format("IN QUEUE: {0}<br>EST. MATCHING TIME: {1}", poolStatistics.groupPlayerCount, this.GetEstimatedMatchingTime(poolStatistics));
	}

	// Token: 0x06000F81 RID: 3969 RVA: 0x00049BC0 File Offset: 0x00047DC0
	private string GetEstimatedMatchingTime(PoolStatistics poolStatistics)
	{
		if (poolStatistics.averageGroupLifetime == null)
		{
			return "NO DATA";
		}
		return TimeSpan.FromMilliseconds(poolStatistics.averageGroupLifetime.Value).Humanize(2, CultureInfo.InvariantCulture, TimeUnit.Week, TimeUnit.Second, ", ", false);
	}

	// Token: 0x06000F82 RID: 3970 RVA: 0x000499C9 File Offset: 0x00047BC9
	private void Event_OnPlayerDataChanged(Dictionary<string, object> message)
	{
		this.UpdateMatchmakingPoolAvailability();
	}

	// Token: 0x06000F83 RID: 3971 RVA: 0x000499C9 File Offset: 0x00047BC9
	private void Event_OnPlayerPartyDataChanged(Dictionary<string, object> message)
	{
		this.UpdateMatchmakingPoolAvailability();
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x000499C9 File Offset: 0x00047BC9
	private void Event_OnPlayerGroupDataChanged(Dictionary<string, object> message)
	{
		this.UpdateMatchmakingPoolAvailability();
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x000499C9 File Offset: 0x00047BC9
	private void Event_OnPlayerMatchDataChanged(Dictionary<string, object> message)
	{
		this.UpdateMatchmakingPoolAvailability();
	}

	// Token: 0x040009D5 RID: 2517
	private UIPlay uiPlay;
}
