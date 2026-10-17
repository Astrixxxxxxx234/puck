using System;

// Token: 0x020000AB RID: 171
public struct ServerState
{
	// Token: 0x0600059C RID: 1436 RVA: 0x0001EDF9 File Offset: 0x0001CFF9
	public bool Equals(ServerState other)
	{
		return this.AuthenticationPhase == other.AuthenticationPhase && this.ServerData == other.ServerData && this.MatchData == other.MatchData;
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x0001EE28 File Offset: 0x0001D028
	public override bool Equals(object obj)
	{
		if (obj is ServerState)
		{
			ServerState other = (ServerState)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x0001EE4D File Offset: 0x0001D04D
	public override int GetHashCode()
	{
		return HashCode.Combine<AuthenticationPhase, ServerData, ServerMatchData>(this.AuthenticationPhase, this.ServerData, this.MatchData);
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x0001EE66 File Offset: 0x0001D066
	public override string ToString()
	{
		return string.Format("AuthenticationPhase: {0}, ServerData: {1}, MatchData: {2}", this.AuthenticationPhase, this.ServerData, this.MatchData);
	}

	// Token: 0x040003A6 RID: 934
	public AuthenticationPhase AuthenticationPhase;

	// Token: 0x040003A7 RID: 935
	public ServerData ServerData;

	// Token: 0x040003A8 RID: 936
	public ServerMatchData MatchData;
}
