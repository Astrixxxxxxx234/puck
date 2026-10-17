using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class Stick : NetworkBehaviour
{
	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060002FE RID: 766 RVA: 0x00013839 File Offset: 0x00011A39
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

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060002FF RID: 767 RVA: 0x00013856 File Offset: 0x00011A56
	[HideInInspector]
	public StickPositioner StickPositioner
	{
		get
		{
			if (!(this.Player == null))
			{
				return this.Player.StickPositioner;
			}
			return null;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000300 RID: 768 RVA: 0x00013873 File Offset: 0x00011A73
	[HideInInspector]
	public StickMesh StickMesh
	{
		get
		{
			return this.stickMesh;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000301 RID: 769 RVA: 0x0001387B File Offset: 0x00011A7B
	[HideInInspector]
	public Vector3 ShaftHandleLocalPosition
	{
		get
		{
			return this.shaftHandle.transform.localPosition;
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06000302 RID: 770 RVA: 0x0001388D File Offset: 0x00011A8D
	[HideInInspector]
	public Vector3 BladeHandleLocalPosition
	{
		get
		{
			return this.bladeHandle.transform.localPosition;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x06000303 RID: 771 RVA: 0x0001389F File Offset: 0x00011A9F
	[HideInInspector]
	public Vector3 ShaftHandlePosition
	{
		get
		{
			return this.shaftHandle.transform.position;
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x06000304 RID: 772 RVA: 0x000138B1 File Offset: 0x00011AB1
	[HideInInspector]
	public Vector3 BladeHandlePosition
	{
		get
		{
			return this.bladeHandle.transform.position;
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x06000305 RID: 773 RVA: 0x000138C3 File Offset: 0x00011AC3
	[HideInInspector]
	public Quaternion BladeHandleRotation
	{
		get
		{
			return this.bladeHandle.transform.rotation;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x06000306 RID: 774 RVA: 0x000138D5 File Offset: 0x00011AD5
	[HideInInspector]
	public float BladeAngle
	{
		get
		{
			if (!(this.Player == null))
			{
				return (float)this.Player.PlayerInput.BladeAngleInput.ServerValue * this.bladeAngleStep;
			}
			return 0f;
		}
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00013908 File Offset: 0x00011B08
	private void Awake()
	{
		this.Rigidbody = base.GetComponent<Rigidbody>();
		this.SynchronizedObject = base.GetComponent<SynchronizedObject>();
		this.NetworkObjectCollisionRecorder = base.GetComponent<NetworkObjectCollisionRecorder>();
		this.Length = Vector3.Distance(this.ShaftHandlePosition, this.BladeHandlePosition);
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00013948 File Offset: 0x00011B48
	protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
	{
		this.InitializeNetworkVariables(default(NetworkObjectReference));
		base.OnNetworkPreSpawn(ref networkManager);
	}

	// Token: 0x06000309 RID: 777 RVA: 0x0001396B File Offset: 0x00011B6B
	public override void OnNetworkSpawn()
	{
		NetworkVariable<NetworkObjectReference> playerReference = this.PlayerReference;
		playerReference.OnValueChanged = (NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate)Delegate.Combine(playerReference.OnValueChanged, new NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate(this.OnPlayerReferenceChanged));
		base.OnNetworkSpawn();
	}

	// Token: 0x0600030A RID: 778 RVA: 0x0001399C File Offset: 0x00011B9C
	protected override void OnNetworkPostSpawn()
	{
		NetworkObjectReference value = this.PlayerReference.Value;
		this.HandlePlayerReference(default(NetworkObjectReference), value);
		EventManager.TriggerEvent("Event_Everyone_OnStickSpawned", new Dictionary<string, object>
		{
			{
				"stick",
				this
			}
		});
		base.OnNetworkPostSpawn();
	}

	// Token: 0x0600030B RID: 779 RVA: 0x000139E8 File Offset: 0x00011BE8
	public override void OnNetworkDespawn()
	{
		EventManager.TriggerEvent("Event_Everyone_OnStickDespawned", new Dictionary<string, object>
		{
			{
				"stick",
				this
			}
		});
		NetworkVariable<NetworkObjectReference> playerReference = this.PlayerReference;
		playerReference.OnValueChanged = (NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate)Delegate.Remove(playerReference.OnValueChanged, new NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate(this.OnPlayerReferenceChanged));
		base.OnNetworkDespawn();
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00013A3D File Offset: 0x00011C3D
	public override void OnDestroy()
	{
		base.transform.DOKill(false);
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00013A4C File Offset: 0x00011C4C
	public void InitializeNetworkVariables(NetworkObjectReference playerReference = default(NetworkObjectReference))
	{
		if (this.isNetworkVariablesInitialized)
		{
			return;
		}
		this.isNetworkVariablesInitialized = true;
		this.PlayerReference = new NetworkVariable<NetworkObjectReference>(playerReference, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00013A6C File Offset: 0x00011C6C
	private void FixedUpdate()
	{
		this.Server_FixedUpdate();
		if (!this.Player)
		{
			return;
		}
		float angle = (float)this.Player.PlayerInput.BladeAngleInput.ServerValue * this.bladeAngleStep;
		this.rotationContainer.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	// Token: 0x0600030F RID: 783 RVA: 0x00013AC8 File Offset: 0x00011CC8
	public void ApplyCustomizations()
	{
		if (this.Player.Team == PlayerTeam.None || this.Player.Role == PlayerRole.None)
		{
			return;
		}
		this.SetSkinID(this.Player.GetPlayerStickSkinID(), this.Player.Team);
		this.SetShaftTapeID(this.Player.GetPlayerStickShaftTapeID());
		this.SetBladeTapeID(this.Player.GetPlayerStickBladeTapeID());
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00013B2E File Offset: 0x00011D2E
	public void SetSkinID(int skinID, PlayerTeam team)
	{
		this.stickMesh.SetSkinID(skinID, team);
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00013B3D File Offset: 0x00011D3D
	public void SetShaftTapeID(int shaftTapeID)
	{
		this.stickMesh.SetShaftTapeID(shaftTapeID);
	}

	// Token: 0x06000312 RID: 786 RVA: 0x00013B4B File Offset: 0x00011D4B
	public void SetBladeTapeID(int bladeTapeID)
	{
		this.stickMesh.SetBladeTapeID(bladeTapeID);
	}

	// Token: 0x06000313 RID: 787 RVA: 0x00013B59 File Offset: 0x00011D59
	private void OnCollisionStay(Collision collision)
	{
		this.Server_OnCollisionStay(collision);
	}

	// Token: 0x06000314 RID: 788 RVA: 0x00013B64 File Offset: 0x00011D64
	private void HandlePlayerReference(NetworkObjectReference oldPlayerReference = default(NetworkObjectReference), NetworkObjectReference newPlayerReference = default(NetworkObjectReference))
	{
		NetworkObject networkObject;
		Player player = oldPlayerReference.TryGet(out networkObject, null) ? networkObject.GetComponent<Player>() : null;
		NetworkObject networkObject2;
		Player player2 = newPlayerReference.TryGet(out networkObject2, null) ? networkObject2.GetComponent<Player>() : null;
		if (player)
		{
			player.Stick = null;
		}
		if (player2)
		{
			this.Player = player2;
			this.Player.Stick = this;
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

	// Token: 0x06000315 RID: 789 RVA: 0x00013BE4 File Offset: 0x00011DE4
	private void OnPlayerReferenceChanged(NetworkObjectReference oldPlayerReference, NetworkObjectReference newPlayerReference)
	{
		this.HandlePlayerReference(oldPlayerReference, newPlayerReference);
		EventManager.TriggerEvent("Event_Everyone_OnStickPlayerReferenceChanged", new Dictionary<string, object>
		{
			{
				"stick",
				this
			},
			{
				"oldPlayerReference",
				oldPlayerReference
			},
			{
				"newPlayerReference",
				newPlayerReference
			}
		});
	}

	// Token: 0x06000316 RID: 790 RVA: 0x00013C38 File Offset: 0x00011E38
	private void Server_FixedUpdate()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.Player || this.SynchronizedObject.IsDrivenExternally)
		{
			return;
		}
		this.Server_UpdatePidControllers(Time.fixedDeltaTime);
		this.Server_ApplyForces();
		this.Server_ClearRollVelocity();
		this.Server_ResetRoll();
		this.Server_ApplyFeedbackForces();
		this.bladeHandleProportionalGainMultiplier = 1f;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00013C9C File Offset: 0x00011E9C
	private void Server_UpdatePidControllers(float deltaTime)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.StickPositioner)
		{
			return;
		}
		this.shaftHandlePIDController.proportionalGain = this.shaftHandleProportionalGain * this.shaftHandleProportionalGainMultiplier;
		this.shaftHandlePIDController.integralGain = this.shaftHandleIntegralGain;
		this.shaftHandlePIDController.integralSaturation = this.shaftHandleIntegralSaturation;
		this.shaftHandlePIDController.derivativeGain = this.shaftHandleDerivativeGain;
		this.shaftHandlePIDController.derivativeSmoothing = this.shaftHandleDerivativeSmoothing;
		this.bladeHandlePIDController.proportionalGain = this.bladeHandleProportionalGain * this.bladeHandleProportionalGainMultiplier;
		this.bladeHandlePIDController.integralGain = this.bladeHandleIntegralGain;
		this.bladeHandlePIDController.integralSaturation = this.bladeHandleIntegralSaturation;
		this.bladeHandlePIDController.derivativeGain = this.bladeHandleDerivativeGain;
		this.bladeHandlePIDController.derivativeSmoothing = this.bladeHandleDerivativeSmoothing;
		this.shaftHandleForce = this.shaftHandlePIDController.Update(deltaTime, this.ShaftHandlePosition, this.StickPositioner.ShaftTargetPosition);
		this.bladeHandleForce = this.bladeHandlePIDController.Update(deltaTime, this.BladeHandlePosition, this.StickPositioner.BladeTargetPosition);
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00013DC4 File Offset: 0x00011FC4
	private void Server_ApplyForces()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.PlayerBody)
		{
			return;
		}
		this.Rigidbody.AddForceAtPosition(this.PlayerBody.Rigidbody.GetPointVelocity(this.shaftHandle.transform.position) * this.linearVelocityTransferMultiplier * Time.fixedDeltaTime, this.shaftHandle.transform.position, ForceMode.VelocityChange);
		this.Rigidbody.AddForceAtPosition(this.PlayerBody.Rigidbody.GetPointVelocity(this.bladeHandle.transform.position) * this.linearVelocityTransferMultiplier * Time.fixedDeltaTime, this.bladeHandle.transform.position, ForceMode.VelocityChange);
		this.Rigidbody.AddForceAtPosition(this.shaftHandleForce * Time.fixedDeltaTime, this.ShaftHandlePosition, ForceMode.VelocityChange);
		this.Rigidbody.AddForceAtPosition(this.bladeHandleForce * Time.fixedDeltaTime, this.BladeHandlePosition, ForceMode.VelocityChange);
	}

	// Token: 0x06000319 RID: 793 RVA: 0x00013ED4 File Offset: 0x000120D4
	private void Server_ClearRollVelocity()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Vector3 direction = base.transform.InverseTransformVector(this.Rigidbody.angularVelocity);
		direction.z = 0f;
		this.Rigidbody.angularVelocity = base.transform.TransformDirection(direction);
	}

	// Token: 0x0600031A RID: 794 RVA: 0x00013F28 File Offset: 0x00012128
	private void Server_ResetRoll()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Vector3 vector = Utils.WrapEulerAngles(base.transform.eulerAngles);
		Quaternion rotation = Quaternion.Euler(new Vector3(vector.x, vector.y, 0f));
		this.Rigidbody.MoveRotation(rotation);
	}

	// Token: 0x0600031B RID: 795 RVA: 0x00013F7C File Offset: 0x0001217C
	private void Server_ApplyFeedbackForces()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.PlayerBody)
		{
			return;
		}
		Vector3 a = Vector3.Scale(this.Rigidbody.angularVelocity, new Vector3(0.5f, 1f, 0f)) * this.angularVelocityTransferMultiplier;
		if (this.transferAngularVelocity)
		{
			this.PlayerBody.Rigidbody.AddTorque(-a, ForceMode.Acceleration);
		}
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00013FF4 File Offset: 0x000121F4
	public void Server_Teleport(Vector3 position, Quaternion rotation)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.Rigidbody.position = position;
		this.Rigidbody.rotation = rotation;
		this.Rigidbody.linearVelocity = Vector3.zero;
		this.Rigidbody.angularVelocity = Vector3.zero;
		this.shaftHandlePIDController.Reset();
		this.bladeHandlePIDController.Reset();
	}

	// Token: 0x0600031D RID: 797 RVA: 0x0001405C File Offset: 0x0001225C
	public void Server_Freeze(RigidbodyConstraints contstraints = RigidbodyConstraints.FreezeAll)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.Rigidbody.constraints = contstraints;
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00014077 File Offset: 0x00012277
	public void Server_Unfreeze()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.Rigidbody.constraints = RigidbodyConstraints.None;
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00014094 File Offset: 0x00012294
	private void Server_OnCollisionStay(Collision collision)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Stick stick;
		if (!collision.gameObject.TryGetComponent<Stick>(out stick))
		{
			return;
		}
		if (collision.contactCount == 0)
		{
			return;
		}
		ContactPoint contact = collision.GetContact(0);
		Component thisCollider = contact.thisCollider;
		Collider otherCollider = contact.otherCollider;
		if (!thisCollider.CompareTag("Stick Blade") || !otherCollider.CompareTag("Stick Shaft"))
		{
			return;
		}
		Vector3 point = contact.point;
		float num = Mathf.Clamp(Vector3.Distance(stick.ShaftHandlePosition, point) / this.Length, this.minShaftHandleProportionalGainMultiplier, 1f);
		this.bladeHandleProportionalGainMultiplier = num;
	}

	// Token: 0x06000322 RID: 802 RVA: 0x0001422C File Offset: 0x0001242C
	protected override void __initializeVariables()
	{
		bool flag = this.PlayerReference == null;
		if (flag)
		{
			throw new Exception("Stick.PlayerReference cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.PlayerReference.Initialize(this);
		base.__nameNetworkVariable(this.PlayerReference, "PlayerReference");
		this.NetworkVariableFields.Add(this.PlayerReference);
		base.__initializeVariables();
	}

	// Token: 0x06000323 RID: 803 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x06000324 RID: 804 RVA: 0x0001428F File Offset: 0x0001248F
	protected internal override string __getTypeName()
	{
		return "Stick";
	}

	// Token: 0x04000238 RID: 568
	private static readonly global::Logger Logger = new global::Logger("Stick");

	// Token: 0x04000239 RID: 569
	[Header("Settings")]
	[SerializeField]
	private float bladeAngleStep = 12.5f;

	// Token: 0x0400023A RID: 570
	[Space(20f)]
	[SerializeField]
	private bool transferAngularVelocity = true;

	// Token: 0x0400023B RID: 571
	[SerializeField]
	private float angularVelocityTransferMultiplier = 0.25f;

	// Token: 0x0400023C RID: 572
	[Space(20f)]
	[SerializeField]
	private float shaftHandleProportionalGain = 500f;

	// Token: 0x0400023D RID: 573
	[SerializeField]
	private float shaftHandleIntegralGain;

	// Token: 0x0400023E RID: 574
	[SerializeField]
	private float shaftHandleIntegralSaturation;

	// Token: 0x0400023F RID: 575
	[SerializeField]
	private float shaftHandleDerivativeGain = 20f;

	// Token: 0x04000240 RID: 576
	[SerializeField]
	private float shaftHandleDerivativeSmoothing = 0.1f;

	// Token: 0x04000241 RID: 577
	[SerializeField]
	private float minShaftHandleProportionalGainMultiplier = 0.25f;

	// Token: 0x04000242 RID: 578
	[Space(20f)]
	[SerializeField]
	private float bladeHandleProportionalGain = 500f;

	// Token: 0x04000243 RID: 579
	[SerializeField]
	private float bladeHandleIntegralGain;

	// Token: 0x04000244 RID: 580
	[SerializeField]
	private float bladeHandleIntegralSaturation;

	// Token: 0x04000245 RID: 581
	[SerializeField]
	private float bladeHandleDerivativeGain = 20f;

	// Token: 0x04000246 RID: 582
	[SerializeField]
	private float bladeHandleDerivativeSmoothing = 0.1f;

	// Token: 0x04000247 RID: 583
	[Space(20f)]
	[SerializeField]
	private float linearVelocityTransferMultiplier = 0.25f;

	// Token: 0x04000248 RID: 584
	[Header("References")]
	[SerializeField]
	private GameObject shaftHandle;

	// Token: 0x04000249 RID: 585
	[SerializeField]
	private GameObject bladeHandle;

	// Token: 0x0400024A RID: 586
	[SerializeField]
	private GameObject rotationContainer;

	// Token: 0x0400024B RID: 587
	[SerializeField]
	private StickMesh stickMesh;

	// Token: 0x0400024C RID: 588
	[HideInInspector]
	public NetworkVariable<NetworkObjectReference> PlayerReference;

	// Token: 0x0400024D RID: 589
	[HideInInspector]
	public Player Player;

	// Token: 0x0400024E RID: 590
	[HideInInspector]
	public Rigidbody Rigidbody;

	// Token: 0x0400024F RID: 591
	[HideInInspector]
	public SynchronizedObject SynchronizedObject;

	// Token: 0x04000250 RID: 592
	[HideInInspector]
	public NetworkObjectCollisionRecorder NetworkObjectCollisionRecorder;

	// Token: 0x04000251 RID: 593
	[HideInInspector]
	public float Length;

	// Token: 0x04000252 RID: 594
	private bool isNetworkVariablesInitialized;

	// Token: 0x04000253 RID: 595
	private Vector3PIDController shaftHandlePIDController = new Vector3PIDController(0f, 0f, 0f);

	// Token: 0x04000254 RID: 596
	private Vector3PIDController bladeHandlePIDController = new Vector3PIDController(0f, 0f, 0f);

	// Token: 0x04000255 RID: 597
	private float shaftHandleProportionalGainMultiplier = 1f;

	// Token: 0x04000256 RID: 598
	private float bladeHandleProportionalGainMultiplier = 1f;

	// Token: 0x04000257 RID: 599
	private Vector3 shaftHandleForce = Vector3.zero;

	// Token: 0x04000258 RID: 600
	private Vector3 bladeHandleForce = Vector3.zero;
}
