using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;

// Token: 0x020000AE RID: 174
public static class BackendManager
{
	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0001EEE5 File Offset: 0x0001D0E5
	// (set) Token: 0x060005A5 RID: 1445 RVA: 0x0001EEEC File Offset: 0x0001D0EC
	public static PlayerState PlayerState
	{
		get
		{
			return BackendManager.playerState;
		}
		set
		{
			if (BackendManager.playerState.Equals(value))
			{
				return;
			}
			PlayerState oldPlayerState = BackendManager.playerState;
			BackendManager.playerState = value;
			BackendManager.OnPlayerStateChanged(oldPlayerState, BackendManager.playerState);
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060005A6 RID: 1446 RVA: 0x0001EF11 File Offset: 0x0001D111
	// (set) Token: 0x060005A7 RID: 1447 RVA: 0x0001EF18 File Offset: 0x0001D118
	public static ServerState ServerState
	{
		get
		{
			return BackendManager.serverState;
		}
		set
		{
			if (BackendManager.serverState.Equals(value))
			{
				return;
			}
			ServerState oldServerState = BackendManager.serverState;
			BackendManager.serverState = value;
			BackendManager.OnServerStateChanged(oldServerState, BackendManager.serverState);
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060005A8 RID: 1448 RVA: 0x0001EF3D File Offset: 0x0001D13D
	// (set) Token: 0x060005A9 RID: 1449 RVA: 0x0001EF44 File Offset: 0x0001D144
	public static TransactionState TransactionState
	{
		get
		{
			return BackendManager.transactionState;
		}
		set
		{
			if (BackendManager.transactionState.Equals(value))
			{
				return;
			}
			TransactionState transactionState = BackendManager.transactionState;
			BackendManager.transactionState = value;
			EventManager.TriggerEvent("Event_OnTransactionStateChanged", new Dictionary<string, object>
			{
				{
					"oldTransactionState",
					transactionState
				},
				{
					"newTransactionState",
					BackendManager.transactionState
				}
			});
		}
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x0001EFA0 File Offset: 0x0001D1A0
	public static void Initialize()
	{
		BackendManagerController.Initialize();
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x0001EFA7 File Offset: 0x0001D1A7
	public static void Dispose()
	{
		BackendManagerController.Dispose();
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x0001EFB0 File Offset: 0x0001D1B0
	public static void SetPlayerState(Dictionary<string, object> updates)
	{
		BackendManager.PlayerState = new PlayerState
		{
			AuthenticationPhase = (updates.ContainsKey("authenticationPhase") ? ((AuthenticationPhase)updates["authenticationPhase"]) : BackendManager.PlayerState.AuthenticationPhase),
			PlayerData = (updates.ContainsKey("playerData") ? ((PlayerData)updates["playerData"]) : BackendManager.PlayerState.PlayerData),
			PartyData = (updates.ContainsKey("partyData") ? ((PlayerPartyData)updates["partyData"]) : BackendManager.PlayerState.PartyData),
			GroupData = (updates.ContainsKey("groupData") ? ((PlayerGroupData)updates["groupData"]) : BackendManager.PlayerState.GroupData),
			MatchData = (updates.ContainsKey("matchData") ? ((PlayerMatchData)updates["matchData"]) : BackendManager.PlayerState.MatchData),
			PlayerStatistics = (updates.ContainsKey("playerStatistics") ? ((PlayerStatistics)updates["playerStatistics"]) : BackendManager.PlayerState.PlayerStatistics),
			Key = (updates.ContainsKey("key") ? ((string)updates["key"]) : BackendManager.PlayerState.Key)
		};
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x0001F11C File Offset: 0x0001D31C
	public static void SetServerState(Dictionary<string, object> updates)
	{
		BackendManager.ServerState = new ServerState
		{
			AuthenticationPhase = (updates.ContainsKey("authenticationPhase") ? ((AuthenticationPhase)updates["authenticationPhase"]) : BackendManager.ServerState.AuthenticationPhase),
			ServerData = (updates.ContainsKey("serverData") ? ((ServerData)updates["serverData"]) : BackendManager.ServerState.ServerData),
			MatchData = (updates.ContainsKey("matchData") ? ((ServerMatchData)updates["matchData"]) : BackendManager.ServerState.MatchData)
		};
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x0001F1C8 File Offset: 0x0001D3C8
	public static void SetTransactionState(Dictionary<string, object> updates)
	{
		BackendManager.TransactionState = new TransactionState
		{
			Phase = (updates.ContainsKey("phase") ? ((TransactionPhase)updates["phase"]) : BackendManager.TransactionState.Phase)
		};
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x0001F214 File Offset: 0x0001D414
	private static void OnPlayerStateChanged(PlayerState oldPlayerState, PlayerState newPlayerState)
	{
		EventManager.TriggerEvent("Event_OnPlayerStateChanged", new Dictionary<string, object>
		{
			{
				"oldPlayerState",
				oldPlayerState
			},
			{
				"newPlayerState",
				newPlayerState
			}
		});
		if (oldPlayerState.PlayerData != newPlayerState.PlayerData)
		{
			EventManager.TriggerEvent("Event_OnPlayerDataChanged", new Dictionary<string, object>
			{
				{
					"oldPlayerData",
					oldPlayerState.PlayerData
				},
				{
					"newPlayerData",
					newPlayerState.PlayerData
				}
			});
			if (newPlayerState.PlayerData != null)
			{
				bool flag = BackendUtils.GetActivePlayerDataBan(oldPlayerState.PlayerData) != null;
				bool flag2 = BackendUtils.GetActivePlayerDataMute(oldPlayerState.PlayerData) != null;
				bool flag3 = BackendUtils.GetActivePlayerDataCooldown(oldPlayerState.PlayerData) != null;
				PlayerBan activePlayerDataBan = BackendUtils.GetActivePlayerDataBan(newPlayerState.PlayerData);
				PlayerMute activePlayerDataMute = BackendUtils.GetActivePlayerDataMute(newPlayerState.PlayerData);
				PlayerCooldown activePlayerDataCooldown = BackendUtils.GetActivePlayerDataCooldown(newPlayerState.PlayerData);
				bool flag4 = activePlayerDataBan != null;
				bool flag5 = activePlayerDataMute != null;
				bool flag6 = activePlayerDataCooldown != null;
				if (flag4 && !flag)
				{
					EventManager.TriggerEvent("Event_OnPlayerBanned", new Dictionary<string, object>
					{
						{
							"reason",
							activePlayerDataBan.reason
						},
						{
							"expiresAt",
							activePlayerDataBan.expiresAt
						}
					});
				}
				else if (!flag4 && flag)
				{
					EventManager.TriggerEvent("Event_OnPlayerUnbanned", null);
				}
				if (flag5 && !flag2)
				{
					EventManager.TriggerEvent("Event_OnPlayerMuted", new Dictionary<string, object>
					{
						{
							"reason",
							activePlayerDataMute.reason
						},
						{
							"expiresAt",
							activePlayerDataMute.expiresAt
						}
					});
				}
				else if (!flag5 && flag2)
				{
					EventManager.TriggerEvent("Event_OnPlayerUnmuted", null);
				}
				if (flag6 && !flag3)
				{
					EventManager.TriggerEvent("Event_OnPlayerCooldown", new Dictionary<string, object>
					{
						{
							"expiresAt",
							activePlayerDataCooldown.expiresAt
						}
					});
				}
				else if (!flag6 && flag3)
				{
					EventManager.TriggerEvent("Event_OnPlayerCooldownExpired", null);
				}
			}
		}
		if (oldPlayerState.PartyData != newPlayerState.PartyData)
		{
			EventManager.TriggerEvent("Event_OnPlayerPartyDataChanged", new Dictionary<string, object>
			{
				{
					"oldPlayerPartyData",
					oldPlayerState.PartyData
				},
				{
					"newPlayerPartyData",
					newPlayerState.PartyData
				}
			});
		}
		if (oldPlayerState.GroupData != newPlayerState.GroupData)
		{
			EventManager.TriggerEvent("Event_OnPlayerGroupDataChanged", new Dictionary<string, object>
			{
				{
					"oldPlayerGroupData",
					oldPlayerState.GroupData
				},
				{
					"newPlayerGroupData",
					newPlayerState.GroupData
				}
			});
			bool flag7 = oldPlayerState.GroupData == null && newPlayerState.GroupData != null;
			bool flag8 = oldPlayerState.GroupData != null && newPlayerState.GroupData == null;
			if (flag7)
			{
				BackendManager.StartMatchingTicker(0);
			}
			else if (flag8)
			{
				BackendManager.StopMatchingTicker();
			}
		}
		if (oldPlayerState.MatchData != newPlayerState.MatchData)
		{
			EventManager.TriggerEvent("Event_OnPlayerMatchDataChanged", new Dictionary<string, object>
			{
				{
					"oldPlayerMatchData",
					oldPlayerState.MatchData
				},
				{
					"newPlayerMatchData",
					newPlayerState.MatchData
				}
			});
			PlayerMatchData matchData = oldPlayerState.MatchData;
			bool flag9;
			if (matchData == null || matchData.JoinTimeoutRemainingSeconds == null)
			{
				PlayerMatchData matchData2 = newPlayerState.MatchData;
				flag9 = (matchData2 != null && matchData2.JoinTimeoutRemainingSeconds != null);
			}
			else
			{
				flag9 = false;
			}
			PlayerMatchData matchData3 = oldPlayerState.MatchData;
			bool flag10;
			if (matchData3 != null && matchData3.JoinTimeoutRemainingSeconds != null)
			{
				PlayerMatchData matchData4 = newPlayerState.MatchData;
				flag10 = (matchData4 == null || matchData4.JoinTimeoutRemainingSeconds == null);
			}
			else
			{
				flag10 = false;
			}
			bool flag11 = flag10;
			if (flag9)
			{
				BackendManager.StartMatchJoinTimeoutTicker(newPlayerState.MatchData.JoinTimeoutRemainingSeconds.Value);
			}
			else if (flag11)
			{
				BackendManager.StopMatchJoinTimeoutTicker();
			}
		}
		if (oldPlayerState.PlayerStatistics != newPlayerState.PlayerStatistics)
		{
			EventManager.TriggerEvent("Event_OnPlayerStatisticsChanged", new Dictionary<string, object>
			{
				{
					"oldPlayerStatistics",
					oldPlayerState.PlayerStatistics
				},
				{
					"newPlayerStatistics",
					newPlayerState.PlayerStatistics
				}
			});
		}
		if (oldPlayerState.Key != newPlayerState.Key)
		{
			EventManager.TriggerEvent("Event_OnPlayerKeyChanged", new Dictionary<string, object>
			{
				{
					"oldKey",
					oldPlayerState.Key
				},
				{
					"newKey",
					newPlayerState.Key
				}
			});
		}
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x0001F618 File Offset: 0x0001D818
	private static void OnServerStateChanged(ServerState oldServerState, ServerState newServerState)
	{
		EventManager.TriggerEvent("Event_OnServerStateChanged", new Dictionary<string, object>
		{
			{
				"oldServerState",
				oldServerState
			},
			{
				"newServerState",
				newServerState
			}
		});
		if (oldServerState.ServerData != newServerState.ServerData)
		{
			EventManager.TriggerEvent("Event_OnServerDataChanged", new Dictionary<string, object>
			{
				{
					"oldServerData",
					oldServerState.ServerData
				},
				{
					"newServerData",
					newServerState.ServerData
				}
			});
		}
		if (oldServerState.MatchData != newServerState.MatchData)
		{
			EventManager.TriggerEvent("Event_OnServerMatchDataChanged", new Dictionary<string, object>
			{
				{
					"oldServerMatchData",
					oldServerState.MatchData
				},
				{
					"newServerMatchData",
					newServerState.MatchData
				}
			});
		}
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x0001F6D4 File Offset: 0x0001D8D4
	private static void StartMatchingTicker(int startingTick)
	{
		EventManager.TriggerEvent("Event_OnMatchingTickerStarted", new Dictionary<string, object>
		{
			{
				"startingTick",
				startingTick
			}
		});
		BackendManager.matchingTick = startingTick;
		Tween tween = BackendManager.matchingTickerTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		BackendManager.matchingTickerTween = DOVirtual.DelayedCall(1f, new TweenCallback(BackendManager.<StartMatchingTicker>g__Tick|23_0), true).SetLoops(-1);
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x0001F73A File Offset: 0x0001D93A
	private static void StopMatchingTicker()
	{
		BackendManager.matchingTick = 0;
		Tween tween = BackendManager.matchingTickerTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		BackendManager.matchingTickerTween = null;
		EventManager.TriggerEvent("Event_OnMatchingTickerStopped", null);
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x0001F764 File Offset: 0x0001D964
	private static void StartMatchJoinTimeoutTicker(int startingTick)
	{
		EventManager.TriggerEvent("Event_OnMatchJoinTimeoutTickerStarted", new Dictionary<string, object>
		{
			{
				"startingTick",
				startingTick
			}
		});
		BackendManager.joinTimeoutTick = startingTick;
		Tween tween = BackendManager.matchJoinTimeoutTickerTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		BackendManager.matchJoinTimeoutTickerTween = DOVirtual.DelayedCall(1f, new TweenCallback(BackendManager.<StartMatchJoinTimeoutTicker>g__Tick|25_0), true).SetLoops(-1);
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x0001F7CA File Offset: 0x0001D9CA
	private static void StopMatchJoinTimeoutTicker()
	{
		BackendManager.joinTimeoutTick = 0;
		Tween tween = BackendManager.matchJoinTimeoutTickerTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		BackendManager.matchJoinTimeoutTickerTween = null;
		EventManager.TriggerEvent("Event_OnMatchJoinTimeoutTickerStopped", null);
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x0001F7F4 File Offset: 0x0001D9F4
	[CompilerGenerated]
	internal static void <StartMatchingTicker>g__Tick|23_0()
	{
		BackendManager.matchingTick++;
		EventManager.TriggerEvent("Event_OnMatchingTickerTick", new Dictionary<string, object>
		{
			{
				"tick",
				BackendManager.matchingTick
			}
		});
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x0001F826 File Offset: 0x0001DA26
	[CompilerGenerated]
	internal static void <StartMatchJoinTimeoutTicker>g__Tick|25_0()
	{
		if (BackendManager.joinTimeoutTick <= 0)
		{
			BackendManager.StopMatchJoinTimeoutTicker();
			return;
		}
		BackendManager.joinTimeoutTick--;
		EventManager.TriggerEvent("Event_OnMatchJoinTimeoutTickerTick", new Dictionary<string, object>
		{
			{
				"tick",
				BackendManager.joinTimeoutTick
			}
		});
	}

	// Token: 0x040003AE RID: 942
	private static PlayerState playerState;

	// Token: 0x040003AF RID: 943
	private static ServerState serverState;

	// Token: 0x040003B0 RID: 944
	private static TransactionState transactionState;

	// Token: 0x040003B1 RID: 945
	private static Tween matchingTickerTween;

	// Token: 0x040003B2 RID: 946
	private static int matchingTick;

	// Token: 0x040003B3 RID: 947
	private static Tween matchJoinTimeoutTickerTween;

	// Token: 0x040003B4 RID: 948
	private static int joinTimeoutTick;
}
