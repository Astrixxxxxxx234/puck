using System;
using Unity.Netcode;

// Token: 0x0200018C RID: 396
public struct SynchronizedObjectTickHeader : INetworkSerializable
{
	// Token: 0x17000140 RID: 320
	// (get) Token: 0x06000C40 RID: 3136 RVA: 0x0003A69D File Offset: 0x0003889D
	public bool HasTimeScale
	{
		get
		{
			return (this.ChangeMask & 1) > 0;
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06000C41 RID: 3137 RVA: 0x0003A6AA File Offset: 0x000388AA
	public bool HasTickInterval
	{
		get
		{
			return (this.ChangeMask & 2) > 0;
		}
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x0003A6B7 File Offset: 0x000388B7
	public SynchronizedObjectTickHeader(ushort sequenceNumber, double serverTime, float timeScale, float tickInterval)
	{
		this.ChangeMask = 3;
		this.SequenceNumber = sequenceNumber;
		this.ServerTime = serverTime;
		this.TimeScale = timeScale;
		this.TickInterval = tickInterval;
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x0003A6E0 File Offset: 0x000388E0
	public byte GetChangeMask(SynchronizedObjectTickHeader other)
	{
		byte b = 0;
		if (this.TimeScale != other.TimeScale)
		{
			b |= 1;
		}
		if (this.TickInterval != other.TickInterval)
		{
			b |= 2;
		}
		return b;
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x0003A718 File Offset: 0x00038918
	public SynchronizedObjectTickHeader WithComponentMask(byte componentMask)
	{
		SynchronizedObjectTickHeader result = this;
		result.ChangeMask = (componentMask & 3);
		return result;
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x0003A738 File Offset: 0x00038938
	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		if (serializer.IsReader)
		{
			this.Read(serializer.GetFastBufferReader());
			return;
		}
		this.Write(serializer.GetFastBufferWriter());
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x0003A760 File Offset: 0x00038960
	private void Read(FastBufferReader reader)
	{
		reader.ReadValueSafe<byte>(out this.ChangeMask, default(FastBufferWriter.ForPrimitives));
		reader.ReadValueSafe<ushort>(out this.SequenceNumber, default(FastBufferWriter.ForPrimitives));
		reader.ReadValueSafe<double>(out this.ServerTime, default(FastBufferWriter.ForPrimitives));
		if (this.HasTimeScale)
		{
			reader.ReadValueSafe<float>(out this.TimeScale, default(FastBufferWriter.ForPrimitives));
		}
		if (this.HasTickInterval)
		{
			reader.ReadValueSafe<float>(out this.TickInterval, default(FastBufferWriter.ForPrimitives));
		}
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x0003A7EC File Offset: 0x000389EC
	private void Write(FastBufferWriter writer)
	{
		writer.WriteValueSafe<byte>(this.ChangeMask, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<ushort>(this.SequenceNumber, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<double>(this.ServerTime, default(FastBufferWriter.ForPrimitives));
		if (this.HasTimeScale)
		{
			writer.WriteValueSafe<float>(this.TimeScale, default(FastBufferWriter.ForPrimitives));
		}
		if (this.HasTickInterval)
		{
			writer.WriteValueSafe<float>(this.TickInterval, default(FastBufferWriter.ForPrimitives));
		}
	}

	// Token: 0x04000795 RID: 1941
	public const byte CHANGE_MASK_TIME_SCALE = 1;

	// Token: 0x04000796 RID: 1942
	public const byte CHANGE_MASK_TICK_INTERVAL = 2;

	// Token: 0x04000797 RID: 1943
	public const byte CHANGE_MASK_ALL_COMPONENTS = 3;

	// Token: 0x04000798 RID: 1944
	public byte ChangeMask;

	// Token: 0x04000799 RID: 1945
	public ushort SequenceNumber;

	// Token: 0x0400079A RID: 1946
	public double ServerTime;

	// Token: 0x0400079B RID: 1947
	public float TimeScale;

	// Token: 0x0400079C RID: 1948
	public float TickInterval;
}
