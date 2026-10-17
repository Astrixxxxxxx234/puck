using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	// Token: 0x0200029C RID: 668
	[UxmlElement]
	public class Mmr : VisualElement
	{
		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x00058570 File Offset: 0x00056770
		// (set) Token: 0x06001425 RID: 5157 RVA: 0x00058578 File Offset: 0x00056778
		public int? CurrentValue
		{
			get
			{
				return this.currentValue;
			}
			set
			{
				int? num = this.currentValue;
				int? num2 = value;
				if (num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null))
				{
					return;
				}
				this.currentValue = value;
				this.Update();
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x000585BF File Offset: 0x000567BF
		// (set) Token: 0x06001427 RID: 5159 RVA: 0x000585C8 File Offset: 0x000567C8
		public int? TargetValue
		{
			get
			{
				return this.targetValue;
			}
			set
			{
				int? num = this.targetValue;
				int? num2 = value;
				if (num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null))
				{
					return;
				}
				this.targetValue = value;
				this.StartCurrentValueTransition();
			}
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0005860F File Offset: 0x0005680F
		public Mmr()
		{
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.TrickleDown);
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0005862A File Offset: 0x0005682A
		private void OnAttachToPanel(AttachToPanelEvent e)
		{
			this.valueLabel = this.Query("ValueLabel", null);
			this.remainderLabel = this.Query("RemainderLabel", null);
			this.Update();
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00058660 File Offset: 0x00056860
		private static Color HexColor(string hex)
		{
			Color result;
			ColorUtility.TryParseHtmlString(hex, out result);
			return result;
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x00058678 File Offset: 0x00056878
		private void StartCurrentValueTransition()
		{
			if (this.TargetValue == null)
			{
				return;
			}
			if (this.CurrentValue == null)
			{
				this.CurrentValue = this.TargetValue;
				return;
			}
			Tween tween = this.transitionTween;
			if (tween != null)
			{
				tween.Kill(false);
			}
			this.transitionTween = DOVirtual.Int(this.CurrentValue.Value, this.TargetValue.Value, 3f, delegate(int value)
			{
				this.CurrentValue = new int?(value);
			}).SetEase(Ease.OutCubic);
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00058704 File Offset: 0x00056904
		private void Update()
		{
			if (this.CurrentValue == null)
			{
				base.style.display = DisplayStyle.None;
				return;
			}
			base.style.display = DisplayStyle.Flex;
			IStyle style = base.style;
			int? num = this.CurrentValue;
			Color v;
			if (num != null)
			{
				int valueOrDefault = num.GetValueOrDefault();
				if (valueOrDefault < 1500)
				{
					if (valueOrDefault < 1000)
					{
						v = Mmr.HexColor("#444751");
						goto IL_E3;
					}
					if (valueOrDefault >= 1250)
					{
						v = Mmr.HexColor("#112061");
						goto IL_E3;
					}
					v = Mmr.HexColor("#2b425a");
					goto IL_E3;
				}
				else if (valueOrDefault < 2000)
				{
					if (valueOrDefault >= 1750)
					{
						v = Mmr.HexColor("#5a005e");
						goto IL_E3;
					}
					v = Mmr.HexColor("#481a60");
					goto IL_E3;
				}
				else if (valueOrDefault < 2250)
				{
					v = Mmr.HexColor("#610809");
					goto IL_E3;
				}
			}
			v = Mmr.HexColor("#625000");
			IL_E3:
			style.unityBackgroundImageTintColor = v;
			if (this.valueLabel != null)
			{
				this.valueLabel.text = this.CurrentValue.ToString();
				IStyle style2 = this.valueLabel.style;
				num = this.CurrentValue;
				if (num != null)
				{
					int valueOrDefault = num.GetValueOrDefault();
					if (valueOrDefault < 1500)
					{
						if (valueOrDefault < 1000)
						{
							v = Mmr.HexColor("#cbd2e2");
							goto IL_1CD;
						}
						if (valueOrDefault >= 1250)
						{
							v = Mmr.HexColor("#4364ff");
							goto IL_1CD;
						}
						v = Mmr.HexColor("#81b6eb");
						goto IL_1CD;
					}
					else if (valueOrDefault < 2000)
					{
						if (valueOrDefault >= 1750)
						{
							v = Mmr.HexColor("#ea02fb");
							goto IL_1CD;
						}
						v = Mmr.HexColor("#c16bff");
						goto IL_1CD;
					}
					else if (valueOrDefault < 2250)
					{
						v = Mmr.HexColor("#f93034");
						goto IL_1CD;
					}
				}
				v = Mmr.HexColor("#f5dc0c");
				IL_1CD:
				style2.color = v;
			}
			if (this.remainderLabel != null)
			{
				num = this.TargetValue;
				int? num2 = this.CurrentValue;
				bool flag = num.GetValueOrDefault() > num2.GetValueOrDefault() & (num != null & num2 != null);
				num2 = this.CurrentValue;
				num = this.TargetValue;
				bool flag2 = !(num2.GetValueOrDefault() == num.GetValueOrDefault() & num2 != null == (num != null));
				this.remainderLabel.style.display = (flag2 ? DisplayStyle.Flex : DisplayStyle.None);
				this.remainderLabel.text = (flag ? string.Format("+{0}", this.TargetValue - this.CurrentValue) : string.Format("{0}", this.TargetValue - this.CurrentValue));
			}
		}

		// Token: 0x04000C99 RID: 3225
		private int? currentValue;

		// Token: 0x04000C9A RID: 3226
		private int? targetValue;

		// Token: 0x04000C9B RID: 3227
		private Tween transitionTween;

		// Token: 0x04000C9C RID: 3228
		private Label valueLabel;

		// Token: 0x04000C9D RID: 3229
		private Label remainderLabel;

		// Token: 0x0200029D RID: 669
		[CompilerGenerated]
		[Serializable]
		public new class UxmlSerializedData : VisualElement.UxmlSerializedData
		{
			// Token: 0x0600142E RID: 5166 RVA: 0x00058A32 File Offset: 0x00056C32
			public override object CreateInstance()
			{
				return new Mmr();
			}
		}
	}
}
