using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class BanManagerController : MonoBehaviour
{
	// Token: 0x06000984 RID: 2436 RVA: 0x0002E641 File Offset: 0x0002C841
	public void Awake()
	{
		this.banManager = base.GetComponent<BanManager>();
		EventManager.AddEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStarted));
		EventManager.AddEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x000023BA File Offset: 0x000005BA
	private void Start()
	{
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x0002E67B File Offset: 0x0002C87B
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStarted));
		EventManager.RemoveEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x0002E6A9 File Offset: 0x0002C8A9
	private void Event_Server_OnServerStarted(Dictionary<string, object> message)
	{
		this.banManager.LoadBannedSteamIds();
		this.banManager.LoadBannedIpAddresses();
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x0002E6C1 File Offset: 0x0002C8C1
	private void Event_Server_OnServerStopped(Dictionary<string, object> message)
	{
		this.banManager.Dispose();
	}

	// Token: 0x040005E3 RID: 1507
	private BanManager banManager;
}
