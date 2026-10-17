using System;
using UnityEngine;

// Token: 0x02000131 RID: 305
public struct ReplayPuckSpawned
{
	// Token: 0x04000593 RID: 1427
	public ulong NetworkObjectId;

	// Token: 0x04000594 RID: 1428
	public Vector3 Position;

	// Token: 0x04000595 RID: 1429
	public Quaternion Rotation;

	// Token: 0x04000596 RID: 1430
	public ReplayTrack<ReplayPoseSample> PoseTrack;
}
