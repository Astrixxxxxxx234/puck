using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	// Token: 0x0200029A RID: 666
	[UxmlElement]
	public class Link : VisualElement
	{
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x000583F2 File Offset: 0x000565F2
		// (set) Token: 0x0600141A RID: 5146 RVA: 0x000583FA File Offset: 0x000565FA
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

		// Token: 0x0600141B RID: 5147 RVA: 0x00058418 File Offset: 0x00056618
		public Link()
		{
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.NoTrickleDown);
			base.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnDetachFromPanel), TrickleDown.NoTrickleDown);
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x00058446 File Offset: 0x00056646
		private void Update()
		{
			if (this.textLabel != null)
			{
				this.textLabel.text = "<a><u>" + this.Text + "</u></a>";
			}
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x00058470 File Offset: 0x00056670
		private void OnAttachToPanel(AttachToPanelEvent e)
		{
			this.textLabel = this.Query("TextLabel", null);
			if (this.textLabel != null)
			{
				this.textLabel.RegisterCallback<ClickEvent>(new EventCallback<ClickEvent>(this.OnTextLabelClicked), TrickleDown.NoTrickleDown);
			}
			this.Update();
			Action ready = this.Ready;
			if (ready == null)
			{
				return;
			}
			ready();
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x000584CA File Offset: 0x000566CA
		private void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			if (this.textLabel != null)
			{
				this.textLabel.UnregisterCallback<ClickEvent>(new EventCallback<ClickEvent>(this.OnTextLabelClicked), TrickleDown.NoTrickleDown);
			}
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x000584EC File Offset: 0x000566EC
		private void OnTextLabelClicked(ClickEvent e)
		{
			Action clicked = this.Clicked;
			if (clicked == null)
			{
				return;
			}
			clicked();
		}

		// Token: 0x04000C93 RID: 3219
		private string text;

		// Token: 0x04000C94 RID: 3220
		public Action Ready;

		// Token: 0x04000C95 RID: 3221
		public Action Clicked;

		// Token: 0x04000C96 RID: 3222
		private Label textLabel;

		// Token: 0x0200029B RID: 667
		[CompilerGenerated]
		[Serializable]
		public new class UxmlSerializedData : VisualElement.UxmlSerializedData
		{
			// Token: 0x06001420 RID: 5152 RVA: 0x000584FE File Offset: 0x000566FE
			[RegisterUxmlCache]
			[Conditional("UNITY_EDITOR")]
			public new static void Register()
			{
				UxmlDescriptionCache.RegisterType(typeof(Link.UxmlSerializedData), new UxmlAttributeNames[]
				{
					new UxmlAttributeNames("Text", "text", null, Array.Empty<string>())
				}, false);
			}

			// Token: 0x06001421 RID: 5153 RVA: 0x00058532 File Offset: 0x00056732
			public override object CreateInstance()
			{
				return new Link();
			}

			// Token: 0x06001422 RID: 5154 RVA: 0x0005853C File Offset: 0x0005673C
			public override void Deserialize(object obj)
			{
				base.Deserialize(obj);
				Link link = (Link)obj;
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Text_UxmlAttributeFlags))
				{
					link.Text = this.Text;
				}
			}

			// Token: 0x04000C97 RID: 3223
			[SerializeField]
			private string Text;

			// Token: 0x04000C98 RID: 3224
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Text_UxmlAttributeFlags;
		}
	}
}
