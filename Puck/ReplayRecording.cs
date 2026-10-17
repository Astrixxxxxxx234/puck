using System;
using System.Collections.Generic;

// Token: 0x0200013F RID: 319
public class ReplayRecording
{
	// Token: 0x170000FD RID: 253
	// (get) Token: 0x0600092A RID: 2346 RVA: 0x0002D5E5 File Offset: 0x0002B7E5
	public List<ReplayDiscreteEvent> Events
	{
		get
		{
			return this.events;
		}
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x0600092B RID: 2347 RVA: 0x0002D5ED File Offset: 0x0002B7ED
	public int LastTick
	{
		get
		{
			return this.TickCount - 1;
		}
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x0002D5F8 File Offset: 0x0002B7F8
	public void AddEvent(int tick, string name, object data)
	{
		this.events.Insert(this.GetIndexAfterTick(tick), new ReplayDiscreteEvent
		{
			Tick = tick,
			Name = name,
			Data = data
		});
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x0002D638 File Offset: 0x0002B838
	public int GetIndexAtOrAfterTick(int tick)
	{
		for (int i = 0; i < this.events.Count; i++)
		{
			if (this.events[i].Tick >= tick)
			{
				return i;
			}
		}
		return this.events.Count;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x0002D67C File Offset: 0x0002B87C
	private int GetIndexAfterTick(int tick)
	{
		for (int i = this.events.Count - 1; i >= 0; i--)
		{
			if (this.events[i].Tick <= tick)
			{
				return i + 1;
			}
		}
		return 0;
	}

	// Token: 0x040005CA RID: 1482
	public int TickCount;

	// Token: 0x040005CB RID: 1483
	private List<ReplayDiscreteEvent> events = new List<ReplayDiscreteEvent>();
}
