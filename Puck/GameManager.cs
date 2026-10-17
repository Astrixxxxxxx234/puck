using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class GameManager : NetworkBehaviourSingleton<GameManager>
{
	// Token: 0x17000096 RID: 150
	// (get) Token: 0x06000649 RID: 1609 RVA: 0x00022BE7 File Offset: 0x00020DE7
	[HideInInspector]
	public GamePhase Phase
	{
		get
		{
			return this.GameState.Value.Phase;
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x0600064A RID: 1610 RVA: 0x00022BF9 File Offset: 0x00020DF9
	[HideInInspector]
	public int Tick
	{
		get
		{
			return this.GameState.Value.Tick;
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x0600064B RID: 1611 RVA: 0x00022C0B File Offset: 0x00020E0B
	[HideInInspector]
	public int Period
	{
		get
		{
			return this.GameState.Value.Period;
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x0600064C RID: 1612 RVA: 0x00022C1D File Offset: 0x00020E1D
	[HideInInspector]
	public int BlueScore
	{
		get
		{
			return this.GameState.Value.BlueScore;
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x0600064D RID: 1613 RVA: 0x00022C2F File Offset: 0x00020E2F
	[HideInInspector]
	public int RedScore
	{
		get
		{
			return this.GameState.Value.RedScore;
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x0600064E RID: 1614 RVA: 0x00022C41 File Offset: 0x00020E41
	[HideInInspector]
	public bool IsOvertime
	{
		get
		{
			return this.GameState.Value.IsOvertime;
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x0600064F RID: 1615 RVA: 0x00022C53 File Offset: 0x00020E53
	public bool IsTicking
	{
		get
		{
			return this.tickTween != null && this.tickTween.IsActive();
		}
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x00022C6C File Offset: 0x00020E6C
	protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
	{
		if (this.GameState == null)
		{
			this.GameState = new NetworkVariable<GameState>(default(GameState), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		}
		if (networkManager.IsServer)
		{
			this.GameState.Value = default(GameState);
		}
		base.OnNetworkPreSpawn(ref networkManager);
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x00022CBB File Offset: 0x00020EBB
	public override void OnNetworkSpawn()
	{
		NetworkVariable<GameState> gameState = this.GameState;
		gameState.OnValueChanged = (NetworkVariable<GameState>.OnValueChangedDelegate)Delegate.Combine(gameState.OnValueChanged, new NetworkVariable<GameState>.OnValueChangedDelegate(this.OnGameStateChanged));
		base.OnNetworkSpawn();
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x00022CEA File Offset: 0x00020EEA
	protected override void OnNetworkPostSpawn()
	{
		if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsConnectedClient)
		{
			this.ProcessInitialNetworkVariableValues();
		}
		base.OnNetworkPostSpawn();
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x00022D10 File Offset: 0x00020F10
	protected override void OnNetworkSessionSynchronized()
	{
		this.ProcessInitialNetworkVariableValues();
		base.OnNetworkSessionSynchronized();
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x00022D20 File Offset: 0x00020F20
	public override void OnNetworkDespawn()
	{
		if (NetworkManager.Singleton.IsServer)
		{
			Tween tween = this.tickTween;
			if (tween != null)
			{
				tween.Kill(false);
			}
			this.Server_StopSlowMotion(false);
		}
		NetworkVariable<GameState> gameState = this.GameState;
		gameState.OnValueChanged = (NetworkVariable<GameState>.OnValueChangedDelegate)Delegate.Remove(gameState.OnValueChanged, new NetworkVariable<GameState>.OnValueChangedDelegate(this.OnGameStateChanged));
		base.OnNetworkDespawn();
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x00022D80 File Offset: 0x00020F80
	private void ProcessInitialNetworkVariableValues()
	{
		this.OnGameStateChanged(default(GameState), this.GameState.Value);
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x00022DA7 File Offset: 0x00020FA7
	private void OnGameStateChanged(GameState oldGameState, GameState newGameState)
	{
		EventManager.TriggerEvent("Event_Everyone_OnGameStateChanged", new Dictionary<string, object>
		{
			{
				"oldGameState",
				oldGameState
			},
			{
				"newGameState",
				newGameState
			}
		});
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x00022DDC File Offset: 0x00020FDC
	private void Server_Tick()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (this.GameState.Value.Tick <= 0)
		{
			this.Server_ExpirePhase();
			return;
		}
		int? tick = new int?(this.GameState.Value.Tick - 1);
		this.Server_SetGameState(null, tick, null, null, null, null);
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x00022E5D File Offset: 0x0002105D
	public void Server_ExpirePhase()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		EventManager.TriggerEvent("Event_Server_OnPhaseExpired", new Dictionary<string, object>
		{
			{
				"phase",
				this.GameState.Value.Phase
			}
		});
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x00022E9C File Offset: 0x0002109C
	public void Server_StartTicking()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Tween tween = this.tickTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.tickTween = DOVirtual.DelayedCall(1f, new TweenCallback(this.Server_Tick), true).SetLoops(-1);
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x00022EEB File Offset: 0x000210EB
	public void Server_StopTicking()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Tween tween = this.tickTween;
		if (tween == null)
		{
			return;
		}
		tween.Kill(false);
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x00022F0C File Offset: 0x0002110C
	public void Server_StartSlowMotion(float scale, float rampInSeconds, float holdSeconds, float rampOutSeconds, float audioPitchFloor = 1f, float cameraFovPunch = 0f)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		float timeScale = Time.timeScale;
		Sequence sequence = this.slowMotionSequence;
		if (sequence != null)
		{
			sequence.Kill(false);
		}
		scale = Mathf.Clamp(scale, 0.01f, 1f);
		this.slowMotionSequence = TweenUtils.RampHoldRamp(timeScale, scale, 1f, rampInSeconds, holdSeconds, rampOutSeconds, Ease.InQuad, Ease.OutQuad, delegate(float value)
		{
			Time.timeScale = value;
		}).SetTarget(this);
		this.slowMotionSequence.OnKill(delegate
		{
			this.slowMotionSequence = null;
		});
		this.Server_NotifySlowMotionRpc(scale, rampInSeconds, holdSeconds, rampOutSeconds, audioPitchFloor, cameraFovPunch);
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x00022FB4 File Offset: 0x000211B4
	[Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	private void Server_NotifySlowMotionRpc(float scale, float rampInSeconds, float holdSeconds, float rampOutSeconds, float audioPitchFloor, float cameraFovPunch)
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 1445539067U;
			RpcParams rpcParams2;
			RpcParams rpcParams = rpcParams2;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Everyone, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<float>(scale, default(FastBufferWriter.ForPrimitives));
			fastBufferWriter.WriteValueSafe<float>(rampInSeconds, default(FastBufferWriter.ForPrimitives));
			fastBufferWriter.WriteValueSafe<float>(holdSeconds, default(FastBufferWriter.ForPrimitives));
			fastBufferWriter.WriteValueSafe<float>(rampOutSeconds, default(FastBufferWriter.ForPrimitives));
			fastBufferWriter.WriteValueSafe<float>(audioPitchFloor, default(FastBufferWriter.ForPrimitives));
			fastBufferWriter.WriteValueSafe<float>(cameraFovPunch, default(FastBufferWriter.ForPrimitives));
			base.__endSendRpc(ref fastBufferWriter, 1445539067U, rpcParams2, attributeParams, SendTo.Everyone, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		EventManager.TriggerEvent("Event_Everyone_OnSlowMotionStarted", new Dictionary<string, object>
		{
			{
				"scale",
				scale
			},
			{
				"rampInSeconds",
				rampInSeconds
			},
			{
				"holdSeconds",
				holdSeconds
			},
			{
				"rampOutSeconds",
				rampOutSeconds
			},
			{
				"audioPitchFloor",
				audioPitchFloor
			},
			{
				"cameraFovPunch",
				cameraFovPunch
			}
		});
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x000231A8 File Offset: 0x000213A8
	[Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_NotifyReplayCellyCamRpc(ulong scorerClientId)
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 3581845443U;
			RpcParams rpcParams2;
			RpcParams rpcParams = rpcParams2;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter writer = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Everyone, RpcDelivery.Reliable);
			BytePacker.WriteValueBitPacked(writer, scorerClientId);
			base.__endSendRpc(ref writer, 3581845443U, rpcParams2, attributeParams, SendTo.Everyone, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		EventManager.TriggerEvent("Event_Everyone_OnReplayCellyCam", new Dictionary<string, object>
		{
			{
				"scorerClientId",
				scorerClientId
			}
		});
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x000232B0 File Offset: 0x000214B0
	public void Server_StopSlowMotion(bool notifyClients = true)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		bool flag = this.slowMotionSequence != null;
		Sequence sequence = this.slowMotionSequence;
		if (sequence != null)
		{
			sequence.Kill(false);
		}
		Time.timeScale = 1f;
		if (notifyClients && flag)
		{
			this.Server_NotifySlowMotionStopRpc();
		}
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x000232FC File Offset: 0x000214FC
	[Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	private void Server_NotifySlowMotionStopRpc()
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 2324051025U;
			RpcParams rpcParams2;
			RpcParams rpcParams = rpcParams2;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Everyone, RpcDelivery.Reliable);
			base.__endSendRpc(ref fastBufferWriter, 2324051025U, rpcParams2, attributeParams, SendTo.Everyone, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		EventManager.TriggerEvent("Event_Everyone_OnSlowMotionStopped", new Dictionary<string, object>());
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x000233E4 File Offset: 0x000215E4
	public void Server_SetGameState(GamePhase? phase = null, int? tick = null, int? period = null, int? blueScore = null, int? redScore = null, bool? isOvertime = null)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		GameState value = new GameState
		{
			Phase = (phase ?? this.GameState.Value.Phase),
			Tick = (tick ?? this.GameState.Value.Tick),
			Period = (period ?? this.GameState.Value.Period),
			BlueScore = (blueScore ?? this.GameState.Value.BlueScore),
			RedScore = (redScore ?? this.GameState.Value.RedScore),
			IsOvertime = (isOvertime ?? this.GameState.Value.IsOvertime)
		};
		this.GameState.Value = value;
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x0002351C File Offset: 0x0002171C
	[Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_NotifyGoalScoredRpc(PlayerTeam byTeam, NetworkObjectReference goalPlayerNetworkObjectReference, NetworkObjectReference assistPlayerNetworkObjectReference, NetworkObjectReference secondAssistPlayerNetworkObjectReference, NetworkObjectReference puckNetworkObjectReference)
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 1809670267U;
			RpcParams rpcParams2;
			RpcParams rpcParams = rpcParams2;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Everyone, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<PlayerTeam>(byTeam, default(FastBufferWriter.ForEnums));
			fastBufferWriter.WriteValueSafe<NetworkObjectReference>(goalPlayerNetworkObjectReference, default(FastBufferWriter.ForNetworkSerializable));
			fastBufferWriter.WriteValueSafe<NetworkObjectReference>(assistPlayerNetworkObjectReference, default(FastBufferWriter.ForNetworkSerializable));
			fastBufferWriter.WriteValueSafe<NetworkObjectReference>(secondAssistPlayerNetworkObjectReference, default(FastBufferWriter.ForNetworkSerializable));
			fastBufferWriter.WriteValueSafe<NetworkObjectReference>(puckNetworkObjectReference, default(FastBufferWriter.ForNetworkSerializable));
			base.__endSendRpc(ref fastBufferWriter, 1809670267U, rpcParams2, attributeParams, SendTo.Everyone, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		Player playerFromNetworkObjectReference = NetworkingUtils.GetPlayerFromNetworkObjectReference(goalPlayerNetworkObjectReference);
		Player playerFromNetworkObjectReference2 = NetworkingUtils.GetPlayerFromNetworkObjectReference(assistPlayerNetworkObjectReference);
		Player playerFromNetworkObjectReference3 = NetworkingUtils.GetPlayerFromNetworkObjectReference(secondAssistPlayerNetworkObjectReference);
		Puck puckFromNetworkObjectReference = NetworkingUtils.GetPuckFromNetworkObjectReference(puckNetworkObjectReference);
		EventManager.TriggerEvent("Event_Everyone_OnGoalScored", new Dictionary<string, object>
		{
			{
				"byTeam",
				byTeam
			},
			{
				"goalPlayer",
				playerFromNetworkObjectReference
			},
			{
				"assistPlayer",
				playerFromNetworkObjectReference2
			},
			{
				"secondAssistPlayer",
				playerFromNetworkObjectReference3
			},
			{
				"puck",
				puckFromNetworkObjectReference
			}
		});
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x000236FC File Offset: 0x000218FC
	protected override void __initializeVariables()
	{
		bool flag = this.GameState == null;
		if (flag)
		{
			throw new Exception("GameManager.GameState cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.GameState.Initialize(this);
		base.__nameNetworkVariable(this.GameState, "GameState");
		this.NetworkVariableFields.Add(this.GameState);
		base.__initializeVariables();
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x00023760 File Offset: 0x00021960
	protected override void __initializeRpcs()
	{
		base.__registerRpc(1445539067U, new NetworkBehaviour.RpcReceiveHandler(GameManager.__rpc_handler_1445539067), "Server_NotifySlowMotionRpc", RpcInvokePermission.Server);
		base.__registerRpc(3581845443U, new NetworkBehaviour.RpcReceiveHandler(GameManager.__rpc_handler_3581845443), "Server_NotifyReplayCellyCamRpc", RpcInvokePermission.Server);
		base.__registerRpc(2324051025U, new NetworkBehaviour.RpcReceiveHandler(GameManager.__rpc_handler_2324051025), "Server_NotifySlowMotionStopRpc", RpcInvokePermission.Server);
		base.__registerRpc(1809670267U, new NetworkBehaviour.RpcReceiveHandler(GameManager.__rpc_handler_1809670267), "Server_NotifyGoalScoredRpc", RpcInvokePermission.Server);
		base.__initializeRpcs();
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x000237FC File Offset: 0x000219FC
	private static void __rpc_handler_1445539067(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		float scale;
		reader.ReadValueSafe<float>(out scale, default(FastBufferWriter.ForPrimitives));
		float rampInSeconds;
		reader.ReadValueSafe<float>(out rampInSeconds, default(FastBufferWriter.ForPrimitives));
		float holdSeconds;
		reader.ReadValueSafe<float>(out holdSeconds, default(FastBufferWriter.ForPrimitives));
		float rampOutSeconds;
		reader.ReadValueSafe<float>(out rampOutSeconds, default(FastBufferWriter.ForPrimitives));
		float audioPitchFloor;
		reader.ReadValueSafe<float>(out audioPitchFloor, default(FastBufferWriter.ForPrimitives));
		float cameraFovPunch;
		reader.ReadValueSafe<float>(out cameraFovPunch, default(FastBufferWriter.ForPrimitives));
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((GameManager)target).Server_NotifySlowMotionRpc(scale, rampInSeconds, holdSeconds, rampOutSeconds, audioPitchFloor, cameraFovPunch);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x00023908 File Offset: 0x00021B08
	private static void __rpc_handler_3581845443(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		ulong scorerClientId;
		ByteUnpacker.ReadValueBitPacked(reader, out scorerClientId);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((GameManager)target).Server_NotifyReplayCellyCamRpc(scorerClientId);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x0002396C File Offset: 0x00021B6C
	private static void __rpc_handler_2324051025(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((GameManager)target).Server_NotifySlowMotionStopRpc();
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x000239C0 File Offset: 0x00021BC0
	private static void __rpc_handler_1809670267(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		PlayerTeam byTeam;
		reader.ReadValueSafe<PlayerTeam>(out byTeam, default(FastBufferWriter.ForEnums));
		NetworkObjectReference goalPlayerNetworkObjectReference;
		reader.ReadValueSafe<NetworkObjectReference>(out goalPlayerNetworkObjectReference, default(FastBufferWriter.ForNetworkSerializable));
		NetworkObjectReference assistPlayerNetworkObjectReference;
		reader.ReadValueSafe<NetworkObjectReference>(out assistPlayerNetworkObjectReference, default(FastBufferWriter.ForNetworkSerializable));
		NetworkObjectReference secondAssistPlayerNetworkObjectReference;
		reader.ReadValueSafe<NetworkObjectReference>(out secondAssistPlayerNetworkObjectReference, default(FastBufferWriter.ForNetworkSerializable));
		NetworkObjectReference puckNetworkObjectReference;
		reader.ReadValueSafe<NetworkObjectReference>(out puckNetworkObjectReference, default(FastBufferWriter.ForNetworkSerializable));
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((GameManager)target).Server_NotifyGoalScoredRpc(byTeam, goalPlayerNetworkObjectReference, assistPlayerNetworkObjectReference, secondAssistPlayerNetworkObjectReference, puckNetworkObjectReference);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x00023AAC File Offset: 0x00021CAC
	protected internal override string __getTypeName()
	{
		return "GameManager";
	}

	// Token: 0x0400041F RID: 1055
	[HideInInspector]
	public NetworkVariable<GameState> GameState;

	// Token: 0x04000420 RID: 1056
	private Tween tickTween;

	// Token: 0x04000421 RID: 1057
	private Sequence slowMotionSequence;
}
