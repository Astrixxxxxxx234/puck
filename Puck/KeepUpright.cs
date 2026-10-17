using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
[RequireComponent(typeof(Rigidbody))]
public class KeepUpright : MonoBehaviour
{
	// Token: 0x06000108 RID: 264 RVA: 0x00005EFE File Offset: 0x000040FE
	private void Awake()
	{
		this.Rigidbody = base.GetComponent<Rigidbody>();
		this.SynchronizedObject = base.GetComponent<SynchronizedObject>();
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00005F18 File Offset: 0x00004118
	private void FixedUpdate()
	{
		this.pidController.proportionalGain = this.proportionalGain * this.Balance;
		this.pidController.integralGain = this.integralGain * this.Balance;
		this.pidController.derivativeGain = this.derivativeGain * this.Balance;
		Vector3 a = Vector3.Cross(this.pidController.Update(Time.fixedDeltaTime, base.transform.up, Vector3.up), Vector3.up);
		if (this.SynchronizedObject.IsDrivenExternally)
		{
			return;
		}
		this.Rigidbody.AddTorque(-a, ForceMode.Acceleration);
	}

	// Token: 0x040000AA RID: 170
	[Header("Settings")]
	[SerializeField]
	private float proportionalGain = 50f;

	// Token: 0x040000AB RID: 171
	[SerializeField]
	private float integralGain;

	// Token: 0x040000AC RID: 172
	[SerializeField]
	private float derivativeGain = 5f;

	// Token: 0x040000AD RID: 173
	[HideInInspector]
	public float Balance = 1f;

	// Token: 0x040000AE RID: 174
	[HideInInspector]
	public Rigidbody Rigidbody;

	// Token: 0x040000AF RID: 175
	[HideInInspector]
	public SynchronizedObject SynchronizedObject;

	// Token: 0x040000B0 RID: 176
	private Vector3PIDController pidController = new Vector3PIDController(0f, 0f, 0f);
}
