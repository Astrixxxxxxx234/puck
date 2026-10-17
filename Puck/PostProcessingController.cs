using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006C RID: 108
public class PostProcessingController : MonoBehaviour
{
	// Token: 0x06000360 RID: 864 RVA: 0x00015484 File Offset: 0x00013684
	private void Awake()
	{
		this.postProcessing = base.GetComponent<PostProcessing>();
		EventManager.AddEventListener("Event_OnShowPuckSilhouetteChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPuckSilhouetteChanged));
		EventManager.AddEventListener("Event_OnShowPuckOutlineChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPuckOutlineChanged));
		EventManager.AddEventListener("Event_OnQualityChanged", new Action<Dictionary<string, object>>(this.Event_OnQualityChanged));
		EventManager.AddEventListener("Event_OnMotionBlurChanged", new Action<Dictionary<string, object>>(this.Event_OnMotionBlurChanged));
	}

	// Token: 0x06000361 RID: 865 RVA: 0x000154F8 File Offset: 0x000136F8
	private void Start()
	{
		this.postProcessing.SetPuckSilhouette(SettingsManager.ShowPuckSilhouette);
		this.postProcessing.SetPuckOutline(SettingsManager.ShowPuckOutline);
		this.postProcessing.SetQuality(SettingsManager.Quality);
		this.postProcessing.SetMotionBlur(SettingsManager.MotionBlur);
	}

	// Token: 0x06000362 RID: 866 RVA: 0x00015548 File Offset: 0x00013748
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnShowPuckSilhouetteChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPuckSilhouetteChanged));
		EventManager.RemoveEventListener("Event_OnShowPuckOutlineChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPuckOutlineChanged));
		EventManager.RemoveEventListener("Event_OnQualityChanged", new Action<Dictionary<string, object>>(this.Event_OnQualityChanged));
		EventManager.RemoveEventListener("Event_OnMotionBlurChanged", new Action<Dictionary<string, object>>(this.Event_OnMotionBlurChanged));
	}

	// Token: 0x06000363 RID: 867 RVA: 0x000155B0 File Offset: 0x000137B0
	private void Event_OnShowPuckSilhouetteChanged(Dictionary<string, object> message)
	{
		bool puckSilhouette = (bool)message["value"];
		this.postProcessing.SetPuckSilhouette(puckSilhouette);
	}

	// Token: 0x06000364 RID: 868 RVA: 0x000155DC File Offset: 0x000137DC
	private void Event_OnShowPuckOutlineChanged(Dictionary<string, object> message)
	{
		bool puckOutline = (bool)message["value"];
		this.postProcessing.SetPuckOutline(puckOutline);
	}

	// Token: 0x06000365 RID: 869 RVA: 0x00015608 File Offset: 0x00013808
	private void Event_OnQualityChanged(Dictionary<string, object> message)
	{
		ApplicationQuality quality = (ApplicationQuality)message["value"];
		this.postProcessing.SetQuality(quality);
	}

	// Token: 0x06000366 RID: 870 RVA: 0x00015634 File Offset: 0x00013834
	private void Event_OnMotionBlurChanged(Dictionary<string, object> message)
	{
		bool motionBlur = (bool)message["value"];
		this.postProcessing.SetMotionBlur(motionBlur);
	}

	// Token: 0x0400028D RID: 653
	private PostProcessing postProcessing;
}
