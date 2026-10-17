using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x0200021A RID: 538
public class UIToastManager : UIView
{
	// Token: 0x06001115 RID: 4373 RVA: 0x00050ED5 File Offset: 0x0004F0D5
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("ToastsView", null);
		this.toasts = base.View.Query("Toasts", null);
		this.toasts.Clear();
	}

	// Token: 0x06001116 RID: 4374 RVA: 0x00050F18 File Offset: 0x0004F118
	public void ShowToast(string name, string content, float hideDelay = 3f)
	{
		if (this.nameToastMap.ContainsKey(name))
		{
			this.HideToast(name);
		}
		VisualElement visualElement = this.toastAsset.Instantiate();
		Toast toast = new Toast(this, visualElement, name, content.ToUpper(), hideDelay);
		this.toasts.Add(toast.VisualElement);
		this.nameToastMap.Add(name, toast);
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x00050F74 File Offset: 0x0004F174
	public void HideToast(string name)
	{
		if (!this.nameToastMap.ContainsKey(name))
		{
			return;
		}
		this.toasts.Remove(this.nameToastMap[name].VisualElement);
		this.nameToastMap[name].Dispose();
		this.nameToastMap.Remove(name);
	}

	// Token: 0x04000AB8 RID: 2744
	[Header("References")]
	[SerializeField]
	public VisualTreeAsset toastAsset;

	// Token: 0x04000AB9 RID: 2745
	private Dictionary<string, Toast> nameToastMap = new Dictionary<string, Toast>();

	// Token: 0x04000ABA RID: 2746
	private VisualElement toasts;
}
