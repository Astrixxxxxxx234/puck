using System;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class PuckElevationIndicator : MonoBehaviour
{
	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000368 RID: 872 RVA: 0x0001565E File Offset: 0x0001385E
	// (set) Token: 0x06000369 RID: 873 RVA: 0x00015666 File Offset: 0x00013866
	public bool IsVisible
	{
		get
		{
			return this.isVisible;
		}
		set
		{
			if (ApplicationManager.IsDedicatedGameServer)
			{
				return;
			}
			this.isVisible = value;
			this.planeMeshRenderer.enabled = this.isVisible;
			this.lineRenderer.enabled = this.isVisible;
		}
	}

	// Token: 0x0600036A RID: 874 RVA: 0x0001569C File Offset: 0x0001389C
	private void Awake()
	{
		this.lineRenderer.positionCount = 2;
		this.material = this.planeMeshRenderer.material;
		this.planeMeshRenderer.enabled = false;
		this.lineRenderer.enabled = false;
		if (ApplicationManager.IsDedicatedGameServer)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600036B RID: 875 RVA: 0x000156EC File Offset: 0x000138EC
	private void OnDestroy()
	{
		Object.Destroy(this.material);
	}

	// Token: 0x0600036C RID: 876 RVA: 0x000156FC File Offset: 0x000138FC
	private void Update()
	{
		if (!this.IsVisible)
		{
			return;
		}
		if (!this.material)
		{
			return;
		}
		Debug.DrawRay(base.transform.position, Vector3.down * float.PositiveInfinity, Color.black);
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, float.PositiveInfinity, this.raycastLayerMask))
		{
			this.planeMeshRenderer.transform.position = raycastHit.point - Vector3.up * this.raycastVerticalOffset;
			this.planeMeshRenderer.transform.rotation = Quaternion.FromToRotation(Vector3.up, raycastHit.normal);
			this.material.SetFloat("_Size", Mathf.Clamp(raycastHit.distance / this.maximumDistance, 0f, 1f));
			this.UpdateLineRendererPositions(raycastHit.point);
			this.planeMeshRenderer.enabled = true;
			this.lineRenderer.enabled = true;
			return;
		}
		this.planeMeshRenderer.enabled = false;
		this.lineRenderer.enabled = false;
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00015828 File Offset: 0x00013A28
	private void UpdateLineRendererPositions(Vector3 hitPosition)
	{
		this.lineRenderer.SetPosition(0, hitPosition);
		this.lineRenderer.SetPosition(1, base.transform.position);
	}

	// Token: 0x0400028E RID: 654
	[Header("Settings")]
	[SerializeField]
	private float maximumDistance = 15f;

	// Token: 0x0400028F RID: 655
	[SerializeField]
	private float raycastVerticalOffset = 0.01f;

	// Token: 0x04000290 RID: 656
	[SerializeField]
	private LayerMask raycastLayerMask;

	// Token: 0x04000291 RID: 657
	[Header("References")]
	[SerializeField]
	private MeshRenderer planeMeshRenderer;

	// Token: 0x04000292 RID: 658
	[SerializeField]
	private LineRenderer lineRenderer;

	// Token: 0x04000293 RID: 659
	private bool isVisible;

	// Token: 0x04000294 RID: 660
	private Material material;
}
