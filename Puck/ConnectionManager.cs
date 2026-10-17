using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

// Token: 0x02000101 RID: 257
public class ConnectionManager : MonoBehaviourSingleton<ConnectionManager>
{
	// Token: 0x060007F7 RID: 2039 RVA: 0x000288DC File Offset: 0x00026ADC
	private void Start()
	{
		this.UnityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
		NetworkManager.Singleton.OnClientStarted += this.Client_OnClientStarted;
		NetworkManager.Singleton.OnClientStopped += this.Client_OnClientStopped;
		NetworkManager.Singleton.NetworkConfig.ProtocolVersion = ApplicationManager.Version;
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x00028939 File Offset: 0x00026B39
	private void OnDestroy()
	{
		if (NetworkManager.Singleton != null)
		{
			NetworkManager.Singleton.OnClientStarted -= this.Client_OnClientStarted;
			NetworkManager.Singleton.OnClientStopped -= this.Client_OnClientStopped;
		}
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x00028974 File Offset: 0x00026B74
	public void Client_StartClient(string ipAddress, ushort port, string password = null)
	{
		ConnectionManager.Logger.Info(string.Format("Starting client {0}:{1}", ipAddress, port));
		if (NetworkManager.Singleton.IsClient)
		{
			Connection value = new Connection
			{
				EndPoint = new EndPoint(ipAddress, port),
				Password = password
			};
			GlobalStateManager.SetConnectionState(new Dictionary<string, object>
			{
				{
					"pendingConnection",
					value
				}
			});
			this.Client_Disconnect();
			return;
		}
		ConnectionData connectionData = new ConnectionData();
		connectionData.SteamId = BackendManager.PlayerState.PlayerData.steamId;
		connectionData.Key = BackendManager.PlayerState.Key;
		connectionData.Password = password;
		connectionData.EnabledModIds = (from mod in ModManager.EnabledMods
		select mod.Id).ToArray<string>();
		connectionData.Handedness = SettingsManager.Handedness;
		connectionData.FlagID = SettingsManager.FlagID;
		connectionData.HeadgearIDBlueAttacker = SettingsManager.HeadgearIDBlueAttacker;
		connectionData.HeadgearIDRedAttacker = SettingsManager.HeadgearIDRedAttacker;
		connectionData.HeadgearIDBlueGoalie = SettingsManager.HeadgearIDBlueGoalie;
		connectionData.HeadgearIDRedGoalie = SettingsManager.HeadgearIDRedGoalie;
		connectionData.MustacheID = SettingsManager.MustacheID;
		connectionData.BeardID = SettingsManager.BeardID;
		connectionData.JerseyIDBlueAttacker = SettingsManager.JerseyIDBlueAttacker;
		connectionData.JerseyIDRedAttacker = SettingsManager.JerseyIDRedAttacker;
		connectionData.JerseyIDBlueGoalie = SettingsManager.JerseyIDBlueGoalie;
		connectionData.JerseyIDRedGoalie = SettingsManager.JerseyIDRedGoalie;
		connectionData.StickSkinIDBlueAttacker = SettingsManager.StickSkinIDBlueAttacker;
		connectionData.StickSkinIDRedAttacker = SettingsManager.StickSkinIDRedAttacker;
		connectionData.StickSkinIDBlueGoalie = SettingsManager.StickSkinIDBlueGoalie;
		connectionData.StickSkinIDRedGoalie = SettingsManager.StickSkinIDRedGoalie;
		connectionData.StickShaftTapeIDBlueAttacker = SettingsManager.StickShaftTapeIDBlueAttacker;
		connectionData.StickShaftTapeIDRedAttacker = SettingsManager.StickShaftTapeIDRedAttacker;
		connectionData.StickShaftTapeIDBlueGoalie = SettingsManager.StickShaftTapeIDBlueGoalie;
		connectionData.StickShaftTapeIDRedGoalie = SettingsManager.StickShaftTapeIDRedGoalie;
		connectionData.StickBladeTapeIDBlueAttacker = SettingsManager.StickBladeTapeIDBlueAttacker;
		connectionData.StickBladeTapeIDRedAttacker = SettingsManager.StickBladeTapeIDRedAttacker;
		connectionData.StickBladeTapeIDBlueGoalie = SettingsManager.StickBladeTapeIDBlueGoalie;
		connectionData.StickBladeTapeIDRedGoalie = SettingsManager.StickBladeTapeIDRedGoalie;
		string s = JsonSerializer.Serialize<ConnectionData>(connectionData, null);
		NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(s);
		this.UnityTransport.SetConnectionData(ipAddress, port, null);
		Connection value2 = new Connection
		{
			EndPoint = new EndPoint(ipAddress, port),
			Password = password
		};
		GlobalStateManager.ClearReconnectionState();
		GlobalStateManager.SetConnectionState(new Dictionary<string, object>
		{
			{
				"connection",
				value2
			},
			{
				"pendingConnection",
				null
			},
			{
				"phase",
				ConnectionPhase.Connecting
			}
		});
		NetworkManager.Singleton.StartClient();
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x00028BD8 File Offset: 0x00026DD8
	private void Client_OnClientStarted()
	{
		EventManager.TriggerEvent("Event_OnClientStarted", null);
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00028BE5 File Offset: 0x00026DE5
	private void Client_OnClientStopped(bool wasHost)
	{
		base.StartCoroutine(this.DelayedOnClientStopped(wasHost));
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x00028BF5 File Offset: 0x00026DF5
	private IEnumerator DelayedOnClientStopped(bool wasHost)
	{
		yield return new WaitForEndOfFrame();
		EventManager.TriggerEvent("Event_OnClientStopped", new Dictionary<string, object>
		{
			{
				"wasHost",
				wasHost
			}
		});
		yield break;
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x00028C04 File Offset: 0x00026E04
	public void Client_Disconnect()
	{
		if (!NetworkManager.Singleton.IsClient)
		{
			return;
		}
		ConnectionManager.Logger.Info(string.Format("Puck ({0}) network shutdown", ApplicationManager.Version));
		NetworkManager.Singleton.Shutdown(true);
	}

	// Token: 0x0400050B RID: 1291
	private static readonly global::Logger Logger = new global::Logger("ConnectionManager");

	// Token: 0x0400050C RID: 1292
	[HideInInspector]
	public UnityTransport UnityTransport;
}
