using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x02000218 RID: 536
public class Toast
{
	// Token: 0x06001109 RID: 4361 RVA: 0x00050D4F File Offset: 0x0004EF4F
	public Toast(UIToastManager uiToastManager, VisualElement visualElement, string name, string content, float hideDelay)
	{
		this.UIToastManager = uiToastManager;
		this.VisualElement = visualElement;
		this.Name = name;
		this.Content = content;
		this.HideDelay = hideDelay;
		this.Initialize();
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x00050D84 File Offset: 0x0004EF84
	public void Initialize()
	{
		this.VisualElement.RegisterCallback<ClickEvent>(new EventCallback<ClickEvent>(this.OnClick), TrickleDown.NoTrickleDown);
		this.contentLabel = this.VisualElement.Query("ContentLabel", null);
		this.contentLabel.text = this.Content;
		this.Hide();
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x00050DDC File Offset: 0x0004EFDC
	public void Hide()
	{
		this.hideCoroutine = this.IHide();
		this.UIToastManager.StartCoroutine(this.hideCoroutine);
	}

	// Token: 0x0600110C RID: 4364 RVA: 0x00050DFC File Offset: 0x0004EFFC
	private IEnumerator IHide()
	{
		yield return new WaitForSeconds(this.HideDelay);
		this.UIToastManager.HideToast(this.Name);
		this.hideCoroutine = null;
		yield break;
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x00050E0B File Offset: 0x0004F00B
	public void Dispose()
	{
		this.VisualElement.UnregisterCallback<ClickEvent>(new EventCallback<ClickEvent>(this.OnClick), TrickleDown.NoTrickleDown);
		if (this.hideCoroutine != null)
		{
			this.UIToastManager.StopCoroutine(this.hideCoroutine);
			this.hideCoroutine = null;
		}
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x00050E45 File Offset: 0x0004F045
	private void OnClick(ClickEvent clickEvent)
	{
		this.UIToastManager.HideToast(this.Name);
	}

	// Token: 0x04000AAE RID: 2734
	public UIToastManager UIToastManager;

	// Token: 0x04000AAF RID: 2735
	public VisualElement VisualElement;

	// Token: 0x04000AB0 RID: 2736
	public string Name;

	// Token: 0x04000AB1 RID: 2737
	public string Content;

	// Token: 0x04000AB2 RID: 2738
	public float HideDelay;

	// Token: 0x04000AB3 RID: 2739
	private IEnumerator hideCoroutine;

	// Token: 0x04000AB4 RID: 2740
	private Label contentLabel;
}
