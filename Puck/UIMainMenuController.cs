using System;
using System.Collections.Generic;

// Token: 0x020001DC RID: 476
public class UIMainMenuController : UIViewController<UIMainMenu>
{
	// Token: 0x06000E88 RID: 3720 RVA: 0x00044CA5 File Offset: 0x00042EA5
	public override void Awake()
	{
		base.Awake();
		this.uiMainMenu = base.GetComponent<UIMainMenu>();
		EventManager.AddEventListener("Event_OnDebugChanged", new Action<Dictionary<string, object>>(this.Event_OnDebugChanged));
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x00044CCF File Offset: 0x00042ECF
	private void Start()
	{
		if (SettingsManager.Debug != DebugMode.Off)
		{
			this.uiMainMenu.ShowDebug();
			return;
		}
		this.uiMainMenu.HideDebug();
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x00044CEF File Offset: 0x00042EEF
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnDebugChanged", new Action<Dictionary<string, object>>(this.Event_OnDebugChanged));
		base.OnDestroy();
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x00044D0D File Offset: 0x00042F0D
	private void Event_OnDebugChanged(Dictionary<string, object> message)
	{
		if ((DebugMode)message["value"] != DebugMode.Off)
		{
			this.uiMainMenu.ShowDebug();
			return;
		}
		this.uiMainMenu.HideDebug();
	}

	// Token: 0x04000946 RID: 2374
	private UIMainMenu uiMainMenu;
}
