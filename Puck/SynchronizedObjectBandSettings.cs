using System;
using UnityEngine;

// Token: 0x02000177 RID: 375
[Serializable]
public struct SynchronizedObjectBandSettings
{
	// Token: 0x040006E1 RID: 1761
	[Tooltip("Distance from the player beyond which this band applies.")]
	public float MinDistance;

	// Token: 0x040006E2 RID: 1762
	[Range(1f, 16f)]
	[Tooltip("Objects in this band update every TickRateDivisor ticks, so 2 sends at 50 Hz from a 100 Hz tick rate. A value of 1 disables throttling for the band.")]
	public byte TickRateDivisor;
}
