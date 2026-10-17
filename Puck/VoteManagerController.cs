using System;
using UnityEngine;

// Token: 0x0200019B RID: 411
public class VoteManagerController : MonoBehaviour
{
	// Token: 0x06000CC9 RID: 3273 RVA: 0x0003CD51 File Offset: 0x0003AF51
	private void Awake()
	{
		this.voteManager = base.GetComponent<VoteManager>();
	}

	// Token: 0x06000CCA RID: 3274 RVA: 0x000023BA File Offset: 0x000005BA
	private void Start()
	{
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x000023BA File Offset: 0x000005BA
	private void OnDestroy()
	{
	}

	// Token: 0x040007F0 RID: 2032
	private static readonly global::Logger Logger = new global::Logger("VoteManagerController");

	// Token: 0x040007F1 RID: 2033
	private VoteManager voteManager;
}
