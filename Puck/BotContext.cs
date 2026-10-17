using System;
using UnityEngine;

// Token: 0x02000025 RID: 37
public struct BotContext
{
	// Token: 0x04000063 RID: 99
	public Puck TargetPuck;

	// Token: 0x04000064 RID: 100
	public bool IsClosestTeammateToPuck;

	// Token: 0x04000065 RID: 101
	public Vector3 HomePosition;

	// Token: 0x04000066 RID: 102
	public bool HasHomePosition;

	// Token: 0x04000067 RID: 103
	public float Seed;

	// Token: 0x04000068 RID: 104
	public PlayerBody TargetPlayerBody;
}
