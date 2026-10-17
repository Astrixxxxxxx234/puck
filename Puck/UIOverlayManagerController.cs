using System;
using System.Collections.Generic;
using Unity.Netcode;

// Token: 0x020001EC RID: 492
internal class UIOverlayManagerController : UIViewController<UIOverlayManager>
{
	// Token: 0x06000F43 RID: 3907 RVA: 0x00048AF0 File Offset: 0x00046CF0
	public override void Awake()
	{
		base.Awake();
		this.uiOverlay = base.GetComponent<UIOverlayManager>();
		EventManager.AddEventListener("Event_Everyone_OnClientConnected", new Action<Dictionary<string, object>>(this.Event_Everyone_OnClientConnected));
		EventManager.AddEventListener("Event_Everyone_OnGoalScored", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGoalScored));
		EventManager.AddEventListener("Event_OnBaseCameraEnabled", new Action<Dictionary<string, object>>(this.Event_OnBaseCameraEnabled));
		EventManager.AddEventListener("Event_OnBaseCameraDisabled", new Action<Dictionary<string, object>>(this.Event_OnBaseCameraDisabled));
		EventManager.AddEventListener("Event_OnClientStarted", new Action<Dictionary<string, object>>(this.Event_OnClientStarted));
		EventManager.AddEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
		EventManager.AddEventListener("Event_OnPopupShow", new Action<Dictionary<string, object>>(this.Event_OnPopupShow));
		EventManager.AddEventListener("Event_OnPopupHide", new Action<Dictionary<string, object>>(this.Event_OnPopupHide));
		WebSocketManager.AddMessageListener("playerData", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerData));
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x00048BD5 File Offset: 0x00046DD5
	private void Start()
	{
		this.uiOverlay.ShowOverlay("loading", true, false, true, 0.25f, false, 0.25f);
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x00048BF8 File Offset: 0x00046DF8
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnClientConnected", new Action<Dictionary<string, object>>(this.Event_Everyone_OnClientConnected));
		EventManager.RemoveEventListener("Event_Everyone_OnGoalScored", new Action<Dictionary<string, object>>(this.Event_Everyone_OnGoalScored));
		EventManager.RemoveEventListener("Event_OnBaseCameraEnabled", new Action<Dictionary<string, object>>(this.Event_OnBaseCameraEnabled));
		EventManager.RemoveEventListener("Event_OnBaseCameraDisabled", new Action<Dictionary<string, object>>(this.Event_OnBaseCameraDisabled));
		EventManager.RemoveEventListener("Event_OnClientStarted", new Action<Dictionary<string, object>>(this.Event_OnClientStarted));
		EventManager.RemoveEventListener("Event_OnClientStopped", new Action<Dictionary<string, object>>(this.Event_OnClientStopped));
		EventManager.RemoveEventListener("Event_OnPopupShow", new Action<Dictionary<string, object>>(this.Event_OnPopupShow));
		EventManager.RemoveEventListener("Event_OnPopupHide", new Action<Dictionary<string, object>>(this.Event_OnPopupHide));
		WebSocketManager.RemoveMessageListener("playerData", new Action<Dictionary<string, object>>(this.WebSocket_Event_OnPlayerData));
		base.OnDestroy();
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x00048CD4 File Offset: 0x00046ED4
	private void Event_Everyone_OnClientConnected(Dictionary<string, object> message)
	{
		ulong num = (ulong)message["clientId"];
		if (NetworkManager.Singleton.LocalClientId != num)
		{
			return;
		}
		this.uiOverlay.HideOverlay("connecting");
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x00048D10 File Offset: 0x00046F10
	private void Event_Everyone_OnGoalScored(Dictionary<string, object> message)
	{
		PlayerTeam team = (PlayerTeam)message["byTeam"];
		this.uiOverlay.FlashScreen(Utils.GetTeamColor(team), 0.4f, 0.08f, 0.28f, 0.42f);
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x00048D53 File Offset: 0x00046F53
	private void Event_OnBaseCameraEnabled(Dictionary<string, object> message)
	{
		this.uiOverlay.HideOverlay("camera");
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x00048D65 File Offset: 0x00046F65
	private void Event_OnBaseCameraDisabled(Dictionary<string, object> message)
	{
		this.uiOverlay.ShowOverlay("camera", false, false, true, 0.25f, false, 0.25f);
	}

	// Token: 0x06000F4A RID: 3914 RVA: 0x00048D85 File Offset: 0x00046F85
	private void Event_OnClientStarted(Dictionary<string, object> message)
	{
		this.uiOverlay.ShowOverlay("connecting", true, false, true, 0.25f, false, 0.25f);
	}

	// Token: 0x06000F4B RID: 3915 RVA: 0x00048DA5 File Offset: 0x00046FA5
	private void Event_OnClientStopped(Dictionary<string, object> message)
	{
		this.uiOverlay.HideOverlay("connecting");
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x00048DB8 File Offset: 0x00046FB8
	private void Event_OnPopupShow(Dictionary<string, object> message)
	{
		string a = (string)message["name"];
		if (a == "missingPassword")
		{
			this.uiOverlay.ShowOverlay("missingPassword", false, false, true, 0.25f, false, 0.25f);
			return;
		}
		if (a == "missingMods")
		{
			this.uiOverlay.ShowOverlay("missingMods", false, false, true, 0.25f, false, 0.25f);
			return;
		}
		if (!(a == "downloadingMods"))
		{
			return;
		}
		this.uiOverlay.ShowOverlay("downloadingMods", true, false, true, 0.25f, false, 0.25f);
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x00048E5C File Offset: 0x0004705C
	private void Event_OnPopupHide(Dictionary<string, object> message)
	{
		string a = (string)message["name"];
		if (a == "missingPassword")
		{
			this.uiOverlay.HideOverlay("missingPassword");
			return;
		}
		if (a == "missingMods")
		{
			this.uiOverlay.HideOverlay("missingMods");
			return;
		}
		if (!(a == "downloadingMods"))
		{
			return;
		}
		this.uiOverlay.HideOverlay("downloadingMods");
	}

	// Token: 0x06000F4E RID: 3918 RVA: 0x00048ED4 File Offset: 0x000470D4
	private void WebSocket_Event_OnPlayerData(Dictionary<string, object> message)
	{
		this.uiOverlay.HideOverlay("loading");
	}

	// Token: 0x040009B8 RID: 2488
	private UIOverlayManager uiOverlay;
}
