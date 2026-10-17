using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Token: 0x020000F2 RID: 242
public class Mod : BasePlugin<BasePluginState>
{
	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x0600076B RID: 1899 RVA: 0x00027758 File Offset: 0x00025958
	public string Id
	{
		get
		{
			return this.SteamWorkshopItem.Id;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x0600076C RID: 1900 RVA: 0x00027765 File Offset: 0x00025965
	public bool HasLocalFiles
	{
		get
		{
			return !string.IsNullOrEmpty(base.Path) && Directory.Exists(base.Path) && Directory.EnumerateFileSystemEntries(base.Path).Any<string>();
		}
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x00027793 File Offset: 0x00025993
	public Mod(SteamWorkshopItem steamWorkshopItem) : base(new BasePluginState
	{
		Path = null,
		IsReady = false,
		IsEnabled = false
	})
	{
		this.SteamWorkshopItem = steamWorkshopItem;
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x000277BC File Offset: 0x000259BC
	public override void Initialize()
	{
		base.Initialize();
		SteamWorkshopItem steamWorkshopItem = this.SteamWorkshopItem;
		steamWorkshopItem.StateChanged = (Action<SteamWorkshopItemState, SteamWorkshopItemState>)Delegate.Combine(steamWorkshopItem.StateChanged, new Action<SteamWorkshopItemState, SteamWorkshopItemState>(this.OnSteamWorkshopItemStateChanged));
		SteamWorkshopItem steamWorkshopItem2 = this.SteamWorkshopItem;
		steamWorkshopItem2.DetailsStateChanged = (Action<SteamWorkshopItemDetailsState, SteamWorkshopItemDetailsState>)Delegate.Combine(steamWorkshopItem2.DetailsStateChanged, new Action<SteamWorkshopItemDetailsState, SteamWorkshopItemDetailsState>(this.OnSteamWorkshopItemDetailsStateChanged));
		this.SetState(new Dictionary<string, object>
		{
			{
				"path",
				this.SteamWorkshopItem.Path
			},
			{
				"isReady",
				this.SteamWorkshopItem.Phase == SteamWorkshopItemPhase.Installed
			}
		});
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x0002785C File Offset: 0x00025A5C
	public override void OnEnableFailed(Exception exception)
	{
		base.OnEnableFailed(exception);
		Mod.Logger.Error("Failed to enable mod " + this.Id + ": " + exception.Message);
		EventManager.TriggerEvent("Event_OnModEnableFailed", new Dictionary<string, object>
		{
			{
				"mod",
				this
			}
		});
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x000278B0 File Offset: 0x00025AB0
	public override void OnDisableFailed(Exception exception)
	{
		base.OnDisableFailed(exception);
		Mod.Logger.Error("Failed to disable mod " + this.Id + ": " + exception.Message);
		EventManager.TriggerEvent("Event_OnModDisableFailed", new Dictionary<string, object>
		{
			{
				"mod",
				this
			}
		});
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x00027904 File Offset: 0x00025B04
	protected override void OnStateChanged(BasePluginState oldState, BasePluginState newState)
	{
		base.OnStateChanged(oldState, newState);
		EventManager.TriggerEvent("Event_OnModStateChanged", new Dictionary<string, object>
		{
			{
				"mod",
				this
			},
			{
				"oldState",
				oldState
			},
			{
				"newState",
				newState
			}
		});
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x00027944 File Offset: 0x00025B44
	private void OnSteamWorkshopItemStateChanged(SteamWorkshopItemState oldState, SteamWorkshopItemState newState)
	{
		EventManager.TriggerEvent("Event_OnModSteamWorkshopItemStateChanged", new Dictionary<string, object>
		{
			{
				"mod",
				this
			},
			{
				"oldState",
				oldState
			},
			{
				"newState",
				newState
			}
		});
		this.SetState(new Dictionary<string, object>
		{
			{
				"path",
				newState.Path
			},
			{
				"isReady",
				newState.Phase == SteamWorkshopItemPhase.Installed
			}
		});
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x000279B9 File Offset: 0x00025BB9
	private void OnSteamWorkshopItemDetailsStateChanged(SteamWorkshopItemDetailsState oldState, SteamWorkshopItemDetailsState newState)
	{
		EventManager.TriggerEvent("Event_OnModSteamWorkshopItemDetailsStateChanged", new Dictionary<string, object>
		{
			{
				"mod",
				this
			},
			{
				"oldState",
				oldState
			},
			{
				"newState",
				newState
			}
		});
	}

	// Token: 0x040004C5 RID: 1221
	private static readonly Logger Logger = new Logger("Mod");

	// Token: 0x040004C6 RID: 1222
	public readonly SteamWorkshopItem SteamWorkshopItem;
}
