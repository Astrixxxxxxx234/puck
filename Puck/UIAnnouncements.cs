using System;
using UnityEngine.UIElements;

// Token: 0x020001AB RID: 427
public class UIAnnouncements : UIView
{
	// Token: 0x06000D1B RID: 3355 RVA: 0x0003E278 File Offset: 0x0003C478
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("AnnouncementsView", null);
		this.announcements = base.View.Query("Announcements", null);
		this.score = this.announcements.Query("Score", null);
		this.headerLabel = this.score.Query("HeaderLabel", null);
		this.goalLabel = this.score.Query("GoalLabel", null);
		this.assistLabel = this.score.Query("AssistLabel", null);
		this.HideScore();
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x0003E32E File Offset: 0x0003C52E
	public override bool Show()
	{
		return SettingsManager.ShowGameUserInterface && base.Show();
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x0003E340 File Offset: 0x0003C540
	public void ShowScore(PlayerTeam team, Player goalPlayer, Player assistPlayer, Player secondAssistPlayer)
	{
		this.headerLabel.text = string.Empty;
		this.goalLabel.text = string.Empty;
		this.assistLabel.text = string.Empty;
		this.score.style.display = DisplayStyle.Flex;
		UIUtils.SetTeamClass(this.score, team);
		if (team != PlayerTeam.Blue)
		{
			if (team == PlayerTeam.Red)
			{
				this.headerLabel.text = "RED SCORES!";
			}
		}
		else
		{
			this.headerLabel.text = "BLUE SCORES!";
		}
		if (goalPlayer)
		{
			this.goalLabel.text = string.Format("#{0} {1}", goalPlayer.Number.Value, goalPlayer.Username.Value);
		}
		if (assistPlayer)
		{
			this.assistLabel.text = string.Format("#{0} {1}", assistPlayer.Number.Value, assistPlayer.Username.Value);
		}
		if (secondAssistPlayer)
		{
			Label label = this.assistLabel;
			label.text += string.Format(" & #{0} {1}", secondAssistPlayer.Number.Value, secondAssistPlayer.Username.Value);
		}
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x0003E490 File Offset: 0x0003C690
	public void HideScore()
	{
		this.headerLabel.text = string.Empty;
		this.goalLabel.text = string.Empty;
		this.assistLabel.text = string.Empty;
		this.score.style.display = DisplayStyle.None;
	}

	// Token: 0x0400082C RID: 2092
	private VisualElement announcements;

	// Token: 0x0400082D RID: 2093
	private VisualElement score;

	// Token: 0x0400082E RID: 2094
	private Label headerLabel;

	// Token: 0x0400082F RID: 2095
	private Label goalLabel;

	// Token: 0x04000830 RID: 2096
	private Label assistLabel;
}
