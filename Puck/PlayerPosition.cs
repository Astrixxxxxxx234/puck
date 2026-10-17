using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class PlayerPosition : NetworkBehaviour
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x060000B6 RID: 182 RVA: 0x000041D6 File Offset: 0x000023D6
	[HideInInspector]
	public bool IsClaimed
	{
		get
		{
			return this.ClaimedByPlayer != null;
		}
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x000041E4 File Offset: 0x000023E4
	protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
	{
		this.InitializeNetworkVariables(default(NetworkObjectReference));
		base.OnNetworkPreSpawn(ref networkManager);
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00004207 File Offset: 0x00002407
	public override void OnNetworkSpawn()
	{
		NetworkVariable<NetworkObjectReference> claimedByPlayerReference = this.ClaimedByPlayerReference;
		claimedByPlayerReference.OnValueChanged = (NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate)Delegate.Combine(claimedByPlayerReference.OnValueChanged, new NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate(this.OnClaimedByReferenceChanged));
		base.OnNetworkSpawn();
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00004238 File Offset: 0x00002438
	protected override void OnNetworkPostSpawn()
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerPositionSpawned", new Dictionary<string, object>
		{
			{
				"playerPosition",
				this
			}
		});
		if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsConnectedClient)
		{
			this.ProcessInitialNetworkVariableValues();
		}
		base.OnNetworkPostSpawn();
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00004284 File Offset: 0x00002484
	protected override void OnNetworkSessionSynchronized()
	{
		this.ProcessInitialNetworkVariableValues();
		base.OnNetworkSessionSynchronized();
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00004294 File Offset: 0x00002494
	public override void OnNetworkDespawn()
	{
		EventManager.TriggerEvent("Event_Everyone_OnPlayerPositionDespawned", new Dictionary<string, object>
		{
			{
				"playerPosition",
				this
			}
		});
		NetworkVariable<NetworkObjectReference> claimedByPlayerReference = this.ClaimedByPlayerReference;
		claimedByPlayerReference.OnValueChanged = (NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate)Delegate.Remove(claimedByPlayerReference.OnValueChanged, new NetworkVariable<NetworkObjectReference>.OnValueChangedDelegate(this.OnClaimedByReferenceChanged));
		base.OnNetworkDespawn();
	}

	// Token: 0x060000BC RID: 188 RVA: 0x000042E9 File Offset: 0x000024E9
	public void InitializeNetworkVariables(NetworkObjectReference claimedByPlayerReference = default(NetworkObjectReference))
	{
		if (this.isNetworkVariablesInitialized)
		{
			return;
		}
		this.isNetworkVariablesInitialized = true;
		this.ClaimedByPlayerReference = new NetworkVariable<NetworkObjectReference>(claimedByPlayerReference, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
	}

	// Token: 0x060000BD RID: 189 RVA: 0x0000430C File Offset: 0x0000250C
	private void ProcessInitialNetworkVariableValues()
	{
		this.OnClaimedByReferenceChanged(default(NetworkObjectReference), this.ClaimedByPlayerReference.Value);
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00004334 File Offset: 0x00002534
	private void OnClaimedByReferenceChanged(NetworkObjectReference oldClaimedByReferece, NetworkObjectReference newClaimedByReferece)
	{
		Player playerFromNetworkObjectReference = NetworkingUtils.GetPlayerFromNetworkObjectReference(oldClaimedByReferece);
		Player playerFromNetworkObjectReference2 = NetworkingUtils.GetPlayerFromNetworkObjectReference(newClaimedByReferece);
		this.ClaimedByPlayer = playerFromNetworkObjectReference2;
		EventManager.TriggerEvent("Event_Everyone_OnPlayerPositionClaimedByPlayerChanged", new Dictionary<string, object>
		{
			{
				"playerPosition",
				this
			},
			{
				"oldClaimedByPlayer",
				playerFromNetworkObjectReference
			},
			{
				"newClaimedByPlayer",
				playerFromNetworkObjectReference2
			}
		});
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00004389 File Offset: 0x00002589
	public void Server_Claim(Player player)
	{
		PlayerPosition.Logger.Info(string.Format("Position {0} claimed by {1}", this.Name, player.OwnerClientId));
		this.ClaimedByPlayerReference.Value = new NetworkObjectReference(player.NetworkObject);
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x000043C8 File Offset: 0x000025C8
	public void Server_Unclaim()
	{
		PlayerPosition.Logger.Info("Position " + this.Name + " unclaimed");
		this.ClaimedByPlayerReference.Value = default(NetworkObjectReference);
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x0000441C File Offset: 0x0000261C
	protected override void __initializeVariables()
	{
		bool flag = this.ClaimedByPlayerReference == null;
		if (flag)
		{
			throw new Exception("PlayerPosition.ClaimedByPlayerReference cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.ClaimedByPlayerReference.Initialize(this);
		base.__nameNetworkVariable(this.ClaimedByPlayerReference, "ClaimedByPlayerReference");
		this.NetworkVariableFields.Add(this.ClaimedByPlayerReference);
		base.__initializeVariables();
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x0000447F File Offset: 0x0000267F
	protected internal override string __getTypeName()
	{
		return "PlayerPosition";
	}

	// Token: 0x0400005B RID: 91
	private static readonly global::Logger Logger = new global::Logger("PlayerPosition");

	// Token: 0x0400005C RID: 92
	[Header("Settings")]
	public string Name;

	// Token: 0x0400005D RID: 93
	public PlayerTeam Team;

	// Token: 0x0400005E RID: 94
	public PlayerRole Role;

	// Token: 0x0400005F RID: 95
	[HideInInspector]
	public NetworkVariable<NetworkObjectReference> ClaimedByPlayerReference;

	// Token: 0x04000060 RID: 96
	[HideInInspector]
	public Player ClaimedByPlayer;

	// Token: 0x04000061 RID: 97
	private bool isNetworkVariablesInitialized;
}
