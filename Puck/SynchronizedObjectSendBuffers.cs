using System;
using System.Collections.Generic;

// Token: 0x02000185 RID: 389
public class SynchronizedObjectSendBuffers
{
	// Token: 0x06000C18 RID: 3096 RVA: 0x00039E49 File Offset: 0x00038049
	public void AddDelta(SynchronizedObjectData data)
	{
		this.deltas.Add(data);
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x00039E57 File Offset: 0x00038057
	public void AddReliable(SynchronizedObjectData data)
	{
		this.reliables.Add(data);
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x00039E65 File Offset: 0x00038065
	public SynchronizedObjectData[] TakeDeltas()
	{
		return this.Take(this.deltas);
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x00039E73 File Offset: 0x00038073
	public SynchronizedObjectData[] TakeReliables()
	{
		return this.Take(this.reliables);
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x00039E81 File Offset: 0x00038081
	public void Clear()
	{
		this.deltas.Clear();
		this.reliables.Clear();
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x00039E99 File Offset: 0x00038099
	public void Reset()
	{
		this.Clear();
		this.resultsByLength.Clear();
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00039EAC File Offset: 0x000380AC
	private SynchronizedObjectData[] Take(List<SynchronizedObjectData> buffer)
	{
		SynchronizedObjectData[] reusableResult = this.GetReusableResult(buffer.Count);
		buffer.CopyTo(reusableResult);
		return reusableResult;
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x00039ED0 File Offset: 0x000380D0
	private SynchronizedObjectData[] GetReusableResult(int length)
	{
		SynchronizedObjectData[] array;
		if (!this.resultsByLength.TryGetValue(length, out array))
		{
			array = new SynchronizedObjectData[length];
			this.resultsByLength.Add(length, array);
		}
		return array;
	}

	// Token: 0x04000776 RID: 1910
	private List<SynchronizedObjectData> deltas = new List<SynchronizedObjectData>();

	// Token: 0x04000777 RID: 1911
	private List<SynchronizedObjectData> reliables = new List<SynchronizedObjectData>();

	// Token: 0x04000778 RID: 1912
	private Dictionary<int, SynchronizedObjectData[]> resultsByLength = new Dictionary<int, SynchronizedObjectData[]>();
}
