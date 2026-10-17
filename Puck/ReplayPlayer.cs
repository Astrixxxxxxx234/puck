using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class ReplayPlayer : MonoBehaviour
{
	// Token: 0x060008E0 RID: 2272 RVA: 0x0002B748 File Offset: 0x00029948
	private void Awake()
	{
		PhysicsManager.OnBeforeSimulate += this.Server_OnBeforeSimulate;
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x0002B75B File Offset: 0x0002995B
	private void OnDestroy()
	{
		PhysicsManager.OnBeforeSimulate -= this.Server_OnBeforeSimulate;
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x0002B770 File Offset: 0x00029970
	private void Server_OnBeforeSimulate(float deltaTime)
	{
		if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsReplaying)
		{
			return;
		}
		if (this.tick >= this.recording.LastTick)
		{
			this.Server_StopReplay();
			return;
		}
		this.tick++;
		this.Server_ApplyEvents();
		this.Server_ApplyStateSamples();
		this.Server_DrivePoses();
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x0002B7DC File Offset: 0x000299DC
	public void Server_StartReplay(ReplayRecording replayRecording, int fromTick)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (this.IsReplaying)
		{
			return;
		}
		if (replayRecording.TickCount == 0)
		{
			return;
		}
		this.recording = replayRecording;
		this.tick = Mathf.Clamp(fromTick, 0, this.recording.LastTick);
		this.eventIndex = this.recording.GetIndexAtOrAfterTick(this.tick);
		this.IsReplaying = true;
		this.Server_SpawnWorldAtTick();
		this.Server_ApplyEvents();
		this.Server_ApplyStateSamples();
		this.Server_DrivePoses();
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x0002B85D File Offset: 0x00029A5D
	public void Server_StopReplay()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.IsReplaying)
		{
			return;
		}
		this.IsReplaying = false;
		if (NetworkBehaviourSingleton<GameManager>.Instance != null)
		{
			NetworkBehaviourSingleton<GameManager>.Instance.Server_StopSlowMotion(true);
		}
		this.Server_Dispose();
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x0002B89C File Offset: 0x00029A9C
	private void Server_ApplyEvents()
	{
		while (this.eventIndex < this.recording.Events.Count && this.recording.Events[this.eventIndex].Tick <= this.tick)
		{
			ReplayDiscreteEvent replayDiscreteEvent = this.recording.Events[this.eventIndex];
			this.eventIndex++;
			this.Server_ReplayEvent(replayDiscreteEvent.Name, replayDiscreteEvent.Data);
		}
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x0002B920 File Offset: 0x00029B20
	private void Server_ApplyStateSamples()
	{
		foreach (ReplayPlayerBodyBinding replayPlayerBodyBinding in this.bindings.PlayerBodyBindings)
		{
			if (replayPlayerBodyBinding.PlayerBody && replayPlayerBodyBinding.PlayerBody.Player)
			{
				ReplayPlayerBodyStateSample replayPlayerBodyStateSample;
				if (replayPlayerBodyBinding.StateTrack.TryGetAtOrBefore(this.tick, out replayPlayerBodyStateSample))
				{
					ReplayPlayer.Server_ApplyPlayerBodyState(replayPlayerBodyBinding.PlayerBody, replayPlayerBodyStateSample);
				}
				ReplayPlayerInputSample replayPlayerInputSample;
				if (replayPlayerBodyBinding.InputTrack.TryGetAtOrBefore(this.tick, out replayPlayerInputSample))
				{
					ReplayPlayer.Server_ApplyPlayerInput(replayPlayerBodyBinding.PlayerBody.Player, replayPlayerInputSample);
				}
			}
		}
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x0002B9DC File Offset: 0x00029BDC
	private void Server_DrivePoses()
	{
		foreach (ReplayPoseBinding replayPoseBinding in this.bindings.PoseBindings)
		{
			ReplayPoseSample replayPoseSample;
			if (replayPoseBinding.SynchronizedObject && replayPoseBinding.Track.TryGetAtOrBefore(this.tick, out replayPoseSample))
			{
				SynchronizedObject synchronizedObject = replayPoseBinding.SynchronizedObject;
				SynchronizedObjectPose synchronizedObjectPose = ReplayPlayer.Server_GetPose(replayPoseSample);
				synchronizedObject.SetDrivenPose(synchronizedObjectPose);
			}
		}
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x0002BA64 File Offset: 0x00029C64
	private static SynchronizedObjectPose Server_GetPose(in ReplayPoseSample poseSample)
	{
		return new SynchronizedObjectPose
		{
			Position = poseSample.Position,
			Rotation = poseSample.Rotation,
			LinearVelocity = poseSample.LinearVelocity,
			AngularVelocity = poseSample.AngularVelocity
		};
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x0002BAB0 File Offset: 0x00029CB0
	private static void Server_ApplyPlayerBodyState(PlayerBody playerBody, in ReplayPlayerBodyStateSample stateSample)
	{
		playerBody.Stamina.Value = stateSample.Stamina;
		playerBody.Speed.Value = stateSample.Speed;
		playerBody.IsSprinting.Value = stateSample.IsSprinting;
		playerBody.IsSliding.Value = stateSample.IsSliding;
		playerBody.IsStopping.Value = stateSample.IsStopping;
		playerBody.IsExtendedLeft.Value = stateSample.IsExtendedLeft;
		playerBody.IsExtendedRight.Value = stateSample.IsExtendedRight;
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x0002BB34 File Offset: 0x00029D34
	private static void Server_ApplyPlayerInput(Player player, in ReplayPlayerInputSample inputSample)
	{
		player.PlayerInput.LookAngleInput.ClientValue = inputSample.LookAngleInput;
		player.PlayerInput.BladeAngleInput.ClientValue = inputSample.BladeAngleInput;
		player.PlayerInput.TrackInput.ClientValue = inputSample.TrackInput;
		player.PlayerInput.LookInput.ClientValue = inputSample.LookInput;
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x0002BB9C File Offset: 0x00029D9C
	private void Server_SpawnWorldAtTick()
	{
		this.Server_CollectSurvivors();
		this.Server_SpawnSurvivingPlayers();
		this.Server_SpawnSurvivingPlayerBodies();
		this.Server_SpawnSurvivingSticks();
		this.Server_SpawnSurvivingPucks();
		this.seekPlayers.Clear();
		this.seekPlayerBodies.Clear();
		this.seekSticks.Clear();
		this.seekPucks.Clear();
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x0002BBF4 File Offset: 0x00029DF4
	private void Server_CollectSurvivors()
	{
		for (int i = 0; i < this.eventIndex; i++)
		{
			ReplayDiscreteEvent replayDiscreteEvent = this.recording.Events[i];
			this.Server_CollectSurvivorEvent(replayDiscreteEvent.Name, replayDiscreteEvent.Data);
		}
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0002BC38 File Offset: 0x00029E38
	private void Server_CollectSurvivorEvent(string eventName, object eventData)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(eventName);
		if (num <= 1238133331U)
		{
			if (num <= 541464130U)
			{
				if (num != 151785130U)
				{
					if (num != 541464130U)
					{
						return;
					}
					if (!(eventName == "PlayerBodySpawned"))
					{
						return;
					}
					ReplayPlayerBodySpawned replayPlayerBodySpawned = (ReplayPlayerBodySpawned)eventData;
					this.seekPlayerBodies[replayPlayerBodySpawned.OwnerClientId] = replayPlayerBodySpawned;
					return;
				}
				else
				{
					if (!(eventName == "PuckSpawned"))
					{
						return;
					}
					ReplayPuckSpawned replayPuckSpawned = (ReplayPuckSpawned)eventData;
					this.seekPucks[replayPuckSpawned.NetworkObjectId] = replayPuckSpawned;
					return;
				}
			}
			else if (num != 921486523U)
			{
				if (num != 1238133331U)
				{
					return;
				}
				if (!(eventName == "StickSpawned"))
				{
					return;
				}
				ReplayStickSpawned replayStickSpawned = (ReplayStickSpawned)eventData;
				this.seekSticks[replayStickSpawned.OwnerClientId] = replayStickSpawned;
				return;
			}
			else
			{
				if (!(eventName == "PuckDespawned"))
				{
					return;
				}
				ReplayPuckDespawned replayPuckDespawned = (ReplayPuckDespawned)eventData;
				this.seekPucks.Remove(replayPuckDespawned.NetworkObjectId);
				return;
			}
		}
		else if (num <= 1946187694U)
		{
			if (num != 1769383370U)
			{
				if (num != 1946187694U)
				{
					return;
				}
				if (!(eventName == "PlayerSpawned"))
				{
					return;
				}
				ReplayPlayerSpawned replayPlayerSpawned = (ReplayPlayerSpawned)eventData;
				this.seekPlayers[replayPlayerSpawned.OwnerClientId] = replayPlayerSpawned;
				return;
			}
			else
			{
				if (!(eventName == "StickDespawned"))
				{
					return;
				}
				ReplayStickDespawned replayStickDespawned = (ReplayStickDespawned)eventData;
				this.seekSticks.Remove(replayStickDespawned.OwnerClientId);
				return;
			}
		}
		else if (num != 3342882255U)
		{
			if (num != 3490707091U)
			{
				return;
			}
			if (!(eventName == "PlayerBodyDespawned"))
			{
				return;
			}
			ReplayPlayerBodyDespawned replayPlayerBodyDespawned = (ReplayPlayerBodyDespawned)eventData;
			this.seekPlayerBodies.Remove(replayPlayerBodyDespawned.OwnerClientId);
			return;
		}
		else
		{
			if (!(eventName == "PlayerDespawned"))
			{
				return;
			}
			ReplayPlayerDespawned replayPlayerDespawned = (ReplayPlayerDespawned)eventData;
			this.seekPlayers.Remove(replayPlayerDespawned.OwnerClientId);
			this.seekPlayerBodies.Remove(replayPlayerDespawned.OwnerClientId);
			this.seekSticks.Remove(replayPlayerDespawned.OwnerClientId);
			return;
		}
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x0002BE34 File Offset: 0x0002A034
	private void Server_SpawnSurvivingPlayers()
	{
		foreach (KeyValuePair<ulong, ReplayPlayerSpawned> keyValuePair in this.seekPlayers)
		{
			ReplayPlayerSpawned value = keyValuePair.Value;
			this.Server_ReplayPlayerSpawned(value);
		}
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x0002BE90 File Offset: 0x0002A090
	private void Server_SpawnSurvivingPlayerBodies()
	{
		foreach (KeyValuePair<ulong, ReplayPlayerBodySpawned> keyValuePair in this.seekPlayerBodies)
		{
			ReplayPlayerBodySpawned value = keyValuePair.Value;
			ReplayPoseSample replayPoseSample;
			if (this.Server_TryGetPoseSampleAtTick(value.PoseTrack, out replayPoseSample))
			{
				value.Position = replayPoseSample.Position;
				value.Rotation = replayPoseSample.Rotation;
			}
			this.Server_ReplayPlayerBodySpawned(value);
		}
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x0002BF18 File Offset: 0x0002A118
	private void Server_SpawnSurvivingSticks()
	{
		foreach (KeyValuePair<ulong, ReplayStickSpawned> keyValuePair in this.seekSticks)
		{
			ReplayStickSpawned value = keyValuePair.Value;
			ReplayPoseSample replayPoseSample;
			if (this.Server_TryGetPoseSampleAtTick(value.PoseTrack, out replayPoseSample))
			{
				value.Position = replayPoseSample.Position;
				value.Rotation = replayPoseSample.Rotation;
			}
			this.Server_ReplayStickSpawned(value);
		}
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x0002BFA0 File Offset: 0x0002A1A0
	private void Server_SpawnSurvivingPucks()
	{
		foreach (KeyValuePair<ulong, ReplayPuckSpawned> keyValuePair in this.seekPucks)
		{
			ReplayPuckSpawned value = keyValuePair.Value;
			ReplayPoseSample replayPoseSample;
			if (this.Server_TryGetPoseSampleAtTick(value.PoseTrack, out replayPoseSample))
			{
				value.Position = replayPoseSample.Position;
				value.Rotation = replayPoseSample.Rotation;
			}
			this.Server_ReplayPuckSpawned(value);
		}
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x0002C028 File Offset: 0x0002A228
	private bool Server_TryGetPoseSampleAtTick(ReplayTrack<ReplayPoseSample> poseTrack, out ReplayPoseSample poseSample)
	{
		poseSample = default(ReplayPoseSample);
		return poseTrack != null && poseTrack.TryGetAtOrBefore(this.tick, out poseSample);
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x0002C044 File Offset: 0x0002A244
	private void Server_ReplayEvent(string eventName, object eventData)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(eventName);
		if (num <= 1769383370U)
		{
			if (num <= 541464130U)
			{
				if (num != 151785130U)
				{
					if (num != 541464130U)
					{
						return;
					}
					if (!(eventName == "PlayerBodySpawned"))
					{
						return;
					}
					ReplayPlayerBodySpawned replayPlayerBodySpawned = (ReplayPlayerBodySpawned)eventData;
					this.Server_ReplayPlayerBodySpawned(replayPlayerBodySpawned);
					return;
				}
				else
				{
					if (!(eventName == "PuckSpawned"))
					{
						return;
					}
					ReplayPuckSpawned replayPuckSpawned = (ReplayPuckSpawned)eventData;
					this.Server_ReplayPuckSpawned(replayPuckSpawned);
					return;
				}
			}
			else if (num != 921486523U)
			{
				if (num != 1238133331U)
				{
					if (num != 1769383370U)
					{
						return;
					}
					if (!(eventName == "StickDespawned"))
					{
						return;
					}
					ReplayStickDespawned replayStickDespawned = (ReplayStickDespawned)eventData;
					this.Server_ReplayStickDespawned(replayStickDespawned);
					return;
				}
				else
				{
					if (!(eventName == "StickSpawned"))
					{
						return;
					}
					ReplayStickSpawned replayStickSpawned = (ReplayStickSpawned)eventData;
					this.Server_ReplayStickSpawned(replayStickSpawned);
					return;
				}
			}
			else
			{
				if (!(eventName == "PuckDespawned"))
				{
					return;
				}
				ReplayPuckDespawned replayPuckDespawned = (ReplayPuckDespawned)eventData;
				this.Server_ReplayPuckDespawned(replayPuckDespawned);
				return;
			}
		}
		else if (num <= 2169208330U)
		{
			if (num != 1946187694U)
			{
				if (num != 2169208330U)
				{
					return;
				}
				if (!(eventName == "SlowMotion"))
				{
					return;
				}
				ReplaySlowMotion replaySlowMotion = (ReplaySlowMotion)eventData;
				ReplayPlayer.Server_ReplaySlowMotion(replaySlowMotion);
				return;
			}
			else
			{
				if (!(eventName == "PlayerSpawned"))
				{
					return;
				}
				ReplayPlayerSpawned replayPlayerSpawned = (ReplayPlayerSpawned)eventData;
				this.Server_ReplayPlayerSpawned(replayPlayerSpawned);
				return;
			}
		}
		else if (num != 3265500691U)
		{
			if (num != 3342882255U)
			{
				if (num != 3490707091U)
				{
					return;
				}
				if (!(eventName == "PlayerBodyDespawned"))
				{
					return;
				}
				ReplayPlayerBodyDespawned replayPlayerBodyDespawned = (ReplayPlayerBodyDespawned)eventData;
				this.Server_ReplayPlayerBodyDespawned(replayPlayerBodyDespawned);
				return;
			}
			else
			{
				if (!(eventName == "PlayerDespawned"))
				{
					return;
				}
				ReplayPlayerDespawned replayPlayerDespawned = (ReplayPlayerDespawned)eventData;
				this.Server_ReplayPlayerDespawned(replayPlayerDespawned);
				return;
			}
		}
		else
		{
			if (!(eventName == "CellyCam"))
			{
				return;
			}
			ReplayCellyCam replayCellyCam = (ReplayCellyCam)eventData;
			ReplayPlayer.Server_ReplayCellyCam(replayCellyCam);
			return;
		}
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x0002C224 File Offset: 0x0002A424
	private void Server_ReplayPlayerSpawned(in ReplayPlayerSpawned playerSpawned)
	{
		PlayerManager instance = MonoBehaviourSingleton<PlayerManager>.Instance;
		ulong ownerClientId = playerSpawned.OwnerClientId;
		PlayerGameState gameState = playerSpawned.GameState;
		PlayerCustomizationState customizationState = playerSpawned.CustomizationState;
		PlayerHandedness handedness = playerSpawned.Handedness;
		FixedString32Bytes fixedString32Bytes = playerSpawned.SteamId;
		string steamID = fixedString32Bytes.ToString();
		fixedString32Bytes = playerSpawned.Username;
		instance.Server_SpawnPlayer(ownerClientId, gameState, customizationState, handedness, steamID, fixedString32Bytes.ToString(), playerSpawned.Number, playerSpawned.PatreonLevel, playerSpawned.AdminLevel, playerSpawned.IsMuted, true);
		Player replayPlayerByClientId = MonoBehaviourSingleton<PlayerManager>.Instance.GetReplayPlayerByClientId(playerSpawned.OwnerClientId);
		if (!replayPlayerByClientId)
		{
			return;
		}
		this.playersByRecordedClientId[playerSpawned.OwnerClientId] = replayPlayerByClientId;
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x0002C2C0 File Offset: 0x0002A4C0
	private void Server_ReplayPlayerDespawned(in ReplayPlayerDespawned playerDespawned)
	{
		Player player;
		if (!this.Server_TryGetReplayPlayer(playerDespawned.OwnerClientId, out player))
		{
			return;
		}
		this.playersByRecordedClientId.Remove(playerDespawned.OwnerClientId);
		if (player.PlayerBody)
		{
			this.bindings.RemovePlayerBody(player.PlayerBody);
		}
		if (player.Stick)
		{
			this.bindings.RemovePose(player.Stick.SynchronizedObject);
		}
		player.NetworkObject.Despawn(true);
		ReplayPlayer.Logger.Info(string.Format("Despawned replay player {0}", player.OwnerClientId));
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x0002C35C File Offset: 0x0002A55C
	private void Server_ReplayPlayerBodySpawned(in ReplayPlayerBodySpawned playerBodySpawned)
	{
		Player player;
		if (!this.Server_TryGetReplayPlayer(playerBodySpawned.OwnerClientId, out player))
		{
			return;
		}
		player.GameState.Value = playerBodySpawned.GameState;
		player.CustomizationState.Value = playerBodySpawned.CustomizationState;
		player.Username.Value = playerBodySpawned.Username;
		player.Number.Value = playerBodySpawned.Number;
		player.Server_SpawnPlayerBody(playerBodySpawned.Position, playerBodySpawned.Rotation, player.Role);
		if (!player.PlayerBody)
		{
			return;
		}
		this.Server_BindDrivenPose(player.PlayerBody.SynchronizedObject, playerBodySpawned.PoseTrack);
		this.bindings.AddPlayerBody(player.PlayerBody, playerBodySpawned.StateTrack, playerBodySpawned.InputTrack);
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x0002C418 File Offset: 0x0002A618
	private void Server_ReplayPlayerBodyDespawned(in ReplayPlayerBodyDespawned playerBodyDespawned)
	{
		Player player;
		if (!this.Server_TryGetReplayPlayer(playerBodyDespawned.OwnerClientId, out player))
		{
			return;
		}
		if (!player.PlayerBody)
		{
			return;
		}
		this.bindings.RemovePlayerBody(player.PlayerBody);
		player.Server_DespawnPlayerBody();
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0002C45C File Offset: 0x0002A65C
	private void Server_ReplayStickSpawned(in ReplayStickSpawned stickSpawned)
	{
		Player player;
		if (!this.Server_TryGetReplayPlayer(stickSpawned.OwnerClientId, out player))
		{
			return;
		}
		player.Server_SpawnStick(stickSpawned.Position, stickSpawned.Rotation, player.Role);
		if (!player.Stick)
		{
			return;
		}
		this.Server_BindDrivenPose(player.Stick.SynchronizedObject, stickSpawned.PoseTrack);
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0002C4B8 File Offset: 0x0002A6B8
	private void Server_ReplayStickDespawned(in ReplayStickDespawned stickDespawned)
	{
		Player player;
		if (!this.Server_TryGetReplayPlayer(stickDespawned.OwnerClientId, out player))
		{
			return;
		}
		if (!player.Stick)
		{
			return;
		}
		this.bindings.RemovePose(player.Stick.SynchronizedObject);
		player.Server_DespawnStick();
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0002C500 File Offset: 0x0002A700
	private void Server_ReplayPuckSpawned(in ReplayPuckSpawned puckSpawned)
	{
		Puck puck = MonoBehaviourSingleton<PuckManager>.Instance.Server_SpawnPuck(puckSpawned.Position, puckSpawned.Rotation, true);
		if (!puck)
		{
			return;
		}
		this.pucksByRecordedNetworkObjectId[puckSpawned.NetworkObjectId] = puck;
		this.Server_BindDrivenPose(puck.SynchronizedObject, puckSpawned.PoseTrack);
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0002C554 File Offset: 0x0002A754
	private void Server_ReplayPuckDespawned(in ReplayPuckDespawned puckDespawned)
	{
		Puck puck;
		if (!this.pucksByRecordedNetworkObjectId.TryGetValue(puckDespawned.NetworkObjectId, out puck) || !puck)
		{
			return;
		}
		this.pucksByRecordedNetworkObjectId.Remove(puckDespawned.NetworkObjectId);
		this.bindings.RemovePose(puck.SynchronizedObject);
		MonoBehaviourSingleton<PuckManager>.Instance.Server_DespawnPuck(puck);
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x0002C5AD File Offset: 0x0002A7AD
	private static void Server_ReplaySlowMotion(in ReplaySlowMotion slowMotion)
	{
		if (NetworkBehaviourSingleton<GameManager>.Instance == null)
		{
			return;
		}
		NetworkBehaviourSingleton<GameManager>.Instance.Server_StartSlowMotion(slowMotion.Scale, slowMotion.RampInSeconds, slowMotion.HoldSeconds, slowMotion.RampOutSeconds, slowMotion.AudioPitchFloor, slowMotion.CameraFovPunch);
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0002C5EB File Offset: 0x0002A7EB
	private static void Server_ReplayCellyCam(in ReplayCellyCam cellyCam)
	{
		if (NetworkBehaviourSingleton<GameManager>.Instance == null)
		{
			return;
		}
		NetworkBehaviourSingleton<GameManager>.Instance.Server_NotifyReplayCellyCamRpc(cellyCam.OwnerClientId);
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0002C60B File Offset: 0x0002A80B
	private void Server_BindDrivenPose(SynchronizedObject synchronizedObject, ReplayTrack<ReplayPoseSample> poseTrack)
	{
		this.bindings.AddPose(synchronizedObject, poseTrack);
		synchronizedObject.DriveExternally(SynchronizedObjectDriveCadence.PerTick);
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0002C621 File Offset: 0x0002A821
	private bool Server_TryGetReplayPlayer(ulong recordedClientId, out Player player)
	{
		if (!this.playersByRecordedClientId.TryGetValue(recordedClientId, out player) || !player)
		{
			player = null;
			return false;
		}
		return true;
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0002C641 File Offset: 0x0002A841
	private void Server_Dispose()
	{
		this.Server_DespawnReplayObjects();
		this.recording = null;
		this.tick = 0;
		this.eventIndex = 0;
		this.bindings.Clear();
		this.playersByRecordedClientId.Clear();
		this.pucksByRecordedNetworkObjectId.Clear();
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x0002C680 File Offset: 0x0002A880
	private void Server_DespawnReplayObjects()
	{
		foreach (Player player in MonoBehaviourSingleton<PlayerManager>.Instance.GetReplayPlayers())
		{
			if (player.PlayerBody)
			{
				player.Server_DespawnPlayerBody();
			}
			if (player.Stick)
			{
				player.Server_DespawnStick();
			}
			player.NetworkObject.Despawn(true);
			ReplayPlayer.Logger.Info(string.Format("Despawned replay player {0}", player.OwnerClientId));
		}
		foreach (Puck puck in MonoBehaviourSingleton<PuckManager>.Instance.GetReplayPucks())
		{
			MonoBehaviourSingleton<PuckManager>.Instance.Server_DespawnPuck(puck);
			ReplayPlayer.Logger.Info(string.Format("Despawned replay puck {0}", puck.OwnerClientId));
		}
	}

	// Token: 0x040005A3 RID: 1443
	private static readonly global::Logger Logger = new global::Logger("ReplayPlayer");

	// Token: 0x040005A4 RID: 1444
	[HideInInspector]
	public bool IsReplaying;

	// Token: 0x040005A5 RID: 1445
	private ReplayRecording recording;

	// Token: 0x040005A6 RID: 1446
	private int tick;

	// Token: 0x040005A7 RID: 1447
	private int eventIndex;

	// Token: 0x040005A8 RID: 1448
	private ReplayBindings bindings = new ReplayBindings();

	// Token: 0x040005A9 RID: 1449
	private Dictionary<ulong, Player> playersByRecordedClientId = new Dictionary<ulong, Player>();

	// Token: 0x040005AA RID: 1450
	private Dictionary<ulong, Puck> pucksByRecordedNetworkObjectId = new Dictionary<ulong, Puck>();

	// Token: 0x040005AB RID: 1451
	private Dictionary<ulong, ReplayPlayerSpawned> seekPlayers = new Dictionary<ulong, ReplayPlayerSpawned>();

	// Token: 0x040005AC RID: 1452
	private Dictionary<ulong, ReplayPlayerBodySpawned> seekPlayerBodies = new Dictionary<ulong, ReplayPlayerBodySpawned>();

	// Token: 0x040005AD RID: 1453
	private Dictionary<ulong, ReplayStickSpawned> seekSticks = new Dictionary<ulong, ReplayStickSpawned>();

	// Token: 0x040005AE RID: 1454
	private Dictionary<ulong, ReplayPuckSpawned> seekPucks = new Dictionary<ulong, ReplayPuckSpawned>();
}
