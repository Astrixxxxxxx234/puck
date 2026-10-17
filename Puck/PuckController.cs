using System;
using Unity.Netcode;

// Token: 0x02000074 RID: 116
public class PuckController : NetworkBehaviour
{
	// Token: 0x060003A8 RID: 936 RVA: 0x000163B7 File Offset: 0x000145B7
	private void Awake()
	{
		this.puck = base.GetComponent<Puck>();
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x00015BC1 File Offset: 0x00013DC1
	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
	}

	// Token: 0x060003AA RID: 938 RVA: 0x000163C5 File Offset: 0x000145C5
	public override void OnNetworkDespawn()
	{
		base.OnNetworkDespawn();
	}

	// Token: 0x060003AC RID: 940 RVA: 0x000163D0 File Offset: 0x000145D0
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x060003AD RID: 941 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x060003AE RID: 942 RVA: 0x000163E6 File Offset: 0x000145E6
	protected internal override string __getTypeName()
	{
		return "PuckController";
	}

	// Token: 0x040002C1 RID: 705
	private Puck puck;
}
