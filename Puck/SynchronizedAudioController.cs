using System;
using System.Collections.Generic;
using Unity.Netcode;

// Token: 0x02000082 RID: 130
public class SynchronizedAudioController : NetworkBehaviour
{
	// Token: 0x0600040B RID: 1035 RVA: 0x00018026 File Offset: 0x00016226
	private void Awake()
	{
		this.synchronizedAudio = base.GetComponent<SynchronizedAudio>();
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x00018034 File Offset: 0x00016234
	public override void OnNetworkSpawn()
	{
		EventManager.AddEventListener("Event_Server_OnClientSceneSynchronizeComplete", new Action<Dictionary<string, object>>(this.Event_Server_OnClientSceneSynchronizeComplete));
		base.OnNetworkSpawn();
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x00018052 File Offset: 0x00016252
	public override void OnNetworkDespawn()
	{
		EventManager.RemoveEventListener("Event_Server_OnClientSceneSynchronizeComplete", new Action<Dictionary<string, object>>(this.Event_Server_OnClientSceneSynchronizeComplete));
		base.OnNetworkDespawn();
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x00018070 File Offset: 0x00016270
	private void Event_Server_OnClientSceneSynchronizeComplete(Dictionary<string, object> message)
	{
		ulong num = (ulong)message["clientId"];
		if (num == 0UL)
		{
			return;
		}
		this.synchronizedAudio.Server_ForceSynchronizeClientId(num);
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x000180A0 File Offset: 0x000162A0
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x000180B6 File Offset: 0x000162B6
	protected internal override string __getTypeName()
	{
		return "SynchronizedAudioController";
	}

	// Token: 0x0400030C RID: 780
	private SynchronizedAudio synchronizedAudio;
}
