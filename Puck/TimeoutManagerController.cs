using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200015A RID: 346
public class TimeoutManagerController : MonoBehaviour
{
	// Token: 0x06000A36 RID: 2614 RVA: 0x00030E89 File Offset: 0x0002F089
	public void Awake()
	{
		this.timeoutManager = base.GetComponent<TimeoutManager>();
		EventManager.AddEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x000023BA File Offset: 0x000005BA
	private void Start()
	{
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x00030EAD File Offset: 0x0002F0AD
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x00030EC5 File Offset: 0x0002F0C5
	private void Event_Server_OnServerStopped(Dictionary<string, object> message)
	{
		this.timeoutManager.Dispose();
	}

	// Token: 0x04000630 RID: 1584
	private TimeoutManager timeoutManager;
}
