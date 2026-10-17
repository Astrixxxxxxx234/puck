using System;
using UnityEngine.UIElements;

// Token: 0x020001F2 RID: 498
public class UIPlayerMenu : UIView
{
	// Token: 0x06000F89 RID: 3977 RVA: 0x00049C24 File Offset: 0x00047E24
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("PlayerMenuView", null);
		this.playerMenu = base.View.Query("PlayerMenu", null);
		this.identityButton = this.playerMenu.Query("IdentityButton", null);
		this.identityButton.clicked += this.OnClickIdentity;
		this.appearanceButton = this.playerMenu.Query("AppearanceButton", null);
		this.appearanceButton.clicked += this.OnClickAppearance;
		this.backButton = this.playerMenu.Query("BackButton", null);
		this.backButton.clicked += this.OnClickBack;
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x00049CFD File Offset: 0x00047EFD
	public override bool Show()
	{
		bool flag = base.Show();
		if (flag)
		{
			EventManager.TriggerEvent("Event_OnPlayerMenuShow", null);
		}
		return flag;
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x00049D13 File Offset: 0x00047F13
	public override bool Hide()
	{
		bool flag = base.Hide();
		if (flag)
		{
			EventManager.TriggerEvent("Event_OnPlayerMenuHide", null);
		}
		return flag;
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x00049D29 File Offset: 0x00047F29
	private void OnClickIdentity()
	{
		EventManager.TriggerEvent("Event_OnPlayerMenuClickIdentity", null);
	}

	// Token: 0x06000F8D RID: 3981 RVA: 0x00049D36 File Offset: 0x00047F36
	private void OnClickAppearance()
	{
		EventManager.TriggerEvent("Event_OnPlayerMenuClickAppearance", null);
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x00049D43 File Offset: 0x00047F43
	private void OnClickBack()
	{
		EventManager.TriggerEvent("Event_OnPlayerMenuClickBack", null);
	}

	// Token: 0x040009D7 RID: 2519
	private VisualElement playerMenu;

	// Token: 0x040009D8 RID: 2520
	private Button identityButton;

	// Token: 0x040009D9 RID: 2521
	private Button appearanceButton;

	// Token: 0x040009DA RID: 2522
	private Button backButton;
}
