using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

// Token: 0x020000AF RID: 175
public static class BackendManagerController
{
	// Token: 0x060005B7 RID: 1463 RVA: 0x0001F868 File Offset: 0x0001DA68
	public static void Initialize()
	{
		EventManager.AddEventListener("Event_OnGetTicketForWebApiResponse", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnGetTicketForWebApiResponse));
		EventManager.AddEventListener("Event_OnFooterClickCreateParty", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnFooterClickCreateParty));
		EventManager.AddEventListener("Event_OnFooterClickLeaveParty", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnFooterClickLeaveParty));
		EventManager.AddEventListener("Event_OnFooterClickDisbandParty", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnFooterClickDisbandParty));
		EventManager.AddEventListener("Event_OnLobbyCreated", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnLobbyCreated));
		EventManager.AddEventListener("Event_OnLobbyEntered", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnLobbyEntered));
		EventManager.AddEventListener("Event_OnMatchmakingMatchingClickStartMatchmaking", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnMatchmakingMatchingClickStartMatchmaking));
		EventManager.AddEventListener("Event_OnMatchmakingMatchingClickClose", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnMatchmakingMatchingClickClose));
		EventManager.AddEventListener("Event_OnPopupClickOk", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnPopupClickOk));
		EventManager.AddEventListener("Event_OnAppearanceClickPurchaseItem", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnAppearanceClickPurchaseItem));
		EventManager.AddEventListener("Event_OnMicroTxnAuthorizationResponse", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnMicroTxnAuthorizationResponse));
		EventManager.AddEventListener("Event_OnNewServerClickStart", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnNewServerClickStart));
		EventManager.AddEventListener("Event_OnServerStateChanged", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnServerStateChanged));
		WebSocketManager.AddMessageListener("disconnected", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnDisconnected));
		WebSocketManager.AddMessageListener("playerAuthenticateResponse", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerAuthenticateResponse));
		WebSocketManager.AddMessageListener("playerData", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerData));
		WebSocketManager.AddMessageListener("playerPartyData", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerPartyData));
		WebSocketManager.AddMessageListener("playerGroupData", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerGroupData));
		WebSocketManager.AddMessageListener("playerMatchData", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerMatchData));
		WebSocketManager.AddMessageListener("playerStatistics", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerStatistics));
		WebSocketManager.AddMessageListener("playerKey", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerKey));
		WebSocketManager.AddMessageListener("playerProbeRttRequest", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerProbeRttRequest));
		WebSocketManager.AddMessageListener("playerMaxRttRequest", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerMaxRttRequest));
		WebSocketManager.AddMessageListener("playerStartTransactionResponse", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerStartTransactionResponse));
		WebSocketManager.AddMessageListener("playerFinalizeTransactionResponse", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerFinalizeTransactionResponse));
		WebSocketManager.AddMessageListener("serverAuthenticateResponse", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnServerAuthenticateResponse));
		WebSocketManager.AddMessageListener("serverUnauthenticateResponse", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnServerUnauthenticateResponse));
		WebSocketManager.AddMessageListener("serverData", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnServerData));
		WebSocketManager.AddMessageListener("serverMatchData", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnServerMatchData));
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x0001FAF4 File Offset: 0x0001DCF4
	public static void Dispose()
	{
		EventManager.RemoveEventListener("Event_OnGetTicketForWebApiResponse", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnGetTicketForWebApiResponse));
		EventManager.RemoveEventListener("Event_OnFooterClickCreateParty", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnFooterClickCreateParty));
		EventManager.RemoveEventListener("Event_OnFooterClickLeaveParty", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnFooterClickLeaveParty));
		EventManager.RemoveEventListener("Event_OnFooterClickDisbandParty", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnFooterClickDisbandParty));
		EventManager.RemoveEventListener("Event_OnLobbyCreated", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnLobbyCreated));
		EventManager.RemoveEventListener("Event_OnLobbyEntered", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnLobbyEntered));
		EventManager.RemoveEventListener("Event_OnMatchmakingMatchingClickStartMatchmaking", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnMatchmakingMatchingClickStartMatchmaking));
		EventManager.RemoveEventListener("Event_OnMatchmakingMatchingClickClose", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnMatchmakingMatchingClickClose));
		EventManager.RemoveEventListener("Event_OnPopupClickOk", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnPopupClickOk));
		EventManager.RemoveEventListener("Event_OnAppearanceClickPurchaseItem", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnAppearanceClickPurchaseItem));
		EventManager.RemoveEventListener("Event_OnMicroTxnAuthorizationResponse", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnMicroTxnAuthorizationResponse));
		EventManager.RemoveEventListener("Event_OnNewServerClickStart", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnNewServerClickStart));
		EventManager.RemoveEventListener("Event_OnServerStateChanged", new Action<Dictionary<string, object>>(BackendManagerController.Event_OnServerStateChanged));
		WebSocketManager.RemoveMessageListener("disconnected", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnDisconnected));
		WebSocketManager.RemoveMessageListener("playerAuthenticateResponse", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerAuthenticateResponse));
		WebSocketManager.RemoveMessageListener("playerData", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerData));
		WebSocketManager.RemoveMessageListener("playerPartyData", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerPartyData));
		WebSocketManager.RemoveMessageListener("playerGroupData", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerGroupData));
		WebSocketManager.RemoveMessageListener("playerMatchData", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerMatchData));
		WebSocketManager.RemoveMessageListener("playerStatistics", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerStatistics));
		WebSocketManager.RemoveMessageListener("playerKey", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerKey));
		WebSocketManager.RemoveMessageListener("playerProbeRttRequest", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerProbeRttRequest));
		WebSocketManager.RemoveMessageListener("playerMaxRttRequest", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerMaxRttRequest));
		WebSocketManager.RemoveMessageListener("playerStartTransactionResponse", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerStartTransactionResponse));
		WebSocketManager.RemoveMessageListener("playerFinalizeTransactionResponse", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnPlayerFinalizeTransactionResponse));
		WebSocketManager.RemoveMessageListener("serverAuthenticateResponse", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnServerAuthenticateResponse));
		WebSocketManager.RemoveMessageListener("serverUnauthenticateResponse", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnServerUnauthenticateResponse));
		WebSocketManager.RemoveMessageListener("serverData", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnServerData));
		WebSocketManager.RemoveMessageListener("serverMatchData", new Action<Dictionary<string, object>>(BackendManagerController.WebSocket_Event_OnServerMatchData));
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x0001FD80 File Offset: 0x0001DF80
	private static int? PingProbe(Probe probe, int connectTimeout, int responseTimeout)
	{
		EndPoint endPoint = new EndPoint(probe.host, probe.port);
		TCPClient tcpClient = new TCPClient(endPoint, connectTimeout, 1000);
		double pingTimestamp = 0.0;
		int? rtt = null;
		ManualResetEventSlim responseEvent = new ManualResetEventSlim(false);
		tcpClient.OnConnected += delegate()
		{
			tcpClient.SendMessage("ping");
		};
		tcpClient.OnMessageSent += delegate(string message)
		{
			pingTimestamp = Utils.GetTimestamp();
		};
		tcpClient.OnMessageReceived += delegate(string message)
		{
			rtt = new int?((int)(Utils.GetTimestamp() - pingTimestamp));
			responseEvent.Set();
		};
		tcpClient.Connect();
		if (tcpClient.IsConnected)
		{
			responseEvent.Wait(responseTimeout);
			tcpClient.Disconnect();
		}
		return rtt;
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x0001FE5C File Offset: 0x0001E05C
	private static void Event_OnGetTicketForWebApiResponse(Dictionary<string, object> message)
	{
		string value = (string)message["ticket"];
		BackendManager.SetPlayerState(new Dictionary<string, object>
		{
			{
				"authenticationPhase",
				AuthenticationPhase.Authenticating
			}
		});
		WebSocketManager.Emit("playerAuthenticateRequest", new Dictionary<string, object>
		{
			{
				"ticket",
				value
			}
		}, "playerAuthenticateResponse");
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x0001FEB5 File Offset: 0x0001E0B5
	private static void Event_OnFooterClickCreateParty(Dictionary<string, object> message)
	{
		WebSocketManager.Emit("playerCreatePartyRequest", null, "playerCreatePartyResponse");
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x0001FEC7 File Offset: 0x0001E0C7
	private static void Event_OnFooterClickLeaveParty(Dictionary<string, object> message)
	{
		WebSocketManager.Emit("playerLeavePartyRequest", null, "playerLeavePartyResponse");
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x0001FED9 File Offset: 0x0001E0D9
	private static void Event_OnFooterClickDisbandParty(Dictionary<string, object> message)
	{
		WebSocketManager.Emit("playerDisbandPartyRequest", null, "playerDisbandPartyResponse");
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x0001FEEC File Offset: 0x0001E0EC
	private static void Event_OnLobbyCreated(Dictionary<string, object> message)
	{
		string value = (string)message["lobbyId"];
		WebSocketManager.Emit("playerUpdatePartyRequest", new Dictionary<string, object>
		{
			{
				"steamLobbyId",
				value
			}
		}, "playerUpdatePartyResponse");
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x0001FF2C File Offset: 0x0001E12C
	private static void Event_OnLobbyEntered(Dictionary<string, object> message)
	{
		string text = (string)message["lobbyId"];
		string text2 = (string)message["ownerSteamId"];
		BackendManagerController.Logger.Info(string.Concat(new string[]
		{
			"Entered lobby ",
			text,
			" owned by ",
			text2,
			" (player's Steam ID: ",
			BackendManager.PlayerState.PlayerData.steamId,
			")"
		}));
		if (text2 != BackendManager.PlayerState.PlayerData.steamId)
		{
			WebSocketManager.Emit("playerJoinPartyRequest", new Dictionary<string, object>
			{
				{
					"steamLobbyId",
					text
				}
			}, "playerJoinPartyResponse");
		}
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x0001FFE4 File Offset: 0x0001E1E4
	private static void Event_OnMatchmakingMatchingClickStartMatchmaking(Dictionary<string, object> message)
	{
		string[] value = (string[])message["poolIds"];
		WebSocketManager.Emit("playerStartMatchmakingRequest", new Dictionary<string, object>
		{
			{
				"poolIds",
				value
			},
			{
				"maxRtt",
				SettingsManager.MaxMatchmakingPing
			}
		}, "playerStartMatchmakingResponse");
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x00020037 File Offset: 0x0001E237
	private static void Event_OnMatchmakingMatchingClickClose(Dictionary<string, object> message)
	{
		WebSocketManager.Emit("playerStopMatchmakingRequest", null, "playerStopMatchmakingResponse");
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x0002004C File Offset: 0x0001E24C
	private static void Event_OnPopupClickOk(Dictionary<string, object> message)
	{
		Popup popup = (Popup)message["popup"];
		if (popup.Name == "identity")
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)popup.Data;
			string value = (string)dictionary["username"];
			int num = (int)dictionary["number"];
			WebSocketManager.Emit("playerSetIdentityRequest", new Dictionary<string, object>
			{
				{
					"username",
					value
				},
				{
					"number",
					num
				}
			}, "playerSetIdentityResponse");
		}
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x000200DC File Offset: 0x0001E2DC
	private static void Event_OnAppearanceClickPurchaseItem(Dictionary<string, object> message)
	{
		Item item = (Item)message["item"];
		BackendManager.SetTransactionState(new Dictionary<string, object>
		{
			{
				"phase",
				TransactionPhase.Starting
			}
		});
		if (!SteamIntegrationManager.IsOverlayEnabled)
		{
			BackendManager.SetTransactionState(new Dictionary<string, object>
			{
				{
					"phase",
					TransactionPhase.None
				}
			});
			return;
		}
		WebSocketManager.Emit("playerStartTransactionRequest", new Dictionary<string, object>
		{
			{
				"itemId",
				item.id
			}
		}, "playerStartTransactionResponse");
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x00020164 File Offset: 0x0001E364
	private static void Event_OnMicroTxnAuthorizationResponse(Dictionary<string, object> message)
	{
		if ((bool)message["authorized"])
		{
			WebSocketManager.Emit("playerFinalizeTransactionRequest", null, "playerFinalizeTransactionResponse");
			return;
		}
		WebSocketManager.Emit("playerCancelTransaction", null, null);
		BackendManager.SetTransactionState(new Dictionary<string, object>
		{
			{
				"phase",
				TransactionPhase.None
			}
		});
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x000201BC File Offset: 0x0001E3BC
	private static void Event_OnNewServerClickStart(Dictionary<string, object> message)
	{
		if ((string)message["type"] != "dedicated")
		{
			return;
		}
		string value = (string)message["name"];
		int num = (int)message["maxPlayers"];
		string value2 = (string)message["password"];
		string value3 = (string)message["probeId"];
		WebSocketManager.Emit("playerDeployServerRequest", new Dictionary<string, object>
		{
			{
				"name",
				value
			},
			{
				"maxPlayers",
				num
			},
			{
				"password",
				value2
			},
			{
				"probeId",
				value3
			}
		}, "playerDeployServerResponse");
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x00020274 File Offset: 0x0001E474
	private static void WebSocket_Event_OnDisconnected(Dictionary<string, object> message)
	{
		BackendManager.SetPlayerState(new Dictionary<string, object>
		{
			{
				"steamId",
				null
			},
			{
				"playerData",
				null
			},
			{
				"partyData",
				null
			},
			{
				"key",
				null
			},
			{
				"authenticationPhase",
				AuthenticationPhase.None
			}
		});
		BackendManager.SetServerState(new Dictionary<string, object>
		{
			{
				"serverData",
				null
			},
			{
				"matchData",
				null
			},
			{
				"authenticationPhase",
				AuthenticationPhase.None
			}
		});
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x00020300 File Offset: 0x0001E500
	private static void WebSocket_Event_OnPlayerAuthenticateResponse(Dictionary<string, object> message)
	{
		PlayerAuthenticateResponse data = ((InMessage)message["inMessage"]).GetData<PlayerAuthenticateResponse>();
		BackendManager.SetPlayerState(new Dictionary<string, object>
		{
			{
				"authenticationPhase",
				data.success ? AuthenticationPhase.Authenticated : AuthenticationPhase.None
			}
		});
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x0002034C File Offset: 0x0001E54C
	private static void WebSocket_Event_OnPlayerData(Dictionary<string, object> message)
	{
		InMessage inMessage = (InMessage)message["inMessage"];
		BackendManager.SetPlayerState(new Dictionary<string, object>
		{
			{
				"playerData",
				inMessage.GetData<PlayerDataMessage>().player
			}
		});
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0002038C File Offset: 0x0001E58C
	private static void WebSocket_Event_OnPlayerPartyData(Dictionary<string, object> message)
	{
		InMessage inMessage = (InMessage)message["inMessage"];
		BackendManager.SetPlayerState(new Dictionary<string, object>
		{
			{
				"partyData",
				inMessage.GetData<PlayerPartyDataMessage>().party
			}
		});
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x000203CC File Offset: 0x0001E5CC
	private static void WebSocket_Event_OnPlayerGroupData(Dictionary<string, object> message)
	{
		InMessage inMessage = (InMessage)message["inMessage"];
		BackendManager.SetPlayerState(new Dictionary<string, object>
		{
			{
				"groupData",
				inMessage.GetData<PlayerGroupDataMessage>().group
			}
		});
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x0002040C File Offset: 0x0001E60C
	private static void WebSocket_Event_OnPlayerMatchData(Dictionary<string, object> message)
	{
		InMessage inMessage = (InMessage)message["inMessage"];
		BackendManager.SetPlayerState(new Dictionary<string, object>
		{
			{
				"matchData",
				inMessage.GetData<PlayerMatchDataMessage>().match
			}
		});
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x0002044C File Offset: 0x0001E64C
	private static void WebSocket_Event_OnPlayerStatistics(Dictionary<string, object> message)
	{
		InMessage inMessage = (InMessage)message["inMessage"];
		BackendManager.SetPlayerState(new Dictionary<string, object>
		{
			{
				"playerStatistics",
				inMessage.GetData<PlayerStatisticsMessage>().statistics
			}
		});
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x0002048C File Offset: 0x0001E68C
	private static void WebSocket_Event_OnPlayerKey(Dictionary<string, object> message)
	{
		InMessage inMessage = (InMessage)message["inMessage"];
		BackendManager.SetPlayerState(new Dictionary<string, object>
		{
			{
				"key",
				inMessage.GetData<PlayerKeyMessage>().key
			}
		});
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x000204CC File Offset: 0x0001E6CC
	private static void WebSocket_Event_OnPlayerProbeRttRequest(Dictionary<string, object> message)
	{
		BackendManagerController.<>c__DisplayClass31_0 CS$<>8__locals1 = new BackendManagerController.<>c__DisplayClass31_0();
		CS$<>8__locals1.inMessage = (InMessage)message["inMessage"];
		CS$<>8__locals1.probes = CS$<>8__locals1.inMessage.GetData<PlayerProbeRttRequestMessage>().probes;
		Task.Run(delegate()
		{
			BackendManagerController.<>c__DisplayClass31_0.<<WebSocket_Event_OnPlayerProbeRttRequest>b__0>d <<WebSocket_Event_OnPlayerProbeRttRequest>b__0>d;
			<<WebSocket_Event_OnPlayerProbeRttRequest>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
			<<WebSocket_Event_OnPlayerProbeRttRequest>b__0>d.<>4__this = CS$<>8__locals1;
			<<WebSocket_Event_OnPlayerProbeRttRequest>b__0>d.<>1__state = -1;
			<<WebSocket_Event_OnPlayerProbeRttRequest>b__0>d.<>t__builder.Start<BackendManagerController.<>c__DisplayClass31_0.<<WebSocket_Event_OnPlayerProbeRttRequest>b__0>d>(ref <<WebSocket_Event_OnPlayerProbeRttRequest>b__0>d);
			return <<WebSocket_Event_OnPlayerProbeRttRequest>b__0>d.<>t__builder.Task;
		});
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0002051B File Offset: 0x0001E71B
	private static void WebSocket_Event_OnPlayerMaxRttRequest(Dictionary<string, object> message)
	{
		((InMessage)message["inMessage"]).Respond(new Dictionary<string, object>
		{
			{
				"maxRtt",
				SettingsManager.MaxMatchmakingPing
			}
		});
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x0002054C File Offset: 0x0001E74C
	private static void WebSocket_Event_OnPlayerStartTransactionResponse(Dictionary<string, object> message)
	{
		if (((InMessage)message["inMessage"]).GetData<PlayerStartTransactionResponse>().success)
		{
			BackendManager.SetTransactionState(new Dictionary<string, object>
			{
				{
					"phase",
					TransactionPhase.Started
				}
			});
			return;
		}
		BackendManager.SetTransactionState(new Dictionary<string, object>
		{
			{
				"phase",
				TransactionPhase.None
			}
		});
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x000205AC File Offset: 0x0001E7AC
	private static void WebSocket_Event_OnPlayerFinalizeTransactionResponse(Dictionary<string, object> message)
	{
		((InMessage)message["inMessage"]).GetData<PlayerFinalizeTransactionResponse>();
		BackendManager.SetTransactionState(new Dictionary<string, object>
		{
			{
				"phase",
				TransactionPhase.None
			}
		});
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x000205E0 File Offset: 0x0001E7E0
	private static void WebSocket_Event_OnServerAuthenticateResponse(Dictionary<string, object> message)
	{
		ServerAuthenticateResponse data = ((InMessage)message["inMessage"]).GetData<ServerAuthenticateResponse>();
		BackendManager.SetServerState(new Dictionary<string, object>
		{
			{
				"authenticationPhase",
				data.success ? AuthenticationPhase.Authenticated : AuthenticationPhase.None
			}
		});
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x00020629 File Offset: 0x0001E829
	private static void WebSocket_Event_OnServerUnauthenticateResponse(Dictionary<string, object> message)
	{
		BackendManagerController.serverHasAuthenticated = false;
		BackendManagerController.StopServerUnlistedWatchdog();
		BackendManager.SetServerState(new Dictionary<string, object>
		{
			{
				"authenticationPhase",
				AuthenticationPhase.None
			}
		});
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x00020654 File Offset: 0x0001E854
	private static void Event_OnServerStateChanged(Dictionary<string, object> message)
	{
		ref ServerState ptr = (ServerState)message["oldServerState"];
		ServerState serverState = (ServerState)message["newServerState"];
		if (ptr.AuthenticationPhase == serverState.AuthenticationPhase)
		{
			return;
		}
		if (serverState.AuthenticationPhase == AuthenticationPhase.Authenticated)
		{
			if (BackendManagerController.serverUnlistedWatchdogTween != null)
			{
				int num = (int)(Time.realtimeSinceStartupAsDouble - BackendManagerController.serverUnlistedSinceTime);
				BackendManagerController.Logger.Info(string.Format("Server re-listed on the public server browser after ~{0}s unlisted.", num));
			}
			BackendManagerController.serverHasAuthenticated = true;
			BackendManagerController.StopServerUnlistedWatchdog();
			return;
		}
		if (!BackendManagerController.serverHasAuthenticated)
		{
			return;
		}
		BackendManagerController.StartServerUnlistedWatchdog();
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x000206E0 File Offset: 0x0001E8E0
	private static void StartServerUnlistedWatchdog()
	{
		if (BackendManagerController.serverUnlistedWatchdogTween != null)
		{
			return;
		}
		BackendManagerController.serverUnlistedSinceTime = Time.realtimeSinceStartupAsDouble;
		BackendManagerController.serverLastUnlistedAlertTime = 0.0;
		BackendManagerController.serverUnlistedWatchdogTween = DOVirtual.DelayedCall(15f, new TweenCallback(BackendManagerController.CheckServerUnlisted), true).SetLoops(-1);
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x0002072F File Offset: 0x0001E92F
	private static void StopServerUnlistedWatchdog()
	{
		Tween tween = BackendManagerController.serverUnlistedWatchdogTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		BackendManagerController.serverUnlistedWatchdogTween = null;
		BackendManagerController.serverLastUnlistedAlertTime = 0.0;
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x00020758 File Offset: 0x0001E958
	private static void CheckServerUnlisted()
	{
		double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
		double num = realtimeSinceStartupAsDouble - BackendManagerController.serverUnlistedSinceTime;
		if (num < 30.0)
		{
			return;
		}
		if (BackendManagerController.serverLastUnlistedAlertTime > 0.0 && realtimeSinceStartupAsDouble - BackendManagerController.serverLastUnlistedAlertTime < 300.0)
		{
			return;
		}
		BackendManagerController.serverLastUnlistedAlertTime = realtimeSinceStartupAsDouble;
		BackendManagerController.Logger.Error(string.Format("Server has been unlisted from the public server browser for ~{0}s ", (int)num) + string.Format("(AuthenticationPhase={0}, websocket ", BackendManager.ServerState.AuthenticationPhase) + (WebSocketManager.IsConnected ? "connected" : "disconnected") + "). Players cannot find it until it re-authenticates with the backend.");
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x000207FC File Offset: 0x0001E9FC
	private static void WebSocket_Event_OnServerData(Dictionary<string, object> message)
	{
		InMessage inMessage = (InMessage)message["inMessage"];
		BackendManager.SetServerState(new Dictionary<string, object>
		{
			{
				"serverData",
				inMessage.GetData<ServerDataMessage>().server
			}
		});
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0002083C File Offset: 0x0001EA3C
	private static void WebSocket_Event_OnServerMatchData(Dictionary<string, object> message)
	{
		InMessage inMessage = (InMessage)message["inMessage"];
		BackendManager.SetServerState(new Dictionary<string, object>
		{
			{
				"matchData",
				inMessage.GetData<ServerMatchDataMessage>().match
			}
		});
	}

	// Token: 0x040003B5 RID: 949
	private static readonly global::Logger Logger = new global::Logger("BackendManagerController");

	// Token: 0x040003B6 RID: 950
	private const float ServerUnlistedCheckIntervalSeconds = 15f;

	// Token: 0x040003B7 RID: 951
	private const float ServerUnlistedAlertDelaySeconds = 30f;

	// Token: 0x040003B8 RID: 952
	private const float ServerUnlistedReAlertIntervalSeconds = 300f;

	// Token: 0x040003B9 RID: 953
	private static bool serverHasAuthenticated;

	// Token: 0x040003BA RID: 954
	private static Tween serverUnlistedWatchdogTween;

	// Token: 0x040003BB RID: 955
	private static double serverUnlistedSinceTime;

	// Token: 0x040003BC RID: 956
	private static double serverLastUnlistedAlertTime;
}
