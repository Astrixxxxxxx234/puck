using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Unity.Netcode;
using UnityEngine;

// Token: 0x0200014A RID: 330
[RequireComponent(typeof(ServerManager))]
[RequireComponent(typeof(TimeoutManager))]
[RequireComponent(typeof(BanManager))]
public class ConnectionApprovalManager : MonoBehaviourSingleton<ConnectionApprovalManager>
{
	// Token: 0x06000990 RID: 2448 RVA: 0x0002E778 File Offset: 0x0002C978
	public override void Awake()
	{
		base.Awake();
		this.ServerManager = base.GetComponent<ServerManager>();
		this.TimeoutManager = base.GetComponent<TimeoutManager>();
		this.BanManager = base.GetComponent<BanManager>();
		this.WhitelistManager = base.GetComponent<WhitelistManager>();
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x0002E7B0 File Offset: 0x0002C9B0
	private void Start()
	{
		this.bufferConnectionApprovals = true;
		NetworkManager.Singleton.ConnectionApprovalCallback = new Action<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse>(this.ConnectionApprovalCallback);
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x0002E7CF File Offset: 0x0002C9CF
	public void Dispose()
	{
		this.clientIdConnectionApprovalMap.Clear();
		this.bufferConnectionApprovals = true;
		this.bufferedConnectionApprovals.Clear();
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x0002E7F0 File Offset: 0x0002C9F0
	private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
	{
		if (request.ClientNetworkId != 0UL && !ServerReadinessManager.IsReady)
		{
			ConnectionApprovalManager.Logger.Info(string.Format("Rejected connection for client {0}: server is starting up", request.ClientNetworkId));
			response.Reason = this.GetRejectionReason(ConnectionRejectionCode.ServerStarting, null);
			response.Approved = false;
			response.Pending = false;
			return;
		}
		if (this.bufferConnectionApprovals)
		{
			this.bufferedConnectionApprovals.Add(new ValueTuple<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse>(request, response));
			return;
		}
		this.HandleConnectionApproval(request, response);
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x0002E870 File Offset: 0x0002CA70
	private void HandleConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
	{
		ulong clientNetworkId = request.ClientNetworkId;
		ConnectionData connectionData = null;
		try
		{
			connectionData = JsonSerializer.Deserialize<ConnectionData>(Encoding.UTF8.GetString(request.Payload), null);
		}
		catch (Exception ex)
		{
			ConnectionApprovalManager.Logger.Error(string.Format("Error deserializing connection data for client {0}: {1}", clientNetworkId, ex.Message));
		}
		this.AddConnectionApproval(clientNetworkId, request, response, connectionData);
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x0002E8DC File Offset: 0x0002CADC
	private void AddConnectionApproval(ulong clientId, NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response, ConnectionData connectionData)
	{
		if (this.clientIdConnectionApprovalMap.ContainsKey(clientId))
		{
			return;
		}
		ConnectionApproval connectionApproval = new ConnectionApproval
		{
			Request = request,
			Response = response,
			ConnectionData = connectionData,
			IpAddress = ((clientId == 0UL) ? "127.0.0.1" : this.ServerManager.UnityTransport.GetEndpoint(clientId).Address)
		};
		this.clientIdConnectionApprovalMap.Add(clientId, connectionApproval);
		this.OnConnectionApprovalStarted(clientId, connectionApproval);
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x0002E954 File Offset: 0x0002CB54
	public void ConsumeBufferedConnectionApprovals(bool stopBuffering = true)
	{
		ConnectionApprovalManager.Logger.Info(string.Format("Consuming {0} buffered connection approvals", this.bufferedConnectionApprovals.Count));
		this.bufferConnectionApprovals = !stopBuffering;
		foreach (ValueTuple<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse> valueTuple in this.bufferedConnectionApprovals.ToList<ValueTuple<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse>>())
		{
			NetworkManager.ConnectionApprovalRequest item = valueTuple.Item1;
			NetworkManager.ConnectionApprovalResponse item2 = valueTuple.Item2;
			this.HandleConnectionApproval(item, item2);
			this.bufferedConnectionApprovals.Remove(new ValueTuple<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse>(item, item2));
		}
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x0002E9FC File Offset: 0x0002CBFC
	public void RemoveConnectionApproval(ulong clientId)
	{
		if (!this.clientIdConnectionApprovalMap.ContainsKey(clientId))
		{
			return;
		}
		this.clientIdConnectionApprovalMap.Remove(clientId);
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x0002EA1A File Offset: 0x0002CC1A
	public ConnectionApproval GetConnectionApprovalByClientId(ulong clientId)
	{
		if (!this.clientIdConnectionApprovalMap.ContainsKey(clientId))
		{
			return null;
		}
		return this.clientIdConnectionApprovalMap[clientId];
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x0002EA38 File Offset: 0x0002CC38
	public ConnectionApproval GetConnectionApprovalBySteamId(string steamId)
	{
		return this.clientIdConnectionApprovalMap.Values.FirstOrDefault((ConnectionApproval approval) => approval.ConnectionData.SteamId == steamId);
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x0002EA70 File Offset: 0x0002CC70
	public void ApproveConnection(ulong clientId, PlayerData playerData)
	{
		if (!this.clientIdConnectionApprovalMap.ContainsKey(clientId))
		{
			return;
		}
		ConnectionApproval connectionApproval = this.clientIdConnectionApprovalMap[clientId];
		connectionApproval.Approve(playerData);
		ConnectionApprovalManager.Logger.Info(string.Format("Approved connection for client {0}", clientId));
		EventManager.TriggerEvent("Event_Server_OnConnectionApproved", new Dictionary<string, object>
		{
			{
				"clientId",
				clientId
			},
			{
				"connectionApproval",
				connectionApproval
			}
		});
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x0002EAE8 File Offset: 0x0002CCE8
	public string GetRejectionReason(ConnectionRejectionCode code, string message = null)
	{
		ConnectionRejection connectionRejection = new ConnectionRejection
		{
			code = code,
			message = message,
			data = null
		};
		if (code == ConnectionRejectionCode.MissingMods)
		{
			connectionRejection.data = new ConnectionRejectionData
			{
				clientRequiredModIds = this.ServerManager.ServerConfig.ClientRequiredModIds
			};
		}
		return JsonSerializer.Serialize<ConnectionRejection>(connectionRejection, new JsonSerializerOptions
		{
			WriteIndented = true
		});
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x0002EB48 File Offset: 0x0002CD48
	public void RejectConnection(ulong clientId, ConnectionRejectionCode code, string message = null)
	{
		if (!this.clientIdConnectionApprovalMap.ContainsKey(clientId))
		{
			return;
		}
		ConnectionApproval connectionApproval = this.clientIdConnectionApprovalMap[clientId];
		connectionApproval.Reject(this.GetRejectionReason(code, message));
		ConnectionApprovalManager.Logger.Info(string.Format("Rejected connection for client {0}: {1}", clientId, Utils.GetConnectionRejectionMessage(code, message)));
		EventManager.TriggerEvent("Event_Server_OnConnectionRejected", new Dictionary<string, object>
		{
			{
				"clientId",
				clientId
			},
			{
				"connectionApproval",
				connectionApproval
			},
			{
				"rejectionCode",
				code
			}
		});
		this.RemoveConnectionApproval(clientId);
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x0002EBE4 File Offset: 0x0002CDE4
	public ConnectionRejectionCode? GetConnectionRejectionCode(ConnectionApproval connectionApproval)
	{
		int num = NetworkManager.Singleton.ConnectedClientsList.Count((NetworkClient c) => c.ClientId != connectionApproval.ClientID);
		bool flag = !string.IsNullOrEmpty(this.ServerManager.ServerConfig.password);
		bool useWhitelist = this.ServerManager.ServerConfig.useWhitelist;
		if (connectionApproval.ConnectionData == null || connectionApproval.ConnectionData.SteamId == null || connectionApproval.ConnectionData.EnabledModIds == null)
		{
			return new ConnectionRejectionCode?(ConnectionRejectionCode.Unknown);
		}
		bool flag2 = this.ServerManager.AdminManager.IsSteamIdAdmin(connectionApproval.ConnectionData.SteamId);
		bool flag3 = num >= this.ServerManager.ServerConfig.maxPlayers;
		bool flag4 = this.TimeoutManager.IsSteamIdTimedOut(connectionApproval.ConnectionData.SteamId);
		bool flag5 = this.BanManager.IsSteamIdBanned(connectionApproval.ConnectionData.SteamId);
		bool flag6 = this.BanManager.IsIpAddressBanned(connectionApproval.IpAddress);
		bool flag7 = this.WhitelistManager.IsSteamIdWhitelisted(connectionApproval.ConnectionData.SteamId);
		bool flag8 = string.IsNullOrEmpty(connectionApproval.ConnectionData.Password) && flag;
		bool flag9 = connectionApproval.ConnectionData.Password == this.ServerManager.ServerConfig.password || !flag;
		bool flag10 = this.ServerManager.ServerConfig.ClientRequiredModIds.Any((string modId) => !connectionApproval.ConnectionData.EnabledModIds.Contains(modId));
		if (flag3 && !flag2)
		{
			return new ConnectionRejectionCode?(ConnectionRejectionCode.ServerFull);
		}
		if (flag4)
		{
			return new ConnectionRejectionCode?(ConnectionRejectionCode.TimedOut);
		}
		if (flag5 || flag6)
		{
			return new ConnectionRejectionCode?(ConnectionRejectionCode.Banned);
		}
		if (useWhitelist && !flag7)
		{
			return new ConnectionRejectionCode?(ConnectionRejectionCode.NotWhitelisted);
		}
		if (flag8)
		{
			return new ConnectionRejectionCode?(ConnectionRejectionCode.MissingPassword);
		}
		if (!flag9)
		{
			return new ConnectionRejectionCode?(ConnectionRejectionCode.InvalidPassword);
		}
		if (flag10)
		{
			return new ConnectionRejectionCode?(ConnectionRejectionCode.MissingMods);
		}
		return null;
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x0002EDF8 File Offset: 0x0002CFF8
	private void OnConnectionApprovalStarted(ulong clientId, ConnectionApproval connectionApproval)
	{
		ConnectionApprovalManager.Logger.Info(string.Format("Started connection approval for client {0}", clientId));
		ConnectionRejectionCode? connectionRejectionCode = this.GetConnectionRejectionCode(connectionApproval);
		if (connectionRejectionCode == null)
		{
			connectionApproval.Halt();
			WebSocketManager.Emit("serverConnectionApprovalRequest", new Dictionary<string, object>
			{
				{
					"steamId",
					connectionApproval.ConnectionData.SteamId
				},
				{
					"key",
					connectionApproval.ConnectionData.Key
				}
			}, "serverConnectionApprovalResponse");
			return;
		}
		this.RejectConnection(clientId, connectionRejectionCode.Value, null);
	}

	// Token: 0x040005EB RID: 1515
	private static readonly global::Logger Logger = new global::Logger("ConnectionApprovalManager");

	// Token: 0x040005EC RID: 1516
	[HideInInspector]
	public ServerManager ServerManager;

	// Token: 0x040005ED RID: 1517
	[HideInInspector]
	public TimeoutManager TimeoutManager;

	// Token: 0x040005EE RID: 1518
	[HideInInspector]
	public BanManager BanManager;

	// Token: 0x040005EF RID: 1519
	[HideInInspector]
	public WhitelistManager WhitelistManager;

	// Token: 0x040005F0 RID: 1520
	private Dictionary<ulong, ConnectionApproval> clientIdConnectionApprovalMap = new Dictionary<ulong, ConnectionApproval>();

	// Token: 0x040005F1 RID: 1521
	private bool bufferConnectionApprovals = true;

	// Token: 0x040005F2 RID: 1522
	private List<ValueTuple<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse>> bufferedConnectionApprovals = new List<ValueTuple<NetworkManager.ConnectionApprovalRequest, NetworkManager.ConnectionApprovalResponse>>();
}
