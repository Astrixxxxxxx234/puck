using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// Token: 0x020000CA RID: 202
public class GameManagerController : MonoBehaviour
{
	// Token: 0x0600066E RID: 1646 RVA: 0x00023AC7 File Offset: 0x00021CC7
	private void Awake()
	{
		this.gameManager = base.GetComponent<GameManager>();
		EventManager.AddEventListener("Event_Everyone_OnSlowMotionStarted", new Action<Dictionary<string, object>>(this.Event_Everyone_OnSlowMotionStarted));
		EventManager.AddEventListener("Event_Everyone_OnSlowMotionStopped", new Action<Dictionary<string, object>>(this.Event_Everyone_OnSlowMotionStopped));
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00023B04 File Offset: 0x00021D04
	private void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnSlowMotionStarted", new Action<Dictionary<string, object>>(this.Event_Everyone_OnSlowMotionStarted));
		EventManager.RemoveEventListener("Event_Everyone_OnSlowMotionStopped", new Action<Dictionary<string, object>>(this.Event_Everyone_OnSlowMotionStopped));
		Sequence sequence = this.slowMotionSequence;
		if (sequence != null)
		{
			sequence.Kill(false);
		}
		this.ApplySlowMotionScale(1f);
		Sequence sequence2 = this.cameraPunchSequence;
		if (sequence2 != null)
		{
			sequence2.Kill(false);
		}
		this.ApplyCameraPunch(0f);
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x00023B78 File Offset: 0x00021D78
	private void Event_Everyone_OnSlowMotionStarted(Dictionary<string, object> message)
	{
		float depth = (float)message["scale"];
		float num = (float)message["rampInSeconds"];
		float num2 = (float)message["holdSeconds"];
		float num3 = (float)message["rampOutSeconds"];
		this.slowMotionTargetScale = depth;
		this.slowMotionAudioPitchFloor = (float)message["audioPitchFloor"];
		this.slowMotionCameraFovPunch = (float)message["cameraFovPunch"];
		Sequence sequence = this.slowMotionSequence;
		if (sequence != null)
		{
			sequence.Kill(false);
		}
		this.slowMotionSequence = TweenUtils.RampHoldRamp(this.slowMotionScale, depth, 1f, num, num2, num3, Ease.InQuad, Ease.OutQuad, new TweenCallback<float>(this.ApplySlowMotionScale)).SetTarget(this);
		this.slowMotionSequence.OnComplete(delegate
		{
			this.ApplySlowMotionScale(1f);
		});
		this.slowMotionSequence.OnKill(delegate
		{
			this.slowMotionSequence = null;
		});
		Sequence sequence2 = this.cameraPunchSequence;
		if (sequence2 != null)
		{
			sequence2.Kill(false);
		}
		if (this.slowMotionCameraFovPunch > 0f)
		{
			this.cameraPunchSequence = DOTween.Sequence().SetUpdate(true).SetTarget(this);
			this.cameraPunchSequence.Append(DOVirtual.Float(this.slowMotionCameraAmount, 1f, num + num2, new TweenCallback<float>(this.ApplyCameraPunch)).SetEase(Ease.InOutSine));
			this.cameraPunchSequence.Append(DOVirtual.Float(1f, 0f, num3, new TweenCallback<float>(this.ApplyCameraPunch)).SetEase(Ease.InOutSine));
			this.cameraPunchSequence.OnComplete(delegate
			{
				this.ApplyCameraPunch(0f);
			});
			this.cameraPunchSequence.OnKill(delegate
			{
				this.cameraPunchSequence = null;
			});
			return;
		}
		this.ApplyCameraPunch(0f);
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x00023D40 File Offset: 0x00021F40
	private void Event_Everyone_OnSlowMotionStopped(Dictionary<string, object> message)
	{
		Sequence sequence = this.slowMotionSequence;
		if (sequence != null)
		{
			sequence.Kill(false);
		}
		Sequence sequence2 = this.cameraPunchSequence;
		if (sequence2 != null)
		{
			sequence2.Kill(false);
		}
		this.ApplySlowMotionScale(1f);
		this.ApplyCameraPunch(0f);
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x00023D7C File Offset: 0x00021F7C
	private void ApplySlowMotionScale(float scale)
	{
		this.slowMotionScale = scale;
		if (MonoBehaviourSingleton<AudioManager>.Instance == null)
		{
			return;
		}
		float t = (this.slowMotionTargetScale < 1f) ? Mathf.InverseLerp(1f, this.slowMotionTargetScale, scale) : 0f;
		float slowMotionLowpassCutoff = Mathf.Exp(Mathf.Lerp(Mathf.Log(22000f), Mathf.Log(900f), t));
		MonoBehaviourSingleton<AudioManager>.Instance.SetSlowMotionLowpassCutoff(slowMotionLowpassCutoff);
		MonoBehaviourSingleton<AudioManager>.Instance.SetSlowMotionPitch(Mathf.Lerp(1f, this.slowMotionAudioPitchFloor, t));
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x00023E0C File Offset: 0x0002200C
	private void ApplyCameraPunch(float amount)
	{
		this.slowMotionCameraAmount = amount;
		BaseCamera activeCamera = CameraManager.GetActiveCamera();
		if (activeCamera == null || activeCamera.Type == global::CameraType.Spectator)
		{
			return;
		}
		activeCamera.SetFieldOfView(SettingsManager.Fov - this.slowMotionCameraFovPunch * amount);
	}

	// Token: 0x04000424 RID: 1060
	private GameManager gameManager;

	// Token: 0x04000425 RID: 1061
	private Sequence slowMotionSequence;

	// Token: 0x04000426 RID: 1062
	private float slowMotionTargetScale = 1f;

	// Token: 0x04000427 RID: 1063
	private float slowMotionAudioPitchFloor = 1f;

	// Token: 0x04000428 RID: 1064
	private float slowMotionScale = 1f;

	// Token: 0x04000429 RID: 1065
	private Sequence cameraPunchSequence;

	// Token: 0x0400042A RID: 1066
	private float slowMotionCameraFovPunch;

	// Token: 0x0400042B RID: 1067
	private float slowMotionCameraAmount;
}
