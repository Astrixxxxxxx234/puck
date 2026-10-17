using System;

// Token: 0x0200008A RID: 138
public class SynchronizedObjectSampleRing
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000441 RID: 1089 RVA: 0x00018AA2 File Offset: 0x00016CA2
	// (set) Token: 0x06000442 RID: 1090 RVA: 0x00018AAA File Offset: 0x00016CAA
	public int Count { get; private set; }

	// Token: 0x06000443 RID: 1091 RVA: 0x00018AB3 File Offset: 0x00016CB3
	public SynchronizedObjectSampleRing(int capacity)
	{
		this.samples = new SynchronizedObjectSample[capacity];
	}

	// Token: 0x17000070 RID: 112
	public SynchronizedObjectSample this[int index]
	{
		get
		{
			return this.samples[(this.head + index) % this.samples.Length];
		}
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000445 RID: 1093 RVA: 0x00018AE5 File Offset: 0x00016CE5
	public SynchronizedObjectSample Newest
	{
		get
		{
			return this[this.Count - 1];
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06000446 RID: 1094 RVA: 0x00018AF5 File Offset: 0x00016CF5
	public SynchronizedObjectSample Oldest
	{
		get
		{
			return this[0];
		}
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x00018B00 File Offset: 0x00016D00
	public void Append(in SynchronizedObjectSample sample)
	{
		this.samples[(this.head + this.Count) % this.samples.Length] = sample;
		if (this.Count < this.samples.Length)
		{
			int count = this.Count;
			this.Count = count + 1;
			return;
		}
		this.head = (this.head + 1) % this.samples.Length;
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x00018B6C File Offset: 0x00016D6C
	public void Reset()
	{
		this.head = 0;
		this.Count = 0;
	}

	// Token: 0x0400032F RID: 815
	private SynchronizedObjectSample[] samples;

	// Token: 0x04000330 RID: 816
	private int head;
}
