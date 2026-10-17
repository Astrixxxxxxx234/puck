using System;
using UnityEngine;

// Token: 0x02000034 RID: 52
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerBody))]
[RequireComponent(typeof(Hover))]
public class Movement : MonoBehaviour
{
	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600010B RID: 267 RVA: 0x00006008 File Offset: 0x00004208
	[HideInInspector]
	public float Speed
	{
		get
		{
			return new Vector3(this.Rigidbody.linearVelocity.x, 0f, this.Rigidbody.linearVelocity.z).magnitude;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600010C RID: 268 RVA: 0x00006047 File Offset: 0x00004247
	[HideInInspector]
	public float NormalizedMaximumSpeed
	{
		get
		{
			return this.Speed / this.MaximumSpeed;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x0600010D RID: 269 RVA: 0x00006056 File Offset: 0x00004256
	[HideInInspector]
	public float NormalizedMinimumSpeed
	{
		get
		{
			return this.Speed / this.MinimumSpeed;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x0600010E RID: 270 RVA: 0x00006065 File Offset: 0x00004265
	[HideInInspector]
	public float TurnSpeed
	{
		get
		{
			return Math.Abs(base.transform.InverseTransformVector(this.Rigidbody.angularVelocity).y);
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x0600010F RID: 271 RVA: 0x00006087 File Offset: 0x00004287
	[HideInInspector]
	public float MaximumSpeed
	{
		get
		{
			return Mathf.Max(Mathf.Max(this.maxForwardsSpeed, this.maxForwardsSprintSpeed), Mathf.Max(this.maxBackwardsSpeed, this.maxBackwardsSprintSpeed));
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000110 RID: 272 RVA: 0x000060B0 File Offset: 0x000042B0
	[HideInInspector]
	public float MinimumSpeed
	{
		get
		{
			return Mathf.Min(Mathf.Min(this.maxForwardsSpeed, this.maxForwardsSprintSpeed), Mathf.Min(this.maxBackwardsSpeed, this.maxBackwardsSprintSpeed));
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000111 RID: 273 RVA: 0x000060D9 File Offset: 0x000042D9
	[HideInInspector]
	public bool IsMovingForwards
	{
		get
		{
			return this.MovementDirection.InverseTransformVector(this.Rigidbody.linearVelocity).z > 0f;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000112 RID: 274 RVA: 0x000060FD File Offset: 0x000042FD
	[HideInInspector]
	public bool IsMovingBackwards
	{
		get
		{
			return this.MovementDirection.InverseTransformVector(this.Rigidbody.linearVelocity).z < 0f;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000113 RID: 275 RVA: 0x00006121 File Offset: 0x00004321
	[HideInInspector]
	public bool IsTurningLeft
	{
		get
		{
			return base.transform.InverseTransformVector(this.Rigidbody.angularVelocity).y < 0f;
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000114 RID: 276 RVA: 0x00006145 File Offset: 0x00004345
	[HideInInspector]
	public bool IsTurningRight
	{
		get
		{
			return base.transform.InverseTransformVector(this.Rigidbody.angularVelocity).y > 0f;
		}
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00006169 File Offset: 0x00004369
	private void Awake()
	{
		this.Rigidbody = base.GetComponent<Rigidbody>();
		this.SynchronizedObject = base.GetComponent<SynchronizedObject>();
		this.PlayerBody = base.GetComponent<PlayerBody>();
		this.Hover = base.GetComponent<Hover>();
	}

	// Token: 0x06000116 RID: 278 RVA: 0x0000619B File Offset: 0x0000439B
	private void Start()
	{
		this.currentMaxSpeed = this.maxForwardsSpeed;
		this.currentAcceleration = this.forwardsAcceleration;
	}

	// Token: 0x06000117 RID: 279 RVA: 0x000061B5 File Offset: 0x000043B5
	private void FixedUpdate()
	{
		if (this.SynchronizedObject.IsDrivenExternally)
		{
			return;
		}
		this.Move();
		this.Turn();
	}

	// Token: 0x06000118 RID: 280 RVA: 0x000061D4 File Offset: 0x000043D4
	private void Move()
	{
		if (!this.Hover.IsGrounded)
		{
			return;
		}
		if (this.IsMovingForwards)
		{
			if (this.Sprint)
			{
				this.currentMaxSpeed = this.maxForwardsSprintSpeed;
				this.currentAcceleration = ((this.Speed < this.maxForwardsSpeed) ? this.forwardsSprintAcceleration : this.forwardsSprintOverspeedAcceleration);
			}
			else
			{
				this.currentMaxSpeed = this.maxForwardsSpeed;
				this.currentAcceleration = this.forwardsAcceleration;
			}
		}
		else if (this.IsMovingBackwards)
		{
			if (this.Sprint)
			{
				this.currentMaxSpeed = this.maxBackwardsSprintSpeed;
				this.currentAcceleration = ((this.Speed < this.maxForwardsSpeed) ? this.backwardsSprintAcceleration : this.backwardsSprintOverspeedAcceleration);
			}
			else
			{
				this.currentMaxSpeed = this.maxBackwardsSpeed;
				this.currentAcceleration = this.backwardsAcceleration;
			}
		}
		if (this.MoveForwards)
		{
			if (this.IsMovingForwards)
			{
				float d = (this.Speed < this.currentMaxSpeed) ? this.currentAcceleration : 0f;
				this.Rigidbody.AddForce(this.MovementDirection.forward * d, ForceMode.Acceleration);
			}
			else if (this.IsMovingBackwards)
			{
				float d2 = this.brakeAcceleration;
				this.Rigidbody.AddForce(this.MovementDirection.forward * d2, ForceMode.Acceleration);
			}
		}
		else if (this.MoveBackwards)
		{
			if (this.IsMovingBackwards)
			{
				float d3 = (this.Speed < this.currentMaxSpeed) ? this.currentAcceleration : 0f;
				this.Rigidbody.AddForce(-this.MovementDirection.forward * d3, ForceMode.Acceleration);
			}
			else if (this.IsMovingForwards)
			{
				float d4 = this.brakeAcceleration;
				this.Rigidbody.AddForce(-this.MovementDirection.forward * d4, ForceMode.Acceleration);
			}
		}
		if (this.Speed > this.MaximumSpeed)
		{
			this.Rigidbody.linearVelocity *= 1f - this.overspeedDrag * Time.fixedDeltaTime;
		}
		else
		{
			this.Rigidbody.linearVelocity *= 1f - this.drag * Time.fixedDeltaTime;
		}
		this.Rigidbody.linearVelocity *= 1f - this.AmbientDrag * Time.fixedDeltaTime;
	}

	// Token: 0x06000119 RID: 281 RVA: 0x0000642C File Offset: 0x0000462C
	private void Turn()
	{
		if (this.TurnLeft)
		{
			if (this.IsTurningLeft)
			{
				float num = (this.TurnSpeed < this.turnMaxSpeed * this.TurnMultiplier) ? this.turnAcceleration : 0f;
				this.Rigidbody.AddTorque(base.transform.up * -num * this.TurnMultiplier, ForceMode.Acceleration);
			}
			else if (this.IsTurningRight)
			{
				float num2 = this.turnBrakeAcceleration;
				this.Rigidbody.AddTorque(base.transform.up * -num2 * this.TurnMultiplier, ForceMode.Acceleration);
			}
		}
		else if (this.TurnRight)
		{
			if (this.IsTurningRight)
			{
				float d = (this.TurnSpeed < this.turnMaxSpeed * this.TurnMultiplier) ? this.turnAcceleration : 0f;
				this.Rigidbody.AddTorque(base.transform.up * d * this.TurnMultiplier, ForceMode.Acceleration);
			}
			else if (this.IsTurningLeft)
			{
				float d2 = this.turnBrakeAcceleration;
				this.Rigidbody.AddTorque(base.transform.up * d2 * this.TurnMultiplier, ForceMode.Acceleration);
			}
		}
		else if (this.TurnSpeed < this.turnMaxSpeed * this.TurnMultiplier)
		{
			this.Rigidbody.angularVelocity *= 1f - this.turnDrag * Time.fixedDeltaTime;
		}
		if (this.TurnSpeed > this.turnMaxSpeed * this.TurnMultiplier)
		{
			this.Rigidbody.angularVelocity *= 1f - this.turnOverspeedDrag * Time.fixedDeltaTime;
		}
	}

	// Token: 0x040000B1 RID: 177
	[Header("Settings")]
	[Tooltip("Acceleration (m/s²) applied while skating forwards without sprinting, until maxForwardsSpeed is reached.")]
	[SerializeField]
	private float forwardsAcceleration = 2f;

	// Token: 0x040000B2 RID: 178
	[Tooltip("Acceleration (m/s²) applied while sprinting forwards below maxForwardsSpeed.")]
	[SerializeField]
	private float forwardsSprintAcceleration = 4.75f;

	// Token: 0x040000B3 RID: 179
	[Tooltip("Acceleration (m/s²) applied while sprinting forwards once already above maxForwardsSpeed — controls how quickly the sprint band between maxForwardsSpeed and maxForwardsSprintSpeed fills up.")]
	[SerializeField]
	private float forwardsSprintOverspeedAcceleration = 1f;

	// Token: 0x040000B4 RID: 180
	[Tooltip("Acceleration (m/s²) applied while skating backwards without sprinting, until maxBackwardsSpeed is reached.")]
	[SerializeField]
	private float backwardsAcceleration = 1.8f;

	// Token: 0x040000B5 RID: 181
	[Tooltip("Acceleration (m/s²) applied while sprinting backwards below maxForwardsSpeed.")]
	[SerializeField]
	private float backwardsSprintAcceleration = 2f;

	// Token: 0x040000B6 RID: 182
	[Tooltip("Acceleration (m/s²) applied while sprinting backwards once already above maxForwardsSpeed.")]
	[SerializeField]
	private float backwardsSprintOverspeedAcceleration = 1f;

	// Token: 0x040000B7 RID: 183
	[Tooltip("Deceleration (m/s²) applied when movement input opposes the current direction of travel (e.g. holding back while still gliding forwards). Higher = sharper stops.")]
	[SerializeField]
	private float brakeAcceleration = 5f;

	// Token: 0x040000B8 RID: 184
	[Tooltip("Baseline velocity damping applied every physics tick (velocity *= 1 - drag * dt) — the 'ice friction' that makes a coasting player slowly glide to a stop.")]
	[SerializeField]
	private float drag = 0.025f;

	// Token: 0x040000B9 RID: 185
	[Tooltip("Velocity damping applied instead of drag while above MaximumSpeed (e.g. after an external boost or collision) — how quickly excess speed bleeds off.")]
	[SerializeField]
	private float overspeedDrag = 0.025f;

	// Token: 0x040000BA RID: 186
	[Space(20f)]
	[Tooltip("Speed (m/s) above which forwards acceleration cuts off when not sprinting.")]
	[SerializeField]
	private float maxForwardsSpeed = 7.5f;

	// Token: 0x040000BB RID: 187
	[Tooltip("Speed (m/s) above which forwards acceleration cuts off while sprinting.")]
	[SerializeField]
	private float maxForwardsSprintSpeed = 8.75f;

	// Token: 0x040000BC RID: 188
	[Tooltip("Speed (m/s) above which backwards acceleration cuts off when not sprinting.")]
	[SerializeField]
	private float maxBackwardsSpeed = 7.25f;

	// Token: 0x040000BD RID: 189
	[Tooltip("Speed (m/s) above which backwards acceleration cuts off while sprinting.")]
	[SerializeField]
	private float maxBackwardsSprintSpeed = 7.25f;

	// Token: 0x040000BE RID: 190
	[Space(20f)]
	[Tooltip("Angular acceleration (rad/s²) applied while turning, until the turn speed cap is reached.")]
	[SerializeField]
	private float turnAcceleration = 1.625f;

	// Token: 0x040000BF RID: 191
	[Tooltip("Angular deceleration (rad/s²) applied when turn input opposes the current rotation direction — how sharply a turn can be reversed.")]
	[SerializeField]
	private float turnBrakeAcceleration = 3.25f;

	// Token: 0x040000C0 RID: 192
	[Tooltip("Maximum turn rate (rad/s) around the player's up axis. Scaled at runtime by TurnMultiplier.")]
	[SerializeField]
	private float turnMaxSpeed = 1.375f;

	// Token: 0x040000C1 RID: 193
	[Tooltip("Angular velocity damping applied every physics tick while no turn input is held — how quickly rotation settles once the keys are released.")]
	[SerializeField]
	private float turnDrag = 3f;

	// Token: 0x040000C2 RID: 194
	[Tooltip("Angular velocity damping applied while spinning faster than the turn speed cap — bleeds off excess rotation.")]
	[SerializeField]
	private float turnOverspeedDrag = 2.25f;

	// Token: 0x040000C3 RID: 195
	[HideInInspector]
	public Rigidbody Rigidbody;

	// Token: 0x040000C4 RID: 196
	[HideInInspector]
	public SynchronizedObject SynchronizedObject;

	// Token: 0x040000C5 RID: 197
	[HideInInspector]
	public PlayerBody PlayerBody;

	// Token: 0x040000C6 RID: 198
	[HideInInspector]
	public Hover Hover;

	// Token: 0x040000C7 RID: 199
	[HideInInspector]
	public bool MoveForwards;

	// Token: 0x040000C8 RID: 200
	[HideInInspector]
	public bool MoveBackwards;

	// Token: 0x040000C9 RID: 201
	[HideInInspector]
	public bool TurnLeft;

	// Token: 0x040000CA RID: 202
	[HideInInspector]
	public bool TurnRight;

	// Token: 0x040000CB RID: 203
	[HideInInspector]
	public float TurnMultiplier;

	// Token: 0x040000CC RID: 204
	[HideInInspector]
	public bool Sprint;

	// Token: 0x040000CD RID: 205
	[HideInInspector]
	public float AmbientDrag;

	// Token: 0x040000CE RID: 206
	[HideInInspector]
	public Transform MovementDirection;

	// Token: 0x040000CF RID: 207
	private float currentMaxSpeed;

	// Token: 0x040000D0 RID: 208
	private float currentAcceleration;
}
