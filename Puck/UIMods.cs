using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x020001E1 RID: 481
public class UIMods : UIView
{
	// Token: 0x06000ED5 RID: 3797 RVA: 0x00046724 File Offset: 0x00044924
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("ModsView", null);
		this.mods = base.View.Query("Mods", null);
		this.modsList = this.mods.Query("ModsList", null);
		this.noMods = this.mods.Query("NoMods", null);
		this.closeIconButton = this.mods.Query("CloseIconButtonContainer", null).First().Query(null, null);
		this.closeIconButton.clicked += this.OnClickClose;
		this.findModsButton = this.mods.Query("FindModsButton", null);
		this.findModsButton.clicked += this.OnClickFindMods;
		this.refreshButton = this.mods.Query("RefreshButton", null);
		this.refreshButton.clicked += this.OnClickRefresh;
		this.modsList.Clear();
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x0004684F File Offset: 0x00044A4F
	public override bool Show()
	{
		bool flag = base.Show();
		if (flag)
		{
			EventManager.TriggerEvent("Event_OnModsShow", null);
		}
		return flag;
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x00046868 File Offset: 0x00044A68
	public void AddPlugin(Plugin plugin)
	{
		if (this.pluginTemplateContainerMap.ContainsKey(plugin))
		{
			return;
		}
		TemplateContainer templateContainer = this.modAsset.Instantiate();
		UI.Mod uiMod = templateContainer.Query(null, null);
		UI.Mod uiMod2 = uiMod;
		EventCallback<ChangeEvent<bool>> <>9__1;
		uiMod2.Ready = (Action)Delegate.Combine(uiMod2.Ready, new Action(delegate()
		{
			uiMod.Toggle.value = plugin.IsEnabled;
			INotifyValueChanged<bool> toggle = uiMod.Toggle;
			EventCallback<ChangeEvent<bool>> callback;
			if ((callback = <>9__1) == null)
			{
				callback = (<>9__1 = delegate(ChangeEvent<bool> e)
				{
					this.OnPluginToggleChanged(plugin, e.newValue);
				});
			}
			toggle.RegisterValueChangedCallback(callback);
			this.UpdatePlugin(plugin);
		}));
		this.pluginTemplateContainerMap.Add(plugin, templateContainer);
		this.modsList.Add(templateContainer);
		this.UpdateNoMods();
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x00046908 File Offset: 0x00044B08
	public void UpdatePlugin(Plugin plugin)
	{
		if (!this.pluginTemplateContainerMap.ContainsKey(plugin))
		{
			return;
		}
		UI.Mod mod = this.pluginTemplateContainerMap[plugin].Query(null, null);
		mod.Description = string.Empty;
		mod.PreviewTexture = this.defaultPreviewTexture;
		mod.HasFiles = true;
		mod.Toggle.enabledSelf = plugin.HasAssembly;
		mod.Toggle.value = plugin.IsEnabled;
		mod.ModPreview.Link.enabledSelf = false;
		mod.ModPreview.Link.Text = plugin.Id;
		mod.ModPreview.IsStatisticsVisible = false;
		mod.Refresh();
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x000469B4 File Offset: 0x00044BB4
	public void RemovePlugin(Plugin plugin)
	{
		if (!this.pluginTemplateContainerMap.ContainsKey(plugin))
		{
			return;
		}
		TemplateContainer element = this.pluginTemplateContainerMap[plugin];
		this.modsList.Remove(element);
		this.pluginTemplateContainerMap.Remove(plugin);
		this.UpdateNoMods();
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x000469FC File Offset: 0x00044BFC
	public void AddMod(global::Mod mod)
	{
		if (this.modTemplateContainerMap.ContainsKey(mod))
		{
			return;
		}
		TemplateContainer templateContainer = this.modAsset.Instantiate();
		UI.Mod uiMod = templateContainer.Query(null, null);
		UI.Mod uiMod2 = uiMod;
		EventCallback<ChangeEvent<bool>> <>9__1;
		Action <>9__2;
		uiMod2.Ready = (Action)Delegate.Combine(uiMod2.Ready, new Action(delegate()
		{
			uiMod.Toggle.value = mod.IsEnabled;
			INotifyValueChanged<bool> toggle = uiMod.Toggle;
			EventCallback<ChangeEvent<bool>> callback;
			if ((callback = <>9__1) == null)
			{
				callback = (<>9__1 = delegate(ChangeEvent<bool> e)
				{
					this.OnModToggleChanged(mod, e.newValue);
				});
			}
			toggle.RegisterValueChangedCallback(callback);
			Link link = uiMod.ModPreview.Link;
			Delegate clicked = link.Clicked;
			Action b;
			if ((b = <>9__2) == null)
			{
				b = (<>9__2 = delegate()
				{
					this.OnModPreviewLinkClicked(mod);
				});
			}
			link.Clicked = (Action)Delegate.Combine(clicked, b);
			this.UpdateMod(mod);
		}));
		this.modTemplateContainerMap.Add(mod, templateContainer);
		this.modsList.Add(templateContainer);
		this.UpdateNoMods();
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x00046A9C File Offset: 0x00044C9C
	public void UpdateMod(global::Mod mod)
	{
		if (!this.modTemplateContainerMap.ContainsKey(mod))
		{
			return;
		}
		UI.Mod mod2 = this.modTemplateContainerMap[mod].Query(null, null);
		SteamWorkshopItem steamWorkshopItem = mod.SteamWorkshopItem;
		string text;
		if (steamWorkshopItem == null)
		{
			text = null;
		}
		else
		{
			SteamWorkshopItemDetails details = steamWorkshopItem.Details;
			text = ((details != null) ? details.Description : null);
		}
		mod2.Description = (text ?? string.Empty);
		SteamWorkshopItem steamWorkshopItem2 = mod.SteamWorkshopItem;
		Texture2D texture2D;
		if (steamWorkshopItem2 == null)
		{
			texture2D = null;
		}
		else
		{
			SteamWorkshopItemDetails details2 = steamWorkshopItem2.Details;
			texture2D = ((details2 != null) ? details2.PreviewTexture : null);
		}
		Texture2D texture2D2 = texture2D;
		mod2.PreviewTexture = ((texture2D2 != null) ? texture2D2 : this.defaultPreviewTexture);
		mod2.HasFiles = mod.HasLocalFiles;
		mod2.Toggle.enabledSelf = mod.HasAssembly;
		mod2.Toggle.value = mod.IsEnabled;
		Link link = mod2.ModPreview.Link;
		SteamWorkshopItem steamWorkshopItem3 = mod.SteamWorkshopItem;
		string text2;
		if (steamWorkshopItem3 == null)
		{
			text2 = null;
		}
		else
		{
			SteamWorkshopItemDetails details3 = steamWorkshopItem3.Details;
			text2 = ((details3 != null) ? details3.Title : null);
		}
		link.Text = (text2 ?? mod.Id);
		ModPreview modPreview = mod2.ModPreview;
		SteamWorkshopItem steamWorkshopItem4 = mod.SteamWorkshopItem;
		modPreview.IsStatisticsVisible = (((steamWorkshopItem4 != null) ? steamWorkshopItem4.Details : null) != null);
		ModPreview modPreview2 = mod2.ModPreview;
		SteamWorkshopItem steamWorkshopItem5 = mod.SteamWorkshopItem;
		int? num;
		if (steamWorkshopItem5 == null)
		{
			num = null;
		}
		else
		{
			SteamWorkshopItemDetails details4 = steamWorkshopItem5.Details;
			num = ((details4 != null) ? new int?(details4.Subscriptions) : null);
		}
		int? num2 = num;
		modPreview2.Subscriptions = num2.GetValueOrDefault();
		ModPreview modPreview3 = mod2.ModPreview;
		SteamWorkshopItem steamWorkshopItem6 = mod.SteamWorkshopItem;
		int? num3;
		if (steamWorkshopItem6 == null)
		{
			num3 = null;
		}
		else
		{
			SteamWorkshopItemDetails details5 = steamWorkshopItem6.Details;
			num3 = ((details5 != null) ? new int?(details5.Upvotes) : null);
		}
		num2 = num3;
		modPreview3.Upvotes = num2.GetValueOrDefault();
		ModPreview modPreview4 = mod2.ModPreview;
		SteamWorkshopItem steamWorkshopItem7 = mod.SteamWorkshopItem;
		int? num4;
		if (steamWorkshopItem7 == null)
		{
			num4 = null;
		}
		else
		{
			SteamWorkshopItemDetails details6 = steamWorkshopItem7.Details;
			num4 = ((details6 != null) ? new int?(details6.Downvotes) : null);
		}
		num2 = num4;
		modPreview4.Downvotes = num2.GetValueOrDefault();
		mod2.Refresh();
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x00046C90 File Offset: 0x00044E90
	public void RemoveMod(global::Mod mod)
	{
		if (!this.modTemplateContainerMap.ContainsKey(mod))
		{
			return;
		}
		TemplateContainer element = this.modTemplateContainerMap[mod];
		this.modsList.Remove(element);
		this.modTemplateContainerMap.Remove(mod);
		this.UpdateNoMods();
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x00046CD8 File Offset: 0x00044ED8
	private void UpdateNoMods()
	{
		this.noMods.style.display = ((this.modTemplateContainerMap.Count > 0 || this.pluginTemplateContainerMap.Count > 0) ? DisplayStyle.None : DisplayStyle.Flex);
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x00046D0F File Offset: 0x00044F0F
	private void OnClickClose()
	{
		EventManager.TriggerEvent("Event_OnModsClickClose", null);
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x00046D1C File Offset: 0x00044F1C
	private void OnClickFindMods()
	{
		EventManager.TriggerEvent("Event_OnModsClickFindMods", null);
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x00046D29 File Offset: 0x00044F29
	private void OnClickRefresh()
	{
		EventManager.TriggerEvent("Event_OnModsClickRefresh", null);
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x00046D36 File Offset: 0x00044F36
	private void OnModToggleChanged(global::Mod mod, bool value)
	{
		if (value)
		{
			EventManager.TriggerEvent("Event_OnModsModEnabled", new Dictionary<string, object>
			{
				{
					"mod",
					mod
				}
			});
			return;
		}
		EventManager.TriggerEvent("Event_OnModsModDisabled", new Dictionary<string, object>
		{
			{
				"mod",
				mod
			}
		});
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x00046D72 File Offset: 0x00044F72
	private void OnModPreviewLinkClicked(global::Mod mod)
	{
		EventManager.TriggerEvent("Event_OnModPreviewLinkClicked", new Dictionary<string, object>
		{
			{
				"id",
				mod.Id
			}
		});
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x00046D94 File Offset: 0x00044F94
	private void OnPluginToggleChanged(Plugin plugin, bool value)
	{
		if (value)
		{
			EventManager.TriggerEvent("Event_OnModsPluginEnabled", new Dictionary<string, object>
			{
				{
					"plugin",
					plugin
				}
			});
			return;
		}
		EventManager.TriggerEvent("Event_OnModsPluginDisabled", new Dictionary<string, object>
		{
			{
				"plugin",
				plugin
			}
		});
	}

	// Token: 0x0400095F RID: 2399
	private static readonly global::Logger Logger = new global::Logger("UIMods");

	// Token: 0x04000960 RID: 2400
	[Header("References")]
	[SerializeField]
	private VisualTreeAsset modAsset;

	// Token: 0x04000961 RID: 2401
	[SerializeField]
	private StyleBackground defaultPreviewTexture;

	// Token: 0x04000962 RID: 2402
	private VisualElement mods;

	// Token: 0x04000963 RID: 2403
	private VisualElement modsList;

	// Token: 0x04000964 RID: 2404
	private VisualElement noMods;

	// Token: 0x04000965 RID: 2405
	private IconButton closeIconButton;

	// Token: 0x04000966 RID: 2406
	private Button findModsButton;

	// Token: 0x04000967 RID: 2407
	private Button refreshButton;

	// Token: 0x04000968 RID: 2408
	private Dictionary<global::Mod, TemplateContainer> modTemplateContainerMap = new Dictionary<global::Mod, TemplateContainer>();

	// Token: 0x04000969 RID: 2409
	private Dictionary<Plugin, TemplateContainer> pluginTemplateContainerMap = new Dictionary<Plugin, TemplateContainer>();
}
