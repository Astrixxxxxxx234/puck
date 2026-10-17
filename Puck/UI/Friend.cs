using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	// Token: 0x02000292 RID: 658
	[UxmlElement]
	public class Friend : VisualElement
	{
		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x00057C42 File Offset: 0x00055E42
		// (set) Token: 0x060013E6 RID: 5094 RVA: 0x00057C4A File Offset: 0x00055E4A
		[UxmlAttribute]
		public string Username
		{
			get
			{
				return this.username;
			}
			set
			{
				if (this.username == value)
				{
					return;
				}
				this.username = value;
				this.Update();
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060013E7 RID: 5095 RVA: 0x00057C68 File Offset: 0x00055E68
		// (set) Token: 0x060013E8 RID: 5096 RVA: 0x00057C70 File Offset: 0x00055E70
		[UxmlAttribute]
		public Texture2D Texture
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

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x00057C8E File Offset: 0x00055E8E
		// (set) Token: 0x060013EA RID: 5098 RVA: 0x00057C96 File Offset: 0x00055E96
		public User User { get; private set; }

		// Token: 0x060013EB RID: 5099 RVA: 0x00057C9F File Offset: 0x00055E9F
		public Friend()
		{
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.TrickleDown);
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x00057CBC File Offset: 0x00055EBC
		private void OnAttachToPanel(AttachToPanelEvent e)
		{
			this.User = this.Query("User", null);
			this.inviteIconButton = this.Query("InviteIconButtonContainer", null).First().Query(null, null);
			this.inviteIconButton.clicked += this.OnClickInviteButton;
			this.Update();
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00057D23 File Offset: 0x00055F23
		private void OnClickInviteButton()
		{
			Action inviteButtonClicked = this.InviteButtonClicked;
			if (inviteButtonClicked == null)
			{
				return;
			}
			inviteButtonClicked();
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00057D35 File Offset: 0x00055F35
		private void Update()
		{
			if (this.User != null)
			{
				this.User.AvatarTexture = this.Texture;
				this.User.Username = this.Username;
			}
		}

		// Token: 0x04000C74 RID: 3188
		private string username;

		// Token: 0x04000C75 RID: 3189
		private Texture2D texture;

		// Token: 0x04000C77 RID: 3191
		public Action InviteButtonClicked;

		// Token: 0x04000C78 RID: 3192
		private IconButton inviteIconButton;

		// Token: 0x02000293 RID: 659
		[CompilerGenerated]
		[Serializable]
		public new class UxmlSerializedData : VisualElement.UxmlSerializedData
		{
			// Token: 0x060013EF RID: 5103 RVA: 0x00057D64 File Offset: 0x00055F64
			[RegisterUxmlCache]
			[Conditional("UNITY_EDITOR")]
			public new static void Register()
			{
				UxmlDescriptionCache.RegisterType(typeof(Friend.UxmlSerializedData), new UxmlAttributeNames[]
				{
					new UxmlAttributeNames("Username", "username", null, Array.Empty<string>()),
					new UxmlAttributeNames("Texture", "texture", null, Array.Empty<string>())
				}, false);
			}

			// Token: 0x060013F0 RID: 5104 RVA: 0x00057DBF File Offset: 0x00055FBF
			public override object CreateInstance()
			{
				return new Friend();
			}

			// Token: 0x060013F1 RID: 5105 RVA: 0x00057DC8 File Offset: 0x00055FC8
			public override void Deserialize(object obj)
			{
				base.Deserialize(obj);
				Friend friend = (Friend)obj;
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Username_UxmlAttributeFlags))
				{
					friend.Username = this.Username;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Texture_UxmlAttributeFlags))
				{
					friend.Texture = this.Texture;
				}
			}

			// Token: 0x04000C79 RID: 3193
			[SerializeField]
			private Texture2D Texture;

			// Token: 0x04000C7A RID: 3194
			[SerializeField]
			private string Username;

			// Token: 0x04000C7B RID: 3195
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Texture_UxmlAttributeFlags;

			// Token: 0x04000C7C RID: 3196
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Username_UxmlAttributeFlags;
		}
	}
}
