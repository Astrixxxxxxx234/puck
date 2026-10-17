using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class PlayerCamera : BaseCamera
{
	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060001CB RID: 459 RVA: 0x0000A3DF File Offset: 0x000085DF
	[HideInInspector]
	public PlayerBody PlayerBody
	{
		get
		{
			if (!(this.Player == null))
			{
				return this.Player.PlayerBody;
			}
			return null;
		}
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0000A3FC File Offset: 0x000085FC
	protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
	{
		this.InitializeNetworkVariables(default(NetworkObjectReference));
		base.OnNetworkPreSpawn(ref networkManager);
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0000A41F File Offset: 0x0000861F
	public override void OnNetworkSpawn()
	{
		NetworkVariable<NetworkObjectReference> playerReference = this.PlayerReference;
		playerReference.OnValueChanged = (NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate)Delegate.Combine(playerReference.OnValueChanged, new NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate(this.OnPlayerReferenceChanged));
		base.OnNetworkSpawn();
	}

	// Token: 0x060001CE RID: 462 RVA: 0x0000A450 File Offset: 0x00008650
	protected override void OnNetworkPostSpawn()
	{
		NetworkObjectReference value = this.PlayerReference.Value;
		this.HandlePlayerReference(default(NetworkObjectReference), value);
		EventManager.TriggerEvent("Event_Everyone_OnPlayerCameraSpawned", new Dictionary<string, object>
		{
			{
				"playerCamera",
				this
			}
		});
		base.OnNetworkPostSpawn();
	}

	// Token: 0x060001CF RID: 463 RVA: 0x0000A49A File Offset: 0x0000869A
	public override void OnNetworkDespawn()
	{
		NetworkVariable<NetworkObjectReference> playerReference = this.PlayerReference;
		playerReference.OnValueChanged = (NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate)Delegate.Remove(playerReference.OnValueChanged, new NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate(this.OnPlayerReferenceChanged));
		base.OnNetworkDespawn();
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x0000A4C9 File Offset: 0x000086C9
	public void InitializeNetworkVariables(NetworkObjectReference playerReference = default(NetworkObjectReference))
	{
		if (this.isNetworkVariablesInitialized)
		{
			return;
		}
		this.isNetworkVariablesInitialized = true;
		this.PlayerReference = new NetworkVariable<NetworkObjectReference>(playerReference, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x0000A4E9 File Offset: 0x000086E9
	public override bool Enable()
	{
		bool flag = base.Enable();
		if (flag)
		{
			EventManager.TriggerEvent("Event_OnPlayerCameraEnabled", new Dictionary<string, object>
			{
				{
					"playerCamera",
					this
				}
			});
		}
		return flag;
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x0000A50F File Offset: 0x0000870F
	public override bool Disable()
	{
		bool flag = base.Disable();
		if (flag)
		{
			EventManager.TriggerEvent("Event_OnPlayerCameraDisabled", new Dictionary<string, object>
			{
				{
					"playerCamera",
					this
				}
			});
		}
		return flag;
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x0000A538 File Offset: 0x00008738
	private void Update()
	{
		if (!this.Player)
		{
			return;
		}
		PlayerInput playerInput = this.Player.PlayerInput;
		if (!playerInput)
		{
			return;
		}
		playerInput.UpdateLookAngle(Time.deltaTime);
		base.transform.localRotation = Quaternion.Euler(this.Player.IsLocalPlayer ? playerInput.LookAngleInput.ClientValue : playerInput.LookAngleInput.ServerValue);
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x0000A5B0 File Offset: 0x000087B0
	private void HandlePlayerReference(NetworkObjectReference oldPlayerReference = default(NetworkObjectReference), NetworkObjectReference newPlayerReference = default(NetworkObjectReference))
	{
		NetworkObject networkObject;
		Player player = oldPlayerReference.TryGet(out networkObject, null) ? networkObject.GetComponent<Player>() : null;
		NetworkObject networkObject2;
		Player player2 = newPlayerReference.TryGet(out networkObject2, null) ? networkObject2.GetComponent<Player>() : null;
		if (player)
		{
			player.PlayerCamera = null;
		}
		if (player2)
		{
			this.Player = player2;
			this.Player.PlayerCamera = this;
			return;
		}
		this.Player = null;
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0000A61B File Offset: 0x0000881B
	private void OnPlayerReferenceChanged(NetworkObjectReference oldPlayerReference, NetworkObjectReference newPlayerReference)
	{
		this.HandlePlayerReference(oldPlayerReference, newPlayerReference);
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0000A628 File Offset: 0x00008828
	protected override void __initializeVariables()
	{
		bool flag = this.PlayerReference == null;
		if (flag)
		{
			throw new Exception("PlayerCamera.PlayerReference cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.PlayerReference.Initialize(this);
		base.__nameNetworkVariable(this.PlayerReference, "PlayerReference");
		this.NetworkVariableFields.Add(this.PlayerReference);
		base.__initializeVariables();
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x00003106 File Offset: 0x00001306
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0000A68B File Offset: 0x0000888B
	protected internal override string __getTypeName()
	{
		return "PlayerCamera";
	}

	// Token: 0x04000191 RID: 401
	[HideInInspector]
	public NetworkVariable<NetworkObjectReference> PlayerReference;

	// Token: 0x04000192 RID: 402
	[HideInInspector]
	public Player Player;

	// Token: 0x04000193 RID: 403
	private bool isNetworkVariablesInitialized;
}
