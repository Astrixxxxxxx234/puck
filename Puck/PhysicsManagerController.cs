using System;
using UnityEngine;

// Token: 0x02000114 RID: 276
public class PhysicsManagerController : MonoBehaviour
{
	// Token: 0x06000857 RID: 2135 RVA: 0x00029F90 File Offset: 0x00028190
	private void Awake()
	{
		this.physicsManager = base.GetComponent<PhysicsManager>();
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x000023BA File Offset: 0x000005BA
	private void Start()
	{
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x000023BA File Offset: 0x000005BA
	private void OnDestroy()
	{
	}

	// Token: 0x04000539 RID: 1337
	private PhysicsManager physicsManager;
}
