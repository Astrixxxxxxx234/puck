using System;
using UnityEngine;
using UnityEngine.Splines;

// Token: 0x0200022E RID: 558
public class MoveAlongSpline : MonoBehaviour
{
	// Token: 0x060011A4 RID: 4516 RVA: 0x000530E8 File Offset: 0x000512E8
	private void Update()
	{
		this.splinePosition += Time.deltaTime * this.speed;
		if (this.splinePosition >= 1f)
		{
			this.splinePosition = 0f;
		}
		Vector3 position = this.spline.EvaluatePosition(this.splinePosition);
		base.transform.position = position;
		base.transform.LookAt(Vector3.zero);
	}

	// Token: 0x04000B83 RID: 2947
	[Header("Settings")]
	public float speed = 1f;

	// Token: 0x04000B84 RID: 2948
	[Header("References")]
	public SplineContainer spline;

	// Token: 0x04000B85 RID: 2949
	private float splinePosition;
}
