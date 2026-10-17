using System;
using System.Collections.Generic;
using Unity.Netcode;

// Token: 0x02000067 RID: 103
public class StickController : NetworkBehaviour
{
	// Token: 0x06000325 RID: 805 RVA: 0x00014296 File Offset: 0x00012496
	private void Awake()
	{
		this.stick = base.GetComponent<Stick>();
	}

	// Token: 0x06000326 RID: 806 RVA: 0x000142A4 File Offset: 0x000124A4
	public override void OnNetworkSpawn()
	{
		EventManager.AddEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerCustomizationStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerCustomizationStateChanged));
		base.OnNetworkSpawn();
	}

	// Token: 0x06000327 RID: 807 RVA: 0x000142D8 File Offset: 0x000124D8
	public override void OnNetworkDespawn()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerCustomizationStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerCustomizationStateChanged));
		base.OnNetworkDespawn();
	}

	// Token: 0x06000328 RID: 808 RVA: 0x0001430C File Offset: 0x0001250C
	private void Event_Everyone_OnPlayerGameStateChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		PlayerGameState playerGameState = (PlayerGameState)message["oldGameState"];
		PlayerGameState playerGameState2 = (PlayerGameState)message["newGameState"];
		if (base.OwnerClientId != player.OwnerClientId)
		{
			return;
		}
		if (playerGameState.Team == playerGameState2.Team && playerGameState.Role == playerGameState2.Role)
		{
			return;
		}
		this.stick.ApplyCustomizations();
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00014384 File Offset: 0x00012584
	private void Event_Everyone_OnPlayerCustomizationStateChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (base.OwnerClientId == player.OwnerClientId)
		{
			this.stick.ApplyCustomizations();
		}
	}

	// Token: 0x0600032B RID: 811 RVA: 0x000143BC File Offset: 0x000125BC
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x0600032C RID: 812 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x0600032D RID: 813 RVA: 0x000143D2 File Offset: 0x000125D2
	protected internal override string __getTypeName()
	{
		return "StickController";
	}

	// Token: 0x04000259 RID: 601
	private Stick stick;
}
