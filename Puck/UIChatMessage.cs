using System;
using DG.Tweening;
using UnityEngine.UIElements;

// Token: 0x020001C4 RID: 452
public class UIChatMessage
{
	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00040CA3 File Offset: 0x0003EEA3
	private double expiryTimestamp
	{
		get
		{
			return this.ChatMessage.Timestamp + (double)(this.ExpiryTime * 1000f);
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000D8D RID: 3469 RVA: 0x00040CBE File Offset: 0x0003EEBE
	private float expiresInTime
	{
		get
		{
			return (float)(this.expiryTimestamp - Utils.GetTimestamp()) / 1000f;
		}
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000D8E RID: 3470 RVA: 0x00040CD3 File Offset: 0x0003EED3
	private bool isExpired
	{
		get
		{
			return Utils.GetTimestamp() > this.expiryTimestamp;
		}
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x00040CE4 File Offset: 0x0003EEE4
	public UIChatMessage(ChatMessage chatMessage, VisualElement visualElement, float expiryTime = 5f)
	{
		this.ChatMessage = chatMessage;
		this.VisualElement = visualElement;
		this.ExpiryTime = expiryTime;
		this.label = this.VisualElement.Query(null, null);
		this.Focus();
		this.StartExpiryTween();
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x00040D30 File Offset: 0x0003EF30
	public void Focus()
	{
		Tween tween = this.blurTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.label.EnableInClassList("blurred", false);
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x00040D55 File Offset: 0x0003EF55
	public void Blur()
	{
		if (!this.isExpired)
		{
			this.StartExpiryTween();
			return;
		}
		Tween tween = this.blurTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.label.EnableInClassList("blurred", true);
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x00040D89 File Offset: 0x0003EF89
	public void Dispose()
	{
		Tween tween = this.blurTween;
		if (tween == null)
		{
			return;
		}
		tween.Kill(false);
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x00040D9C File Offset: 0x0003EF9C
	private void StartExpiryTween()
	{
		Tween tween = this.blurTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.blurTween = DOVirtual.DelayedCall(this.expiresInTime, delegate
		{
			this.Blur();
		}, true);
	}

	// Token: 0x0400089F RID: 2207
	public ChatMessage ChatMessage;

	// Token: 0x040008A0 RID: 2208
	public VisualElement VisualElement;

	// Token: 0x040008A1 RID: 2209
	public float ExpiryTime;

	// Token: 0x040008A2 RID: 2210
	private Label label;

	// Token: 0x040008A3 RID: 2211
	private Tween blurTween;
}
