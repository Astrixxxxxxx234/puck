using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class LockerRoomController : MonoBehaviour
{
	// Token: 0x0600009B RID: 155 RVA: 0x00003CCC File Offset: 0x00001ECC
	private void Awake()
	{
		this.lockerRoom = base.GetComponent<LockerRoom>();
	}

	// Token: 0x0600009C RID: 156 RVA: 0x000023BA File Offset: 0x000005BA
	private void OnDestroy()
	{
	}

	// Token: 0x0400004C RID: 76
	private LockerRoom lockerRoom;
}
