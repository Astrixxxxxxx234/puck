using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class BaseCamera : NetworkBehaviour
{
	// Token: 0x06000005 RID: 5 RVA: 0x0000208E File Offset: 0x0000028E
	public virtual void Awake()
	{
		this.UnityCamera = base.GetComponent<Camera>();
		this.AudioListener = base.GetComponent<AudioListener>();
		this.UnityCamera.enabled = this.IsEnabled;
		this.AudioListener.enabled = this.IsEnabled;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000020CA File Offset: 0x000002CA
	public virtual void Start()
	{
		EventManager.TriggerEvent("Event_OnBaseCameraStarted", new Dictionary<string, object>
		{
			{
				"baseCamera",
				this
			}
		});
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000020E7 File Offset: 0x000002E7
	public override void OnDestroy()
	{
		this.Disable();
		EventManager.TriggerEvent("Event_OnBaseCameraDestroyed", new Dictionary<string, object>
		{
			{
				"baseCamera",
				this
			}
		});
		base.OnDestroy();
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002114 File Offset: 0x00000314
	public virtual bool Enable()
	{
		if (this.IsEnabled)
		{
			return false;
		}
		this.IsEnabled = true;
		this.UnityCamera.enabled = this.IsEnabled;
		this.AudioListener.enabled = this.IsEnabled;
		EventManager.TriggerEvent("Event_OnBaseCameraEnabled", new Dictionary<string, object>
		{
			{
				"baseCamera",
				this
			}
		});
		return true;
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002170 File Offset: 0x00000370
	public virtual bool Disable()
	{
		if (!this.IsEnabled)
		{
			return false;
		}
		this.IsEnabled = false;
		this.UnityCamera.enabled = this.IsEnabled;
		this.AudioListener.enabled = this.IsEnabled;
		EventManager.TriggerEvent("Event_OnBaseCameraDisabled", new Dictionary<string, object>
		{
			{
				"baseCamera",
				this
			}
		});
		return true;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000021CC File Offset: 0x000003CC
	public virtual void SetFieldOfView(float fieldOfView)
	{
		this.UnityCamera.fieldOfView = fieldOfView;
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000021E4 File Offset: 0x000003E4
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000021FA File Offset: 0x000003FA
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002204 File Offset: 0x00000404
	protected internal override string __getTypeName()
	{
		return "BaseCamera";
	}

	// Token: 0x04000003 RID: 3
	[Header("Settings")]
	public global::CameraType Type;

	// Token: 0x04000004 RID: 4
	[HideInInspector]
	public Camera UnityCamera;

	// Token: 0x04000005 RID: 5
	[HideInInspector]
	public AudioListener AudioListener;

	// Token: 0x04000006 RID: 6
	[HideInInspector]
	public bool IsEnabled;
}
