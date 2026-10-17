using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class Level : NetworkBehaviour
{
	// Token: 0x0600004E RID: 78 RVA: 0x00002DB0 File Offset: 0x00000FB0
	private void Awake()
	{
		MeshRenderer component = this.boundsGameObject.GetComponent<MeshRenderer>();
		if (component == null)
		{
			Level.Logger.Warning("boundsGameObject does not have a MeshRenderer component");
			return;
		}
		this.Bounds = component.bounds;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00002DEE File Offset: 0x00000FEE
	protected override void OnNetworkPostSpawn()
	{
		EventManager.TriggerEvent("Event_Everyone_OnLevelSpawned", new Dictionary<string, object>
		{
			{
				"level",
				this
			}
		});
		base.OnNetworkPostSpawn();
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00002E11 File Offset: 0x00001011
	public override void OnNetworkDespawn()
	{
		EventManager.TriggerEvent("Event_Everyone_OnLevelDespawned", new Dictionary<string, object>
		{
			{
				"level",
				this
			}
		});
		base.OnNetworkDespawn();
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00002E34 File Offset: 0x00001034
	public void SetBlueGoalLightEnabled(bool isEnabled)
	{
		this.blueGoalLight.enabled = isEnabled;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00002E42 File Offset: 0x00001042
	public void SetRedGoalLightEnabled(bool isEnabled)
	{
		this.redGoalLight.enabled = isEnabled;
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00002E50 File Offset: 0x00001050
	public void Server_PlayBlueGoalSound()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.blueGoalSound.Server_Play(-1f, -1f, false, -1, 0f, false, false, false, 0f, false, 0f, -1f);
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00002E9C File Offset: 0x0000109C
	public void Server_PlayRedGoalSound()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.redGoalSound.Server_Play(-1f, -1f, false, -1, 0f, false, false, false, 0f, false, 0f, -1f);
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00002EE8 File Offset: 0x000010E8
	public void Server_PlayerCheerSound(float duration)
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.cheerSounds.ForEach(delegate(SynchronizedAudio cheerSound)
		{
			cheerSound.Server_Play(-1f, -1f, false, -1, 0f, false, false, true, 3f, true, 3f, duration);
		});
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00002F28 File Offset: 0x00001128
	public void Server_PlayHornSound()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			return;
		}
		this.hornSound.Server_Play(-1f, -1f, false, -1, 0f, false, false, false, 0f, false, 0f, -1f);
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00002F98 File Offset: 0x00001198
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x0600005A RID: 90 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00002FAE File Offset: 0x000011AE
	protected internal override string __getTypeName()
	{
		return "Level";
	}

	// Token: 0x0400002F RID: 47
	private static readonly global::Logger Logger = new global::Logger("Level");

	// Token: 0x04000030 RID: 48
	[Header("References")]
	[SerializeField]
	private GameObject boundsGameObject;

	// Token: 0x04000031 RID: 49
	[SerializeField]
	private Light blueGoalLight;

	// Token: 0x04000032 RID: 50
	[SerializeField]
	private Light redGoalLight;

	// Token: 0x04000033 RID: 51
	[SerializeField]
	private SynchronizedAudio blueGoalSound;

	// Token: 0x04000034 RID: 52
	[SerializeField]
	private SynchronizedAudio redGoalSound;

	// Token: 0x04000035 RID: 53
	[SerializeField]
	private List<SynchronizedAudio> cheerSounds = new List<SynchronizedAudio>();

	// Token: 0x04000036 RID: 54
	[SerializeField]
	private SynchronizedAudio hornSound;

	// Token: 0x04000037 RID: 55
	[HideInInspector]
	public Bounds Bounds;
}
