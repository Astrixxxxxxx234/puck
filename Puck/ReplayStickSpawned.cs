using System;
using UnityEngine;

// Token: 0x0200012F RID: 303
public struct ReplayStickSpawned
{
	// Token: 0x0400058E RID: 1422
	public ulong OwnerClientId;

	// Token: 0x0400058F RID: 1423
	public Vector3 Position;

	// Token: 0x04000590 RID: 1424
	public Quaternion Rotation;

	// Token: 0x04000591 RID: 1425
	public ReplayTrack<ReplayPoseSample> PoseTrack;
}
