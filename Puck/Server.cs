using System;
using Unity.Collections;
using Unity.Netcode;

// Token: 0x02000154 RID: 340
public struct Server : INetworkSerializable, IEquatable<Server>
{
	// Token: 0x060009F1 RID: 2545 RVA: 0x0002F8FC File Offset: 0x0002DAFC
	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		if (serializer.IsReader)
		{
			FastBufferReader fastBufferReader = serializer.GetFastBufferReader();
			fastBufferReader.ReadValueSafe<FixedString32Bytes>(out this.IpAddress, default(FastBufferWriter.ForFixedStrings));
			fastBufferReader.ReadValueSafe<ushort>(out this.Port, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<FixedString128Bytes>(out this.Name, default(FastBufferWriter.ForFixedStrings));
			fastBufferReader.ReadValueSafe<int>(out this.MaxPlayers, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.TickRate, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<bool>(out this.UseVoip, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<FixedString64Bytes>(out this.GameMode, default(FastBufferWriter.ForFixedStrings));
			return;
		}
		FastBufferWriter fastBufferWriter = serializer.GetFastBufferWriter();
		fastBufferWriter.WriteValueSafe<FixedString32Bytes>(this.IpAddress, default(FastBufferWriter.ForFixedStrings));
		fastBufferWriter.WriteValueSafe<ushort>(this.Port, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<FixedString128Bytes>(this.Name, default(FastBufferWriter.ForFixedStrings));
		fastBufferWriter.WriteValueSafe<int>(this.MaxPlayers, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.TickRate, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<bool>(this.UseVoip, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<FixedString64Bytes>(this.GameMode, default(FastBufferWriter.ForFixedStrings));
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x0002FA5C File Offset: 0x0002DC5C
	public bool Equals(Server other)
	{
		return this.IpAddress == other.IpAddress && this.Port == other.Port && this.Name == other.Name && this.MaxPlayers == other.MaxPlayers && this.TickRate == other.TickRate && this.UseVoip == other.UseVoip && this.GameMode == other.GameMode;
	}

	// Token: 0x04000615 RID: 1557
	public FixedString32Bytes IpAddress;

	// Token: 0x04000616 RID: 1558
	public ushort Port;

	// Token: 0x04000617 RID: 1559
	public FixedString128Bytes Name;

	// Token: 0x04000618 RID: 1560
	public int MaxPlayers;

	// Token: 0x04000619 RID: 1561
	public int TickRate;

	// Token: 0x0400061A RID: 1562
	public bool UseVoip;

	// Token: 0x0400061B RID: 1563
	public FixedString64Bytes GameMode;
}
