using System;
using Unity.Netcode;

// Token: 0x02000059 RID: 89
public struct PlayerGameState : INetworkSerializable, IEquatable<PlayerGameState>
{
	// Token: 0x06000275 RID: 629 RVA: 0x0001023C File Offset: 0x0000E43C
	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		if (serializer.IsReader)
		{
			FastBufferReader fastBufferReader = serializer.GetFastBufferReader();
			fastBufferReader.ReadValueSafe<PlayerPhase>(out this.Phase, default(FastBufferWriter.ForEnums));
			fastBufferReader.ReadValueSafe<PlayerTeam>(out this.Team, default(FastBufferWriter.ForEnums));
			fastBufferReader.ReadValueSafe<PlayerRole>(out this.Role, default(FastBufferWriter.ForEnums));
			return;
		}
		FastBufferWriter fastBufferWriter = serializer.GetFastBufferWriter();
		fastBufferWriter.WriteValueSafe<PlayerPhase>(this.Phase, default(FastBufferWriter.ForEnums));
		fastBufferWriter.WriteValueSafe<PlayerTeam>(this.Team, default(FastBufferWriter.ForEnums));
		fastBufferWriter.WriteValueSafe<PlayerRole>(this.Role, default(FastBufferWriter.ForEnums));
	}

	// Token: 0x06000276 RID: 630 RVA: 0x000102E7 File Offset: 0x0000E4E7
	public bool Equals(PlayerGameState other)
	{
		return this.Phase == other.Phase && this.Team == other.Team && this.Role == other.Role;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00010315 File Offset: 0x0000E515
	public override string ToString()
	{
		return string.Format("Phase: {0}, Team: {1}, Role: {2}", this.Phase, this.Team, this.Role);
	}

	// Token: 0x040001DE RID: 478
	public PlayerPhase Phase;

	// Token: 0x040001DF RID: 479
	public PlayerTeam Team;

	// Token: 0x040001E0 RID: 480
	public PlayerRole Role;
}
