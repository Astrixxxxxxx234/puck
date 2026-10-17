using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	// Token: 0x020002A7 RID: 679
	[UxmlElement]
	public class User : VisualElement
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06001470 RID: 5232 RVA: 0x000594F5 File Offset: 0x000576F5
		// (set) Token: 0x06001471 RID: 5233 RVA: 0x000594FD File Offset: 0x000576FD
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

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x0005951B File Offset: 0x0005771B
		// (set) Token: 0x06001473 RID: 5235 RVA: 0x00059523 File Offset: 0x00057723
		[UxmlAttribute]
		public Texture2D AvatarTexture
		{
			get
			{
				return this.avatarTexture;
			}
			set
			{
				if (this.avatarTexture == value)
				{
					return;
				}
				this.avatarTexture = value;
				this.Update();
			}
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x00059541 File Offset: 0x00057741
		public User()
		{
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.TrickleDown);
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0005955C File Offset: 0x0005775C
		private void OnAttachToPanel(AttachToPanelEvent e)
		{
			this.avatarIcon = this.Query("AvatarIconContainer", null).First().Query(null, null);
			this.usernameLabel = this.Query("UsernameLabel", null);
			this.Update();
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x000595AC File Offset: 0x000577AC
		private void Update()
		{
			if (this.usernameLabel != null)
			{
				this.usernameLabel.text = this.Username;
			}
			if (this.avatarIcon != null)
			{
				this.avatarIcon.Texture = this.AvatarTexture;
			}
		}

		// Token: 0x04000CD3 RID: 3283
		private string username;

		// Token: 0x04000CD4 RID: 3284
		private Texture2D avatarTexture;

		// Token: 0x04000CD5 RID: 3285
		private Icon avatarIcon;

		// Token: 0x04000CD6 RID: 3286
		private Label usernameLabel;

		// Token: 0x020002A8 RID: 680
		[CompilerGenerated]
		[Serializable]
		public new class UxmlSerializedData : VisualElement.UxmlSerializedData
		{
			// Token: 0x06001477 RID: 5239 RVA: 0x000595E8 File Offset: 0x000577E8
			[RegisterUxmlCache]
			[Conditional("UNITY_EDITOR")]
			public new static void Register()
			{
				UxmlDescriptionCache.RegisterType(typeof(User.UxmlSerializedData), new UxmlAttributeNames[]
				{
					new UxmlAttributeNames("Username", "username", null, Array.Empty<string>()),
					new UxmlAttributeNames("AvatarTexture", "avatar-texture", null, Array.Empty<string>())
				}, false);
			}

			// Token: 0x06001478 RID: 5240 RVA: 0x00059643 File Offset: 0x00057843
			public override object CreateInstance()
			{
				return new User();
			}

			// Token: 0x06001479 RID: 5241 RVA: 0x0005964C File Offset: 0x0005784C
			public override void Deserialize(object obj)
			{
				base.Deserialize(obj);
				User user = (User)obj;
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.Username_UxmlAttributeFlags))
				{
					user.Username = this.Username;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.AvatarTexture_UxmlAttributeFlags))
				{
					user.AvatarTexture = this.AvatarTexture;
				}
			}

			// Token: 0x04000CD7 RID: 3287
			[SerializeField]
			private Texture2D AvatarTexture;

			// Token: 0x04000CD8 RID: 3288
			[SerializeField]
			private string Username;

			// Token: 0x04000CD9 RID: 3289
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags AvatarTexture_UxmlAttributeFlags;

			// Token: 0x04000CDA RID: 3290
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags Username_UxmlAttributeFlags;
		}
	}
}
