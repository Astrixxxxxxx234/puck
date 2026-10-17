using System;
using System.Collections.Generic;
using Steamworks;
using Unity.Netcode;

// Token: 0x02000054 RID: 84
public class PlayerVoiceRecorderController : NetworkBehaviour
{
	// Token: 0x0600026B RID: 619 RVA: 0x0001010C File Offset: 0x0000E30C
	private void Awake()
	{
		this.playerVoiceRecorder = base.GetComponent<PlayerVoiceRecorder>();
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0001011A File Offset: 0x0000E31A
	private void Start()
	{
		this.playerVoiceRecorder.IsEnabled = NetworkBehaviourSingleton<ServerManager>.Instance.Server.Value.UseVoip;
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0001013B File Offset: 0x0000E33B
	public override void OnNetworkSpawn()
	{
		EventManager.AddEventListener("Event_Everyone_OnPlayerTalkInput", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerTalkInput));
		EventManager.AddEventListener("Event_Everyone_OnServerChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnServerChanged));
		base.OnNetworkSpawn();
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0001016F File Offset: 0x0000E36F
	public override void OnNetworkDespawn()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerTalkInput", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerTalkInput));
		EventManager.RemoveEventListener("Event_Everyone_OnServerChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnServerChanged));
		base.OnNetworkDespawn();
	}

	// Token: 0x0600026F RID: 623 RVA: 0x000101A4 File Offset: 0x0000E3A4
	private void Event_Everyone_OnPlayerTalkInput(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		bool flag = (bool)message["value"];
		if (base.OwnerClientId != player.OwnerClientId)
		{
			return;
		}
		if (!player.IsLocalPlayer)
		{
			return;
		}
		if (flag)
		{
			this.playerVoiceRecorder.Client_RequestVoiceStartRpc(SteamUser.GetVoiceOptimalSampleRate(), default(RpcParams));
			return;
		}
		this.playerVoiceRecorder.Client_RequestVoiceStopRpc(default(RpcParams));
	}

	// Token: 0x06000270 RID: 624 RVA: 0x0001011A File Offset: 0x0000E31A
	private void Event_Everyone_OnServerChanged(Dictionary<string, object> message)
	{
		this.playerVoiceRecorder.IsEnabled = NetworkBehaviourSingleton<ServerManager>.Instance.Server.Value.UseVoip;
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0001021C File Offset: 0x0000E41C
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x06000273 RID: 627 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x06000274 RID: 628 RVA: 0x00010232 File Offset: 0x0000E432
	protected internal override string __getTypeName()
	{
		return "PlayerVoiceRecorderController";
	}

	// Token: 0x040001C9 RID: 457
	private PlayerVoiceRecorder playerVoiceRecorder;
}
