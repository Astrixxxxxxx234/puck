using System;

// Token: 0x020000AD RID: 173
public struct TransactionState
{
	// Token: 0x060005A0 RID: 1440 RVA: 0x0001EE89 File Offset: 0x0001D089
	public bool Equals(TransactionState other)
	{
		return this.Phase == other.Phase;
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x0001EE9C File Offset: 0x0001D09C
	public override bool Equals(object obj)
	{
		if (obj is TransactionState)
		{
			TransactionState other = (TransactionState)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x0001EEC1 File Offset: 0x0001D0C1
	public override int GetHashCode()
	{
		return HashCode.Combine<TransactionPhase>(this.Phase);
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x0001EECE File Offset: 0x0001D0CE
	public override string ToString()
	{
		return string.Format("Phase: {0}", this.Phase);
	}

	// Token: 0x040003AD RID: 941
	public TransactionPhase Phase;
}
