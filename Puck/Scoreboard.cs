using System;
using TMPro;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class Scoreboard : MonoBehaviour
{
	// Token: 0x060003C9 RID: 969 RVA: 0x000168E4 File Offset: 0x00014AE4
	public void TurnOn()
	{
		this.minutesText.gameObject.SetActive(true);
		this.secondsText.gameObject.SetActive(true);
		this.periodText.gameObject.SetActive(true);
		this.blueScoreText.gameObject.SetActive(true);
		this.redScoreText.gameObject.SetActive(true);
	}

	// Token: 0x060003CA RID: 970 RVA: 0x00016948 File Offset: 0x00014B48
	public void TurnOff()
	{
		this.minutesText.gameObject.SetActive(false);
		this.secondsText.gameObject.SetActive(false);
		this.periodText.gameObject.SetActive(false);
		this.blueScoreText.gameObject.SetActive(false);
		this.redScoreText.gameObject.SetActive(false);
	}

	// Token: 0x060003CB RID: 971 RVA: 0x000169AC File Offset: 0x00014BAC
	public void SetTick(int tick)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)tick);
		this.minutesText.text = timeSpan.Minutes.ToString("D2");
		this.secondsText.text = timeSpan.Seconds.ToString("D2");
	}

	// Token: 0x060003CC RID: 972 RVA: 0x000169FF File Offset: 0x00014BFF
	public void SetPeriod(int period)
	{
		this.periodText.text = period.ToString();
	}

	// Token: 0x060003CD RID: 973 RVA: 0x00016A13 File Offset: 0x00014C13
	public void SetBlueScore(int score)
	{
		this.blueScoreText.text = score.ToString();
	}

	// Token: 0x060003CE RID: 974 RVA: 0x00016A27 File Offset: 0x00014C27
	public void SetRedScore(int score)
	{
		this.redScoreText.text = score.ToString();
	}

	// Token: 0x040002D2 RID: 722
	[Header("References")]
	[SerializeField]
	private TMP_Text minutesText;

	// Token: 0x040002D3 RID: 723
	[SerializeField]
	private TMP_Text secondsText;

	// Token: 0x040002D4 RID: 724
	[SerializeField]
	private TMP_Text periodText;

	// Token: 0x040002D5 RID: 725
	[SerializeField]
	private TMP_Text blueScoreText;

	// Token: 0x040002D6 RID: 726
	[SerializeField]
	private TMP_Text redScoreText;
}
