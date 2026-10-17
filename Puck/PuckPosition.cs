using System;
using System.Collections.Generic;
using Unity.Netcode;

// Token: 0x0200006F RID: 111
public class PuckPosition : NetworkBehaviour
{
	// Token: 0x06000374 RID: 884 RVA: 0x000158E6 File Offset: 0x00013AE6
	protected override void OnNetworkPostSpawn()
	{
		EventManager.TriggerEvent("Event_Everyone_OnPuckPositionSpawned", new Dictionary<string, object>
		{
			{
				"puckPosition",
				this
			}
		});
		base.OnNetworkPostSpawn();
	}

	// Token: 0x06000375 RID: 885 RVA: 0x00015909 File Offset: 0x00013B09
	public override void OnNetworkDespawn()
	{
		EventManager.TriggerEvent("Event_Everyone_OnPuckPositionDespawned", new Dictionary<string, object>
		{
			{
				"puckPosition",
				this
			}
		});
		base.OnNetworkDespawn();
	}

	// Token: 0x06000377 RID: 887 RVA: 0x0001592C File Offset: 0x00013B2C
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x06000378 RID: 888 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00015942 File Offset: 0x00013B42
	protected internal override string __getTypeName()
	{
		return "PuckPosition";
	}

	// Token: 0x04000296 RID: 662
	public GamePhase Phase;
}
