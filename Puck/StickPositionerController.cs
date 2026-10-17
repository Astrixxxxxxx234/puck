using System;
using System.Collections.Generic;
using Unity.Netcode;

// Token: 0x02000069 RID: 105
public class StickPositionerController : NetworkBehaviour
{
	// Token: 0x0600034E RID: 846 RVA: 0x00015282 File Offset: 0x00013482
	private void Awake()
	{
		this.stickPositioner = base.GetComponent<StickPositioner>();
	}

	// Token: 0x0600034F RID: 847 RVA: 0x00015290 File Offset: 0x00013490
	public override void OnNetworkSpawn()
	{
		EventManager.AddEventListener("Event_Everyone_OnStickSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnStickSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerHandednessChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerHandednessChanged));
		base.OnNetworkSpawn();
	}

	// Token: 0x06000350 RID: 848 RVA: 0x000152C4 File Offset: 0x000134C4
	public override void OnNetworkDespawn()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnStickSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnStickSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerHandednessChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerHandednessChanged));
		base.OnNetworkDespawn();
	}

	// Token: 0x06000351 RID: 849 RVA: 0x000152F8 File Offset: 0x000134F8
	private void Event_Everyone_OnStickSpawned(Dictionary<string, object> message)
	{
		Stick stick = (Stick)message["stick"];
		if (base.OwnerClientId == stick.OwnerClientId)
		{
			this.stickPositioner.PrepareShaftTarget(stick);
		}
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00015330 File Offset: 0x00013530
	private void Event_Everyone_OnPlayerHandednessChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (base.OwnerClientId == player.OwnerClientId)
		{
			this.stickPositioner.Handedness = player.Handedness.Value;
		}
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00015374 File Offset: 0x00013574
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x06000355 RID: 853 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x06000356 RID: 854 RVA: 0x0001538A File Offset: 0x0001358A
	protected internal override string __getTypeName()
	{
		return "StickPositionerController";
	}

	// Token: 0x04000286 RID: 646
	private StickPositioner stickPositioner;
}
