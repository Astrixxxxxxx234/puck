using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
[RequireComponent(typeof(Rigidbody))]
public class Skate : MonoBehaviour
{
	// Token: 0x060001C5 RID: 453 RVA: 0x0000A19D File Offset: 0x0000839D
	private void Awake()
	{
		this.Rigidbody = base.GetComponent<Rigidbody>();
		this.SynchronizedObject = base.GetComponent<SynchronizedObject>();
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0000A1B8 File Offset: 0x000083B8
	private void FixedUpdate()
	{
		Vector3 vector = this.MovementDirection.InverseTransformVector(this.Rigidbody.linearVelocity);
		vector.y = 0f;
		vector.z = 0f;
		float num = -vector.x;
		this.IsTractionLost = (num > this.traction * Time.fixedDeltaTime);
		num = Mathf.Clamp(num, -this.traction * Time.fixedDeltaTime, this.traction * Time.fixedDeltaTime);
		if (this.SynchronizedObject.IsDrivenExternally)
		{
			return;
		}
		this.Rigidbody.AddForce(this.MovementDirection.right * num * this.Intensity, ForceMode.VelocityChange);
	}

	// Token: 0x04000182 RID: 386
	[Header("Settings")]
	[SerializeField]
	private float traction = 0.15f;

	// Token: 0x04000183 RID: 387
	[HideInInspector]
	public Rigidbody Rigidbody;

	// Token: 0x04000184 RID: 388
	[HideInInspector]
	public SynchronizedObject SynchronizedObject;

	// Token: 0x04000185 RID: 389
	[HideInInspector]
	public float Intensity = 1f;

	// Token: 0x04000186 RID: 390
	[HideInInspector]
	public bool IsTractionLost;

	// Token: 0x04000187 RID: 391
	[HideInInspector]
	public Transform MovementDirection;
}
