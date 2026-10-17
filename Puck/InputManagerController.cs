using System;
using System.Collections.Generic;

// Token: 0x020000E2 RID: 226
public static class InputManagerController
{
	// Token: 0x060006FD RID: 1789 RVA: 0x00026735 File Offset: 0x00024935
	public static void Initialize()
	{
		if (!ApplicationManager.IsDedicatedGameServer)
		{
			InputManager.LoadKeyBinds();
			InputManager.MigrateChatOpenInteractionToPress();
		}
		InputManagerController.AddSettingsEventListeners();
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x0002674D File Offset: 0x0002494D
	public static void Dispose()
	{
		InputManagerController.RemoveSettingsEventListeners();
		if (!ApplicationManager.IsDedicatedGameServer)
		{
			InputManager.SaveKeyBinds();
		}
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x00026760 File Offset: 0x00024960
	private static void AddSettingsEventListeners()
	{
		EventManager.AddEventListener("Event_OnSettingsKeyBindInputClicked", new Action<Dictionary<string, object>>(InputManagerController.Event_OnSettingsKeyBindInputClicked));
		EventManager.AddEventListener("Event_OnSettingsKeyBindInputInteractionChanged", new Action<Dictionary<string, object>>(InputManagerController.Event_OnSettingsKeyBindInputInteractionChanged));
		EventManager.AddEventListener("Event_OnPopupClickOk", new Action<Dictionary<string, object>>(InputManagerController.Event_OnPopupClickOk));
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x000267B0 File Offset: 0x000249B0
	private static void RemoveSettingsEventListeners()
	{
		EventManager.RemoveEventListener("Event_OnSettingsKeyBindInputClicked", new Action<Dictionary<string, object>>(InputManagerController.Event_OnSettingsKeyBindInputClicked));
		EventManager.RemoveEventListener("Event_OnSettingsKeyBindInputInteractionChanged", new Action<Dictionary<string, object>>(InputManagerController.Event_OnSettingsKeyBindInputInteractionChanged));
		EventManager.RemoveEventListener("Event_OnPopupClickOk", new Action<Dictionary<string, object>>(InputManagerController.Event_OnPopupClickOk));
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x000267FF File Offset: 0x000249FF
	private static void Event_OnSettingsKeyBindInputClicked(Dictionary<string, object> message)
	{
		InputManager.RebindButtonInteractively((string)message["actionName"]);
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x00026818 File Offset: 0x00024A18
	private static void Event_OnSettingsKeyBindInputInteractionChanged(Dictionary<string, object> message)
	{
		string actionName = (string)message["actionName"];
		KeyBindInteraction keyBindInteraction = (KeyBindInteraction)message["interaction"];
		InputManager.SetActionInteractions(actionName, Utils.GetInteractionFromKeyBindInteraction(keyBindInteraction));
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x00026851 File Offset: 0x00024A51
	private static void Event_OnPopupClickOk(Dictionary<string, object> message)
	{
		if (((Popup)message["popup"]).Name == "settingsResetToDefault")
		{
			InputManager.ResetToDefault();
		}
	}
}
