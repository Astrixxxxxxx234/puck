using System;
using Unity.Collections;
using UnityEngine;

// Token: 0x0200012D RID: 301
public struct ReplayPlayerBodySpawned
{
	// Token: 0x04000583 RID: 1411
	public ulong OwnerClientId;

	// Token: 0x04000584 RID: 1412
	public Vector3 Position;

	// Token: 0x04000585 RID: 1413
	public Quaternion Rotation;

	// Token: 0x04000586 RID: 1414
	public PlayerGameState GameState;

	// Token: 0x04000587 RID: 1415
	public PlayerCustomizationState CustomizationState;

	// Token: 0x04000588 RID: 1416
	public FixedString32Bytes Username;

	// Token: 0x04000589 RID: 1417
	public int Number;

	// Token: 0x0400058A RID: 1418
	public ReplayTrack<ReplayPoseSample> PoseTrack;

	// Token: 0x0400058B RID: 1419
	public ReplayTrack<ReplayPlayerBodyStateSample> StateTrack;

	// Token: 0x0400058C RID: 1420
	public ReplayTrack<ReplayPlayerInputSample> InputTrack;
}
