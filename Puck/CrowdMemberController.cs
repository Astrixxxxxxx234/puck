using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class CrowdMemberController : MonoBehaviour
{
	// Token: 0x0600002B RID: 43 RVA: 0x00002796 File Offset: 0x00000996
	private void Awake()
	{
		this.crowdMember = base.GetComponent<CrowdMember>();
	}

	// Token: 0x0600002C RID: 44 RVA: 0x000023BA File Offset: 0x000005BA
	private void OnDestroy()
	{
	}

	// Token: 0x0400001F RID: 31
	private CrowdMember crowdMember;
}
