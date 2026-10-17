using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class MeshRendererHider : MonoBehaviour
{
	// Token: 0x0600009E RID: 158 RVA: 0x00003CDA File Offset: 0x00001EDA
	private void Awake()
	{
		if (this.useChildrenMeshRenderers)
		{
			this.meshRenderers = new List<MeshRenderer>(base.GetComponentsInChildren<MeshRenderer>(true));
			this.meshRenderers.RemoveAll((MeshRenderer meshRenderer) => this.meshRendererBlacklist.Contains(meshRenderer));
		}
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00003D10 File Offset: 0x00001F10
	public void HideMeshRenderers()
	{
		foreach (MeshRenderer meshRenderer in this.meshRenderers)
		{
			meshRenderer.enabled = false;
		}
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00003D64 File Offset: 0x00001F64
	public void ShowMeshRenderers()
	{
		foreach (MeshRenderer meshRenderer in this.meshRenderers)
		{
			meshRenderer.enabled = true;
		}
	}

	// Token: 0x0400004D RID: 77
	[Header("Settings")]
	[SerializeField]
	public List<MeshRenderer> meshRenderers;

	// Token: 0x0400004E RID: 78
	[SerializeField]
	public List<MeshRenderer> meshRendererBlacklist;

	// Token: 0x0400004F RID: 79
	[SerializeField]
	public bool useChildrenMeshRenderers = true;
}
