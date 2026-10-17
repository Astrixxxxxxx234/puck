using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008D RID: 141
[RequireComponent(typeof(Zone))]
public class ZoneController : MonoBehaviour
{
	// Token: 0x0600045B RID: 1115 RVA: 0x00018E33 File Offset: 0x00017033
	private void Awake()
	{
		this.zone = base.GetComponent<Zone>();
		EventManager.AddEventListener("Event_Everyone_OnPlayerBodyDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodyDespawned));
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x00018E57 File Offset: 0x00017057
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerBodyDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodyDespawned));
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x00018E70 File Offset: 0x00017070
	private void Event_Everyone_OnPlayerBodyDespawned(Dictionary<string, object> message)
	{
		PlayerBody playerBody = (PlayerBody)message["playerBody"];
		this.zone.ClearOccupant(playerBody);
	}

	// Token: 0x0400033C RID: 828
	private Zone zone;
}
