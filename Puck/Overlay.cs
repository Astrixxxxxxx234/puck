using System;
using DG.Tweening;
using UI;
using UnityEngine.UIElements;

// Token: 0x020001E9 RID: 489
public class Overlay
{
	// Token: 0x06000F2E RID: 3886 RVA: 0x000482B8 File Offset: 0x000464B8
	public Overlay(VisualElement visualElement, string identifier, bool requiresSpinner = false)
	{
		this.VisualElement = visualElement;
		this.Identifier = identifier;
		this.VisualElement.style.display = DisplayStyle.None;
		this.VisualElement.style.opacity = 0f;
		this.spinner = this.VisualElement.Query(null, null);
		this.spinner.style.display = (requiresSpinner ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x00048340 File Offset: 0x00046540
	public void Show()
	{
		Tween tween = this.fadeOutTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		if (this.fadeInTween != null)
		{
			this.fadeInTween.Kill(false);
		}
		if (this.autoHideTween != null)
		{
			this.autoHideTween.Kill(false);
		}
		Action showing = this.Showing;
		if (showing != null)
		{
			showing();
		}
		if (this.FadeIn)
		{
			this.fadeInTween = DOVirtual.Float(this.VisualElement.style.opacity.value, 1f, this.FadeTime, delegate(float value)
			{
				this.VisualElement.style.opacity = value;
			}).OnStart(delegate
			{
				this.VisualElement.style.display = DisplayStyle.Flex;
				Action shown2 = this.Shown;
				if (shown2 == null)
				{
					return;
				}
				shown2();
			});
			if (this.AutoHide)
			{
				this.autoHideTween = DOVirtual.DelayedCall(this.FadeTime + this.HideTimeout, delegate
				{
					this.Hide();
				}, true);
				return;
			}
		}
		else
		{
			this.VisualElement.style.display = DisplayStyle.Flex;
			this.VisualElement.style.opacity = 1f;
			Action shown = this.Shown;
			if (shown != null)
			{
				shown();
			}
			if (this.AutoHide)
			{
				this.autoHideTween = DOVirtual.DelayedCall(this.HideTimeout, delegate
				{
					this.Hide();
				}, true);
			}
		}
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x00048484 File Offset: 0x00046684
	public void Hide()
	{
		Tween tween = this.fadeInTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		if (this.fadeOutTween != null)
		{
			this.fadeOutTween.Kill(false);
		}
		if (this.autoHideTween != null)
		{
			this.autoHideTween.Kill(false);
		}
		Action hiding = this.Hiding;
		if (hiding != null)
		{
			hiding();
		}
		if (this.FadeOut)
		{
			this.fadeOutTween = DOVirtual.Float(this.VisualElement.style.opacity.value, 0f, this.FadeTime, delegate(float value)
			{
				this.VisualElement.style.opacity = value;
			}).OnComplete(delegate
			{
				this.VisualElement.style.display = DisplayStyle.None;
				Action hidden2 = this.Hidden;
				if (hidden2 == null)
				{
					return;
				}
				hidden2();
			});
			return;
		}
		this.VisualElement.style.display = DisplayStyle.None;
		this.VisualElement.style.opacity = 0f;
		Action hidden = this.Hidden;
		if (hidden == null)
		{
			return;
		}
		hidden();
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x00048570 File Offset: 0x00046770
	public void Dispose()
	{
		Tween tween = this.fadeInTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		Tween tween2 = this.fadeOutTween;
		if (tween2 != null)
		{
			tween2.Kill(false);
		}
		Tween tween3 = this.autoHideTween;
		if (tween3 == null)
		{
			return;
		}
		tween3.Kill(false);
	}

	// Token: 0x0400099F RID: 2463
	public VisualElement VisualElement;

	// Token: 0x040009A0 RID: 2464
	public string Identifier;

	// Token: 0x040009A1 RID: 2465
	public bool RequiresSpinner;

	// Token: 0x040009A2 RID: 2466
	public bool FadeIn;

	// Token: 0x040009A3 RID: 2467
	public bool FadeOut;

	// Token: 0x040009A4 RID: 2468
	public float FadeTime;

	// Token: 0x040009A5 RID: 2469
	public bool AutoHide;

	// Token: 0x040009A6 RID: 2470
	public float HideTimeout;

	// Token: 0x040009A7 RID: 2471
	public Action Showing;

	// Token: 0x040009A8 RID: 2472
	public Action Shown;

	// Token: 0x040009A9 RID: 2473
	public Action Hiding;

	// Token: 0x040009AA RID: 2474
	public Action Hidden;

	// Token: 0x040009AB RID: 2475
	private Tween fadeInTween;

	// Token: 0x040009AC RID: 2476
	private Tween fadeOutTween;

	// Token: 0x040009AD RID: 2477
	private Tween autoHideTween;

	// Token: 0x040009AE RID: 2478
	private Spinner spinner;
}
