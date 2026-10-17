using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000150 RID: 336
public class DeploymentManagerController : MonoBehaviour
{
	// Token: 0x060009C4 RID: 2500 RVA: 0x0002F560 File Offset: 0x0002D760
	public void Awake()
	{
		this.deploymentManager = base.GetComponent<DeploymentManager>();
		EventManager.AddEventListener("Event_OnServerStateChanged", new Action<Dictionary<string, object>>(this.Event_OnServerStateChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerAdded", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerAdded));
		EventManager.AddEventListener("Event_Everyone_OnPlayerRemoved", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerRemoved));
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x0002F5BB File Offset: 0x0002D7BB
	private void Start()
	{
		this.deploymentManager.StartEmptyTimeout();
		this.deploymentManager.StartAuthenticationTimeout();
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x0002F5D4 File Offset: 0x0002D7D4
	private void OnDestroy()
	{
		this.deploymentManager.StopEmptyTimeout();
		this.deploymentManager.StopAuthenticationTimeout();
		EventManager.RemoveEventListener("Event_OnServerStateChanged", new Action<Dictionary<string, object>>(this.Event_OnServerStateChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerAdded", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerAdded));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerRemoved", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerRemoved));
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x0002F63C File Offset: 0x0002D83C
	private void Event_OnServerStateChanged(Dictionary<string, object> message)
	{
		ref ServerState ptr = (ServerState)message["oldServerState"];
		ServerState serverState = (ServerState)message["newServerState"];
		if (ptr.AuthenticationPhase == serverState.AuthenticationPhase)
		{
			return;
		}
		if (serverState.AuthenticationPhase == AuthenticationPhase.Authenticated)
		{
			this.deploymentManager.StopAuthenticationTimeout();
			return;
		}
		this.deploymentManager.StopDeployment();
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x0002F698 File Offset: 0x0002D898
	private void Event_Everyone_OnPlayerAdded(Dictionary<string, object> message)
	{
		this.deploymentManager.StopEmptyTimeout();
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x0002F6A5 File Offset: 0x0002D8A5
	private void Event_Everyone_OnPlayerRemoved(Dictionary<string, object> message)
	{
		if (MonoBehaviourSingleton<PlayerManager>.Instance.GetPlayers(false).Count == 0)
		{
			this.deploymentManager.StartEmptyTimeout();
		}
	}

	// Token: 0x04000601 RID: 1537
	private DeploymentManager deploymentManager;
}
