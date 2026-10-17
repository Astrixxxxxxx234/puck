using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class NetworkObjectCollisionRecorder : NetworkBehaviour
{
	// Token: 0x17000004 RID: 4
	// (get) Token: 0x060000A8 RID: 168 RVA: 0x00003F2C File Offset: 0x0000212C
	[HideInInspector]
	public List<NetworkObjectCollision> NetworkObjectCollisions
	{
		get
		{
			if (this.cachedNetworkObjectCollisions == null)
			{
				this.cachedNetworkObjectCollisions = this.Buffer.AsNativeArray().ToList<NetworkObjectCollision>();
			}
			return this.cachedNetworkObjectCollisions;
		}
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00003F57 File Offset: 0x00002157
	protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
	{
		this.InitializeNetworkVariables(null);
		base.OnNetworkPreSpawn(ref networkManager);
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00003F67 File Offset: 0x00002167
	public override void OnNetworkSpawn()
	{
		this.Buffer.OnListChanged += this.OnBufferChanged;
		base.OnNetworkSpawn();
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00003F86 File Offset: 0x00002186
	protected override void OnNetworkPostSpawn()
	{
		if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsConnectedClient)
		{
			this.ProcessInitialNetworkVariableValues();
		}
		base.OnNetworkPostSpawn();
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00003FAC File Offset: 0x000021AC
	protected override void OnNetworkSessionSynchronized()
	{
		this.ProcessInitialNetworkVariableValues();
		base.OnNetworkSessionSynchronized();
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00003FBA File Offset: 0x000021BA
	public override void OnNetworkDespawn()
	{
		this.Buffer.OnListChanged -= this.OnBufferChanged;
		base.OnNetworkDespawn();
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00003FD9 File Offset: 0x000021D9
	public void InitializeNetworkVariables(List<NetworkObjectCollision> buffer = null)
	{
		if (this.isNetworkVariablesInitialized)
		{
			return;
		}
		this.isNetworkVariablesInitialized = true;
		this.Buffer = new NetworkList<NetworkObjectCollision>(buffer, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00003FFC File Offset: 0x000021FC
	private void ProcessInitialNetworkVariableValues()
	{
		this.OnBufferChanged(new NetworkListEvent<NetworkObjectCollision>
		{
			Type = NetworkListEvent<NetworkObjectCollision>.EventType.Full
		});
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00004020 File Offset: 0x00002220
	private void OnBufferChanged(NetworkListEvent<NetworkObjectCollision> changeEvent)
	{
		this.cachedNetworkObjectCollisions = null;
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x0000402C File Offset: 0x0000222C
	private void OnCollisionEnter(Collision collision)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if ((this.collisionLayers.value & 1 << collision.gameObject.layer) == 0)
		{
			return;
		}
		NetworkObject component = collision.gameObject.GetComponent<NetworkObject>();
		if (!component)
		{
			return;
		}
		NetworkObjectReference networkObjectReference = new NetworkObjectReference(component);
		NetworkObjectCollision? networkObjectCollision = null;
		foreach (NetworkObjectCollision networkObjectCollision2 in this.Buffer)
		{
			NetworkObjectReference networkObjectReference2 = networkObjectCollision2.NetworkObjectReference;
			if (networkObjectReference2.Equals(networkObjectReference))
			{
				networkObjectCollision = new NetworkObjectCollision?(networkObjectCollision2);
				break;
			}
		}
		if (networkObjectCollision != null && this.Buffer.Contains(networkObjectCollision.Value))
		{
			this.Buffer.Remove(networkObjectCollision.Value);
		}
		if (this.Buffer.Count >= this.bufferSize)
		{
			this.Buffer.RemoveAt(0);
		}
		this.Buffer.Add(new NetworkObjectCollision
		{
			NetworkObjectReference = networkObjectReference,
			Time = Time.time
		});
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x0000416C File Offset: 0x0000236C
	protected override void __initializeVariables()
	{
		bool flag = this.Buffer == null;
		if (flag)
		{
			throw new Exception("NetworkObjectCollisionRecorder.Buffer cannot be null. All NetworkVariableBase instances must be initialized.");
		}
		this.Buffer.Initialize(this);
		base.__nameNetworkVariable(this.Buffer, "Buffer");
		this.NetworkVariableFields.Add(this.Buffer);
		base.__initializeVariables();
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x000041CF File Offset: 0x000023CF
	protected internal override string __getTypeName()
	{
		return "NetworkObjectCollisionRecorder";
	}

	// Token: 0x04000056 RID: 86
	[Header("Settings")]
	[SerializeField]
	private int bufferSize = 10;

	// Token: 0x04000057 RID: 87
	[SerializeField]
	private LayerMask collisionLayers;

	// Token: 0x04000058 RID: 88
	[HideInInspector]
	public NetworkList<NetworkObjectCollision> Buffer;

	// Token: 0x04000059 RID: 89
	private List<NetworkObjectCollision> cachedNetworkObjectCollisions;

	// Token: 0x0400005A RID: 90
	private bool isNetworkVariablesInitialized;
}
