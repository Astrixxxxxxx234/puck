using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	// Token: 0x02000296 RID: 662
	[UxmlElement]
	public class IconButton : Button
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x00057EE0 File Offset: 0x000560E0
		// (set) Token: 0x060013FD RID: 5117 RVA: 0x00057EE8 File Offset: 0x000560E8
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

		// Token: 0x060013FE RID: 5118 RVA: 0x00057F06 File Offset: 0x00056106
		public IconButton()
		{
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.TrickleDown);
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x00057F21 File Offset: 0x00056121
		private void OnAttachToPanel(AttachToPanelEvent e)
		{
			this.icon = this.Query(null, null);
			this.Update();
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x00057F3C File Offset: 0x0005613C
		private void Update()
		{
			if (this.icon != null)
			{
				this.icon.Texture = this.Texture;
			}
		}

		// Token: 0x04000C80 RID: 3200
		private StyleBackground texture;

		// Token: 0x04000C81 RID: 3201
		private Icon icon;

		// Token: 0x02000297 RID: 663
		[CompilerGenerated]
		[Serializable]
		public new class UxmlSerializedData : Button.UxmlSerializedData
		{
			// Token: 0x06001401 RID: 5121 RVA: 0x00057F57 File Offset: 0x00056157
			[RegisterUxmlCache]
			[Conditional("UNITY_EDITOR")]
			public new static void Register()
			{
				UxmlDescriptionCache.RegisterType(typeof(IconButton.UxmlSerializedData), new UxmlAttributeNames[]
				{
					new UxmlAttributeNames("Texture", "texture", null, Array.Empty<string>())
				}, false);
			}

			// Token: 0x06001402 RID: 5122 RVA: 0x00057F8B File Offset: 0x0005618B
			public override object CreateInstance()
			{
				return new IconButton();
			}

			// Token: 0x06001403 RID: 5123 RVA: 0x00057F94 File Offset: 0x00056194
			public override void Deserialize(object obj)
			{
				base.Deserialize(obj);
				IconButton iconButton = (IconButton)obj;
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Texture_UxmlAttributeFlags))
				{
					iconButton.Texture = this.Texture;
				}
			}

			// Token: 0x04000C82 RID: 3202
			[SerializeField]
			private StyleBackground Texture;

			// Token: 0x04000C83 RID: 3203
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Texture_UxmlAttributeFlags;
		}
	}
}
