using System;
using System.Collections.Generic;

// Token: 0x02000210 RID: 528
public class UIServerBrowserController : UIViewController<UIServerBrowser>
{
	// Token: 0x0600104D RID: 4173 RVA: 0x0004D670 File Offset: 0x0004B870
	public override void Awake()
	{
		base.Awake();
		this.uiServerBrowser = base.GetComponent<UIServerBrowser>();
		EventManager.AddEventListener("Event_OnServerBrowserShow", new Action<Dictionary<string, object>>(this.Event_OnServerBrowserShow));
		EventManager.AddEventListener("Event_OnServerBrowserClickRefresh", new Action<Dictionary<string, object>>(this.Event_OnServerBrowserClickRefresh));
		WebSocketManager.AddMessageListener("playerGetServerBrowserEndPointsResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerGetServerBrowserEndPointsResponse));
	}

	// Token: 0x0600104E RID: 4174 RVA: 0x0004D6D4 File Offset: 0x0004B8D4
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnServerBrowserShow", new Action<Dictionary<string, object>>(this.Event_OnServerBrowserShow));
		EventManager.RemoveEventListener("Event_OnServerBrowserClickRefresh", new Action<Dictionary<string, object>>(this.Event_OnServerBrowserClickRefresh));
		WebSocketManager.RemoveMessageListener("playerGetServerBrowserEndPointsResponse", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerGetServerBrowserEndPointsResponse));
		base.OnDestroy();
	}

	// Token: 0x0600104F RID: 4175 RVA: 0x0004D729 File Offset: 0x0004B929
	private void Event_OnServerBrowserShow(Dictionary<string, object> message)
	{
		if (this.uiServerBrowser.ServerCount == 0)
		{
			this.uiServerBrowser.Refresh();
		}
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x0004D743 File Offset: 0x0004B943
	private void Event_OnServerBrowserClickRefresh(Dictionary<string, object> message)
	{
		this.uiServerBrowser.Refresh();
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x0004D750 File Offset: 0x0004B950
	private void WebSocket_Event_OnPlayerGetServerBrowserEndPointsResponse(Dictionary<string, object> message)
	{
		ServerBrowserEndPointsResponse data = ((InMessage)message["inMessage"]).GetData<ServerBrowserEndPointsResponse>();
		List<EndPoint> list = new List<EndPoint>(data.data.endPoints);
		if (data.success)
		{
			this.uiServerBrowser.UpdateEndPoints(list.ToArray());
		}
	}

	// Token: 0x04000A67 RID: 2663
	private UIServerBrowser uiServerBrowser;
}
