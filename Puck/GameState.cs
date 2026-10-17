using System;
using Unity.Netcode;

// Token: 0x020000C7 RID: 199
public struct GameState : INetworkSerializable, IEquatable<GameState>
{
	// Token: 0x06000647 RID: 1607 RVA: 0x00022A40 File Offset: 0x00020C40
	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		if (serializer.IsReader)
		{
			FastBufferReader fastBufferReader = serializer.GetFastBufferReader();
			fastBufferReader.ReadValueSafe<GamePhase>(out this.Phase, default(FastBufferWriter.ForEnums));
			fastBufferReader.ReadValueSafe<int>(out this.Tick, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.Period, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.BlueScore, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.RedScore, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<bool>(out this.IsOvertime, default(FastBufferWriter.ForPrimitives));
			return;
		}
		FastBufferWriter fastBufferWriter = serializer.GetFastBufferWriter();
		fastBufferWriter.WriteValueSafe<GamePhase>(this.Phase, default(FastBufferWriter.ForEnums));
		fastBufferWriter.WriteValueSafe<int>(this.Tick, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.Period, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.BlueScore, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.RedScore, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<bool>(this.IsOvertime, default(FastBufferWriter.ForPrimitives));
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x00022B74 File Offset: 0x00020D74
	public bool Equals(GameState other)
	{
		return this.Phase.Equals(other.Phase) && this.Tick == other.Tick && this.Period == other.Period && this.BlueScore == other.BlueScore && this.RedScore == other.RedScore && this.IsOvertime == other.IsOvertime;
	}

	// Token: 0x04000419 RID: 1049
	public GamePhase Phase;

	// Token: 0x0400041A RID: 1050
	public int Tick;

	// Token: 0x0400041B RID: 1051
	public int Period;

	// Token: 0x0400041C RID: 1052
	public int BlueScore;

	// Token: 0x0400041D RID: 1053
	public int RedScore;

	// Token: 0x0400041E RID: 1054
	public bool IsOvertime;
}
