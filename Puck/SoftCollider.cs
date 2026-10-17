using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200007D RID: 125
public class SoftCollider : MonoBehaviour
{
	// Token: 0x060003DB RID: 987 RVA: 0x00016D9A File Offset: 0x00014F9A
	private void Awake()
	{
		this.Rigidbody = base.GetComponent<Rigidbody>();
		this.SynchronizedObject = base.GetComponent<SynchronizedObject>();
	}

	// Token: 0x060003DC RID: 988 RVA: 0x00016DB4 File Offset: 0x00014FB4
	public void SetSoftCollisionIgnored(Rigidbody other, bool ignored)
	{
		if (ignored && other != null)
		{
			this.ignoredRigidbodies.Add(other);
		}
		else
		{
			this.ignoredRigidbodies.Remove(other);
		}
		this.ignoredRigidbodies.RemoveWhere((Rigidbody rigidbody) => rigidbody == null);
	}

	// Token: 0x060003DD RID: 989 RVA: 0x00016E14 File Offset: 0x00015014
	private void FixedUpdate()
	{
		if (this.SynchronizedObject.IsDrivenExternally)
		{
			return;
		}
		this.worldOrigin = base.transform.TransformPoint(this.localOrigin);
		Vector3 forward = base.transform.forward;
		Vector3 right = base.transform.right;
		this.rayDirections[0] = (forward + right).normalized;
		this.rayDirections[1] = (forward - right).normalized;
		this.rayDirections[2] = (-forward + right).normalized;
		this.rayDirections[3] = (-forward - right).normalized;
		foreach (Vector3 vector in this.rayDirections)
		{
			Debug.DrawRay(this.worldOrigin, vector * this.distance, Color.black);
			RaycastHit raycastHit;
			if (Physics.Raycast(this.worldOrigin, vector, out raycastHit, this.distance, this.layerMask) && (!(raycastHit.rigidbody != null) || !this.ignoredRigidbodies.Contains(raycastHit.rigidbody)))
			{
				Debug.DrawRay(this.worldOrigin, vector * raycastHit.distance, Color.white);
				float d = this.distance - raycastHit.distance;
				float magnitude = Vector3.Cross(raycastHit.normal, vector).magnitude;
				float num = 1f - magnitude;
				Debug.DrawRay(raycastHit.point, raycastHit.normal * d * this.force, Color.green);
				this.Rigidbody.AddForceAtPosition(raycastHit.normal * d * (this.force * num), raycastHit.point, ForceMode.Acceleration);
			}
		}
	}

	// Token: 0x040002E0 RID: 736
	[Header("Settings")]
	[SerializeField]
	private Vector3 localOrigin = Vector3.zero;

	// Token: 0x040002E1 RID: 737
	[SerializeField]
	private LayerMask layerMask;

	// Token: 0x040002E2 RID: 738
	[SerializeField]
	private float distance = 0.5f;

	// Token: 0x040002E3 RID: 739
	[SerializeField]
	private float force = 10f;

	// Token: 0x040002E4 RID: 740
	[HideInInspector]
	public float Intensity = 1f;

	// Token: 0x040002E5 RID: 741
	[HideInInspector]
	public Rigidbody Rigidbody;

	// Token: 0x040002E6 RID: 742
	[HideInInspector]
	public SynchronizedObject SynchronizedObject;

	// Token: 0x040002E7 RID: 743
	private readonly HashSet<Rigidbody> ignoredRigidbodies = new HashSet<Rigidbody>();

	// Token: 0x040002E8 RID: 744
	private Vector3 worldOrigin = Vector3.zero;

	// Token: 0x040002E9 RID: 745
	private readonly Vector3[] rayDirections = new Vector3[4];
}
