using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
[ExecuteInEditMode]
public class MeshRendererTexturer : MonoBehaviour
{
	// Token: 0x060000A3 RID: 163 RVA: 0x00003DD5 File Offset: 0x00001FD5
	private void Awake()
	{
		if (!this.meshRenderer)
		{
			this.meshRenderer = base.GetComponent<MeshRenderer>();
		}
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00003DF0 File Offset: 0x00001FF0
	public void SetTexture(Texture texture)
	{
		if (!this.meshRenderer)
		{
			return;
		}
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		this.meshRenderer.GetPropertyBlock(this.propertyBlock, this.materialIndex);
		if (texture != null)
		{
			this.propertyBlock.SetTexture(this.texturePropertyName, texture);
		}
		else
		{
			this.propertyBlock.Clear();
		}
		this.meshRenderer.SetPropertyBlock(this.propertyBlock, this.materialIndex);
	}

	// Token: 0x04000050 RID: 80
	[Header("References")]
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x04000051 RID: 81
	[SerializeField]
	private string texturePropertyName = "_BaseMap";

	// Token: 0x04000052 RID: 82
	[SerializeField]
	private int materialIndex;

	// Token: 0x04000053 RID: 83
	private MaterialPropertyBlock propertyBlock;
}
