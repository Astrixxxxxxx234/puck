using System;

// Token: 0x02000187 RID: 391
public struct SynchronizedObjectSendState
{
	// Token: 0x0400077C RID: 1916
	public SynchronizedObjectLodSelection LodSelection;

	// Token: 0x0400077D RID: 1917
	public SynchronizedObjectData LastSentData;

	// Token: 0x0400077E RID: 1918
	public bool HasLastSentData;

	// Token: 0x0400077F RID: 1919
	public bool IsAwake;

	// Token: 0x04000780 RID: 1920
	public uint LastFullSendTick;
}
