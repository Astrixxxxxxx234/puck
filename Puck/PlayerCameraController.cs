using System;

// Token: 0x0200004F RID: 79
public class PlayerCameraController : BaseCameraController
{
	// Token: 0x060001DA RID: 474 RVA: 0x0000A692 File Offset: 0x00008892
	public override void Awake()
	{
		base.Awake();
		this.playerCamera = base.GetComponent<PlayerCamera>();
	}

	// Token: 0x04000194 RID: 404
	private PlayerCamera playerCamera;
}
