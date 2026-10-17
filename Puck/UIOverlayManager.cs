using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x020001EA RID: 490
public class UIOverlayManager : UIView
{
	// Token: 0x06000F38 RID: 3896 RVA: 0x00048617 File Offset: 0x00046817
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("OverlaysView", null);
		this.overlays = base.View.Query("Overlays", null);
		this.overlays.Clear();
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x00048658 File Offset: 0x00046858
	private void OnDestroy()
	{
		Sequence sequence = this.flashSequence;
		if (sequence != null)
		{
			sequence.Kill(false);
		}
		if (this.vignetteTexture != null)
		{
			Object.Destroy(this.vignetteTexture);
		}
		foreach (Overlay overlay in this.identifierOverlaysMap.Values)
		{
			overlay.Dispose();
		}
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x000486D8 File Offset: 0x000468D8
	public void FlashScreen(Color color, float peakOpacity, float rampInSeconds, float holdSeconds, float rampOutSeconds)
	{
		if (this.overlays == null)
		{
			return;
		}
		if (this.vignetteTexture == null)
		{
			this.vignetteTexture = this.CreateVignetteTexture(256);
		}
		if (this.flashElement == null)
		{
			this.flashElement = new VisualElement();
			this.flashElement.style.position = Position.Absolute;
			this.flashElement.style.left = 0f;
			this.flashElement.style.right = 0f;
			this.flashElement.style.top = 0f;
			this.flashElement.style.bottom = 0f;
			this.flashElement.style.opacity = 0f;
			this.flashElement.style.backgroundImage = new StyleBackground(this.vignetteTexture);
			this.flashElement.style.unityBackgroundScaleMode = ScaleMode.StretchToFill;
			this.flashElement.pickingMode = PickingMode.Ignore;
			this.overlays.Add(this.flashElement);
		}
		this.flashElement.BringToFront();
		this.flashElement.style.unityBackgroundImageTintColor = color;
		Sequence sequence = this.flashSequence;
		if (sequence != null)
		{
			sequence.Kill(false);
		}
		this.flashSequence = TweenUtils.RampHoldRamp(0f, peakOpacity, rampInSeconds, holdSeconds, rampOutSeconds, Ease.OutQuad, Ease.OutQuad, delegate(float value)
		{
			this.flashElement.style.opacity = value;
		});
		this.flashSequence.OnKill(delegate
		{
			this.flashSequence = null;
		});
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x00048878 File Offset: 0x00046A78
	private Texture2D CreateVignetteTexture(int size = 256)
	{
		Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGBA32, false)
		{
			wrapMode = TextureWrapMode.Clamp
		};
		Color[] array = new Color[size * size];
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				float num = (float)j / (float)(size - 1) * 2f - 1f;
				float num2 = (float)i / (float)(size - 1) * 2f - 1f;
				float num3 = Mathf.Clamp01((Mathf.Sqrt(num * num + num2 * num2) - 0.5f) / 0.70000005f);
				float a = num3 * num3 * (3f - 2f * num3);
				array[i * size + j] = new Color(1f, 1f, 1f, a);
			}
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x06000F3C RID: 3900 RVA: 0x00048950 File Offset: 0x00046B50
	public void ShowOverlay(string identifier, bool requiresSpinner = false, bool fadeIn = false, bool fadeOut = false, float fadeTime = 0.25f, bool autoHide = false, float hideTimeout = 0.25f)
	{
		Overlay overlay;
		if (!this.identifierOverlaysMap.ContainsKey(identifier))
		{
			overlay = new Overlay(this.overlayAsset.Instantiate(), identifier, requiresSpinner);
			Overlay overlay2 = overlay;
			overlay2.Hidden = (Action)Delegate.Combine(overlay2.Hidden, new Action(delegate()
			{
				overlay.Dispose();
				this.identifierOverlaysMap.Remove(identifier);
				this.overlays.Remove(overlay.VisualElement);
			}));
			this.identifierOverlaysMap.Add(identifier, overlay);
			this.overlays.Add(overlay.VisualElement);
		}
		else
		{
			overlay = this.identifierOverlaysMap[identifier];
		}
		overlay.FadeIn = fadeIn;
		overlay.FadeOut = fadeOut;
		overlay.FadeTime = fadeTime;
		overlay.AutoHide = autoHide;
		overlay.HideTimeout = hideTimeout;
		overlay.Show();
	}

	// Token: 0x06000F3D RID: 3901 RVA: 0x00048A59 File Offset: 0x00046C59
	public void HideOverlay(string identifier)
	{
		if (!this.identifierOverlaysMap.ContainsKey(identifier))
		{
			return;
		}
		this.identifierOverlaysMap[identifier].Hide();
	}

	// Token: 0x040009AF RID: 2479
	[Header("References")]
	public VisualTreeAsset overlayAsset;

	// Token: 0x040009B0 RID: 2480
	private VisualElement overlays;

	// Token: 0x040009B1 RID: 2481
	private VisualElement flashElement;

	// Token: 0x040009B2 RID: 2482
	private Sequence flashSequence;

	// Token: 0x040009B3 RID: 2483
	private Texture2D vignetteTexture;

	// Token: 0x040009B4 RID: 2484
	private Dictionary<string, Overlay> identifierOverlaysMap = new Dictionary<string, Overlay>();
}
