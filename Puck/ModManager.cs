using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

// Token: 0x020000F3 RID: 243
public static class ModManager
{
	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06000775 RID: 1909 RVA: 0x000279FF File Offset: 0x00025BFF
	public static Plugin[] ReadyPlugins
	{
		get
		{
			return (from plugin in ModManager.Plugins
			where plugin.IsReady
			select plugin).ToArray<Plugin>();
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000776 RID: 1910 RVA: 0x00027A2F File Offset: 0x00025C2F
	public static Plugin[] EnabledPlugins
	{
		get
		{
			return (from plugin in ModManager.Plugins
			where plugin.IsEnabled
			select plugin).ToArray<Plugin>();
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06000777 RID: 1911 RVA: 0x00027A5F File Offset: 0x00025C5F
	public static Mod[] ReadyMods
	{
		get
		{
			return (from mod in ModManager.Mods
			where mod.IsReady
			select mod).ToArray<Mod>();
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06000778 RID: 1912 RVA: 0x00027A8F File Offset: 0x00025C8F
	public static Mod[] EnabledMods
	{
		get
		{
			return (from mod in ModManager.Mods
			where mod.IsEnabled
			select mod).ToArray<Mod>();
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06000779 RID: 1913 RVA: 0x00027ABF File Offset: 0x00025CBF
	private static string pluginsDirectoryPath
	{
		get
		{
			return Path.Combine(Path.GetFullPath("."), "Plugins");
		}
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x00027AD5 File Offset: 0x00025CD5
	public static void Initialize()
	{
		ModManagerController.Initialize();
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00027ADC File Offset: 0x00025CDC
	public static void Dispose()
	{
		ModManagerController.Dispose();
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x00027AE4 File Offset: 0x00025CE4
	public static void LoadModsStatus()
	{
		try
		{
			string @string = SaveManager.GetString("modsStatus", null);
			if (string.IsNullOrEmpty(@string))
			{
				throw new Exception("No mods status found");
			}
			ModManager.ModsStatus = JsonSerializer.Deserialize<Dictionary<string, bool>>(@string, null);
		}
		catch (Exception ex)
		{
			ModManager.Logger.Error("Failed to load mods status: " + ex.Message);
			ModManager.SaveModsStatus();
			ModManager.LoadModsStatus();
		}
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x00027B54 File Offset: 0x00025D54
	public static bool GetModStatus(string id)
	{
		bool flag;
		return ModManager.ModsStatus.TryGetValue(id, out flag) && flag;
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x00027B73 File Offset: 0x00025D73
	public static void SetModStatus(string id, bool isEnabled)
	{
		ModManager.ModsStatus[id] = isEnabled;
		ModManager.SaveModsStatus();
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x00027B88 File Offset: 0x00025D88
	private static void SaveModsStatus()
	{
		try
		{
			string value = JsonSerializer.Serialize<Dictionary<string, bool>>(ModManager.ModsStatus, null);
			SaveManager.SetString("modsStatus", value);
		}
		catch (Exception ex)
		{
			ModManager.Logger.Error("Failed to save mods status: " + ex.Message);
		}
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x00027BDC File Offset: 0x00025DDC
	public static void ApplyModStatus()
	{
		foreach (Mod mod in ModManager.Mods)
		{
			if (ModManager.GetModStatus(mod.Id))
			{
				mod.Enable();
			}
			if (!ModManager.GetModStatus(mod.Id))
			{
				mod.Disable();
			}
		}
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x00027C50 File Offset: 0x00025E50
	public static void LoadPlugins()
	{
		ModManager.Logger.Info("Loading plugins from " + ModManager.pluginsDirectoryPath);
		if (!Directory.Exists(ModManager.pluginsDirectoryPath))
		{
			Directory.CreateDirectory(ModManager.pluginsDirectoryPath);
		}
		foreach (string path in Directory.GetDirectories(ModManager.pluginsDirectoryPath))
		{
			ModManager.AddPlugin(Path.GetFileName(path), path);
		}
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x00027CB8 File Offset: 0x00025EB8
	public static Plugin AddPlugin(string id, string path)
	{
		ModManager.Logger.Info("Adding plugin " + id);
		if (ModManager.GetPluginById(id) != null)
		{
			return null;
		}
		Plugin plugin = new Plugin(id, path);
		ModManager.Plugins.Add(plugin);
		EventManager.TriggerEvent("Event_OnPluginAdded", new Dictionary<string, object>
		{
			{
				"plugin",
				plugin
			}
		});
		plugin.Initialize();
		return plugin;
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x00027D1C File Offset: 0x00025F1C
	public static Plugin RemovePlugin(string id)
	{
		ModManager.Logger.Info("Removing plugin " + id);
		Plugin pluginById = ModManager.GetPluginById(id);
		if (pluginById == null)
		{
			return null;
		}
		ModManager.Plugins.Remove(pluginById);
		EventManager.TriggerEvent("Event_OnPluginRemoved", new Dictionary<string, object>
		{
			{
				"plugin",
				pluginById
			}
		});
		pluginById.Dispose();
		return pluginById;
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x00027D78 File Offset: 0x00025F78
	public static Mod AddMod(SteamWorkshopItem item)
	{
		ModManager.Logger.Info("Adding mod " + item.Id);
		if (ModManager.GetModById(item.Id) != null)
		{
			return null;
		}
		Mod mod = new Mod(item);
		ModManager.Mods.Add(mod);
		EventManager.TriggerEvent("Event_OnModAdded", new Dictionary<string, object>
		{
			{
				"mod",
				mod
			}
		});
		mod.Initialize();
		return mod;
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x00027DE4 File Offset: 0x00025FE4
	public static Mod RemoveMod(string id)
	{
		ModManager.Logger.Info("Removing mod " + id);
		Mod modById = ModManager.GetModById(id);
		if (modById == null)
		{
			return null;
		}
		ModManager.Mods.Remove(modById);
		EventManager.TriggerEvent("Event_OnModRemoved", new Dictionary<string, object>
		{
			{
				"mod",
				modById
			}
		});
		modById.Dispose();
		return modById;
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x00027E40 File Offset: 0x00026040
	public static Plugin GetPluginById(string id)
	{
		return ModManager.Plugins.Find((Plugin plugin) => plugin.Id == id);
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x00027E70 File Offset: 0x00026070
	public static Mod GetModById(string id)
	{
		return ModManager.Mods.Find((Mod mod) => mod.Id == id);
	}

	// Token: 0x040004C7 RID: 1223
	private static readonly Logger Logger = new Logger("ModManager");

	// Token: 0x040004C8 RID: 1224
	public static List<Plugin> Plugins = new List<Plugin>();

	// Token: 0x040004C9 RID: 1225
	public static List<Mod> Mods = new List<Mod>();

	// Token: 0x040004CA RID: 1226
	public static Dictionary<string, bool> ModsStatus = new Dictionary<string, bool>();
}
