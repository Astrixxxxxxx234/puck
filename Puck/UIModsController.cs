using System;
using System.Collections.Generic;

// Token: 0x020001E4 RID: 484
public class UIModsController : UIViewController<UIMods>
{
	// Token: 0x06000EED RID: 3821 RVA: 0x00046F60 File Offset: 0x00045160
	public override void Awake()
	{
		base.Awake();
		this.uiMods = base.GetComponent<UIMods>();
		EventManager.AddEventListener("Event_OnPluginAdded", new Action<Dictionary<string, object>>(this.Event_OnPluginAdded));
		EventManager.AddEventListener("Event_OnPluginStateChanged", new Action<Dictionary<string, object>>(this.Event_OnPluginStateChanged));
		EventManager.AddEventListener("Event_OnPluginEnableFailed", new Action<Dictionary<string, object>>(this.Event_OnPluginEnableFailed));
		EventManager.AddEventListener("Event_OnPluginDisableFailed", new Action<Dictionary<string, object>>(this.Event_OnPluginDisableFailed));
		EventManager.AddEventListener("Event_OnPluginRemoved", new Action<Dictionary<string, object>>(this.Event_OnPluginRemoved));
		EventManager.AddEventListener("Event_OnModAdded", new Action<Dictionary<string, object>>(this.Event_OnModAdded));
		EventManager.AddEventListener("Event_OnModStateChanged", new Action<Dictionary<string, object>>(this.Event_OnModStateChanged));
		EventManager.AddEventListener("Event_OnModSteamWorkshopItemStateChanged", new Action<Dictionary<string, object>>(this.Event_OnModSteamWorkshopItemStateChanged));
		EventManager.AddEventListener("Event_OnModSteamWorkshopItemDetailsStateChanged", new Action<Dictionary<string, object>>(this.Event_OnModSteamWorkshopItemDetailsStateChanged));
		EventManager.AddEventListener("Event_OnModEnableFailed", new Action<Dictionary<string, object>>(this.Event_OnModEnableFailed));
		EventManager.AddEventListener("Event_OnModDisableFailed", new Action<Dictionary<string, object>>(this.Event_OnModDisableFailed));
		EventManager.AddEventListener("Event_OnModRemoved", new Action<Dictionary<string, object>>(this.Event_OnModRemoved));
		EventManager.AddEventListener("Event_OnModsShow", new Action<Dictionary<string, object>>(this.Event_OnModsShow));
	}

	// Token: 0x06000EEE RID: 3822 RVA: 0x0004709D File Offset: 0x0004529D
	private void Start()
	{
		ModManager.Plugins.ForEach(delegate(Plugin plugin)
		{
			this.uiMods.AddPlugin(plugin);
		});
		ModManager.Mods.ForEach(delegate(Mod mod)
		{
			this.uiMods.AddMod(mod);
		});
	}

