using System;

// Token: 0x02000138 RID: 312
public struct ReplayPlayerBodyBinding
{
	// Token: 0x040005AF RID: 1455
	public PlayerBody PlayerBody;

	// Token: 0x040005B0 RID: 1456
	public ReplayTrack<ReplayPlayerBodyStateSample> StateTrack;

	// Token: 0x040005B1 RID: 1457
	public ReplayTrack<ReplayPlayerInputSample> InputTrack;
}
