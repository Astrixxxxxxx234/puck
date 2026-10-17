using System;
using System.Collections.Generic;
using Unity.Netcode;

// Token: 0x02000052 RID: 82
public class PlayerInputController : NetworkBehaviour
{
	// Token: 0x06000241 RID: 577 RVA: 0x0000F101 File Offset: 0x0000D301
	private void Awake()
	{
		this.playerInput = base.GetComponent<PlayerInput>();
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0000F10F File Offset: 0x0000D30F
	private void Start()
	{
		this.playerInput.InitialLookAngle = SettingsManager.CameraAngle;
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000F124 File Offset: 0x0000D324
	public override void OnNetworkSpawn()
	{
		EventManager.AddEventListener("Event_Everyone_OnPlayerBodySpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodySpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerHandednessChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerHandednessChanged));
		EventManager.AddEventListener("Event_Everyone_OnServerChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnServerChanged));
		EventManager.AddEventListener("Event_Server_OnClientSceneSynchronizeComplete", new Action<Dictionary<string, object>>(this.Event_Server_OnClientSceneSynchronizeComplete));
		EventManager.AddEventListener("Event_OnCameraAngleChanged", new Action<Dictionary<string, object>>(this.Event_OnCameraAngleChanged));
		base.OnNetworkSpawn();
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000F1A8 File Offset: 0x0000D3A8
	public override void OnNetworkDespawn()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerBodySpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodySpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerHandednessChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerHandednessChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnServerChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnServerChanged));
		EventManager.RemoveEventListener("Event_Server_OnClientSceneSynchronizeComplete", new Action<Dictionary<string, object>>(this.Event_Server_OnClientSceneSynchronizeComplete));
		EventManager.RemoveEventListener("Event_OnCameraAngleChanged", new Action<Dictionary<string, object>>(this.Event_OnCameraAngleChanged));
		base.OnNetworkDespawn();
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000F22C File Offset: 0x0000D42C
	private void Event_Everyone_OnPlayerBodySpawned(Dictionary<string, object> message)
	{
		PlayerBody playerBody = (PlayerBody)message["playerBody"];
		if (playerBody.Player.IsLocalPlayer)
		{
			this.playerInput.ResetInputs(playerBody.Player.Handedness.Value);
		}
	}

	// Token: 0x06000246 RID: 582 RVA: 0x0000F274 File Offset: 0x0000D474
	private void Event_Everyone_OnPlayerHandednessChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (player.IsLocalPlayer)
		{
			this.playerInput.ResetInputs(player.Handedness.Value);
		}
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0000F2B0 File Offset: 0x0000D4B0
	private void Event_Everyone_OnServerChanged(Dictionary<string, object> message)
	{
		this.playerInput.TickRate = NetworkBehaviourSingleton<ServerManager>.Instance.Server.Value.TickRate;
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0000F2D4 File Offset: 0x0000D4D4
	private void Event_Server_OnClientSceneSynchronizeComplete(Dictionary<string, object> message)
	{
		ulong num = (ulong)message["clientId"];
		if (num == 0UL)
		{
			return;
		}
		this.playerInput.Server_ForceSynchronizeClientId(num);
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0000F304 File Offset: 0x0000D504
	private void Event_OnCameraAngleChanged(Dictionary<string, object> message)
	{
		float initialLookAngle = (float)message["value"];
		this.playerInput.InitialLookAngle = initialLookAngle;
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000F330 File Offset: 0x0000D530
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x0600024C RID: 588 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x0600024D RID: 589 RVA: 0x0000F346 File Offset: 0x0000D546
	protected internal override string __getTypeName()
	{
		return "PlayerInputController";
	}

	// Token: 0x040001BE RID: 446
	private PlayerInput playerInput;
}
