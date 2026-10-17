using System;

// Token: 0x020001F3 RID: 499
public class UIPlayerMenuController : UIViewController<UIPlayerMenu>
{
	// Token: 0x06000F90 RID: 3984 RVA: 0x00049D50 File Offset: 0x00047F50
	public override void Awake()
	{
		base.Awake();
		this.uiPlayerMenu = base.GetComponent<UIPlayerMenu>();
	}

	// Token: 0x06000F91 RID: 3985 RVA: 0x00049D64 File Offset: 0x00047F64
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040009DB RID: 2523
	private UIPlayerMenu uiPlayerMenu;
}
