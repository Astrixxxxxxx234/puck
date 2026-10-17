using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Unity.Netcode;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class PlayerBody : NetworkBehaviour
{
	// Token: 0x17000018 RID: 24
	// (get) Token: 0x0600016F RID: 367 RVA: 0x00007AF6 File Offset: 0x00005CF6
	[HideInInspector]
	public PlayerCamera PlayerCamera
	{
		get
		{
			if (!(this.Player == null))
			{
				return this.Player.PlayerCamera;
			}
			return null;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000170 RID: 368 RVA: 0x00007B13 File Offset: 0x00005D13
	[HideInInspector]
	public Stick Stick
	{
		get
		{
			if (!(this.Player == null))
			{
				return this.Player.Stick;
			}
			return null;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000171 RID: 369 RVA: 0x00007B30 File Offset: 0x00005D30
	[HideInInspector]
	public PlayerMesh PlayerMesh
	{
		get
		{
			return this.playerMesh;
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000172 RID: 370 RVA: 0x00007B38 File Offset: 0x00005D38
	[HideInInspector]
	public AudioSource VoiceAudioSource
	{
		get
		{
			return this.voiceAudioSource;
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000173 RID: 371 RVA: 0x00007B40 File Offset: 0x00005D40
	[HideInInspector]
	public float Upwardness
	{
		get
		{
			return Vector3.Dot(base.transform.up, Vector3.up);
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000174 RID: 372 RVA: 0x00007B57 File Offset: 0x00005D57
	[HideInInspector]
	public bool IsUpright
	{
		get
		{
			return this.Upwardness > this.upwardnessThreshold;
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000175 RID: 373 RVA: 0x00007B67 File Offset: 0x00005D67
	[HideInInspector]
	public bool IsSlipping
	{
		get
		{
			return this.Upwardness < this.upwardnessThreshold;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000176 RID: 374 RVA: 0x00007B77 File Offset: 0x00005D77
	[HideInInspector]
	public bool IsSideways
	{
		get
		{
			return this.Upwardness < this.sidewaysThreshold;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000177 RID: 375 RVA: 0x00007B87 File Offset: 0x00005D87
	[HideInInspector]
	public bool IsGrounded
	{
		get
		{
			return this.Hover.IsGrounded && this.IsUpright;
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000178 RID: 376 RVA: 0x00007B9E File Offset: 0x00005D9E
	[HideInInspector]
	public bool IsJumping
	{
		get
		{
			return !this.Hover.IsGrounded && this.IsUpright;
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000179 RID: 377 RVA: 0x00007BB5 File Offset: 0x00005DB5
	[HideInInspector]
	public bool IsBalanced
	{
		get
		{
			return this.KeepUpright.Balance >= 1f;
		}
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x0600017A RID: 378 RVA: 0x00007BCC File Offset: 0x00005DCC
	[HideInInspector]
	public Transform MovementDirection
	{
		get
		{
			return this.movementDirection;
		}
	}

	// Token: 0x0600017B RID: 379 RVA: 0x00007BD4 File Offset: 0x00005DD4
	public virtual void Awake()
	{
		this.Rigidbody = base.GetComponent<Rigidbody>();
		this.SynchronizedObject = base.GetComponent<SynchronizedObject>();
		this.bodyColliders = base.GetComponents<Collider>();
		this.softCollider = base.GetComponent<SoftCollider>();
		this.MeshRendererHider = base.GetComponent<MeshRendererHider>();
		this.CollisionRecorder = base.GetComponent<CollisionRecorder>();
		CollisionRecorder collisionRecorder = this.CollisionRecorder;
		collisionRecorder.CollisionDeferred = (Action<GameObject, float>)Delegate.Combine(collisionRecorder.CollisionDeferred, new Action<GameObject, float>(this.Server_OnCollisionDeferred));
		this.Movement = base.GetComponent<Movement>();
		this.Movement.MovementDirection = this.MovementDirection;
		this.VelocityLean = base.GetComponent<VelocityLean>();
		this.VelocityLean.MovementDirection = this.MovementDirection;
		this.Hover = base.GetComponent<Hover>();
		this.Skate = base.GetComponent<Skate>();
		this.Skate.MovementDirection = this.MovementDirection;
		this.KeepUpright = base.GetComponent<KeepUpright>();
	}

	// Token: 0x0600017C RID: 380 RVA: 0x00007CC0 File Offset: 0x00005EC0
	public void SetCollisionFilteredWith(PlayerBody other, bool filtered)
	{
		if (other == null || this.bodyColliders == null || other.bodyColliders == null)
		{
			return;
		}
		foreach (Collider collider in this.bodyColliders)
		{
			if (!(collider == null))
			{
				foreach (Collider collider2 in other.bodyColliders)
				{
					if (!(collider2 == null))
					{
						Physics.IgnoreCollision(collider, collider2, filtered);
					}
				}
			}
		}
		if (this.softCollider != null)
		{
			this.softCollider.SetSoftCollisionIgnored(other.Rigidbody, filtered);
		}
		if (other.softCollider != null)
		{
			other.softCollider.SetSoftCollisionIgnored(this.Rigidbody, filtered);
		}
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00007D7C File Offset: 0x00005F7C
	protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
	{
		this.InitializeNetworkVariables(default(NetworkObjectReference), 0f, 0f, false, false, false, false, false, false);
		base.OnNetworkPreSpawn(ref networkManager);
	}

	// Token: 0x0600017E RID: 382 RVA: 0x00007DB0 File Offset: 0x00005FB0
	public override void OnNetworkSpawn()
	{
		NetworkVariable<NetworkObjectReference> playerReference = this.PlayerReference;
		playerReference.OnValueChanged = (NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate)Delegate.Combine(playerReference.OnValueChanged, new NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate(this.OnPlayerReferenceChanged));
		this.Stamina.OnRawValueChanged += this.OnStaminaChanged;
		this.Speed.OnRawValueChanged += this.OnSpeedChanged;
		NetworkVariable<bool> isSprinting = this.IsSprinting;
		isSprinting.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Combine(isSprinting.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(this.OnIsSprintingChanged));
		NetworkVariable<bool> isSliding = this.IsSliding;
		isSliding.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Combine(isSliding.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(this.OnIsSlidingChanged));
		NetworkVariable<bool> isStopping = this.IsStopping;
		isStopping.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Combine(isStopping.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(this.OnIsStoppingChanged));
		NetworkVariable<bool> isExtendedLeft = this.IsExtendedLeft;
		isExtendedLeft.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Combine(isExtendedLeft.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(this.OnIsExtendedLeftChanged));
		NetworkVariable<bool> isExtendedRight = this.IsExtendedRight;
		isExtendedRight.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Combine(isExtendedRight.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(this.OnIsExtendedRightChanged));
		base.OnNetworkSpawn();
	}

	// Token: 0x0600017F RID: 383 RVA: 0x00007EDC File Offset: 0x000060DC
	protected override void OnNetworkPostSpawn()
	{
		NetworkObjectReference value = this.PlayerReference.Value;
		this.HandlePlayerReference(default(NetworkObjectReference), value);
		EventManager.TriggerEvent("Event_Everyone_OnPlayerBodySpawned", new Dictionary<string, object>
		{
			{
				"playerBody",
				this
			}
		});
		base.OnNetworkPostSpawn();
	}

	// Token: 0x06000180 RID: 384 RVA: 0x00007F28 File Offset: 0x00006128
	public override void OnNetworkDespawn()
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerBodyDespawned", new Dictionary<string, object>
		{
			{
				"playerBody",
				this
			}
		});
		NetworkVariable<NetworkObjectReference> playerReference = this.PlayerReference;
		playerReference.OnValueChanged = (NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate)Delegate.Remove(playerReference.OnValueChanged, new NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate(this.OnPlayerReferenceChanged));
		this.Stamina.OnRawValueChanged -= this.OnStaminaChanged;
		this.Speed.OnRawValueChanged -= this.OnSpeedChanged;
		NetworkVariable<bool> isSprinting = this.IsSprinting;
		isSprinting.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Remove(isSprinting.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(this.OnIsSprintingChanged));
		NetworkVariable<bool> isSliding = this.IsSliding;
		isSliding.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Remove(isSliding.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(this.OnIsSlidingChanged));
		NetworkVariable<bool> isStopping = this.IsStopping;
		isStopping.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Remove(isStopping.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(this.OnIsStoppingChanged));
		NetworkVariable<bool> isExtendedLeft = this.IsExtendedLeft;
		isExtendedLeft.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Remove(isExtendedLeft.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(this.OnIsExtendedLeftChanged));
		NetworkVariable<bool> isExtendedRight = this.IsExtendedRight;
		isExtendedRight.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Remove(isExtendedRight.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(this.OnIsExtendedRightChanged));
		base.OnNetworkDespawn();
	}

	// Token: 0x06000181 RID: 385 RVA: 0x00008070 File Offset: 0x00006270
	public override void OnDestroy()
	{
		Tween tween = this.balanceLossTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		Tween tween2 = this.balanceRecoveryTween;
		if (tween2 != null)
		{
			tween2.Kill(false);
		}
		Tween tween3 = this.dashDragTween;
		if (tween3 != null)
		{
			tween3.Kill(false);
		}
		Tween tween4 = this.dashLegPadTween;
		if (tween4 != null)
		{
			tween4.Kill(false);
		}
		CollisionRecorder collisionRecorder = this.CollisionRecorder;
		collisionRecorder.CollisionDeferred = (Action<GameObject, float>)Delegate.Remove(collisionRecorder.CollisionDeferred, new Action<GameObject, float>(this.Server_OnCollisionDeferred));
		base.OnDestroy();
	}

	// Token: 0x06000182 RID: 386 RVA: 0x000080F4 File Offset: 0x000062F4
	public void InitializeNetworkVariables(NetworkObjectReference playerReference = default(NetworkObjectReference), float stamina = 0f, float speed = 0f, bool isSprinting = false, bool isSliding = false, bool isStopping = false, bool isExtendedLeft = false, bool isExtendedRight = false, bool hasFallen = false)
	{
		if (this.isNetworkVariablesInitialized)
		{
			return;
		}
		this.isNetworkVariablesInitialized = true;
		this.PlayerReference = new NetworkVariable<NetworkObjectReference>(playerReference, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.Stamina = CompressedNetworkVariable<float, byte>.CreateFloatToByte(0f, 1f, stamina, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Server);
		this.Speed = CompressedNetworkVariable<float, byte>.CreateFloatToByte(0f, 16f, speed, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Server);
		this.IsSprinting = new NetworkVariable<bool>(isSprinting, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.IsSliding = new NetworkVariable<bool>(isSliding, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.IsStopping = new NetworkVariable<bool>(isStopping, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.IsExtendedLeft = new NetworkVariable<bool>(isExtendedLeft, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.IsExtendedRight = new NetworkVariable<bool>(isExtendedRight, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.HasFallen = new NetworkVariable<bool>(hasFallen, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
	}

	// Token: 0x06000183 RID: 387 RVA: 0x000081A9 File Offset: 0x000063A9
	private void Update()
	{
		if (!this.Player || !this.Player.PlayerInput)
		{
			return;
		}
		this.HandleMeshLookAt(this.Player.PlayerInput);
		this.HandleMeshStretch();
	}

	// Token: 0x06000184 RID: 388 RVA: 0x000081E4 File Offset: 0x000063E4
	private void HandleMeshLookAt(PlayerInput playerInput)
	{
		if (playerInput.LookInput.ServerValue || playerInput.TrackInput.ServerValue)
		{
			if (this.PlayerCamera)
			{
				this.playerMesh.LookAt(this.PlayerCamera.transform.position + this.PlayerCamera.transform.forward * 10f, Time.deltaTime, true, true);
				return;
			}
		}
		else if (this.Stick)
		{
			this.playerMesh.LookAt(this.Stick.BladeHandlePosition, Time.deltaTime, true, true);
		}
	}

	// Token: 0x06000185 RID: 389 RVA: 0x00008284 File Offset: 0x00006484
	private void HandleMeshStretch()
	{
		float b = this.IsJumping ? 1.05f : (this.IsSliding.Value ? 0.95f : 1f);
		this.PlayerMesh.Stretch = Mathf.Lerp(this.PlayerMesh.Stretch, b, Time.deltaTime * this.stretchSpeed);
	}

	// Token: 0x06000186 RID: 390 RVA: 0x000082E4 File Offset: 0x000064E4
	private void FixedUpdate()
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
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.Player.IsReplay.Value)
		{
			this.HandleInputs(playerInput);
		}
		this.Speed.Value = this.Movement.Speed;
		if (this.IsSprinting.Value)
		{
			this.Stamina.Value -= (this.IsSprinting.Value ? (Time.deltaTime / this.sprintStaminaDrainRate) : 0f);
		}
		else if (this.Stamina.Value < 1f)
		{
			this.Stamina.Value += Time.fixedDeltaTime / this.staminaRegenerationRate;
			this.Stamina.Value = Mathf.Clamp(this.Stamina.Value, 0f, 1f);
		}
		if (this.IsUpright)
		{
			this.Rigidbody.AddForce(Vector3.up * -Physics.gravity.y, ForceMode.Acceleration);
			this.Rigidbody.AddForce(Vector3.down * -Physics.gravity.y * this.gravityMultiplier, ForceMode.Acceleration);
		}
		this.MovementDirection.localRotation = Quaternion.FromToRotation(base.transform.forward, Utils.Vector3Slerp3(-base.transform.right, base.transform.forward, base.transform.right, this.Laterality));
		this.Movement.Sprint = this.IsSprinting.Value;
		this.Movement.TurnMultiplier = (this.IsSliding.Value ? this.slideTurnMultiplier : (this.IsJumping ? this.jumpTurnMultiplier : 1f));
		this.Movement.AmbientDrag = (this.HasFallen.Value ? this.fallenDrag : (this.HasDashed ? this.Movement.AmbientDrag : (this.IsStopping.Value ? this.stopDrag : (this.IsSliding.Value ? this.slideDrag : 0f))));
		this.Hover.TargetDistance = (this.IsSliding.Value ? this.slideHoverDistance : (this.KeepUpright.Balance * this.hoverDistance));
		this.Skate.Intensity = ((this.IsSliding.Value || this.IsStopping.Value || !this.IsGrounded) ? 0f : this.KeepUpright.Balance);
		this.VelocityLean.AngularIntensity = Mathf.Max(0.1f, this.Movement.NormalizedMaximumSpeed) / (this.IsSliding.Value ? 2f : (this.IsJumping ? 2f : 1f));
		this.VelocityLean.Inverted = (!this.IsJumping && !this.IsSliding.Value && this.Movement.IsMovingBackwards);
		this.VelocityLean.UseWorldLinearVelocity = (this.IsJumping || this.IsSliding.Value);
		if (!this.HasSlipped && !this.HasFallen.Value && this.IsSlipping)
		{
			this.OnSlip();
		}
		else if (this.HasSlipped && !this.HasFallen.Value && this.IsSideways)
		{
			this.OnFall();
		}
		else if (this.HasFallen.Value && !this.HasSlipped && this.IsUpright)
		{
			this.OnStandUp();
		}
		this.Server_UpdateAudio();
	}

	// Token: 0x06000187 RID: 391 RVA: 0x000086B4 File Offset: 0x000068B4
	public void ApplyCustomizations()
	{
		if (this.Player.Team == PlayerTeam.None || this.Player.Role == PlayerRole.None)
		{
			return;
		}
		this.PlayerMesh.SetUsername(this.Player.Username.Value.ToString());
		this.PlayerMesh.SetNumber(this.Player.Number.Value.ToString());
		this.PlayerMesh.SetLegsPadsActive(this.Player.Role == PlayerRole.Goalie);
		this.PlayerMesh.SetFlagID(this.Player.FlagID);
		this.PlayerMesh.SetHeadgearID(this.Player.GetPlayerHeadgearID(), this.Player.Role);
		this.PlayerMesh.SetMustacheID(this.Player.MustacheID);
		this.PlayerMesh.SetBeardID(this.Player.BeardID);
		this.PlayerMesh.SetJerseyID(this.Player.GetPlayerJerseyID(), this.Player.Team);
	}

	// Token: 0x06000188 RID: 392 RVA: 0x000087C8 File Offset: 0x000069C8
	private void HandleInputs(PlayerInput playerInput)
	{
		if (!this.IsSprinting.Value && playerInput.SprintInput.ServerValue && !this.IsSliding.Value && this.IsGrounded && this.Stamina.Value > 0.25f)
		{
			this.IsSprinting.Value = true;
		}
		else if (this.IsSprinting.Value && !playerInput.SprintInput.ServerValue)
		{
			this.IsSprinting.Value = false;
		}
		else if (this.IsSprinting.Value)
		{
			this.IsSprinting.Value = (!this.IsSliding.Value && this.IsGrounded && this.Stamina.Value > 0f);
		}
		this.IsSliding.Value = (playerInput.SlideInput.ServerValue && this.IsGrounded);
		this.IsStopping.Value = (playerInput.StopInput.ServerValue && this.IsGrounded);
		if (!this.HasDashExtended)
		{
			this.IsExtendedLeft.Value = (playerInput.ExtendLeftInput.ServerValue && this.IsGrounded && this.IsSliding.Value);
			this.IsExtendedRight.Value = (playerInput.ExtendRightInput.ServerValue && this.IsGrounded && this.IsSliding.Value);
		}
		this.Movement.MoveForwards = (!this.IsSliding.Value && playerInput.MoveInput.ServerValue.y > 0.05f);
		this.Movement.MoveBackwards = (!this.IsSliding.Value && playerInput.MoveInput.ServerValue.y < -0.05f);
		this.Movement.TurnRight = (playerInput.MoveInput.ServerValue.x > 0.05f);
		this.Movement.TurnLeft = (playerInput.MoveInput.ServerValue.x < -0.05f);
		float t = Mathf.Clamp01(1f - this.Movement.NormalizedMinimumSpeed);
		float num = Mathf.Lerp(this.minimumLateralitySpeed, this.maximumLateralitySpeed, t);
		float num2 = Mathf.Lerp(this.minimumLaterality, this.maximumLaterality, t);
		if (playerInput.LateralLeftInput.ServerValue)
		{
			this.Laterality = Mathf.Lerp(this.Laterality, -num2, Time.fixedDeltaTime * num);
			return;
		}
		if (playerInput.LateralRightInput.ServerValue)
		{
			this.Laterality = Mathf.Lerp(this.Laterality, num2, Time.fixedDeltaTime * num);
			return;
		}
		this.Laterality = Mathf.Lerp(this.Laterality, 0f, Time.fixedDeltaTime * num);
	}

	// Token: 0x06000189 RID: 393 RVA: 0x00008A88 File Offset: 0x00006C88
	public void OnSlip()
	{
		this.HasSlipped = true;
		this.HasFallen.Value = false;
		Tween tween = this.balanceRecoveryTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		Tween tween2 = this.balanceLossTween;
		if (tween2 != null)
		{
			tween2.Kill(false);
		}
		this.balanceLossTween = DOTween.To(() => this.KeepUpright.Balance, delegate(float value)
		{
			this.KeepUpright.Balance = value;
		}, 0f, this.balanceLossTime).SetEase(Ease.Linear);
	}

	// Token: 0x0600018A RID: 394 RVA: 0x00008B00 File Offset: 0x00006D00
	public void OnFall()
	{
		this.HasSlipped = false;
		this.HasFallen.Value = true;
		Tween tween = this.balanceRecoveryTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.balanceRecoveryTween = DOTween.To(() => this.KeepUpright.Balance, delegate(float value)
		{
			this.KeepUpright.Balance = value;
		}, 1f, this.balanceRecoveryTime).SetEase(Ease.Linear);
	}

	// Token: 0x0600018B RID: 395 RVA: 0x00008B66 File Offset: 0x00006D66
	public void OnStandUp()
	{
		this.HasSlipped = false;
		this.HasFallen.Value = false;
	}

	// Token: 0x0600018C RID: 396 RVA: 0x00008B7C File Offset: 0x00006D7C
	public void Jump()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsGrounded)
		{
			return;
		}
		if (this.Stamina.Value < this.jumpStaminaDrain)
		{
			return;
		}
		this.Rigidbody.AddForce(Vector3.up * this.jumpVelocity, ForceMode.VelocityChange);
		this.Stamina.Value = Mathf.Max(0f, this.Stamina.Value - this.jumpStaminaDrain);
	}

	// Token: 0x0600018D RID: 397 RVA: 0x00008BF8 File Offset: 0x00006DF8
	public void TwistLeft()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsJumping)
		{
			return;
		}
		if (this.Stamina.Value < this.twistStaminaDrain)
		{
			return;
		}
		this.Rigidbody.AddTorque(-base.transform.up * this.twistVelocity, ForceMode.VelocityChange);
		this.Stamina.Value = Mathf.Max(0f, this.Stamina.Value - this.twistStaminaDrain);
	}

	// Token: 0x0600018E RID: 398 RVA: 0x00008C80 File Offset: 0x00006E80
	public void TwistRight()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsJumping)
		{
			return;
		}
		if (this.Stamina.Value < this.twistStaminaDrain)
		{
			return;
		}
		this.Rigidbody.AddTorque(base.transform.up * this.twistVelocity, ForceMode.VelocityChange);
		this.Stamina.Value = Mathf.Max(0f, this.Stamina.Value - this.twistStaminaDrain);
	}

	// Token: 0x0600018F RID: 399 RVA: 0x00008D00 File Offset: 0x00006F00
	public void DashLeft()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.canDash)
		{
			return;
		}
		if (!this.IsSliding.Value)
		{
			return;
		}
		if (this.Stamina.Value < this.dashStaminaDrain)
		{
			return;
		}
		this.Rigidbody.AddForce(-base.transform.right * this.dashVelocity, ForceMode.VelocityChange);
		this.Stamina.Value = Mathf.Max(0f, this.Stamina.Value - this.dashStaminaDrain);
		this.HasDashed = true;
		this.Movement.AmbientDrag = this.dashDrag;
		Tween tween = this.dashDragTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.dashDragTween = DOTween.To(() => this.Movement.AmbientDrag, delegate(float value)
		{
			this.Movement.AmbientDrag = value;
		}, 0f, this.dashDragTime).OnComplete(delegate
		{
			this.HasDashed = false;
		}).SetEase(Ease.Linear);
		this.HasDashExtended = true;
		this.IsExtendedRight.Value = false;
		this.IsExtendedRight.Value = true;
		Tween tween2 = this.dashLegPadTween;
		if (tween2 != null)
		{
			tween2.Kill(false);
		}
		this.dashLegPadTween = DOVirtual.DelayedCall(this.dashDragTime / 4f, delegate
		{
			this.HasDashExtended = false;
		}, true);
	}

	// Token: 0x06000190 RID: 400 RVA: 0x00008E58 File Offset: 0x00007058
	public void DashRight()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.canDash)
		{
			return;
		}
		if (!this.IsSliding.Value)
		{
			return;
		}
		if (this.Stamina.Value < this.dashStaminaDrain)
		{
			return;
		}
		this.Rigidbody.AddForce(base.transform.right * this.dashVelocity, ForceMode.VelocityChange);
		this.Stamina.Value = Mathf.Max(0f, this.Stamina.Value - this.dashStaminaDrain);
		this.HasDashed = true;
		this.Movement.AmbientDrag = this.dashDrag;
		Tween tween = this.dashDragTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.dashDragTween = DOTween.To(() => this.Movement.AmbientDrag, delegate(float value)
		{
			this.Movement.AmbientDrag = value;
		}, 0f, this.dashDragTime).OnComplete(delegate
		{
			this.HasDashed = false;
		}).SetEase(Ease.Linear);
		this.HasDashExtended = true;
		this.IsExtendedLeft.Value = false;
		this.IsExtendedLeft.Value = true;
		Tween tween2 = this.dashLegPadTween;
		if (tween2 != null)
		{
			tween2.Kill(false);
		}
		this.dashLegPadTween = DOVirtual.DelayedCall(this.dashDragTime / 4f, delegate
		{
			this.HasDashExtended = false;
		}, true);
	}

	// Token: 0x06000191 RID: 401 RVA: 0x00008FAC File Offset: 0x000071AC
	public void CancelDash()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Tween tween = this.dashDragTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.HasDashed = false;
		Tween tween2 = this.dashLegPadTween;
		if (tween2 != null)
		{
			tween2.Kill(false);
		}
		this.HasDashExtended = false;
		this.IsExtendedLeft.Value = false;
		this.IsExtendedRight.Value = false;
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00009010 File Offset: 0x00007210
	private void HandlePlayerReference(NetworkObjectReference oldPlayerReference = default(NetworkObjectReference), NetworkObjectReference newPlayerReference = default(NetworkObjectReference))
	{
		NetworkObject networkObject;
		Player player = oldPlayerReference.TryGet(out networkObject, null) ? networkObject.GetComponent<Player>() : null;
		NetworkObject networkObject2;
		Player player2 = newPlayerReference.TryGet(out networkObject2, null) ? networkObject2.GetComponent<Player>() : null;
		if (player)
		{
			player.PlayerBody = null;
		}
		if (player2)
		{
			this.Player = player2;
			this.Player.PlayerBody = this;
		}
		else
		{
			this.Player = null;
		}
		if (this.Player)
		{
			this.ApplyCustomizations();
		}
	}

	// Token: 0x06000193 RID: 403 RVA: 0x00009090 File Offset: 0x00007290
	private void OnPlayerReferenceChanged(NetworkObjectReference oldReference, NetworkObjectReference newReference)
	{
		this.HandlePlayerReference(oldReference, newReference);
		EventManager.TriggerEvent("Event_Everyone_OnPlayerBodyPlayerReferenceChanged", new Dictionary<string, object>
		{
			{
				"playerBody",
				this
			},
			{
				"oldPlayerReference",
				oldReference
			},
			{
				"newPlayerReference",
				newReference
			}
		});
	}

	// Token: 0x06000194 RID: 404 RVA: 0x000090E4 File Offset: 0x000072E4
	private void OnStaminaChanged(float oldStamina, float newStamina)
	{
		if (!EventManager.HasEventListeners("Event_Everyone_OnPlayerBodyStaminaChanged"))
		{
			return;
		}
		EventManager.TriggerEvent("Event_Everyone_OnPlayerBodyStaminaChanged", new Dictionary<string, object>
		{
			{
				"playerBody",
				this
			},
			{
				"oldStamina",
				oldStamina
			},
			{
				"newStamina",
				newStamina
			}
		});
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000913C File Offset: 0x0000733C
	private void OnSpeedChanged(float oldSpeed, float newSpeed)
	{
		if (!EventManager.HasEventListeners("Event_Everyone_OnPlayerBodySpeedChanged"))
		{
			return;
		}
		EventManager.TriggerEvent("Event_Everyone_OnPlayerBodySpeedChanged", new Dictionary<string, object>
		{
			{
				"playerBody",
				this
			},
			{
				"oldSpeed",
				oldSpeed
			},
			{
				"newSpeed",
				newSpeed
			}
		});
	}

	// Token: 0x06000196 RID: 406 RVA: 0x00009193 File Offset: 0x00007393
	private void OnIsSprintingChanged(bool oldIsSprinting, bool newIsSprinting)
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerBodyIsSprintingChanged", new Dictionary<string, object>
		{
			{
				"playerBody",
				this
			},
			{
				"oldIsSprinting",
				oldIsSprinting
			},
			{
				"newIsSprinting",
				newIsSprinting
			}
		});
	}

	// Token: 0x06000197 RID: 407 RVA: 0x000091D4 File Offset: 0x000073D4
	private void OnIsSlidingChanged(bool oldIsSliding, bool newIsSliding)
	{
		this.PlayerMesh.PlayerLegPadLeft.State = (newIsSliding ? (this.IsExtendedLeft.Value ? PlayerLegPadState.ButterflyExtended : PlayerLegPadState.Butterfly) : PlayerLegPadState.Idle);
		this.PlayerMesh.PlayerLegPadRight.State = (newIsSliding ? (this.IsExtendedRight.Value ? PlayerLegPadState.ButterflyExtended : PlayerLegPadState.Butterfly) : PlayerLegPadState.Idle);
		this.CancelDash();
		EventManager.TriggerEvent("Event_Everyone_OnPlayerBodyIsSlidingChanged", new Dictionary<string, object>
		{
			{
				"playerBody",
				this
			},
			{
				"oldIsSliding",
				oldIsSliding
			},
			{
				"newIsSliding",
				newIsSliding
			}
		});
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00009272 File Offset: 0x00007472
	private void OnIsStoppingChanged(bool oldIsStopping, bool newIsStopping)
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerBodyIsStoppingChanged", new Dictionary<string, object>
		{
			{
				"playerBody",
				this
			},
			{
				"oldIsStopping",
				oldIsStopping
			},
			{
				"newIsStopping",
				newIsStopping
			}
		});
	}

	// Token: 0x06000199 RID: 409 RVA: 0x000092B4 File Offset: 0x000074B4
	private void OnIsExtendedLeftChanged(bool oldIsExtendedLeft, bool newIsExtendedLeft)
	{
		this.PlayerMesh.PlayerLegPadLeft.State = (this.IsSliding.Value ? (newIsExtendedLeft ? PlayerLegPadState.ButterflyExtended : PlayerLegPadState.Butterfly) : PlayerLegPadState.Idle);
		EventManager.TriggerEvent("Event_Everyone_OnPlayerBodyIsExtendedLeftChanged", new Dictionary<string, object>
		{
			{
				"playerBody",
				this
			},
			{
				"oldIsExtendedLeft",
				oldIsExtendedLeft
			},
			{
				"newIsExtendedLeft",
				newIsExtendedLeft
			}
		});
	}

	// Token: 0x0600019A RID: 410 RVA: 0x00009328 File Offset: 0x00007528
	private void OnIsExtendedRightChanged(bool oldIsExtendedRight, bool newIsExtendedRight)
	{
		this.PlayerMesh.PlayerLegPadRight.State = (this.IsSliding.Value ? (newIsExtendedRight ? PlayerLegPadState.ButterflyExtended : PlayerLegPadState.Butterfly) : PlayerLegPadState.Idle);
		EventManager.TriggerEvent("Event_Everyone_OnPlayerBodyIsExtendedRightChanged", new Dictionary<string, object>
		{
			{
				"playerBody",
				this
			},
			{
				"oldIsExtendedRight",
				oldIsExtendedRight
			},
			{
				"newIsExtendedRight",
				newIsExtendedRight
			}
		});
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000939C File Offset: 0x0000759C
	public void Server_Teleport(Vector3 position, Quaternion rotation)
	{
		base.transform.position = position;
		base.transform.rotation = rotation;
		this.Rigidbody.position = position;
		this.Rigidbody.rotation = rotation;
		this.Rigidbody.linearVelocity = Vector3.zero;
		this.Rigidbody.angularVelocity = Vector3.zero;
		this.Stick.Server_Teleport(position, rotation);
	}

	// Token: 0x0600019C RID: 412 RVA: 0x00009406 File Offset: 0x00007606
	public void Server_Freeze(RigidbodyConstraints contstraints = RigidbodyConstraints.FreezeAll)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.Rigidbody.constraints = contstraints;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x00009421 File Offset: 0x00007621
	public void Server_Unfreeze()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.Rigidbody.constraints = RigidbodyConstraints.None;
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000943C File Offset: 0x0000763C
	private void Server_UpdateAudio()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (this.Rigidbody.constraints != RigidbodyConstraints.None)
		{
			this.windAudioSource.Server_SetVolume(0f);
			this.iceAudioSource.Server_SetVolume(0f);
			return;
		}
		float time = Mathf.Min(this.Movement.NormalizedMaximumSpeed, 1f);
		this.windAudioSource.Server_SetVolume(this.windVolumeCurve.Evaluate(time));
		float num = (!this.IsGrounded) ? 0f : (this.IsStopping.Value ? 3f : (this.IsSliding.Value ? 1.5f : (this.Skate.IsTractionLost ? 2f : 1f)));
		float num2 = this.IsStopping.Value ? 3f : (this.IsSliding.Value ? 1.5f : (this.Skate.IsTractionLost ? 2f : 1f));
		float time2 = Mathf.Min(this.Movement.NormalizedMaximumSpeed, 1f);
		this.iceAudioSource.Server_SetVolume(this.iceVolumeCurve.Evaluate(time2) * num);
		float time3 = Mathf.Min(this.Movement.NormalizedMaximumSpeed, 1f);
		this.iceAudioSource.Server_SetPitch(this.icePitchCurve.Evaluate(time3) * num2);
	}

	// Token: 0x0600019F RID: 415 RVA: 0x000095A4 File Offset: 0x000077A4
	private void Server_OnCollisionDeferred(GameObject gameObject, float force)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.gruntAudioSource.Server_Play(this.gruntVolumeCurve.Evaluate(force), this.gruntPitchCurve.Evaluate(force), true, -1, 0f, true, false, false, 0f, false, 0f, -1f);
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x000095FC File Offset: 0x000077FC
	private void OnCollisionEnter(Collision collision)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
		{
			return;
		}
		PlayerBody playerBody;
		if (!collision.gameObject.TryGetComponent<PlayerBody>(out playerBody))
		{
			return;
		}
		float normalizedMaximumSpeed = this.Movement.NormalizedMaximumSpeed;
		float normalizedMaximumSpeed2 = playerBody.Movement.NormalizedMaximumSpeed;
		float collisionForce = Utils.GetCollisionForce(collision);
		if (this.Speed.Value < this.tackleSpeedThreshold)
		{
			return;
		}
		if (normalizedMaximumSpeed < normalizedMaximumSpeed2)
		{
			return;
		}
		if (collisionForce < this.tackleForceThreshold)
		{
			return;
		}
		if (this.IsGrounded)
		{
			return;
		}
		if (!this.IsBalanced)
		{
			return;
		}
		if (this.HasSlipped || this.HasFallen.Value)
		{
			return;
		}
		playerBody.OnSlip();
		Vector3 a = Vector3.ClampMagnitude(collision.relativeVelocity, this.tackleBounceMaximumMagnitude);
		playerBody.Rigidbody.AddForceAtPosition(-a * this.tackleForceMultiplier, this.Rigidbody.worldCenterOfMass + base.transform.up * 0.5f, ForceMode.VelocityChange);
	}

	// Token: 0x060001AE RID: 430 RVA: 0x000098BC File Offset: 0x00007ABC
	protected override void __initializeVariables()
	{
		bool flag = this.PlayerReference == null;
		if (flag)
		{
			throw new Exception("PlayerBody.PlayerReference cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.PlayerReference.Initialize(this);
		base.__nameNetworkVariable(this.PlayerReference, "PlayerReference");
		this.NetworkVariableFields.Add(this.PlayerReference);
		flag = (this.Stamina == null);
		if (flag)
		{
			throw new Exception("PlayerBody.Stamina cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.Stamina.Initialize(this);
		base.__nameNetworkVariable(this.Stamina, "Stamina");
		this.NetworkVariableFields.Add(this.Stamina);
		flag = (this.Speed == null);
		if (flag)
		{
			throw new Exception("PlayerBody.Speed cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.Speed.Initialize(this);
		base.__nameNetworkVariable(this.Speed, "Speed");
		this.NetworkVariableFields.Add(this.Speed);
		flag = (this.IsSprinting == null);
		if (flag)
		{
			throw new Exception("PlayerBody.IsSprinting cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.IsSprinting.Initialize(this);
		base.__nameNetworkVariable(this.IsSprinting, "IsSprinting");
		this.NetworkVariableFields.Add(this.IsSprinting);
		flag = (this.IsSliding == null);
		if (flag)
		{
			throw new Exception("PlayerBody.IsSliding cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.IsSliding.Initialize(this);
		base.__nameNetworkVariable(this.IsSliding, "IsSliding");
		this.NetworkVariableFields.Add(this.IsSliding);
		flag = (this.IsStopping == null);
		if (flag)
		{
			throw new Exception("PlayerBody.IsStopping cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.IsStopping.Initialize(this);
		base.__nameNetworkVariable(this.IsStopping, "IsStopping");
		this.NetworkVariableFields.Add(this.IsStopping);
		flag = (this.IsExtendedLeft == null);
		if (flag)
		{
			throw new Exception("PlayerBody.IsExtendedLeft cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.IsExtendedLeft.Initialize(this);
		base.__nameNetworkVariable(this.IsExtendedLeft, "IsExtendedLeft");
		this.NetworkVariableFields.Add(this.IsExtendedLeft);
		flag = (this.IsExtendedRight == null);
		if (flag)
		{
			throw new Exception("PlayerBody.IsExtendedRight cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.IsExtendedRight.Initialize(this);
		base.__nameNetworkVariable(this.IsExtendedRight, "IsExtendedRight");
		this.NetworkVariableFields.Add(this.IsExtendedRight);
		flag = (this.HasFallen == null);
		if (flag)
		{
			throw new Exception("PlayerBody.HasFallen cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.HasFallen.Initialize(this);
		base.__nameNetworkVariable(this.HasFallen, "HasFallen");
		this.NetworkVariableFields.Add(this.HasFallen);
		base.__initializeVariables();
	}

	// Token: 0x060001AF RID: 431 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00009B87 File Offset: 0x00007D87
	protected internal override string __getTypeName()
	{
		return "PlayerBody";
	}

	// Token: 0x04000138 RID: 312
	[Header("Settings")]
	[SerializeField]
	private float gravityMultiplier = 2f;

	// Token: 0x04000139 RID: 313
	[SerializeField]
	private float hoverDistance = 1.2f;

	// Token: 0x0400013A RID: 314
	[Space(20f)]
	[SerializeField]
	private float upwardnessThreshold = 0.8f;

	// Token: 0x0400013B RID: 315
	[SerializeField]
	private float sidewaysThreshold = 0.2f;

	// Token: 0x0400013C RID: 316
	[Space(20f)]
	[SerializeField]
	private float balanceLossTime = 0.25f;

	// Token: 0x0400013D RID: 317
	[SerializeField]
	private float balanceRecoveryTime = 5f;

	// Token: 0x0400013E RID: 318
	[Space(20f)]
	[SerializeField]
	private float staminaRegenerationRate = 10f;

	// Token: 0x0400013F RID: 319
	[Space(20f)]
	[SerializeField]
	private float sprintStaminaDrainRate = 1.4f;

	// Token: 0x04000140 RID: 320
	[Space(20f)]
	[SerializeField]
	private float slideTurnMultiplier = 2f;

	// Token: 0x04000141 RID: 321
	[SerializeField]
	private float slideHoverDistance = 0.8f;

	// Token: 0x04000142 RID: 322
	[Space(20f)]
	[SerializeField]
	private float jumpVelocity = 6f;

	// Token: 0x04000143 RID: 323
	[SerializeField]
	private float jumpStaminaDrain = 0.125f;

	// Token: 0x04000144 RID: 324
	[SerializeField]
	private float jumpTurnMultiplier = 5f;

	// Token: 0x04000145 RID: 325
	[Space(20f)]
	[SerializeField]
	private float twistVelocity = 5f;

	// Token: 0x04000146 RID: 326
	[SerializeField]
	private float twistStaminaDrain = 0.125f;

	// Token: 0x04000147 RID: 327
	[Space(20f)]
	[SerializeField]
	private bool canDash = true;

	// Token: 0x04000148 RID: 328
	[SerializeField]
	private float dashVelocity = 6f;

	// Token: 0x04000149 RID: 329
	[SerializeField]
	private float dashStaminaDrain = 0.125f;

	// Token: 0x0400014A RID: 330
	[SerializeField]
	private float dashDrag = 5f;

	// Token: 0x0400014B RID: 331
	[SerializeField]
	private float dashDragTime = 1f;

	// Token: 0x0400014C RID: 332
	[Space(20f)]
	[SerializeField]
	private float slideDrag = 0.2f;

	// Token: 0x0400014D RID: 333
	[SerializeField]
	private float stopDrag = 2.5f;

	// Token: 0x0400014E RID: 334
	[SerializeField]
	private float fallenDrag = 0.2f;

	// Token: 0x0400014F RID: 335
	[Space(20f)]
	[SerializeField]
	private float tackleSpeedThreshold = 7.6f;

	// Token: 0x04000150 RID: 336
	[SerializeField]
	private float tackleForceThreshold = 7f;

	// Token: 0x04000151 RID: 337
	[SerializeField]
	private float tackleForceMultiplier = 0.3f;

	// Token: 0x04000152 RID: 338
	[SerializeField]
	private float tackleBounceMaximumMagnitude = 10f;

	// Token: 0x04000153 RID: 339
	[Space(20f)]
	[SerializeField]
	private float stretchSpeed = 10f;

	// Token: 0x04000154 RID: 340
	[Space(20f)]
	[SerializeField]
	private float maximumLaterality = 1f;

	// Token: 0x04000155 RID: 341
	[SerializeField]
	private float minimumLaterality = 0.5f;

	// Token: 0x04000156 RID: 342
	[SerializeField]
	private float minimumLateralitySpeed = 2f;

	// Token: 0x04000157 RID: 343
	[SerializeField]
	private float maximumLateralitySpeed = 5f;

	// Token: 0x04000158 RID: 344
	[Space(20f)]
	[SerializeField]
	private AnimationCurve windVolumeCurve;

	// Token: 0x04000159 RID: 345
	[SerializeField]
	private AnimationCurve iceVolumeCurve;

	// Token: 0x0400015A RID: 346
	[SerializeField]
	private AnimationCurve icePitchCurve;

	// Token: 0x0400015B RID: 347
	[SerializeField]
	private AnimationCurve gruntVolumeCurve;

	// Token: 0x0400015C RID: 348
	[SerializeField]
	private AnimationCurve gruntPitchCurve;

	// Token: 0x0400015D RID: 349
	[Header("References")]
	[SerializeField]
	private Transform movementDirection;

	// Token: 0x0400015E RID: 350
	[SerializeField]
	private PlayerMesh playerMesh;

	// Token: 0x0400015F RID: 351
	[SerializeField]
	private SynchronizedAudio windAudioSource;

	// Token: 0x04000160 RID: 352
	[SerializeField]
	private SynchronizedAudio iceAudioSource;

	// Token: 0x04000161 RID: 353
	[SerializeField]
	private SynchronizedAudio gruntAudioSource;

	// Token: 0x04000162 RID: 354
	[SerializeField]
	private AudioSource voiceAudioSource;

	// Token: 0x04000163 RID: 355
	[HideInInspector]
	public NetworkVariable<NetworkObjectReference> PlayerReference;

	// Token: 0x04000164 RID: 356
	[HideInInspector]
	public CompressedNetworkVariable<float, byte> Stamina;

	// Token: 0x04000165 RID: 357
	[HideInInspector]
	public CompressedNetworkVariable<float, byte> Speed;

	// Token: 0x04000166 RID: 358
	[HideInInspector]
	public NetworkVariable<bool> IsSprinting;

	// Token: 0x04000167 RID: 359
	[HideInInspector]
	public NetworkVariable<bool> IsSliding;

	// Token: 0x04000168 RID: 360
	[HideInInspector]
	public NetworkVariable<bool> IsStopping;

	// Token: 0x04000169 RID: 361
	[HideInInspector]
	public NetworkVariable<bool> IsExtendedLeft;

	// Token: 0x0400016A RID: 362
	[HideInInspector]
	public NetworkVariable<bool> IsExtendedRight;

	// Token: 0x0400016B RID: 363
	[HideInInspector]
	public Player Player;

	// Token: 0x0400016C RID: 364
	[HideInInspector]
	public Rigidbody Rigidbody;

	// Token: 0x0400016D RID: 365
	[HideInInspector]
	public SynchronizedObject SynchronizedObject;

	// Token: 0x0400016E RID: 366
	[HideInInspector]
	public Movement Movement;

	// Token: 0x0400016F RID: 367
	[HideInInspector]
	public VelocityLean VelocityLean;

	// Token: 0x04000170 RID: 368
	[HideInInspector]
	public Hover Hover;

	// Token: 0x04000171 RID: 369
	[HideInInspector]
	public Skate Skate;

	// Token: 0x04000172 RID: 370
	[HideInInspector]
	public KeepUpright KeepUpright;

	// Token: 0x04000173 RID: 371
	[HideInInspector]
	public MeshRendererHider MeshRendererHider;

	// Token: 0x04000174 RID: 372
	[HideInInspector]
	public CollisionRecorder CollisionRecorder;

	// Token: 0x04000175 RID: 373
	[HideInInspector]
	public bool HasDashed;

	// Token: 0x04000176 RID: 374
	[HideInInspector]
	public bool HasDashExtended;

	// Token: 0x04000177 RID: 375
	[HideInInspector]
	public bool HasSlipped;

	// Token: 0x04000178 RID: 376
	[HideInInspector]
	public NetworkVariable<bool> HasFallen;

	// Token: 0x04000179 RID: 377
	[HideInInspector]
	public float Laterality;

	// Token: 0x0400017A RID: 378
	private bool isNetworkVariablesInitialized;

	// Token: 0x0400017B RID: 379
	private Collider[] bodyColliders;

	// Token: 0x0400017C RID: 380
	private SoftCollider softCollider;

	// Token: 0x0400017D RID: 381
	private Tween balanceLossTween;

	// Token: 0x0400017E RID: 382
	private Tween balanceRecoveryTween;

	// Token: 0x0400017F RID: 383
	private Tween dashDragTween;

	// Token: 0x04000180 RID: 384
	private Tween dashLegPadTween;
}
