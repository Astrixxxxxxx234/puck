using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200015C RID: 348
public class WhitelistManagerController : MonoBehaviour
{
	// Token: 0x06000A49 RID: 2633 RVA: 0x000311D2 File Offset: 0x0002F3D2
	public void Awake()
	{
		this.whitelistManager = base.GetComponent<WhitelistManager>();
		EventManager.AddEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStarted));
		EventManager.AddEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x000023BA File Offset: 0x000005BA
	private void Start()
	{
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x0003120C File Offset: 0x0002F40C
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Server_OnServerStarted", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStarted));
		EventManager.RemoveEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x0003123A File Offset: 0x0002F43A
	private void Event_Server_OnServerStarted(Dictionary<string, object> message)
	{
		this.whitelistManager.LoadWhitelistedSteamIds();
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x00031247 File Offset: 0x0002F447
	private void Event_Server_OnServerStopped(Dictionary<string, object> message)
	{
		this.whitelistManager.Dispose();
	}

	// Token: 0x04000635 RID: 1589
	private WhitelistManager whitelistManager;
}
