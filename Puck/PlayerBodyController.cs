using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class PlayerBodyController : NetworkBehaviour
{
	// Token: 0x060001B1 RID: 433 RVA: 0x00009B8E File Offset: 0x00007D8E
	private void Awake()
	{
		this.playerBody = base.GetComponent<PlayerBody>();
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00009B9C File Offset: 0x00007D9C
	public override void OnNetworkSpawn()
	{
		EventManager.AddEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerUsernameChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerUsernameChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerNumberChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerNumberChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerCustomizationStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerCustomizationStateChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerVoiceStarted", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerVoiceStarted));
		EventManager.AddEventListener("Event_Everyone_OnPlayerVoiceStopped", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerVoiceStopped));
		EventManager.AddEventListener("Event_Server_OnPlayerJumpInput", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerJumpInput));
		EventManager.AddEventListener("Event_Server_OnPlayerDashLeftInput", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerDashLeftInput));
		EventManager.AddEventListener("Event_Server_OnPlayerDashRightInput", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerDashRightInput));
		EventManager.AddEventListener("Event_Server_OnPlayerTwistLeftInput", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerTwistLeftInput));
		EventManager.AddEventListener("Event_Server_OnPlayerTwistRightInput", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerTwistRightInput));
		EventManager.AddEventListener("Event_OnPlayerCameraEnabled", new Action<Dictionary<string, object>>(this.Event_OnPlayerCameraEnabled));
		EventManager.AddEventListener("Event_OnPlayerCameraDisabled", new Action<Dictionary<string, object>>(this.Event_OnPlayerCameraDisabled));
		base.OnNetworkSpawn();
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x00009CD0 File Offset: 0x00007ED0
	public override void OnNetworkDespawn()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerGameStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerGameStateChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerUsernameChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerUsernameChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerNumberChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerNumberChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerCustomizationStateChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerCustomizationStateChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerVoiceStarted", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerVoiceStarted));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerVoiceStopped", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerVoiceStopped));
		EventManager.RemoveEventListener("Event_Server_OnPlayerJumpInput", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerJumpInput));
		EventManager.RemoveEventListener("Event_Server_OnPlayerDashLeftInput", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerDashLeftInput));
		EventManager.RemoveEventListener("Event_Server_OnPlayerDashRightInput", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerDashRightInput));
		EventManager.RemoveEventListener("Event_Server_OnPlayerTwistLeftInput", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerTwistLeftInput));
		EventManager.RemoveEventListener("Event_Server_OnPlayerTwistRightInput", new Action<Dictionary<string, object>>(this.Event_Server_OnPlayerTwistRightInput));
		EventManager.RemoveEventListener("Event_OnPlayerCameraEnabled", new Action<Dictionary<string, object>>(this.Event_OnPlayerCameraEnabled));
		EventManager.RemoveEventListener("Event_OnPlayerCameraDisabled", new Action<Dictionary<string, object>>(this.Event_OnPlayerCameraDisabled));
		base.OnNetworkDespawn();
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00009E04 File Offset: 0x00008004
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
		this.playerBody.ApplyCustomizations();
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x00009E7C File Offset: 0x0000807C
	private void Event_Everyone_OnPlayerUsernameChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (base.OwnerClientId == player.OwnerClientId)
		{
			this.playerBody.ApplyCustomizations();
		}
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x00009EB4 File Offset: 0x000080B4
	private void Event_Everyone_OnPlayerNumberChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (base.OwnerClientId == player.OwnerClientId)
		{
			this.playerBody.ApplyCustomizations();
		}
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00009EEC File Offset: 0x000080EC
	private void Event_Everyone_OnPlayerCustomizationStateChanged(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (base.OwnerClientId == player.OwnerClientId)
		{
			this.playerBody.ApplyCustomizations();
		}
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x00009F24 File Offset: 0x00008124
	private void Event_Everyone_OnPlayerVoiceStarted(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		AudioClip clip = (AudioClip)message["audioClip"];
		if (base.OwnerClientId != player.OwnerClientId)
		{
			return;
		}
		if (player.IsLocalPlayer)
		{
			return;
		}
		this.playerBody.VoiceAudioSource.clip = clip;
		this.playerBody.VoiceAudioSource.loop = true;
		this.playerBody.VoiceAudioSource.Play();
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x00009FA0 File Offset: 0x000081A0
	private void Event_Everyone_OnPlayerVoiceStopped(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (base.OwnerClientId != player.OwnerClientId)
		{
			return;
		}
		if (player.IsLocalPlayer)
		{
			return;
		}
		this.playerBody.VoiceAudioSource.Stop();
	}

	// Token: 0x060001BA RID: 442 RVA: 0x00009FE8 File Offset: 0x000081E8
	private void Event_Server_OnPlayerJumpInput(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (base.OwnerClientId != player.OwnerClientId)
		{
			return;
		}
		this.playerBody.Jump();
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0000A020 File Offset: 0x00008220
	private void Event_Server_OnPlayerTwistLeftInput(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (base.OwnerClientId != player.OwnerClientId)
		{
			return;
		}
		this.playerBody.TwistLeft();
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000A058 File Offset: 0x00008258
	private void Event_Server_OnPlayerTwistRightInput(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (base.OwnerClientId != player.OwnerClientId)
		{
			return;
		}
		this.playerBody.TwistRight();
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0000A090 File Offset: 0x00008290
	private void Event_Server_OnPlayerDashLeftInput(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (base.OwnerClientId != player.OwnerClientId)
		{
			return;
		}
		this.playerBody.DashLeft();
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000A0C8 File Offset: 0x000082C8
	private void Event_Server_OnPlayerDashRightInput(Dictionary<string, object> message)
	{
		Player player = (Player)message["player"];
		if (base.OwnerClientId != player.OwnerClientId)
		{
			return;
		}
		this.playerBody.DashRight();
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000A100 File Offset: 0x00008300
	private void Event_OnPlayerCameraEnabled(Dictionary<string, object> message)
	{
		PlayerCamera playerCamera = (PlayerCamera)message["playerCamera"];
		if (base.OwnerClientId != playerCamera.OwnerClientId)
		{
			return;
		}
		this.playerBody.MeshRendererHider.HideMeshRenderers();
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0000A140 File Offset: 0x00008340
	private void Event_OnPlayerCameraDisabled(Dictionary<string, object> message)
	{
		PlayerCamera playerCamera = (PlayerCamera)message["playerCamera"];
		if (base.OwnerClientId != playerCamera.OwnerClientId)
		{
			return;
		}
		this.playerBody.MeshRendererHider.ShowMeshRenderers();
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0000A180 File Offset: 0x00008380
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000A196 File Offset: 0x00008396
	protected internal override string __getTypeName()
	{
		return "PlayerBodyController";
	}

	// Token: 0x04000181 RID: 385
	private PlayerBody playerBody;
}
