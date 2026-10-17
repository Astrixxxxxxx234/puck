using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class SpectatorCamera : BaseCamera
{
	// Token: 0x060003E2 RID: 994 RVA: 0x00017088 File Offset: 0x00015288
	protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
	{
		this.InitializeNetworkVariables(default(NetworkObjectReference));
		base.OnNetworkPreSpawn(ref networkManager);
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x000170AB File Offset: 0x000152AB
	public override void OnNetworkSpawn()
	{
		NetworkVariable<NetworkObjectReference> playerReference = this.PlayerReference;
		playerReference.OnValueChanged = (NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate)Delegate.Combine(playerReference.OnValueChanged, new NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate(this.OnPlayerReferenceChanged));
		base.OnNetworkSpawn();
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x000170DC File Offset: 0x000152DC
	protected override void OnNetworkPostSpawn()
	{
		this.position = base.transform.position;
		this.pitch = base.transform.eulerAngles.x;
		this.yaw = base.transform.eulerAngles.y;
		this.targetPitch = this.pitch;
		this.targetYaw = this.yaw;
		NetworkObjectReference value = this.PlayerReference.Value;
		this.HandlePlayerReference(default(NetworkObjectReference), value);
		EventManager.TriggerEvent("Event_Everyone_OnSpectatorCameraSpawned", new Dictionary<string, object>
		{
			{
				"spectatorCamera",
				this
			}
		});
		base.OnNetworkPostSpawn();
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x0001717B File Offset: 0x0001537B
	public override void OnNetworkDespawn()
	{
		NetworkVariable<NetworkObjectReference> playerReference = this.PlayerReference;
		playerReference.OnValueChanged = (NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate)Delegate.Remove(playerReference.OnValueChanged, new NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate(this.OnPlayerReferenceChanged));
		base.OnNetworkDespawn();
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x000171AA File Offset: 0x000153AA
	public void InitializeNetworkVariables(NetworkObjectReference playerReference = default(NetworkObjectReference))
	{
		if (this.isNetworkVariablesInitialized)
		{
			return;
		}
		this.isNetworkVariablesInitialized = true;
		this.PlayerReference = new NetworkVariable<NetworkObjectReference>(playerReference, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x000171CC File Offset: 0x000153CC
	private void Update()
	{
		if (!base.IsOwner)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		float d = (float)((InputManager.TurnRightAction.IsPressed() ? 1 : 0) + (InputManager.TurnLeftAction.IsPressed() ? -1 : 0));
		float d2 = (float)(InputManager.JumpAction.IsPressed() ? 1 : (InputManager.SlideAction.IsPressed() ? -1 : 0));
		float d3 = (float)((InputManager.MoveForwardAction.IsPressed() ? 1 : 0) + (InputManager.MoveBackwardAction.IsPressed() ? -1 : 0));
		bool flag = InputManager.SprintAction.IsPressed();
		Vector2 vector = InputManager.StickAction.ReadValue<Vector2>();
		vector = new Vector2(float.IsNaN(vector.x) ? 0f : (vector.x * SettingsManager.LookSensitivity), float.IsNaN(vector.y) ? 0f : (vector.y * SettingsManager.LookSensitivity));
		if (GlobalStateManager.UIState.IsMouseRequired)
		{
			d = 0f;
			d2 = 0f;
			d3 = 0f;
			flag = false;
			vector = Vector2.zero;
		}
		float d4 = flag ? (this.movementSpeed * 2f) : this.movementSpeed;
		this.position += base.transform.right * d * d4 * deltaTime;
		this.position += base.transform.up * d2 * d4 * deltaTime;
		this.position += base.transform.forward * d3 * d4 * deltaTime;
		base.transform.position = Vector3.SmoothDamp(base.transform.position, this.position, ref this.positionVelocity, this.positionSmoothTime, float.PositiveInfinity, deltaTime);
		this.targetPitch = Mathf.Clamp(this.targetPitch - vector.y, this.pitchMin, this.pitchMax);
		this.targetYaw += vector.x;
		this.pitch = Mathf.Lerp(this.pitch, this.targetPitch, this.lookSmoothing * deltaTime);
		this.yaw = Mathf.Lerp(this.yaw, this.targetYaw, this.lookSmoothing * deltaTime);
		base.transform.rotation = Quaternion.Euler(this.pitch, this.yaw, 0f);
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x0001744C File Offset: 0x0001564C
	private void HandlePlayerReference(NetworkObjectReference oldPlayerReference = default(NetworkObjectReference), NetworkObjectReference newPlayerReference = default(NetworkObjectReference))
	{
		NetworkObject networkObject;
		Player player = oldPlayerReference.TryGet(out networkObject, null) ? networkObject.GetComponent<Player>() : null;
		NetworkObject networkObject2;
		Player player2 = newPlayerReference.TryGet(out networkObject2, null) ? networkObject2.GetComponent<Player>() : null;
		if (player)
		{
			player.SpectatorCamera = null;
		}
		if (player2)
		{
			this.Player = player2;
			this.Player.SpectatorCamera = this;
			return;
		}
		this.Player = null;
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x000174B7 File Offset: 0x000156B7
	private void OnPlayerReferenceChanged(NetworkObjectReference oldPlayerReference, NetworkObjectReference newPlayerReference)
	{
		this.HandlePlayerReference(oldPlayerReference, newPlayerReference);
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x00017524 File Offset: 0x00015724
	protected override void __initializeVariables()
	{
		bool flag = this.PlayerReference == null;
		if (flag)
		{
			throw new Exception("SpectatorCamera.PlayerReference cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.PlayerReference.Initialize(this);
		base.__nameNetworkVariable(this.PlayerReference, "PlayerReference");
		this.NetworkVariableFields.Add(this.PlayerReference);
		base.__initializeVariables();
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x00003106 File Offset: 0x00001306
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x00017587 File Offset: 0x00015787
	protected internal override string __getTypeName()
	{
		return "SpectatorCamera";
	}

	// Token: 0x040002EC RID: 748
	[Header("Movement Settings")]
	[SerializeField]
	private float movementSpeed = 5f;

	// Token: 0x040002ED RID: 749
	[SerializeField]
	private float positionSmoothTime = 0.25f;

	// Token: 0x040002EE RID: 750
	[SerializeField]
	private float lookSmoothing = 10f;

	// Token: 0x040002EF RID: 751
	[SerializeField]
	private float pitchMin = -89f;

	// Token: 0x040002F0 RID: 752
	[SerializeField]
	private float pitchMax = 89f;

	// Token: 0x040002F1 RID: 753
	[HideInInspector]
	public NetworkVariable<NetworkObjectReference> PlayerReference;

	// Token: 0x040002F2 RID: 754
	[HideInInspector]
	public Player Player;

	// Token: 0x040002F3 RID: 755
	private Vector3 position = Vector3.zero;

	// Token: 0x040002F4 RID: 756
	private Vector3 positionVelocity = Vector3.zero;

	// Token: 0x040002F5 RID: 757
	private float pitch;

	// Token: 0x040002F6 RID: 758
	private float yaw;

	// Token: 0x040002F7 RID: 759
	private float targetPitch;

	// Token: 0x040002F8 RID: 760
	private float targetYaw;

	// Token: 0x040002F9 RID: 761
	private bool isNetworkVariablesInitialized;
}