	// Token: 0x06000EEF RID: 3823 RVA: 0x000470CC File Offset: 0x000452CC
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnPluginAdded", new Action<Dictionary<string, object>>(this.Event_OnPluginAdded));
		EventManager.RemoveEventListener("Event_OnPluginStateChanged", new Action<Dictionary<string, object>>(this.Event_OnPluginStateChanged));
		EventManager.RemoveEventListener("Event_OnPluginEnableFailed", new Action<Dictionary<string, object>>(this.Event_OnPluginEnableFailed));
		EventManager.RemoveEventListener("Event_OnPluginDisableFailed", new Action<Dictionary<string, object>>(this.Event_OnPluginDisableFailed));
		EventManager.RemoveEventListener("Event_OnPluginRemoved", new Action<Dictionary<string, object>>(this.Event_OnPluginRemoved));
		EventManager.RemoveEventListener("Event_OnModAdded", new Action<Dictionary<string, object>>(this.Event_OnModAdded));
		EventManager.RemoveEventListener("Event_OnModStateChanged", new Action<Dictionary<string, object>>(this.Event_OnModStateChanged));
		EventManager.RemoveEventListener("Event_OnModSteamWorkshopItemStateChanged", new Action<Dictionary<string, object>>(this.Event_OnModSteamWorkshopItemStateChanged));
		EventManager.RemoveEventListener("Event_OnModSteamWorkshopItemDetailsStateChanged", new Action<Dictionary<string, object>>(this.Event_OnModSteamWorkshopItemDetailsStateChanged));
		EventManager.RemoveEventListener("Event_OnModEnableFailed", new Action<Dictionary<string, object>>(this.Event_OnModEnableFailed));
		EventManager.RemoveEventListener("Event_OnModDisableFailed", new Action<Dictionary<string, object>>(this.Event_OnModDisableFailed));
		EventManager.RemoveEventListener("Event_OnModRemoved", new Action<Dictionary<string, object>>(this.Event_OnModRemoved));
		EventManager.RemoveEventListener("Event_OnModsShow", new Action<Dictionary<string, object>>(this.Event_OnModsShow));
		base.OnDestroy();
	}

	// Token: 0x06000EF0 RID: 3824 RVA: 0x000471FD File Offset: 0x000453FD
	private void Event_OnModsShow(Dictionary<string, object> message)
	{
		ModManager.Plugins.ForEach(delegate(Plugin plugin)
		{
			this.uiMods.UpdatePlugin(plugin);
		});
		ModManager.Mods.ForEach(delegate(Mod mod)
		{
			this.uiMods.UpdateMod(mod);
		});
	}

	// Token: 0x06000EF1 RID: 3825 RVA: 0x0004722C File Offset: 0x0004542C
	private void Event_OnPluginAdded(Dictionary<string, object> message)
	{
		Plugin plugin = (Plugin)message["plugin"];
		this.uiMods.AddPlugin(plugin);
	}

	// Token: 0x06000EF2 RID: 3826 RVA: 0x00047258 File Offset: 0x00045458
	private void Event_OnPluginStateChanged(Dictionary<string, object> message)
	{
		Plugin plugin = (Plugin)message["plugin"];
		this.uiMods.UpdatePlugin(plugin);
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x00047284 File Offset: 0x00045484
	private void Event_OnPluginEnableFailed(Dictionary<string, object> message)
	{
		Plugin plugin = (Plugin)message["plugin"];
		this.uiMods.UpdatePlugin(plugin);
	}

	// Token: 0x06000EF4 RID: 3828 RVA: 0x000472B0 File Offset: 0x000454B0
	private void Event_OnPluginDisableFailed(Dictionary<string, object> message)
	{
		Plugin plugin = (Plugin)message["plugin"];
		this.uiMods.UpdatePlugin(plugin);
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x000472DC File Offset: 0x000454DC
	private void Event_OnPluginRemoved(Dictionary<string, object> message)
	{
		Plugin plugin = (Plugin)message["plugin"];
		this.uiMods.RemovePlugin(plugin);
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x00047308 File Offset: 0x00045508
	private void Event_OnModAdded(Dictionary<string, object> message)
	{
		Mod mod = (Mod)message["mod"];
		this.uiMods.AddMod(mod);
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x00047334 File Offset: 0x00045534
	private void Event_OnModStateChanged(Dictionary<string, object> message)
	{
		Mod mod = (Mod)message["mod"];
		this.uiMods.UpdateMod(mod);
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x00047360 File Offset: 0x00045560
	private void Event_OnModSteamWorkshopItemStateChanged(Dictionary<string, object> message)
	{
		Mod mod = (Mod)message["mod"];
		this.uiMods.UpdateMod(mod);
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x0004738C File Offset: 0x0004558C
	private void Event_OnModSteamWorkshopItemDetailsStateChanged(Dictionary<string, object> message)
	{
		Mod mod = (Mod)message["mod"];
		this.uiMods.UpdateMod(mod);
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x000473B8 File Offset: 0x000455B8
	private void Event_OnModEnableFailed(Dictionary<string, object> message)
	{
		Mod mod = (Mod)message["mod"];
		this.uiMods.UpdateMod(mod);
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x000473E4 File Offset: 0x000455E4
	private void Event_OnModDisableFailed(Dictionary<string, object> message)
	{
		Mod mod = (Mod)message["mod"];
		this.uiMods.UpdateMod(mod);
	}

	// Token: 0x06000EFC RID: 3836 RVA: 0x00047410 File Offset: 0x00045610
	private void Event_OnModRemoved(Dictionary<string, object> message)
	{
		Mod mod = (Mod)message["mod"];
		this.uiMods.RemoveMod(mod);
	}

	// Token: 0x04000973 RID: 2419
	private UIMods uiMods;
}
