using System;

// Token: 0x0200017D RID: 381
public struct SynchronizedObjectLodSelection
{
	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x00038A50 File Offset: 0x00036C50
	public bool IsCulled
	{
		get
		{
			return this.Source == SynchronizedObjectLodSource.Culled;
		}
	}

	// Token: 0x04000740 RID: 1856
	public const int NO_BAND_INDEX = -1;

	// Token: 0x04000741 RID: 1857
	public static readonly SynchronizedObjectLodSelection Default = new SynchronizedObjectLodSelection
	{
		BandIndex = -1,
		TickRateDivisor = 1
	};

	// Token: 0x04000742 RID: 1858
	public int BandIndex;

	// Token: 0x04000743 RID: 1859
	public SynchronizedObjectLodSource Source;

	// Token: 0x04000744 RID: 1860
	public byte TickRateDivisor;

	// Token: 0x04000745 RID: 1861
	public bool UseHighPrecisionRotation;
}
