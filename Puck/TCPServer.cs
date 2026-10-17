using System;
using System.Text;
using System.Threading.Tasks;
using SuperSimpleTcp;

// Token: 0x02000241 RID: 577
public class TCPServer
{
	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06001215 RID: 4629 RVA: 0x00054554 File Offset: 0x00052754
	// (remove) Token: 0x06001216 RID: 4630 RVA: 0x0005458C File Offset: 0x0005278C
	public event Action<ushort> OnServerStarted;

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06001217 RID: 4631 RVA: 0x000545C4 File Offset: 0x000527C4
	// (remove) Token: 0x06001218 RID: 4632 RVA: 0x000545FC File Offset: 0x000527FC
	public event Action<Exception> OnServerStartFailed;

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06001219 RID: 4633 RVA: 0x00054634 File Offset: 0x00052834
	// (remove) Token: 0x0600121A RID: 4634 RVA: 0x0005466C File Offset: 0x0005286C
	public event Action<ushort> OnServerStopped;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x0600121B RID: 4635 RVA: 0x000546A4 File Offset: 0x000528A4
	// (remove) Token: 0x0600121C RID: 4636 RVA: 0x000546DC File Offset: 0x000528DC
	public event Action<string> OnClientConnected;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x0600121D RID: 4637 RVA: 0x00054714 File Offset: 0x00052914
	// (remove) Token: 0x0600121E RID: 4638 RVA: 0x0005474C File Offset: 0x0005294C
	public event Action<string> OnClientDisconnected;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x0600121F RID: 4639 RVA: 0x00054784 File Offset: 0x00052984
	// (remove) Token: 0x06001220 RID: 4640 RVA: 0x000547BC File Offset: 0x000529BC
	public event Action<string, string> OnMessageReceived;

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x06001221 RID: 4641 RVA: 0x000547F4 File Offset: 0x000529F4
	// (remove) Token: 0x06001222 RID: 4642 RVA: 0x0005482C File Offset: 0x00052A2C
	public event Action<string, string> OnMessageSent;

	// Token: 0x06001223 RID: 4643 RVA: 0x00054864 File Offset: 0x00052A64
	public TCPServer(ushort port)
	{
		this.Server = new SimpleTcpServer("0.0.0.0", (int)port);
		this.Server.Settings.IdleClientTimeoutMs = 1000;
		this.Server.Settings.NoDelay = true;
		this.Server.Settings.UseAsyncDataReceivedEvents = false;
		this.Server.Events.ClientConnected += delegate(object sender, ConnectionEventArgs args)
		{
			Action<string> onClientConnected = this.OnClientConnected;
			if (onClientConnected == null)
			{
				return;
			}
			onClientConnected(args.IpPort);
		};
		this.Server.Events.ClientDisconnected += delegate(object sender, ConnectionEventArgs args)
		{
			Action<string> onClientDisconnected = this.OnClientDisconnected;
			if (onClientDisconnected == null)
			{
				return;
			}
			onClientDisconnected(args.IpPort);
		};
		this.Server.Events.DataReceived += delegate(object sender, DataReceivedEventArgs args)
		{
			this.OnDataReceived(sender, args);
		};
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x00054914 File Offset: 0x00052B14
	public void Start()
	{
		try
		{
			if (!this.Server.IsListening)
			{
				this.Server.Start();
				TCPServer.Logger.Info(string.Format("Started TCP listener on {0}:{1}", "0.0.0.0", this.Server.Port));
				Action<ushort> onServerStarted = this.OnServerStarted;
				if (onServerStarted != null)
				{
					onServerStarted((ushort)this.Server.Port);
				}
			}
		}
		catch (Exception ex)
		{
			TCPServer.Logger.Error(string.Format("Server start failed on port {0}: {1}", this.Server.Port, ex.Message));
			Action<Exception> onServerStartFailed = this.OnServerStartFailed;
			if (onServerStartFailed != null)
			{
				onServerStartFailed(ex);
			}
		}
	}

	// Token: 0x06001225 RID: 4645 RVA: 0x000549D8 File Offset: 0x00052BD8
	public void StartAsync()
	{
		Task.Run(delegate()
		{
			this.Start();
		});
	}

	// Token: 0x06001226 RID: 4646 RVA: 0x000549EC File Offset: 0x00052BEC
	public void Stop()
	{
		this.Server.Stop();
		TCPServer.Logger.Info(string.Format("Server stopped on port {0}", this.Server.Port));
		Action<ushort> onServerStopped = this.OnServerStopped;
		if (onServerStopped == null)
		{
			return;
		}
		onServerStopped((ushort)this.Server.Port);
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x00054A44 File Offset: 0x00052C44
	public void StopAsync()
	{
		Task.Run(delegate()
		{
			this.Stop();
		});
	}

	// Token: 0x06001228 RID: 4648 RVA: 0x00054A58 File Offset: 0x00052C58
	public void SendMessage(string ipPort, string message)
	{
		try
		{
			byte[] bytes = Encoding.UTF8.GetBytes(message);
			this.Server.Send(ipPort, bytes);
			Action<string, string> onMessageSent = this.OnMessageSent;
			if (onMessageSent != null)
			{
				onMessageSent(ipPort, message);
			}
		}
		catch (Exception ex)
		{
			TCPServer.Logger.Error("Error sending message to client " + ipPort + ": " + ex.Message);
		}
	}

	// Token: 0x06001229 RID: 4649 RVA: 0x00054AC8 File Offset: 0x00052CC8
	public void SendMessageAsync(string ipPort, string message)
	{
		Task.Run(delegate()
		{
			this.SendMessage(ipPort, message);
		});
	}

	// Token: 0x0600122A RID: 4650 RVA: 0x00054AF8 File Offset: 0x00052CF8
	public void DisconnectClient(string ipPort)
	{
		try
		{
			this.Server.DisconnectClient(ipPort);
		}
		catch (Exception ex)
		{
			TCPServer.Logger.Error("Error disconnecting client " + ipPort + ": " + ex.Message);
		}
	}

	// Token: 0x0600122B RID: 4651 RVA: 0x00054B48 File Offset: 0x00052D48
	private void OnDataReceived(object sender, DataReceivedEventArgs args)
	{
		try
		{
			string @string = Encoding.UTF8.GetString(args.Data);
			Action<string, string> onMessageReceived = this.OnMessageReceived;
			if (onMessageReceived != null)
			{
				onMessageReceived(args.IpPort, @string);
			}
		}
		catch (Exception ex)
		{
			TCPServer.Logger.Error("Error deserializing message from client " + args.IpPort + ": " + ex.Message);
		}
	}

	// Token: 0x04000BB8 RID: 3000
	private static readonly Logger Logger = new Logger("TCPServer");

	// Token: 0x04000BC0 RID: 3008
	public SimpleTcpServer Server;
}
