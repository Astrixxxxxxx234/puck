using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UI;
using UnityEngine.UIElements;

// Token: 0x020001F7 RID: 503
public class PopupMissingModsPopupContent : BasePopupContent
{
	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x00049F38 File Offset: 0x00048138
	// (set) Token: 0x06000FA6 RID: 4006 RVA: 0x00049F40 File Offset: 0x00048140
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

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x00049F5E File Offset: 0x0004815E
	// (set) Token: 0x06000FA8 RID: 4008 RVA: 0x00049F66 File Offset: 0x00048166
	public string Notice
	{
		get
		{
			return this.notice;
		}
		set
		{
			if (this.notice == value)
			{
				return;
			}
			this.notice = value;
			this.Update();
		}
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x00049F84 File Offset: 0x00048184
	public PopupMissingModsPopupContent(VisualTreeAsset asset, VisualTreeAsset modPreviewAsset, string text, string notice, string[] missingModIds) : base(asset)
	{
		this.modPreviewAsset = modPreviewAsset;
		this.text = text;
		this.notice = notice;
		this.steamWorkshopItems = (from modId in missingModIds
		select new SteamWorkshopItem(modId, null)).ToArray<SteamWorkshopItem>();
	}

	// Token: 0x06000FAA RID: 4010 RVA: 0x00049FEC File Offset: 0x000481EC
	public override void Initialize()
	{
		base.Initialize();
		this.textLabel = base.VisualElement.Query("TextLabel", null);
		this.noticeLabel = base.VisualElement.Query("NoticeLabel", null);
		this.missingModsList = base.VisualElement.Query("MissingModsList", null);
		SteamWorkshopItem[] array = this.steamWorkshopItems;
		for (int i = 0; i < array.Length; i++)
		{
			SteamWorkshopItem steamWorkshopItem = array[i];
			this.AddModPreview(steamWorkshopItem);
			SteamWorkshopItem steamWorkshopItem3 = steamWorkshopItem;
			steamWorkshopItem3.StateChanged = (Action<SteamWorkshopItemState, SteamWorkshopItemState>)Delegate.Combine(steamWorkshopItem3.StateChanged, new Action<SteamWorkshopItemState, SteamWorkshopItemState>(delegate(SteamWorkshopItemState oldState, SteamWorkshopItemState newState)
			{
				this.OnSteamWorkshopItemStateChanged(steamWorkshopItem.Id, oldState, newState);
			}));
			SteamWorkshopItem steamWorkshopItem2 = steamWorkshopItem;
			steamWorkshopItem2.DetailsStateChanged = (Action<SteamWorkshopItemDetailsState, SteamWorkshopItemDetailsState>)Delegate.Combine(steamWorkshopItem2.DetailsStateChanged, new Action<SteamWorkshopItemDetailsState, SteamWorkshopItemDetailsState>(delegate(SteamWorkshopItemDetailsState oldDetailsState, SteamWorkshopItemDetailsState newDetailsState)
			{
				this.OnSteamWorkshopItemDetailsStateChanged(steamWorkshopItem.Id, oldDetailsState, newDetailsState);
			}));
			steamWorkshopItem.Initialize();
		}
		this.Update();
	}

	// Token: 0x06000FAB RID: 4011 RVA: 0x0004A0EC File Offset: 0x000482EC
	internal override void Update()
	{
		base.Update();
		if (this.textLabel != null)
		{
			this.textLabel.text = this.Text;
		}
		if (this.noticeLabel != null)
		{
			this.noticeLabel.text = this.Notice;
		}
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x0004A128 File Offset: 0x00048328
	public override void Dispose()
	{
		base.Dispose();
		foreach (SteamWorkshopItem steamWorkshopItem in this.steamWorkshopItems)
		{
			steamWorkshopItem.Dispose();
			this.RemoveMod(steamWorkshopItem);
		}
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x0004A164 File Offset: 0x00048364
	private void AddModPreview(SteamWorkshopItem steamWorkshopItem)
	{
		TemplateContainer templateContainer = this.modPreviewAsset.Instantiate();
		ModPreview uiModPreview = templateContainer.Query(null, null);
		ModPreview uiModPreview2 = uiModPreview;
		Action <>9__1;
		uiModPreview2.Ready = (Action)Delegate.Combine(uiModPreview2.Ready, new Action(delegate()
		{
			Link link = uiModPreview.Link;
			Delegate clicked = link.Clicked;
			Action b;
			if ((b = <>9__1) == null)
			{
				b = (<>9__1 = delegate()
				{
					this.OnModLinkClicked(steamWorkshopItem);
				});
			}
			link.Clicked = (Action)Delegate.Combine(clicked, b);
			this.UpdateMod(steamWorkshopItem);
		}));
		this.steamWorkshopItemTemplateContainerMap.Add(steamWorkshopItem, templateContainer);
		this.missingModsList.Add(templateContainer);
	}

	// Token: 0x06000FAE RID: 4014 RVA: 0x0004A1EC File Offset: 0x000483EC
	private void UpdateMod(SteamWorkshopItem steamWorkshopItem)
	{
		if (!this.steamWorkshopItemTemplateContainerMap.ContainsKey(steamWorkshopItem))
		{
			return;
		}
		ModPreview modPreview = this.steamWorkshopItemTemplateContainerMap[steamWorkshopItem].Query(null, null);
		modPreview.IsStatisticsVisible = (steamWorkshopItem.Details != null);
		SteamWorkshopItemDetails details = steamWorkshopItem.Details;
		modPreview.Subscriptions = ((details != null) ? details.Subscriptions : 0);
		SteamWorkshopItemDetails details2 = steamWorkshopItem.Details;
		modPreview.Upvotes = ((details2 != null) ? details2.Upvotes : 0);
		SteamWorkshopItemDetails details3 = steamWorkshopItem.Details;
		modPreview.Downvotes = ((details3 != null) ? details3.Downvotes : 0);
		Link link = modPreview.Link;
		SteamWorkshopItemDetails details4 = steamWorkshopItem.Details;
		link.Text = (((details4 != null) ? details4.Title : null) ?? steamWorkshopItem.Id.ToString());
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x0004A2A4 File Offset: 0x000484A4
	private void RemoveMod(SteamWorkshopItem steamWorkshopItem)
	{
		if (!this.steamWorkshopItemTemplateContainerMap.ContainsKey(steamWorkshopItem))
		{
			return;
		}
		TemplateContainer element = this.steamWorkshopItemTemplateContainerMap[steamWorkshopItem];
		this.missingModsList.Remove(element);
		this.steamWorkshopItemTemplateContainerMap.Remove(steamWorkshopItem);
	}

	// Token: 0x06000FB0 RID: 4016 RVA: 0x0004A2E8 File Offset: 0x000484E8
	private void OnSteamWorkshopItemStateChanged(string id, SteamWorkshopItemState oldState, SteamWorkshopItemState newState)
	{
		SteamWorkshopItem steamWorkshopItem = this.steamWorkshopItems.FirstOrDefault((SteamWorkshopItem item) => item.Id == id);
		if (steamWorkshopItem != null)
		{
			this.UpdateMod(steamWorkshopItem);
		}
	}

	// Token: 0x06000FB1 RID: 4017 RVA: 0x0004A324 File Offset: 0x00048524
	private void OnSteamWorkshopItemDetailsStateChanged(string id, SteamWorkshopItemDetailsState oldDetailsState, SteamWorkshopItemDetailsState newDetailsState)
	{
		SteamWorkshopItem steamWorkshopItem = this.steamWorkshopItems.FirstOrDefault((SteamWorkshopItem item) => item.Id == id);
		if (steamWorkshopItem != null)
		{
			this.UpdateMod(steamWorkshopItem);
		}
	}

	// Token: 0x06000FB2 RID: 4018 RVA: 0x0004A360 File Offset: 0x00048560
	private void OnSteamWorkshopItemPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		SteamWorkshopItem steamWorkshopItem = (SteamWorkshopItem)sender;
		this.UpdateMod(steamWorkshopItem);
	}

	// Token: 0x06000FB3 RID: 4019 RVA: 0x0004A37B File Offset: 0x0004857B
	private void OnModLinkClicked(SteamWorkshopItem steamWorkshopItem)
	{
		EventManager.TriggerEvent("Event_OnModPreviewLinkClicked", new Dictionary<string, object>
		{
			{
				"id",
				steamWorkshopItem.Id
			}
		});
	}

	// Token: 0x040009E4 RID: 2532
	private static readonly Logger Logger = new Logger("PopupMissingModsPopupContent");

	// Token: 0x040009E5 RID: 2533
	private string text;

	// Token: 0x040009E6 RID: 2534
	private string notice;

	// Token: 0x040009E7 RID: 2535
	private VisualTreeAsset modPreviewAsset;

	// Token: 0x040009E8 RID: 2536
	private Label textLabel;

	// Token: 0x040009E9 RID: 2537
	private Label noticeLabel;

	// Token: 0x040009EA RID: 2538
	private VisualElement missingModsList;

	// Token: 0x040009EB RID: 2539
	private SteamWorkshopItem[] steamWorkshopItems;

	// Token: 0x040009EC RID: 2540
	private Dictionary<SteamWorkshopItem, TemplateContainer> steamWorkshopItemTemplateContainerMap = new Dictionary<SteamWorkshopItem, TemplateContainer>();
}
