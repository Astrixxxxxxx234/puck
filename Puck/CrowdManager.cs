using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public class CrowdManager : MonoBehaviourSingleton<CrowdManager>
{
	// Token: 0x06000633 RID: 1587 RVA: 0x00022448 File Offset: 0x00020648
	private void Update()
	{
		int count = this.crowdMembers.Count;
		if (count == 0)
		{
			return;
		}
		for (int i = 0; i < this.crowdUpdatesPerFrame; i++)
		{
			int index = (this.updateBatch + i) % count;
			this.crowdMembers[index].UpdateAnimation();
		}
		this.updateBatch = (this.updateBatch + this.crowdUpdatesPerFrame) % count;
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x000224A8 File Offset: 0x000206A8
	public void RegisterCrowdPosition(CrowdPosition position)
	{
		if (this.crowdPositionMemberMap.ContainsKey(position))
		{
			return;
		}
		if (Random.value > this.crowdDensity)
		{
			return;
		}
		CrowdMember crowdMember = Object.Instantiate<CrowdMember>(this.crowdMemberPrefab, position.transform.position, position.transform.rotation, base.transform);
		this.crowdPositionMemberMap[position] = crowdMember;
		this.crowdMembers.Add(crowdMember);
		crowdMember.RandomizeAppearance();
		crowdMember.PlayAnimation(this.currentAnimation);
		crowdMember.LookTarget = this.currentLookTarget;
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x00022534 File Offset: 0x00020734
	public void UnregisterCrowdPosition(CrowdPosition position)
	{
		if (!this.crowdPositionMemberMap.ContainsKey(position))
		{
			return;
		}
		CrowdMember crowdMember = this.crowdPositionMemberMap[position];
		Object.Destroy(crowdMember.gameObject);
		this.crowdPositionMemberMap.Remove(position);
		this.crowdMembers.Remove(crowdMember);
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x00022584 File Offset: 0x00020784
	public void SetCrowdLookTarget(Transform lookTarget)
	{
		this.currentLookTarget = lookTarget;
		foreach (CrowdMember crowdMember in this.crowdPositionMemberMap.Values)
		{
			crowdMember.LookTarget = this.currentLookTarget;
		}
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x000225E8 File Offset: 0x000207E8
	public void SetCrowdAnimation(string animationName)
	{
		this.currentAnimation = animationName;
		foreach (CrowdMember crowdMember in this.crowdPositionMemberMap.Values)
		{
			crowdMember.PlayAnimation(this.currentAnimation);
		}
	}

	// Token: 0x04000402 RID: 1026
	[Header("Settings")]
	[SerializeField]
	private float crowdDensity = 0.25f;

	// Token: 0x04000403 RID: 1027
	[SerializeField]
	private int crowdUpdatesPerFrame = 8;

	// Token: 0x04000404 RID: 1028
	[Header("Prefabs")]
	[SerializeField]
	private CrowdMember crowdMemberPrefab;

	// Token: 0x04000405 RID: 1029
	private Dictionary<CrowdPosition, CrowdMember> crowdPositionMemberMap = new Dictionary<CrowdPosition, CrowdMember>();

	// Token: 0x04000406 RID: 1030
	private List<CrowdMember> crowdMembers = new List<CrowdMember>();

	// Token: 0x04000407 RID: 1031
	private Transform currentLookTarget;

	// Token: 0x04000408 RID: 1032
	private string currentAnimation = "Seated";

	// Token: 0x04000409 RID: 1033
	private int updateBatch;
}
