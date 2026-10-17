using System;

// Token: 0x02000186 RID: 390
public class SynchronizedObjectSendPlanner
{
	// Token: 0x06000C21 RID: 3105 RVA: 0x00039F2B File Offset: 0x0003812B
	public void Configure(SynchronizedObjectBandSettings[] lodBands, SynchronizedObjectBandSettings culling, byte noOriginTickRateDivisor, float lodHysteresis, bool useHighPrecisionRotation)
	{
		this.lodSelector.Configure(lodBands, culling, noOriginTickRateDivisor, lodHysteresis, useHighPrecisionRotation);
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x00039F40 File Offset: 0x00038140
	public void Plan(SynchronizedObjectSnapshot snapshot, SynchronizedPlayerState playerState, uint tickCount, int tickRate, SynchronizedObjectSendBuffers buffers, SynchronizedObjectLodCounts lodCounts)
	{
		this.tickCount = tickCount;
		this.tickRate = tickRate;
		buffers.Clear();
		this.lodSelector.BeginPlayer(playerState.Player);
		for (int i = 0; i < snapshot.Count; i++)
		{
			this.PlanObject(snapshot, i, playerState, buffers, lodCounts);
		}
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x00039F94 File Offset: 0x00038194
	private void PlanObject(SynchronizedObjectSnapshot snapshot, int index, SynchronizedPlayerState playerState, SynchronizedObjectSendBuffers buffers, SynchronizedObjectLodCounts lodCounts)
	{
		SynchronizedObjectData data = snapshot.GetData(index);
		ulong networkObjectId = (ulong)data.NetworkObjectId;
		SynchronizedObjectSendState sendState = playerState.GetSendState(networkObjectId);
		sendState.LodSelection = this.lodSelector.Select(snapshot.GetPosition(index), sendState.LodSelection, snapshot.GetUsesLod(index), snapshot.GetUsesCulling(index));
		if (lodCounts != null)
		{
			lodCounts.Add(sendState.LodSelection);
		}
		this.PlanSend(data, ref sendState, buffers);
		playerState.SetSendState(networkObjectId, sendState);
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x0003A00C File Offset: 0x0003820C
	private void PlanSend(SynchronizedObjectData synchronizedObjectData, ref SynchronizedObjectSendState sendState, SynchronizedObjectSendBuffers buffers)
	{
		SynchronizedObjectLodSelection lodSelection = sendState.LodSelection;
		if (this.tickCount % (uint)lodSelection.TickRateDivisor != 0U)
		{
			return;
		}
		synchronizedObjectData = synchronizedObjectData.WithTickRateDivisor(lodSelection.TickRateDivisor);
		bool flag = this.IsFullSendDue(sendState);
		ushort num = flag ? 3071 : synchronizedObjectData.GetChangeMask(sendState.LastSentData, lodSelection.UseHighPrecisionRotation);
		if ((num & 1023) == 0)
		{
			if (!sendState.IsAwake)
			{
				return;
			}
			sendState.IsAwake = false;
			sendState.LastSentData = synchronizedObjectData;
			sendState.HasLastSentData = true;
			buffers.AddReliable(synchronizedObjectData.WithAsleep());
			return;
		}
		else
		{
			SynchronizedObjectData synchronizedObjectData2 = synchronizedObjectData.WithComponentMask(num, lodSelection.UseHighPrecisionRotation);
			if (flag)
			{
				sendState.LastFullSendTick = this.GetFullSendTick(sendState, synchronizedObjectData.NetworkObjectId);
			}
			sendState.LastSentData = (sendState.HasLastSentData ? sendState.LastSentData.Merge(synchronizedObjectData2) : synchronizedObjectData2);
			sendState.HasLastSentData = true;
			sendState.IsAwake = true;
			if (flag)
			{
				buffers.AddReliable(synchronizedObjectData2);
				return;
			}
			buffers.AddDelta(synchronizedObjectData2);
			return;
		}
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x0003A0FC File Offset: 0x000382FC
	private bool IsFullSendDue(in SynchronizedObjectSendState sendState)
	{
		return !sendState.HasLastSentData || (sendState.IsAwake && this.tickCount - sendState.LastFullSendTick >= (uint)this.tickRate);
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x0003A12A File Offset: 0x0003832A
	private uint GetFullSendTick(in SynchronizedObjectSendState sendState, ushort networkObjectId)
	{
		if (sendState.HasLastSentData)
		{
			return this.tickCount;
		}
		return this.tickCount - (uint)((int)networkObjectId % this.tickRate);
	}

	// Token: 0x04000779 RID: 1913
	private SynchronizedObjectLodSelector lodSelector = new SynchronizedObjectLodSelector();

	// Token: 0x0400077A RID: 1914
	private uint tickCount;

	// Token: 0x0400077B RID: 1915
	private int tickRate = 1;
}
