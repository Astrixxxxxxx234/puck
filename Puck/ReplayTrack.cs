using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000140 RID: 320
public class ReplayTrack<TSample> where TSample : struct
{
	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000930 RID: 2352 RVA: 0x0002D6CD File Offset: 0x0002B8CD
	private int EndTick
	{
		get
		{
			return this.startTick + this.count - 1;
		}
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0002D6DE File Offset: 0x0002B8DE
	public ReplayTrack(int retainedTicks)
	{
		this.retainedChunks = ReplayTrack<TSample>.GetChunksAlwaysCoveringAtLeast(retainedTicks);
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x0002D6FD File Offset: 0x0002B8FD
	private static int GetChunksAlwaysCoveringAtLeast(int retainedTicks)
	{
		if (retainedTicks <= 0)
		{
			return 0;
		}
		return Mathf.CeilToInt((float)retainedTicks / 1024f) + 1;
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x0002D714 File Offset: 0x0002B914
	public void Append(int tick, in TSample sample)
	{
		if (this.count == 0)
		{
			this.startTick = tick;
			this.Store(0, sample);
			this.count = 1;
			return;
		}
		if (tick <= this.EndTick)
		{
			return;
		}
		this.RepeatLastSampleUntil(tick);
		this.Store(this.count, sample);
		this.count++;
		this.DropExpiredChunks();
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x0002D772 File Offset: 0x0002B972
	public bool TryGetAtOrBefore(int tick, out TSample sample)
	{
		if (this.count == 0 || tick < this.startTick)
		{
			sample = default(TSample);
			return false;
		}
		sample = this.Read(Mathf.Min(tick - this.startTick, this.count - 1));
		return true;
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0002D7B0 File Offset: 0x0002B9B0
	private void RepeatLastSampleUntil(int tick)
	{
		if (this.EndTick >= tick - 1)
		{
			return;
		}
		TSample tsample = this.Read(this.count - 1);
		while (this.EndTick < tick - 1)
		{
			this.Store(this.count, tsample);
			this.count++;
		}
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0002D804 File Offset: 0x0002BA04
	private void Store(int index, in TSample sample)
	{
		while (index >= this.chunks.Count * 1024)
		{
			this.chunks.Add(new TSample[1024]);
		}
		this.chunks[index / 1024][index % 1024] = sample;
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x0002D860 File Offset: 0x0002BA60
	private TSample Read(int index)
	{
		return this.chunks[index / 1024][index % 1024];
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x0002D880 File Offset: 0x0002BA80
	private void DropExpiredChunks()
	{
		if (this.retainedChunks <= 0)
		{
			return;
		}
		while (this.chunks.Count > this.retainedChunks)
		{
			this.chunks.RemoveAt(0);
			this.startTick += 1024;
			this.count -= 1024;
		}
	}

	// Token: 0x040005CC RID: 1484
	private const int CHUNK_SAMPLES = 1024;

	// Token: 0x040005CD RID: 1485
	private List<TSample[]> chunks = new List<TSample[]>();

	// Token: 0x040005CE RID: 1486
	private int retainedChunks;

	// Token: 0x040005CF RID: 1487
	private int startTick;

	// Token: 0x040005D0 RID: 1488
	private int count;
}
