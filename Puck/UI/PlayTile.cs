using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	// Token: 0x020002A2 RID: 674
	[UxmlElement]
	public class PlayTile : VisualElement
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x0005913B File Offset: 0x0005733B
		// (set) Token: 0x06001458 RID: 5208 RVA: 0x00059143 File Offset: 0x00057343
		[UxmlAttribute]
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (this.text == value)
				{
					return;
				}
				this.text = value;
				this.Update();
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x00059161 File Offset: 0x00057361
		// (set) Token: 0x0600145A RID: 5210 RVA: 0x00059169 File Offset: 0x00057369
		[UxmlAttribute]
		public string Subtitle
		{
			get
			{
				return this.subtitle;
			}
			set
			{
				if (this.subtitle == value)
				{
					return;
				}
				this.subtitle = value;
				this.Update();
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x00059187 File Offset: 0x00057387
		// (set) Token: 0x0600145C RID: 5212 RVA: 0x0005918F File Offset: 0x0005738F
		[UxmlAttribute]
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				if (this.description == value)
				{
					return;
				}
				this.description = value;
				this.Update();
			}
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x000591AD File Offset: 0x000573AD
		public PlayTile()
		{
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.TrickleDown);
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x000591C8 File Offset: 0x000573C8
		private void OnAttachToPanel(AttachToPanelEvent e)
		{
			this.textLabel = this.Query("TextLabel", null);
			this.subtitleLabel = this.Query("SubtitleLabel", null);
			this.descriptionLabel = this.Query("DescriptionLabel", null);
			this.Update();
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x00059220 File Offset: 0x00057420
		private void Update()
		{
			if (this.textLabel != null)
			{
				this.textLabel.text = this.Text;
			}
			if (this.subtitleLabel != null)
			{
				this.subtitleLabel.text = this.Subtitle;
			}
			if (this.descriptionLabel != null)
			{
				this.descriptionLabel.text = this.Description;
			}
		}

		// Token: 0x04000CBD RID: 3261
		private string text;

		// Token: 0x04000CBE RID: 3262
		private string subtitle;

		// Token: 0x04000CBF RID: 3263
		private string description;

		// Token: 0x04000CC0 RID: 3264
		private Label textLabel;

		// Token: 0x04000CC1 RID: 3265
		private Label subtitleLabel;

		// Token: 0x04000CC2 RID: 3266
		private Label descriptionLabel;

		// Token: 0x020002A3 RID: 675
		[CompilerGenerated]
		[Serializable]
		public new class UxmlSerializedData : VisualElement.UxmlSerializedData
		{
			// Token: 0x06001460 RID: 5216 RVA: 0x00059278 File Offset: 0x00057478
			[RegisterUxmlCache]
			[Conditional("UNITY_EDITOR")]
			public new static void Register()
			{
				UxmlDescriptionCache.RegisterType(typeof(PlayTile.UxmlSerializedData), new UxmlAttributeNames[]
				{
					new UxmlAttributeNames("Text", "text", null, Array.Empty<string>()),
					new UxmlAttributeNames("Subtitle", "subtitle", null, Array.Empty<string>()),
					new UxmlAttributeNames("Description", "description", null, Array.Empty<string>())
				}, false);
			}

			// Token: 0x06001461 RID: 5217 RVA: 0x000592EF File Offset: 0x000574EF
			public override object CreateInstance()
			{
				return new PlayTile();
			}

			// Token: 0x06001462 RID: 5218 RVA: 0x000592F8 File Offset: 0x000574F8
			public override void Deserialize(object obj)
			{
				base.Deserialize(obj);
				PlayTile playTile = (PlayTile)obj;
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Text_UxmlAttributeFlags))
				{
					playTile.Text = this.Text;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Subtitle_UxmlAttributeFlags))
				{
					playTile.Subtitle = this.Subtitle;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Description_UxmlAttributeFlags))
				{
					playTile.Description = this.Description;
				}
			}

			// Token: 0x04000CC3 RID: 3267
			[SerializeField]
			private string Text;

			// Token: 0x04000CC4 RID: 3268
			[SerializeField]
			private string Subtitle;

			// Token: 0x04000CC5 RID: 3269
			[SerializeField]
			private string Description;

			// Token: 0x04000CC6 RID: 3270
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Text_UxmlAttributeFlags;

			// Token: 0x04000CC7 RID: 3271
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Subtitle_UxmlAttributeFlags;

			// Token: 0x04000CC8 RID: 3272
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Description_UxmlAttributeFlags;
		}
	}
}
