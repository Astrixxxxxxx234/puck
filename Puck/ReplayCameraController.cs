using System;
using System.Collections.Generic;

// Token: 0x02000079 RID: 121
public class ReplayCameraController : BaseCameraController
{
	// Token: 0x060003C4 RID: 964 RVA: 0x000167CD File Offset: 0x000149CD
	public override void Awake()
	{
		base.Awake();
		this.replayCamera = base.GetComponent<ReplayCamera>();
		EventManager.AddEventListener("Event_Everyone_OnPuckSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckSpawned));
		EventManager.AddEventListener("Event_Everyone_OnReplayCellyCam", new Action<Dictionary<string, object>>(this.Event_Everyone_OnReplayCellyCam));
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x0001680D File Offset: 0x00014A0D
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnPuckSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPuckSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnReplayCellyCam", new Action<Dictionary<string, object>>(this.Event_Everyone_OnReplayCellyCam));
		base.OnDestroy();
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00016844 File Offset: 0x00014A44
	private void Event_Everyone_OnPuckSpawned(Dictionary<string, object> message)
	{
		Puck puck = (Puck)message["puck"];
		this.replayCamera.IsCellyCam = false;
		this.replayCamera.Target = puck.transform;
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x00016880 File Offset: 0x00014A80
	private void Event_Everyone_OnReplayCellyCam(Dictionary<string, object> message)
	{
		ulong clientId = (ulong)message["scorerClientId"];
		Player replayPlayerByClientId = MonoBehaviourSingleton<PlayerManager>.Instance.GetReplayPlayerByClientId(clientId);
		if (replayPlayerByClientId && replayPlayerByClientId.PlayerBody)
		{
			this.replayCamera.IsCellyCam = true;
			this.replayCamera.Target = replayPlayerByClientId.PlayerBody.transform;
		}
	}

	// Token: 0x040002D1 RID: 721
	private ReplayCamera replayCamera;
}
