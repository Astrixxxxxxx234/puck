using System;
using UnityEngine.UIElements;

// Token: 0x020001D5 RID: 469
public class UIGameState : UIView
{
	// Token: 0x06000E3B RID: 3643 RVA: 0x0004393C File Offset: 0x00041B3C
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("GameStateView", null);
		this.gameState = base.View.Query("GameState", null);
		this.blueScoreLabel = this.gameState.Query("BlueScoreLabel", null);
		this.redScoreLabel = this.gameState.Query("RedScoreLabel", null);
		this.timeLabel = this.gameState.Query("TimeLabel", null);
		this.phaseLabel = this.gameState.Query("PhaseLabel", null);
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x0003E32E File Offset: 0x0003C52E
	public override bool Show()
	{
		return SettingsManager.ShowGameUserInterface && base.Show();
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x000439EC File Offset: 0x00041BEC
	public void SetScore(PlayerTeam team, int score)
	{
		if (team == PlayerTeam.Blue)
		{
			this.blueScoreLabel.text = string.Format("{0}", score);
			return;
		}
		if (team == PlayerTeam.Red)
		{
			this.redScoreLabel.text = string.Format("{0}", score);
		}
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x00043A38 File Offset: 0x00041C38
	public void SetTick(int tick)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)tick);
		if (timeSpan.TotalHours < 1.0)
		{
			this.timeLabel.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
			return;
		}
		this.timeLabel.text = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds);
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x00043AC7 File Offset: 0x00041CC7
	public void SetPhase(string text)
	{
		this.phaseLabel.text = text;
	}

	// Token: 0x04000918 RID: 2328
	private VisualElement gameState;

	// Token: 0x04000919 RID: 2329
	private Label blueScoreLabel;

	// Token: 0x0400091A RID: 2330
	private Label redScoreLabel;

	// Token: 0x0400091B RID: 2331
	private Label timeLabel;

	// Token: 0x0400091C RID: 2332
	private Label phaseLabel;
}
