using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	// Token: 0x02000290 RID: 656
	[UxmlElement]
	public class CurvedSlider : Slider
	{
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060013C8 RID: 5064 RVA: 0x0005759A File Offset: 0x0005579A
		// (set) Token: 0x060013C9 RID: 5065 RVA: 0x000575A2 File Offset: 0x000557A2
		[UxmlAttribute]
		public int KneeValue
		{
			get
			{
				return this.kneeValue;
			}
			set
			{
				this.kneeValue = value;
				this.UpdateDisplay();
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x000575B1 File Offset: 0x000557B1
		// (set) Token: 0x060013CB RID: 5067 RVA: 0x000575B9 File Offset: 0x000557B9
		[UxmlAttribute]
		public float KneePosition
		{
			get
			{
				return this.kneePosition;
			}
			set
			{
				this.kneePosition = Mathf.Clamp(value, 0.01f, 0.99f);
				this.UpdateDisplay();
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060013CC RID: 5068 RVA: 0x000575D7 File Offset: 0x000557D7
		// (set) Token: 0x060013CD RID: 5069 RVA: 0x000575DF File Offset: 0x000557DF
		[UxmlAttribute]
		public int Ceiling
		{
			get
			{
				return this.ceiling;
			}
			set
			{
				this.ceiling = value;
				this.UpdateDisplay();
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060013CE RID: 5070 RVA: 0x000575EE File Offset: 0x000557EE
		// (set) Token: 0x060013CF RID: 5071 RVA: 0x000575F6 File Offset: 0x000557F6
		[UxmlAttribute]
		public string ValueLabel
		{
			get
			{
				return this.valueLabel;
			}
			set
			{
				this.valueLabel = value;
				this.UpdateDisplay();
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060013D0 RID: 5072 RVA: 0x00057605 File Offset: 0x00055805
		// (set) Token: 0x060013D1 RID: 5073 RVA: 0x0005760D File Offset: 0x0005580D
		[UxmlAttribute]
		public string CeilingLabel
		{
			get
			{
				return this.ceilingLabel;
			}
			set
			{
				this.ceilingLabel = value;
				this.UpdateDisplay();
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060013D2 RID: 5074 RVA: 0x0005761C File Offset: 0x0005581C
		// (set) Token: 0x060013D3 RID: 5075 RVA: 0x00057624 File Offset: 0x00055824
		[UxmlAttribute]
		public string Unit
		{
			get
			{
				return this.unit;
			}
			set
			{
				this.unit = value;
				this.UpdateDisplay();
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060013D4 RID: 5076 RVA: 0x00057633 File Offset: 0x00055833
		// (set) Token: 0x060013D5 RID: 5077 RVA: 0x0005763B File Offset: 0x0005583B
		[UxmlAttribute]
		public bool TrackGradient
		{
			get
			{
				return this.trackGradient;
			}
			set
			{
				this.trackGradient = value;
				this.ApplyTrackGradient();
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060013D6 RID: 5078 RVA: 0x0005764A File Offset: 0x0005584A
		// (set) Token: 0x060013D7 RID: 5079 RVA: 0x00057658 File Offset: 0x00055858
		public int MappedValue
		{
			get
			{
				return this.SliderToValue(this.value);
			}
			set
			{
				this.value = this.ValueToSlider(value);
			}
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x00057668 File Offset: 0x00055868
		public CurvedSlider()
		{
			base.lowValue = 0f;
			base.highValue = 1f;
			this.showInputField = false;
			this.inputField = new TextField
			{
				isDelayed = true
			};
			this.inputField.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnInputFieldChanged));
			this.Q(null, "unity-base-field__input").Add(this.inputField);
			this.RegisterValueChangedCallback(new EventCallback<ChangeEvent<float>>(this.OnValueChanged));
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x0005772B File Offset: 0x0005592B
		public void SetMappedValueWithoutNotify(int mappedValue)
		{
			this.SetValueWithoutNotify(this.ValueToSlider(mappedValue));
			this.UpdateDisplay();
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x00057740 File Offset: 0x00055940
		private int SliderToValue(float sliderValue)
		{
			if (sliderValue >= 1f)
			{
				if (!string.IsNullOrEmpty(this.ceilingLabel))
				{
					return int.MaxValue;
				}
				return this.ceiling;
			}
			else
			{
				if (sliderValue <= this.kneePosition)
				{
					return Mathf.RoundToInt((float)this.kneeValue * (sliderValue / this.kneePosition));
				}
				float num = (sliderValue - this.kneePosition) / (1f - this.kneePosition);
				return Mathf.RoundToInt((float)this.kneeValue + (float)(this.ceiling - this.kneeValue) * num);
			}
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x000577C4 File Offset: 0x000559C4
		private float ValueToSlider(int mappedValue)
		{
			if (mappedValue >= this.ceiling)
			{
				return 1f;
			}
			if (mappedValue <= this.kneeValue)
			{
				return this.kneePosition * ((float)mappedValue / (float)this.kneeValue);
			}
			float num = (float)(mappedValue - this.kneeValue) / (float)(this.ceiling - this.kneeValue);
			return this.kneePosition + (1f - this.kneePosition) * num;
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0005782C File Offset: 0x00055A2C
		private void ApplyTrackGradient()
		{
			VisualElement visualElement = this.Q(null, "unity-base-slider__drag-container");
			if (visualElement == null)
			{
				return;
			}
			if (!this.trackGradient)
			{
				visualElement.style.backgroundImage = StyleKeyword.Null;
				return;
			}
			if (this.gradientTexture == null)
			{
				this.gradientTexture = CurvedSlider.BuildGradientTexture();
			}
			visualElement.style.backgroundImage = Background.FromTexture2D(this.gradientTexture);
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x00057894 File Offset: 0x00055A94
		private static Texture2D BuildGradientTexture()
		{
			Color a = new Color(0.33f, 0.5f, 0.33f);
			Color color = new Color(0.55f, 0.5f, 0.28f);
			Color b = new Color(0.55f, 0.3f, 0.3f);
			Texture2D texture2D = new Texture2D(256, 1, TextureFormat.RGBA32, false)
			{
				name = "CurvedSliderTrackGradient",
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear,
				hideFlags = HideFlags.HideAndDontSave
			};
			for (int i = 0; i < 256; i++)
			{
				float num = (float)i / 255f;
				Color color2 = (num < 0.5f) ? Color.Lerp(a, color, num * 2f) : Color.Lerp(color, b, (num - 0.5f) * 2f);
				texture2D.SetPixel(i, 0, color2);
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x00057974 File Offset: 0x00055B74
		private void UpdateDisplay()
		{
			base.label = this.valueLabel;
			int num = this.SliderToValue(this.value);
			string valueWithoutNotify = (num == int.MaxValue) ? this.ceilingLabel : string.Format("{0}{1}", num, this.unit);
			this.inputField.SetValueWithoutNotify(valueWithoutNotify);
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x000579CD File Offset: 0x00055BCD
		private void OnValueChanged(ChangeEvent<float> changeEvent)
		{
			this.UpdateDisplay();
			Action<int> mappedValueChanged = this.MappedValueChanged;
			if (mappedValueChanged == null)
			{
				return;
			}
			mappedValueChanged(this.SliderToValue(changeEvent.newValue));
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x000579F4 File Offset: 0x00055BF4
		private void OnInputFieldChanged(ChangeEvent<string> changeEvent)
		{
			string text = changeEvent.newValue.Trim();
			int value;
			if (text.Length == 0 || (!string.IsNullOrEmpty(this.ceilingLabel) && string.Equals(text, this.ceilingLabel, StringComparison.OrdinalIgnoreCase)))
			{
				this.MappedValue = this.ceiling;
			}
			else if (int.TryParse(new string(text.Where(new Func<char, bool>(char.IsDigit)).ToArray<char>()), out value))
			{
				this.MappedValue = Mathf.Clamp(value, 0, this.ceiling);
			}
			this.UpdateDisplay();
		}

		// Token: 0x04000C5C RID: 3164
		private readonly TextField inputField;

		// Token: 0x04000C5D RID: 3165
		private Texture2D gradientTexture;

		// Token: 0x04000C5E RID: 3166
		private int kneeValue = 150;

		// Token: 0x04000C5F RID: 3167
		private float kneePosition = 0.65f;

		// Token: 0x04000C60 RID: 3168
		private int ceiling = 100;

		// Token: 0x04000C61 RID: 3169
		private string valueLabel = "";

		// Token: 0x04000C62 RID: 3170
		private string ceilingLabel = "";

		// Token: 0x04000C63 RID: 3171
		private string unit = "";

		// Token: 0x04000C64 RID: 3172
		private bool trackGradient;

		// Token: 0x04000C65 RID: 3173
		public Action<int> MappedValueChanged;

		// Token: 0x02000291 RID: 657
		[CompilerGenerated]
		[Serializable]
		public new class UxmlSerializedData : Slider.UxmlSerializedData
		{
			// Token: 0x060013E1 RID: 5089 RVA: 0x00057A80 File Offset: 0x00055C80
			[RegisterUxmlCache]
			[Conditional("UNITY_EDITOR")]
			public new static void Register()
			{
				UxmlDescriptionCache.RegisterType(typeof(CurvedSlider.UxmlSerializedData), new UxmlAttributeNames[]
				{
					new UxmlAttributeNames("KneeValue", "knee-value", null, Array.Empty<string>()),
					new UxmlAttributeNames("KneePosition", "knee-position", null, Array.Empty<string>()),
					new UxmlAttributeNames("Ceiling", "ceiling", null, Array.Empty<string>()),
					new UxmlAttributeNames("ValueLabel", "value-label", null, Array.Empty<string>()),
					new UxmlAttributeNames("CeilingLabel", "ceiling-label", null, Array.Empty<string>()),
					new UxmlAttributeNames("Unit", "unit", null, Array.Empty<string>()),
					new UxmlAttributeNames("TrackGradient", "track-gradient", null, Array.Empty<string>())
				}, false);
			}

			// Token: 0x060013E2 RID: 5090 RVA: 0x00057B67 File Offset: 0x00055D67
			public override object CreateInstance()
			{
				return new CurvedSlider();
			}

			// Token: 0x060013E3 RID: 5091 RVA: 0x00057B70 File Offset: 0x00055D70
			public override void Deserialize(object obj)
			{
				base.Deserialize(obj);
				CurvedSlider curvedSlider = (CurvedSlider)obj;
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.KneeValue_UxmlAttributeFlags))
				{
					curvedSlider.KneeValue = this.KneeValue;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.KneePosition_UxmlAttributeFlags))
				{
					curvedSlider.KneePosition = this.KneePosition;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Ceiling_UxmlAttributeFlags))
				{
					curvedSlider.Ceiling = this.Ceiling;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.ValueLabel_UxmlAttributeFlags))
				{
					curvedSlider.ValueLabel = this.ValueLabel;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.CeilingLabel_UxmlAttributeFlags))
				{
					curvedSlider.CeilingLabel = this.CeilingLabel;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Unit_UxmlAttributeFlags))
				{
					curvedSlider.Unit = this.Unit;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.TrackGradient_UxmlAttributeFlags))
				{
					curvedSlider.TrackGradient = this.TrackGradient;
				}
			}

			// Token: 0x04000C66 RID: 3174
			[SerializeField]
			private int KneeValue;

			// Token: 0x04000C67 RID: 3175
			[SerializeField]
			private float KneePosition;

			// Token: 0x04000C68 RID: 3176
			[SerializeField]
			private int Ceiling;

			// Token: 0x04000C69 RID: 3177
			[SerializeField]
			private string ValueLabel;

			// Token: 0x04000C6A RID: 3178
			[SerializeField]
			private string CeilingLabel;

			// Token: 0x04000C6B RID: 3179
			[SerializeField]
			private string Unit;

			// Token: 0x04000C6C RID: 3180
			[SerializeField]
			private bool TrackGradient;

			// Token: 0x04000C6D RID: 3181
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags KneeValue_UxmlAttributeFlags;

			// Token: 0x04000C6E RID: 3182
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags KneePosition_UxmlAttributeFlags;

			// Token: 0x04000C6F RID: 3183
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Ceiling_UxmlAttributeFlags;

			// Token: 0x04000C70 RID: 3184
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags ValueLabel_UxmlAttributeFlags;

			// Token: 0x04000C71 RID: 3185
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags CeilingLabel_UxmlAttributeFlags;

			// Token: 0x04000C72 RID: 3186
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Unit_UxmlAttributeFlags;

			// Token: 0x04000C73 RID: 3187
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags TrackGradient_UxmlAttributeFlags;
		}
	}
}
