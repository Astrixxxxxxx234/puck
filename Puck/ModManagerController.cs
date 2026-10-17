using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020000F7 RID: 247
public static class ModManagerController
{
	// Token: 0x06000793 RID: 1939 RVA: 0x00027F14 File Offset: 0x00026114
	public static void Initialize()
	{
		EventManager.AddEventListener("Event_OnSteamWorkshopItemAdded", new Action<Dictionary<string, object>>(ModManagerController.Event_OnSteamWorkshopItemAdded));
		EventManager.AddEventListener("Event_OnSteamWorkshopItemRemoved", new Action<Dictionary<string, object>>(ModManagerController.Event_OnSteamWorkshopItemRemoved));
		EventManager.AddEventListener("Event_OnModsPluginEnabled", new Action<Dictionary<string, object>>(ModManagerController.Event_OnModsPluginEnabled));
		EventManager.AddEventListener("Event_OnModsPluginDisabled", new Action<Dictionary<string, object>>(ModManagerController.Event_OnModsPluginDisabled));
		EventManager.AddEventListener("Event_OnModsModEnabled", new Action<Dictionary<string, object>>(ModManagerController.Event_OnModsModEnabled));
		EventManager.AddEventListener("Event_OnModsModDisabled", new Action<Dictionary<string, object>>(ModManagerController.Event_OnModsModDisabled));
		EventManager.AddEventListener("Event_OnModStateChanged", new Action<Dictionary<string, object>>(ModManagerController.Event_OnModStateChanged));
		EventManager.AddEventListener("Event_OnPluginStateChanged", new Action<Dictionary<string, object>>(ModManagerController.Event_OnPluginStateChanged));
		EventManager.AddEventListener("Event_OnConnectionStateChanged", new Action<Dictionary<string, object>>(ModManagerController.Event_OnConnectionStateChanged));
		EventManager.AddEventListener("Event_OnReconnectionStateChanged", new Action<Dictionary<string, object>>(ModManagerController.Event_OnReconnectionStateChanged));
		EventManager.AddEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(ModManagerController.Event_Server_OnServerStarted));
		if (!ApplicationManager.IsDedicatedGameServer)
		{
			ModManager.LoadModsStatus();
			ModManager.LoadPlugins();
		}
		SteamWorkshopManager.Items.ForEach(delegate(SteamWorkshopItem item)
		{
			ModManager.AddMod(item);
		});
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x00028050 File Offset: 0x00026250
	public static void Dispose()
	{
		EventManager.RemoveEventListener("Event_OnSteamWorkshopItemAdded", new Action<Dictionary<string, object>>(ModManagerController.Event_OnSteamWorkshopItemAdded));
		EventManager.RemoveEventListener("Event_OnSteamWorkshopItemRemoved", new Action<Dictionary<string, object>>(ModManagerController.Event_OnSteamWorkshopItemRemoved));
		EventManager.RemoveEventListener("Event_OnModsPluginEnabled", new Action<Dictionary<string, object>>(ModManagerController.Event_OnModsPluginEnabled));
		EventManager.RemoveEventListener("Event_OnModsPluginDisabled", new Action<Dictionary<string, object>>(ModManagerController.Event_OnModsPluginDisabled));
		EventManager.RemoveEventListener("Event_OnModsModEnabled", new Action<Dictionary<string, object>>(ModManagerController.Event_OnModsModEnabled));
		EventManager.RemoveEventListener("Event_OnModsModDisabled", new Action<Dictionary<string, object>>(ModManagerController.Event_OnModsModDisabled));
		EventManager.RemoveEventListener("Event_OnModStateChanged", new Action<Dictionary<string, object>>(ModManagerController.Event_OnModStateChanged));
		EventManager.RemoveEventListener("Event_OnPluginStateChanged", new Action<Dictionary<string, object>>(ModManagerController.Event_OnPluginStateChanged));
		EventManager.RemoveEventListener("Event_OnConnectionStateChanged", new Action<Dictionary<string, object>>(ModManagerController.Event_OnConnectionStateChanged));
		EventManager.RemoveEventListener("Event_OnReconnectionStateChanged", new Action<Dictionary<string, object>>(ModManagerController.Event_OnReconnectionStateChanged));
		EventManager.RemoveEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(ModManagerController.Event_Server_OnServerStarted));
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x0002814F File Offset: 0x0002634F
	private static void Event_OnSteamWorkshopItemAdded(Dictionary<string, object> message)
	{
		ModManager.AddMod((SteamWorkshopItem)message["item"]);
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x00028167 File Offset: 0x00026367
	private static void Event_OnSteamWorkshopItemRemoved(Dictionary<string, object> message)
	{
		ModManager.RemoveMod(((SteamWorkshopItem)message["item"]).Id);
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x00028184 File Offset: 0x00026384
	private static void Event_OnModsPluginEnabled(Dictionary<string, object> message)
	{
		Plugin plugin = (Plugin)message["plugin"];
		Plugin pluginById = ModManager.GetPluginById(plugin.Id);
		bool? flag = (pluginById != null) ? new bool?(pluginById.Enable()) : null;
		if (flag != null && flag.Value)
		{
			ModManager.SetModStatus(plugin.Id, true);
		}
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x000281E8 File Offset: 0x000263E8
	private static void Event_OnModsPluginDisabled(Dictionary<string, object> message)
	{
		Plugin plugin = (Plugin)message["plugin"];
		Plugin pluginById = ModManager.GetPluginById(plugin.Id);
		bool? flag = (pluginById != null) ? new bool?(pluginById.Disable()) : null;
		if (flag != null && flag.Value)
		{
			ModManager.SetModStatus(plugin.Id, false);
		}
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x0002824C File Offset: 0x0002644C
	private static void Event_OnModsModEnabled(Dictionary<string, object> message)
	{
		Mod mod = (Mod)message["mod"];
		Mod modById = ModManager.GetModById(mod.Id);
		bool? flag = (modById != null) ? new bool?(modById.Enable()) : null;
		if (flag != null && flag.Value)
		{
			ModManager.SetModStatus(mod.Id, true);
		}
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x000282B0 File Offset: 0x000264B0
	private static void Event_OnModsModDisabled(Dictionary<string, object> message)
	{
		Mod mod = (Mod)message["mod"];
		Mod modById = ModManager.GetModById(mod.Id);
		bool? flag = (modById != null) ? new bool?(modById.Disable()) : null;
		if (flag != null && flag.Value)
		{
			ModManager.SetModStatus(mod.Id, false);
		}
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x00028314 File Offset: 0x00026514
	private static void Event_OnModStateChanged(Dictionary<string, object> message)
	{
		Mod mod = (Mod)message["mod"];
		BasePluginState basePluginState = (BasePluginState)message["oldState"];
		BasePluginState basePluginState2 = (BasePluginState)message["newState"];
		if (basePluginState.IsReady != basePluginState2.IsReady && basePluginState2.IsReady)
		{
			if (ApplicationManager.IsDedicatedGameServer)
			{
				mod.Enable();
				return;
			}
			if (ModManager.GetModStatus(mod.Id) && !GlobalStateManager.ReconnectionState.IsPendingModId(mod.Id))
			{
				mod.Enable();
			}
		}
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x000283A8 File Offset: 0x000265A8
	private static void Event_OnPluginStateChanged(Dictionary<string, object> message)
	{
		Plugin plugin = (Plugin)message["plugin"];
		BasePluginState basePluginState = (BasePluginState)message["oldState"];
		BasePluginState basePluginState2 = (BasePluginState)message["newState"];
		if (basePluginState.IsReady != basePluginState2.IsReady && basePluginState2.IsReady)
		{
			if (ApplicationManager.IsDedicatedGameServer)
			{
				plugin.Enable();
				return;
			}
			if (ModManager.GetModStatus(plugin.Id))
			{
				plugin.Enable();
			}
		}
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x00028424 File Offset: 0x00026624
	private static void Event_OnConnectionStateChanged(Dictionary<string, object> message)
	{
		ConnectionState connectionState = (ConnectionState)message["newConnectionState"];
		if (((ConnectionState)message["oldConnectionState"]).Phase != connectionState.Phase && connectionState.Phase == ConnectionPhase.Disconnected)
		{
			ModManager.ApplyModStatus();
		}
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x00028474 File Offset: 0x00026674
	private static void Event_OnReconnectionStateChanged(Dictionary<string, object> message)
	{
		ReconnectionState reconnectionState = (ReconnectionState)message["newReconnectionState"];
		ReconnectionState reconnectionState2 = (ReconnectionState)message["oldReconnectionState"];
		ReconnectionPhase phase = reconnectionState.Phase;
		if (phase != ReconnectionPhase.None)
		{
			if (phase != ReconnectionPhase.AwaitingMods)
			{
				return;
			}
			if (!reconnectionState2.PendingEnablingModIds.SequenceEqual(reconnectionState.PendingEnablingModIds) && reconnectionState.PendingReadinessModIds.Length == 0)
			{
				Mod modById = ModManager.GetModById(reconnectionState.PendingEnablingModIds.FirstOrDefault<string>());
				if (modById != null)
				{
					modById.Enable();
				}
			}
		}
		else if (reconnectionState2.Phase == ReconnectionPhase.AwaitingMods && reconnectionState2.PendingModIds.Length != 0)
		{
			ModManager.ApplyModStatus();
			return;
		}
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x00028505 File Offset: 0x00026705
	private static void Event_Server_OnServerStarted(Dictionary<string, object> message)
	{
		if (!ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		ModManager.LoadPlugins();
	}

	// Token: 0x040004D2 RID: 1234
	private static readonly Logger Logger = new Logger("ModManagerController");
}
