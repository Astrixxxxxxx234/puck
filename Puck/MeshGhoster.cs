using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000042 RID: 66
public class MeshGhoster : MonoBehaviour
{
	// Token: 0x17000015 RID: 21
	// (get) Token: 0x0600013C RID: 316 RVA: 0x00006B8A File Offset: 0x00004D8A
	private Camera GhostCamera
	{
		get
		{
			if (this.ghostCamera == null || !this.ghostCamera.isActiveAndEnabled)
			{
				this.ghostCamera = Camera.main;
			}
			return this.ghostCamera;
		}
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00006BB8 File Offset: 0x00004DB8
	public void SetTransparency(float strength)
	{
		strength = Mathf.Clamp01(strength);
		if (this.strength == strength)
		{
			return;
		}
		if (strength > 0f && this.ghostMaterial == null)
		{
			MeshGhoster.Logger.Warning("'" + base.name + "' has no ghost material assigned — can't render the crease ghost.");
			return;
		}
		bool flag = this.strength > 0f;
		this.strength = strength;
		if (strength > 0f && !flag)
		{
			this.ApplyGhostMaterials();
		}
		if (strength <= 0f)
		{
			this.RestoreMaterials();
			return;
		}
		this.ApplyGhostStrength();
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00006C48 File Offset: 0x00004E48
	private void Update()
	{
		if (this.ghostInstances != null)
		{
			this.ApplyGhostTextAlpha();
		}
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00006C58 File Offset: 0x00004E58
	private void ApplyGhostMaterials()
	{
		if (this.ghostableRenderers == null)
		{
			this.ghostableRenderers = this.CollectGhostableRenderers();
			this.ghostableTexts = base.GetComponentsInChildren<TMP_Text>(true);
		}
		Shader shader = this.ghostMaterial.shader;
		this.ghostFadeNear = this.ghostMaterial.GetFloat(MeshGhoster.FadeNearId);
		this.ghostFadeFar = this.ghostMaterial.GetFloat(MeshGhoster.FadeFarId);
		this.ghostNearAlpha = this.ghostMaterial.GetFloat(MeshGhoster.NearAlphaId);
		this.ghostFarAlpha = this.ghostMaterial.GetFloat(MeshGhoster.FarAlphaId);
		this.ghostCameraFade = this.ghostMaterial.GetFloat(MeshGhoster.CameraFadeId);
		this.CaptureTextAlphas();
		this.preGhostMaterials = new Material[this.ghostableRenderers.Length][];
		this.ghostInstances = new Material[this.ghostableRenderers.Length][];
		for (int i = 0; i < this.ghostableRenderers.Length; i++)
		{
			Renderer renderer = this.ghostableRenderers[i];
			if (!(renderer == null))
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				this.preGhostMaterials[i] = sharedMaterials;
				Material[] array = new Material[sharedMaterials.Length];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = this.BuildGhostInstance(sharedMaterials[j], shader);
				}
				renderer.sharedMaterials = array;
				this.ghostInstances[i] = array;
			}
		}
	}

	// Token: 0x06000140 RID: 320 RVA: 0x00006DA0 File Offset: 0x00004FA0
	private Renderer[] CollectGhostableRenderers()
	{
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>(true);
		List<Renderer> list = new List<Renderer>(componentsInChildren.Length);
		foreach (Renderer renderer in componentsInChildren)
		{
			Shader shader = (renderer != null && renderer.sharedMaterial != null) ? renderer.sharedMaterial.shader : null;
			if (!(shader != null) || !shader.name.StartsWith("TextMeshPro"))
			{
				list.Add(renderer);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00006E24 File Offset: 0x00005024
	private Material BuildGhostInstance(Material original, Shader ghostShader)
	{
		if (original == null)
		{
			return new Material(ghostShader)
			{
				hideFlags = HideFlags.HideAndDontSave
			};
		}
		if (original.shader != null && original.shader.name == "Custom/Depth")
		{
			return original;
		}
		Color color = original.color;
		Texture mainTexture = original.mainTexture;
		Vector2 mainTextureScale = original.mainTextureScale;
		Vector2 mainTextureOffset = original.mainTextureOffset;
		Texture texture = original.HasProperty(MeshGhoster.BumpMapId) ? original.GetTexture(MeshGhoster.BumpMapId) : null;
		Material material = new Material(original)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		material.shader = ghostShader;
		material.SetColor(MeshGhoster.BaseColorId, color);
		material.SetTexture(MeshGhoster.BaseMapId, mainTexture);
		material.SetTextureScale(MeshGhoster.BaseMapId, mainTextureScale);
		material.SetTextureOffset(MeshGhoster.BaseMapId, mainTextureOffset);
		if (texture != null)
		{
			material.SetTexture(MeshGhoster.BumpMapId, texture);
		}
		bool flag = (original.HasProperty(MeshGhoster.SurfaceId) && original.GetFloat(MeshGhoster.SurfaceId) >= 0.5f) || original.renderQueue >= 2501;
		material.SetFloat(MeshGhoster.BaseOpacityId, flag ? Mathf.Clamp01(color.a) : 1f);
		if (flag)
		{
			material.SetShaderPassEnabled("SRPDefaultUnlit", false);
		}
		material.SetFloat(MeshGhoster.FadeNearId, this.ghostFadeNear);
		material.SetFloat(MeshGhoster.FadeFarId, this.ghostFadeFar);
		material.SetFloat(MeshGhoster.NearAlphaId, this.ghostNearAlpha);
		material.SetFloat(MeshGhoster.FarAlphaId, this.ghostFarAlpha);
		material.SetFloat(MeshGhoster.CameraFadeId, this.ghostCameraFade);
		material.SetFloat(MeshGhoster.GhostFadeId, this.strength);
		return material;
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00006FE4 File Offset: 0x000051E4
	private void ApplyGhostStrength()
	{
		if (this.ghostInstances == null)
		{
			return;
		}
		foreach (Material[] array2 in this.ghostInstances)
		{
			if (array2 != null)
			{
				foreach (Material material in array2)
				{
					if (material != null)
					{
						material.SetFloat(MeshGhoster.GhostFadeId, this.strength);
					}
				}
			}
		}
	}

	// Token: 0x06000143 RID: 323 RVA: 0x0000704C File Offset: 0x0000524C
	private void ApplyGhostTextAlpha()
	{
		if (this.ghostableTexts == null)
		{
			return;
		}
		Camera camera = this.GhostCamera;
		Vector3 cameraPosition = (camera != null) ? camera.transform.position : Vector3.zero;
		foreach (TMP_Text tmp_Text in this.ghostableTexts)
		{
			if (!(tmp_Text == null))
			{
				tmp_Text.alpha = ((camera != null) ? this.GhostAlphaAt(tmp_Text.transform.position, cameraPosition) : Mathf.Lerp(1f, this.ghostFarAlpha, this.strength));
			}
		}
	}

	// Token: 0x06000144 RID: 324 RVA: 0x000070E4 File Offset: 0x000052E4
	private float GhostAlphaAt(Vector3 worldPosition, Vector3 cameraPosition)
	{
		float value = Vector3.Distance(worldPosition, cameraPosition);
		float t = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(this.ghostFadeNear, this.ghostFadeFar, value));
		float b = Mathf.Lerp(this.ghostNearAlpha, this.ghostFarAlpha, t);
		float b2 = Mathf.Lerp(this.ghostFarAlpha, b, this.ghostCameraFade);
		return Mathf.Lerp(1f, b2, this.strength);
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00007154 File Offset: 0x00005354
	private void CaptureTextAlphas()
	{
		if (this.ghostableTexts == null)
		{
			return;
		}
		this.preGhostTextAlphas = new float[this.ghostableTexts.Length];
		for (int i = 0; i < this.ghostableTexts.Length; i++)
		{
			this.preGhostTextAlphas[i] = ((this.ghostableTexts[i] != null) ? this.ghostableTexts[i].alpha : 1f);
		}
	}

	// Token: 0x06000146 RID: 326 RVA: 0x000071BC File Offset: 0x000053BC
	private void RestoreMaterials()
	{
		this.strength = 0f;
		if (this.ghostInstances == null)
		{
			return;
		}
		for (int i = 0; i < this.ghostableRenderers.Length; i++)
		{
			Renderer renderer = this.ghostableRenderers[i];
			if (renderer != null && this.preGhostMaterials[i] != null)
			{
				renderer.sharedMaterials = this.preGhostMaterials[i];
			}
			if (this.ghostInstances[i] != null)
			{
				for (int j = 0; j < this.ghostInstances[i].Length; j++)
				{
					Material material = this.ghostInstances[i][j];
					if (material != null && material != this.preGhostMaterials[i][j])
					{
						Object.Destroy(material);
					}
				}
			}
		}
		if (this.ghostableTexts != null && this.preGhostTextAlphas != null)
		{
			for (int k = 0; k < this.ghostableTexts.Length; k++)
			{
				if (this.ghostableTexts[k] != null)
				{
					this.ghostableTexts[k].alpha = this.preGhostTextAlphas[k];
				}
			}
		}
		this.preGhostMaterials = null;
		this.ghostInstances = null;
		this.preGhostTextAlphas = null;
	}

	// Token: 0x040000F4 RID: 244
	private static readonly global::Logger Logger = new global::Logger("MeshGhoster");

	// Token: 0x040000F5 RID: 245
	private static readonly int GhostFadeId = Shader.PropertyToID("_GhostFade");

	// Token: 0x040000F6 RID: 246
	private static readonly int CameraFadeId = Shader.PropertyToID("_CameraFade");

	// Token: 0x040000F7 RID: 247
	private static readonly int FadeNearId = Shader.PropertyToID("_FadeNear");

	// Token: 0x040000F8 RID: 248
	private static readonly int FadeFarId = Shader.PropertyToID("_FadeFar");

	// Token: 0x040000F9 RID: 249
	private static readonly int NearAlphaId = Shader.PropertyToID("_NearAlpha");

	// Token: 0x040000FA RID: 250
	private static readonly int FarAlphaId = Shader.PropertyToID("_FarAlpha");

	// Token: 0x040000FB RID: 251
	private static readonly int BaseMapId = Shader.PropertyToID("_BaseMap");

	// Token: 0x040000FC RID: 252
	private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

	// Token: 0x040000FD RID: 253
	private static readonly int BumpMapId = Shader.PropertyToID("_BumpMap");

	// Token: 0x040000FE RID: 254
	private static readonly int BaseOpacityId = Shader.PropertyToID("_BaseOpacity");

	// Token: 0x040000FF RID: 255
	private static readonly int SurfaceId = Shader.PropertyToID("_Surface");

	// Token: 0x04000100 RID: 256
	private const string DepthShaderName = "Custom/Depth";

	// Token: 0x04000101 RID: 257
	[Header("References")]
	[SerializeField]
	private Material ghostMaterial;

	// Token: 0x04000102 RID: 258
	private Renderer[] ghostableRenderers;

	// Token: 0x04000103 RID: 259
	private TMP_Text[] ghostableTexts;

	// Token: 0x04000104 RID: 260
	private Material[][] preGhostMaterials;

	// Token: 0x04000105 RID: 261
	private Material[][] ghostInstances;

	// Token: 0x04000106 RID: 262
	private float[] preGhostTextAlphas;

	// Token: 0x04000107 RID: 263
	private float ghostFadeNear;

	// Token: 0x04000108 RID: 264
	private float ghostFadeFar;

	// Token: 0x04000109 RID: 265
	private float ghostNearAlpha;

	// Token: 0x0400010A RID: 266
	private float ghostFarAlpha;

	// Token: 0x0400010B RID: 267
	private float ghostCameraFade;

	// Token: 0x0400010C RID: 268
	private float strength;

	// Token: 0x0400010D RID: 269
	private Camera ghostCamera;
}
