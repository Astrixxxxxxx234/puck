using System;
using System.Text;
using System.Threading.Tasks;
using SuperSimpleTcp;

// Token: 0x0200023F RID: 575
public class TCPClient
{
	// Token: 0x1400000B RID: 11
	// (add) Token: 0x060011F8 RID: 4600 RVA: 0x00053F9C File Offset: 0x0005219C
	// (remove) Token: 0x060011F9 RID: 4601 RVA: 0x00053FD4 File Offset: 0x000521D4
	public event Action OnConnected;

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x060011FA RID: 4602 RVA: 0x0005400C File Offset: 0x0005220C
	// (remove) Token: 0x060011FB RID: 4603 RVA: 0x00054044 File Offset: 0x00052244
	public event Action OnConnectionFailed;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x060011FC RID: 4604 RVA: 0x0005407C File Offset: 0x0005227C
	// (remove) Token: 0x060011FD RID: 4605 RVA: 0x000540B4 File Offset: 0x000522B4
	public event Action OnDisconnected;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x060011FE RID: 4606 RVA: 0x000540EC File Offset: 0x000522EC
	// (remove) Token: 0x060011FF RID: 4607 RVA: 0x00054124 File Offset: 0x00052324
	public event Action<string> OnMessageReceived;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06001200 RID: 4608 RVA: 0x0005415C File Offset: 0x0005235C
	// (remove) Token: 0x06001201 RID: 4609 RVA: 0x00054194 File Offset: 0x00052394
	public event Action<string> OnMessageSent;

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06001202 RID: 4610 RVA: 0x000541C9 File Offset: 0x000523C9
	// (set) Token: 0x06001203 RID: 4611 RVA: 0x000541D1 File Offset: 0x000523D1
	public bool IsConnecting { get; private set; }

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06001204 RID: 4612 RVA: 0x000541DA File Offset: 0x000523DA
	public bool IsConnected
	{
		get
		{
			return this.Client.IsConnected;
		}
	}

	// Token: 0x06001205 RID: 4613 RVA: 0x000541E8 File Offset: 0x000523E8
	public TCPClient(EndPoint endPoint, int connectTimeoutMs = 1000, int readTimeoutMs = 1000)
	{
		this.EndPoint = endPoint;
		this.Client = new SimpleTcpClient(this.EndPoint.ipAddress, (int)this.EndPoint.port);
		this.Client.Settings.NoDelay = true;
		this.Client.Settings.UseAsyncDataReceivedEvents = false;
		this.Client.Settings.ConnectTimeoutMs = connectTimeoutMs;
		this.Client.Settings.ReadTimeoutMs = readTimeoutMs;
		this.Client.Events.Connected += delegate(object sender, ConnectionEventArgs args)
		{
			Action onConnected = this.OnConnected;
			if (onConnected == null)
			{
				return;
			}
			onConnected();
		};
		this.Client.Events.Disconnected += delegate(object sender, ConnectionEventArgs args)
		{
			Action onDisconnected = this.OnDisconnected;
			if (onDisconnected == null)
			{
				return;
			}
			onDisconnected();
		};
		this.Client.Events.DataReceived += delegate(object sender, DataReceivedEventArgs args)
		{
			this.OnDataReceived(sender, args);
		};
	}

	// Token: 0x06001206 RID: 4614 RVA: 0x000542BC File Offset: 0x000524BC
	public void Connect()
	{
		try
		{
			if (!this.IsConnecting)
			{
				this.IsConnecting = true;
				this.Client.Connect();
				this.IsConnecting = false;
			}
		}
		catch (TimeoutException)
		{
			this.IsConnecting = false;
			TCPClient.Logger.Error(string.Format("Connection to server {0} timed out", this.EndPoint));
			Action onConnectionFailed = this.OnConnectionFailed;
			if (onConnectionFailed != null)
			{
				onConnectionFailed();
			}
		}
		catch (Exception ex)
		{
			this.IsConnecting = false;
			TCPClient.Logger.Error(string.Format("Connection to server {0} failed: {1}", this.EndPoint, ex.Message));
			Action onConnectionFailed2 = this.OnConnectionFailed;
			if (onConnectionFailed2 != null)
			{
				onConnectionFailed2();
			}
		}
	}

	// Token: 0x06001207 RID: 4615 RVA: 0x00054380 File Offset: 0x00052580
	public void ConnectAsync()
	{
		Task.Run(delegate()
		{
			this.Connect();
		});
	}

	// Token: 0x06001208 RID: 4616 RVA: 0x00054394 File Offset: 0x00052594
	public void Disconnect()
	{
		try
		{
			this.Client.Disconnect();
		}
		catch (Exception ex)
		{
			TCPClient.Logger.Error("Error disconnecting: " + ex.Message);
		}
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x000543DC File Offset: 0x000525DC
	public void DisconnectAsync()
	{
		Task.Run(delegate()
		{
			this.Disconnect();
		});
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x000543F0 File Offset: 0x000525F0
	public void SendMessage(string message)
	{
		try
		{
			byte[] bytes = Encoding.UTF8.GetBytes(message);
			this.Client.Send(bytes);
			Action<string> onMessageSent = this.OnMessageSent;
			if (onMessageSent != null)
			{
				onMessageSent(message);
			}
		}
		catch (Exception ex)
		{
			TCPClient.Logger.Error(string.Format("Error sending message to server {0}: {1}", this.EndPoint, ex.Message));
		}
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x0005445C File Offset: 0x0005265C
	public void SendMessageAsync(string message)
	{
		Task.Run(delegate()
		{
			this.SendMessage(message);
		});
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x00054484 File Offset: 0x00052684
	private void OnDataReceived(object sender, DataReceivedEventArgs args)
	{
		try
		{
			string @string = Encoding.UTF8.GetString(args.Data);
			Action<string> onMessageReceived = this.OnMessageReceived;
			if (onMessageReceived != null)
			{
				onMessageReceived(@string);
			}
		}
		catch (Exception ex)
		{
			TCPClient.Logger.Error(string.Format("Error deserializing message from server {0}: {1}", this.EndPoint, ex.Message));
		}
	}

	// Token: 0x04000BAD RID: 2989
	private static readonly Logger Logger = new Logger("TCPClient");

	// Token: 0x04000BB3 RID: 2995
	public SimpleTcpClient Client;

	// Token: 0x04000BB4 RID: 2996
	public EndPoint EndPoint;
}
