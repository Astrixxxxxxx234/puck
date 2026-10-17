using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	// Token: 0x02000294 RID: 660
	[UxmlElement]
	public class Icon : VisualElement
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x00057E15 File Offset: 0x00056015
		// (set) Token: 0x060013F4 RID: 5108 RVA: 0x00057E1D File Offset: 0x0005601D
		[UxmlAttribute]
		public StyleBackground Texture
		{
			get
			{
				return this.texture;
			}
			set
			{
				if (this.texture == value)
				{
					return;
				}
				this.texture = value;
				this.Update();
			}
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x00057E3B File Offset: 0x0005603B
		public Icon()
		{
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.TrickleDown);
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x00057E56 File Offset: 0x00056056
		private void OnAttachToPanel(AttachToPanelEvent e)
		{
			this.Update();
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x00057E5E File Offset: 0x0005605E
		private void Update()
		{
			base.style.backgroundImage = this.Texture;
		}

		// Token: 0x04000C7D RID: 3197
		private StyleBackground texture;

		// Token: 0x02000295 RID: 661
		[CompilerGenerated]
		[Serializable]
		public new class UxmlSerializedData : VisualElement.UxmlSerializedData
		{
			// Token: 0x060013F8 RID: 5112 RVA: 0x00057E71 File Offset: 0x00056071
			[RegisterUxmlCache]
			[Conditional("UNITY_EDITOR")]
			public new static void Register()
			{
				UxmlDescriptionCache.RegisterType(typeof(Icon.UxmlSerializedData), new UxmlAttributeNames[]
				{
					new UxmlAttributeNames("Texture", "texture", null, Array.Empty<string>())
				}, false);
			}

			// Token: 0x060013F9 RID: 5113 RVA: 0x00057EA5 File Offset: 0x000560A5
			public override object CreateInstance()
			{
				return new Icon();
			}

			// Token: 0x060013FA RID: 5114 RVA: 0x00057EAC File Offset: 0x000560AC
			public override void Deserialize(object obj)
			{
				base.Deserialize(obj);
				Icon icon = (Icon)obj;
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Texture_UxmlAttributeFlags))
				{
					icon.Texture = this.Texture;
				}
			}

			// Token: 0x04000C7E RID: 3198
			[SerializeField]
			private StyleBackground Texture;

			// Token: 0x04000C7F RID: 3199
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Texture_UxmlAttributeFlags;
		}
	}
}
