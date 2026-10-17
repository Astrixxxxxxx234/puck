using System;

// Token: 0x02000275 RID: 629
public class EndPoint : IEquatable<EndPoint>
{
	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06001301 RID: 4865 RVA: 0x00056A13 File Offset: 0x00054C13
	// (set) Token: 0x06001302 RID: 4866 RVA: 0x00056A1B File Offset: 0x00054C1B
	public string ipAddress { get; set; }

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06001303 RID: 4867 RVA: 0x00056A24 File Offset: 0x00054C24
	// (set) Token: 0x06001304 RID: 4868 RVA: 0x00056A2C File Offset: 0x00054C2C
	public ushort port { get; set; }

	// Token: 0x06001305 RID: 4869 RVA: 0x00056A35 File Offset: 0x00054C35
	public EndPoint(string ipAddress, ushort port)
	{
		this.ipAddress = ipAddress;
		this.port = port;
	}

	// Token: 0x06001306 RID: 4870 RVA: 0x00056A4B File Offset: 0x00054C4B
	public bool Equals(EndPoint other)
	{
		return other != null && this.ipAddress == other.ipAddress && this.port == other.port;
	}

	// Token: 0x06001307 RID: 4871 RVA: 0x00056A78 File Offset: 0x00054C78
	public override bool Equals(object obj)
	{
		EndPoint endPoint = obj as EndPoint;
		return endPoint != null && this.Equals(endPoint);
	}

	// Token: 0x06001308 RID: 4872 RVA: 0x00056A98 File Offset: 0x00054C98
	public static bool operator ==(EndPoint a, EndPoint b)
	{
		if (a == null)
		{
			return b == null;
		}
		return a.Equals(b);
	}

	// Token: 0x06001309 RID: 4873 RVA: 0x00056AA9 File Offset: 0x00054CA9
	public static bool operator !=(EndPoint a, EndPoint b)
	{
		return !(a == b);
	}

	// Token: 0x0600130A RID: 4874 RVA: 0x00056AB5 File Offset: 0x00054CB5
	public override int GetHashCode()
	{
		return HashCode.Combine<string, ushort>(this.ipAddress, this.port);
	}

	// Token: 0x0600130B RID: 4875 RVA: 0x00056AC8 File Offset: 0x00054CC8
	public override string ToString()
	{
		return string.Format("{0}:{1}", this.ipAddress, this.port);
	}
}
