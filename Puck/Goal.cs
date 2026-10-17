using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class Goal : MonoBehaviour
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600003E RID: 62 RVA: 0x00002A68 File Offset: 0x00000C68
	public Cloth NetCloth
	{
		get
		{
			return this.netCloth;
		}
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00002A70 File Offset: 0x00000C70
	public void Server_OnPuckEnterGoal(Puck puck)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		EventManager.TriggerEvent("Event_Server_OnPuckEnterGoal", new Dictionary<string, object>
		{
			{
				"puck",
				puck
			},
			{
				"team",
				this.Team
			}
		});
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00002AB0 File Offset: 0x00000CB0
	public void Client_AddNetClothSphereCollider(SphereCollider sphereCollider)
	{
		if (!NetworkManager.Singleton.IsClient)
		{
			return;
		}
		if (!sphereCollider)
		{
			return;
		}
		List<ClothSphereColliderPair> list = this.netCloth.sphereColliders.ToList<ClothSphereColliderPair>();
		list.Add(new ClothSphereColliderPair(sphereCollider));
		this.netCloth.sphereColliders = list.ToArray();
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00002B04 File Offset: 0x00000D04
	public void Client_RemoveNetClothSphereCollider(SphereCollider sphereCollider)
	{
		if (!NetworkManager.Singleton.IsClient)
		{
			return;
		}
		if (!sphereCollider)
		{
			return;
		}
		List<ClothSphereColliderPair> list = this.netCloth.sphereColliders.ToList<ClothSphereColliderPair>();
		list.RemoveAll((ClothSphereColliderPair pair) => pair.first == sphereCollider);
		this.netCloth.sphereColliders = list.ToArray();
	}

	// Token: 0x04000027 RID: 39
	[Header("Settings")]
	public PlayerTeam Team;

	// Token: 0x04000028 RID: 40
	[Header("References")]
	[SerializeField]
	private Cloth netCloth;
}
