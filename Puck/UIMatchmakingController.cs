using System;
using System.Collections.Generic;

// Token: 0x020001DE RID: 478
public class UIMatchmakingController : UIViewController<UIMatchmaking>
{
	// Token: 0x06000E9C RID: 3740 RVA: 0x00045010 File Offset: 0x00043210
	public override void Awake()
	{
		base.Awake();
		this.uiMatchmaking = base.GetComponent<UIMatchmaking>();
		EventManager.AddEventListener("Event_OnPlaySelectedPoolsChanged", new Action<Dictionary<string, object>>(this.Event_OnPlaySelectedPoolsChanged));
		EventManager.AddEventListener("Event_OnPlayerGroupDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerGroupDataChanged));
		EventManager.AddEventListener("Event_OnPlayerMatchDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerMatchDataChanged));
		EventManager.AddEventListener("Event_OnConnectionStateChanged", new Action<Dictionary<string, object>>(this.Event_OnConnectionStateChanged));
		EventManager.AddEventListener("Event_OnMatchingTickerStarted", new Action<Dictionary<string, object>>(this.Event_OnMatchingTickerStarted));
		EventManager.AddEventListener("Event_OnMatchingTickerTick", new Action<Dictionary<string, object>>(this.Event_OnMatchingTickerTick));
		EventManager.AddEventListener("Event_OnMatchingTickerStopped", new Action<Dictionary<string, object>>(this.Event_OnMatchingTickerStopped));
		EventManager.AddEventListener("Event_OnMatchJoinTimeoutTickerStarted", new Action<Dictionary<string, object>>(this.Event_OnMatchJoinTimeoutTickerStarted));
		EventManager.AddEventListener("Event_OnMatchJoinTimeoutTickerTick", new Action<Dictionary<string, object>>(this.Event_OnMatchJoinTimeoutTickerTick));
		EventManager.AddEventListener("Event_OnMatchJoinTimeoutTickerStopped", new Action<Dictionary<string, object>>(this.Event_OnMatchJoinTimeoutTickerStopped));
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x0004510C File Offset: 0x0004330C
	private void Start()
	{
		this.uiMatchmaking.SetMatchingVisibility(false);
		this.uiMatchmaking.SetMatchingPhaseText(string.Empty);
		this.uiMatchmaking.SetMatchingTimeVisibility(false);
		this.uiMatchmaking.SetMatchingTimeText(0);
		this.uiMatchmaking.SetMatchingStartMatchmakingButtonVisibility(false);
		this.uiMatchmaking.SetMatchingConnectButtonVisibility(false);
		this.uiMatchmaking.SetMatchingCloseButtonVisibility(false);
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x00045174 File Offset: 0x00043374
	public override void OnDestroy()
	{
		base.OnDestroy();
		EventManager.RemoveEventListener("Event_OnPlaySelectedPoolsChanged", new Action<Dictionary<string, object>>(this.Event_OnPlaySelectedPoolsChanged));
		EventManager.RemoveEventListener("Event_OnPlayerGroupDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerGroupDataChanged));
		EventManager.RemoveEventListener("Event_OnPlayerMatchDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerMatchDataChanged));
		EventManager.RemoveEventListener("Event_OnConnectionStateChanged", new Action<Dictionary<string, object>>(this.Event_OnConnectionStateChanged));
		EventManager.RemoveEventListener("Event_OnMatchingTickerStarted", new Action<Dictionary<string, object>>(this.Event_OnMatchingTickerStarted));
		EventManager.RemoveEventListener("Event_OnMatchingTickerTick", new Action<Dictionary<string, object>>(this.Event_OnMatchingTickerTick));
		EventManager.RemoveEventListener("Event_OnMatchingTickerStopped", new Action<Dictionary<string, object>>(this.Event_OnMatchingTickerStopped));
		EventManager.RemoveEventListener("Event_OnMatchJoinTimeoutTickerStarted", new Action<Dictionary<string, object>>(this.Event_OnMatchJoinTimeoutTickerStarted));
		EventManager.RemoveEventListener("Event_OnMatchJoinTimeoutTickerTick", new Action<Dictionary<string, object>>(this.Event_OnMatchJoinTimeoutTickerTick));
		EventManager.RemoveEventListener("Event_OnMatchJoinTimeoutTickerStopped", new Action<Dictionary<string, object>>(this.Event_OnMatchJoinTimeoutTickerStopped));
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x00045264 File Offset: 0x00043464
	private void UpdateMatching()
	{
		bool groupData = BackendManager.PlayerState.GroupData != null;
		PlayerMatchData matchData = BackendManager.PlayerState.MatchData;
		if (groupData)
		{
			this.uiMatchmaking.SetMatchingVisibility(true);
			this.uiMatchmaking.SetMatchingPhaseText("LOOKING FOR A MATCH...");
			this.uiMatchmaking.SetMatchingStartMatchmakingButtonVisibility(false);
			this.uiMatchmaking.SetMatchingConnectButtonVisibility(false);
			this.uiMatchmaking.SetMatchingCloseButtonVisibility(true);
			return;
		}
		if (matchData != null && !BackendUtils.IsConnectedToMatchEndPoint())
		{
			this.uiMatchmaking.SetMatchingVisibility(true);
			this.uiMatchmaking.SetMatchingStartMatchmakingButtonVisibility(false);
			this.uiMatchmaking.SetMatchingCloseButtonVisibility(false);
			if (matchData.endPoint == null)
			{
				this.uiMatchmaking.SetMatchingPhaseText("MATCH FOUND! DEPLOYING MATCH SERVER...");
				this.uiMatchmaking.SetMatchingConnectButtonVisibility(false);
				return;
			}
			this.uiMatchmaking.SetMatchingPhaseText("MATCH READY!");
			this.uiMatchmaking.SetMatchingConnectButtonVisibility(true);
			return;
		}
		else
		{
			if (this.uiMatchmaking.MatchingSelectedPoolIds.Length != 0)
			{
				this.uiMatchmaking.SetMatchingVisibility(true);
				this.uiMatchmaking.SetMatchingPhaseText("READY TO QUEUE");
				this.uiMatchmaking.SetMatchingStartMatchmakingButtonVisibility(true);
				this.uiMatchmaking.SetMatchingConnectButtonVisibility(false);
				this.uiMatchmaking.SetMatchingCloseButtonVisibility(false);
				return;
			}
			this.uiMatchmaking.SetMatchingVisibility(false);
			this.uiMatchmaking.SetMatchingPhaseText(string.Empty);
			this.uiMatchmaking.SetMatchingStartMatchmakingButtonVisibility(false);
			this.uiMatchmaking.SetMatchingConnectButtonVisibility(false);
			this.uiMatchmaking.SetMatchingCloseButtonVisibility(false);
			return;
		}
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x000453D0 File Offset: 0x000435D0
	private void Event_OnPlaySelectedPoolsChanged(Dictionary<string, object> message)
	{
		string[] matchingSelectedPoolIds = (string[])message["poolIds"];
		this.uiMatchmaking.SetMatchingSelectedPoolIds(matchingSelectedPoolIds);
		this.UpdateMatching();
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x00045400 File Offset: 0x00043600
	private void Event_OnPlayerGroupDataChanged(Dictionary<string, object> message)
	{
		this.UpdateMatching();
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x00045400 File Offset: 0x00043600
	private void Event_OnPlayerMatchDataChanged(Dictionary<string, object> message)
	{
		this.UpdateMatching();
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x00045400 File Offset: 0x00043600
	private void Event_OnConnectionStateChanged(Dictionary<string, object> message)
	{
		this.UpdateMatching();
	}

	// Token: 0x06000EA4 RID: 3748 RVA: 0x00045408 File Offset: 0x00043608
	private void Event_OnMatchingTickerStarted(Dictionary<string, object> message)
	{
		int matchingTimeText = (int)message["startingTick"];
		this.uiMatchmaking.SetMatchingTimeVisibility(true);
		this.uiMatchmaking.SetMatchingTimeText(matchingTimeText);
	}

	// Token: 0x06000EA5 RID: 3749 RVA: 0x00045440 File Offset: 0x00043640
	private void Event_OnMatchingTickerTick(Dictionary<string, object> message)
	{
		int matchingTimeText = (int)message["tick"];
		this.uiMatchmaking.SetMatchingTimeText(matchingTimeText);
	}

	// Token: 0x06000EA6 RID: 3750 RVA: 0x0004546A File Offset: 0x0004366A
	private void Event_OnMatchingTickerStopped(Dictionary<string, object> message)
	{
		this.uiMatchmaking.SetMatchingTimeVisibility(false);
		this.uiMatchmaking.SetMatchingTimeText(0);
	}

	// Token: 0x06000EA7 RID: 3751 RVA: 0x00045484 File Offset: 0x00043684
	private void Event_OnMatchJoinTimeoutTickerStarted(Dictionary<string, object> message)
	{
		int matchingTimeText = (int)message["startingTick"];
		this.uiMatchmaking.SetMatchingTimeVisibility(true);
		this.uiMatchmaking.SetMatchingTimeText(matchingTimeText);
	}

	// Token: 0x06000EA8 RID: 3752 RVA: 0x000454BC File Offset: 0x000436BC
	private void Event_OnMatchJoinTimeoutTickerTick(Dictionary<string, object> message)
	{
		int matchingTimeText = (int)message["tick"];
		this.uiMatchmaking.SetMatchingTimeText(matchingTimeText);
	}

	// Token: 0x06000EA9 RID: 3753 RVA: 0x0004546A File Offset: 0x0004366A
	private void Event_OnMatchJoinTimeoutTickerStopped(Dictionary<string, object> message)
	{
		this.uiMatchmaking.SetMatchingTimeVisibility(false);
		this.uiMatchmaking.SetMatchingTimeText(0);
	}

	// Token: 0x0400094F RID: 2383
	private UIMatchmaking uiMatchmaking;
}
