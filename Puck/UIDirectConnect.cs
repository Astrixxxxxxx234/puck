using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x020001CD RID: 461
public class UIDirectConnect : UIView
{
	// Token: 0x06000E04 RID: 3588 RVA: 0x00042B24 File Offset: 0x00040D24
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("DirectConnectView", null);
		this.directConnect = base.View.Query("DirectConnect", null);
		this.closeIconButton = this.directConnect.Query("CloseIconButtonContainer", null).First().Query(null, null);
		this.closeIconButton.clicked += this.OnClickClose;
		this.connectButton = this.directConnect.Query("ConnectButton", null);
		this.connectButton.clicked += this.OnClickConnect;
		this.ipAddressTextField = this.directConnect.Query("IpAddressTextFieldInput", null).First().Query(null, null);
		this.ipAddressTextField.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnIpAddressChanged));
		this.ipAddressTextField.value = this.ipAddress;
		this.portIntegerField = this.directConnect.Query("PortIntegerFieldInput", null).First().Query(null, null);
		this.portIntegerField.RegisterValueChangedCallback(new EventCallback<ChangeEvent<int>>(this.OnPortChanged));
		this.portIntegerField.value = this.port;
		this.RefreshConnectButton();
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x00042C87 File Offset: 0x00040E87
	private void RefreshConnectButton()
	{
		this.connectButton.SetEnabled(!string.IsNullOrWhiteSpace(this.ipAddress));
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x00042CA2 File Offset: 0x00040EA2
	private void OnClickClose()
	{
		EventManager.TriggerEvent("Event_OnDirectConnectClickClose", null);
	}

	// Token: 0x06000E07 RID: 3591 RVA: 0x00042CB0 File Offset: 0x00040EB0
	private void OnClickConnect()
	{
		if (string.IsNullOrWhiteSpace(this.ipAddress))
		{
			return;
		}
		EventManager.TriggerEvent("Event_OnDirectConnectClickConnect", new Dictionary<string, object>
		{
			{
				"ipAddress",
				this.ipAddress.Trim()
			},
			{
				"port",
				this.port
			}
		});
	}

	// Token: 0x06000E08 RID: 3592 RVA: 0x00042D06 File Offset: 0x00040F06
	private void OnIpAddressChanged(ChangeEvent<string> changeEvent)
	{
		this.ipAddress = changeEvent.newValue;
		this.RefreshConnectButton();
	}

	// Token: 0x06000E09 RID: 3593 RVA: 0x00042D1A File Offset: 0x00040F1A
	private void OnPortChanged(ChangeEvent<int> changeEvent)
	{
		this.port = Mathf.Clamp(changeEvent.newValue, 1, 65535);
		this.portIntegerField.SetValueWithoutNotify(this.port);
	}

	// Token: 0x040008F1 RID: 2289
	private VisualElement directConnect;

	// Token: 0x040008F2 RID: 2290
	private IconButton closeIconButton;

	// Token: 0x040008F3 RID: 2291
	private Button connectButton;

	// Token: 0x040008F4 RID: 2292
	private TextField ipAddressTextField;

	// Token: 0x040008F5 RID: 2293
	private IntegerField portIntegerField;

	// Token: 0x040008F6 RID: 2294
	private string ipAddress = string.Empty;

	// Token: 0x040008F7 RID: 2295
	private int port = 30609;
}
