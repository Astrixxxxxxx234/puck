using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	// Token: 0x02000298 RID: 664
	[UxmlElement]
	public class KeyBindField : VisualElement
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x00057FD0 File Offset: 0x000561D0
		// (set) Token: 0x06001406 RID: 5126 RVA: 0x00057FD8 File Offset: 0x000561D8
		[UxmlAttribute]
		public KeyBindInteractionType InteractionType
		{
			get
			{
				return this.interactionType;
			}
			set
			{
				if (this.interactionType == value)
				{
					return;
				}
				this.interactionType = value;
				this.OnInteractionTypeChanged();
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06001407 RID: 5127 RVA: 0x00057FF1 File Offset: 0x000561F1
		// (set) Token: 0x06001408 RID: 5128 RVA: 0x00057FF9 File Offset: 0x000561F9
		[UxmlAttribute]
		public string Label
		{
			get
			{
				return this.label;
			}
			set
			{
				if (this.label == value)
				{
					return;
				}
				this.label = value;
				this.OnLabelChanged();
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06001409 RID: 5129 RVA: 0x00058017 File Offset: 0x00056217
		// (set) Token: 0x0600140A RID: 5130 RVA: 0x0005801F File Offset: 0x0005621F
		[UxmlAttribute]
		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				if (this.path == value)
				{
					return;
				}
				this.path = value;
				this.OnPathChanged();
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x0005803D File Offset: 0x0005623D
		// (set) Token: 0x0600140C RID: 5132 RVA: 0x00058045 File Offset: 0x00056245
		public KeyBindInteraction Interaction
		{
			get
			{
				return this.interaction;
			}
			set
			{
				if (this.interaction == value)
				{
					return;
				}
				this.interaction = value;
				this.OnInteractionChanged();
			}
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0005805E File Offset: 0x0005625E
		public KeyBindField()
		{
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.TrickleDown);
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0005807C File Offset: 0x0005627C
		private void OnAttachToPanel(AttachToPanelEvent e)
		{
			this.nameLabel = this.Query("NameLabel", null).First();
			this.pathTextField = this.Query("PathTextField", null).First();
			this.pathTextField.RegisterCallback<ClickEvent>(new EventCallback<ClickEvent>(this.OnPathTextFieldClicked), TrickleDown.NoTrickleDown);
			this.interactionDropdownField = this.Query("InteractionDropdownField", null).First();
			this.interactionDropdownField.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnInteractionDropdownFieldValueChanged));
			this.OnLabelChanged();
			this.OnPathChanged();
			this.OnInteractionTypeChanged();
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0005811C File Offset: 0x0005631C
		private void OnInteractionTypeChanged()
		{
			if (this.interactionDropdownField == null)
			{
				return;
			}
			KeyBindInteractionType keyBindInteractionType = this.InteractionType;
			if (keyBindInteractionType != KeyBindInteractionType.Press)
			{
				if (keyBindInteractionType == KeyBindInteractionType.Hold)
				{
					this.interactionDropdownField.choices = new List<string>
					{
						"CONTINUOUS",
						"TOGGLE"
					};
				}
			}
			else
			{
				this.interactionDropdownField.choices = new List<string>
				{
					"PRESS",
					"RELEASE",
					"DOUBLE PRESS",
					"HOLD"
				};
			}
			this.OnInteractionChanged();
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x000581AC File Offset: 0x000563AC
		private void OnLabelChanged()
		{
			if (this.nameLabel == null)
			{
				return;
			}
			this.nameLabel.text = this.label;
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x000581C8 File Offset: 0x000563C8
		private void OnPathChanged()
		{
			if (this.pathTextField == null)
			{
				return;
			}
			this.pathTextField.value = this.path;
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x000581E4 File Offset: 0x000563E4
		private void OnInteractionChanged()
		{
			if (this.interactionDropdownField == null)
			{
				return;
			}
			string valueWithoutNotify;
			switch (this.interaction)
			{
			case KeyBindInteraction.Release:
				valueWithoutNotify = "RELEASE";
				goto IL_63;
			case KeyBindInteraction.DoublePress:
				valueWithoutNotify = "DOUBLE PRESS";
				goto IL_63;
			case KeyBindInteraction.Hold:
				valueWithoutNotify = "HOLD";
				goto IL_63;
			case KeyBindInteraction.Toggle:
				valueWithoutNotify = "TOGGLE";
				goto IL_63;
			}
			valueWithoutNotify = ((this.InteractionType == KeyBindInteractionType.Press) ? "PRESS" : "CONTINUOUS");
			IL_63:
			this.interactionDropdownField.SetValueWithoutNotify(valueWithoutNotify);
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x00058260 File Offset: 0x00056460
		private void OnPathTextFieldClicked(ClickEvent clickEvent)
		{
			Action click = this.Click;
			if (click == null)
			{
				return;
			}
			click();
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x00058274 File Offset: 0x00056474
		private void OnInteractionDropdownFieldValueChanged(ChangeEvent<string> changeEvent)
		{
			string newValue = changeEvent.newValue;
			if (!(newValue == "RELEASE"))
			{
				if (!(newValue == "DOUBLE PRESS"))
				{
					if (!(newValue == "HOLD"))
					{
						if (!(newValue == "TOGGLE"))
						{
							this.interaction = ((this.InteractionType == KeyBindInteractionType.Press) ? KeyBindInteraction.Press : KeyBindInteraction.Continuous);
						}
						else
						{
							this.interaction = KeyBindInteraction.Toggle;
						}
					}
					else
					{
						this.interaction = KeyBindInteraction.Hold;
					}
				}
				else
				{
					this.interaction = KeyBindInteraction.DoublePress;
				}
			}
			else
			{
				this.interaction = KeyBindInteraction.Release;
			}
			Action<KeyBindInteraction> interactionChange = this.InteractionChange;
			if (interactionChange == null)
			{
				return;
			}
			interactionChange(this.interaction);
		}

		// Token: 0x04000C84 RID: 3204
		private KeyBindInteractionType interactionType;

		// Token: 0x04000C85 RID: 3205
		private string label;

		// Token: 0x04000C86 RID: 3206
		private string path;

		// Token: 0x04000C87 RID: 3207
		private KeyBindInteraction interaction;

		// Token: 0x04000C88 RID: 3208
		public Action Click;

		// Token: 0x04000C89 RID: 3209
		public Action<KeyBindInteraction> InteractionChange;

		// Token: 0x04000C8A RID: 3210
		private Label nameLabel;

		// Token: 0x04000C8B RID: 3211
		private TextField pathTextField;

		// Token: 0x04000C8C RID: 3212
		private DropdownField interactionDropdownField;

		// Token: 0x02000299 RID: 665
		[CompilerGenerated]
		[Serializable]
		public new class UxmlSerializedData : VisualElement.UxmlSerializedData
		{
			// Token: 0x06001415 RID: 5141 RVA: 0x0005830C File Offset: 0x0005650C
			[RegisterUxmlCache]
			[Conditional("UNITY_EDITOR")]
			public new static void Register()
			{
				UxmlDescriptionCache.RegisterType(typeof(KeyBindField.UxmlSerializedData), new UxmlAttributeNames[]
				{
					new UxmlAttributeNames("InteractionType", "interaction-type", null, Array.Empty<string>()),
					new UxmlAttributeNames("Label", "label", null, Array.Empty<string>()),
					new UxmlAttributeNames("Path", "path", null, Array.Empty<string>())
				}, false);
			}

			// Token: 0x06001416 RID: 5142 RVA: 0x00058383 File Offset: 0x00056583
			public override object CreateInstance()
			{
				return new KeyBindField();
			}

			// Token: 0x06001417 RID: 5143 RVA: 0x0005838C File Offset: 0x0005658C
			public override void Deserialize(object obj)
			{
				base.Deserialize(obj);
				KeyBindField keyBindField = (KeyBindField)obj;
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.InteractionType_UxmlAttributeFlags))
				{
					keyBindField.InteractionType = this.InteractionType;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Label_UxmlAttributeFlags))
				{
					keyBindField.Label = this.Label;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Path_UxmlAttributeFlags))
				{
					keyBindField.Path = this.Path;
				}
			}

			// Token: 0x04000C8D RID: 3213
			[SerializeField]
			private KeyBindInteractionType InteractionType;

			// Token: 0x04000C8E RID: 3214
			[SerializeField]
			private string Label;

			// Token: 0x04000C8F RID: 3215
			[SerializeField]
			private string Path;

			// Token: 0x04000C90 RID: 3216
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags InteractionType_UxmlAttributeFlags;

			// Token: 0x04000C91 RID: 3217
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Label_UxmlAttributeFlags;

			// Token: 0x04000C92 RID: 3218
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Path_UxmlAttributeFlags;
		}
	}
}
