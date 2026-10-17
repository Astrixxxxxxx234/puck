using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class Player : NetworkBehaviour
{
	// Token: 0x1700002D RID: 45
	// (get) Token: 0x0600027B RID: 635 RVA: 0x00010A4A File Offset: 0x0000EC4A
	// (set) Token: 0x0600027C RID: 636 RVA: 0x00010A52 File Offset: 0x0000EC52
	[HideInInspector]
	public float ChatTickets { get; private set; }

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x0600027D RID: 637 RVA: 0x00010A5B File Offset: 0x0000EC5B
	[HideInInspector]
	public bool IsChatAvailable
	{
		get
		{
			return this.ChatTickets >= 1f;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x0600027E RID: 638 RVA: 0x00010A6D File Offset: 0x0000EC6D
	[HideInInspector]
	public PlayerPhase Phase
	{
		get
		{
			return this.GameState.Value.Phase;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x0600027F RID: 639 RVA: 0x00010A7F File Offset: 0x0000EC7F
	[HideInInspector]
	public PlayerTeam Team
	{
		get
		{
			return this.GameState.Value.Team;
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000280 RID: 640 RVA: 0x00010A91 File Offset: 0x0000EC91
	[HideInInspector]
	public PlayerRole Role
	{
		get
		{
			return this.GameState.Value.Role;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000281 RID: 641 RVA: 0x00010AA3 File Offset: 0x0000ECA3
	[HideInInspector]
	public int FlagID
	{
		get
		{
			return this.CustomizationState.Value.FlagID;
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000282 RID: 642 RVA: 0x00010AB5 File Offset: 0x0000ECB5
	[HideInInspector]
	public int HeadgearIDBlueAttacker
	{
		get
		{
			return this.CustomizationState.Value.HeadgearIDBlueAttacker;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000283 RID: 643 RVA: 0x00010AC7 File Offset: 0x0000ECC7
	[HideInInspector]
	public int HeadgearIDRedAttacker
	{
		get
		{
			return this.CustomizationState.Value.HeadgearIDRedAttacker;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000284 RID: 644 RVA: 0x00010AD9 File Offset: 0x0000ECD9
	[HideInInspector]
	public int HeadgearIDBlueGoalie
	{
		get
		{
			return this.CustomizationState.Value.HeadgearIDBlueGoalie;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000285 RID: 645 RVA: 0x00010AEB File Offset: 0x0000ECEB
	[HideInInspector]
	public int HeadgearIDRedGoalie
	{
		get
		{
			return this.CustomizationState.Value.HeadgearIDRedGoalie;
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000286 RID: 646 RVA: 0x00010AFD File Offset: 0x0000ECFD
	[HideInInspector]
	public int MustacheID
	{
		get
		{
			return this.CustomizationState.Value.MustacheID;
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000287 RID: 647 RVA: 0x00010B0F File Offset: 0x0000ED0F
	[HideInInspector]
	public int BeardID
	{
		get
		{
			return this.CustomizationState.Value.BeardID;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000288 RID: 648 RVA: 0x00010B21 File Offset: 0x0000ED21
	[HideInInspector]
	public int JerseyIDBlueAttacker
	{
		get
		{
			return this.CustomizationState.Value.JerseyIDBlueAttacker;
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000289 RID: 649 RVA: 0x00010B33 File Offset: 0x0000ED33
	[HideInInspector]
	public int JerseyIDRedAttacker
	{
		get
		{
			return this.CustomizationState.Value.JerseyIDRedAttacker;
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x0600028A RID: 650 RVA: 0x00010B45 File Offset: 0x0000ED45
	[HideInInspector]
	public int JerseyIDBlueGoalie
	{
		get
		{
			return this.CustomizationState.Value.JerseyIDBlueGoalie;
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x0600028B RID: 651 RVA: 0x00010B57 File Offset: 0x0000ED57
	[HideInInspector]
	public int JerseyIDRedGoalie
	{
		get
		{
			return this.CustomizationState.Value.JerseyIDRedGoalie;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x0600028C RID: 652 RVA: 0x00010B69 File Offset: 0x0000ED69
	[HideInInspector]
	public int StickSkinIDBlueAttacker
	{
		get
		{
			return this.CustomizationState.Value.StickSkinIDBlueAttacker;
		}
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x0600028D RID: 653 RVA: 0x00010B7B File Offset: 0x0000ED7B
	[HideInInspector]
	public int StickSkinIDRedAttacker
	{
		get
		{
			return this.CustomizationState.Value.StickSkinIDRedAttacker;
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x0600028E RID: 654 RVA: 0x00010B8D File Offset: 0x0000ED8D
	[HideInInspector]
	public int StickSkinIDBlueGoalie
	{
		get
		{
			return this.CustomizationState.Value.StickSkinIDBlueGoalie;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x0600028F RID: 655 RVA: 0x00010B9F File Offset: 0x0000ED9F
	[HideInInspector]
	public int StickSkinIDRedGoalie
	{
		get
		{
			return this.CustomizationState.Value.StickSkinIDRedGoalie;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000290 RID: 656 RVA: 0x00010BB1 File Offset: 0x0000EDB1
	[HideInInspector]
	public int StickShaftTapeIDBlueAttacker
	{
		get
		{
			return this.CustomizationState.Value.StickShaftTapeIDBlueAttacker;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000291 RID: 657 RVA: 0x00010BC3 File Offset: 0x0000EDC3
	[HideInInspector]
	public int StickShaftTapeIDRedAttacker
	{
		get
		{
			return this.CustomizationState.Value.StickShaftTapeIDRedAttacker;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000292 RID: 658 RVA: 0x00010BD5 File Offset: 0x0000EDD5
	[HideInInspector]
	public int StickShaftTapeIDBlueGoalie
	{
		get
		{
			return this.CustomizationState.Value.StickShaftTapeIDBlueGoalie;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x06000293 RID: 659 RVA: 0x00010BE7 File Offset: 0x0000EDE7
	[HideInInspector]
	public int StickShaftTapeIDRedGoalie
	{
		get
		{
			return this.CustomizationState.Value.StickShaftTapeIDRedGoalie;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000294 RID: 660 RVA: 0x00010BF9 File Offset: 0x0000EDF9
	[HideInInspector]
	public int StickBladeTapeIDBlueAttacker
	{
		get
		{
			return this.CustomizationState.Value.StickBladeTapeIDBlueAttacker;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000295 RID: 661 RVA: 0x00010C0B File Offset: 0x0000EE0B
	[HideInInspector]
	public int StickBladeTapeIDRedAttacker
	{
		get
		{
			return this.CustomizationState.Value.StickBladeTapeIDRedAttacker;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06000296 RID: 662 RVA: 0x00010C1D File Offset: 0x0000EE1D
	[HideInInspector]
	public int StickBladeTapeIDBlueGoalie
	{
		get
		{
			return this.CustomizationState.Value.StickBladeTapeIDBlueGoalie;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000297 RID: 663 RVA: 0x00010C2F File Offset: 0x0000EE2F
	[HideInInspector]
	public int StickBladeTapeIDRedGoalie
	{
		get
		{
			return this.CustomizationState.Value.StickBladeTapeIDRedGoalie;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06000298 RID: 664 RVA: 0x00010C41 File Offset: 0x0000EE41
	[HideInInspector]
	public bool IsCharacterSpawned
	{
		get
		{
			return this.PlayerCamera && this.PlayerBody && this.StickPositioner && this.Stick;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06000299 RID: 665 RVA: 0x00010C77 File Offset: 0x0000EE77
	[HideInInspector]
	public bool IsSpectatorCameraSpawned
	{
		get
		{
			return this.SpectatorCamera != null;
		}
	}

	// Token: 0x0600029A RID: 666 RVA: 0x00010C85 File Offset: 0x0000EE85
	private void Awake()
	{
		this.PlayerInput = base.GetComponent<PlayerInput>();
		this.ChatTickets = (float)this.maxChatTickets;
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00010CA0 File Offset: 0x0000EEA0
	protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
	{
		this.InitializeNetworkVariables(default(PlayerGameState), default(PlayerCustomizationState), PlayerHandedness.None, default(FixedString32Bytes), default(FixedString32Bytes), 0, 0, 0, 0, 0, 0UL, default(NetworkObjectReference), false, false);
		base.OnNetworkPreSpawn(ref networkManager);
	}

	// Token: 0x0600029C RID: 668 RVA: 0x00010CF4 File Offset: 0x0000EEF4
	public override void OnNetworkSpawn()
	{
		NetworkVariable<PlayerGameState> gameState = this.GameState;
		gameState.OnValueChanged = (NetworkVariable<PlayerGameState>.OnValueChangedDelegate)Delegate.Combine(gameState.OnValueChanged, new NetworkVariable<PlayerGameState>.OnValueChangedDelegate(this.OnPlayerGameStateChanged));
		NetworkVariable<PlayerCustomizationState> customizationState = this.CustomizationState;
		customizationState.OnValueChanged = (NetworkVariable<PlayerCustomizationState>.OnValueChangedDelegate)Delegate.Combine(customizationState.OnValueChanged, new NetworkVariable<PlayerCustomizationState>.OnValueChangedDelegate(this.OnPlayerCustomizationStateChanged));
		NetworkVariable<PlayerHandedness> handedness = this.Handedness;
		handedness.OnValueChanged = (NetworkVariable<PlayerHandedness>.OnValueChangedDelegate)Delegate.Combine(handedness.OnValueChanged, new NetworkVariable<PlayerHandedness>.OnValueChangedDelegate(this.OnPlayerHandednessChanged));
		NetworkVariable<FixedString32Bytes> steamId = this.SteamId;
		steamId.OnValueChanged = (NetworkVariable<FixedString32Bytes>.OnValueChangedDelegate)Delegate.Combine(steamId.OnValueChanged, new NetworkVariable<FixedString32Bytes>.OnValueChangedDelegate(this.OnPlayerSteamIdChanged));
		NetworkVariable<FixedString32Bytes> username = this.Username;
		username.OnValueChanged = (NetworkVariable<FixedString32Bytes>.OnValueChangedDelegate)Delegate.Combine(username.OnValueChanged, new NetworkVariable<FixedString32Bytes>.OnValueChangedDelegate(this.OnPlayerUsernameChanged));
		NetworkVariable<int> number = this.Number;
		number.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Combine(number.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(this.OnPlayerNumberChanged));
		NetworkVariable<int> patreonLevel = this.PatreonLevel;
		patreonLevel.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Combine(patreonLevel.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(this.OnPlayerPatreonLevelChanged));
		NetworkVariable<int> adminLevel = this.AdminLevel;
		adminLevel.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Combine(adminLevel.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(this.OnPlayerAdminLevelChanged));
		NetworkVariable<int> goals = this.Goals;
		goals.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Combine(goals.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(this.OnPlayerGoalsChanged));
		NetworkVariable<int> assists = this.Assists;
		assists.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Combine(assists.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(this.OnPlayerAssistsChanged));
		NetworkVariable<ulong> ping = this.Ping;
		ping.OnValueChanged = (NetworkVariable<ulong>.OnValueChangedDelegate)Delegate.Combine(ping.OnValueChanged, new NetworkVariable<ulong>.OnValueChangedDelegate(this.OnPlayerPingChanged));
		NetworkVariable<NetworkObjectReference> playerPositionReference = this.PlayerPositionReference;
		playerPositionReference.OnValueChanged = (NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate)Delegate.Combine(playerPositionReference.OnValueChanged, new NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate(this.OnPlayerPlayerPositionReferenceChanged));
		NetworkVariable<bool> isMuted = this.IsMuted;
		isMuted.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Combine(isMuted.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(this.OnPlayerIsMutedChanged));
		this.PlayerPosition = NetworkingUtils.GetPlayerPositionFromNetworkObjectReference(this.PlayerPositionReference.Value);
		base.OnNetworkSpawn();
	}

	// Token: 0x0600029D RID: 669 RVA: 0x00010F18 File Offset: 0x0000F118
	protected override void OnNetworkPostSpawn()
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerSpawned", new Dictionary<string, object>
		{
			{
				"player",
				this
			}
		});
		base.OnNetworkPostSpawn();
	}

	// Token: 0x0600029E RID: 670 RVA: 0x00010F3C File Offset: 0x0000F13C
	protected override void OnNetworkSessionSynchronized()
	{
		this.OnPlayerPlayerPositionReferenceChanged(default(NetworkObjectReference), this.PlayerPositionReference.Value);
		base.OnNetworkSessionSynchronized();
	}

	// Token: 0x0600029F RID: 671 RVA: 0x00010F6C File Offset: 0x0000F16C
	private void Update()
	{
		if (NetworkManager.Singleton.IsServer && this.ChatTickets < (float)this.maxChatTickets)
		{
			this.ChatTickets += this.ChatTicketsPerSecond * Time.deltaTime;
			if (this.ChatTickets > (float)this.maxChatTickets)
			{
				this.ChatTickets = (float)this.maxChatTickets;
			}
		}
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x00010FCC File Offset: 0x0000F1CC
	public override void OnNetworkDespawn()
	{
		if (NetworkManager.Singleton.IsServer)
		{
			this.Server_CancelDelayedGameState();
			if (this.IsCharacterSpawned)
			{
				this.Server_DespawnCharacter();
			}
			if (this.IsSpectatorCameraSpawned)
			{
				this.Server_DespawnSpectatorCamera();
			}
		}
		Player.Logger.Info(string.Format("Despawning Player ({0})", base.OwnerClientId));
		EventManager.TriggerEvent("Event_Everyone_OnPlayerDespawned", new Dictionary<string, object>
		{
			{
				"player",
				this
			}
		});
		NetworkVariable<PlayerGameState> gameState = this.GameState;
		gameState.OnValueChanged = (NetworkVariable<PlayerGameState>.OnValueChangedDelegate)Delegate.Remove(gameState.OnValueChanged, new NetworkVariable<PlayerGameState>.OnValueChangedDelegate(this.OnPlayerGameStateChanged));
		NetworkVariable<PlayerCustomizationState> customizationState = this.CustomizationState;
		customizationState.OnValueChanged = (NetworkVariable<PlayerCustomizationState>.OnValueChangedDelegate)Delegate.Remove(customizationState.OnValueChanged, new NetworkVariable<PlayerCustomizationState>.OnValueChangedDelegate(this.OnPlayerCustomizationStateChanged));
		NetworkVariable<PlayerHandedness> handedness = this.Handedness;
		handedness.OnValueChanged = (NetworkVariable<PlayerHandedness>.OnValueChangedDelegate)Delegate.Remove(handedness.OnValueChanged, new NetworkVariable<PlayerHandedness>.OnValueChangedDelegate(this.OnPlayerHandednessChanged));
		NetworkVariable<FixedString32Bytes> steamId = this.SteamId;
		steamId.OnValueChanged = (NetworkVariable<FixedString32Bytes>.OnValueChangedDelegate)Delegate.Remove(steamId.OnValueChanged, new NetworkVariable<FixedString32Bytes>.OnValueChangedDelegate(this.OnPlayerSteamIdChanged));
		NetworkVariable<FixedString32Bytes> username = this.Username;
		username.OnValueChanged = (NetworkVariable<FixedString32Bytes>.OnValueChangedDelegate)Delegate.Remove(username.OnValueChanged, new NetworkVariable<FixedString32Bytes>.OnValueChangedDelegate(this.OnPlayerUsernameChanged));
		NetworkVariable<int> number = this.Number;
		number.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Remove(number.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(this.OnPlayerNumberChanged));
		NetworkVariable<int> patreonLevel = this.PatreonLevel;
		patreonLevel.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Remove(patreonLevel.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(this.OnPlayerPatreonLevelChanged));
		NetworkVariable<int> adminLevel = this.AdminLevel;
		adminLevel.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Remove(adminLevel.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(this.OnPlayerAdminLevelChanged));
		NetworkVariable<int> goals = this.Goals;
		goals.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Remove(goals.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(this.OnPlayerGoalsChanged));
		NetworkVariable<int> assists = this.Assists;
		assists.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Remove(assists.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(this.OnPlayerAssistsChanged));
		NetworkVariable<ulong> ping = this.Ping;
		ping.OnValueChanged = (NetworkVariable<ulong>.OnValueChangedDelegate)Delegate.Remove(ping.OnValueChanged, new NetworkVariable<ulong>.OnValueChangedDelegate(this.OnPlayerPingChanged));
		NetworkVariable<NetworkObjectReference> playerPositionReference = this.PlayerPositionReference;
		playerPositionReference.OnValueChanged = (NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate)Delegate.Remove(playerPositionReference.OnValueChanged, new NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate(this.OnPlayerPlayerPositionReferenceChanged));
		NetworkVariable<bool> isMuted = this.IsMuted;
		isMuted.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Remove(isMuted.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(this.OnPlayerIsMutedChanged));
		base.OnNetworkDespawn();
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x00011244 File Offset: 0x0000F444
	public void InitializeNetworkVariables(PlayerGameState gameState = default(PlayerGameState), PlayerCustomizationState customizationState = default(PlayerCustomizationState), PlayerHandedness handedness = PlayerHandedness.None, FixedString32Bytes steamID = default(FixedString32Bytes), FixedString32Bytes username = default(FixedString32Bytes), int number = 0, int patreonLevel = 0, int adminLevel = 0, int goals = 0, int assists = 0, ulong ping = 0UL, NetworkObjectReference playerPositionReference = default(NetworkObjectReference), bool isMuted = false, bool isReplay = false)
	{
		if (this.isNetworkVariablesInitialized)
		{
			return;
		}
		this.isNetworkVariablesInitialized = true;
		this.GameState = new NetworkVariable<PlayerGameState>(gameState, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.CustomizationState = new NetworkVariable<PlayerCustomizationState>(customizationState, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.Handedness = new NetworkVariable<PlayerHandedness>(handedness, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.SteamId = new NetworkVariable<FixedString32Bytes>(steamID, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.Username = new NetworkVariable<FixedString32Bytes>(username, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.Number = new NetworkVariable<int>(number, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.PatreonLevel = new NetworkVariable<int>(patreonLevel, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.AdminLevel = new NetworkVariable<int>(adminLevel, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.Goals = new NetworkVariable<int>(goals, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.Assists = new NetworkVariable<int>(assists, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.Ping = new NetworkVariable<ulong>(ping, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.PlayerPositionReference = new NetworkVariable<NetworkObjectReference>(playerPositionReference, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.IsMuted = new NetworkVariable<bool>(isMuted, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
		this.IsReplay = new NetworkVariable<bool>(isReplay, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x00011330 File Offset: 0x0000F530
	public int GetPlayerJerseyID()
	{
		PlayerTeam team = this.Team;
		if (team != PlayerTeam.Blue)
		{
			if (team != PlayerTeam.Red)
			{
				return 0;
			}
			if (this.Role != PlayerRole.Attacker)
			{
				return this.JerseyIDRedGoalie;
			}
			return this.JerseyIDRedAttacker;
		}
		else
		{
			if (this.Role != PlayerRole.Attacker)
			{
				return this.JerseyIDBlueGoalie;
			}
			return this.JerseyIDBlueAttacker;
		}
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x00011380 File Offset: 0x0000F580
	public int GetPlayerHeadgearID()
	{
		PlayerTeam team = this.Team;
		if (team != PlayerTeam.Blue)
		{
			if (team != PlayerTeam.Red)
			{
				return 0;
			}
			if (this.Role != PlayerRole.Attacker)
			{
				return this.HeadgearIDRedGoalie;
			}
			return this.HeadgearIDRedAttacker;
		}
		else
		{
			if (this.Role != PlayerRole.Attacker)
			{
				return this.HeadgearIDBlueGoalie;
			}
			return this.HeadgearIDBlueAttacker;
		}
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x000113D0 File Offset: 0x0000F5D0
	public int GetPlayerStickSkinID()
	{
		PlayerTeam team = this.Team;
		if (team != PlayerTeam.Blue)
		{
			if (team != PlayerTeam.Red)
			{
				return 0;
			}
			if (this.Role != PlayerRole.Attacker)
			{
				return this.StickSkinIDRedGoalie;
			}
			return this.StickSkinIDRedAttacker;
		}
		else
		{
			if (this.Role != PlayerRole.Attacker)
			{
				return this.StickSkinIDBlueGoalie;
			}
			return this.StickSkinIDBlueAttacker;
		}
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x00011420 File Offset: 0x0000F620
	public int GetPlayerStickShaftTapeID()
	{
		PlayerTeam team = this.Team;
		if (team != PlayerTeam.Blue)
		{
			if (team != PlayerTeam.Red)
			{
				return 0;
			}
			if (this.Role != PlayerRole.Attacker)
			{
				return this.StickShaftTapeIDRedGoalie;
			}
			return this.StickShaftTapeIDRedAttacker;
		}
		else
		{
			if (this.Role != PlayerRole.Attacker)
			{
				return this.StickShaftTapeIDBlueGoalie;
			}
			return this.StickShaftTapeIDBlueAttacker;
		}
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00011470 File Offset: 0x0000F670
	public int GetPlayerStickBladeTapeID()
	{
		PlayerTeam team = this.Team;
		if (team != PlayerTeam.Blue)
		{
			if (team != PlayerTeam.Red)
			{
				return 0;
			}
			if (this.Role != PlayerRole.Attacker)
			{
				return this.StickBladeTapeIDRedGoalie;
			}
			return this.StickBladeTapeIDRedAttacker;
		}
		else
		{
			if (this.Role != PlayerRole.Attacker)
			{
				return this.StickBladeTapeIDBlueGoalie;
			}
			return this.StickBladeTapeIDBlueAttacker;
		}
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x000114BD File Offset: 0x0000F6BD
	private void OnPlayerGameStateChanged(PlayerGameState oldGameState, PlayerGameState newGameState)
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerGameStateChanged", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
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

	// Token: 0x060002A8 RID: 680 RVA: 0x000114FC File Offset: 0x0000F6FC
	private void OnPlayerCustomizationStateChanged(PlayerCustomizationState oldCustomizationState, PlayerCustomizationState newCustomizationState)
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerCustomizationStateChanged", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"oldCustomizationState",
				oldCustomizationState
			},
			{
				"newCustomizationState",
				newCustomizationState
			}
		});
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x0001153B File Offset: 0x0000F73B
	private void OnPlayerHandednessChanged(PlayerHandedness oldHandedness, PlayerHandedness newHandedness)
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerHandednessChanged", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"oldHandedness",
				oldHandedness
			},
			{
				"newHandedness",
				newHandedness
			}
		});
	}

	// Token: 0x060002AA RID: 682 RVA: 0x0001157A File Offset: 0x0000F77A
	private void OnPlayerSteamIdChanged(FixedString32Bytes oldSteamId, FixedString32Bytes newSteamId)
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerSteamIdChanged", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"oldSteamId",
				oldSteamId
			},
			{
				"newSteamId",
				newSteamId
			}
		});
	}

	// Token: 0x060002AB RID: 683 RVA: 0x000115B9 File Offset: 0x0000F7B9
	private void OnPlayerUsernameChanged(FixedString32Bytes oldUsername, FixedString32Bytes newUsername)
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerUsernameChanged", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"oldUsername",
				oldUsername
			},
			{
				"newUsername",
				newUsername
			}
		});
	}

	// Token: 0x060002AC RID: 684 RVA: 0x000115F8 File Offset: 0x0000F7F8
	private void OnPlayerNumberChanged(int oldNumber, int newNumber)
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerNumberChanged", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"oldNumber",
				oldNumber
			},
			{
				"newNumber",
				newNumber
			}
		});
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00011637 File Offset: 0x0000F837
	private void OnPlayerPatreonLevelChanged(int oldPatreonLevel, int newPatreonLevel)
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerPatreonLevelChanged", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"oldPatreonLevel",
				oldPatreonLevel
			},
			{
				"newPatreonLevel",
				newPatreonLevel
			}
		});
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00011676 File Offset: 0x0000F876
	private void OnPlayerAdminLevelChanged(int oldAdminLevel, int newAdminLevel)
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerAdminLevelChanged", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"oldAdminLevel",
				oldAdminLevel
			},
			{
				"newAdminLevel",
				newAdminLevel
			}
		});
	}

	// Token: 0x060002AF RID: 687 RVA: 0x000116B5 File Offset: 0x0000F8B5
	private void OnPlayerGoalsChanged(int oldGoals, int newGoals)
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerGoalsChanged", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"oldGoals",
				oldGoals
			},
			{
				"newGoals",
				newGoals
			}
		});
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x000116F4 File Offset: 0x0000F8F4
	private void OnPlayerAssistsChanged(int oldAssists, int newAssists)
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerAssistsChanged", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"oldAssists",
				oldAssists
			},
			{
				"newAssists",
				newAssists
			}
		});
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x00011734 File Offset: 0x0000F934
	private void OnPlayerPingChanged(ulong oldPing, ulong newPing)
	{
		if (!EventManager.HasEventListeners("Event_Everyone_OnPlayerPingChanged"))
		{
			return;
		}
		EventManager.TriggerEvent("Event_Everyone_OnPlayerPingChanged", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"oldPing",
				oldPing
			},
			{
				"newPing",
				newPing
			}
		});
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x0001178C File Offset: 0x0000F98C
	private void OnPlayerPlayerPositionReferenceChanged(NetworkObjectReference oldPlayerPositionReference, NetworkObjectReference newPlayerPositionReference)
	{
		PlayerPosition playerPositionFromNetworkObjectReference = NetworkingUtils.GetPlayerPositionFromNetworkObjectReference(oldPlayerPositionReference);
		PlayerPosition playerPositionFromNetworkObjectReference2 = NetworkingUtils.GetPlayerPositionFromNetworkObjectReference(newPlayerPositionReference);
		this.PlayerPosition = playerPositionFromNetworkObjectReference2;
		this.OnPlayerPositionChanged(playerPositionFromNetworkObjectReference, this.PlayerPosition);
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x000117BB File Offset: 0x0000F9BB
	private void OnPlayerPositionChanged(PlayerPosition oldPlayerPosition, PlayerPosition newPlayerPosition)
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerPositionChanged", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"oldPlayerPosition",
				oldPlayerPosition
			},
			{
				"newPlayerPosition",
				newPlayerPosition
			}
		});
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x000117F0 File Offset: 0x0000F9F0
	private void OnPlayerIsMutedChanged(bool oldIsMuted, bool newIsMuted)
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerIsMutedChanged", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"oldIsMuted",
				oldIsMuted
			},
			{
				"newIsMuted",
				newIsMuted
			}
		});
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00011830 File Offset: 0x0000FA30
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_RequestTeamRpc(PlayerTeam team, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 2620210071U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<PlayerTeam>(team, default(FastBufferWriter.ForEnums));
			base.__endSendRpc(ref fastBufferWriter, 2620210071U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
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
		EventManager.TriggerEvent("Event_Server_OnPlayerRequestTeam", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"team",
				team
			}
		});
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00011958 File Offset: 0x0000FB58
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_RequestClaimPositionRpc(NetworkObjectReference playerPositionReference, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 949682089U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<NetworkObjectReference>(playerPositionReference, default(FastBufferWriter.ForNetworkSerializable));
			base.__endSendRpc(ref fastBufferWriter, 949682089U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
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
		PlayerPosition playerPositionFromNetworkObjectReference = NetworkingUtils.GetPlayerPositionFromNetworkObjectReference(playerPositionReference);
		EventManager.TriggerEvent("Event_Server_OnPlayerRequestPosition", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"playerPosition",
				playerPositionFromNetworkObjectReference
			}
		});
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00011A80 File Offset: 0x0000FC80
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_RequestTeamSelectRpc(RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 4280154797U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			base.__endSendRpc(ref fastBufferWriter, 4280154797U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
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
		EventManager.TriggerEvent("Event_Server_OnPlayerRequestTeamSelect", new Dictionary<string, object>
		{
			{
				"player",
				this
			}
		});
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x00011B7C File Offset: 0x0000FD7C
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_RequestPositionSelectRpc(RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 3454979199U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			base.__endSendRpc(ref fastBufferWriter, 3454979199U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
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
		EventManager.TriggerEvent("Event_Server_OnPlayerRequestPositionSelect", new Dictionary<string, object>
		{
			{
				"player",
				this
			}
		});
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00011C78 File Offset: 0x0000FE78
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_RequestForfeitRpc(RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 3378576624U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			base.__endSendRpc(ref fastBufferWriter, 3378576624U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
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
		EventManager.TriggerEvent("Event_Server_OnPlayerRequestForfeit", new Dictionary<string, object>
		{
			{
				"player",
				this
			}
		});
	}

	// Token: 0x060002BA RID: 698 RVA: 0x00011D74 File Offset: 0x0000FF74
	[Rpc(SendTo.Server, DeferLocal = true)]
	public void Client_RequestHandednessRpc(PlayerHandedness handedness, RpcParams rpcParams = default(RpcParams))
	{
		NetworkManager networkManager = base.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
			return;
		}
		if (this.__rpc_exec_stage != NetworkBehaviour.__RpcExecStage.Execute)
		{
			uint rpcMethodId = 744616166U;
			RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
			{
				DeferLocal = true
			};
			FastBufferWriter fastBufferWriter = base.__beginSendRpc(rpcMethodId, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			fastBufferWriter.WriteValueSafe<PlayerHandedness>(handedness, default(FastBufferWriter.ForEnums));
			base.__endSendRpc(ref fastBufferWriter, 744616166U, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
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
		EventManager.TriggerEvent("Event_Server_OnPlayerRequestHandedness", new Dictionary<string, object>
		{
			{
				"player",
				this
			},
			{
				"handedness",
				handedness
			}
		});
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00011E9C File Offset: 0x0001009C
	public void Server_SetGameState(PlayerPhase? phase = null, PlayerTeam? team = null, PlayerRole? role = null, float? delay = null)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.Server_CancelDelayedGameState();
		if (delay == null)
		{
			PlayerGameState value = new PlayerGameState
			{
				Phase = (phase ?? this.Phase),
				Team = (team ?? this.Team),
				Role = (role ?? this.Role)
			};
			this.GameState.Value = value;
			return;
		}
		this.delayedGameStateTween = DOVirtual.DelayedCall(delay.Value, delegate
		{
			this.Server_SetGameState(phase, team, role, null);
		}, true);
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00011F94 File Offset: 0x00010194
	public void Server_CancelDelayedGameState()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Tween tween = this.delayedGameStateTween;
		if (tween == null)
		{
			return;
		}
		tween.Kill(false);
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00011FB4 File Offset: 0x000101B4
	public void Server_SpawnCharacter(Vector3 position, Quaternion rotation, PlayerRole role)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player.Logger.Info(string.Format("Spawning character ({0})", base.OwnerClientId));
		if (this.IsCharacterSpawned)
		{
			this.Server_DespawnCharacter();
		}
		if (this.IsSpectatorCameraSpawned)
		{
			this.Server_DespawnSpectatorCamera();
		}
		this.Server_SpawnPlayerBody(position, rotation, role);
		this.Server_SpawnStick(this.StickPositioner.RaycastOriginPosition, Quaternion.LookRotation(this.PlayerBody.transform.right, Vector3.up), role);
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00012040 File Offset: 0x00010240
	public void Server_SpawnPlayerCamera(PlayerBody playerBody)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player.Logger.Info(string.Format("Spawning PlayerCamera ({0})", base.OwnerClientId));
		PlayerCamera playerCamera = Object.Instantiate<PlayerCamera>(this.playerCameraPrefab);
		playerCamera.InitializeNetworkVariables(new NetworkObjectReference(base.NetworkObject));
		playerCamera.NetworkObject.SpawnWithOwnership(base.OwnerClientId, false);
		if (!playerCamera.NetworkObject.TrySetParent(playerBody.transform, false))
		{
			Player.Logger.Error(string.Format("Failed to set parent for PlayerCamera ({0})", base.OwnerClientId));
		}
	}

	// Token: 0x060002BF RID: 703 RVA: 0x000120DC File Offset: 0x000102DC
	public void Server_SpawnPlayerBody(Vector3 position, Quaternion rotation, PlayerRole role)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player.Logger.Info(string.Format("Spawning PlayerBody ({0})", base.OwnerClientId));
		PlayerBody playerBody = null;
		if (role != PlayerRole.Attacker)
		{
			if (role == PlayerRole.Goalie)
			{
				playerBody = this.playerBodyGoaliePrefab;
			}
		}
		else
		{
			playerBody = this.playerBodyAttackerPrefab;
		}
		if (playerBody == null)
		{
			Player.Logger.Error(string.Format("Failed to spawn PlayerBody ({0}): Missing prefab for role {1}", base.OwnerClientId, role));
			return;
		}
		PlayerBody playerBody2 = Object.Instantiate<PlayerBody>(playerBody, position, rotation);
		playerBody2.InitializeNetworkVariables(new NetworkObjectReference(base.NetworkObject), 1f, 0f, false, false, false, false, false, false);
		playerBody2.NetworkObject.SpawnWithOwnership(base.OwnerClientId, false);
		if (playerBody2.NetworkObject.TrySetParent(base.gameObject.transform, true))
		{
			this.Server_SpawnPlayerCamera(playerBody2);
			this.Server_SpawnStickPositioner(playerBody2);
			return;
		}
		Player.Logger.Error(string.Format("Failed to set parent for PlayerBody ({0})", base.OwnerClientId));
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x000121E4 File Offset: 0x000103E4
	public void Server_SpawnStickPositioner(PlayerBody playerBody)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player.Logger.Info(string.Format("Spawning StickPositioner ({0})", base.OwnerClientId));
		StickPositioner stickPositioner = Object.Instantiate<StickPositioner>(this.stickPositionerPrefab);
		stickPositioner.InitializeNetworkVariables(new NetworkObjectReference(base.NetworkObject));
		stickPositioner.NetworkObject.SpawnWithOwnership(base.OwnerClientId, false);
		if (!stickPositioner.NetworkObject.TrySetParent(playerBody.transform, false))
		{
			Player.Logger.Error(string.Format("Failed to set parent for StickPositioner ({0})", base.OwnerClientId));
		}
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00012280 File Offset: 0x00010480
	public void Server_SpawnStick(Vector3 position, Quaternion rotation, PlayerRole role)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player.Logger.Info(string.Format("Spawning Stick ({0})", base.OwnerClientId));
		Stick stick = null;
		if (role != PlayerRole.Attacker)
		{
			if (role == PlayerRole.Goalie)
			{
				stick = this.stickGoaliePrefab;
			}
		}
		else
		{
			stick = this.stickAttackerPrefab;
		}
		if (stick == null)
		{
			Player.Logger.Error(string.Format("Failed to spawn Stick ({0}): Missing prefab for role {1}", base.OwnerClientId, role));
			return;
		}
		Stick stick2 = Object.Instantiate<Stick>(stick, position, rotation);
		stick2.InitializeNetworkVariables(new NetworkObjectReference(base.NetworkObject));
		stick2.NetworkObject.SpawnWithOwnership(base.OwnerClientId, false);
		if (!stick2.NetworkObject.TrySetParent(base.gameObject.transform, true))
		{
			Player.Logger.Error(string.Format("Failed to set parent for Stick ({0})", base.OwnerClientId));
		}
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x00012366 File Offset: 0x00010566
	public void Server_DespawnCharacter()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player.Logger.Info(string.Format("Despawning character ({0})", base.OwnerClientId));
		this.Server_DespawnPlayerBody();
		this.Server_DespawnStick();
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x000123A0 File Offset: 0x000105A0
	public void Server_DespawnPlayerCamera()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player.Logger.Info(string.Format("Despawning PlayerCamera ({0})", base.OwnerClientId));
		if (this.PlayerCamera && this.PlayerCamera.NetworkObject.IsSpawned)
		{
			this.PlayerCamera.NetworkObject.Despawn(true);
		}
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x0001240C File Offset: 0x0001060C
	public void Server_DespawnPlayerBody()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player.Logger.Info(string.Format("Despawning PlayerBody ({0})", base.OwnerClientId));
		this.Server_DespawnPlayerCamera();
		this.Server_DespawnStickPositioner();
		if (this.PlayerBody && this.PlayerBody.NetworkObject.IsSpawned)
		{
			this.PlayerBody.NetworkObject.Despawn(true);
		}
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x00012484 File Offset: 0x00010684
	public void Server_DespawnStickPositioner()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player.Logger.Info(string.Format("Despawning StickPositioner ({0})", base.OwnerClientId));
		if (this.StickPositioner && this.StickPositioner.NetworkObject.IsSpawned)
		{
			this.StickPositioner.NetworkObject.Despawn(true);
		}
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x000124F0 File Offset: 0x000106F0
	public void Server_DespawnStick()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player.Logger.Info(string.Format("Despawning Stick ({0})", base.OwnerClientId));
		if (this.Stick && this.Stick.NetworkObject.IsSpawned)
		{
			this.Stick.NetworkObject.Despawn(true);
		}
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x0001255C File Offset: 0x0001075C
	public void Server_SpawnSpectatorCamera(Vector3 position, Quaternion rotation)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player.Logger.Info(string.Format("Spawning SpectatorCamera ({0})", base.OwnerClientId));
		if (this.IsCharacterSpawned)
		{
			this.Server_DespawnCharacter();
		}
		if (this.IsSpectatorCameraSpawned)
		{
			this.Server_DespawnSpectatorCamera();
		}
		SpectatorCamera spectatorCamera = Object.Instantiate<SpectatorCamera>(this.spectatorCameraPrefab, position, rotation);
		spectatorCamera.InitializeNetworkVariables(new NetworkObjectReference(base.NetworkObject));
		spectatorCamera.PlayerReference.Value = new NetworkObjectReference(base.NetworkObject);
		spectatorCamera.NetworkObject.SpawnWithOwnership(base.OwnerClientId, false);
		if (!spectatorCamera.NetworkObject.TrySetParent(base.gameObject.transform, true))
		{
			Player.Logger.Error(string.Format("Failed to set parent for SpectatorCamera ({0})", base.OwnerClientId));
		}
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x00012630 File Offset: 0x00010830
	public void Server_DespawnSpectatorCamera()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		Player.Logger.Info(string.Format("Despawning SpectatorCamera ({0})", base.OwnerClientId));
		if (this.SpectatorCamera && this.SpectatorCamera.NetworkObject.IsSpawned)
		{
			this.SpectatorCamera.NetworkObject.Despawn(true);
		}
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x0001269C File Offset: 0x0001089C
	public void Server_GoalScored()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		NetworkVariable<int> goals = this.Goals;
		int value = goals.Value;
		goals.Value = value + 1;
	}

	// Token: 0x060002CA RID: 714 RVA: 0x000126CC File Offset: 0x000108CC
	public void Server_AssistScored()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		NetworkVariable<int> assists = this.Assists;
		int value = assists.Value;
		assists.Value = value + 1;
	}

	// Token: 0x060002CB RID: 715 RVA: 0x000126FB File Offset: 0x000108FB
	public void Server_ResetPoints()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.Goals.Value = 0;
		this.Assists.Value = 0;
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00012722 File Offset: 0x00010922
	public void Server_UpdatePing()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.Ping.Value = NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetCurrentRtt(base.OwnerClientId);
	}

	// Token: 0x060002CD RID: 717 RVA: 0x00012756 File Offset: 0x00010956
	public void Server_ConsumeChatTicket()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.ChatTickets = Mathf.Max(0f, this.ChatTickets - 1f);
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x000127AC File Offset: 0x000109AC
	protected override void __initializeVariables()
	{
		bool flag = this.GameState == null;
		if (flag)
		{
			throw new Exception("Player.GameState cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.GameState.Initialize(this);
		base.__nameNetworkVariable(this.GameState, "GameState");
		this.NetworkVariableFields.Add(this.GameState);
		flag = (this.CustomizationState == null);
		if (flag)
		{
			throw new Exception("Player.CustomizationState cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.CustomizationState.Initialize(this);
		base.__nameNetworkVariable(this.CustomizationState, "CustomizationState");
		this.NetworkVariableFields.Add(this.CustomizationState);
		flag = (this.Handedness == null);
		if (flag)
		{
			throw new Exception("Player.Handedness cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.Handedness.Initialize(this);
		base.__nameNetworkVariable(this.Handedness, "Handedness");
		this.NetworkVariableFields.Add(this.Handedness);
		flag = (this.SteamId == null);
		if (flag)
		{
			throw new Exception("Player.SteamId cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.SteamId.Initialize(this);
		base.__nameNetworkVariable(this.SteamId, "SteamId");
		this.NetworkVariableFields.Add(this.SteamId);
		flag = (this.Username == null);
		if (flag)
		{
			throw new Exception("Player.Username cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.Username.Initialize(this);
		base.__nameNetworkVariable(this.Username, "Username");
		this.NetworkVariableFields.Add(this.Username);
		flag = (this.Number == null);
		if (flag)
		{
			throw new Exception("Player.Number cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.Number.Initialize(this);
		base.__nameNetworkVariable(this.Number, "Number");
		this.NetworkVariableFields.Add(this.Number);
		flag = (this.PatreonLevel == null);
		if (flag)
		{
			throw new Exception("Player.PatreonLevel cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.PatreonLevel.Initialize(this);
		base.__nameNetworkVariable(this.PatreonLevel, "PatreonLevel");
		this.NetworkVariableFields.Add(this.PatreonLevel);
		flag = (this.AdminLevel == null);
		if (flag)
		{
			throw new Exception("Player.AdminLevel cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.AdminLevel.Initialize(this);
		base.__nameNetworkVariable(this.AdminLevel, "AdminLevel");
		this.NetworkVariableFields.Add(this.AdminLevel);
		flag = (this.Goals == null);
		if (flag)
		{
			throw new Exception("Player.Goals cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.Goals.Initialize(this);
		base.__nameNetworkVariable(this.Goals, "Goals");
		this.NetworkVariableFields.Add(this.Goals);
		flag = (this.Assists == null);
		if (flag)
		{
			throw new Exception("Player.Assists cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.Assists.Initialize(this);
		base.__nameNetworkVariable(this.Assists, "Assists");
		this.NetworkVariableFields.Add(this.Assists);
		flag = (this.Ping == null);
		if (flag)
		{
			throw new Exception("Player.Ping cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.Ping.Initialize(this);
		base.__nameNetworkVariable(this.Ping, "Ping");
		this.NetworkVariableFields.Add(this.Ping);
		flag = (this.PlayerPositionReference == null);
		if (flag)
		{
			throw new Exception("Player.PlayerPositionReference cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.PlayerPositionReference.Initialize(this);
		base.__nameNetworkVariable(this.PlayerPositionReference, "PlayerPositionReference");
		this.NetworkVariableFields.Add(this.PlayerPositionReference);
		flag = (this.IsMuted == null);
		if (flag)
		{
			throw new Exception("Player.IsMuted cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.IsMuted.Initialize(this);
		base.__nameNetworkVariable(this.IsMuted, "IsMuted");
		this.NetworkVariableFields.Add(this.IsMuted);
		flag = (this.IsReplay == null);
		if (flag)
		{
			throw new Exception("Player.IsReplay cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.IsReplay.Initialize(this);
		base.__nameNetworkVariable(this.IsReplay, "IsReplay");
		this.NetworkVariableFields.Add(this.IsReplay);
		base.__initializeVariables();
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x00012BF8 File Offset: 0x00010DF8
	protected override void __initializeRpcs()
	{
		base.__registerRpc(2620210071U, new NetworkBehaviour.RpcReceiveHandler(Player.__rpc_handler_2620210071), "Client_RequestTeamRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(949682089U, new NetworkBehaviour.RpcReceiveHandler(Player.__rpc_handler_949682089), "Client_RequestClaimPositionRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(4280154797U, new NetworkBehaviour.RpcReceiveHandler(Player.__rpc_handler_4280154797), "Client_RequestTeamSelectRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(3454979199U, new NetworkBehaviour.RpcReceiveHandler(Player.__rpc_handler_3454979199), "Client_RequestPositionSelectRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(3378576624U, new NetworkBehaviour.RpcReceiveHandler(Player.__rpc_handler_3378576624), "Client_RequestForfeitRpc", RpcInvokePermission.Everyone);
		base.__registerRpc(744616166U, new NetworkBehaviour.RpcReceiveHandler(Player.__rpc_handler_744616166), "Client_RequestHandednessRpc", RpcInvokePermission.Everyone);
		base.__initializeRpcs();
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x00012CD4 File Offset: 0x00010ED4
	private static void __rpc_handler_2620210071(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		PlayerTeam team;
		reader.ReadValueSafe<PlayerTeam>(out team, default(FastBufferWriter.ForEnums));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((Player)target).Client_RequestTeamRpc(team, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00012D54 File Offset: 0x00010F54
	private static void __rpc_handler_949682089(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		NetworkObjectReference playerPositionReference;
		reader.ReadValueSafe<NetworkObjectReference>(out playerPositionReference, default(FastBufferWriter.ForNetworkSerializable));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((Player)target).Client_RequestClaimPositionRpc(playerPositionReference, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00012DD4 File Offset: 0x00010FD4
	private static void __rpc_handler_4280154797(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((Player)target).Client_RequestTeamSelectRpc(ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00012E34 File Offset: 0x00011034
	private static void __rpc_handler_3454979199(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((Player)target).Client_RequestPositionSelectRpc(ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x00012E94 File Offset: 0x00011094
	private static void __rpc_handler_3378576624(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((Player)target).Client_RequestForfeitRpc(ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00012EF4 File Offset: 0x000110F4
	private static void __rpc_handler_744616166(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
	{
		NetworkManager networkManager = target.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}
		PlayerHandedness handedness;
		reader.ReadValueSafe<PlayerHandedness>(out handedness, default(FastBufferWriter.ForEnums));
		RpcParams ext = rpcParams.Ext;
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Execute;
		((Player)target).Client_RequestHandednessRpc(handedness, ext);
		target.__rpc_exec_stage = NetworkBehaviour.__RpcExecStage.Send;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x00012F72 File Offset: 0x00011172
	protected internal override string __getTypeName()
	{
		return "Player";
	}

	// Token: 0x040001F8 RID: 504
	private static readonly global::Logger Logger = new global::Logger("Player");

	// Token: 0x040001F9 RID: 505
	[Header("Settings")]
	[SerializeField]
	private int maxChatTickets = 3;

	// Token: 0x040001FA RID: 506
	[SerializeField]
	public float ChatTicketsPerSecond = 3f;

	// Token: 0x040001FB RID: 507
	[Header("Prefabs")]
	[SerializeField]
	private PlayerCamera playerCameraPrefab;

	// Token: 0x040001FC RID: 508
	[SerializeField]
	private PlayerBody playerBodyAttackerPrefab;

	// Token: 0x040001FD RID: 509
	[SerializeField]
	private PlayerBody playerBodyGoaliePrefab;

	// Token: 0x040001FE RID: 510
	[SerializeField]
	private StickPositioner stickPositionerPrefab;

	// Token: 0x040001FF RID: 511
	[SerializeField]
	private SpectatorCamera spectatorCameraPrefab;

	// Token: 0x04000200 RID: 512
	[SerializeField]
	private Stick stickAttackerPrefab;

	// Token: 0x04000201 RID: 513
	[SerializeField]
	private Stick stickGoaliePrefab;

	// Token: 0x04000202 RID: 514
	[HideInInspector]
	public NetworkVariable<PlayerGameState> GameState;

	// Token: 0x04000203 RID: 515
	[HideInInspector]
	public NetworkVariable<PlayerCustomizationState> CustomizationState;

	// Token: 0x04000204 RID: 516
	[HideInInspector]
	public NetworkVariable<PlayerHandedness> Handedness;

	// Token: 0x04000205 RID: 517
	[HideInInspector]
	public NetworkVariable<FixedString32Bytes> SteamId;

	// Token: 0x04000206 RID: 518
	[HideInInspector]
	public NetworkVariable<FixedString32Bytes> Username;

	// Token: 0x04000207 RID: 519
	[HideInInspector]
	public NetworkVariable<int> Number;

	// Token: 0x04000208 RID: 520
	[HideInInspector]
	public NetworkVariable<int> PatreonLevel;

	// Token: 0x04000209 RID: 521
	[HideInInspector]
	public NetworkVariable<int> AdminLevel;

	// Token: 0x0400020A RID: 522
	[HideInInspector]
	public NetworkVariable<int> Goals;

	// Token: 0x0400020B RID: 523
	[HideInInspector]
	public NetworkVariable<int> Assists;

	// Token: 0x0400020C RID: 524
	[HideInInspector]
	public NetworkVariable<ulong> Ping;

	// Token: 0x0400020D RID: 525
	[HideInInspector]
	public NetworkVariable<NetworkObjectReference> PlayerPositionReference;

	// Token: 0x0400020E RID: 526
	[HideInInspector]
	public NetworkVariable<bool> IsMuted;

	// Token: 0x0400020F RID: 527
	[HideInInspector]
	public NetworkVariable<bool> IsReplay;

	// Token: 0x04000211 RID: 529
	[HideInInspector]
	public PlayerInput PlayerInput;

	// Token: 0x04000212 RID: 530
	[HideInInspector]
	public SpectatorCamera SpectatorCamera;

	// Token: 0x04000213 RID: 531
	[HideInInspector]
	public PlayerCamera PlayerCamera;

	// Token: 0x04000214 RID: 532
	[HideInInspector]
	public PlayerBody PlayerBody;

	// Token: 0x04000215 RID: 533
	[HideInInspector]
	public StickPositioner StickPositioner;

	// Token: 0x04000216 RID: 534
	[HideInInspector]
	public Stick Stick;

	// Token: 0x04000217 RID: 535
	[HideInInspector]
	public PlayerPosition PlayerPosition;

	// Token: 0x04000218 RID: 536
	private bool isNetworkVariablesInitialized;

	// Token: 0x04000219 RID: 537
	private Tween delayedGameStateTween;
}
