using System;

// Token: 0x020000D1 RID: 209
public struct ConnectionState
{
	// Token: 0x060006A2 RID: 1698 RVA: 0x0002461D File Offset: 0x0002281D
	public ConnectionState()
	{
		this.Connection = null;
		this.ConnectionRejection = null;
		this.Disconnection = null;
		this.LastConnection = null;
		this.PendingConnection = null;
		this.Phase = ConnectionPhase.Disconnected;
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0002464C File Offset: 0x0002284C
	public bool Equals(ConnectionState other)
	{
		return this.Connection == other.Connection && this.LastConnection == other.LastConnection && this.ConnectionRejection == other.ConnectionRejection && this.Disconnection == other.Disconnection && this.PendingConnection == other.PendingConnection && this.Phase == other.Phase;
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x000246B0 File Offset: 0x000228B0
	public override bool Equals(object obj)
	{
		if (obj is ConnectionState)
		{
			ConnectionState other = (ConnectionState)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x000246D5 File Offset: 0x000228D5
	public override int GetHashCode()
	{
		return HashCode.Combine<Connection, Connection, ConnectionRejection, Disconnection, Connection, ConnectionPhase>(this.Connection, this.LastConnection, this.ConnectionRejection, this.Disconnection, this.PendingConnection, this.Phase);
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x00024700 File Offset: 0x00022900
	public override string ToString()
	{
		string format = "Connection: {0}, LastConnection: {1}, ConnectionRejection: {2}, Disconnection: {3}, PendingConnection: {4}, Phase: {5}";
		object[] array = new object[6];
		int num = 0;
		Connection connection = this.Connection;
		array[num] = (((connection != null) ? connection.ToString() : null) ?? "null");
		int num2 = 1;
		Connection lastConnection = this.LastConnection;
		array[num2] = (((lastConnection != null) ? lastConnection.ToString() : null) ?? "null");
		int num3 = 2;
		ConnectionRejection connectionRejection = this.ConnectionRejection;
		array[num3] = (((connectionRejection != null) ? connectionRejection.ToString() : null) ?? "null");
		int num4 = 3;
		Disconnection disconnection = this.Disconnection;
		array[num4] = (((disconnection != null) ? disconnection.ToString() : null) ?? "null");
		int num5 = 4;
		Connection pendingConnection = this.PendingConnection;
		array[num5] = (((pendingConnection != null) ? pendingConnection.ToString() : null) ?? "null");
		array[5] = this.Phase;
		return string.Format(format, array);
	}

	// Token: 0x04000443 RID: 1091
	public Connection Connection;

	// Token: 0x04000444 RID: 1092
	public ConnectionRejection ConnectionRejection;

	// Token: 0x04000445 RID: 1093
	public Disconnection Disconnection;

	// Token: 0x04000446 RID: 1094
	public Connection LastConnection;

	// Token: 0x04000447 RID: 1095
	public Connection PendingConnection;

	// Token: 0x04000448 RID: 1096
	public ConnectionPhase Phase;
}
