using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class PuckElevationIndicatorController : MonoBehaviour
{
	// Token: 0x0600036F RID: 879 RVA: 0x0001586C File Offset: 0x00013A6C
	private void Awake()
	{
		this.puckElevationIndicator = base.GetComponent<PuckElevationIndicator>();
	}

	// Token: 0x06000370 RID: 880 RVA: 0x0001587A File Offset: 0x00013A7A
	private void Start()
	{
		EventManager.AddEventListener("Event_OnShowPuckElevationChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPuckElevationChanged));
		this.puckElevationIndicator.IsVisible = SettingsManager.ShowPuckElevation;
	}

	// Token: 0x06000371 RID: 881 RVA: 0x000158A2 File Offset: 0x00013AA2
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnShowPuckElevationChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPuckElevationChanged));
	}

	// Token: 0x06000372 RID: 882 RVA: 0x000158BC File Offset: 0x00013ABC
	private void Event_OnShowPuckElevationChanged(Dictionary<string, object> message)
	{
		bool isVisible = (bool)message["value"];
		this.puckElevationIndicator.IsVisible = isVisible;
	}

	// Token: 0x04000295 RID: 661
	private PuckElevationIndicator puckElevationIndicator;
}
