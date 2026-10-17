using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000136 RID: 310
public class ReplayManagerController : MonoBehaviour
{
	// Token: 0x060008DC RID: 2268 RVA: 0x0002B6F4 File Offset: 0x000298F4
	private void Awake()
	{
		this.replayManager = base.GetComponent<ReplayManager>();
		EventManager.AddEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x0002B718 File Offset: 0x00029918
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x0002B730 File Offset: 0x00029930
	private void Event_Server_OnServerStopped(Dictionary<string, object> message)
	{
		this.replayManager.Server_StopReplaying();
		this.replayManager.Server_StopRecording();
	}

	// Token: 0x040005A2 RID: 1442
	private ReplayManager replayManager;
}
