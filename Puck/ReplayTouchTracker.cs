using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class ReplayTouchTracker : MonoBehaviour
{
	// Token: 0x1700006A RID: 106
	// (get) Token: 0x060003AF RID: 943 RVA: 0x000163ED File Offset: 0x000145ED
	public IReadOnlyDictionary<Player, float> LastContactTimeByPlayer
	{
		get
		{
			return this.lastContactTimeByPlayer;
		}
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x000163F5 File Offset: 0x000145F5
	private void OnCollisionEnter(Collision collision)
	{
		this.RecordContact(collision.gameObject, true);
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x00016404 File Offset: 0x00014604
	private void OnCollisionExit(Collision collision)
	{
		this.RecordContact(collision.gameObject, false);
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x00016414 File Offset: 0x00014614
	private void RecordContact(GameObject collisionObject, bool entering)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		NetworkObject networkObject;
		if (!collisionObject.TryGetComponent<NetworkObject>(out networkObject))
		{
			return;
		}
		Player player = ReplayTouchTracker.ResolvePlayer(networkObject);
		if (!player)
		{
			return;
		}
		if (entering)
		{
			this.activeContactObjectIds.Add(networkObject.NetworkObjectId);
		}
		else
		{
			this.activeContactObjectIds.Remove(networkObject.NetworkObjectId);
		}
		this.lastContactTimeByPlayer[player] = Time.time;
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x00016484 File Offset: 0x00014684
	private static Player ResolvePlayer(NetworkObject networkObject)
	{
		PlayerBody playerBody;
		if (networkObject.TryGetComponent<PlayerBody>(out playerBody))
		{
			return playerBody.Player;
		}
		Stick stick;
		if (networkObject.TryGetComponent<Stick>(out stick))
		{
			return stick.Player;
		}
		return null;
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x000164B4 File Offset: 0x000146B4
	public bool IsActivelyTouching(NetworkObject networkObject)
	{
		if (networkObject == null)
		{
			return false;
		}
		this.activeContactObjectIds.RemoveWhere((ulong id) => !NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(id));
		return this.activeContactObjectIds.Contains(networkObject.NetworkObjectId);
	}

	// Token: 0x040002C2 RID: 706
	private HashSet<ulong> activeContactObjectIds = new HashSet<ulong>();

	// Token: 0x040002C3 RID: 707
	private Dictionary<Player, float> lastContactTimeByPlayer = new Dictionary<Player, float>();
}
