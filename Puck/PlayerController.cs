using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine.InputSystem;

// Token: 0x0200005D RID: 93
public class PlayerController : NetworkBehaviour
{
	// Token: 0x060002DB RID: 731 RVA: 0x00012FAF File Offset: 0x000111AF
	private void Awake()
	{
		this.player = base.GetComponent<Player>();
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00012FC0 File Offset: 0x000111C0
	public override void OnNetworkSpawn()
	{
		InputManager.PositionSelectAction.performed += this.OnPositionSelectActionPerformed;
		EventManager.AddEventListener("Event_OnTeamSelectClickTeam", new Action<Dictionary<string, object>>(this.Event_OnTeamSelectClickTeam));
		EventManager.AddEventListener("Event_OnPositionSelectClickPosition", new Action<Dictionary<string, object>>(this.Event_OnPositionSelectClickPosition));
		EventManager.AddEventListener("Event_OnPauseMenuClickSelectTeam", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickSelectTeam));
		EventManager.AddEventListener("Event_OnPauseMenuClickSelectPosition", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickSelectPosition));
		EventManager.AddEventListener("Event_OnPauseMenuClickForfeit", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickForfeit));
		EventManager.AddEventListener("Event_OnHandednessChanged", new Action<Dictionary<string, object>>(this.Event_OnHandednessChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerPositionClaimedByPlayerChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPositionClaimedByPlayerChanged));
		if (NetworkManager.Singleton.IsServer)
		{
			this.pingTween = DOVirtual.DelayedCall(1f, delegate
			{
				this.player.Server_UpdatePing();
			}, true).SetLoops(-1);
		}
		base.OnNetworkSpawn();
	}

	// Token: 0x060002DD RID: 733 RVA: 0x000130B4 File Offset: 0x000112B4
	public override void OnNetworkDespawn()
	{
		InputManager.PositionSelectAction.performed -= this.OnPositionSelectActionPerformed;
		EventManager.RemoveEventListener("Event_OnTeamSelectClickTeam", new Action<Dictionary<string, object>>(this.Event_OnTeamSelectClickTeam));
		EventManager.RemoveEventListener("Event_OnPositionSelectClickPosition", new Action<Dictionary<string, object>>(this.Event_OnPositionSelectClickPosition));
		EventManager.RemoveEventListener("Event_OnPauseMenuClickSelectTeam", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickSelectTeam));
		EventManager.RemoveEventListener("Event_OnPauseMenuClickSelectPosition", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickSelectPosition));
		EventManager.RemoveEventListener("Event_OnPauseMenuClickForfeit", new Action<Dictionary<string, object>>(this.Event_OnPauseMenuClickForfeit));
		EventManager.RemoveEventListener("Event_OnHandednessChanged", new Action<Dictionary<string, object>>(this.Event_OnHandednessChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerPositionClaimedByPlayerChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerPositionClaimedByPlayerChanged));
		if (NetworkManager.Singleton.IsServer)
		{
			Tween tween = this.pingTween;
			if (tween != null)
			{
				tween.Kill(false);
			}
		}
		base.OnNetworkDespawn();
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00013198 File Offset: 0x00011398
	private void OnPositionSelectActionPerformed(InputAction.CallbackContext context)
	{
		if (GlobalStateManager.UIState.Phase != UIPhase.Playing)
		{
			return;
		}
		if (GlobalStateManager.UIState.IsInteracting)
		{
			return;
		}
		if (!this.player.IsLocalPlayer)
		{
			return;
		}
		this.player.Client_RequestPositionSelectRpc(default(RpcParams));
	}

	// Token: 0x060002DF RID: 735 RVA: 0x000131E8 File Offset: 0x000113E8
	private void Event_OnTeamSelectClickTeam(Dictionary<string, object> message)
	{
		PlayerTeam team = (PlayerTeam)message["team"];
		if (!this.player.IsLocalPlayer)
		{
			return;
		}
		this.player.Client_RequestTeamRpc(team, default(RpcParams));
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x0001322C File Offset: 0x0001142C
	private void Event_OnPositionSelectClickPosition(Dictionary<string, object> message)
	{
		PlayerPosition playerPosition = (PlayerPosition)message["playerPosition"];
		if (!this.player.IsLocalPlayer)
		{
			return;
		}
		NetworkObjectReference playerPositionReference = new NetworkObjectReference(playerPosition.NetworkObject);
		this.player.Client_RequestClaimPositionRpc(playerPositionReference, default(RpcParams));
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x0001327C File Offset: 0x0001147C
	private void Event_OnPauseMenuClickSelectTeam(Dictionary<string, object> message)
	{
		if (!this.player.IsLocalPlayer)
		{
			return;
		}
		this.player.Client_RequestTeamSelectRpc(default(RpcParams));
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x000132AC File Offset: 0x000114AC
	private void Event_OnPauseMenuClickSelectPosition(Dictionary<string, object> message)
	{
		if (!this.player.IsLocalPlayer)
		{
			return;
		}
		this.player.Client_RequestPositionSelectRpc(default(RpcParams));
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x000132DC File Offset: 0x000114DC
	private void Event_OnPauseMenuClickForfeit(Dictionary<string, object> message)
	{
		if (!this.player.IsLocalPlayer)
		{
			return;
		}
		this.player.Client_RequestForfeitRpc(default(RpcParams));
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x0001330C File Offset: 0x0001150C
	private void Event_OnHandednessChanged(Dictionary<string, object> message)
	{
		PlayerHandedness handedness = (PlayerHandedness)message["value"];
		if (!this.player.IsLocalPlayer)
		{
			return;
		}
		this.player.Client_RequestHandednessRpc(handedness, default(RpcParams));
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00013350 File Offset: 0x00011550
	private void Event_Everyone_OnPlayerPositionClaimedByPlayerChanged(Dictionary<string, object> message)
	{
		PlayerPosition playerPosition = (PlayerPosition)message["playerPosition"];
		Player x = (Player)message["oldClaimedByPlayer"];
		Player x2 = (Player)message["newClaimedByPlayer"];
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (x2 == this.player)
		{
			this.player.PlayerPositionReference.Value = new NetworkObjectReference(playerPosition.NetworkObject);
			return;
		}
		if (x == this.player && playerPosition == this.player.PlayerPosition)
		{
			this.player.PlayerPositionReference.Value = default(NetworkObjectReference);
		}
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x00013410 File Offset: 0x00011610
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x060002EA RID: 746 RVA: 0x00013426 File Offset: 0x00011626
	protected internal override string __getTypeName()
	{
		return "PlayerController";
	}

	// Token: 0x0400021E RID: 542
	private Player player;

	// Token: 0x0400021F RID: 543
	private Tween pingTween;
}
