using System;
using Unity.Netcode;
using UnityEngine;

// Token: 0x0200013D RID: 317
public class ReplayRecorder : MonoBehaviour
{
	// Token: 0x170000FC RID: 252
	// (get) Token: 0x06000904 RID: 2308 RVA: 0x0002C7FD File Offset: 0x0002A9FD
	[HideInInspector]
	public int Tick
	{
		get
		{
			return this.recording.TickCount;
		}
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x0002C80A File Offset: 0x0002AA0A
	private void Awake()
	{
		PhysicsManager.OnAfterSimulate += this.Server_OnAfterSimulate;
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0002C81D File Offset: 0x0002AA1D
	private void OnDestroy()
	{
		PhysicsManager.OnAfterSimulate -= this.Server_OnAfterSimulate;
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0002C830 File Offset: 0x0002AA30
	private void Server_OnAfterSimulate(float deltaTime)
	{
		if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.tickInterval = deltaTime;
		if (!this.IsRecording)
		{
			return;
		}
		this.Server_Tick();
		this.recording.TickCount++;
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x0002C880 File Offset: 0x0002AA80
	public void Server_StartRecording()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (this.IsRecording)
		{
			return;
		}
		this.recording = new ReplayRecording();
		this.bindings.Clear();
		this.IsRecording = true;
		ReplayRecorder.Logger.Info("Replay recording started");
		this.Server_SeedFromWorld();
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x0002C8D5 File Offset: 0x0002AAD5
	public void Server_StopRecording()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsRecording)
		{
			return;
		}
		ReplayRecorder.Logger.Info("Replay recording stopped");
		this.IsRecording = false;
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x0002C903 File Offset: 0x0002AB03
	public ReplayRecording Server_TakeRecording()
	{
		ReplayRecording result = this.recording;
		this.recording = new ReplayRecording();
		this.bindings.Clear();
		if (this.IsRecording)
		{
			this.Server_SeedFromWorld();
		}
		return result;
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0002C92F File Offset: 0x0002AB2F
	public int Server_GetTicks(float seconds)
	{
		if (this.tickInterval <= 0f)
		{
			return 0;
		}
		return Mathf.RoundToInt(seconds / this.tickInterval);
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0002C94D File Offset: 0x0002AB4D
	private int Server_GetRetainedTicks()
	{
		if (this.retainedSeconds <= 0f)
		{
			return 0;
		}
		return this.Server_GetTicks(this.retainedSeconds);
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0002C96C File Offset: 0x0002AB6C
	private void Server_SeedFromWorld()
	{
		foreach (Player player in MonoBehaviourSingleton<PlayerManager>.Instance.GetPlayers(false))
		{
			this.Server_AddPlayerSpawnedEvent(player);
			if (player.PlayerBody)
			{
				this.Server_AddPlayerBodySpawnedEvent(player.PlayerBody);
			}
			if (player.Stick)
			{
				this.Server_AddStickSpawnedEvent(player.Stick);
			}
		}
		foreach (Puck puck in MonoBehaviourSingleton<PuckManager>.Instance.GetPucks(false))
		{
			this.Server_AddPuckSpawnedEvent(puck);
		}
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x0002CA40 File Offset: 0x0002AC40
	private void Server_Tick()
	{
		foreach (ReplayPoseBinding replayPoseBinding in this.bindings.PoseBindings)
		{
			if (replayPoseBinding.SynchronizedObject)
			{
				ReplayTrack<ReplayPoseSample> track = replayPoseBinding.Track;
				int tick = this.Tick;
				ReplayPoseSample replayPoseSample = ReplayRecorder.Server_GetPoseSample(replayPoseBinding.SynchronizedObject);
				track.Append(tick, replayPoseSample);
			}
		}
		foreach (ReplayPlayerBodyBinding replayPlayerBodyBinding in this.bindings.PlayerBodyBindings)
		{
			if (replayPlayerBodyBinding.PlayerBody && replayPlayerBodyBinding.PlayerBody.Player)
			{
				ReplayTrack<ReplayPlayerBodyStateSample> stateTrack = replayPlayerBodyBinding.StateTrack;
				int tick2 = this.Tick;
				ReplayPlayerBodyStateSample replayPlayerBodyStateSample = ReplayRecorder.Server_GetPlayerBodyStateSample(replayPlayerBodyBinding.PlayerBody);
				stateTrack.Append(tick2, replayPlayerBodyStateSample);
				ReplayTrack<ReplayPlayerInputSample> inputTrack = replayPlayerBodyBinding.InputTrack;
				int tick3 = this.Tick;
				ReplayPlayerInputSample replayPlayerInputSample = ReplayRecorder.Server_GetPlayerInputSample(replayPlayerBodyBinding.PlayerBody.Player);
				inputTrack.Append(tick3, replayPlayerInputSample);
			}
		}
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0002CB68 File Offset: 0x0002AD68
	private static ReplayPoseSample Server_GetPoseSample(SynchronizedObject synchronizedObject)
	{
		return new ReplayPoseSample
		{
			Position = synchronizedObject.Rigidbody.position,
			Rotation = synchronizedObject.Rigidbody.rotation,
			LinearVelocity = synchronizedObject.GetLinearVelocityAtPosition(),
			AngularVelocity = synchronizedObject.Rigidbody.angularVelocity
		};
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0002CBC4 File Offset: 0x0002ADC4
	private static ReplayPlayerBodyStateSample Server_GetPlayerBodyStateSample(PlayerBody playerBody)
	{
		return new ReplayPlayerBodyStateSample
		{
			Stamina = playerBody.Stamina.Value,
			Speed = playerBody.Speed.Value,
			IsSprinting = playerBody.IsSprinting.Value,
			IsSliding = playerBody.IsSliding.Value,
			IsStopping = playerBody.IsStopping.Value,
			IsExtendedLeft = playerBody.IsExtendedLeft.Value,
			IsExtendedRight = playerBody.IsExtendedRight.Value
		};
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x0002CC58 File Offset: 0x0002AE58
	private static ReplayPlayerInputSample Server_GetPlayerInputSample(Player player)
	{
		return new ReplayPlayerInputSample
		{
			LookAngleInput = player.PlayerInput.LookAngleInput.ServerValue,
			BladeAngleInput = player.PlayerInput.BladeAngleInput.ServerValue,
			TrackInput = player.PlayerInput.TrackInput.ServerValue,
			LookInput = player.PlayerInput.LookInput.ServerValue
		};
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x0002CCCA File Offset: 0x0002AECA
	public void Server_AddReplayEvent(string eventName, object eventData)
	{
		this.Server_AddReplayEventAtTick(this.Tick, eventName, eventData);
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x0002CCDA File Offset: 0x0002AEDA
	public void Server_AddReplayEventAtTick(int tick, string eventName, object eventData)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsRecording)
		{
			return;
		}
		if (tick < 0)
		{
			return;
		}
		this.recording.AddEvent(tick, eventName, eventData);
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x0002CD08 File Offset: 0x0002AF08
	public void Server_AddPlayerSpawnedEvent(Player player)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsRecording)
		{
			return;
		}
		this.Server_AddReplayEvent("PlayerSpawned", new ReplayPlayerSpawned
		{
			OwnerClientId = player.OwnerClientId,
			GameState = player.GameState.Value,
			CustomizationState = player.CustomizationState.Value,
			Handedness = player.Handedness.Value,
			SteamId = player.SteamId.Value,
			Username = player.Username.Value,
			Number = player.Number.Value,
			PatreonLevel = player.PatreonLevel.Value,
			AdminLevel = player.AdminLevel.Value,
			IsMuted = player.IsMuted.Value
		});
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x0002CDF4 File Offset: 0x0002AFF4
	public void Server_AddPlayerDespawnedEvent(Player player)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsRecording)
		{
			return;
		}
		if (player.PlayerBody)
		{
			this.bindings.RemovePlayerBody(player.PlayerBody);
		}
		if (player.Stick)
		{
			this.bindings.RemovePose(player.Stick.SynchronizedObject);
		}
		this.Server_AddReplayEvent("PlayerDespawned", new ReplayPlayerDespawned
		{
			OwnerClientId = player.OwnerClientId
		});
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x0002CE80 File Offset: 0x0002B080
	public void Server_AddPlayerBodySpawnedEvent(PlayerBody playerBody)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsRecording)
		{
			return;
		}
		int retainedTicks = this.Server_GetRetainedTicks();
		ReplayTrack<ReplayPoseSample> replayTrack = new ReplayTrack<ReplayPoseSample>(retainedTicks);
		ReplayTrack<ReplayPlayerBodyStateSample> stateTrack = new ReplayTrack<ReplayPlayerBodyStateSample>(retainedTicks);
		ReplayTrack<ReplayPlayerInputSample> inputTrack = new ReplayTrack<ReplayPlayerInputSample>(retainedTicks);
		this.bindings.AddPose(playerBody.SynchronizedObject, replayTrack);
		this.bindings.AddPlayerBody(playerBody, stateTrack, inputTrack);
		this.Server_AddReplayEvent("PlayerBodySpawned", new ReplayPlayerBodySpawned
		{
			OwnerClientId = playerBody.OwnerClientId,
			Position = playerBody.transform.position,
			Rotation = playerBody.transform.rotation,
			GameState = playerBody.Player.GameState.Value,
			CustomizationState = playerBody.Player.CustomizationState.Value,
			Username = playerBody.Player.Username.Value,
			Number = playerBody.Player.Number.Value,
			PoseTrack = replayTrack,
			StateTrack = stateTrack,
			InputTrack = inputTrack
		});
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x0002CF9C File Offset: 0x0002B19C
	public void Server_AddPlayerBodyDespawnedEvent(PlayerBody playerBody)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsRecording)
		{
			return;
		}
		this.bindings.RemovePlayerBody(playerBody);
		this.Server_AddReplayEvent("PlayerBodyDespawned", new ReplayPlayerBodyDespawned
		{
			OwnerClientId = playerBody.OwnerClientId
		});
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x0002CFF4 File Offset: 0x0002B1F4
	public void Server_AddStickSpawnedEvent(Stick stick)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsRecording)
		{
			return;
		}
		ReplayTrack<ReplayPoseSample> replayTrack = new ReplayTrack<ReplayPoseSample>(this.Server_GetRetainedTicks());
		this.bindings.AddPose(stick.SynchronizedObject, replayTrack);
		this.Server_AddReplayEvent("StickSpawned", new ReplayStickSpawned
		{
			OwnerClientId = stick.OwnerClientId,
			Position = stick.transform.position,
			Rotation = stick.transform.rotation,
			PoseTrack = replayTrack
		});
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x0002D088 File Offset: 0x0002B288
	public void Server_AddStickDespawnedEvent(Stick stick)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsRecording)
		{
			return;
		}
		this.bindings.RemovePose(stick.SynchronizedObject);
		this.Server_AddReplayEvent("StickDespawned", new ReplayStickDespawned
		{
			OwnerClientId = stick.OwnerClientId
		});
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x0002D0E4 File Offset: 0x0002B2E4
	public void Server_AddPuckSpawnedEvent(Puck puck)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsRecording)
		{
			return;
		}
		ReplayTrack<ReplayPoseSample> replayTrack = new ReplayTrack<ReplayPoseSample>(this.Server_GetRetainedTicks());
		this.bindings.AddPose(puck.SynchronizedObject, replayTrack);
		this.Server_AddReplayEvent("PuckSpawned", new ReplayPuckSpawned
		{
			NetworkObjectId = puck.NetworkObjectId,
			Position = puck.transform.position,
			Rotation = puck.transform.rotation,
			PoseTrack = replayTrack
		});
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x0002D178 File Offset: 0x0002B378
	public void Server_AddPuckDespawnedEvent(Puck puck)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsRecording)
		{
			return;
		}
		this.bindings.RemovePose(puck.SynchronizedObject);
		this.Server_AddReplayEvent("PuckDespawned", new ReplayPuckDespawned
		{
			NetworkObjectId = puck.NetworkObjectId
		});
	}

	// Token: 0x040005C3 RID: 1475
	private static readonly global::Logger Logger = new global::Logger("ReplayRecorder");

	// Token: 0x040005C4 RID: 1476
	[Header("Settings")]
	[Tooltip("Seconds of motion always available to play back. Older samples are dropped as recording continues, so memory stays flat however long a period runs. Must exceed the Replay phase duration, or a replay will show objects frozen at their spawn pose. Zero keeps the entire recording, which is what writing a whole match to a file will need.")]
	[SerializeField]
	private float retainedSeconds = 20f;

	// Token: 0x040005C5 RID: 1477
	[HideInInspector]
	public bool IsRecording;

	// Token: 0x040005C6 RID: 1478
	private float tickInterval;

	// Token: 0x040005C7 RID: 1479
	private ReplayRecording recording = new ReplayRecording();

	// Token: 0x040005C8 RID: 1480
	private ReplayBindings bindings = new ReplayBindings();
}
