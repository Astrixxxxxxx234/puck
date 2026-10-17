using System;
using System.Collections.Generic;

// Token: 0x02000018 RID: 24
public class LockerRoomCameraController : BaseCameraController
{
	// Token: 0x06000068 RID: 104 RVA: 0x00003118 File Offset: 0x00001318
	public override void Awake()
	{
		base.Awake();
		this.lockerRoomCamera = base.GetComponent<LockerRoomCamera>();
		EventManager.AddEventListener("Event_OnMainMenuShow", new Action<Dictionary<string, object>>(this.Event_OnMainMenuShow));
		EventManager.AddEventListener("Event_OnPlayerMenuShow", new Action<Dictionary<string, object>>(this.Event_OnPlayerMenuShow));
		EventManager.AddEventListener("Event_OnAppearanceShow", new Action<Dictionary<string, object>>(this.Event_OnAppearanceShow));
		EventManager.AddEventListener("Event_OnAppearanceCategoryChanged", new Action<Dictionary<string, object>>(this.Event_OnAppearanceCategoryChanged));
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00003190 File Offset: 0x00001390
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnMainMenuShow", new Action<Dictionary<string, object>>(this.Event_OnMainMenuShow));
		EventManager.RemoveEventListener("Event_OnPlayerMenuShow", new Action<Dictionary<string, object>>(this.Event_OnPlayerMenuShow));
		EventManager.RemoveEventListener("Event_OnAppearanceShow", new Action<Dictionary<string, object>>(this.Event_OnAppearanceShow));
		EventManager.RemoveEventListener("Event_OnAppearanceCategoryChanged", new Action<Dictionary<string, object>>(this.Event_OnAppearanceCategoryChanged));
		base.OnDestroy();
	}

	// Token: 0x0600006A RID: 106 RVA: 0x000031FB File Offset: 0x000013FB
	private void SetAppearancePosition(AppearanceCategory category, AppearanceSubcategory subcategory)
	{
		if (category == AppearanceCategory.Head)
		{
			this.lockerRoomCamera.SetPosition("headCloseUp");
			return;
		}
		if (category != AppearanceCategory.Stick)
		{
			this.lockerRoomCamera.SetPosition("bodyCloseUp");
			return;
		}
		this.lockerRoomCamera.SetPosition("stickCloseUp");
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00003238 File Offset: 0x00001438
	private void Event_OnMainMenuShow(Dictionary<string, object> message)
	{
		this.lockerRoomCamera.SetPosition("default");
	}

	// Token: 0x0600006C RID: 108 RVA: 0x0000324A File Offset: 0x0000144A
	private void Event_OnPlayerMenuShow(Dictionary<string, object> message)
	{
		this.lockerRoomCamera.SetPosition("bodyCloseUp");
	}

	// Token: 0x0600006D RID: 109 RVA: 0x0000325C File Offset: 0x0000145C
	private void Event_OnAppearanceShow(Dictionary<string, object> message)
	{
		AppearanceCategory category = (AppearanceCategory)message["category"];
		AppearanceSubcategory subcategory = (AppearanceSubcategory)message["subcategory"];
		this.SetAppearancePosition(category, subcategory);
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00003294 File Offset: 0x00001494
	private void Event_OnAppearanceCategoryChanged(Dictionary<string, object> message)
	{
		AppearanceCategory category = (AppearanceCategory)message["category"];
		AppearanceSubcategory subcategory = (AppearanceSubcategory)message["subcategory"];
		this.SetAppearancePosition(category, subcategory);
	}

	// Token: 0x0400003B RID: 59
	private LockerRoomCamera lockerRoomCamera;
}
