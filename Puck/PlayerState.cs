using System;

// Token: 0x020000AA RID: 170
public struct PlayerState
{
	// Token: 0x06000598 RID: 1432 RVA: 0x0001ECCC File Offset: 0x0001CECC
	public bool Equals(PlayerState other)
	{
		return this.AuthenticationPhase == other.AuthenticationPhase && this.PlayerData == other.PlayerData && this.PartyData == other.PartyData && this.GroupData == other.GroupData && this.MatchData == other.MatchData && this.PlayerStatistics == other.PlayerStatistics && this.Key == other.Key;
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x0001ED40 File Offset: 0x0001CF40
	public override bool Equals(object obj)
	{
		if (obj is PlayerState)
		{
			PlayerState other = (PlayerState)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x0001ED65 File Offset: 0x0001CF65
	public override int GetHashCode()
	{
		return HashCode.Combine<AuthenticationPhase, PlayerData, PlayerPartyData, PlayerGroupData, PlayerMatchData, PlayerStatistics, string>(this.AuthenticationPhase, this.PlayerData, this.PartyData, this.GroupData, this.MatchData, this.PlayerStatistics, this.Key);
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x0001ED98 File Offset: 0x0001CF98
	public override string ToString()
	{
		return string.Format("AuthenticationPhase: {0}, PlayerData: {1}, PartyData: {2}, GroupData: {3}, MatchData: {4}, PlayerStatistics: {5}, Key: {6}", new object[]
		{
			this.AuthenticationPhase,
			this.PlayerData,
			this.PartyData,
			this.GroupData,
			this.MatchData,
			this.PlayerStatistics,
			this.Key
		});
	}

	// Token: 0x0400039F RID: 927
	public AuthenticationPhase AuthenticationPhase;

	// Token: 0x040003A0 RID: 928
	public PlayerData PlayerData;

	// Token: 0x040003A1 RID: 929
	public PlayerPartyData PartyData;

	// Token: 0x040003A2 RID: 930
	public PlayerGroupData GroupData;

	// Token: 0x040003A3 RID: 931
	public PlayerMatchData MatchData;

	// Token: 0x040003A4 RID: 932
	public PlayerStatistics PlayerStatistics;

	// Token: 0x040003A5 RID: 933
	public string Key;
}
