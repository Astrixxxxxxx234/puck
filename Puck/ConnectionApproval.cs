using System;
using Unity.Netcode;

// Token: 0x02000149 RID: 329
public class ConnectionApproval
{
	// Token: 0x17000101 RID: 257
	// (get) Token: 0x0600098A RID: 2442 RVA: 0x0002E6CE File Offset: 0x0002C8CE
	public ulong ClientID
	{
		get
		{
			return this.Request.ClientNetworkId;
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x0600098B RID: 2443 RVA: 0x0002E6DB File Offset: 0x0002C8DB
	public bool IsHost
	{
		get
		{
			return this.ClientID == 0UL;
		}
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x0002E6E7 File Offset: 0x0002C8E7
	public void Halt()
	{
		this.IsInProgress = true;
		this.Response.Pending = this.IsInProgress;
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x0002E701 File Offset: 0x0002C901
	public void Approve(PlayerData playerData)
	{
		this.PlayerData = playerData;
		this.IsApproved = true;
		this.IsInProgress = false;
		this.Response.Approved = this.IsApproved;
		this.Response.Pending = this.IsInProgress;
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x0002E73A File Offset: 0x0002C93A
	public void Reject(string reason)
	{
		this.IsApproved = false;
		this.IsInProgress = false;
		this.Response.Reason = reason;
		this.Response.Approved = this.IsApproved;
		this.Response.Pending = this.IsInProgress;
	}

	// Token: 0x040005E4 RID: 1508
	public NetworkManager.ConnectionApprovalRequest Request;

	// Token: 0x040005E5 RID: 1509
	public NetworkManager.ConnectionApprovalResponse Response;

	// Token: 0x040005E6 RID: 1510
	public ConnectionData ConnectionData;

	// Token: 0x040005E7 RID: 1511
	public PlayerData PlayerData;

	// Token: 0x040005E8 RID: 1512
	public string IpAddress;

	// Token: 0x040005E9 RID: 1513
	public bool IsApproved;

	// Token: 0x040005EA RID: 1514
	public bool IsInProgress;
}
