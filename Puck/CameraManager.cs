using System;
using System.Collections.Generic;

// Token: 0x020000B8 RID: 184
public static class CameraManager
{
	// Token: 0x060005EE RID: 1518 RVA: 0x00020CC9 File Offset: 0x0001EEC9
	public static void Initialize()
	{
		CameraManagerController.Initialize();
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x00020CD0 File Offset: 0x0001EED0
	public static void Dispose()
	{
		CameraManagerController.Dispose();
		CameraManager.DisableAllCameras();
		CameraManager.cameras.Clear();
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x00020CE8 File Offset: 0x0001EEE8
	public static void RegisterCamera(BaseCamera camera)
	{
		if (CameraManager.cameras.Contains(camera))
		{
			return;
		}
		CameraManager.cameras.Add(camera);
		EventManager.TriggerEvent("Event_OnCameraRegistered", new Dictionary<string, object>
		{
			{
				"camera",
				camera
			}
		});
		if (CameraManager.IsActiveCamera(camera))
		{
			CameraManager.EnableCamera(camera);
		}
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x00020D38 File Offset: 0x0001EF38
	public static void UnregisterCamera(BaseCamera camera)
	{
		if (CameraManager.cameras.Contains(camera))
		{
			CameraManager.cameras.Remove(camera);
		}
		if (CameraManager.activeCamera == camera)
		{
			CameraManager.activeCameraType = CameraType.None;
			CameraManager.activeCameraOwnerClientId = null;
			CameraManager.activeCamera = null;
		}
		EventManager.TriggerEvent("Event_OnCameraUnregistered", new Dictionary<string, object>
		{
			{
				"camera",
				camera
			}
		});
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x00020DA0 File Offset: 0x0001EFA0
	public static BaseCamera GetCameraByType(CameraType cameraType)
	{
		return CameraManager.cameras.Find((BaseCamera camera) => camera.Type == cameraType);
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x00020DD0 File Offset: 0x0001EFD0
	public static BaseCamera GetCameraByOwnerClientId(ulong ownerClientId)
	{
		return CameraManager.cameras.Find((BaseCamera camera) => camera.OwnerClientId == ownerClientId);
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x00020E00 File Offset: 0x0001F000
	public static BaseCamera GetActiveCamera()
	{
		return CameraManager.cameras.Find((BaseCamera camera) => CameraManager.IsActiveCamera(camera));
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x00020E2C File Offset: 0x0001F02C
	public static void SetActiveCamera(CameraType type, ulong? ownerClientId = null)
	{
		CameraManager.Logger.Info(string.Format("Setting active camera to type {0}", type));
		CameraManager.activeCameraType = type;
		CameraManager.activeCameraOwnerClientId = ownerClientId;
		BaseCamera baseCamera = CameraManager.GetActiveCamera();
		if (baseCamera != null)
		{
			CameraManager.EnableCamera(baseCamera);
		}
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x00020E74 File Offset: 0x0001F074
	public static bool IsActiveCamera(BaseCamera camera)
	{
		if (CameraManager.activeCameraType != camera.Type)
		{
			return false;
		}
		if (CameraManager.activeCameraOwnerClientId != null)
		{
			ulong? num = CameraManager.activeCameraOwnerClientId;
			ulong ownerClientId = camera.OwnerClientId;
			return num.GetValueOrDefault() == ownerClientId & num != null;
		}
		return true;
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x00020EBD File Offset: 0x0001F0BD
	public static void EnableCamera(BaseCamera camera)
	{
		if (camera.IsEnabled)
		{
			return;
		}
		CameraManager.DisableAllCameras();
		CameraManager.Logger.Info(string.Format("Enabling camera of type {0}", camera.Type));
		camera.Enable();
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x00020EF3 File Offset: 0x0001F0F3
	public static void DisableCamera(BaseCamera camera)
	{
		if (!camera.IsEnabled)
		{
			return;
		}
		CameraManager.Logger.Info(string.Format("Disabling camera of type {0}", camera.Type));
		camera.Disable();
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x00020F24 File Offset: 0x0001F124
	public static void DisableAllCameras()
	{
		foreach (BaseCamera camera in CameraManager.cameras)
		{
			CameraManager.DisableCamera(camera);
		}
	}

	// Token: 0x040003DD RID: 989
	private static readonly Logger Logger = new Logger("CameraManager");

	// Token: 0x040003DE RID: 990
	private static List<BaseCamera> cameras = new List<BaseCamera>();

	// Token: 0x040003DF RID: 991
	private static BaseCamera activeCamera = null;

	// Token: 0x040003E0 RID: 992
	private static CameraType activeCameraType = CameraType.None;

	// Token: 0x040003E1 RID: 993
	private static ulong? activeCameraOwnerClientId = null;
}
