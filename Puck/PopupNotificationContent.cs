using System;
using UnityEngine.UIElements;

// Token: 0x020001F5 RID: 501
public class PopupNotificationContent : BasePopupContent
{
	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000F99 RID: 3993 RVA: 0x00049DC9 File Offset: 0x00047FC9
	// (set) Token: 0x06000F9A RID: 3994 RVA: 0x00049DD1 File Offset: 0x00047FD1
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

	// Token: 0x06000F9B RID: 3995 RVA: 0x00049DEF File Offset: 0x00047FEF
	public PopupNotificationContent(VisualTreeAsset asset, string text) : base(asset)
	{
		this.text = text;
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x00049DFF File Offset: 0x00047FFF
	public override void Initialize()
	{
		base.Initialize();
		this.textLabel = base.VisualElement.Query("TextLabel", null);
		this.Update();
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x00049E29 File Offset: 0x00048029
	internal override void Update()
	{
		base.Update();
		if (this.textLabel != null)
		{
			this.textLabel.text = this.Text;
		}
	}

	// Token: 0x040009E0 RID: 2528
	private string text;

	// Token: 0x040009E1 RID: 2529
	private Label textLabel;
}
