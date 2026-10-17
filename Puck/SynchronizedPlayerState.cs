using System;
using System.Collections.Generic;
using Unity.Netcode;

// Token: 0x0200018E RID: 398
public class SynchronizedPlayerState
{
	// Token: 0x06000C53 RID: 3155 RVA: 0x0003AA04 File Offset: 0x00038C04
	public SynchronizedObjectSendState GetSendState(ulong networkObjectId)
	{
		SynchronizedObjectSendState result;
		if (this.sendStates.TryGetValue(networkObjectId, out result))
		{
			return result;
		}
		return new SynchronizedObjectSendState
		{
			LodSelection = SynchronizedObjectLodSelection.Default
		};
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x0003AA38 File Offset: 0x00038C38
	public void SetSendState(ulong networkObjectId, SynchronizedObjectSendState sendState)
	{
		this.sendStates[networkObjectId] = sendState;
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x0003AA47 File Offset: 0x00038C47
	public void Forget(ulong networkObjectId)
	{
		this.sendStates.Remove(networkObjectId);
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x0003AA56 File Offset: 0x00038C56
	public void Dispose()
	{
		BaseRpcTarget rpcTarget = this.RpcTarget;
		if (rpcTarget != null)
		{
			rpcTarget.Dispose();
		}
		this.RpcTarget = null;
	}

	// Token: 0x040007A4 RID: 1956
	public Player Player;

	// Token: 0x040007A5 RID: 1957
	public BaseRpcTarget RpcTarget;

	// Token: 0x040007A6 RID: 1958
	public ushort SendSequenceNumber;

	// Token: 0x040007A7 RID: 1959
	public SynchronizedObjectTickHeader LastSentTickHeader;

	// Token: 0x040007A8 RID: 1960
	public bool HasSentTickHeader;

	// Token: 0x040007A9 RID: 1961
	private Dictionary<ulong, SynchronizedObjectSendState> sendStates = new Dictionary<ulong, SynchronizedObjectSendState>();
}
