using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Token: 0x02000147 RID: 327
public class BanManager : MonoBehaviourSingleton<BanManager>
{
	// Token: 0x06000970 RID: 2416 RVA: 0x0002E170 File Offset: 0x0002C370
	public override void Awake()
	{
		base.Awake();
		this.bannedSteamIdsFilePath = (Utils.GetCommandLineArgument("--bannedSteamIdsPath", null) ?? "./banned_steam_ids.json");
		this.bannedSteamIdsFilePath = Path.GetFullPath(this.bannedSteamIdsFilePath);
		this.bannedIpAddressesFilePath = (Utils.GetCommandLineArgument("--bannedIpAddressesPath", null) ?? "./banned_ip_addresses.json");
		this.bannedIpAddressesFilePath = Path.GetFullPath(this.bannedIpAddressesFilePath);
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x0002E1DC File Offset: 0x0002C3DC
	public void Dispose()
	{
		this.bannedSteamIds.Clear();
		if (this.bannedSteamIdsWatcher != null)
		{
			this.bannedSteamIdsWatcher.Dispose();
			this.bannedSteamIdsWatcher = null;
		}
		this.bannedIpAddresses.Clear();
		if (this.bannedIpAddressesWatcher != null)
		{
			this.bannedIpAddressesWatcher.Dispose();
			this.bannedIpAddressesWatcher = null;
		}
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0002E234 File Offset: 0x0002C434
	public void LoadBannedSteamIds()
	{
		if (!File.Exists(this.bannedSteamIdsFilePath))
		{
			BanManager.Logger.Warning("Banned steam ids file not found at " + this.bannedSteamIdsFilePath + ", creating default...");
			File.AppendAllText(this.bannedSteamIdsFilePath, JsonSerializer.Serialize<List<string>>(new List<string>(), new JsonSerializerOptions
			{
				WriteIndented = true
			}));
		}
		this.ReadBannedSteamIds();
		this.WatchBannedSteamIds(this.bannedSteamIdsFilePath);
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x0002E2A0 File Offset: 0x0002C4A0
	public void SaveBannedSteamIds()
	{
		File.WriteAllText(this.bannedSteamIdsFilePath, JsonSerializer.Serialize<List<string>>(this.bannedSteamIds, new JsonSerializerOptions
		{
			WriteIndented = true
		}));
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x0002E2C4 File Offset: 0x0002C4C4
	public void ReadBannedSteamIds()
	{
		List<string> list;
		if (ConfigUtils.TryLoadFromFile<List<string>>(this.bannedSteamIdsFilePath, out list))
		{
			this.bannedSteamIds = (list ?? new List<string>());
			return;
		}
		BanManager.Logger.Error("Could not load banned Steam IDs (see the error above); keeping the previous list.");
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x0002E300 File Offset: 0x0002C500
	public void WatchBannedSteamIds(string bannedSteamIdsFilePath)
	{
		if (this.bannedSteamIdsWatcher != null)
		{
			return;
		}
		string fileName = Path.GetFileName(bannedSteamIdsFilePath);
		string path = bannedSteamIdsFilePath.Replace(fileName, string.Empty);
		this.bannedSteamIdsWatcher = new FileSystemWatcher(path);
		this.bannedSteamIdsWatcher.NotifyFilter = NotifyFilters.LastWrite;
		this.bannedSteamIdsWatcher.Filter = fileName;
		this.bannedSteamIdsWatcher.EnableRaisingEvents = true;
		this.bannedSteamIdsWatcher.Changed += this.OnBannedSteamIdsFileChanged;
		BanManager.Logger.Info("Watching banned Steam IDs file " + fileName);
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x0002E387 File Offset: 0x0002C587
	public void AddBannedSteamId(string steamId)
	{
		if (this.bannedSteamIds.Contains(steamId))
		{
			return;
		}
		this.bannedSteamIds.Add(steamId);
		this.SaveBannedSteamIds();
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0002E3AA File Offset: 0x0002C5AA
	public void RemoveBannedSteamId(string steamId)
	{
		if (!this.bannedSteamIds.Contains(steamId))
		{
			return;
		}
		this.bannedSteamIds.Remove(steamId);
		this.SaveBannedSteamIds();
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x0002E3CE File Offset: 0x0002C5CE
	public bool IsSteamIdBanned(string steamId)
	{
		return this.bannedSteamIds.Contains(steamId);
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x0002E3DC File Offset: 0x0002C5DC
	public void LoadBannedIpAddresses()
	{
		if (!File.Exists(this.bannedIpAddressesFilePath))
		{
			BanManager.Logger.Warning("Banned IP addresses file not found at " + this.bannedIpAddressesFilePath + ", creating default...");
			File.AppendAllText(this.bannedIpAddressesFilePath, JsonSerializer.Serialize<List<string>>(new List<string>(), new JsonSerializerOptions
			{
				WriteIndented = true
			}));
		}
		this.ReadBannedIpAddresses();
		this.WatchBannedIpAddresses(this.bannedIpAddressesFilePath);
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x0002E448 File Offset: 0x0002C648
	public void SaveBannedIpAddresses()
	{
		File.WriteAllText(this.bannedIpAddressesFilePath, JsonSerializer.Serialize<List<string>>(this.bannedIpAddresses, new JsonSerializerOptions
		{
			WriteIndented = true
		}));
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x0002E46C File Offset: 0x0002C66C
	public void ReadBannedIpAddresses()
	{
		List<string> list;
		if (ConfigUtils.TryLoadFromFile<List<string>>(this.bannedIpAddressesFilePath, out list))
		{
			this.bannedIpAddresses = (list ?? new List<string>());
			return;
		}
		BanManager.Logger.Error("Could not load banned IP addresses (see the error above); keeping the previous list.");
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x0002E4A8 File Offset: 0x0002C6A8
	public void WatchBannedIpAddresses(string bannedIpAddressesFilePath)
	{
		if (this.bannedIpAddressesWatcher != null)
		{
			return;
		}
		string fileName = Path.GetFileName(bannedIpAddressesFilePath);
		string path = bannedIpAddressesFilePath.Replace(fileName, string.Empty);
		this.bannedIpAddressesWatcher = new FileSystemWatcher(path);
		this.bannedIpAddressesWatcher.NotifyFilter = NotifyFilters.LastWrite;
		this.bannedIpAddressesWatcher.Filter = fileName;
		this.bannedIpAddressesWatcher.Changed += this.OnBannedIpAddressesFileChanged;
		this.bannedIpAddressesWatcher.EnableRaisingEvents = true;
		BanManager.Logger.Info("Watching banned IP addresses file " + fileName);
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x0002E52F File Offset: 0x0002C72F
	public void AddBannedIpAddress(string ipAddress)
	{
		if (this.bannedIpAddresses.Contains(ipAddress))
		{
			return;
		}
		this.bannedIpAddresses.Add(ipAddress);
		this.SaveBannedIpAddresses();
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x0002E552 File Offset: 0x0002C752
	public void RemoveBannedIpAddress(string ipAddress)
	{
		if (!this.bannedIpAddresses.Contains(ipAddress))
		{
			return;
		}
		this.bannedIpAddresses.Remove(ipAddress);
		this.SaveBannedIpAddresses();
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x0002E576 File Offset: 0x0002C776
	public bool IsIpAddressBanned(string ipAddress)
	{
		return this.bannedIpAddresses.Contains(ipAddress);
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x0002E584 File Offset: 0x0002C784
	private void OnBannedSteamIdsFileChanged(object sender, FileSystemEventArgs e)
	{
		BanManager.Logger.Info("Banned Steam IDs file changed: " + e.FullPath);
		List<string> list;
		if (ConfigUtils.TryLoadFromFile<List<string>>(e.FullPath, out list))
		{
			this.bannedSteamIds = (list ?? new List<string>());
		}
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x0002E5CC File Offset: 0x0002C7CC
	private void OnBannedIpAddressesFileChanged(object sender, FileSystemEventArgs e)
	{
		BanManager.Logger.Info("Banned IP addresses file changed: " + e.FullPath);
		List<string> list;
		if (ConfigUtils.TryLoadFromFile<List<string>>(e.FullPath, out list))
		{
			this.bannedIpAddresses = (list ?? new List<string>());
		}
	}

	// Token: 0x040005DC RID: 1500
	private static readonly Logger Logger = new Logger("BanManager");

	// Token: 0x040005DD RID: 1501
	private List<string> bannedSteamIds = new List<string>();

	// Token: 0x040005DE RID: 1502
	private string bannedSteamIdsFilePath;

	// Token: 0x040005DF RID: 1503
	private FileSystemWatcher bannedSteamIdsWatcher;

	// Token: 0x040005E0 RID: 1504
	private List<string> bannedIpAddresses = new List<string>();

	// Token: 0x040005E1 RID: 1505
	private string bannedIpAddressesFilePath;

	// Token: 0x040005E2 RID: 1506
	private FileSystemWatcher bannedIpAddressesWatcher;
}
