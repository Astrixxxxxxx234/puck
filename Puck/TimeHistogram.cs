using System;

// Token: 0x02000244 RID: 580
public class TimeHistogram
{
	// Token: 0x1700017B RID: 379
	// (get) Token: 0x06001241 RID: 4673 RVA: 0x00054ED5 File Offset: 0x000530D5
	public float BucketSize { get; }

	// Token: 0x06001242 RID: 4674 RVA: 0x00054EDD File Offset: 0x000530DD
	public TimeHistogram(float bucketSize = 0.0005f, int bucketCount = 101)
	{
		this.BucketSize = bucketSize;
		this.buckets = new int[bucketCount];
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06001243 RID: 4675 RVA: 0x00054F07 File Offset: 0x00053107
	// (set) Token: 0x06001244 RID: 4676 RVA: 0x00054F0F File Offset: 0x0005310F
	public int Count { get; private set; }

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06001245 RID: 4677 RVA: 0x00054F18 File Offset: 0x00053118
	public double Min
	{
		get
		{
			if (this.Count <= 0)
			{
				return 0.0;
			}
			return this.min;
		}
	}

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06001246 RID: 4678 RVA: 0x00054F33 File Offset: 0x00053133
	public double Max
	{
		get
		{
			return this.max;
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06001247 RID: 4679 RVA: 0x00054F3B File Offset: 0x0005313B
	public double Average
	{
		get
		{
			if (this.Count <= 0)
			{
				return 0.0;
			}
			return this.sum / (double)this.Count;
		}
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x06001248 RID: 4680 RVA: 0x00054F60 File Offset: 0x00053160
	public double StandardDeviation
	{
		get
		{
			if (this.Count <= 0)
			{
				return 0.0;
			}
			return Math.Sqrt(Math.Max(0.0, this.squaredSum / (double)this.Count - this.Average * this.Average));
		}
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x00054FB0 File Offset: 0x000531B0
	public void Add(float time)
	{
		int num = Math.Clamp((int)(time / this.BucketSize), 0, this.buckets.Length - 1);
		this.buckets[num]++;
		int count = this.Count;
		this.Count = count + 1;
		this.sum += (double)time;
		this.squaredSum += (double)time * (double)time;
		this.min = Math.Min(this.min, (double)time);
		this.max = Math.Max(this.max, (double)time);
	}

	// Token: 0x0600124A RID: 4682 RVA: 0x00055040 File Offset: 0x00053240
	public double GetPercentile(double percentile)
	{
		if (this.Count == 0)
		{
			return 0.0;
		}
		int num = (int)Math.Ceiling(percentile * (double)this.Count);
		int num2 = 0;
		for (int i = 0; i < this.buckets.Length - 1; i++)
		{
			num2 += this.buckets[i];
			if (num2 >= num)
			{
				return Math.Min((double)(i + 1) * (double)this.BucketSize, this.max);
			}
		}
		return this.max;
	}

	// Token: 0x0600124B RID: 4683 RVA: 0x000550B3 File Offset: 0x000532B3
	public int[] ToArray()
	{
		return (int[])this.buckets.Clone();
	}

	// Token: 0x0600124C RID: 4684 RVA: 0x000550C8 File Offset: 0x000532C8
	public void Reset()
	{
		Array.Clear(this.buckets, 0, this.buckets.Length);
		this.Count = 0;
		this.sum = 0.0;
		this.squaredSum = 0.0;
		this.min = double.MaxValue;
		this.max = 0.0;
	}

	// Token: 0x04000BCB RID: 3019
	public const float DEFAULT_BUCKET_SIZE = 0.0005f;

	// Token: 0x04000BCC RID: 3020
	public const int DEFAULT_BUCKET_COUNT = 101;

	// Token: 0x04000BCE RID: 3022
	private int[] buckets;

	// Token: 0x04000BCF RID: 3023
	private double sum;

	// Token: 0x04000BD0 RID: 3024
	private double squaredSum;

	// Token: 0x04000BD1 RID: 3025
	private double min = double.MaxValue;

	// Token: 0x04000BD2 RID: 3026
	private double max;
}
