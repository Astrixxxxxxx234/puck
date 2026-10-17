using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x0200014D RID: 333
public class ConnectionApprovalManagerController : MonoBehaviour
{
	// Token: 0x060009A6 RID: 2470 RVA: 0x0002EF0C File Offset: 0x0002D10C
	private void Awake()
	{
		this.connectionApprovalManager = base.GetComponent<ConnectionApprovalManager>();
		EventManager.AddEventListener("Event_Everyone_OnClientConnected", new Action<Dictionary<string, object>>(this.Event_Everyone_OnClientConnected));
		EventManager.AddEventListener("Event_Everyone_OnClientDisconnected", new Action<Dictionary<string, object>>(this.Event_Everyone_OnClientDisconnected));
		EventManager.AddEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
		EventManager.AddEventListener("Event_Server_OnLoadSceneEventCompleted", new Action<Dictionary<string, object>>(this.Event_Server_OnLoadSceneEventCompleted));
		EventManager.AddEventListener("Event_Server_OnConnectionApproved", new Action<Dictionary<string, object>>(this.Event_Server_OnConnectionApproved));
		WebSocketManager.AddMessageListener("serverConnectionApprovalResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnServerConnectionApprovalResponse));
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x000023BA File Offset: 0x000005BA
	private void Start()
	{
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x0002EFAC File Offset: 0x0002D1AC
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnClientConnected", new Action<Dictionary<string, object>>(this.Event_Everyone_OnClientConnected));
		EventManager.RemoveEventListener("Event_Everyone_OnClientDisconnected", new Action<Dictionary<string, object>>(this.Event_Everyone_OnClientDisconnected));
		EventManager.RemoveEventListener("Event_Server_OnServerStopped", new Action<Dictionary<string, object>>(this.Event_Server_OnServerStopped));
		EventManager.RemoveEventListener("Event_Server_OnLoadSceneEventCompleted", new Action<Dictionary<string, object>>(this.Event_Server_OnLoadSceneEventCompleted));
		EventManager.RemoveEventListener("Event_Server_OnConnectionApproved", new Action<Dictionary<string, object>>(this.Event_Server_OnConnectionApproved));
		WebSocketManager.RemoveMessageListener("serverConnectionApprovalResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnServerConnectionApprovalResponse));
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x0002F040 File Offset: 0x0002D240
	private void Event_Everyone_OnClientConnected(Dictionary<string, object> message)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		ulong num = (ulong)message["clientId"];
		ConnectionApproval connectionApprovalByClientId = this.connectionApprovalManager.GetConnectionApprovalByClientId(num);
		if (connectionApprovalByClientId == null || !connectionApprovalByClientId.IsApproved)
		{
			return;
		}
		if (connectionApprovalByClientId.IsHost)
		{
			return;
		}
		EventManager.TriggerEvent("Event_Server_OnApprovedClientConnected", new Dictionary<string, object>
		{
			{
				"clientId",
				num
			},
			{
				"connectionApproval",
				connectionApprovalByClientId
			}
		});
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x0002F0BC File Offset: 0x0002D2BC
	private void Event_Everyone_OnClientDisconnected(Dictionary<string, object> message)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		ulong num = (ulong)message["clientId"];
		ConnectionApproval connectionApprovalByClientId = this.connectionApprovalManager.GetConnectionApprovalByClientId(num);
		if (connectionApprovalByClientId == null || !connectionApprovalByClientId.IsApproved)
		{
			return;
		}
		EventManager.TriggerEvent("Event_Server_OnApprovedClientDisconnected", new Dictionary<string, object>
		{
			{
				"clientId",
				num
			},
			{
				"connectionApproval",
				connectionApprovalByClientId
			}
		});
		this.connectionApprovalManager.RemoveConnectionApproval(num);
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0002F138 File Offset: 0x0002D338
	private void Event_Server_OnServerStopped(Dictionary<string, object> message)
	{
		this.connectionApprovalManager.Dispose();
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x0002F145 File Offset: 0x0002D345
	private void Event_Server_OnLoadSceneEventCompleted(Dictionary<string, object> message)
	{
		if ((bool)message["isInitialScene"])
		{
			this.connectionApprovalManager.ConsumeBufferedConnectionApprovals(true);
		}
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x0002F168 File Offset: 0x0002D368
	private void Event_Server_OnConnectionApproved(Dictionary<string, object> message)
	{
		ulong num = (ulong)message["clientId"];
		ConnectionApproval connectionApproval = (ConnectionApproval)message["connectionApproval"];
		if (connectionApproval.IsHost)
		{
			EventManager.TriggerEvent("Event_Server_OnApprovedClientConnected", new Dictionary<string, object>
			{
				{
					"clientId",
					num
				},
				{
					"connectionApproval",
					connectionApproval
				}
			});
		}
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x0002F1CC File Offset: 0x0002D3CC
	private void WebSocket_Event_OnServerConnectionApprovalResponse(Dictionary<string, object> message)
	{
		OutMessage outMessage = (OutMessage)message["outMessage"];
		ServerConnectionApprovalResponse data = ((InMessage)message["inMessage"]).GetData<ServerConnectionApprovalResponse>();
		string steamId = (string)outMessage.Data["steamId"];
		ConnectionApproval connectionApprovalBySteamId = this.connectionApprovalManager.GetConnectionApprovalBySteamId(steamId);
		if (connectionApprovalBySteamId == null)
		{
			return;
		}
		ulong clientID = connectionApprovalBySteamId.ClientID;
		if (!data.success)
		{
			this.connectionApprovalManager.RejectConnection(clientID, ConnectionRejectionCode.Unknown, data.errorData.message);
			return;
		}
		ConnectionRejectionCode? connectionRejectionCode = this.connectionApprovalManager.GetConnectionRejectionCode(connectionApprovalBySteamId);
		bool flag = connectionRejectionCode == null;
		if (flag && BackendUtils.GetActivePlayerDataBan(data.data.playerData) != null)
		{
			connectionRejectionCode = new ConnectionRejectionCode?(ConnectionRejectionCode.Banned);
			flag = false;
		}
		if (flag)
		{
			this.connectionApprovalManager.ApproveConnection(clientID, data.data.playerData);
			return;
		}
		this.connectionApprovalManager.RejectConnection(clientID, connectionRejectionCode.Value, null);
	}

	// Token: 0x040005F5 RID: 1525
	private ConnectionApprovalManager connectionApprovalManager;
}
