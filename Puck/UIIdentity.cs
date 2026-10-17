using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x020001D9 RID: 473
public class UIIdentity : UIView
{
	// Token: 0x06000E67 RID: 3687 RVA: 0x00044464 File Offset: 0x00042664
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("IdentityView", null);
		this.identity = base.View.Query("Identity", null);
		this.closeIconButton = this.identity.Query("CloseIconButtonContainer", null).First().Query(null, null);
		this.closeIconButton.clicked += this.OnClickClose;
		this.confirmButton = this.identity.Query("ConfirmButton", null);
		this.confirmButton.clicked += this.OnClickConfirm;
		this.usernameTextField = this.identity.Query("UsernameTextField", null).First().Query(null, null);
		this.usernameTextField.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnNameChanged));
		this.usernameTextField.RegisterCallback<FocusOutEvent>(new EventCallback<FocusOutEvent>(this.OnNameFocusOut), TrickleDown.NoTrickleDown);
		this.numberIntegerField = this.identity.Query("NumberIntegerField", null).First().Query(null, null);
		this.numberIntegerField.RegisterValueChangedCallback(new EventCallback<ChangeEvent<int>>(this.OnNumberChanged));
		this.numberIntegerField.RegisterCallback<FocusOutEvent>(new EventCallback<FocusOutEvent>(this.OnNumberFocusOut), TrickleDown.NoTrickleDown);
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x000445CF File Offset: 0x000427CF
	public override bool Show()
	{
		bool flag = base.Show();
		if (flag)
		{
			EventManager.TriggerEvent("Event_OnIdentityShow", null);
		}
		return flag;
	}

	// Token: 0x06000E69 RID: 3689 RVA: 0x000445E5 File Offset: 0x000427E5
	public override bool Hide()
	{
		bool flag = base.Hide();
		if (flag)
		{
			EventManager.TriggerEvent("Event_OnIdentityHide", null);
		}
		return flag;
	}

	// Token: 0x06000E6A RID: 3690 RVA: 0x000445FB File Offset: 0x000427FB
	public void SetIdentity(string username, int number)
	{
		this.username = username;
		this.usernameTextField.value = this.username;
		this.number = number;
		this.numberIntegerField.value = this.number;
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x0004462D File Offset: 0x0004282D
	private void OnNameChanged(ChangeEvent<string> changeEvent)
	{
		this.username = changeEvent.newValue;
		if (!string.IsNullOrEmpty(this.username))
		{
			this.username = StringUtils.FilterStringNotLetters(this.username);
			this.usernameTextField.value = this.username;
		}
	}

	// Token: 0x06000E6C RID: 3692 RVA: 0x0004466C File Offset: 0x0004286C
	private void OnNameFocusOut(FocusOutEvent focusOutEvent)
	{
		this.username = this.usernameTextField.value;
		if (!string.IsNullOrEmpty(this.username))
		{
			this.username = StringUtils.FilterStringNotLetters(this.username);
			this.username = StringUtils.FilterStringProfanity(this.username, false);
			this.usernameTextField.value = this.username;
		}
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x000446CB File Offset: 0x000428CB
	private void OnNumberChanged(ChangeEvent<int> changeEvent)
	{
		this.number = changeEvent.newValue;
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x000446D9 File Offset: 0x000428D9
	private void OnNumberFocusOut(FocusOutEvent focusOutEvent)
	{
		this.number = Mathf.Clamp(this.numberIntegerField.value, 1, 99);
		this.numberIntegerField.value = this.number;
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x00044705 File Offset: 0x00042905
	private void OnClickClose()
	{
		EventManager.TriggerEvent("Event_OnIdentityClickClose", null);
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x00044712 File Offset: 0x00042912
	private void OnClickConfirm()
	{
		EventManager.TriggerEvent("Event_OnIdentityClickConfirm", new Dictionary<string, object>
		{
			{
				"username",
				this.username
			},
			{
				"number",
				this.number
			}
		});
	}

	// Token: 0x0400092C RID: 2348
	private VisualElement identity;

	// Token: 0x0400092D RID: 2349
	private TextField usernameTextField;

	// Token: 0x0400092E RID: 2350
	private IntegerField numberIntegerField;

	// Token: 0x0400092F RID: 2351
	private IconButton closeIconButton;

	// Token: 0x04000930 RID: 2352
	private Button confirmButton;

	// Token: 0x04000931 RID: 2353
	private string username;

	// Token: 0x04000932 RID: 2354
	private int number;
}
