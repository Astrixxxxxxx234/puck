using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class PuckManager : MonoBehaviourSingleton<PuckManager>
{
	// Token: 0x060008A9 RID: 2217 RVA: 0x0002AE60 File Offset: 0x00029060
	public void AddPuckPosition(PuckPosition puckPosition)
	{
		PuckManager.Logger.Info(string.Format("Added puck position for phase {0}", puckPosition.Phase));
		this.puckPositions.Add(puckPosition);
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x0002AE8D File Offset: 0x0002908D
	public void RemovePuckPosition(PuckPosition puckPosition)
	{
		this.puckPositions.Remove(puckPosition);
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x0002AE9C File Offset: 0x0002909C
	public void AddPuck(Puck puck)
	{
		this.pucks.Add(puck);
		if (!puck.IsReplay.Value)
		{
			this.nonReplayPucks.Add(puck);
		}
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x0002AEC3 File Offset: 0x000290C3
	public void RemovePuck(Puck puck)
	{
		this.pucks.Remove(puck);
		this.nonReplayPucks.Remove(puck);
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x0002AEDF File Offset: 0x000290DF
	public List<Puck> GetPucks(bool includeReplay = false)
	{
		if (includeReplay)
		{
			return this.pucks;
		}
		return this.nonReplayPucks;
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x0002AEF1 File Offset: 0x000290F1
	public List<Puck> GetReplayPucks()
	{
		return (from puck in this.pucks
		where puck.IsReplay.Value
		select puck).ToList<Puck>();
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x0002AF22 File Offset: 0x00029122
	public Puck GetPuck(bool includeReplay = false)
	{
		return this.GetPucks(includeReplay).FirstOrDefault((Puck puck) => puck);
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x0002AF50 File Offset: 0x00029150
	public Puck GetPlayerPuck(ulong clientId)
	{
		Player playerByClientId = MonoBehaviourSingleton<PlayerManager>.Instance.GetPlayerByClientId(clientId);
		if (!playerByClientId)
		{
			return null;
		}
		if (!playerByClientId.Stick)
		{
			return null;
		}
		NetworkList<NetworkObjectCollision> buffer = playerByClientId.Stick.NetworkObjectCollisionRecorder.Buffer;
		if (buffer.Count == 0)
		{
			return null;
		}
		return NetworkingUtils.GetPuckFromNetworkObjectReference(buffer[buffer.Count - 1].NetworkObjectReference);
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x0002AFB8 File Offset: 0x000291B8
	public Puck GetPuckByNetworkObjectId(ulong networkObjectId)
	{
		return this.GetPucks(false).FirstOrDefault((Puck puck) => puck.NetworkObjectId == networkObjectId);
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x0002AFEC File Offset: 0x000291EC
	public Puck GetReplayPuckByNetworkObjectId(ulong networkObjectId)
	{
		return this.GetReplayPucks().FirstOrDefault((Puck puck) => puck.NetworkObjectId == networkObjectId);
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0002B020 File Offset: 0x00029220
	public Puck Server_SpawnPuck(Vector3 position, Quaternion rotation, bool isReplay = false)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return null;
		}
		Puck puck = Object.Instantiate<Puck>(this.puckPrefab, position, rotation);
		puck.InitializeNetworkVariables(isReplay);
		puck.NetworkObject.Spawn(false);
		PuckManager.Logger.Info(string.Format("Spawned puck {0}", puck.NetworkObjectId));
		return puck;
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x0002B07C File Offset: 0x0002927C
	public void Server_DespawnPuck(Puck puck)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		puck.NetworkObject.Despawn(true);
		PuckManager.Logger.Info(string.Format("Despawned puck {0}", puck.NetworkObjectId));
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0002B0B8 File Offset: 0x000292B8
	public void Server_DespawnPucks(bool includeReplay = false)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		PuckManager.Logger.Info(string.Format("Despawning {0} pucks (includeReplay: {1})", this.pucks.Count, includeReplay));
		foreach (Puck puck in this.pucks.ToList<Puck>())
		{
			if (includeReplay || !puck.IsReplay.Value)
			{
				this.Server_DespawnPuck(puck);
			}
		}
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x0002B158 File Offset: 0x00029358
	public void Server_SpawnPucksForPhase(GamePhase phase)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		PuckManager.Logger.Info(string.Format("Spawning pucks for phase {0}", phase));
		this.puckPositions.FindAll((PuckPosition puckPosition) => puckPosition.Phase == phase).ForEach(delegate(PuckPosition puckPosition)
		{
			this.Server_SpawnPuck(puckPosition.transform.position, puckPosition.transform.rotation, false);
		});
	}

	// Token: 0x04000566 RID: 1382
	private static readonly global::Logger Logger = new global::Logger("PuckManager");

	// Token: 0x04000567 RID: 1383
	[Header("Prefabs")]
	[SerializeField]
	private Puck puckPrefab;

	// Token: 0x04000568 RID: 1384
	private List<PuckPosition> puckPositions = new List<PuckPosition>();

	// Token: 0x04000569 RID: 1385
	private List<Puck> pucks = new List<Puck>();

	// Token: 0x0400056A RID: 1386
	private List<Puck> nonReplayPucks = new List<Puck>();
}
