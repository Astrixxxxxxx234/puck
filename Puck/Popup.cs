using System;
using System.Collections.Generic;
using UI;
using UnityEngine.UIElements;

// Token: 0x020001FD RID: 509
public class Popup
{
	// Token: 0x06000FC2 RID: 4034 RVA: 0x0004A48E File Offset: 0x0004868E
	public Popup(VisualElement visualElement, string name, string title, BasePopupContent content, bool showOkButton, bool showCloseButton, object data = null)
	{
		this.VisualElement = visualElement;
		this.Name = name;
		this.Title = title;
		this.Content = content;
		this.ShowOkButton = showOkButton;
		this.ShowCloseButton = showCloseButton;
		this.Data = data;
	}

	// Token: 0x06000FC3 RID: 4035 RVA: 0x0004A4CC File Offset: 0x000486CC
	public void Initialize()
	{
		this.header = this.VisualElement.Query("Header", null);
		this.content = this.VisualElement.Query("Content", null);
		this.footer = this.VisualElement.Query("Footer", null);
		this.titleLabel = this.header.Query(null, null);
		this.titleLabel.text = this.Title;
		this.closeIconButton = this.header.Query("CloseIconButtonContainer", null).First().Query(null, null);
		this.closeIconButton.clicked += this.OnClickClose;
		this.okButton = this.footer.Query("OkButton", null);
		this.okButton.clicked += this.OnClickOk;
		if (!this.ShowOkButton)
		{
			this.okButton.style.display = DisplayStyle.None;
		}
		if (!this.ShowCloseButton)
		{
			this.closeIconButton.style.display = DisplayStyle.None;
		}
		this.Content.Initialize();
		this.content.Add(this.Content.VisualElement);
	}

	// Token: 0x06000FC4 RID: 4036 RVA: 0x0004A628 File Offset: 0x00048828
	public void Dispose()
	{
		this.closeIconButton.clicked -= this.OnClickClose;
		this.okButton.clicked -= this.OnClickOk;
		this.content.Remove(this.Content.VisualElement);
		this.Content.Dispose();
	}

	// Token: 0x06000FC5 RID: 4037 RVA: 0x0004A684 File Offset: 0x00048884
	private void OnClickClose()
	{
		EventManager.TriggerEvent("Event_OnPopupClickClose", new Dictionary<string, object>
		{
			{
				"popup",
				this
			}
		});
	}

	// Token: 0x06000FC6 RID: 4038 RVA: 0x0004A6A1 File Offset: 0x000488A1
	private void OnClickOk()
	{
		EventManager.TriggerEvent("Event_OnPopupClickOk", new Dictionary<string, object>
		{
			{
				"popup",
				this
			}
		});
	}

	// Token: 0x040009F7 RID: 2551
	public TemplateContainer TemplateContainer;

	// Token: 0x040009F8 RID: 2552
	public VisualElement VisualElement;

	// Token: 0x040009F9 RID: 2553
	public string Name;

	// Token: 0x040009FA RID: 2554
	public string Title;

	// Token: 0x040009FB RID: 2555
	public BasePopupContent Content;

	// Token: 0x040009FC RID: 2556
	public bool ShowOkButton;

	// Token: 0x040009FD RID: 2557
	public bool ShowCloseButton;

	// Token: 0x040009FE RID: 2558
	public object Data;

	// Token: 0x040009FF RID: 2559
	private VisualElement header;

	// Token: 0x04000A00 RID: 2560
	private VisualElement content;

	// Token: 0x04000A01 RID: 2561
	private VisualElement footer;

	// Token: 0x04000A02 RID: 2562
	private Label titleLabel;

	// Token: 0x04000A03 RID: 2563
	private Button okButton;

	// Token: 0x04000A04 RID: 2564
	private IconButton closeIconButton;
}
