using System;
using System.Linq;

// Token: 0x020000D3 RID: 211
public struct ReconnectionState
{
	// Token: 0x1700009F RID: 159
	// (get) Token: 0x060006A7 RID: 1703 RVA: 0x000247C1 File Offset: 0x000229C1
	public string[] PendingModIds
	{
		get
		{
			return this.PendingReadinessModIds.Union(this.PendingEnablingModIds).ToArray<string>();
		}
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x000247D9 File Offset: 0x000229D9
	public ReconnectionState()
	{
		this.Phase = ReconnectionPhase.None;
		this.Password = null;
		this.ClientRequiredModIds = new string[0];
		this.PendingReadinessModIds = new string[0];
		this.PendingEnablingModIds = new string[0];
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x0002480D File Offset: 0x00022A0D
	public bool IsPendingModId(string modId)
	{
		return this.PendingReadinessModIds.Contains(modId) || this.PendingEnablingModIds.Contains(modId);
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x0002482C File Offset: 0x00022A2C
	public bool Equals(ReconnectionState other)
	{
		return this.Phase == other.Phase && this.Password == other.Password && this.ClientRequiredModIds.SequenceEqual(other.ClientRequiredModIds) && this.PendingReadinessModIds.SequenceEqual(other.PendingReadinessModIds) && this.PendingEnablingModIds.SequenceEqual(other.PendingEnablingModIds);
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x00024894 File Offset: 0x00022A94
	public override bool Equals(object obj)
	{
		if (obj is ReconnectionState)
		{
			ReconnectionState other = (ReconnectionState)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x000248B9 File Offset: 0x00022AB9
	public override int GetHashCode()
	{
		return HashCode.Combine<ReconnectionPhase, string, string[], string[], string[]>(this.Phase, this.Password, this.ClientRequiredModIds, this.PendingReadinessModIds, this.PendingEnablingModIds);
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x000248E0 File Offset: 0x00022AE0
	public override string ToString()
	{
		return string.Format("Phase: {0}, Password: {1}, ClientRequiredModIds: [{2}], PendingReadinessModIds: [{3}], PendingEnablingModIds: [{4}]", new object[]
		{
			this.Phase,
			this.Password ?? "null",
			string.Join(", ", this.ClientRequiredModIds),
			string.Join(", ", this.PendingReadinessModIds),
			string.Join(", ", this.PendingEnablingModIds)
		});
	}

	// Token: 0x0400044D RID: 1101
	public ReconnectionPhase Phase;

	// Token: 0x0400044E RID: 1102
	public string Password;

	// Token: 0x0400044F RID: 1103
	public string[] ClientRequiredModIds;

	// Token: 0x04000450 RID: 1104
	public string[] PendingReadinessModIds;

	// Token: 0x04000451 RID: 1105
	public string[] PendingEnablingModIds;
}
