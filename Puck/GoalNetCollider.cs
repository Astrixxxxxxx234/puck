using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class GoalNetCollider : MonoBehaviour
{
	// Token: 0x0600004A RID: 74 RVA: 0x00002C64 File Offset: 0x00000E64
	private void OnCollisionEnter(Collision collision)
	{
		Puck componentInParent = collision.gameObject.GetComponentInParent<Puck>();
		if (!componentInParent)
		{
			return;
		}
		if (componentInParent.IsGrounded)
		{
			return;
		}
		componentInParent.Rigidbody.linearVelocity *= 1f - this.damping;
		componentInParent.Rigidbody.angularVelocity *= 1f - this.damping;
		if (componentInParent.Rigidbody.linearVelocity.magnitude > this.linearVelocityMaximumMagnitude)
		{
			componentInParent.Rigidbody.linearVelocity = componentInParent.Rigidbody.linearVelocity.normalized * this.linearVelocityMaximumMagnitude;
		}
		if (componentInParent.Rigidbody.angularVelocity.magnitude > this.angularVelocityMaximumMagnitude)
		{
			componentInParent.Rigidbody.angularVelocity = componentInParent.Rigidbody.angularVelocity.normalized * this.angularVelocityMaximumMagnitude;
		}
	}

	// Token: 0x0400002B RID: 43
	[Header("Settings")]
	[SerializeField]
	private float damping = 0.25f;

	// Token: 0x0400002C RID: 44
	[SerializeField]
	private float linearVelocityMaximumMagnitude = 2f;

	// Token: 0x0400002D RID: 45
	[SerializeField]
	private float angularVelocityMaximumMagnitude = 2f;
}
