using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class PlayerPositionController : MonoBehaviour
{
	// Token: 0x060000C6 RID: 198 RVA: 0x00004486 File Offset: 0x00002686
	private void Awake()
	{
		this.playerPosition = base.GetComponent<PlayerPosition>();
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x000023BA File Offset: 0x000005BA
	public void Start()
	{
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x000023BA File Offset: 0x000005BA
	private void OnDestroy()
	{
	}

	// Token: 0x04000062 RID: 98
	private PlayerPosition playerPosition;
}
