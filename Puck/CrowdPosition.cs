using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class CrowdPosition : MonoBehaviour
{
	// Token: 0x0600002E RID: 46 RVA: 0x000027A4 File Offset: 0x000009A4
	private void Start()
	{
		EventManager.TriggerEvent("Event_OnCrowdPositionSpawned", new Dictionary<string, object>
		{
			{
				"crowdPosition",
				this
			}
		});
	}

	// Token: 0x0600002F RID: 47 RVA: 0x000027C1 File Offset: 0x000009C1
	private void OnDestroy()
	{
		EventManager.TriggerEvent("Event_OnCrowdPositionDespawned", new Dictionary<string, object>
		{
			{
				"crowdPosition",
				this
			}
		});
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000027E0 File Offset: 0x000009E0
	private void OnDrawGizmos()
	{
		if (!Application.isEditor)
		{
			return;
		}
		Gizmos.color = Color.white;
		Gizmos.DrawLine(base.transform.position, base.transform.position + Vector3.up * 0.5f);
	}
}
