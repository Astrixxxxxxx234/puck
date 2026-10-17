using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Token: 0x02000145 RID: 325
public class AdminManager : MonoBehaviourSingleton<AdminManager>
{
	// Token: 0x0600095E RID: 2398 RVA: 0x0002DE7C File Offset: 0x0002C07C
	public override void Awake()
	{
		base.Awake();
		this.adminSteamIdsFilePath = (Utils.GetCommandLineArgument("--adminSteamIdsPath", null) ?? "./admin_steam_ids.json");
		this.adminSteamIdsFilePath = Path.GetFullPath(this.adminSteamIdsFilePath);
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0002DEAF File Offset: 0x0002C0AF
	public void Dispose()
	{
		this.adminSteamIds.Clear();
		if (this.adminSteamIdsWatcher != null)
		{
			this.adminSteamIdsWatcher.Dispose();
			this.adminSteamIdsWatcher = null;
		}
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x0002DED8 File Offset: 0x0002C0D8
	public void LoadAdminSteamIds()
	{
		if (!File.Exists(this.adminSteamIdsFilePath))
		{
			AdminManager.Logger.Warning("Admin steam ids file not found at " + this.adminSteamIdsFilePath + ", creating default...");
			File.AppendAllText(this.adminSteamIdsFilePath, JsonSerializer.Serialize<List<string>>(new List<string>(), new JsonSerializerOptions
			{
				WriteIndented = true
			}));
		}
		this.ReadAdminSteamIds();
		this.WatchAdminSteamIds(this.adminSteamIdsFilePath);
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x0002DF44 File Offset: 0x0002C144
	public void SaveAdminSteamIds()
	{
		File.WriteAllText(this.adminSteamIdsFilePath, JsonSerializer.Serialize<List<string>>(this.adminSteamIds, new JsonSerializerOptions
		{
			WriteIndented = true
		}));
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x0002DF68 File Offset: 0x0002C168
	public void ReadAdminSteamIds()
	{
		List<string> list;
		if (ConfigUtils.TryLoadFromFile<List<string>>(this.adminSteamIdsFilePath, out list))
		{
			this.adminSteamIds = (list ?? new List<string>());
			return;
		}
		AdminManager.Logger.Error("Could not load admin Steam IDs (see the error above); keeping the previous list.");
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x0002DFA4 File Offset: 0x0002C1A4
	public void WatchAdminSteamIds(string adminSteamIdsFilePath)
	{
		if (this.adminSteamIdsWatcher != null)
		{
			return;
		}
		string fileName = Path.GetFileName(adminSteamIdsFilePath);
		string path = adminSteamIdsFilePath.Replace(fileName, string.Empty);
		this.adminSteamIdsWatcher = new FileSystemWatcher(path);
		this.adminSteamIdsWatcher.NotifyFilter = NotifyFilters.LastWrite;
		this.adminSteamIdsWatcher.Filter = fileName;
		this.adminSteamIdsWatcher.EnableRaisingEvents = true;
		this.adminSteamIdsWatcher.Changed += this.OnAdminSteamIdsFileChanged;
		AdminManager.Logger.Info("Watching admin Steam IDs file " + fileName);
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x0002E02C File Offset: 0x0002C22C
	private void OnAdminSteamIdsFileChanged(object sender, FileSystemEventArgs e)
	{
		AdminManager.Logger.Info("Admin Steam IDs file changed: " + e.FullPath);
		List<string> list;
		if (ConfigUtils.TryLoadFromFile<List<string>>(e.FullPath, out list))
		{
			this.adminSteamIds = (list ?? new List<string>());
		}
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x0002E072 File Offset: 0x0002C272
	public void AddAdminSteamId(string steamId)
	{
		if (this.adminSteamIds.Contains(steamId))
		{
			return;
		}
		this.adminSteamIds.Add(steamId);
		this.SaveAdminSteamIds();
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x0002E095 File Offset: 0x0002C295
	public void RemoveAdminSteamId(string steamId)
	{
		if (!this.adminSteamIds.Contains(steamId))
		{
			return;
		}
		this.adminSteamIds.Remove(steamId);
		this.SaveAdminSteamIds();
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x0002E0B9 File Offset: 0x0002C2B9
	public bool IsSteamIdAdmin(string steamId)
	{
		return this.adminSteamIds.Contains(steamId);
	}

	// Token: 0x040005D7 RID: 1495
	private static readonly Logger Logger = new Logger("AdminManager");

	// Token: 0x040005D8 RID: 1496
	private List<string> adminSteamIds = new List<string>();

	// Token: 0x040005D9 RID: 1497
	private string adminSteamIdsFilePath;

	// Token: 0x040005DA RID: 1498
	private FileSystemWatcher adminSteamIdsWatcher;
}
