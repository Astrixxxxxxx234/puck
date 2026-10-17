using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class ReplayCamera : BaseCamera
{
	// Token: 0x060003BF RID: 959 RVA: 0x00016658 File Offset: 0x00014858
	private void Update()
	{
		if (!this.Target)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		float num = this.IsCellyCam ? this.cellyCamFollowSpeed : this.followSpeed;
		float num2 = this.IsCellyCam ? this.cellyCamRotationSpeed : this.rotationSpeed;
		Vector3 normalized = (this.CenterPoint - this.Target.position).normalized;
		Vector3 b = this.Target.position + normalized * this.followDistance;
		b.y = this.followHeight;
		base.transform.position = Vector3.Lerp(base.transform.position, b, deltaTime * num);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(this.Target.position - base.transform.position), deltaTime * num2);
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x000167B0 File Offset: 0x000149B0
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x00003106 File Offset: 0x00001306
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x000167C6 File Offset: 0x000149C6
	protected internal override string __getTypeName()
	{
		return "ReplayCamera";
	}

	// Token: 0x040002C8 RID: 712
	[Header("Settings")]
	[SerializeField]
	private float followSpeed = 10f;

	// Token: 0x040002C9 RID: 713
	[SerializeField]
	private float followDistance = 5f;

	// Token: 0x040002CA RID: 714
	[SerializeField]
	private float followHeight = 5f;

	// Token: 0x040002CB RID: 715
	[SerializeField]
	private float rotationSpeed = 10f;

	// Token: 0x040002CC RID: 716
	[Header("Celly Cam")]
	[SerializeField]
	private float cellyCamFollowSpeed = 2f;

	// Token: 0x040002CD RID: 717
	[SerializeField]
	private float cellyCamRotationSpeed = 2f;

	// Token: 0x040002CE RID: 718
	[HideInInspector]
	public Transform Target;

	// Token: 0x040002CF RID: 719
	[HideInInspector]
	public Vector3 CenterPoint = Vector3.zero;

	// Token: 0x040002D0 RID: 720
	[HideInInspector]
	public bool IsCellyCam;
}
