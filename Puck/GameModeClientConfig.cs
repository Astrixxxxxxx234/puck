using System;
using Unity.Netcode;

// Token: 0x020000CC RID: 204
public struct GameModeClientConfig : INetworkSerializable, IEquatable<GameModeClientConfig>
{
	// Token: 0x0600067D RID: 1661 RVA: 0x00023EA4 File Offset: 0x000220A4
	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		serializer.SerializeValue<bool>(ref this.GoalieCreaseProtection, default(FastBufferWriter.ForPrimitives));
		serializer.SerializeValue<int>(ref this.MaxPeriods, default(FastBufferWriter.ForPrimitives));
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x00023EDD File Offset: 0x000220DD
	public bool Equals(GameModeClientConfig other)
	{
		return this.GoalieCreaseProtection == other.GoalieCreaseProtection && this.MaxPeriods == other.MaxPeriods;
	}

	// Token: 0x0400042C RID: 1068
	public bool GoalieCreaseProtection;

	// Token: 0x0400042D RID: 1069
	public int MaxPeriods;
}
