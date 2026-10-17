using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
[RequireComponent(typeof(Rigidbody))]
public class Hover : MonoBehaviour
{
	// Token: 0x06000105 RID: 261 RVA: 0x00005D4F File Offset: 0x00003F4F
	private void Awake()
	{
		this.Rigidbody = base.GetComponent<Rigidbody>();
		this.SynchronizedObject = base.GetComponent<SynchronizedObject>();
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00005D6C File Offset: 0x00003F6C
	private void FixedUpdate()
	{
		this.pidController.proportionalGain = this.proportionalGain;
		this.pidController.integralGain = this.integralGain;
		this.pidController.derivativeGain = this.derivativeGain;
		Vector3 vector = base.transform.position + this.raycastOffset;
		Vector3 down = Vector3.down;
		Debug.DrawRay(vector, down * this.raycastDistance, Color.black);
		RaycastHit raycastHit;
		if (Physics.Raycast(vector, down, out raycastHit, this.raycastDistance, this.raycastLayerMask))
		{
			this.currentDistance = raycastHit.distance;
		}
		else
		{
			this.currentDistance = this.raycastDistance;
		}
		this.IsGrounded = (this.currentDistance < this.raycastDistance);
		float num = this.pidController.Update(Time.fixedDeltaTime, this.currentDistance, this.TargetDistance);
		num = Mathf.Clamp(num, 0f, this.maxForce);
		if (this.SynchronizedObject.IsDrivenExternally)
		{
			return;
		}
		this.Rigidbody.AddForce(Vector3.up * num, ForceMode.Acceleration);
	}

	// Token: 0x0400009D RID: 157
	[Header("Settings")]
	[SerializeField]
	private float maxForce = 40f;

	// Token: 0x0400009E RID: 158
	[SerializeField]
	private Vector3 raycastOffset = new Vector3(0f, 1f, 0f);

	// Token: 0x0400009F RID: 159
	[SerializeField]
	private float raycastDistance = 1.45f;

	// Token: 0x040000A0 RID: 160
	[SerializeField]
	private LayerMask raycastLayerMask;

	// Token: 0x040000A1 RID: 161
	[Space(20f)]
	[SerializeField]
	private float proportionalGain = 100f;

	// Token: 0x040000A2 RID: 162
	[SerializeField]
	private float integralGain;

	// Token: 0x040000A3 RID: 163
	[SerializeField]
	private float derivativeGain = 15f;

	// Token: 0x040000A4 RID: 164
	[Space(20f)]
	public float TargetDistance = 1f;

	// Token: 0x040000A5 RID: 165
	[HideInInspector]
	public bool IsGrounded;

	// Token: 0x040000A6 RID: 166
	[HideInInspector]
	public Rigidbody Rigidbody;

	// Token: 0x040000A7 RID: 167
	[HideInInspector]
	public SynchronizedObject SynchronizedObject;

	// Token: 0x040000A8 RID: 168
	private PIDController pidController = new PIDController(0f, 0f, 0f);

	// Token: 0x040000A9 RID: 169
	private float currentDistance;
}
