using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008C RID: 140
[RequireComponent(typeof(Collider))]
public class Zone : MonoBehaviour
{
	// Token: 0x17000073 RID: 115
	// (get) Token: 0x06000450 RID: 1104 RVA: 0x00018CFD File Offset: 0x00016EFD
	public string ZoneId
	{
		get
		{
			return this.zoneId;
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x06000451 RID: 1105 RVA: 0x00018D05 File Offset: 0x00016F05
	public Vector3 Origin
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x06000452 RID: 1106 RVA: 0x00018D12 File Offset: 0x00016F12
	public IEnumerable<PlayerBody> Occupants
	{
		get
		{
			return this.occupants.Keys;
		}
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00018D1F File Offset: 0x00016F1F
	private void OnTriggerEnter(Collider other)
	{
		this.TryAddOccupant(other);
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x00018D1F File Offset: 0x00016F1F
	private void OnTriggerStay(Collider other)
	{
		this.TryAddOccupant(other);
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x00018D28 File Offset: 0x00016F28
	private void TryAddOccupant(Collider other)
	{
		PlayerBody componentInParent = other.GetComponentInParent<PlayerBody>();
		if (componentInParent == null)
		{
			return;
		}
		HashSet<Collider> hashSet;
		if (!this.occupants.TryGetValue(componentInParent, out hashSet))
		{
			hashSet = new HashSet<Collider>();
			this.occupants[componentInParent] = hashSet;
		}
		bool flag = hashSet.Count == 0;
		hashSet.Add(other);
		if (flag)
		{
			this.Fire("Event_Everyone_OnPlayerEnterZone", componentInParent);
		}
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00018D88 File Offset: 0x00016F88
	private void OnTriggerExit(Collider other)
	{
		PlayerBody componentInParent = other.GetComponentInParent<PlayerBody>();
		HashSet<Collider> hashSet;
		if (componentInParent == null || !this.occupants.TryGetValue(componentInParent, out hashSet))
		{
			return;
		}
		hashSet.Remove(other);
		if (hashSet.Count == 0)
		{
			this.occupants.Remove(componentInParent);
			this.Fire("Event_Everyone_OnPlayerExitZone", componentInParent);
		}
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00018DDF File Offset: 0x00016FDF
	public void ClearOccupant(PlayerBody playerBody)
	{
		this.occupants.Remove(playerBody);
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x00018DEE File Offset: 0x00016FEE
	private void OnDisable()
	{
		this.occupants.Clear();
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x00018DFB File Offset: 0x00016FFB
	private void Fire(string eventName, PlayerBody playerBody)
	{
		EventManager.TriggerEvent(eventName, new Dictionary<string, object>
		{
			{
				"zone",
				this
			},
			{
				"playerBody",
				playerBody
			}
		});
	}

	// Token: 0x0400033A RID: 826
	[Header("Settings")]
	[SerializeField]
	private string zoneId;

	// Token: 0x0400033B RID: 827
	private readonly Dictionary<PlayerBody, HashSet<Collider>> occupants = new Dictionary<PlayerBody, HashSet<Collider>>();
}
