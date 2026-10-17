using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x020000A7 RID: 167
public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
	// Token: 0x06000587 RID: 1415 RVA: 0x0001E95E File Offset: 0x0001CB5E
	public void SetGlobalVolume(float volume)
	{
		this.mixer.SetFloat("globalVolume", Mathf.Log(volume + 0.001f) * 20f);
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x0001E983 File Offset: 0x0001CB83
	public void SetAmbientVolume(float volume)
	{
		this.mixer.SetFloat("ambientVolume", Mathf.Log(volume + 0.001f) * 20f);
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x0001E9A8 File Offset: 0x0001CBA8
	public void SetGameVolume(float volume)
	{
		float value = Mathf.Log(volume + 0.001f) * 20f;
		this.mixer.SetFloat("gameVolume", value);
		this.mixer.SetFloat("hornVolume", value);
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x0001E9EC File Offset: 0x0001CBEC
	public void SetVoiceVolume(float volume)
	{
		this.mixer.SetFloat("voiceVolume", Mathf.Log(volume + 0.001f) * 20f);
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x0001EA11 File Offset: 0x0001CC11
	public void SetUIVolume(float volume)
	{
		this.mixer.SetFloat("uiVolume", Mathf.Log(volume + 0.001f) * 20f);
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x0001EA36 File Offset: 0x0001CC36
	public void SetSlowMotionLowpassCutoff(float cutoffHz)
	{
		this.mixer.SetFloat("gameLowpassCutoff", cutoffHz);
		this.mixer.SetFloat("ambientLowpassCutoff", cutoffHz);
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x0001EA5C File Offset: 0x0001CC5C
	public void SetSlowMotionPitch(float pitch)
	{
		this.mixer.SetFloat("gamePitch", pitch);
		this.mixer.SetFloat("ambientPitch", pitch);
	}

	// Token: 0x04000399 RID: 921
	[Header("References")]
	[SerializeField]
	private AudioMixer mixer;
}
