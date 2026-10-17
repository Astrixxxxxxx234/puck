using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000180 RID: 384
public class SynchronizedObjectManager : NetworkBehaviourSingleton<SynchronizedObjectManager>
{
	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x00038D48 File Offset: 0x00036F48
	// (set) Token: 0x06000BE3 RID: 3043 RVA: 0x00038D50 File Offset: 0x00036F50
	[HideInInspector]
	public int TickRate
	{
		get
		{
			return this.tickRate;
		}
		set
		{
			if (this.tickRate == value)
			{
				return;
			}
			this.tickRate = value;
			this.clientReceiver.SetTickInterval(this.TickInterval);
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x00038D74 File Offset: 0x00036F74
	[HideInInspector]
	public float TickInterval
	{
		get
		{
			return 1f / (float)this.TickRate;
		}
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x00038D84 File Offset: 0x00036F84
	public override void Awake()
	{
		base.Awake();
		this.clientReceiver = new SynchronizedObjectClientReceiver(this.synchronizedObjectRegistry);
		this.clientReceiver.Configure(this.clientNetworkSmoothingSettings, this.clientDiagnosticsInterval);
		this.clientReceiver.SetTickInterval(this.TickInterval);
		PhysicsManager.OnAfterSimulate += this.OnAfterSimulate;
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x00038DE1 File Offset: 0x00036FE1
	public override void OnDestroy()
	{
		PhysicsManager.OnAfterSimulate -= this.OnAfterSimulate;
		base.OnDestroy();
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x00038DFA File Offset: 0x00036FFA
	private void Update()
	{
		if (!base.IsSpawned || NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.clientReceiver.Update(Time.unscaledDeltaTime);
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x00038E24 File Offset: 0x00037024
	private void OnAfterSimulate(float deltaTime)
	{
		if (!base.IsSpawned || !NetworkManager.Singleton.IsServer)
		{
			return;
		}
		try
		{
			this.Server_Tick(deltaTime);
		}
		catch (Exception arg)
		{
			SynchronizedObjectManager.Logger.Error(string.Format("Synchronization tick failed and was skipped: {0}", arg));
		}
	}

	// Token: 0x06000BE9 RID: 3049 RVA: 0x00038E78 File Offset: 0x00037078
	public void AddSynchronizedObject(SynchronizedObject synchronizedObject)
	{
		this.synchronizedObjectRegistry.Add(synchronizedObject);
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x00038E88 File Offset: 0x00037088
	public void RemoveSynchronizedObject(SynchronizedObject synchronizedObject)
	{
		this.synchronizedObjectRegistry.Remove(synchronizedObject);
		this.clientReceiver.Forget(synchronizedObject.NetworkObjectId);
		foreach (SynchronizedPlayerState synchronizedPlayerState in this.serverSynchronizedPlayerStates)
		{
			synchronizedPlayerState.Forget(synchronizedObject.NetworkObjectId);
		}
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x00038EFC File Offset: 0x000370FC
	public bool TryGetSynchronizedObject(ulong networkObjectId, out SynchronizedObject synchronizedObject)
	{
		return this.synchronizedObjectRegistry.TryGet(networkObjectId, out synchronizedObject);
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x00038F0B File Offset: 0x0003710B
	public void Client_SetTargetTimelinePosition(float value)
	{
		this.clientNetworkSmoothingSettings.TargetTimelinePosition = value;
		this.clientReceiver.Configure(this.clientNetworkSmoothingSettings, this.clientDiagnosticsInterval);
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x00038F30 File Offset: 0x00037130
	public void Client_Dispose()
	{
		this.clientReceiver.Dispose();
		this.synchronizedObjectRegistry.Clear();
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x00038F48 File Offset: 0x00037148
	public void Server_Dispose()
	{
		this.serverTickCount = 0U;
		this.serverTickTime = 0.0;
		this.serverTickTimeScale = 1f;
		this.serverSnapshot.Clear();
		this.serverSendBuffers.Reset();
		this.serverForceSnapshot.Clear();
		this.serverForceSendBuffers.Reset();
		this.serverDiagnostics.Reset();
		this.synchronizedObjectRegistry.Clear();
		this.Server_ClearSynchronizedPlayerStates();
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x00038FC0 File Offset: 0x000371C0
	public void Server_AddSynchronizedPlayer(Player player)
	{
		SynchronizedPlayerState synchronizedPlayerState;
		if (this.Server_TryGetSynchronizedPlayerState(player.OwnerClientId, out synchronizedPlayerState))
		{
			synchronizedPlayerState.Player = player;
			return;
		}
		this.serverSynchronizedPlayerStates.Add(new SynchronizedPlayerState
		{
			Player = player,
			RpcTarget = base.RpcTarget.Single(player.OwnerClientId, RpcTargetUse.Persistent)
		});
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x00039014 File Offset: 0x00037214
	public void Server_RemoveSynchronizedPlayer(Player player)
	{
		SynchronizedPlayerState synchronizedPlayerState;
		if (!this.Server_TryGetSynchronizedPlayerState(player.OwnerClientId, out synchronizedPlayerState))
		{
			return;
		}
		synchronizedPlayerState.Dispose();
		this.serverSynchronizedPlayerStates.Remove(synchronizedPlayerState);
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x00039048 File Offset: 0x00037248
	public void Server_ForceSynchronizeClientId(ulong clientId)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.serverForceSnapshot.Capture(this.synchronizedObjectRegistry);
		this.serverForceSendBuffers.Clear();
		for (int i = 0; i < this.serverForceSnapshot.Count; i++)
		{
			this.serverForceSendBuffers.AddReliable(this.serverForceSnapshot.GetData(i).WithAsleep());
		}
		SynchronizedObjectData[] array = this.serverForceSendBuffers.TakeReliables();
		if (array.Length == 0)
		{
			return;
		}
		this.Server_ReliableSynchronizeObjectsRpc(this.Server_GetTickHeader(0), array, base.RpcTarget.Single(clientId, RpcTargetUse.Temp));
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x000390E4 File Offset: 0x000372E4
	private bool Server_TryGetSynchronizedPlayerState(ulong clientId, out SynchronizedPlayerState synchronizedPlayerState)
	{
		foreach (SynchronizedPlayerState synchronizedPlayerState2 in this.serverSynchronizedPlayerStates)
		{
			if (!(synchronizedPlayerState2.Player == null) && synchronizedPlayerState2.Player.OwnerClientId == clientId)
			{
				synchronizedPlayerState = synchronizedPlayerState2;
				return true;
			}
		}
		synchronizedPlayerState = null;
		return false;
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x0003915C File Offset: 0x0003735C
	private void Server_ClearSynchronizedPlayerStates()
	{
		foreach (SynchronizedPlayerState synchronizedPlayerState in this.serverSynchronizedPlayerStates)
		{
			synchronizedPlayerState.Dispose();
		}
		this.serverSynchronizedPlayerStates.Clear();
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x000391B8 File Offset: 0x000373B8
	private void Server_Tick(float deltaTime)
	{
		this.serverTickCount += 1U;
		this.serverTickTime += (double)deltaTime;
		this.serverTickTimeScale = Time.timeScale;
		this.serverDiagnostics.Configure(this.serverDiagnosticsInterval, this.serverLodBands);
		this.serverDiagnostics.Step(deltaTime);
		if (this.serverSynchronizedPlayerStates.Count == 0)
		{
			return;
		}
		this.serverSendPlanner.Configure(this.serverLodBands, this.serverCulling, this.serverNoOriginTickRateDivisor, this.serverLodHysteresis, this.serverUseHighPrecisionRotation);
		this.serverSnapshot.Capture(this.synchronizedObjectRegistry);
		foreach (SynchronizedPlayerState synchronizedPlayerState in this.serverSynchronizedPlayerStates)
		{
			this.Server_SynchronizePlayer(synchronizedPlayerState);
		}
		SynchronizedObjectServerDiagnosticsData synchronizedObjectServerDiagnosticsData;
		if (this.serverDiagnostics.TryGetData(out synchronizedObjectServerDiagnosticsData))
		{
			this.Server_ReportDiagnostics(synchronizedObjectServerDiagnosticsData);
		}
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x000392B8 File Offset: 0x000374B8
	private void Server_SynchronizePlayer(SynchronizedPlayerState synchronizedPlayerState)
	{
		if (synchronizedPlayerState.RpcTarget == null)
		{
			return;
		}
		this.serverSendPlanner.Plan(this.serverSnapshot, synchronizedPlayerState, this.serverTickCount, this.TickRate, this.serverSendBuffers, this.serverDiagnostics.BeginPlayer());
		synchronizedPlayerState.SendSequenceNumber += 1;
		SynchronizedObjectTickHeader synchronizedObjectTickHeader = this.Server_TakeTickHeader(synchronizedPlayerState);
		this.Server_SynchronizeObjectsRpc(synchronizedObjectTickHeader, this.serverSendBuffers.TakeDeltas(), synchronizedPlayerState.RpcTarget);
		SynchronizedObjectData[] array = this.serverSendBuffers.TakeReliables();
		if (array.Length != 0 || synchronizedObjectTickHeader.ChangeMask != 0)
		{
			this.Server_ReliableSynchronizeObjectsRpc(synchronizedObjectTickHeader, array, synchronizedPlayerState.RpcTarget);
		}
		this.serverDiagnostics.EndPlayer(synchronizedPlayerState);
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x00039368 File Offset: 0x00037568
	private void Server_ReportDiagnostics(in SynchronizedObjectServerDiagnosticsData diagnosticsData)
	{
		if (!EventManager.HasEventListeners("Event_Server_OnObjectSynchronizationDiagnostics"))
		{
			return;
		}
		EventManager.TriggerEvent("Event_Server_OnObjectSynchronizationDiagnostics", new Dictionary<string, object>
		{
			{
				"diagnosticsData",
				diagnosticsData
			}
		});
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x0003939C File Offset: 0x0003759C
	private SynchronizedObjectTickHeader Server_GetTickHeader(ushort sequenceNumber)
	{
		return new SynchronizedObjectTickHeader(sequenceNumber, this.serverTickTime, this.serverTickTimeScale, this.TickInterval);
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x000393B8 File Offset: 0x000375B8
	private SynchronizedObjectTickHeader Server_TakeTickHeader(SynchronizedPlayerState synchronizedPlayerState)
	{
		SynchronizedObjectTickHeader lastSentTickHeader = this.Server_GetTickHeader(synchronizedPlayerState.SendSequenceNumber);
		byte componentMask = this.Server_IsTickHeaderFullSendDue(synchronizedPlayerState) ? 3 : lastSentTickHeader.GetChangeMask(synchronizedPlayerState.LastSentTickHeader);
		synchronizedPlayerState.LastSentTickHeader = lastSentTickHeader;
		synchronizedPlayerState.HasSentTickHeader = true;
		return lastSentTickHeader.WithComponentMask(componentMask);
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x00039402 File Offset: 0x00037602
	private bool Server_IsTickHeaderFullSendDue(SynchronizedPlayerState synchronizedPlayerState)
	{
		return !synchronizedPlayerState.HasSentTickHeader || this.serverTickCount % (uint)this.TickRate == 0U;
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x00039420 File Offset: 0x00037620
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, Delivery = RpcDelivery.Unreliable)]
	private void Server_SynchronizeObjectsRpc(SynchronizedObjectTickHeader tickHeader, SynchronizedObjectData[] synchronizedObjectsData, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 2889824980U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				Delivery = RpcDelivery.Unreliable
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Unreliable);
			fastBufferWriter.WriteValueSafe<SynchronizedObjectTickHeader>(tickHeader, default(FastBufferWriter.ForNetworkSerializable));
			bool flag = synchronizedObjectsData != null;
			fastBufferWriter.WriteValueSafe<bool>(flag, default(FastBufferWriter.ForPrimitives));
			if (flag)
			{
				fastBufferWriter.WriteValueSafe<SynchronizedObjectData>(synchronizedObjectsData, default(FastBufferWriter.ForNetworkSerializable));
			}
			base.__endSendRpc(ref fastBufferWriter, 2889824980U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Unreliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		this.clientReceiver.ReceiveTick(tickHeader, synchronizedObjectsData);
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x0003956C File Offset: 0x0003776C
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, Delivery = RpcDelivery.Reliable)]
	private void Server_ReliableSynchronizeObjectsRpc(SynchronizedObjectTickHeader tickHeader, SynchronizedObjectData[] synchronizedObjectsData, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 3545231968U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				Delivery = RpcDelivery.Reliable
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<SynchronizedObjectTickHeader>(tickHeader, default(FastBufferWriter.ForNetworkSerializable));
			bool flag = synchronizedObjectsData != null;
			fastBufferWriter.WriteValueSafe<bool>(flag, default(FastBufferWriter.ForPrimitives));
			if (flag)
			{
				fastBufferWriter.WriteValueSafe<SynchronizedObjectData>(synchronizedObjectsData, default(FastBufferWriter.ForNetworkSerializable));
			}
			base.__endSendRpc(ref fastBufferWriter, 3545231968U, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		this.clientReceiver.ReceiveReliable(tickHeader, synchronizedObjectsData);
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x00039778 File Offset: 0x00037978
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x00039790 File Offset: 0x00037990
	protected override void __initializeRpcs()
	{
		base.__registerRpc(2889824980U, new NetworkBehaviour.RpcReceiveHandler(SynchronizedObjectManager.__rpc_handler_2889824980), "Server_SynchronizeObjectsRpc", RpcInvokePermission.Server);
		base.__registerRpc(3545231968U, new NetworkBehaviour.RpcReceiveHandler(SynchronizedObjectManager.__rpc_handler_3545231968), "Server_ReliableSynchronizeObjectsRpc", RpcInvokePermission.Server);
		base.__initializeRpcs();
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x000397E8 File Offset: 0x000379E8
	private static void __rpc_handler_2889824980(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		SynchronizedObjectTickHeader tickHeader;
		reader.ReadValueSafe<SynchronizedObjectTickHeader>(out tickHeader, default(FastBufferWriter.ForNetworkSerializable));
		bool flag;
		reader.ReadValueSafe<bool>(out flag, default(FastBufferWriter.ForPrimitives));
		SynchronizedObjectData[] synchronizedObjectsData = null;
		if (flag)
		{
			reader.ReadValueSafe<SynchronizedObjectData>(out synchronizedObjectsData, default(FastBufferWriter.ForNetworkSerializable));
		}
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((SynchronizedObjectManager)target).Server_SynchronizeObjectsRpc(tickHeader, synchronizedObjectsData, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x000398B0 File Offset: 0x00037AB0
	private static void __rpc_handler_3545231968(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		SynchronizedObjectTickHeader tickHeader;
		reader.ReadValueSafe<SynchronizedObjectTickHeader>(out tickHeader, default(FastBufferWriter.ForNetworkSerializable));
		bool flag;
		reader.ReadValueSafe<bool>(out flag, default(FastBufferWriter.ForPrimitives));
		SynchronizedObjectData[] synchronizedObjectsData = null;
		if (flag)
		{
			reader.ReadValueSafe<SynchronizedObjectData>(out synchronizedObjectsData, default(FastBufferWriter.ForNetworkSerializable));
		}
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((SynchronizedObjectManager)target).Server_ReliableSynchronizeObjectsRpc(tickHeader, synchronizedObjectsData, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000C02 RID: 3074 RVA: 0x00039977 File Offset: 0x00037B77
	protected internal override string __getTypeName()
	{
		return "SynchronizedObjectManager";
	}

	// Token: 0x04000753 RID: 1875
	private static readonly global::Logger Logger = new global::Logger("SynchronizedObjectManager");

	// Token: 0x04000754 RID: 1876
	[Header("Settings")]
	[SerializeField]
	[Tooltip("Sends rotation at full 8-byte precision instead of 4-byte smallest-three compression for objects that no LOD band and no culling applies to.")]
	private bool serverUseHighPrecisionRotation = true;

	// Token: 0x04000755 RID: 1877
	[SerializeField]
	[Range(0f, 0.5f)]
	[Tooltip("Fraction of a band's MinDistance an object must move past the boundary before it changes LOD band or culling state, so objects hovering near a boundary do not switch every tick. 0 disables hysteresis.")]
	private float serverLodHysteresis = 0.1f;

	// Token: 0x04000756 RID: 1878
	[SerializeField]
	[Tooltip("Distance bands ordered ascending by MinDistance, overriding the defaults above for objects farther than a band's MinDistance from a player's LOD origin. Objects nearer than the first band, and every object while empty, update every tick.")]
	private SynchronizedObjectBandSettings[] serverLodBands;

	// Token: 0x04000757 RID: 1879
	[SerializeField]
	[Tooltip("Overrides the defaults above and any LOD band for objects farther than MinDistance from a player's LOD origin and behind the player's camera. A TickRateDivisor of 1 disables culling.")]
	private SynchronizedObjectBandSettings serverCulling;

	// Token: 0x04000758 RID: 1880
	[SerializeField]
	[Range(1f, 16f)]
	[Tooltip("Replaces the LOD bands for players with no LOD origin, meaning spectators and players whose body has not spawned. Their viewpoint is unknown to the server, so neither distance nor culling can be measured.")]
	private byte serverNoOriginTickRateDivisor = 4;

	// Token: 0x04000759 RID: 1881
	[SerializeField]
	[Tooltip("Seconds between Event_Server_OnObjectSynchronizationDiagnostics events, one per player, naming how many objects landed on the defaults, in each LOD band, culled and on the no-origin fallback. 0 disables diagnostics.")]
	private float serverDiagnosticsInterval;

	// Token: 0x0400075A RID: 1882
	[SerializeField]
	[Tooltip("Client-side snapshot interpolation tuning.")]
	private SynchronizedObjectNetworkSmoothingSettings clientNetworkSmoothingSettings = SynchronizedObjectNetworkSmoothingSettings.Default;

	// Token: 0x0400075B RID: 1883
	[SerializeField]
	[Tooltip("Seconds between Event_Client_OnObjectSynchronizationDiagnostics events measuring the incoming synchronization message stream for UI display. 0 disables diagnostics.")]
	private float clientDiagnosticsInterval = 1f;

	// Token: 0x0400075C RID: 1884
	private int tickRate = 100;

	// Token: 0x0400075D RID: 1885
	private SynchronizedObjectRegistry synchronizedObjectRegistry = new SynchronizedObjectRegistry();

	// Token: 0x0400075E RID: 1886
	private List<SynchronizedPlayerState> serverSynchronizedPlayerStates = new List<SynchronizedPlayerState>();

	// Token: 0x0400075F RID: 1887
	private uint serverTickCount;

	// Token: 0x04000760 RID: 1888
	private double serverTickTime;

	// Token: 0x04000761 RID: 1889
	private float serverTickTimeScale = 1f;

	// Token: 0x04000762 RID: 1890
	private SynchronizedObjectSnapshot serverSnapshot = new SynchronizedObjectSnapshot();

	// Token: 0x04000763 RID: 1891
	private SynchronizedObjectSendPlanner serverSendPlanner = new SynchronizedObjectSendPlanner();

	// Token: 0x04000764 RID: 1892
	private SynchronizedObjectSendBuffers serverSendBuffers = new SynchronizedObjectSendBuffers();

	// Token: 0x04000765 RID: 1893
	private SynchronizedObjectServerDiagnostics serverDiagnostics = new SynchronizedObjectServerDiagnostics();

	// Token: 0x04000766 RID: 1894
	private SynchronizedObjectSnapshot serverForceSnapshot = new SynchronizedObjectSnapshot();

	// Token: 0x04000767 RID: 1895
	private SynchronizedObjectSendBuffers serverForceSendBuffers = new SynchronizedObjectSendBuffers();

	// Token: 0x04000768 RID: 1896
	private SynchronizedObjectClientReceiver clientReceiver;
}
