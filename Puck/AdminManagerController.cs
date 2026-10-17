using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class AdminManagerController : MonoBehaviour
{
	// Token: 0x0600096A RID: 2410 RVA: 0x0002E0EB File Offset: 0x0002C2EB
	public void Awake()
	{
		this.adminManager = base.GetComponent<AdminManager>();
		EventManager.AddEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStarted));
		EventManager.AddEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x000023BA File Offset: 0x000005BA
	private void Start()
	{
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0002E125 File Offset: 0x0002C325
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStarted));
		EventManager.RemoveEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0002E153 File Offset: 0x0002C353
	private void Event_Server_OnServerStarted(Dictionary<string, object> message)
	{
		this.adminManager.LoadAdminSteamIds();
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x0002E160 File Offset: 0x0002C360
	private void Event_Server_OnServerStopped(Dictionary<string, object> message)
	{
		this.adminManager.Dispose();
	}

	// Token: 0x040005DB RID: 1499
	private AdminManager adminManager;
}
