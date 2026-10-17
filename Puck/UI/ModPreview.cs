using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	// Token: 0x020002A0 RID: 672
	[UxmlElement]
	public class ModPreview : VisualElement
	{
		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x00058DA9 File Offset: 0x00056FA9
		// (set) Token: 0x06001445 RID: 5189 RVA: 0x00058DB1 File Offset: 0x00056FB1
		[UxmlAttribute]
		public bool IsStatisticsVisible
		{
			get
			{
				return this.isStatisticsVisible;
			}
			set
			{
				if (this.isStatisticsVisible == value)
				{
					return;
				}
				this.isStatisticsVisible = value;
				this.Update();
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x00058DCA File Offset: 0x00056FCA
		// (set) Token: 0x06001447 RID: 5191 RVA: 0x00058DD2 File Offset: 0x00056FD2
		[UxmlAttribute]
		public int Subscriptions
		{
			get
			{
				return this.subscriptions;
			}
			set
			{
				if (this.subscriptions == value)
				{
					return;
				}
				this.subscriptions = value;
				this.Update();
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x00058DEB File Offset: 0x00056FEB
		// (set) Token: 0x06001449 RID: 5193 RVA: 0x00058DF3 File Offset: 0x00056FF3
		[UxmlAttribute]
		public int Upvotes
		{
			get
			{
				return this.upvotes;
			}
			set
			{
				if (this.upvotes == value)
				{
					return;
				}
				this.upvotes = value;
				this.Update();
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600144A RID: 5194 RVA: 0x00058E0C File Offset: 0x0005700C
		// (set) Token: 0x0600144B RID: 5195 RVA: 0x00058E14 File Offset: 0x00057014
		[UxmlAttribute]
		public int Downvotes
		{
			get
			{
				return this.downvotes;
			}
			set
			{
				if (this.downvotes == value)
				{
					return;
				}
				this.downvotes = value;
				this.Update();
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x00058E2D File Offset: 0x0005702D
		public Link Link
		{
			get
			{
				return this.link;
			}
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00058E35 File Offset: 0x00057035
		public ModPreview()
		{
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.NoTrickleDown);
			base.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnDetachFromPanel), TrickleDown.NoTrickleDown);
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x00058E6C File Offset: 0x0005706C
		private void Update()
		{
			if (this.statistics != null)
			{
				this.statistics.style.display = (this.IsStatisticsVisible ? DisplayStyle.Flex : DisplayStyle.None);
			}
			if (this.subscriptionsLabel != null)
			{
				this.subscriptionsLabel.text = this.Subscriptions.ToString();
			}
			if (this.upvotesLabel != null)
			{
				this.upvotesLabel.text = this.Upvotes.ToString();
			}
			if (this.downvotesLabel != null)
			{
				this.downvotesLabel.text = this.Downvotes.ToString();
			}
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x00058F08 File Offset: 0x00057108
		private void OnAttachToPanel(AttachToPanelEvent e)
		{
			this.link = this.Query("Link", null);
			this.statistics = this.Query("Statistics", null);
			this.subscriptionsLabel = this.statistics.Query("Subscriptions", null).First().Query(null, null);
			this.upvotesLabel = this.statistics.Query("Upvotes", null).First().Query(null, null);
			this.downvotesLabel = this.statistics.Query("Downvotes", null).First().Query(null, null);
			this.Update();
			Utils.WhenAllActions(delegate
			{
				Action ready = this.Ready;
				if (ready == null)
				{
					return;
				}
				ready();
			}, new Action<Action>[]
			{
				delegate(Action a)
				{
					Link link = this.link;
					link.Ready = (Action)Delegate.Combine(link.Ready, a);
				}
			});
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x000023BA File Offset: 0x000005BA
		private void OnDetachFromPanel(DetachFromPanelEvent e)
		{
		}

		// Token: 0x04000CAB RID: 3243
		private bool isStatisticsVisible = true;

		// Token: 0x04000CAC RID: 3244
		private int subscriptions;

		// Token: 0x04000CAD RID: 3245
		private int upvotes;

		// Token: 0x04000CAE RID: 3246
		private int downvotes;

		// Token: 0x04000CAF RID: 3247
		public Action Ready;

		// Token: 0x04000CB0 RID: 3248
		private Link link;

		// Token: 0x04000CB1 RID: 3249
		private VisualElement statistics;

		// Token: 0x04000CB2 RID: 3250
		private Label subscriptionsLabel;

		// Token: 0x04000CB3 RID: 3251
		private Label upvotesLabel;

		// Token: 0x04000CB4 RID: 3252
		private Label downvotesLabel;

		// Token: 0x020002A1 RID: 673
		[CompilerGenerated]
		[Serializable]
		public new class UxmlSerializedData : VisualElement.UxmlSerializedData
		{
			// Token: 0x06001453 RID: 5203 RVA: 0x00059020 File Offset: 0x00057220
			[RegisterUxmlCache]
			[Conditional("UNITY_EDITOR")]
			public new static void Register()
			{
				UxmlDescriptionCache.RegisterType(typeof(ModPreview.UxmlSerializedData), new UxmlAttributeNames[]
				{
					new UxmlAttributeNames("IsStatisticsVisible", "is-statistics-visible", null, Array.Empty<string>()),
					new UxmlAttributeNames("Subscriptions", "subscriptions", null, Array.Empty<string>()),
					new UxmlAttributeNames("Upvotes", "upvotes", null, Array.Empty<string>()),
					new UxmlAttributeNames("Downvotes", "downvotes", null, Array.Empty<string>())
				}, false);
			}

			// Token: 0x06001454 RID: 5204 RVA: 0x000590B3 File Offset: 0x000572B3
			public override object CreateInstance()
			{
				return new ModPreview();
			}

			// Token: 0x06001455 RID: 5205 RVA: 0x000590BC File Offset: 0x000572BC
			public override void Deserialize(object obj)
			{
				base.Deserialize(obj);
				ModPreview modPreview = (ModPreview)obj;
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.IsStatisticsVisible_UxmlAttributeFlags))
				{
					modPreview.IsStatisticsVisible = this.IsStatisticsVisible;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Subscriptions_UxmlAttributeFlags))
				{
					modPreview.Subscriptions = this.Subscriptions;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Upvotes_UxmlAttributeFlags))
				{
					modPreview.Upvotes = this.Upvotes;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Downvotes_UxmlAttributeFlags))
				{
					modPreview.Downvotes = this.Downvotes;
				}
			}

			// Token: 0x04000CB5 RID: 3253
			[SerializeField]
			private bool IsStatisticsVisible;

			// Token: 0x04000CB6 RID: 3254
			[SerializeField]
			private int Subscriptions;

			// Token: 0x04000CB7 RID: 3255
			[SerializeField]
			private int Upvotes;

			// Token: 0x04000CB8 RID: 3256
			[SerializeField]
			private int Downvotes;

			// Token: 0x04000CB9 RID: 3257
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags IsStatisticsVisible_UxmlAttributeFlags;

			// Token: 0x04000CBA RID: 3258
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Subscriptions_UxmlAttributeFlags;

			// Token: 0x04000CBB RID: 3259
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Upvotes_UxmlAttributeFlags;

			// Token: 0x04000CBC RID: 3260
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Downvotes_UxmlAttributeFlags;
		}
	}
}
