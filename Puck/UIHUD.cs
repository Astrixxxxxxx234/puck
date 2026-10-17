using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x020001D7 RID: 471
public class UIHUD : UIView
{
	// Token: 0x06000E49 RID: 3657 RVA: 0x00043CC8 File Offset: 0x00041EC8
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("HUDView", null);
		this.staminaProgressBar = base.View.Query("StaminaProgressBar", null);
		this.teamColorBar = base.View.Query("TeamColorBar", null);
		this.teamIndicator = base.View.Query("TeamIndicator", null);
		this.teamIndicatorLabel = this.teamIndicator.Query("TeamIndicatorLabel", null);
		this.teamIndicatorPosition = this.teamIndicator.Query("TeamIndicatorPosition", null);
		this.speed = base.View.Query("Speed", null);
		this.speedLabel = this.speed.Query("SpeedLabel", null);
		this.unitsLabel = this.speed.Query("UnitsLabel", null);
		this.voiceTransmitIndicator = base.View.Query("VoiceTransmitIndicator", null);
		this.voiceTransmitFill = this.voiceTransmitIndicator.Query("VoiceTransmitFill", null);
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x00043E04 File Offset: 0x00042004
	public void SetVoiceTransmitting(bool value)
	{
		this.voiceTransmitIndicator.style.display = (value ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06000E4B RID: 3659 RVA: 0x00043E22 File Offset: 0x00042022
	public void SetVoiceLevel(float level)
	{
		this.voiceTransmitFill.style.height = Length.Percent(Mathf.Clamp01(level) * 100f);
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x00043E4A File Offset: 0x0004204A
	public void SetStamina(float value)
	{
		this.staminaProgressBar.EnableInClassList("warning", value < 0.25f);
		this.staminaProgressBar.value = value;
	}

	// Token: 0x06000E4D RID: 3661 RVA: 0x00043E70 File Offset: 0x00042070
	public void SetSpeed(float value)
	{
		float num = (float)Math.Round((double)((SettingsManager.Units == Units.Metric) ? Utils.GameUnitsToMetric(value) : Utils.GameUnitsToImperial(value)), 1);
		this.speedLabel.text = num.ToString("F1", CultureInfo.InvariantCulture);
	}

	// Token: 0x06000E4E RID: 3662 RVA: 0x00043EB7 File Offset: 0x000420B7
	public void SetUnits(string units)
	{
		this.unitsLabel.text = units;
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x00043EC8 File Offset: 0x000420C8
	public void SetTeam(PlayerTeam team)
	{
		UIUtils.SetTeamClass(this.teamColorBar, team);
		UIUtils.SetTeamClass(this.teamIndicator, team);
		Label label = this.teamIndicatorLabel;
		string text;
		if (team != PlayerTeam.Blue)
		{
			if (team != PlayerTeam.Red)
			{
				text = string.Empty;
			}
			else
			{
				text = "RED";
			}
		}
		else
		{
			text = "BLUE";
		}
		label.text = text;
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x00043F1B File Offset: 0x0004211B
	public void SetPosition(string positionName)
	{
		this.teamIndicatorPosition.text = positionName;
	}

	// Token: 0x0400091E RID: 2334
	private ProgressBar staminaProgressBar;

	// Token: 0x0400091F RID: 2335
	private VisualElement speed;

	// Token: 0x04000920 RID: 2336
	private Label speedLabel;

	// Token: 0x04000921 RID: 2337
	private Label unitsLabel;

	// Token: 0x04000922 RID: 2338
	private VisualElement teamColorBar;

	// Token: 0x04000923 RID: 2339
	private VisualElement teamIndicator;

	// Token: 0x04000924 RID: 2340
	private Label teamIndicatorLabel;

	// Token: 0x04000925 RID: 2341
	private Label teamIndicatorPosition;

	// Token: 0x04000926 RID: 2342
	private VisualElement voiceTransmitIndicator;

	// Token: 0x04000927 RID: 2343
	private VisualElement voiceTransmitFill;
}
