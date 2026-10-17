using System;
using UnityEngine.UIElements;

// Token: 0x020001F6 RID: 502
public class PopupMissingPasswordContent : BasePopupContent
{
	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000F9E RID: 3998 RVA: 0x00049E4A File Offset: 0x0004804A
	// (set) Token: 0x06000F9F RID: 3999 RVA: 0x00049E52 File Offset: 0x00048052
	public string Password
	{
		get
		{
			return this.password;
		}
		set
		{
			if (this.password == value)
			{
				return;
			}
			this.password = value;
			this.Update();
		}
	}

	// Token: 0x06000FA0 RID: 4000 RVA: 0x00049E70 File Offset: 0x00048070
	public PopupMissingPasswordContent(VisualTreeAsset asset) : base(asset)
	{
	}

	// Token: 0x06000FA1 RID: 4001 RVA: 0x00049E84 File Offset: 0x00048084
	public override void Initialize()
	{
		base.Initialize();
		this.textField = base.VisualElement.Query("PasswordTextField", null).First().Query(null, null);
		this.textField.value = this.Password;
		this.textField.RegisterCallback<ChangeEvent<string>>(new EventCallback<ChangeEvent<string>>(this.OnPasswordChanged), TrickleDown.NoTrickleDown);
		this.Update();
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x00049EF1 File Offset: 0x000480F1
	public override void Dispose()
	{
		base.Dispose();
		this.textField.UnregisterCallback<ChangeEvent<string>>(new EventCallback<ChangeEvent<string>>(this.OnPasswordChanged), TrickleDown.NoTrickleDown);
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x00049F11 File Offset: 0x00048111
	internal override void Update()
	{
		base.Update();
		this.textField.value = this.Password;
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x00049F2A File Offset: 0x0004812A
	private void OnPasswordChanged(ChangeEvent<string> changeEvent)
	{
		this.Password = changeEvent.newValue;
	}

	// Token: 0x040009E2 RID: 2530
	private string password = string.Empty;

	// Token: 0x040009E3 RID: 2531
	private TextField textField;
}
