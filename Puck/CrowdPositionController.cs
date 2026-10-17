using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class CrowdPositionController : MonoBehaviour
{
	// Token: 0x06000032 RID: 50 RVA: 0x0000282E File Offset: 0x00000A2E
	private void Awake()
	{
		this.crowdPosition = base.GetComponent<CrowdPosition>();
	}

	// Token: 0x06000033 RID: 51 RVA: 0x000023BA File Offset: 0x000005BA
	public void Start()
	{
	}

	// Token: 0x06000034 RID: 52 RVA: 0x000023BA File Offset: 0x000005BA
	private void OnDestroy()
	{
	}

	// Token: 0x04000020 RID: 32
	private CrowdPosition crowdPosition;
}
