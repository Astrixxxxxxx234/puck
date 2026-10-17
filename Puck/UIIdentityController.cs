using System;
using System.Collections.Generic;

// Token: 0x020001DA RID: 474
public class UIIdentityController : UIViewController<UIIdentity>
{
	// Token: 0x06000E72 RID: 3698 RVA: 0x0004474A File Offset: 0x0004294A
	public override void Awake()
	{
		base.Awake();
		this.uiIdentity = base.GetComponent<UIIdentity>();
		EventManager.AddEventListener("Event_OnPlayerDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerDataChanged));
	}

	// Token: 0x06000E73 RID: 3699 RVA: 0x00044774 File Offset: 0x00042974
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnPlayerDataChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerDataChanged));
		base.OnDestroy();
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x00044794 File Offset: 0x00042994
	private void Event_OnPlayerDataChanged(Dictionary<string, object> message)
	{
		PlayerData playerData = (PlayerData)message["newPlayerData"];
		if (playerData == null)
		{
			return;
		}
		this.uiIdentity.SetIdentity(playerData.username, playerData.number);
	}

	// Token: 0x04000933 RID: 2355
	private UIIdentity uiIdentity;
}
