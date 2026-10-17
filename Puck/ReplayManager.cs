using System;
using UnityEngine;

// Token: 0x02000135 RID: 309
public class ReplayManager : MonoBehaviourSingleton<ReplayManager>
{
	// Token: 0x060008D3 RID: 2259 RVA: 0x0002B563 File Offset: 0x00029763
	public override void Awake()
	{
		base.Awake();
		this.ReplayRecorder = base.GetComponent<ReplayRecorder>();
		this.ReplayPlayer = base.GetComponent<ReplayPlayer>();
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x0002B583 File Offset: 0x00029783
	public void Server_StartRecording()
	{
		this.ReplayRecorder.Server_StartRecording();
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x0002B590 File Offset: 0x00029790
	public void Server_StopRecording()
	{
		this.ReplayRecorder.Server_StopRecording();
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x0002B5A0 File Offset: 0x000297A0
	public void Server_StartReplaying(float secondsToReplay)
	{
		ReplayRecording replayRecording = this.ReplayRecorder.Server_TakeRecording();
		if (replayRecording.TickCount == 0)
		{
			return;
		}
		int num = this.ReplayRecorder.Server_GetTicks(secondsToReplay);
		this.ReplayPlayer.Server_StartReplay(replayRecording, replayRecording.LastTick - num);
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x0002B5E3 File Offset: 0x000297E3
	public void Server_StopReplaying()
	{
		this.ReplayPlayer.Server_StopReplay();
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x0002B5F0 File Offset: 0x000297F0
	public void Server_ScheduleSlowMotionAtWorldTime(float worldTime, float scale, float rampInSeconds, float holdSeconds, float rampOutSeconds, float audioPitchFloor = 1f, float cameraFovPunch = 0f)
	{
		if (!this.ReplayRecorder.IsRecording)
		{
			return;
		}
		int num = this.ReplayRecorder.Server_GetTicks(Time.time - worldTime);
		int tick = this.ReplayRecorder.Tick - num;
		this.ReplayRecorder.Server_AddReplayEventAtTick(tick, "SlowMotion", new ReplaySlowMotion
		{
			Scale = scale,
			RampInSeconds = rampInSeconds,
			HoldSeconds = holdSeconds,
			RampOutSeconds = rampOutSeconds,
			AudioPitchFloor = audioPitchFloor,
			CameraFovPunch = cameraFovPunch
		});
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x0002B680 File Offset: 0x00029880
	public void Server_ScheduleCellyCam(ulong ownerClientId, float delaySeconds = 0f)
	{
		if (!this.ReplayRecorder.IsRecording)
		{
			return;
		}
		int num = this.ReplayRecorder.Server_GetTicks(delaySeconds);
		this.ReplayRecorder.Server_AddReplayEventAtTick(this.ReplayRecorder.Tick + num, "CellyCam", new ReplayCellyCam
		{
			OwnerClientId = ownerClientId
		});
	}

	// Token: 0x0400059F RID: 1439
	private static readonly global::Logger Logger = new global::Logger("ReplayManager");

	// Token: 0x040005A0 RID: 1440
	public ReplayRecorder ReplayRecorder;

	// Token: 0x040005A1 RID: 1441
	public ReplayPlayer ReplayPlayer;
}
