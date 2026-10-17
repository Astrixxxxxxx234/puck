using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class BaseCameraController : MonoBehaviour
{
	// Token: 0x0600000F RID: 15 RVA: 0x0000220B File Offset: 0x0000040B
	public virtual void Awake()
	{
		this.baseCamera = base.GetComponent<BaseCamera>();
		EventManager.AddEventListener("Event_OnFovChanged", new Action<Dictionary<string, object>>(this.Event_OnFovChanged));
	}

	// Token: 0x06000010 RID: 16 RVA: 0x0000222F File Offset: 0x0000042F
	public virtual void Start()
	{
		this.baseCamera.SetFieldOfView(SettingsManager.Fov);
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002241 File Offset: 0x00000441
	public virtual void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_OnFovChanged", new Action<Dictionary<string, object>>(this.Event_OnFovChanged));
	}

	// Token: 0x06000012 RID: 18 RVA: 0x0000225C File Offset: 0x0000045C
	private void Event_OnFovChanged(Dictionary<string, object> message)
	{
		float fieldOfView = (float)message["value"];
		this.baseCamera.SetFieldOfView(fieldOfView);
	}

	// Token: 0x04000007 RID: 7
	private BaseCamera baseCamera;
}
