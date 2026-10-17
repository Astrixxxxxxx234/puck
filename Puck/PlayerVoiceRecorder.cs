using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Steamworks;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class PlayerVoiceRecorder : NetworkBehaviour
{
	// Token: 0x1700002C RID: 44
	// (get) Token: 0x0600024E RID: 590 RVA: 0x0000F34D File Offset: 0x0000D54D
	// (set) Token: 0x0600024F RID: 591 RVA: 0x0000F355 File Offset: 0x0000D555
	public float TransmitLevel { get; private set; }

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000250 RID: 592 RVA: 0x0000F360 File Offset: 0x0000D560
	// (remove) Token: 0x06000251 RID: 593 RVA: 0x0000F398 File Offset: 0x0000D598
	public event Action<float> OnTransmitLevelChanged;

	// Token: 0x06000252 RID: 594 RVA: 0x0000F3CD File Offset: 0x0000D5CD
	private void Awake()
	{
		this.Player = base.GetComponent<Player>();
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0000F3DB File Offset: 0x0000D5DB
	private void Update()
	{
		if (this.Player.IsLocalPlayer && this.IsRecording)
		{
			this.Client_SendVoiceData();
		}
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000F3F8 File Offset: 0x0000D5F8
	private AudioClip InitializeAudioClip(uint sampleRate)
	{
		this.audioQueue = new ConcurrentQueue<float>();
		return AudioClip.Create("VoiceData", (int)sampleRate, 1, (int)sampleRate, true, new AudioClip.PCMReaderCallback(this.OnAudioRead), null);
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0000F420 File Offset: 0x0000D620
	private void OnAudioRead(float[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			float num;
			if (this.audioQueue != null && this.audioQueue.TryDequeue(out num))
			{
				data[i] = num;
			}
			else
			{
				data[i] = 0f;
			}
		}
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0000F460 File Offset: 0x0000D660
	private void WriteToClip(byte[] decompressed, int bytesWritten)
	{
		int num = 0;
		while (num < bytesWritten - 1 && num + 1 < decompressed.Length)
		{
			float f = (float)((short)((int)decompressed[num] | (int)decompressed[num + 1] << 8)) / 32768f;
			this.WriteToClip(f);
			num += 2;
		}
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000F49E File Offset: 0x0000D69E
	private void WriteToClip(float f)
	{
		if (this.audioQueue != null)
		{
			this.audioQueue.Enqueue(f);
		}
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000F4B4 File Offset: 0x0000D6B4
	private void Client_SendVoiceData()
	{
		uint num;
		SteamUser.GetAvailableVoice(out num);
		float rawLevel = 0f;
		bool hasVoice = false;
		if (num > 0U)
		{
			byte[] array = new byte[num];
			uint num2;
			SteamUser.GetVoice(true, array, num, out num2);
			if (num2 > 0U)
			{
				PlayerVoiceRecorder.Logger.Info(string.Format("Sending voice data to server ({0}b)", array.Length));
				this.Client_VoiceDataRpc(array, default(RpcParams));
				if (this.SampleRate > 0U)
				{
					if (this.levelDecompressBuffer == null)
					{
						this.levelDecompressBuffer = new byte[this.SampleRate * 2U];
					}
					rawLevel = VoiceUtils.ComputeLevel(array, num2, this.SampleRate, this.levelDecompressBuffer);
					hasVoice = true;
				}
			}
		}
		this.SetTransmitLevel(this.transmitMeter.Update(rawLevel, hasVoice, Time.deltaTime));
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000F56C File Offset: 0x0000D76C
	private void SetTransmitLevel(float level)
	{
		this.TransmitLevel = level;
		Action<float> onTransmitLevelChanged = this.OnTransmitLevelChanged;
		if (onTransmitLevelChanged == null)
		{
			return;
		}
		onTransmitLevelChanged(level);
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0000F588 File Offset: 0x0000D788
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_RequestVoiceStartRpc(uint sampleRate, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 1666941424U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter writer = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			BytePacker.WriteValueBitPacked(writer, sampleRate);
			base.__endSendRpc(ref writer, 1666941424U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId)
		{
			return;
		}
		if (!this.IsEnabled)
		{
			return;
		}
		if (this.IsRecording)
		{
			return;
		}
		this.Server_VoiceStartRpc(sampleRate);
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000F68C File Offset: 0x0000D88C
	[Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_VoiceStartRpc(uint sampleRate)
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 236455107U;
			RpcParams rpcParams2;
			RpcParams rpcParams = rpcParams2;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter writer = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Everyone, RpcDelivery.Reliable);
			BytePacker.WriteValueBitPacked(writer, sampleRate);
			base.__endSendRpc(ref writer, 236455107U, rpcParams2, attributeParams, SendTo.Everyone, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (this.Player.IsMuted.Value)
		{
			PlayerVoiceRecorder.Logger.Info(string.Format("Player is muted, not starting voice recording ({0})", base.OwnerClientId));
			return;
		}
		AudioClip value = this.InitializeAudioClip(sampleRate);
		if (this.Player.IsLocalPlayer)
		{
			PlayerVoiceRecorder.Logger.Info(string.Format("Recording Steam voice at {0}Hz", sampleRate));
			SteamUser.StartVoiceRecording();
		}
		PlayerVoiceRecorder.Logger.Info(string.Format("Player started voice recording ({0})", base.OwnerClientId));
		this.IsRecording = true;
		this.SampleRate = sampleRate;
		EventManager.TriggerEvent("Event_Everyone_OnPlayerVoiceStarted", new Dictionary<string, object>
		{
			{
				"player",
				this.Player
			},
			{
				"audioClip",
				value
			}
		});
	}

	// Token: 0x0600025C RID: 604 RVA: 0x0000F830 File Offset: 0x0000DA30
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_VoiceDataRpc(byte[] voiceData, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 198182109U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			bool flag = voiceData != null;
			fastBufferWriter.WriteValueSafe<bool>(flag, default(FastBufferWriter.ForPrimitives));
			if (flag)
			{
				fastBufferWriter.WriteValueSafe<byte>(voiceData, default(FastBufferWriter.ForPrimitives));
			}
			base.__endSendRpc(ref fastBufferWriter, 198182109U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (this.Player.IsMuted.Value)
		{
			return;
		}
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId)
		{
			return;
		}
		if (!this.IsRecording)
		{
			return;
		}
		this.Server_VoiceDataRpc(voiceData);
	}

	// Token: 0x0600025D RID: 605 RVA: 0x0000F97C File Offset: 0x0000DB7C
	[Rpc(SendTo.ClientsAndHost, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_VoiceDataRpc(byte[] voiceData)
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 2054004997U;
			RpcParams rpcParams2;
			RpcParams rpcParams = rpcParams2;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.ClientsAndHost, RpcDelivery.Reliable);
			bool flag = voiceData != null;
			fastBufferWriter.WriteValueSafe<bool>(flag, default(FastBufferWriter.ForPrimitives));
			if (flag)
			{
				fastBufferWriter.WriteValueSafe<byte>(voiceData, default(FastBufferWriter.ForPrimitives));
			}
			base.__endSendRpc(ref fastBufferWriter, 2054004997U, rpcParams2, attributeParams, SendTo.ClientsAndHost, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		byte[] array = new byte[this.SampleRate * 2U];
		uint num;
		SteamUser.DecompressVoice(voiceData, (uint)voiceData.Length, array, (uint)array.Length, out num, this.SampleRate);
		if (num > 0U)
		{
			this.WriteToClip(array, (int)num);
		}
	}

	// Token: 0x0600025E RID: 606 RVA: 0x0000FAD0 File Offset: 0x0000DCD0
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_RequestVoiceStopRpc(RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 2728922553U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			base.__endSendRpc(ref fastBufferWriter, 2728922553U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (rpcParams.Receive.SenderClientId != base.OwnerClientId)
		{
			return;
		}
		if (!this.IsRecording)
		{
			return;
		}
		this.Server_VoiceStopRpc();
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000FBC0 File Offset: 0x0000DDC0
	[Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_VoiceStopRpc()
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 824274507U;
			RpcParams rpcParams2;
			RpcParams rpcParams = rpcParams2;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				InvokePermission = RpcInvokePermission.Server,
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Everyone, RpcDelivery.Reliable);
			base.__endSendRpc(ref fastBufferWriter, 824274507U, rpcParams2, attributeParams, SendTo.Everyone, RpcDelivery.Reliable);
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			return;
		}
		this.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
		if (this.Player.IsLocalPlayer)
		{
			PlayerVoiceRecorder.Logger.Info("Stopping Steam voice recording");
			SteamUser.StopVoiceRecording();
		}
		PlayerVoiceRecorder.Logger.Info(string.Format("Player stopped voice recording ({0})", base.OwnerClientId));
		this.IsRecording = false;
		if (this.Player.IsLocalPlayer)
		{
			this.transmitMeter.Reset();
			this.SetTransmitLevel(0f);
		}
		EventManager.TriggerEvent("Event_Everyone_OnPlayerVoiceStopped", new Dictionary<string, object>
		{
			{
				"player",
				this.Player
			}
		});
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0000FD48 File Offset: 0x0000DF48
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0000FD60 File Offset: 0x0000DF60
	protected override void __initializeRpcs()
	{
		base.__registerRpc(1666941424U, new NetworkBehaviour.RpcReceiveHandler(PlayerVoiceRecorder.__rpc_handler_1666941424), "Client_RequestVoiceStartRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(236455107U, new NetworkBehaviour.RpcReceiveHandler(PlayerVoiceRecorder.__rpc_handler_236455107), "Server_VoiceStartRpc", RpcInvokePermission.Server);
		base.__registerRpc(198182109U, new NetworkBehaviour.RpcReceiveHandler(PlayerVoiceRecorder.__rpc_handler_198182109), "Client_VoiceDataRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(2054004997U, new NetworkBehaviour.RpcReceiveHandler(PlayerVoiceRecorder.__rpc_handler_2054004997), "Server_VoiceDataRpc", RpcInvokePermission.Server);
		base.__registerRpc(2728922553U, new NetworkBehaviour.RpcReceiveHandler(PlayerVoiceRecorder.__rpc_handler_2728922553), "Client_RequestVoiceStopRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(824274507U, new NetworkBehaviour.RpcReceiveHandler(PlayerVoiceRecorder.__rpc_handler_824274507), "Server_VoiceStopRpc", RpcInvokePermission.Server);
		base.__initializeRpcs();
	}

	// Token: 0x06000264 RID: 612 RVA: 0x0000FE3C File Offset: 0x0000E03C
	private static void __rpc_handler_1666941424(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		uint sampleRate;
		ByteUnpacker.ReadValueBitPacked(reader, out sampleRate);
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((PlayerVoiceRecorder)target).Client_RequestVoiceStartRpc(sampleRate, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0000FEAC File Offset: 0x0000E0AC
	private static void __rpc_handler_236455107(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		uint sampleRate;
		ByteUnpacker.ReadValueBitPacked(reader, out sampleRate);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((PlayerVoiceRecorder)target).Server_VoiceStartRpc(sampleRate);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0000FF10 File Offset: 0x0000E110
	private static void __rpc_handler_198182109(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool flag;
		reader.ReadValueSafe<bool>(out flag, default(FastBufferWriter.ForPrimitives));
		byte[] voiceData = null;
		if (flag)
		{
			reader.ReadValueSafe<byte>(out voiceData, default(FastBufferWriter.ForPrimitives));
		}
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((PlayerVoiceRecorder)target).Client_VoiceDataRpc(voiceData, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000267 RID: 615 RVA: 0x0000FFB8 File Offset: 0x0000E1B8
	private static void __rpc_handler_2054004997(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		bool flag;
		reader.ReadValueSafe<bool>(out flag, default(FastBufferWriter.ForPrimitives));
		byte[] voiceData = null;
		if (flag)
		{
			reader.ReadValueSafe<byte>(out voiceData, default(FastBufferWriter.ForPrimitives));
		}
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((PlayerVoiceRecorder)target).Server_VoiceDataRpc(voiceData);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000268 RID: 616 RVA: 0x00010054 File Offset: 0x0000E254
	private static void __rpc_handler_2728922553(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((PlayerVoiceRecorder)target).Client_RequestVoiceStopRpc(ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x06000269 RID: 617 RVA: 0x000100B4 File Offset: 0x0000E2B4
	private static void __rpc_handler_824274507(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((PlayerVoiceRecorder)target).Server_VoiceStopRpc();
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00010105 File Offset: 0x0000E305
	protected internal override string __getTypeName()
	{
		return "PlayerVoiceRecorder";
	}

	// Token: 0x040001BF RID: 447
	private static readonly global::Logger Logger = new global::Logger("PlayerVoiceRecorder");

	// Token: 0x040001C0 RID: 448
	[HideInInspector]
	public Player Player;

	// Token: 0x040001C1 RID: 449
	[HideInInspector]
	public bool IsEnabled;

	// Token: 0x040001C2 RID: 450
	[HideInInspector]
	public bool IsRecording;

	// Token: 0x040001C3 RID: 451
	[HideInInspector]
	public uint SampleRate;

	// Token: 0x040001C6 RID: 454
	private ConcurrentQueue<float> audioQueue;

	// Token: 0x040001C7 RID: 455
	private byte[] levelDecompressBuffer;

	// Token: 0x040001C8 RID: 456
	private readonly VoiceLevelMeter transmitMeter = new VoiceLevelMeter();
}
