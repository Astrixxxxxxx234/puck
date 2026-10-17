using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Analytics;
using UnityEngine;

// Token: 0x02000106 RID: 262
public class NetworkStatistics : MonoBehaviour
{
	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000827 RID: 2087 RVA: 0x0001A735 File Offset: 0x00018935
	private static bool IsServer
	{
		get
		{
			return NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer;
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000828 RID: 2088 RVA: 0x000296F2 File Offset: 0x000278F2
	private float DiagnosticsInterval
	{
		get
		{
			if (!NetworkStatistics.IsServer)
			{
				return this.clientDiagnosticsInterval;
			}
			return this.serverDiagnosticsInterval;
		}
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x00029708 File Offset: 0x00027908
	private void Update()
	{
		float diagnosticsInterval = this.DiagnosticsInterval;
		if (diagnosticsInterval <= 0f)
		{
			return;
		}
		DriverStatistics driverStatistics;
		if (!NetworkStatistics.TryGetDriverStatistics(out driverStatistics))
		{
			this.ResetDiagnostics();
			return;
		}
		this.RecordStall(driverStatistics.RxTotalPackets);
		this.diagnosticsTime += Time.unscaledDeltaTime;
		if (this.diagnosticsTime < diagnosticsInterval)
		{
			return;
		}
		this.TakeDiagnostics(driverStatistics);
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x00029765 File Offset: 0x00027965
	private void ResetDiagnostics()
	{
		this.lastDriverStatistics = null;
		this.hasPolledDriver = false;
		this.diagnosticsTime = 0f;
		this.maxStallTime = 0.0;
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x00029794 File Offset: 0x00027994
	private void RecordStall(ulong receivedPacketCount)
	{
		double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
		if (!this.hasPolledDriver || receivedPacketCount != this.lastReceivedPacketCount)
		{
			this.lastReceivedPacketCount = receivedPacketCount;
			this.lastReceivedPacketTime = realtimeSinceStartupAsDouble;
			this.hasPolledDriver = true;
		}
		this.maxStallTime = Math.Max(this.maxStallTime, realtimeSinceStartupAsDouble - this.lastReceivedPacketTime);
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x000297E8 File Offset: 0x000279E8
	private void TakeDiagnostics(in DriverStatistics driverStatistics)
	{
		if (this.lastDriverStatistics != null)
		{
			DriverStatistics value = this.lastDriverStatistics.Value;
			this.TriggerDiagnosticsEvent(this.GetNetworkDiagnostics(driverStatistics, value));
		}
		this.lastDriverStatistics = new DriverStatistics?(driverStatistics);
		this.diagnosticsTime = 0f;
		this.maxStallTime = 0.0;
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x00029848 File Offset: 0x00027A48
	private NetworkDiagnostics GetNetworkDiagnostics(in DriverStatistics driverStatistics, in DriverStatistics lastStatistics)
	{
		return new NetworkDiagnostics
		{
			WindowTime = this.diagnosticsTime,
			MaxStallTime = this.maxStallTime,
			SentWireByteCount = NetworkStatistics.GetDelta(driverStatistics.TxTotalBytes, lastStatistics.TxTotalBytes),
			ReceivedWireByteCount = NetworkStatistics.GetDelta(driverStatistics.RxTotalBytes, lastStatistics.RxTotalBytes),
			SentPacketCount = NetworkStatistics.GetDelta(driverStatistics.TxTotalPackets, lastStatistics.TxTotalPackets),
			ReceivedPacketCount = NetworkStatistics.GetDelta(driverStatistics.RxTotalPackets, lastStatistics.RxTotalPackets)
		};
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x000298D8 File Offset: 0x00027AD8
	private void TriggerDiagnosticsEvent(NetworkDiagnostics networkDiagnostics)
	{
		string eventName = NetworkStatistics.IsServer ? "Event_Server_OnNetworkDiagnostics" : "Event_Client_OnNetworkDiagnostics";
		if (!EventManager.HasEventListeners(eventName))
		{
			return;
		}
		EventManager.TriggerEvent(eventName, new Dictionary<string, object>
		{
			{
				"networkDiagnostics",
				networkDiagnostics
			}
		});
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00029920 File Offset: 0x00027B20
	private static bool TryGetDriverStatistics(out DriverStatistics driverStatistics)
	{
		driverStatistics = default(DriverStatistics);
		NetworkManager singleton = NetworkManager.Singleton;
		if (singleton == null || !singleton.IsListening)
		{
			return false;
		}
		UnityTransport unityTransport = singleton.NetworkConfig.NetworkTransport as UnityTransport;
		if (!unityTransport)
		{
			return false;
		}
		ref NetworkDriver networkDriver = ref unityTransport.GetNetworkDriver();
		if (!networkDriver.IsCreated)
		{
			return false;
		}
		driverStatistics = networkDriver.GetStatistics();
		return true;
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x00029986 File Offset: 0x00027B86
	private static ulong GetDelta(ulong current, ulong last)
	{
		if (current < last)
		{
			return current;
		}
		return current - last;
	}

	// Token: 0x0400051B RID: 1307
	[Header("Settings")]
	[SerializeField]
	[Tooltip("Seconds between Event_Server_OnNetworkDiagnostics events measuring transport traffic while running as the server. 0 disables diagnostics.")]
	private float serverDiagnosticsInterval;

	// Token: 0x0400051C RID: 1308
	[SerializeField]
	[Tooltip("Seconds between Event_Client_OnNetworkDiagnostics events measuring transport traffic while running as a client, for UI display. 0 disables diagnostics.")]
	private float clientDiagnosticsInterval = 1f;

	// Token: 0x0400051D RID: 1309
	private float diagnosticsTime;

	// Token: 0x0400051E RID: 1310
	private DriverStatistics? lastDriverStatistics;

	// Token: 0x0400051F RID: 1311
	private ulong lastReceivedPacketCount;

	// Token: 0x04000520 RID: 1312
	private double lastReceivedPacketTime;

	// Token: 0x04000521 RID: 1313
	private bool hasPolledDriver;

	// Token: 0x04000522 RID: 1314
	private double maxStallTime;
}
