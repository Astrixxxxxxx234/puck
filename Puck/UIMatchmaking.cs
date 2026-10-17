using System;
using System.Collections.Generic;
using UI;
using UnityEngine.UIElements;

// Token: 0x020001DD RID: 477
public class UIMatchmaking : UIView
{
	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000E8D RID: 3725 RVA: 0x00044D40 File Offset: 0x00042F40
	// (set) Token: 0x06000E8E RID: 3726 RVA: 0x00044D48 File Offset: 0x00042F48
	public string[] MatchingSelectedPoolIds { get; private set; } = new string[0];

	// Token: 0x06000E8F RID: 3727 RVA: 0x00044D54 File Offset: 0x00042F54
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("MatchmakingView", null);
		this.matching = base.View.Query("Matching", null);
		this.matchingPhaseLabel = this.matching.Query("PhaseLabel", null);
		this.matchingTimeLabel = this.matching.Query("TimeLabel", null);
		this.matchingCloseIconButtonContainer = this.matching.Query("CloseIconButtonContainer", null);
		this.matchingCloseIconButton = this.matchingCloseIconButtonContainer.Query("IconButton", null);
		this.matchingCloseIconButton.clicked += this.OnClickMatchingClose;
		this.matchingStartMatchmakingButton = base.View.Query("StartMatchmakingButton", null);
		this.matchingStartMatchmakingButton.clicked += this.OnClickMatchingStartMatchmaking;
		this.matchingConnectButton = this.matching.Query("ConnectButton", null);
		this.matchingConnectButton.clicked += this.OnClickMatchingConnect;
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x00044E81 File Offset: 0x00043081
	public void SetMatchingVisibility(bool isVisible)
	{
		this.matching.style.display = (isVisible ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06000E91 RID: 3729 RVA: 0x00044E9F File Offset: 0x0004309F
	public void SetMatchingPhaseText(string text)
	{
		this.matchingPhaseLabel.text = text;
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x00044EAD File Offset: 0x000430AD
	public void SetMatchingTimeVisibility(bool isVisible)
	{
		this.matchingTimeLabel.style.display = (isVisible ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x00044ECC File Offset: 0x000430CC
	public void SetMatchingTimeText(int seconds)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)seconds);
		if (timeSpan.TotalHours < 1.0)
		{
			this.matchingTimeLabel.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
			return;
		}
		this.matchingTimeLabel.text = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds);
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x00044F5B File Offset: 0x0004315B
	public void SetMatchingSelectedPoolIds(string[] poolIds)
	{
		this.MatchingSelectedPoolIds = poolIds;
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x00044F64 File Offset: 0x00043164
	public void SetMatchingStartMatchmakingButtonVisibility(bool isVisible)
	{
		this.matchingStartMatchmakingButton.style.display = (isVisible ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x00044F82 File Offset: 0x00043182
	public void SetMatchingConnectButtonVisibility(bool isVisible)
	{
		this.matchingConnectButton.style.display = (isVisible ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06000E97 RID: 3735 RVA: 0x00044FA0 File Offset: 0x000431A0
	public void SetMatchingCloseButtonVisibility(bool isVisible)
	{
		this.matchingCloseIconButtonContainer.style.display = (isVisible ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x00044FBE File Offset: 0x000431BE
	private void OnClickMatchingClose()
	{
		EventManager.TriggerEvent("Event_OnMatchmakingMatchingClickClose", null);
	}

	// Token: 0x06000E99 RID: 3737 RVA: 0x00044FCB File Offset: 0x000431CB
	private void OnClickMatchingStartMatchmaking()
	{
		EventManager.TriggerEvent("Event_OnMatchmakingMatchingClickStartMatchmaking", new Dictionary<string, object>
		{
			{
				"poolIds",
				this.MatchingSelectedPoolIds
			}
		});
	}

	// Token: 0x06000E9A RID: 3738 RVA: 0x00044FED File Offset: 0x000431ED
	private void OnClickMatchingConnect()
	{
		EventManager.TriggerEvent("Event_OnMatchmakingMatchingClickConnect", null);
	}

	// Token: 0x04000947 RID: 2375
	private VisualElement matching;

	// Token: 0x04000948 RID: 2376
	private Label matchingPhaseLabel;

	// Token: 0x04000949 RID: 2377
	private Label matchingTimeLabel;

	// Token: 0x0400094A RID: 2378
	private Button matchingStartMatchmakingButton;

	// Token: 0x0400094B RID: 2379
	private Button matchingConnectButton;

	// Token: 0x0400094C RID: 2380
	private TemplateContainer matchingCloseIconButtonContainer;

	// Token: 0x0400094D RID: 2381
	private IconButton matchingCloseIconButton;
}
