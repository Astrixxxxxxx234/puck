using System;
using Steamworks;
using UnityEngine;

// Token: 0x02000211 RID: 529
public class MicTester : MonoBehaviour
{
	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06001053 RID: 4179 RVA: 0x0004D7A3 File Offset: 0x0004B9A3
	// (set) Token: 0x06001054 RID: 4180 RVA: 0x0004D7AB File Offset: 0x0004B9AB
	public bool IsActive { get; private set; }

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06001055 RID: 4181 RVA: 0x0004D7B4 File Offset: 0x0004B9B4
	// (remove) Token: 0x06001056 RID: 4182 RVA: 0x0004D7EC File Offset: 0x0004B9EC
	public event Action<float> OnLevelChanged;

	// Token: 0x06001057 RID: 4183 RVA: 0x0004D824 File Offset: 0x0004BA24
	private void Update()
	{
		if (!this.IsActive)
		{
			return;
		}
		PlayerVoiceRecorder localPlayerRecorder = this.GetLocalPlayerRecorder();
		if (localPlayerRecorder != null && localPlayerRecorder.IsRecording)
		{
			this.ReleaseRecording(true);
			this.meter.Reset();
			this.SetLevel(localPlayerRecorder.TransmitLevel);
			return;
		}
		if (!this.ownsRecording)
		{
			SteamUser.StartVoiceRecording();
			this.ownsRecording = true;
		}
		bool hasVoice;
		float rawLevel = this.Capture(out hasVoice);
		this.SetLevel(this.meter.Update(rawLevel, hasVoice, Time.deltaTime));
	}

	// Token: 0x06001058 RID: 4184 RVA: 0x0004D8A9 File Offset: 0x0004BAA9
	private void OnDestroy()
	{
		this.Stop();
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x0004D8B1 File Offset: 0x0004BAB1
	public void SetActive(bool value)
	{
		if (value)
		{
			this.Begin();
			return;
		}
		this.Stop();
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x0004D8C3 File Offset: 0x0004BAC3
	private void Begin()
	{
		if (this.IsActive || !SteamManager.IsInitialized)
		{
			return;
		}
		this.IsActive = true;
		this.sampleRate = SteamUser.GetVoiceOptimalSampleRate();
		if (this.sampleRate > 0U)
		{
			this.decompressBuffer = new byte[this.sampleRate * 2U];
		}
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x0004D903 File Offset: 0x0004BB03
	private void Stop()
	{
		if (!this.IsActive)
		{
			return;
		}
		this.IsActive = false;
		this.ReleaseRecording(false);
		this.meter.Reset();
		this.SetLevel(0f);
	}

	// Token: 0x0600105C RID: 4188 RVA: 0x0004D934 File Offset: 0x0004BB34
	private float Capture(out bool hasVoice)
	{
		hasVoice = false;
		uint num;
		SteamUser.GetAvailableVoice(out num);
		if (num == 0U)
		{
			return 0f;
		}
		byte[] array = new byte[num];
		uint num2;
		SteamUser.GetVoice(true, array, num, out num2);
		if (num2 == 0U)
		{
			return 0f;
		}
		hasVoice = true;
		return VoiceUtils.ComputeLevel(array, num2, this.sampleRate, this.decompressBuffer);
	}

	// Token: 0x0600105D RID: 4189 RVA: 0x0004D986 File Offset: 0x0004BB86
	private void ReleaseRecording(bool playerStillRecording)
	{
		if (this.ownsRecording && !playerStillRecording)
		{
			SteamUser.StopVoiceRecording();
		}
		this.ownsRecording = false;
	}

	// Token: 0x0600105E RID: 4190 RVA: 0x0004D99F File Offset: 0x0004BB9F
	private void SetLevel(float value)
	{
		this.level = value;
		Action<float> onLevelChanged = this.OnLevelChanged;
		if (onLevelChanged == null)
		{
			return;
		}
		onLevelChanged(value);
	}

	// Token: 0x0600105F RID: 4191 RVA: 0x0004D9BC File Offset: 0x0004BBBC
	private PlayerVoiceRecorder GetLocalPlayerRecorder()
	{
		Player player = (MonoBehaviourSingleton<PlayerManager>.Instance != null) ? MonoBehaviourSingleton<PlayerManager>.Instance.GetLocalPlayer() : null;
		if (!(player != null))
		{
			return null;
		}
		return player.GetComponent<PlayerVoiceRecorder>();
	}

	// Token: 0x04000A6A RID: 2666
	private uint sampleRate;

	// Token: 0x04000A6B RID: 2667
	private byte[] decompressBuffer;

	// Token: 0x04000A6C RID: 2668
	private bool ownsRecording;

	// Token: 0x04000A6D RID: 2669
	private float level;

	// Token: 0x04000A6E RID: 2670
	private readonly VoiceLevelMeter meter = new VoiceLevelMeter();
}
