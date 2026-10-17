using System;

// Token: 0x0200017C RID: 380
public class SynchronizedObjectLodCounts
{
	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06000BCD RID: 3021 RVA: 0x00038930 File Offset: 0x00036B30
	public int DefaultCount
	{
		get
		{
			return this.defaultCount;
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000BCE RID: 3022 RVA: 0x00038938 File Offset: 0x00036B38
	public int CulledCount
	{
		get
		{
			return this.culledCount;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000BCF RID: 3023 RVA: 0x00038940 File Offset: 0x00036B40
	public int NoOriginCount
	{
		get
		{
			return this.noOriginCount;
		}
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x00038948 File Offset: 0x00036B48
	public void Configure(int bandCount)
	{
		if (this.bandCounts.Length != bandCount)
		{
			this.bandCounts = new int[bandCount];
		}
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x00038964 File Offset: 0x00036B64
	public void Clear()
	{
		for (int i = 0; i < this.bandCounts.Length; i++)
		{
			this.bandCounts[i] = 0;
		}
		this.defaultCount = 0;
		this.culledCount = 0;
		this.noOriginCount = 0;
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x000389A4 File Offset: 0x00036BA4
	public void Add(in SynchronizedObjectLodSelection selection)
	{
		switch (selection.Source)
		{
		case SynchronizedObjectLodSource.Band:
			this.AddBand(selection.BandIndex);
			return;
		case SynchronizedObjectLodSource.Culled:
			this.culledCount++;
			return;
		case SynchronizedObjectLodSource.NoOrigin:
			this.noOriginCount++;
			return;
		default:
			this.defaultCount++;
			return;
		}
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x00038A07 File Offset: 0x00036C07
	public int[] TakeBandCounts()
	{
		return (int[])this.bandCounts.Clone();
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x00038A19 File Offset: 0x00036C19
	private void AddBand(int bandIndex)
	{
		if (bandIndex < 0 || bandIndex >= this.bandCounts.Length)
		{
			return;
		}
		this.bandCounts[bandIndex]++;
	}

	// Token: 0x0400073C RID: 1852
	private int[] bandCounts = new int[0];

	// Token: 0x0400073D RID: 1853
	private int defaultCount;

	// Token: 0x0400073E RID: 1854
	private int culledCount;

	// Token: 0x0400073F RID: 1855
	private int noOriginCount;
}
