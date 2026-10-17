using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

// Token: 0x02000216 RID: 534
public class UITeamSelect : UIView
{
	// Token: 0x060010F9 RID: 4345 RVA: 0x00050A38 File Offset: 0x0004EC38
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("TeamSelectView", null);
		this.teamSelect = base.View.Query("TeamSelect", null);
		this.blueButton = this.teamSelect.Query("BlueButton", null);
		this.blueButton.clicked += this.OnClickTeamBlue;
		this.blueCountLabel = this.blueButton.Query("CountLabel", null);
		this.redButton = this.teamSelect.Query("RedButton", null);
		this.redButton.clicked += this.OnClickTeamRed;
		this.redCountLabel = this.redButton.Query("CountLabel", null);
		this.spectatorButton = this.teamSelect.Query("SpectatorButton", null);
		this.spectatorButton.clicked += this.OnClickTeamSpectator;
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x00050B49 File Offset: 0x0004ED49
	public override bool Show()
	{
		bool flag = base.Show();
		if (flag)
		{
			EventManager.TriggerEvent("Event_OnTeamSelectShow", null);
		}
		return flag;
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x00050B5F File Offset: 0x0004ED5F
	public void StyleTeamCounts(int blueCount, int redCount)
	{
		this.blueCountLabel.text = this.FormatPlayerCount(blueCount);
		this.redCountLabel.text = this.FormatPlayerCount(redCount);
	}

	// Token: 0x060010FC RID: 4348 RVA: 0x00050B85 File Offset: 0x0004ED85
	private string FormatPlayerCount(int count)
	{
		if (count != 1)
		{
			return string.Format("{0} PLAYERS", count);
		}
		return "1 PLAYER";
	}

	// Token: 0x060010FD RID: 4349 RVA: 0x00050BA1 File Offset: 0x0004EDA1
	private void OnClickTeamBlue()
	{
		EventManager.TriggerEvent("Event_OnTeamSelectClickTeam", new Dictionary<string, object>
		{
			{
				"team",
				PlayerTeam.Blue
			}
		});
	}

	// Token: 0x060010FE RID: 4350 RVA: 0x00050BC3 File Offset: 0x0004EDC3
	private void OnClickTeamRed()
	{
		EventManager.TriggerEvent("Event_OnTeamSelectClickTeam", new Dictionary<string, object>
		{
			{
				"team",
				PlayerTeam.Red
			}
		});
	}

	// Token: 0x060010FF RID: 4351 RVA: 0x00050BE5 File Offset: 0x0004EDE5
	private void OnClickTeamSpectator()
	{
		EventManager.TriggerEvent("Event_OnTeamSelectClickTeam", new Dictionary<string, object>
		{
			{
				"team",
				PlayerTeam.Spectator
			}
		});
	}

	// Token: 0x04000AA7 RID: 2727
	private VisualElement teamSelect;

	// Token: 0x04000AA8 RID: 2728
	private Button blueButton;

	// Token: 0x04000AA9 RID: 2729
	private Button redButton;

	// Token: 0x04000AAA RID: 2730
	private Button spectatorButton;

	// Token: 0x04000AAB RID: 2731
	private Label blueCountLabel;

	// Token: 0x04000AAC RID: 2732
	private Label redCountLabel;
}
