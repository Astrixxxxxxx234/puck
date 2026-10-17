using System;

// Token: 0x02000080 RID: 128
public class SpectatorCameraController : BaseCameraController
{
	// Token: 0x060003EE RID: 1006 RVA: 0x0001758E File Offset: 0x0001578E
	public override void Awake()
	{
		base.Awake();
		this.spectatorCamera = base.GetComponent<SpectatorCamera>();
	}

	// Token: 0x040002FA RID: 762
	private SpectatorCamera spectatorCamera;
}
