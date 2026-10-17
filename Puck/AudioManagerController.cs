using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class AudioManagerController : MonoBehaviour
{
	// Token: 0x0600058F RID: 1423 RVA: 0x0001EA8C File Offset: 0x0001CC8C
	private void Awake()
	{
		this.audioManager = base.GetComponent<AudioManager>();
		EventManager.AddEventListener("Event_OnGlobalVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnGlobalVolumeChanged));
		EventManager.AddEventListener("Event_OnAmbientVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnAmbientVolumeChanged));
		EventManager.AddEventListener("Event_OnGameVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnGameVolumeChanged));
		EventManager.AddEventListener("Event_OnVoiceVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnVoiceVolumeChanged));
		EventManager.AddEventListener("Event_OnUIVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnUIVolumeChanged));
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x0001EB14 File Offset: 0x0001CD14
	private void Start()
	{
		this.audioManager.SetGlobalVolume(SettingsManager.GlobalVolume);
		this.audioManager.SetAmbientVolume(SettingsManager.AmbientVolume);
		this.audioManager.SetGameVolume(SettingsManager.GameVolume);
		this.audioManager.SetVoiceVolume(SettingsManager.VoiceVolume);
		this.audioManager.SetUIVolume(SettingsManager.UIVolume);
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x0001EB74 File Offset: 0x0001CD74
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnGlobalVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnGlobalVolumeChanged));
		EventManager.RemoveEventListener("Event_OnAmbientVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnAmbientVolumeChanged));
		EventManager.RemoveEventListener("Event_OnGameVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnGameVolumeChanged));
		EventManager.RemoveEventListener("Event_OnVoiceVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnVoiceVolumeChanged));
		EventManager.RemoveEventListener("Event_OnUIVolumeChanged", new Action<Dictionary<string, object>>(this.Event_OnUIVolumeChanged));
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x0001EBF0 File Offset: 0x0001CDF0
	private void Event_OnGlobalVolumeChanged(Dictionary<string, object> eventParams)
	{
		float globalVolume = (float)eventParams["value"];
		this.audioManager.SetGlobalVolume(globalVolume);
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x0001EC1C File Offset: 0x0001CE1C
	private void Event_OnAmbientVolumeChanged(Dictionary<string, object> eventParams)
	{
		float ambientVolume = (float)eventParams["value"];
		this.audioManager.SetAmbientVolume(ambientVolume);
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x0001EC48 File Offset: 0x0001CE48
	private void Event_OnGameVolumeChanged(Dictionary<string, object> eventParams)
	{
		float gameVolume = (float)eventParams["value"];
		this.audioManager.SetGameVolume(gameVolume);
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x0001EC74 File Offset: 0x0001CE74
	private void Event_OnVoiceVolumeChanged(Dictionary<string, object> eventParams)
	{
		float voiceVolume = (float)eventParams["value"];
		this.audioManager.SetVoiceVolume(voiceVolume);
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x0001ECA0 File Offset: 0x0001CEA0
	private void Event_OnUIVolumeChanged(Dictionary<string, object> eventParams)
	{
		float uivolume = (float)eventParams["value"];
		this.audioManager.SetUIVolume(uivolume);
	}

	// Token: 0x0400039A RID: 922
	private AudioManager audioManager;
}
