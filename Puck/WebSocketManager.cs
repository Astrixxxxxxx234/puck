using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SocketIOClient;
using SocketIOClient.Common;
using SocketIOClient.Common.Messages;
using SocketIOClient.Protocol.WebSocket;
using UnityEngine;

// Token: 0x0200019E RID: 414
public static class WebSocketManager
{
	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x0003CF34 File Offset: 0x0003B134
	public static bool IsConnected
	{
		get
		{
			return WebSocketManager.socket != null && WebSocketManager.socket.Connected;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x0003CF49 File Offset: 0x0003B149
	public static bool IsReconnecting
	{
		get
		{
			return WebSocketManager.IsConnectionInProgress && !WebSocketManager.IsConnected;
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x0003CF5C File Offset: 0x0003B15C
	public static bool IsConnectionInProgress
	{
		get
		{
			return WebSocketManager.cancellationTokenSource != null;
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06000CDA RID: 3290 RVA: 0x0003CF68 File Offset: 0x0003B168
	public static string BackendUrl
	{
		get
		{
			TextAsset textAsset = Resources.Load<TextAsset>("backend_url");
			if (textAsset == null)
			{
				WebSocketManager.Logger.Error("Error loading backend url asset, falling back to ws://localhost:8080");
				return "ws://localhost:8080";
			}
			return textAsset.text.Trim();
		}
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x0003CFAC File Offset: 0x0003B1AC
	public static void Initialize()
	{
		bool flag;
		WebSocketManager.forcePolling = (bool.TryParse(Utils.GetCommandLineArgument("--polling", null), out flag) && flag);
		WebSocketManagerController.Initialize();
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x0003CFDB File Offset: 0x0003B1DB
	public static void Dispose()
	{
		WebSocketManagerController.Dispose();
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x0003CFE4 File Offset: 0x0003B1E4
	public static Task Connect(string targetUrl)
	{
		WebSocketManager.<Connect>d__22 <Connect>d__;
		<Connect>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<Connect>d__.targetUrl = targetUrl;
		<Connect>d__.<>1__state = -1;
		<Connect>d__.<>t__builder.Start<WebSocketManager.<Connect>d__22>(ref <Connect>d__);
		return <Connect>d__.<>t__builder.Task;
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x0003D028 File Offset: 0x0003B228
	public static Task CancelConnection()
	{
		WebSocketManager.<CancelConnection>d__23 <CancelConnection>d__;
		<CancelConnection>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<CancelConnection>d__.<>1__state = -1;
		<CancelConnection>d__.<>t__builder.Start<WebSocketManager.<CancelConnection>d__23>(ref <CancelConnection>d__);
		return <CancelConnection>d__.<>t__builder.Task;
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x0003D064 File Offset: 0x0003B264
	public static Task Disconnect()
	{
		WebSocketManager.<Disconnect>d__24 <Disconnect>d__;
		<Disconnect>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<Disconnect>d__.<>1__state = -1;
		<Disconnect>d__.<>t__builder.Start<WebSocketManager.<Disconnect>d__24>(ref <Disconnect>d__);
		return <Disconnect>d__.<>t__builder.Task;
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x0003D0A0 File Offset: 0x0003B2A0
	private static int GetReconnectDelay(int attempt)
	{
		double num = Math.Min(500.0 * Math.Pow(2.0, (double)(attempt - 1)), 30000.0);
		return (int)(num / 2.0 + WebSocketManager.reconnectRandom.NextDouble() * (num / 2.0));
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x0003D0FC File Offset: 0x0003B2FC
	private static void StartWatchdog()
	{
		WebSocketManager.StopWatchdog();
		Interlocked.Exchange(ref WebSocketManager.lastServerPingTicks, DateTime.UtcNow.Ticks);
		WebSocketManager.watchdogCancellationTokenSource = new CancellationTokenSource();
		WebSocketManager.RunWatchdog(WebSocketManager.watchdogCancellationTokenSource.Token);
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x0003D140 File Offset: 0x0003B340
	private static void StopWatchdog()
	{
		if (WebSocketManager.watchdogCancellationTokenSource == null)
		{
			return;
		}
		WebSocketManager.watchdogCancellationTokenSource.Cancel();
		WebSocketManager.watchdogCancellationTokenSource = null;
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x0003D15C File Offset: 0x0003B35C
	private static Task RunWatchdog(CancellationToken token)
	{
		WebSocketManager.<RunWatchdog>d__28 <RunWatchdog>d__;
		<RunWatchdog>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<RunWatchdog>d__.token = token;
		<RunWatchdog>d__.<>1__state = -1;
		<RunWatchdog>d__.<>t__builder.Start<WebSocketManager.<RunWatchdog>d__28>(ref <RunWatchdog>d__);
		return <RunWatchdog>d__.<>t__builder.Task;
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x0003D19F File Offset: 0x0003B39F
	private static void ForceReconnect(string reason)
	{
		WebSocketManager.StopWatchdog();
		WebSocketManager.DisposeSocket();
		WebSocketManager.TriggerMessage("disconnected", new Dictionary<string, object>
		{
			{
				"reason",
				reason
			}
		});
		if (!WebSocketManager.isIntentionalDisconnect)
		{
			WebSocketManager.Connect(WebSocketManager.url);
		}
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x0003D1D8 File Offset: 0x0003B3D8
	public static void CreateSocket(string url)
	{
		if (WebSocketManager.socket != null)
		{
			WebSocketManager.DisposeSocket();
		}
		SocketIOOptions socketIOOptions = new SocketIOOptions
		{
			ConnectionTimeout = TimeSpan.FromMilliseconds(5000.0),
			Transport = (WebSocketManager.forcePolling ? TransportProtocol.Polling : TransportProtocol.WebSocket),
			AutoUpgrade = false,
			Reconnection = false
		};
		WebSocketManager.Logger.Info(string.Format("Creating WebSocket (url: {0}, transport: {1})", url, socketIOOptions.Transport));
		WebSocketManager.socket = new SocketIO(new Uri(url), socketIOOptions, delegate(IServiceCollection services)
		{
			services.AddSystemTextJson(WebSocketManager.JsonOptions);
			if (!url.StartsWith("wss://", StringComparison.OrdinalIgnoreCase) && !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
			{
				WebSocketOptions webSocketOptions = new WebSocketOptions();
				webSocketOptions.RemoteCertificateValidationCallback = ((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true);
				services.AddSingleton(webSocketOptions);
			}
		});
		WebSocketManager.socket.OnConnected += WebSocketManager.OnConnected;
		WebSocketManager.socket.OnDisconnected += WebSocketManager.OnDisconnected;
		WebSocketManager.socket.OnError += WebSocketManager.OnError;
		WebSocketManager.socket.OnPing += WebSocketManager.OnPing;
		WebSocketManager.socket.OnAny(new Func<string, IEventContext, Task>(WebSocketManager.OnAny));
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x0003D2EC File Offset: 0x0003B4EC
	public static void DisposeSocket()
	{
		if (WebSocketManager.socket == null)
		{
			return;
		}
		WebSocketManager.Logger.Info("Disposing WebSocket");
		WebSocketManager.socket.OnConnected -= WebSocketManager.OnConnected;
		WebSocketManager.socket.OnDisconnected -= WebSocketManager.OnDisconnected;
		WebSocketManager.socket.OnError -= WebSocketManager.OnError;
		WebSocketManager.socket.OnPing -= WebSocketManager.OnPing;
		WebSocketManager.socket.OffAny(new Func<string, IEventContext, Task>(WebSocketManager.OnAny));
		WebSocketManager.socket = null;
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x0003D384 File Offset: 0x0003B584
	public static void Emit(string messageName, Dictionary<string, object> data = null, string responseMessageName = null)
	{
		OutMessage outMessage = new OutMessage(messageName, data, responseMessageName);
		if (outMessage.IsRequestMessage)
		{
			WebSocketManager.Logger.Info(string.Format("WebSocket sending request message {0} ({1})", outMessage.MessageName, outMessage));
			Func<IDataMessage, Task> ack = (IDataMessage dataMessage) => WebSocketManager.OnCallback(outMessage, dataMessage);
			SocketIO socketIO = WebSocketManager.socket;
			string messageName2 = outMessage.MessageName;
			object[] data2;
			if (outMessage.Data != null)
			{
				(data2 = new object[1])[0] = outMessage.Data;
			}
			else
			{
				data2 = Array.Empty<object>();
			}
			socketIO.EmitAsync(messageName2, data2, ack);
		}
		else
		{
			WebSocketManager.Logger.Info(string.Format("WebSocket sending message {0} ({1})", outMessage.MessageName, outMessage));
			SocketIO socketIO2 = WebSocketManager.socket;
			string messageName3 = outMessage.MessageName;
			object[] data3;
			if (outMessage.Data != null)
			{
				(data3 = new object[1])[0] = outMessage.Data;
			}
			else
			{
				data3 = Array.Empty<object>();
			}
			socketIO2.EmitAsync(messageName3, data3);
		}
		WebSocketManager.TriggerMessage("emit", new Dictionary<string, object>
		{
			{
				"messageName",
				outMessage.MessageName
			}
		});
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0003D4AD File Offset: 0x0003B6AD
	private static void OnConnected(object sender, EventArgs args)
	{
		WebSocketManager.Logger.Info("WebSocket connected");
		WebSocketManager.StartWatchdog();
		WebSocketManager.TriggerMessage("connected", new Dictionary<string, object>
		{
			{
				"socket",
				WebSocketManager.socket
			}
		});
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x0003D4E4 File Offset: 0x0003B6E4
	private static void OnDisconnected(object sender, string reason)
	{
		WebSocketManager.Logger.Info("WebSocket disconnected (" + reason + ")");
		WebSocketManager.StopWatchdog();
		WebSocketManager.TriggerMessage("disconnected", new Dictionary<string, object>
		{
			{
				"reason",
				reason
			}
		});
		if (!WebSocketManager.isIntentionalDisconnect)
		{
			WebSocketManager.Connect(WebSocketManager.url);
		}
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x0003D53D File Offset: 0x0003B73D
	private static void OnError(object sender, string error)
	{
		WebSocketManager.Logger.Error("WebSocket error: " + error);
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x0003D554 File Offset: 0x0003B754
	private static void OnPing(object sender, EventArgs args)
	{
		Interlocked.Exchange(ref WebSocketManager.lastServerPingTicks, DateTime.UtcNow.Ticks);
	}

	// Token: 0x06000CEC RID: 3308 RVA: 0x0003D57C File Offset: 0x0003B77C
	private static string DescribeException(Exception exception)
	{
		string text = "";
		for (Exception ex = exception; ex != null; ex = ex.InnerException)
		{
			text = string.Concat(new string[]
			{
				text,
				(text.Length > 0) ? " ---> " : "",
				ex.GetType().Name,
				": ",
				ex.Message
			});
		}
		return text;
	}

	// Token: 0x06000CED RID: 3309 RVA: 0x0003D5E8 File Offset: 0x0003B7E8
	private static Task OnAny(string messageName, IEventContext eventContext)
	{
		InMessage inMessage = new InMessage(messageName, eventContext, null);
		WebSocketManager.Logger.Info(string.Format("WebSocket received message {0} ({1})", messageName, inMessage));
		WebSocketManager.TriggerMessage(messageName, new Dictionary<string, object>
		{
			{
				"inMessage",
				inMessage
			}
		});
		return Task.CompletedTask;
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x0003D630 File Offset: 0x0003B830
	private static Task OnCallback(OutMessage outMessage, IDataMessage dataMessage)
	{
		InMessage inMessage = new InMessage(outMessage.ResponseMessageName, null, dataMessage);
		WebSocketManager.Logger.Info(string.Format("WebSocket received response to message {0} -> {1} ({2})", outMessage.MessageName, inMessage.MessageName, inMessage));
		WebSocketManager.TriggerMessage(inMessage.MessageName, new Dictionary<string, object>
		{
			{
				"outMessage",
				outMessage
			},
			{
				"inMessage",
				inMessage
			}
		});
		return Task.CompletedTask;
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x0003D69C File Offset: 0x0003B89C
	public static void AddMessageListener(string messageName, Action<Dictionary<string, object>> listener)
	{
		if (WebSocketManager.events.ContainsKey(messageName))
		{
			Dictionary<string, Action<Dictionary<string, object>>> dictionary = WebSocketManager.events;
			dictionary[messageName] = (Action<Dictionary<string, object>>)Delegate.Combine(dictionary[messageName], listener);
			return;
		}
		Action<Dictionary<string, object>> action = null;
		action = (Action<Dictionary<string, object>>)Delegate.Combine(action, listener);
		WebSocketManager.events.Add(messageName, action);
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x0003D6F4 File Offset: 0x0003B8F4
	public static void RemoveMessageListener(string messageName, Action<Dictionary<string, object>> listener)
	{
		if (WebSocketManager.events.ContainsKey(messageName))
		{
			Dictionary<string, Action<Dictionary<string, object>>> dictionary = WebSocketManager.events;
			dictionary[messageName] = (Action<Dictionary<string, object>>)Delegate.Remove(dictionary[messageName], listener);
		}
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x0003D730 File Offset: 0x0003B930
	public static void TriggerMessage(string messageName, Dictionary<string, object> message = null)
	{
		MonoBehaviourSingleton<ThreadManager>.Instance.Enqueue(delegate()
		{
			if (WebSocketManager.events.ContainsKey(messageName))
			{
				Action<Dictionary<string, object>> action = WebSocketManager.events[messageName];
				if (action == null)
				{
					return;
				}
				action(message);
			}
		});
	}

	// Token: 0x040007FA RID: 2042
	private static readonly global::Logger Logger = new global::Logger("WebsocketManager");

	// Token: 0x040007FB RID: 2043
	public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
	{
		Converters = 
		{
			new JsonStringEnumConverter()
		}
	};

	// Token: 0x040007FC RID: 2044
	private static Dictionary<string, Action<Dictionary<string, object>>> events = new Dictionary<string, Action<Dictionary<string, object>>>();

	// Token: 0x040007FD RID: 2045
	private static SocketIO socket = null;

	// Token: 0x040007FE RID: 2046
	private static CancellationTokenSource cancellationTokenSource = null;

	// Token: 0x040007FF RID: 2047
	private static string url = null;

	// Token: 0x04000800 RID: 2048
	private static bool isIntentionalDisconnect = false;

	// Token: 0x04000801 RID: 2049
	private static long lastServerPingTicks = 0L;

	// Token: 0x04000802 RID: 2050
	private static CancellationTokenSource watchdogCancellationTokenSource = null;

	// Token: 0x04000803 RID: 2051
	private static readonly Random reconnectRandom = new Random();

	// Token: 0x04000804 RID: 2052
	private static bool forcePolling = false;

	// Token: 0x04000805 RID: 2053
	private const string BackendUrlAssetName = "backend_url";
}
