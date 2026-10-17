using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class Puck : NetworkBehaviour
{
	// Token: 0x1700005F RID: 95
	// (get) Token: 0x0600037C RID: 892 RVA: 0x00015957 File Offset: 0x00013B57
	[HideInInspector]
	public float PredictedSpeed
	{
		get
		{
			return this.SynchronizedObject.PredictedLinearVelocity.magnitude;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x0600037D RID: 893 RVA: 0x00015969 File Offset: 0x00013B69
	[HideInInspector]
	public float PredictedAngularSpeed
	{
		get
		{
			return this.SynchronizedObject.PredictedAngularVelocity.magnitude;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x0600037E RID: 894 RVA: 0x0001597B File Offset: 0x00013B7B
	// (set) Token: 0x0600037F RID: 895 RVA: 0x00015983 File Offset: 0x00013B83
	[HideInInspector]
	public float ShotSpeed { get; private set; }

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000380 RID: 896 RVA: 0x0001598C File Offset: 0x00013B8C
	// (set) Token: 0x06000381 RID: 897 RVA: 0x00015994 File Offset: 0x00013B94
	[HideInInspector]
	public bool IsGrounded { get; private set; }

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000382 RID: 898 RVA: 0x0001599D File Offset: 0x00013B9D
	[HideInInspector]
	public SphereCollider NetSphereCollider
	{
		get
		{
			return this.netSphereCollider;
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06000383 RID: 899 RVA: 0x000159A5 File Offset: 0x00013BA5
	[HideInInspector]
	public Collider StickCollider
	{
		get
		{
			return this.stickCollider;
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06000384 RID: 900 RVA: 0x000159AD File Offset: 0x00013BAD
	[HideInInspector]
	public Collider IceCollider
	{
		get
		{
			return this.iceCollider;
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x06000385 RID: 901 RVA: 0x000159B5 File Offset: 0x00013BB5
	// (set) Token: 0x06000386 RID: 902 RVA: 0x000159BD File Offset: 0x00013BBD
	[HideInInspector]
	public Stick TouchingStick { get; private set; }

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x06000387 RID: 903 RVA: 0x000159C6 File Offset: 0x00013BC6
	[HideInInspector]
	public bool IsTouchingStick
	{
		get
		{
			return this.TouchingStick != null;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000388 RID: 904 RVA: 0x000159D4 File Offset: 0x00013BD4
	[HideInInspector]
	public float MaxSpeed
	{
		get
		{
			return this.maxSpeed;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000389 RID: 905 RVA: 0x000159DC File Offset: 0x00013BDC
	[HideInInspector]
	public float MaxAngularSpeed
	{
		get
		{
			return this.maxAngularSpeed;
		}
	}

	// Token: 0x0600038A RID: 906 RVA: 0x000159E4 File Offset: 0x00013BE4
	private void Awake()
	{
		this.Rigidbody = base.GetComponent<Rigidbody>();
		this.SynchronizedObject = base.GetComponent<SynchronizedObject>();
		this.NetworkObjectCollisionRecorder = base.GetComponent<NetworkObjectCollisionRecorder>();
		this.ReplayTouchTracker = base.GetComponent<ReplayTouchTracker>();
		this.CollisionRecorder = base.GetComponent<CollisionRecorder>();
		CollisionRecorder collisionRecorder = this.CollisionRecorder;
		collisionRecorder.CollisionDeferred = (Action<GameObject, float>)Delegate.Combine(collisionRecorder.CollisionDeferred, new Action<GameObject, float>(this.OnCollisionDeferred));
		this.NetSphereCollider.enabled = false;
	}

	// Token: 0x0600038B RID: 907 RVA: 0x00015A60 File Offset: 0x00013C60
	private void FixedUpdate()
	{
		this.Speed = this.Rigidbody.linearVelocity.magnitude;
		this.AngularSpeed = this.Rigidbody.angularVelocity.magnitude;
		this.IsGrounded = Physics.CheckSphere(base.transform.position, this.groundedCheckSphereRadius, this.groundedCheckSphereLayerMask);
		if (this.IsGrounded)
		{
			this.Rigidbody.centerOfMass = base.transform.TransformVector(this.groundedCenterOfMass);
		}
		else
		{
			this.Rigidbody.centerOfMass = Vector3.zero;
		}
		float num = this.IsGrounded ? 0f : Mathf.Clamp(this.PredictedSpeed * 0.025f, 0.15f, 0.75f);
		if (this.NetSphereCollider.radius < num)
		{
			this.NetSphereCollider.radius = num;
		}
		else if (this.NetSphereCollider.radius > num)
		{
			this.NetSphereCollider.radius = Mathf.Lerp(this.NetSphereCollider.radius, num, Time.fixedDeltaTime * 5f);
		}
		if (this.IsTouchingStick)
		{
			this.Server_UpdateStickTensor(this.stickTensor, Quaternion.identity);
			this.TouchingStick = null;
		}
		else
		{
			this.Server_UpdateStickTensor(this.defaultTensor, Quaternion.identity);
		}
		this.Server_UpdateAudio();
	}

	// Token: 0x0600038C RID: 908 RVA: 0x00015BB1 File Offset: 0x00013DB1
	protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
	{
		this.InitializeNetworkVariables(false);
		base.OnNetworkPreSpawn(ref networkManager);
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00015BC1 File Offset: 0x00013DC1
	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00015BC9 File Offset: 0x00013DC9
	protected override void OnNetworkPostSpawn()
	{
		EventManager.TriggerEvent("Event_Everyone_OnPuckSpawned", new Dictionary<string, object>
		{
			{
				"puck",
				this
			}
		});
		base.OnNetworkPostSpawn();
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00015BEC File Offset: 0x00013DEC
	public override void OnNetworkDespawn()
	{
		EventManager.TriggerEvent("Event_Everyone_OnPuckDespawned", new Dictionary<string, object>
		{
			{
				"puck",
				this
			}
		});
		base.OnNetworkDespawn();
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00015C0F File Offset: 0x00013E0F
	public override void OnDestroy()
	{
		CollisionRecorder collisionRecorder = this.CollisionRecorder;
		collisionRecorder.CollisionDeferred = (Action<GameObject, float>)Delegate.Remove(collisionRecorder.CollisionDeferred, new Action<GameObject, float>(this.OnCollisionDeferred));
		base.transform.DOKill(false);
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00015C45 File Offset: 0x00013E45
	public void InitializeNetworkVariables(bool isReplay = false)
	{
		if (this.isNetworkVariablesInitialized)
		{
			return;
		}
		this.isNetworkVariablesInitialized = true;
		this.IsReplay = new NetworkVariable<bool>(isReplay, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
	}

	// Token: 0x06000392 RID: 914 RVA: 0x00015C65 File Offset: 0x00013E65
	public void Server_Freeze(RigidbodyConstraints contstraints = RigidbodyConstraints.FreezeAll)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.Rigidbody.constraints = contstraints;
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00015C80 File Offset: 0x00013E80
	public void Server_Unfreeze()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.Rigidbody.constraints = RigidbodyConstraints.None;
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00015C9C File Offset: 0x00013E9C
	public List<KeyValuePair<Player, float>> GetPlayerCollisions()
	{
		List<KeyValuePair<Player, float>> list = this.NetworkObjectCollisionRecorder.NetworkObjectCollisions.Select(delegate(NetworkObjectCollision collision)
		{
			NetworkObject networkObject;
			if (collision.NetworkObjectReference.TryGet(out networkObject, null))
			{
				PlayerBody playerBody;
				networkObject.TryGetComponent<PlayerBody>(out playerBody);
				Stick stick;
				networkObject.TryGetComponent<Stick>(out stick);
				if (playerBody)
				{
					return new KeyValuePair<Player, float>(playerBody.Player, collision.Time);
				}
				if (stick)
				{
					return new KeyValuePair<Player, float>(stick.Player, collision.Time);
				}
			}
			return new KeyValuePair<Player, float>(null, collision.Time);
		}).ToList<KeyValuePair<Player, float>>();
		list.RemoveAll((KeyValuePair<Player, float> collision) => collision.Key == null);
		return list;
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00015D04 File Offset: 0x00013F04
	public List<KeyValuePair<Player, float>> GetPlayerCollisionsByTeam(PlayerTeam team)
	{
		return this.GetPlayerCollisions().Where(delegate(KeyValuePair<Player, float> collision)
		{
			Player key = collision.Key;
			return key != null && key.Team == team;
		}).ToList<KeyValuePair<Player, float>>();
	}

	// Token: 0x06000396 RID: 918 RVA: 0x00015D3C File Offset: 0x00013F3C
	public bool IsActivelyTouchedBy(Player player)
	{
		return player && ((player.Stick && this.ReplayTouchTracker.IsActivelyTouching(player.Stick.NetworkObject)) || (player.PlayerBody && this.ReplayTouchTracker.IsActivelyTouching(player.PlayerBody.NetworkObject)));
	}

	// Token: 0x06000397 RID: 919 RVA: 0x00015DA2 File Offset: 0x00013FA2
	private void Server_UpdateStickTensor(Vector3 inertiaTensor, Quaternion inertiaTensorRotation)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.Rigidbody.inertiaTensor = inertiaTensor;
		this.Rigidbody.inertiaTensorRotation = inertiaTensorRotation;
	}

	// Token: 0x06000398 RID: 920 RVA: 0x00015DCC File Offset: 0x00013FCC
	private void Server_UpdateAudio()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (this.Rigidbody.constraints != RigidbodyConstraints.None)
		{
			this.windAudioSource.Server_SetVolume(0f);
			return;
		}
		float time = Mathf.Min(this.Speed / this.MaxSpeed, 1f);
		this.windAudioSource.Server_SetVolume(this.windVolumeCurve.Evaluate(time));
		float time2 = Mathf.Min(this.Speed / this.MaxSpeed, 1f);
		this.windAudioSource.Server_SetPitch(this.windPitchCurve.Evaluate(time2));
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00015E64 File Offset: 0x00014064
	private void OnCollisionDeferred(GameObject gameObject, float force)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!gameObject)
		{
			return;
		}
		string a = LayerMask.LayerToName(gameObject.layer);
		if (a == "Goal Post")
		{
			this.hitGoalPostAudioSource.Server_Play(this.hitGoalPostVolumeCurve.Evaluate(force), this.hitGoalPostPitchCurve.Evaluate(force), true, -1, 0f, true, false, false, 0f, false, 0f, -1f);
			return;
		}
		if (!(a == "Barrier"))
		{
			this.hitIceAudioSource.Server_Play(this.hitIceVolumeCurve.Evaluate(force), this.hitIcePitchCurve.Evaluate(force), true, -1, 0f, true, false, false, 0f, false, 0f, -1f);
			return;
		}
		this.hitBoardsAudioSource.Server_Play(this.hitBoardsVolumeCurve.Evaluate(force), this.hitBoardsPitchCurve.Evaluate(force), true, -1, 0f, true, false, false, 0f, false, 0f, -1f);
	}

	// Token: 0x0600039A RID: 922 RVA: 0x00015F68 File Offset: 0x00014168
	private void OnCollisionEnter(Collision collision)
	{
		Stick touchingStick;
		if (collision.gameObject.TryGetComponent<Stick>(out touchingStick))
		{
			this.TouchingStick = touchingStick;
			this.ShotSpeed = 0f;
		}
		if (this.SynchronizedObject.IsDrivenExternally)
		{
			return;
		}
		if (this.IsGrounded)
		{
			return;
		}
		string a = LayerMask.LayerToName(collision.gameObject.layer);
		Vector3 a2 = Vector3.zero;
		int contactCount = collision.contactCount;
		if (contactCount == 0)
		{
			return;
		}
		for (int i = 0; i < contactCount; i++)
		{
			a2 += collision.GetContact(i).normal;
		}
		a2 /= (float)contactCount;
		float t = Mathf.Abs(Vector3.Dot(collision.relativeVelocity.normalized, a2.normalized));
		if (!(a == "Goal Net"))
		{
			a == "Goal Post";
			return;
		}
		if (this.Rigidbody.linearVelocity.magnitude > this.goalNetLinearVelocityMaximumMagnitude)
		{
			Vector3 b = this.Rigidbody.linearVelocity.normalized * this.goalNetLinearVelocityMaximumMagnitude;
			b.y = 0f;
			this.Rigidbody.linearVelocity = Vector3.Lerp(this.Rigidbody.linearVelocity, b, t);
		}
		if (this.Rigidbody.angularVelocity.magnitude > this.goalNetAngularVelocityMaximumMagnitude)
		{
			Vector3 b2 = this.Rigidbody.angularVelocity.normalized * this.goalNetAngularVelocityMaximumMagnitude;
			this.Rigidbody.angularVelocity = Vector3.Lerp(this.Rigidbody.angularVelocity, b2, t);
		}
	}

	// Token: 0x0600039B RID: 923 RVA: 0x00016100 File Offset: 0x00014300
	private void OnCollisionStay(Collision collision)
	{
		Stick touchingStick;
		if (!collision.gameObject.TryGetComponent<Stick>(out touchingStick))
		{
			return;
		}
		this.TouchingStick = touchingStick;
	}

	// Token: 0x0600039C RID: 924 RVA: 0x00016124 File Offset: 0x00014324
	private void OnCollisionExit(Collision collision)
	{
		Stick stick;
		if (!collision.gameObject.TryGetComponent<Stick>(out stick))
		{
			return;
		}
		this.ShotSpeed = this.Speed;
		if (this.SynchronizedObject.IsDrivenExternally)
		{
			return;
		}
		this.Rigidbody.linearVelocity = Vector3.ClampMagnitude(this.Rigidbody.linearVelocity, this.MaxSpeed);
		this.Rigidbody.angularVelocity = Vector3.ClampMagnitude(this.Rigidbody.angularVelocity, this.MaxAngularSpeed);
		Vector3 force = new Vector3(0f, Mathf.Min(0f, -this.Rigidbody.linearVelocity.y), 0f) * 5f;
		this.Rigidbody.AddForce(force, ForceMode.Acceleration);
	}

	// Token: 0x0600039D RID: 925 RVA: 0x000161DF File Offset: 0x000143DF
	public void OnDrawGizmos()
	{
		if (!Application.isEditor)
		{
			return;
		}
		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere(base.transform.position, this.groundedCheckSphereRadius);
	}

	// Token: 0x0600039F RID: 927 RVA: 0x000162A4 File Offset: 0x000144A4
	protected override void __initializeVariables()
	{
		bool flag = this.IsReplay == null;
		if (flag)
		{
			throw new Exception("Puck.IsReplay cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.IsReplay.Initialize(this);
		base.__nameNetworkVariable(this.IsReplay, "IsReplay");
		this.NetworkVariableFields.Add(this.IsReplay);
		base.__initializeVariables();
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x00016307 File Offset: 0x00014507
	protected internal override string __getTypeName()
	{
		return "Puck";
	}

	// Token: 0x04000298 RID: 664
	[Header("Settings")]
	[SerializeField]
	private float maxSpeed = 30f;

	// Token: 0x04000299 RID: 665
	[SerializeField]
	private float maxAngularSpeed = 30f;

	// Token: 0x0400029A RID: 666
	[Space(20f)]
	[SerializeField]
	private Vector3 stickTensor = new Vector3(0.006f, 0.002f, 0.006f);

	// Token: 0x0400029B RID: 667
	[SerializeField]
	private Vector3 defaultTensor = new Vector3(0.002f, 0.002f, 0.002f);

	// Token: 0x0400029C RID: 668
	[Space(20f)]
	[SerializeField]
	private float groundedCheckSphereRadius = 0.075f;

	// Token: 0x0400029D RID: 669
	[SerializeField]
	private LayerMask groundedCheckSphereLayerMask;

	// Token: 0x0400029E RID: 670
	[Space(20f)]
	[SerializeField]
	private Vector3 groundedCenterOfMass = new Vector3(0f, -0.01f, 0f);

	// Token: 0x0400029F RID: 671
	[Space(20f)]
	[SerializeField]
	private float goalNetLinearVelocityMaximumMagnitude = 2f;

	// Token: 0x040002A0 RID: 672
	[SerializeField]
	private float goalNetAngularVelocityMaximumMagnitude = 2f;

	// Token: 0x040002A1 RID: 673
	[Space(20f)]
	[SerializeField]
	private AnimationCurve hitIceVolumeCurve;

	// Token: 0x040002A2 RID: 674
	[SerializeField]
	private AnimationCurve hitIcePitchCurve;

	// Token: 0x040002A3 RID: 675
	[SerializeField]
	private AnimationCurve hitBoardsVolumeCurve;

	// Token: 0x040002A4 RID: 676
	[SerializeField]
	private AnimationCurve hitBoardsPitchCurve;

	// Token: 0x040002A5 RID: 677
	[SerializeField]
	private AnimationCurve hitGoalPostVolumeCurve;

	// Token: 0x040002A6 RID: 678
	[SerializeField]
	private AnimationCurve hitGoalPostPitchCurve;

	// Token: 0x040002A7 RID: 679
	[SerializeField]
	private AnimationCurve windVolumeCurve;

	// Token: 0x040002A8 RID: 680
	[SerializeField]
	private AnimationCurve windPitchCurve;

	// Token: 0x040002A9 RID: 681
	[Header("References")]
	[SerializeField]
	private PuckElevationIndicator verticalityIndicator;

	// Token: 0x040002AA RID: 682
	[SerializeField]
	private SphereCollider netSphereCollider;

	// Token: 0x040002AB RID: 683
	[SerializeField]
	private Collider stickCollider;

	// Token: 0x040002AC RID: 684
	[SerializeField]
	private Collider iceCollider;

	// Token: 0x040002AD RID: 685
	[Space(20f)]
	[SerializeField]
	private SynchronizedAudio hitIceAudioSource;

	// Token: 0x040002AE RID: 686
	[SerializeField]
	private SynchronizedAudio hitBoardsAudioSource;

	// Token: 0x040002AF RID: 687
	[SerializeField]
	private SynchronizedAudio hitGoalPostAudioSource;

	// Token: 0x040002B0 RID: 688
	[SerializeField]
	private SynchronizedAudio windAudioSource;

	// Token: 0x040002B1 RID: 689
	[HideInInspector]
	public NetworkVariable<bool> IsReplay;

	// Token: 0x040002B2 RID: 690
	[HideInInspector]
	public Rigidbody Rigidbody;

	// Token: 0x040002B3 RID: 691
	[HideInInspector]
	public SynchronizedObject SynchronizedObject;

	// Token: 0x040002B4 RID: 692
	[HideInInspector]
	public NetworkObjectCollisionRecorder NetworkObjectCollisionRecorder;

	// Token: 0x040002B5 RID: 693
	[HideInInspector]
	public ReplayTouchTracker ReplayTouchTracker;

	// Token: 0x040002B6 RID: 694
	[HideInInspector]
	public CollisionRecorder CollisionRecorder;

	// Token: 0x040002B7 RID: 695
	[HideInInspector]
	public float Speed;

	// Token: 0x040002B8 RID: 696
	[HideInInspector]
	public float AngularSpeed;

	// Token: 0x040002BC RID: 700
	private bool isNetworkVariablesInitialized;
}
