using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
[RequireComponent(typeof(Rigidbody))]
public class VelocityLean : MonoBehaviour
{
	// Token: 0x060001C8 RID: 456 RVA: 0x0000A285 File Offset: 0x00008485
	private void Awake()
	{
		this.Rigidbody = base.GetComponent<Rigidbody>();
		this.SynchronizedObject = base.GetComponent<SynchronizedObject>();
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x0000A2A0 File Offset: 0x000084A0
	private void FixedUpdate()
	{
		float d = this.UseWorldLinearVelocity ? this.Rigidbody.linearVelocity.magnitude : this.MovementDirection.InverseTransformVector(this.Rigidbody.linearVelocity).z;
		float y = this.MovementDirection.InverseTransformVector(this.Rigidbody.angularVelocity).y;
		if (this.SynchronizedObject.IsDrivenExternally)
		{
			return;
		}
		this.Rigidbody.AddTorque(d * (this.Inverted ? (-base.transform.right) : base.transform.right) * this.linearForceMultiplier * this.LinearIntensity, ForceMode.Acceleration);
		this.Rigidbody.AddTorque(-y * (this.Inverted ? (-base.transform.forward) : base.transform.forward) * this.angularForceMultiplier * this.AngularIntensity, ForceMode.Acceleration);
	}

	// Token: 0x04000188 RID: 392
	[Header("Settings")]
	[SerializeField]
	private float linearForceMultiplier = 1f;

	// Token: 0x04000189 RID: 393
	[SerializeField]
	private float angularForceMultiplier = 6f;

	// Token: 0x0400018A RID: 394
	[HideInInspector]
	public Rigidbody Rigidbody;

	// Token: 0x0400018B RID: 395
	[HideInInspector]
	public SynchronizedObject SynchronizedObject;

	// Token: 0x0400018C RID: 396
	[HideInInspector]
	public float LinearIntensity = 1f;

	// Token: 0x0400018D RID: 397
	[HideInInspector]
	public float AngularIntensity = 1f;

	// Token: 0x0400018E RID: 398
	[HideInInspector]
	public bool Inverted;

	// Token: 0x0400018F RID: 399
	[HideInInspector]
	public bool UseWorldLinearVelocity;

	// Token: 0x04000190 RID: 400
	[HideInInspector]
	public Transform MovementDirection;
}
