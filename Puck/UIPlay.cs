using System;
using System.Collections.Generic;
using UI;
using UnityEngine.UIElements;

// Token: 0x020001EF RID: 495
public class UIPlay : UIView
{
	// Token: 0x06000F6A RID: 3946 RVA: 0x00049594 File Offset: 0x00047794
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("PlayView", null);
		this.play = base.View.Query("Play", null);
		this.closeIconButton = base.View.Query("CloseIconButtonContainer", null).First().Query(null, null);
		this.closeIconButton.clicked += this.OnClickClose;
		this.threeVsThreeTile = this.play.Query("ThreeVsThreePlayTile", null).First().Query(null, null);
		this.threeVsThreeToggle = this.threeVsThreeTile.Query(null, null);
		this.threeVsThreeToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnPoolSelectionChanged));
		this.fiveVsFiveTile = this.play.Query("FiveVsFivePlayTile", null).First().Query(null, null);
		this.fiveVsFiveToggle = this.fiveVsFiveTile.Query(null, null);
		this.fiveVsFiveToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnPoolSelectionChanged));
		this.practiceButton = this.play.Query("PracticePlayTile", null).First().Query(null, null);
		this.practiceButton.clicked += this.OnClickPractice;
		this.serverBrowserButton = this.play.Query("ServerBrowserPlayTile", null).First().Query(null, null);
		this.serverBrowserButton.clicked += this.OnClickServerBrowser;
		this.statistics = this.play.Query("Statistics", null);
		this.playersLabel = this.statistics.Query("PlayersLabel", null);
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x00049788 File Offset: 0x00047988
	public override bool Show()
	{
		bool flag = base.Show();
		if (flag)
		{
			this.RefreshPoolSelection();
		}
		return flag;
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x00049799 File Offset: 0x00047999
	public override bool Hide()
	{
		bool flag = base.Hide();
		if (flag)
		{
			this.RefreshPoolSelection();
		}
		return flag;
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x000497AA File Offset: 0x000479AA
	public void SetPoolsAvailable(bool isMatchmakingAvailable, bool threeVsThreeFitsParty, bool fiveVsFiveFitsParty)
	{
		UIPlay.SetPoolAvailable(this.threeVsThreeTile, this.threeVsThreeToggle, isMatchmakingAvailable, threeVsThreeFitsParty);
		UIPlay.SetPoolAvailable(this.fiveVsFiveTile, this.fiveVsFiveToggle, isMatchmakingAvailable, fiveVsFiveFitsParty);
		this.RefreshPoolSelection();
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x000497D8 File Offset: 0x000479D8
	private static void SetPoolAvailable(PlayTile tile, Toggle toggle, bool isMatchmakingAvailable, bool fitsParty)
	{
		if (!fitsParty)
		{
			toggle.SetValueWithoutNotify(false);
		}
		tile.SetEnabled(isMatchmakingAvailable && fitsParty);
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x000497ED File Offset: 0x000479ED
	public void SetThreeVsThreePoolDescription(string description)
	{
		this.threeVsThreeTile.Description = description;
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x000497FB File Offset: 0x000479FB
	public void SetFiveVsFivePoolDescription(string description)
	{
		this.fiveVsFiveTile.Description = description;
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x00049809 File Offset: 0x00047A09
	private bool IsPoolSelected(PlayTile tile, Toggle toggle)
	{
		return base.IsVisible && tile.enabledSelf && toggle.value;
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x00049824 File Offset: 0x00047A24
	private string[] GetSelectedPoolIds()
	{
		List<string> list = new List<string>();
		if (this.IsPoolSelected(this.threeVsThreeTile, this.threeVsThreeToggle))
		{
			list.Add("3v3");
		}
		if (this.IsPoolSelected(this.fiveVsFiveTile, this.fiveVsFiveToggle))
		{
			list.Add("5v5");
		}
		return list.ToArray();
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x0004987C File Offset: 0x00047A7C
	private void RefreshPoolSelection()
	{
		this.threeVsThreeTile.EnableInClassList("selected", this.IsPoolSelected(this.threeVsThreeTile, this.threeVsThreeToggle));
		this.fiveVsFiveTile.EnableInClassList("selected", this.IsPoolSelected(this.fiveVsFiveTile, this.fiveVsFiveToggle));
		EventManager.TriggerEvent("Event_OnPlaySelectedPoolsChanged", new Dictionary<string, object>
		{
			{
				"poolIds",
				this.GetSelectedPoolIds()
			}
		});
	}

	// Token: 0x06000F74 RID: 3956 RVA: 0x000498ED File Offset: 0x00047AED
	public void SetStatistics(int players)
	{
		this.playersLabel.text = string.Format("PLAYERS ONLINE: {0}", players);
	}

	// Token: 0x06000F75 RID: 3957 RVA: 0x0004990A File Offset: 0x00047B0A
	private void OnClickClose()
	{
		EventManager.TriggerEvent("Event_OnPlayClickClose", null);
	}

	// Token: 0x06000F76 RID: 3958 RVA: 0x00049917 File Offset: 0x00047B17
	private void OnPoolSelectionChanged(ChangeEvent<bool> changeEvent)
	{
		this.RefreshPoolSelection();
	}

	// Token: 0x06000F77 RID: 3959 RVA: 0x0004991F File Offset: 0x00047B1F
	private void OnClickPractice()
	{
		EventManager.TriggerEvent("Event_OnPlayClickPractice", null);
	}

	// Token: 0x06000F78 RID: 3960 RVA: 0x0004992C File Offset: 0x00047B2C
	private void OnClickServerBrowser()
	{
		EventManager.TriggerEvent("Event_OnPlayClickServerBrowser", null);
	}

	// Token: 0x040009CB RID: 2507
	private VisualElement play;

	// Token: 0x040009CC RID: 2508
	private IconButton closeIconButton;

	// Token: 0x040009CD RID: 2509
	private PlayTile threeVsThreeTile;

	// Token: 0x040009CE RID: 2510
	private PlayTile fiveVsFiveTile;

	// Token: 0x040009CF RID: 2511
	private Toggle threeVsThreeToggle;

	// Token: 0x040009D0 RID: 2512
	private Toggle fiveVsFiveToggle;

	// Token: 0x040009D1 RID: 2513
	private Button practiceButton;

	// Token: 0x040009D2 RID: 2514
	private Button serverBrowserButton;

	// Token: 0x040009D3 RID: 2515
	private VisualElement statistics;

	// Token: 0x040009D4 RID: 2516
	private Label playersLabel;
}
