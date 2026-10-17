using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class StickPositioner : NetworkBehaviour
{
	// Token: 0x17000056 RID: 86
	// (get) Token: 0x0600032E RID: 814 RVA: 0x000143D9 File Offset: 0x000125D9
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

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x0600032F RID: 815 RVA: 0x000143F6 File Offset: 0x000125F6
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

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x06000330 RID: 816 RVA: 0x00014413 File Offset: 0x00012613
	[HideInInspector]
	public Vector3 BladeTargetPosition
	{
		get
		{
			return this.bladeTarget.transform.position;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000331 RID: 817 RVA: 0x00014425 File Offset: 0x00012625
	[HideInInspector]
	public Vector3 BladeTargetVelocity
	{
		get
		{
			return this.bladeTargetVelocity;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000332 RID: 818 RVA: 0x0001442D File Offset: 0x0001262D
	[HideInInspector]
	public Vector3 ShaftTargetPosition
	{
		get
		{
			return this.shaftTarget.transform.position;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000333 RID: 819 RVA: 0x0001443F File Offset: 0x0001263F
	[HideInInspector]
	public Vector3 RaycastOriginPosition
	{
		get
		{
			return this.raycastOrigin.transform.position;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000334 RID: 820 RVA: 0x00014451 File Offset: 0x00012651
	private Vector3 BladeTargetFocusPointInitialLocalPosition
	{
		get
		{
			if (this.Handedness != PlayerHandedness.Left)
			{
				return this.bladeTargetFocusPointInitialLocalPosition;
			}
			return new Vector3(-this.bladeTargetFocusPointInitialLocalPosition.x, this.bladeTargetFocusPointInitialLocalPosition.y, this.bladeTargetFocusPointInitialLocalPosition.z);
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000335 RID: 821 RVA: 0x0001448A File Offset: 0x0001268A
	private Vector3 RaycastOriginInitialLocalPosition
	{
		get
		{
			if (this.Handedness != PlayerHandedness.Left)
			{
				return this.raycastOriginInitialLocalPosition;
			}
			return new Vector3(-this.raycastOriginInitialLocalPosition.x, this.raycastOriginInitialLocalPosition.y, this.raycastOriginInitialLocalPosition.z);
		}
	}

	// Token: 0x06000336 RID: 822 RVA: 0x000144C3 File Offset: 0x000126C3
	private void Awake()
	{
		this.bladeTargetFocusPointInitialLocalPosition = this.bladeTargetFocusPoint.transform.localPosition;
		this.raycastOriginInitialLocalPosition = this.raycastOrigin.transform.localPosition;
	}

	// Token: 0x06000337 RID: 823 RVA: 0x000144F4 File Offset: 0x000126F4
	protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
	{
		this.InitializeNetworkVariables(default(NetworkObjectReference));
		base.OnNetworkPreSpawn(ref networkManager);
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00014517 File Offset: 0x00012717
	public override void OnNetworkSpawn()
	{
		NetworkVariable<NetworkObjectReference> playerReference = this.PlayerReference;
		playerReference.OnValueChanged = (NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate)Delegate.Combine(playerReference.OnValueChanged, new NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate(this.OnPlayerReferenceChanged));
		base.OnNetworkSpawn();
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00014548 File Offset: 0x00012748
	protected override void OnNetworkPostSpawn()
	{
		NetworkObjectReference value = this.PlayerReference.Value;
		this.HandlePlayerReference(default(NetworkObjectReference), value);
		EventManager.TriggerEvent("Event_Everyone_OnStickPositionerSpawned", new Dictionary<string, object>
		{
			{
				"stickPositioner",
				this
			}
		});
		base.OnNetworkPostSpawn();
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00014592 File Offset: 0x00012792
	public override void OnNetworkDespawn()
	{
		NetworkVariable<NetworkObjectReference> playerReference = this.PlayerReference;
		playerReference.OnValueChanged = (NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate)Delegate.Remove(playerReference.OnValueChanged, new NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate(this.OnPlayerReferenceChanged));
		base.OnNetworkDespawn();
	}

	// Token: 0x0600033B RID: 827 RVA: 0x000145C1 File Offset: 0x000127C1
	public void InitializeNetworkVariables(NetworkObjectReference playerReference = default(NetworkObjectReference))
	{
		if (this.isNetworkVariablesInitialized)
		{
			return;
		}
		this.isNetworkVariablesInitialized = true;
		this.PlayerReference = new NetworkVariable<NetworkObjectReference>(playerReference, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
	}

	// Token: 0x0600033C RID: 828 RVA: 0x000145E4 File Offset: 0x000127E4
	private void FixedUpdate()
	{
		if (!this.Player)
		{
			return;
		}
		this.pidController.proportionalGain = this.proportionalGain;
		this.pidController.integralGain = this.integralGain;
		this.pidController.integralSaturation = this.integralSaturation;
		this.pidController.derivativeMeasurement = this.derivativeMeasurement;
		this.pidController.derivativeGain = this.derivativeGain;
		this.pidController.derivativeSmoothing = this.derivativeSmoothing;
		this.pidController.outputMin = this.outputMin;
		this.pidController.outputMax = this.outputMax;
		this.raycastOriginAngleTarget = this.Player.PlayerInput.StickRaycastOriginAngleInput.ServerValue;
		this.ShootPaddingRay();
		this.RotateRaycastOrigin();
		this.ShootRaycast();
		this.UpdateAudio();
	}

	// Token: 0x0600033D RID: 829 RVA: 0x000146BC File Offset: 0x000128BC
	private void ShootPaddingRay()
	{
		Vector3 vector = new Vector3(0f, this.RaycastOriginInitialLocalPosition.y, 0f);
		Vector3 normalized = (this.RaycastOriginInitialLocalPosition - vector).normalized;
		float num = Vector3.Distance(vector, this.RaycastOriginInitialLocalPosition) + this.raycastOriginPadding;
		Vector3 vector2 = base.transform.TransformPoint(vector);
		Vector3 vector3 = base.transform.TransformDirection(normalized);
		Debug.DrawRay(vector2, vector3 * num, Color.yellow);
		RaycastHit raycastHit;
		if (Physics.Raycast(vector2, vector3, out raycastHit, num, this.raycastLayerMask))
		{
			this.raycastOrigin.transform.localPosition = vector + normalized * (raycastHit.distance - this.raycastOriginPadding);
			return;
		}
		this.raycastOrigin.transform.localPosition = this.RaycastOriginInitialLocalPosition;
	}

	// Token: 0x0600033E RID: 830 RVA: 0x00014794 File Offset: 0x00012994
	private void RotateRaycastOrigin()
	{
		this.raycastOriginAngleDelta = this.pidController.Update(Time.fixedDeltaTime, this.raycastOriginAngle, this.raycastOriginAngleTarget);
		this.raycastOriginAngle += this.raycastOriginAngleDelta * Time.fixedDeltaTime;
		this.raycastOrigin.transform.localRotation = Quaternion.Euler(this.raycastOriginAngle);
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00014814 File Offset: 0x00012A14
	private void ShootRaycast()
	{
		RaycastHit hit;
		Vector3 vector5;
		if (Physics.Raycast(this.raycastOrigin.transform.position, this.raycastOrigin.transform.forward, out hit, this.maximumReach, this.raycastLayerMask))
		{
			this.OnGrounded(hit.transform.gameObject);
			Vector3 vector = this.raycastOrigin.transform.position + this.raycastOrigin.transform.forward * this.maximumReach;
			Vector3 b = Vector3.Scale(Utils.Vector3Abs(hit.normal), hit.point);
			Vector3 vector2 = vector - Vector3.Scale(Utils.Vector3Abs(hit.normal), vector) + b;
			Debug.DrawRay(this.raycastOrigin.transform.position, this.raycastOrigin.transform.forward * hit.distance, Color.red);
			Vector3 normalized = (vector2 - this.raycastOrigin.transform.position).normalized;
			RaycastHit raycastHit;
			if (Physics.Raycast(this.raycastOrigin.transform.position, normalized, out raycastHit, this.maximumReach, this.raycastLayerMask))
			{
				Vector3 vector3 = this.raycastOrigin.transform.position + this.raycastOrigin.transform.forward * this.maximumReach;
				Vector3 b2 = Vector3.Scale(Utils.Vector3Abs(raycastHit.normal), raycastHit.point);
				Vector3 vector4 = vector3 - Vector3.Scale(Utils.Vector3Abs(raycastHit.normal), vector3) + b2;
				if (hit.normal == Vector3.up && raycastHit.normal == Vector3.up)
				{
					vector5 = vector2;
				}
				else if (hit.normal == Vector3.up && raycastHit.normal != Vector3.up)
				{
					vector5 = vector4;
					vector5.y = Mathf.Max(0f, vector5.y);
				}
				else
				{
					vector5 = vector2;
					vector5.y = Mathf.Max(0f, vector5.y);
				}
				Debug.DrawRay(this.raycastOrigin.transform.position, normalized * raycastHit.distance, Color.blue);
			}
			else
			{
				vector5 = hit.point;
			}
			this.ApplySoftCollision(hit, vector5);
		}
		else
		{
			this.OnUngrounded();
			vector5 = this.raycastOrigin.transform.position + this.raycastOrigin.transform.forward * this.maximumReach;
			Debug.DrawRay(this.raycastOrigin.transform.position, this.raycastOrigin.transform.forward * this.maximumReach, Color.red);
		}
		this.PositionBladeTarget(vector5);
		this.PositionBladeTargetFocusPoint(vector5);
		this.RotateBladeTargetFocusPoint();
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00014B18 File Offset: 0x00012D18
	private void PositionBladeTarget(Vector3 hitPosition)
	{
		this.bladeTarget.transform.position = hitPosition;
		this.bladeTarget.transform.rotation = Quaternion.LookRotation(this.bladeTarget.transform.position - this.bladeTargetFocusPoint.transform.position);
		this.bladeTargetVelocity = (this.bladeTarget.transform.position - this.lastBladeTargetPosition) / Time.fixedDeltaTime;
		this.lastBladeTargetPosition = this.bladeTarget.transform.position;
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00014BB4 File Offset: 0x00012DB4
	private void PositionBladeTargetFocusPoint(Vector3 hitPosition)
	{
		float num = Vector3.Distance(this.raycastOrigin.transform.position, new Vector3(hitPosition.x, this.raycastOrigin.transform.position.y, hitPosition.z));
		float d = this.maximumReach - num;
		Vector3 vector = this.raycastOrigin.transform.localPosition - base.transform.InverseTransformPoint(hitPosition);
		Vector3 normalized = new Vector3(vector.x, 0f, vector.z).normalized;
		Vector3 a = base.transform.TransformDirection(normalized);
		Debug.DrawRay(this.bladeTargetFocusPoint.transform.position, -a * d, Color.grey);
		Vector3 b = normalized * d;
		this.bladeTargetFocusPoint.transform.localPosition = this.BladeTargetFocusPointInitialLocalPosition + b;
	}

	// Token: 0x06000342 RID: 834 RVA: 0x00014CA4 File Offset: 0x00012EA4
	private void RotateBladeTargetFocusPoint()
	{
		PlayerInput playerInput = this.Player.PlayerInput;
		if (!playerInput)
		{
			return;
		}
		float num = Mathf.Lerp(1f, 0f, (playerInput.MaximumStickRaycastOriginAngle.x - this.raycastOriginAngle.x) / this.bladeTargetRotationThreshold);
		num *= (float)((this.Handedness == PlayerHandedness.Left) ? -1 : 1);
		this.bladeTargetFocusPoint.transform.localPosition = Utils.RotatePointAroundPivot(this.bladeTargetFocusPoint.transform.localPosition, this.bladeTarget.transform.localPosition, new Vector3(0f, this.bladeTargetMaxAngle * num, 0f));
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00014D51 File Offset: 0x00012F51
	public void PrepareShaftTarget(Stick stick)
	{
		this.shaftTarget.transform.localPosition = stick.ShaftHandleLocalPosition - stick.BladeHandleLocalPosition;
	}

	// Token: 0x06000344 RID: 836 RVA: 0x00014D74 File Offset: 0x00012F74
	private void ApplySoftCollision(RaycastHit hit, Vector3 hitPosition)
	{
		if (!this.applySoftCollision)
		{
			return;
		}
		if (!this.PlayerBody || this.PlayerBody.SynchronizedObject.IsDrivenExternally)
		{
			return;
		}
		if (hit.collider.CompareTag("Soft Collider"))
		{
			float d = this.maximumReach - hit.distance;
			float magnitude = Vector3.Cross(hit.normal, this.raycastOrigin.transform.forward).magnitude;
			float num = 1f - magnitude;
			Debug.DrawRay(hitPosition, hit.normal * d * this.softCollisionForce, Color.green);
			this.PlayerBody.Rigidbody.AddForceAtPosition(hit.normal * d * (this.softCollisionForce * num), hitPosition, ForceMode.Acceleration);
		}
	}

	// Token: 0x06000345 RID: 837 RVA: 0x00014E4C File Offset: 0x0001304C
	private void UpdateAudio()
	{
		this.windAudioSource.transform.position = this.BladeTargetPosition;
		this.iceHitAudioSource.transform.position = this.BladeTargetPosition;
		this.iceDragAudioSource.transform.position = this.BladeTargetPosition;
		if (this.PlayerBody && this.PlayerBody.Rigidbody.constraints != RigidbodyConstraints.None)
		{
			this.windAudioSource.Server_SetVolume(0f);
			this.iceDragAudioSource.Server_SetVolume(0f);
			return;
		}
		float num = this.IsGrounded ? this.iceDragVolumeCurve.Evaluate(this.BladeTargetVelocity.magnitude) : 0f;
		if (num > this.iceDragVolume)
		{
			this.iceDragVolume = num;
		}
		else
		{
			this.iceDragVolume = Mathf.Lerp(this.iceDragVolume, num, Time.fixedDeltaTime * this.iceDragVolumeFallOffSpeed);
		}
		this.iceDragPitch = this.iceDragPitchCurve.Evaluate(this.BladeTargetVelocity.magnitude);
		this.iceDragAudioSource.Server_SetVolume(this.iceDragVolume);
		this.iceDragAudioSource.Server_SetPitch(this.iceDragPitch);
		float volume = this.windVolumeCurve.Evaluate(this.raycastOriginAngleDelta.magnitude);
		float pitch = this.windPitchCurve.Evaluate(this.raycastOriginAngleDelta.magnitude);
		this.windAudioSource.Server_SetVolume(volume);
		this.windAudioSource.Server_SetPitch(pitch);
	}

	// Token: 0x06000346 RID: 838 RVA: 0x00014FBC File Offset: 0x000131BC
	private void OnGrounded(GameObject ground)
	{
		if (this.IsGrounded)
		{
			return;
		}
		if (ground.layer == LayerMask.NameToLayer("Ice"))
		{
			float volume = this.iceHitVolumeCurve.Evaluate(Mathf.Abs(this.raycastOriginAngleDelta.x));
			float pitch = this.iceHitPitchCurve.Evaluate(Mathf.Abs(this.raycastOriginAngleDelta.x));
			this.iceHitAudioSource.Server_Play(volume, pitch, true, -1, 0f, true, false, false, 0f, false, 0f, -1f);
		}
		this.IsGrounded = true;
	}

	// Token: 0x06000347 RID: 839 RVA: 0x0001504A File Offset: 0x0001324A
	private void OnUngrounded()
	{
		if (!this.IsGrounded)
		{
			return;
		}
		this.IsGrounded = false;
	}

	// Token: 0x06000348 RID: 840 RVA: 0x0001505C File Offset: 0x0001325C
	private void HandlePlayerReference(NetworkObjectReference oldPlayerReference = default(NetworkObjectReference), NetworkObjectReference newPlayerReference = default(NetworkObjectReference))
	{
		NetworkObject networkObject;
		Player player = oldPlayerReference.TryGet(out networkObject, null) ? networkObject.GetComponent<Player>() : null;
		NetworkObject networkObject2;
		Player player2 = newPlayerReference.TryGet(out networkObject2, null) ? networkObject2.GetComponent<Player>() : null;
		if (player)
		{
			player.StickPositioner = null;
		}
		if (player2)
		{
			this.Player = player2;
			this.Player.StickPositioner = this;
		}
		else
		{
			this.Player = null;
		}
		if (this.Player)
		{
			if (this.Player.Stick)
			{
				this.PrepareShaftTarget(this.Player.Stick);
			}
			this.Handedness = this.Player.Handedness.Value;
		}
	}

	// Token: 0x06000349 RID: 841 RVA: 0x0001510E File Offset: 0x0001330E
	private void OnPlayerReferenceChanged(NetworkObjectReference oldPlayerReference, NetworkObjectReference newPlayerReference)
	{
		this.HandlePlayerReference(oldPlayerReference, newPlayerReference);
	}

	// Token: 0x0600034B RID: 843 RVA: 0x00015218 File Offset: 0x00013418
	protected override void __initializeVariables()
	{
		bool flag = this.PlayerReference == null;
		if (flag)
		{
			throw new Exception("StickPositioner.PlayerReference cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.PlayerReference.Initialize(this);
		base.__nameNetworkVariable(this.PlayerReference, "PlayerReference");
		this.NetworkVariableFields.Add(this.PlayerReference);
		base.__initializeVariables();
	}

	// Token: 0x0600034C RID: 844 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x0600034D RID: 845 RVA: 0x0001527B File Offset: 0x0001347B
	protected internal override string __getTypeName()
	{
		return "StickPositioner";
	}

	// Token: 0x0400025A RID: 602
	[Header("Settings")]
	[SerializeField]
	private float proportionalGain = 0.75f;

	// Token: 0x0400025B RID: 603
	[SerializeField]
	private float integralGain = 5f;

	// Token: 0x0400025C RID: 604
	[SerializeField]
	private float integralSaturation = 5f;

	// Token: 0x0400025D RID: 605
	[SerializeField]
	private DerivativeMeasurement derivativeMeasurement;

	// Token: 0x0400025E RID: 606
	[SerializeField]
	private float derivativeGain;

	// Token: 0x0400025F RID: 607
	[SerializeField]
	private float derivativeSmoothing;

	// Token: 0x04000260 RID: 608
	[SerializeField]
	private float outputMin = -15f;

	// Token: 0x04000261 RID: 609
	[SerializeField]
	private float outputMax = 15f;

	// Token: 0x04000262 RID: 610
	[Space(20f)]
	[SerializeField]
	private float maximumReach = 2.5f;

	// Token: 0x04000263 RID: 611
	[Space(20f)]
	[SerializeField]
	private float bladeTargetRotationThreshold = 25f;

	// Token: 0x04000264 RID: 612
	[SerializeField]
	private float bladeTargetMaxAngle = 45f;

	// Token: 0x04000265 RID: 613
	[Space(20f)]
	[SerializeField]
	private LayerMask raycastLayerMask;

	// Token: 0x04000266 RID: 614
	[Space(20f)]
	[SerializeField]
	private float raycastOriginPadding = 0.2f;

	// Token: 0x04000267 RID: 615
	[Space(20f)]
	[SerializeField]
	private bool applySoftCollision = true;

	// Token: 0x04000268 RID: 616
	[SerializeField]
	private float softCollisionForce = 1f;

	// Token: 0x04000269 RID: 617
	[Space(20f)]
	[SerializeField]
	private AnimationCurve windVolumeCurve;

	// Token: 0x0400026A RID: 618
	[SerializeField]
	private AnimationCurve windPitchCurve;

	// Token: 0x0400026B RID: 619
	[SerializeField]
	private AnimationCurve iceHitVolumeCurve;

	// Token: 0x0400026C RID: 620
	[SerializeField]
	private AnimationCurve iceHitPitchCurve;

	// Token: 0x0400026D RID: 621
	[SerializeField]
	private AnimationCurve iceDragVolumeCurve;

	// Token: 0x0400026E RID: 622
	[SerializeField]
	private AnimationCurve iceDragPitchCurve;

	// Token: 0x0400026F RID: 623
	[Header("References")]
	[SerializeField]
	private GameObject raycastOrigin;

	// Token: 0x04000270 RID: 624
	[SerializeField]
	private GameObject bladeTargetFocusPoint;

	// Token: 0x04000271 RID: 625
	[SerializeField]
	private GameObject bladeTarget;

	// Token: 0x04000272 RID: 626
	[SerializeField]
	private GameObject shaftTarget;

	// Token: 0x04000273 RID: 627
	[Space(20f)]
	[SerializeField]
	private SynchronizedAudio windAudioSource;

	// Token: 0x04000274 RID: 628
	[SerializeField]
	private SynchronizedAudio iceHitAudioSource;

	// Token: 0x04000275 RID: 629
	[SerializeField]
	private SynchronizedAudio iceDragAudioSource;

	// Token: 0x04000276 RID: 630
	[SerializeField]
	private float iceDragVolumeFallOffSpeed = 10f;

	// Token: 0x04000277 RID: 631
	[HideInInspector]
	public NetworkVariable<NetworkObjectReference> PlayerReference;

	// Token: 0x04000278 RID: 632
	[HideInInspector]
	public Player Player;

	// Token: 0x04000279 RID: 633
	[HideInInspector]
	public bool IsGrounded;

	// Token: 0x0400027A RID: 634
	[HideInInspector]
	public PlayerHandedness Handedness;

	// Token: 0x0400027B RID: 635
	private bool isNetworkVariablesInitialized;

	// Token: 0x0400027C RID: 636
	private Vector3 lastBladeTargetPosition = Vector3.zero;

	// Token: 0x0400027D RID: 637
	private Vector3 bladeTargetVelocity = Vector3.zero;

	// Token: 0x0400027E RID: 638
	private Vector3 bladeTargetFocusPointInitialLocalPosition = Vector3.zero;

	// Token: 0x0400027F RID: 639
	private Vector3 raycastOriginInitialLocalPosition = Vector3.zero;

	// Token: 0x04000280 RID: 640
	private Vector2 raycastOriginAngleTarget = Vector2.zero;

	// Token: 0x04000281 RID: 641
	private Vector2 raycastOriginAngle = Vector2.zero;

	// Token: 0x04000282 RID: 642
	private Vector2 raycastOriginAngleDelta = Vector3.zero;

	// Token: 0x04000283 RID: 643
	private float iceDragVolume;

	// Token: 0x04000284 RID: 644
	private float iceDragPitch;

	// Token: 0x04000285 RID: 645
	private Vector3PIDController pidController = new Vector3PIDController(0f, 0f, 0f);
}
