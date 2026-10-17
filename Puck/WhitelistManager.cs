using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Token: 0x0200015B RID: 347
public class WhitelistManager : MonoBehaviourSingleton<WhitelistManager>
{
	// Token: 0x06000A3B RID: 2619 RVA: 0x00030ED2 File Offset: 0x0002F0D2
	public override void Awake()
	{
		base.Awake();
		this.whitelistedSteamIdsFilePath = (Utils.GetCommandLineArgument("--whitelistedSteamIdsPath", null) ?? "./whitelisted_steam_ids.json");
		this.whitelistedSteamIdsFilePath = Path.GetFullPath(this.whitelistedSteamIdsFilePath);
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x00030F05 File Offset: 0x0002F105
	public void Dispose()
	{
		this.whitelistedSteamIds.Clear();
		if (this.whitelistedSteamIdsWatcher != null)
		{
			this.whitelistedSteamIdsWatcher.Dispose();
			this.whitelistedSteamIdsWatcher = null;
		}
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x00030F2C File Offset: 0x0002F12C
	public void LoadWhitelistedSteamIds()
	{
		if (!File.Exists(this.whitelistedSteamIdsFilePath))
		{
			WhitelistManager.Logger.Warning("Whitelisted Steam IDs file not found at " + this.whitelistedSteamIdsFilePath + ", creating default...");
			File.AppendAllText(this.whitelistedSteamIdsFilePath, JsonSerializer.Serialize<List<string>>(new List<string>(), new JsonSerializerOptions
			{
				WriteIndented = true
			}));
		}
		this.ReadWhitelistedSteamIds();
		this.WatchWhitelistedSteamIds(this.whitelistedSteamIdsFilePath);
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x00030F98 File Offset: 0x0002F198
	public void SaveWhitelistedSteamIds()
	{
		File.WriteAllText(this.whitelistedSteamIdsFilePath, JsonSerializer.Serialize<List<string>>(this.whitelistedSteamIds, new JsonSerializerOptions
		{
			WriteIndented = true
		}));
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x00030FBC File Offset: 0x0002F1BC
	public void ReadWhitelistedSteamIds()
	{
		List<string> list;
		if (ConfigUtils.TryLoadFromFile<List<string>>(this.whitelistedSteamIdsFilePath, out list))
		{
			this.whitelistedSteamIds = (list ?? new List<string>());
			return;
		}
		WhitelistManager.Logger.Error("Could not load whitelisted Steam IDs (see the error above); keeping the previous list.");
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x00030FF8 File Offset: 0x0002F1F8
	public void WatchWhitelistedSteamIds(string whitelistedSteamIdsFilePath)
	{
		if (this.whitelistedSteamIdsWatcher != null)
		{
			return;
		}
		string fileName = Path.GetFileName(whitelistedSteamIdsFilePath);
		string path = whitelistedSteamIdsFilePath.Replace(fileName, string.Empty);
		this.whitelistedSteamIdsWatcher = new FileSystemWatcher(path);
		this.whitelistedSteamIdsWatcher.NotifyFilter = NotifyFilters.LastWrite;
		this.whitelistedSteamIdsWatcher.Filter = fileName;
		this.whitelistedSteamIdsWatcher.EnableRaisingEvents = true;
		this.whitelistedSteamIdsWatcher.Changed += this.OnWhitelistedSteamIdsFileChanged;
		WhitelistManager.Logger.Info("Watching whitelisted Steam IDs file " + fileName);
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x0003107F File Offset: 0x0002F27F
	public void AddWhitelistedSteamId(string steamId)
	{
		if (this.whitelistedSteamIds.Contains(steamId))
		{
			return;
		}
		this.whitelistedSteamIds.Add(steamId);
		this.SaveWhitelistedSteamIds();
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x000310A4 File Offset: 0x0002F2A4
	public void AddWhitelistedSteamIds(params string[] steamIds)
	{
		bool flag = false;
		foreach (string item in steamIds)
		{
			if (!this.whitelistedSteamIds.Contains(item))
			{
				this.whitelistedSteamIds.Add(item);
				flag = true;
			}
		}
		if (flag)
		{
			this.SaveWhitelistedSteamIds();
		}
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x000310EC File Offset: 0x0002F2EC
	public void RemoveWhitelistedSteamId(string steamId)
	{
		if (!this.whitelistedSteamIds.Contains(steamId))
		{
			return;
		}
		this.whitelistedSteamIds.Remove(steamId);
		this.SaveWhitelistedSteamIds();
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x00031110 File Offset: 0x0002F310
	public void RemoveWhitelistedSteamIds(params string[] steamIds)
	{
		bool flag = false;
		foreach (string item in steamIds)
		{
			if (this.whitelistedSteamIds.Contains(item))
			{
				this.whitelistedSteamIds.Remove(item);
				flag = true;
			}
		}
		if (flag)
		{
			this.SaveWhitelistedSteamIds();
		}
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x00031159 File Offset: 0x0002F359
	public bool IsSteamIdWhitelisted(string steamId)
	{
		return this.whitelistedSteamIds.Contains(steamId);
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x00031168 File Offset: 0x0002F368
	private void OnWhitelistedSteamIdsFileChanged(object sender, FileSystemEventArgs e)
	{
		WhitelistManager.Logger.Info("Whitelisted Steam IDs file changed: " + e.FullPath);
		List<string> list;
		if (ConfigUtils.TryLoadFromFile<List<string>>(e.FullPath, out list))
		{
			this.whitelistedSteamIds = (list ?? new List<string>());
		}
	}

	// Token: 0x04000631 RID: 1585
	private static readonly Logger Logger = new Logger("WhitelistManager");

	// Token: 0x04000632 RID: 1586
	private List<string> whitelistedSteamIds = new List<string>();

	// Token: 0x04000633 RID: 1587
	private string whitelistedSteamIdsFilePath;

	// Token: 0x04000634 RID: 1588
	private FileSystemWatcher whitelistedSteamIdsWatcher;
}
