using System;
using Unity.Netcode;

// Token: 0x02000084 RID: 132
public class SynchronizedObjectController : NetworkBehaviour
{
	// Token: 0x06000426 RID: 1062 RVA: 0x000183D5 File Offset: 0x000165D5
	private void Awake()
	{
		this.synchronizedObject = base.GetComponent<SynchronizedObject>();
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x000183E3 File Offset: 0x000165E3
	public override void OnNetworkSpawn()
	{
		if (!NetworkManager.Singleton.IsServer)
		{
			this.synchronizedObject.DriveExternally(SynchronizedObjectDriveCadence.PerFrame);
		}
		base.OnNetworkSpawn();
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x00018404 File Offset: 0x00016604
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x0001841A File Offset: 0x0001661A
	protected internal override string __getTypeName()
	{
		return "SynchronizedObjectController";
	}

	// Token: 0x04000317 RID: 791
	private SynchronizedObject synchronizedObject;
}
