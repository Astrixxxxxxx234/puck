using System;
using Linework.SoftOutline;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x0200006A RID: 106
public class PostProcessing : MonoBehaviour
{
	// Token: 0x06000357 RID: 855 RVA: 0x00015391 File Offset: 0x00013591
	private void Awake()
	{
		this.volume = base.GetComponent<Volume>();
	}

	// Token: 0x06000358 RID: 856 RVA: 0x0001539F File Offset: 0x0001359F
	public void SetPuckSilhouette(bool enabled)
	{
		this.universalRendererData.rendererFeatures.Find((ScriptableRendererFeature x) => x.name == "Puck Silhouette").SetActive(enabled);
	}

	// Token: 0x06000359 RID: 857 RVA: 0x000153D6 File Offset: 0x000135D6
	public void SetPuckOutline(bool enabled)
	{
		this.puckOutlineSettings.SetActive(enabled);
	}

	// Token: 0x0600035A RID: 858 RVA: 0x000153E4 File Offset: 0x000135E4
	public void SetQuality(ApplicationQuality quality)
	{
		switch (quality)
		{
		case ApplicationQuality.Low:
			this.renderPipelineAsset.msaaSampleCount = 1;
			return;
		case ApplicationQuality.Medium:
			this.renderPipelineAsset.msaaSampleCount = 2;
			return;
		case ApplicationQuality.High:
			this.renderPipelineAsset.msaaSampleCount = 4;
			return;
		case ApplicationQuality.Ultra:
			this.renderPipelineAsset.msaaSampleCount = 8;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600035B RID: 859 RVA: 0x0001543C File Offset: 0x0001363C
	public void SetMotionBlur(bool enabled)
	{
		MotionBlur motionBlur;
		if (this.volume.profile.TryGet<MotionBlur>(out motionBlur))
		{
			motionBlur.active = enabled;
		}
	}

	// Token: 0x04000287 RID: 647
	[Header("References")]
	[SerializeField]
	private UniversalRenderPipelineAsset renderPipelineAsset;

	// Token: 0x04000288 RID: 648
	[SerializeField]
	private UniversalRendererData universalRendererData;

	// Token: 0x04000289 RID: 649
	[SerializeField]
	private SoftOutlineSettings puckOutlineSettings;

	// Token: 0x0400028A RID: 650
	private Volume volume;
}
