using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	// Token: 0x0200029E RID: 670
	[UxmlElement]
	public class Mod : VisualElement
	{
		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x00058A39 File Offset: 0x00056C39
		// (set) Token: 0x06001431 RID: 5169 RVA: 0x00058A41 File Offset: 0x00056C41
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

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x00058A5F File Offset: 0x00056C5F
		// (set) Token: 0x06001433 RID: 5171 RVA: 0x00058A67 File Offset: 0x00056C67
		[UxmlAttribute]
		public StyleBackground PreviewTexture
		{
			get
			{
				return this.previewTexture;
			}
			set
			{
				if (this.previewTexture == value)
				{
					return;
				}
				this.previewTexture = value;
				this.Update();
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x00058A85 File Offset: 0x00056C85
		// (set) Token: 0x06001435 RID: 5173 RVA: 0x00058A8D File Offset: 0x00056C8D
		public bool HasFiles
		{
			get
			{
				return this.hasFiles;
			}
			set
			{
				if (this.hasFiles == value)
				{
					return;
				}
				this.hasFiles = value;
				this.Refresh();
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06001436 RID: 5174 RVA: 0x00058AA6 File Offset: 0x00056CA6
		public Toggle Toggle
		{
			get
			{
				return this.toggle;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06001437 RID: 5175 RVA: 0x00058AAE File Offset: 0x00056CAE
		public ModPreview ModPreview
		{
			get
			{
				return this.modPreview;
			}
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x00058AB6 File Offset: 0x00056CB6
		public Mod()
		{
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.NoTrickleDown);
			base.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnDetachFromPanel), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x00058AEC File Offset: 0x00056CEC
		private void OnAttachToPanel(AttachToPanelEvent e)
		{
			this.descriptionLabel = this.Query("DescriptionLabel", null);
			this.previewVisualElement = this.Query("Preview", null);
			this.statusLabel = this.Query("Status", null);
			this.toggle = this.Query(null, null);
			this.toggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnToggleValueChanged));
			this.Refresh();
			this.modPreview = this.Query("ModPreviewContainer", null).First().Query(null, null);
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
					ModPreview modPreview = this.modPreview;
					modPreview.Ready = (Action)Delegate.Combine(modPreview.Ready, a);
				}
			});
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x00058BC1 File Offset: 0x00056DC1
		private void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			if (this.toggle != null)
			{
				this.toggle.UnregisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnToggleValueChanged));
			}
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x00058BE3 File Offset: 0x00056DE3
		private void Update()
		{
			if (this.descriptionLabel != null)
			{
				this.descriptionLabel.text = this.Description;
			}
			if (this.previewVisualElement != null)
			{
				this.previewVisualElement.style.backgroundImage = this.PreviewTexture;
			}
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x00058C1C File Offset: 0x00056E1C
		public void Refresh()
		{
			if (this.toggle == null)
			{
				return;
			}
			bool value = this.toggle.value;
			bool enable = !this.hasFiles || (this.toggle.enabledSelf && !value);
			this.toggle.label = (value ? "ENABLED" : "DISABLED");
			this.toggle.EnableInClassList("enabled", value);
			base.EnableInClassList("disabled", enable);
			if (this.statusLabel != null)
			{
				this.statusLabel.style.display = (this.hasFiles ? DisplayStyle.None : DisplayStyle.Flex);
			}
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x00058CBE File Offset: 0x00056EBE
		private void OnToggleValueChanged(ChangeEvent<bool> e)
		{
			this.Refresh();
		}

		// Token: 0x04000C9E RID: 3230
		private string description;

		// Token: 0x04000C9F RID: 3231
		private StyleBackground previewTexture;

		// Token: 0x04000CA0 RID: 3232
		private bool hasFiles = true;

		// Token: 0x04000CA1 RID: 3233
		public Action Ready;

		// Token: 0x04000CA2 RID: 3234
		private VisualElement previewVisualElement;

		// Token: 0x04000CA3 RID: 3235
		private Label descriptionLabel;

		// Token: 0x04000CA4 RID: 3236
		private Label statusLabel;

		// Token: 0x04000CA5 RID: 3237
		private Toggle toggle;

		// Token: 0x04000CA6 RID: 3238
		private ModPreview modPreview;

		// Token: 0x0200029F RID: 671
		[CompilerGenerated]
		[Serializable]
		public new class UxmlSerializedData : VisualElement.UxmlSerializedData
		{
			// Token: 0x06001440 RID: 5184 RVA: 0x00058CF8 File Offset: 0x00056EF8
			[RegisterUxmlCache]
			[Conditional("UNITY_EDITOR")]
			public new static void Register()
			{
				UxmlDescriptionCache.RegisterType(typeof(Mod.UxmlSerializedData), new UxmlAttributeNames[]
				{
					new UxmlAttributeNames("Description", "description", null, Array.Empty<string>()),
					new UxmlAttributeNames("PreviewTexture", "preview-texture", null, Array.Empty<string>())
				}, false);
			}

			// Token: 0x06001441 RID: 5185 RVA: 0x00058D53 File Offset: 0x00056F53
			public override object CreateInstance()
			{
				return new Mod();
			}

			// Token: 0x06001442 RID: 5186 RVA: 0x00058D5C File Offset: 0x00056F5C
			public override void Deserialize(object obj)
			{
				base.Deserialize(obj);
				Mod mod = (Mod)obj;
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Description_UxmlAttributeFlags))
				{
					mod.Description = this.Description;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.PreviewTexture_UxmlAttributeFlags))
				{
					mod.PreviewTexture = this.PreviewTexture;
				}
			}

			// Token: 0x04000CA7 RID: 3239
			[SerializeField]
			private string Description;

			// Token: 0x04000CA8 RID: 3240
			[SerializeField]
			private StyleBackground PreviewTexture;

			// Token: 0x04000CA9 RID: 3241
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Description_UxmlAttributeFlags;

			// Token: 0x04000CAA RID: 3242
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags PreviewTexture_UxmlAttributeFlags;
		}
	}
}
