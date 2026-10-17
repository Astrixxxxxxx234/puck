using System;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class PuckCollisionDetectionModeSwitcher : MonoBehaviour
{
	// Token: 0x060003B9 RID: 953 RVA: 0x0001654C File Offset: 0x0001474C
	private void Awake()
	{
		this.Rigidbody = base.GetComponent<Rigidbody>();
		Utils.SetRigidbodyCollisionDetectionMode(this.Rigidbody, CollisionDetectionMode.ContinuousDynamic);
	}

	// Token: 0x060003BA RID: 954 RVA: 0x00016566 File Offset: 0x00014766
	private void FixedUpdate()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (this.IsContactingStick)
		{
			Utils.SetRigidbodyCollisionDetectionMode(this.Rigidbody, CollisionDetectionMode.ContinuousSpeculative);
		}
		else
		{
			Utils.SetRigidbodyCollisionDetectionMode(this.Rigidbody, CollisionDetectionMode.ContinuousDynamic);
		}
		this.IsContactingStick = false;
	}

	// Token: 0x060003BB RID: 955 RVA: 0x000165A0 File Offset: 0x000147A0
	private void OnCollisionEnter(Collision collision)
	{
		Stick stick;
		if (!collision.gameObject.TryGetComponent<Stick>(out stick))
		{
			return;
		}
		this.IsContactingStick = true;
		Utils.SetRigidbodyCollisionDetectionMode(this.Rigidbody, CollisionDetectionMode.ContinuousSpeculative);
	}

	// Token: 0x060003BC RID: 956 RVA: 0x000165D0 File Offset: 0x000147D0
	private void OnCollisionStay(Collision collision)
	{
		Stick stick;
		if (!collision.gameObject.TryGetComponent<Stick>(out stick))
		{
			return;
		}
		this.IsContactingStick = true;
		Utils.SetRigidbodyCollisionDetectionMode(this.Rigidbody, CollisionDetectionMode.ContinuousSpeculative);
	}

	// Token: 0x060003BD RID: 957 RVA: 0x00016600 File Offset: 0x00014800
	public void OnDrawGizmos()
	{
		if (!Application.isEditor)
		{
			return;
		}
		if (this.Rigidbody)
		{
			Gizmos.color = ((this.Rigidbody.collisionDetectionMode == CollisionDetectionMode.ContinuousSpeculative) ? Color.red : Color.green);
		}
		Gizmos.DrawWireSphere(base.transform.position, 0.5f);
	}

	// Token: 0x040002C6 RID: 710
	[HideInInspector]
	public Rigidbody Rigidbody;

	// Token: 0x040002C7 RID: 711
	[HideInInspector]
	public bool IsContactingStick;
}
