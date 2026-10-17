using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020001E7 RID: 487
public class UINewServerController : UIViewController<UINewServer>
{
	// Token: 0x06000F23 RID: 3875 RVA: 0x00048128 File Offset: 0x00046328
	public override void Awake()
	{
		base.Awake();
		this.uiNewServer = base.GetComponent<UINewServer>();
		EventManager.AddEventListener("Event_OnPlayerDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerDataChanged));
		WebSocketManager.AddMessageListener("playerGetProbesResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerGetProbesResponse));
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x00048168 File Offset: 0x00046368
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnPlayerDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerDataChanged));
		WebSocketManager.RemoveMessageListener("playerGetProbesResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerGetProbesResponse));
		base.OnDestroy();
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x0004819C File Offset: 0x0004639C
	private void Event_OnPlayerDataChanged(Dictionary<string, object> message)
	{
		PlayerData playerData = (PlayerData)message["newPlayerData"];
		if (playerData == null)
		{
			return;
		}
		this.uiNewServer.SetCloudAvailable(playerData.patreonLevel >= 1);
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x000481D8 File Offset: 0x000463D8
	private void WebSocket_Event_OnPlayerGetProbesResponse(Dictionary<string, object> message)
	{
		Probe[] dedicatedProbes = (from probe in ((InMessage)message["inMessage"]).GetData<PlayerGetProbesResponse>().data.probes
		group probe by probe.ToLabel() into @group
		select (from probe in @group
		orderby probe.priority descending
		select probe).First<Probe>()).ToArray<Probe>();
		this.uiNewServer.SetDedicatedProbes(dedicatedProbes);
	}

	// Token: 0x04000999 RID: 2457
	private static readonly Logger Logger = new Logger("UINewServerController");

	// Token: 0x0400099A RID: 2458
	private UINewServer uiNewServer;
}
