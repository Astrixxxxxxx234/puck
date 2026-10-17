using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x020001FE RID: 510
public class UIPopupManager : UIView
{
	// Token: 0x06000FC7 RID: 4039 RVA: 0x0004A6C0 File Offset: 0x000488C0
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("PopupsView", null);
		this.popups = base.View.Query("Popups", null);
		this.popups.Clear();
		this.UpdateFocus();
		this.UpdateVisibility();
	}

	// Token: 0x06000FC8 RID: 4040 RVA: 0x0004A718 File Offset: 0x00048918
	public void ShowPopup(string name, string title, BasePopupContent content, bool showOkButton, bool showCloseButton, object data = null)
	{
		if (this.GetPopupByName(name) != null)
		{
			return;
		}
		Popup popup = new Popup(this.popupAsset.Instantiate(), name, title, content, showOkButton, showCloseButton, data);
		this.popups.Add(popup.VisualElement);
		this.namePopupMap.Add(name, popup);
		popup.Initialize();
		popup.VisualElement.BringToFront();
		this.UpdateFocus();
		this.UpdateVisibility();
		EventManager.TriggerEvent("Event_OnPopupShow", new Dictionary<string, object>
		{
			{
				"name",
				name
			}
		});
	}

	// Token: 0x06000FC9 RID: 4041 RVA: 0x0004A7A0 File Offset: 0x000489A0
	public void HidePopup(string name)
	{
		Popup popupByName = this.GetPopupByName(name);
		if (popupByName == null)
		{
			return;
		}
		this.popups.Remove(popupByName.VisualElement);
		this.namePopupMap.Remove(name);
		popupByName.Dispose();
		this.UpdateFocus();
		this.UpdateVisibility();
		EventManager.TriggerEvent("Event_OnPopupHide", new Dictionary<string, object>
		{
			{
				"name",
				name
			}
		});
	}

	// Token: 0x06000FCA RID: 4042 RVA: 0x0004A804 File Offset: 0x00048A04
	public Popup GetPopupByName(string name)
	{
		if (this.namePopupMap.ContainsKey(name))
		{
			return this.namePopupMap[name];
		}
		return null;
	}

	// Token: 0x06000FCB RID: 4043 RVA: 0x0004A822 File Offset: 0x00048A22
	private void UpdateFocus()
	{
		base.IsFocused = (this.namePopupMap.Count > 0);
	}

	// Token: 0x06000FCC RID: 4044 RVA: 0x0004A838 File Offset: 0x00048A38
	private void UpdateVisibility()
	{
		this.popups.style.display = ((this.namePopupMap.Count > 0) ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06000FCD RID: 4045 RVA: 0x0004A861 File Offset: 0x00048A61
	public PopupNotificationContent CreateNotificationContent(string text)
	{
		return new PopupNotificationContent(this.notificationContentAsset, text);
	}

	// Token: 0x06000FCE RID: 4046 RVA: 0x0004A86F File Offset: 0x00048A6F
	public PopupMissingPasswordContent CreateMissingPasswordContent()
	{
		return new PopupMissingPasswordContent(this.missingPasswordContentAsset);
	}

	// Token: 0x06000FCF RID: 4047 RVA: 0x0004A87C File Offset: 0x00048A7C
	public PopupMissingModsPopupContent CreateMissingModsContent(string text, string notice, string[] missingModIds)
	{
		return new PopupMissingModsPopupContent(this.missingModsContentAsset, this.modPreviewAsset, text, notice, missingModIds);
	}

	// Token: 0x04000A05 RID: 2565
	[Header("References")]
	[SerializeField]
	private VisualTreeAsset popupAsset;

	// Token: 0x04000A06 RID: 2566
	[SerializeField]
	private VisualTreeAsset notificationContentAsset;

	// Token: 0x04000A07 RID: 2567
	[SerializeField]
	private VisualTreeAsset missingPasswordContentAsset;

	// Token: 0x04000A08 RID: 2568
	[SerializeField]
	private VisualTreeAsset missingModsContentAsset;

	// Token: 0x04000A09 RID: 2569
	[SerializeField]
	private VisualTreeAsset modPreviewAsset;

	// Token: 0x04000A0A RID: 2570
	private VisualElement popups;

	// Token: 0x04000A0B RID: 2571
	private Dictionary<string, Popup> namePopupMap = new Dictionary<string, Popup>();
}
