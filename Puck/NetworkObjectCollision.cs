using System;
using Unity.Netcode;

// Token: 0x02000021 RID: 33
public struct NetworkObjectCollision : INetworkSerializable, IEquatable<NetworkObjectCollision>
{
	// Token: 0x060000A6 RID: 166 RVA: 0x00003E88 File Offset: 0x00002088
	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		if (serializer.IsReader)
		{
			FastBufferReader fastBufferReader = serializer.GetFastBufferReader();
			fastBufferReader.ReadValueSafe<NetworkObjectReference>(out this.NetworkObjectReference, default(FastBufferWriter.ForNetworkSerializable));
			fastBufferReader.ReadValueSafe<float>(out this.Time, default(FastBufferWriter.ForPrimitives));
			return;
		}
		FastBufferWriter fastBufferWriter = serializer.GetFastBufferWriter();
		fastBufferWriter.WriteValueSafe<NetworkObjectReference>(this.NetworkObjectReference, default(FastBufferWriter.ForNetworkSerializable));
		fastBufferWriter.WriteValueSafe<float>(this.Time, default(FastBufferWriter.ForPrimitives));
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00003F07 File Offset: 0x00002107
	public bool Equals(NetworkObjectCollision other)
	{
		return this.NetworkObjectReference.Equals(other.NetworkObjectReference) && this.Time == other.Time;
	}

	// Token: 0x04000054 RID: 84
	public NetworkObjectReference NetworkObjectReference;

	// Token: 0x04000055 RID: 85
	public float Time;
}
