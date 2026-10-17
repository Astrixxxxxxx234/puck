using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class LockerRoomPlayer : MonoBehaviour
{
	// Token: 0x06000070 RID: 112 RVA: 0x000032D4 File Offset: 0x000014D4
	private void Awake()
	{
		this.initialRotation = base.transform.rotation.eulerAngles;
		this.targetRotation = this.initialRotation;
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00003306 File Offset: 0x00001506
	private void Start()
	{
		this.SetRotationFromPreset(this.defaultRotationPreset);
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00003314 File Offset: 0x00001514
	private void Update()
	{
		Vector2 vector = InputManager.PointAction.ReadValue<Vector2>();
		if (this.AllowRotation)
		{
			if (InputManager.ClickAction.WasPressedThisFrame() && !GlobalStateManager.UIState.IsMouseOverUI)
			{
				this.IsRotating = true;
				this.lastPointerPosition = vector;
			}
			else if (InputManager.ClickAction.WasReleasedThisFrame())
			{
				this.IsRotating = false;
			}
			if (this.IsRotating)
			{
				Vector2 vector2 = vector - this.lastPointerPosition;
				this.lastPointerPosition = vector;
				if (this.IsRotating)
				{
					this.targetRotation.y = this.targetRotation.y + vector2.x * this.rotationSpeed * Time.deltaTime;
				}
			}
		}
		Quaternion b = Quaternion.Euler(this.targetRotation);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime / this.rotationSmoothing);
		BaseCamera activeCamera = CameraManager.GetActiveCamera();
		if (activeCamera)
		{
			Plane plane = new Plane(activeCamera.transform.forward, activeCamera.transform.position + activeCamera.transform.forward);
			Ray ray = activeCamera.UnityCamera.ScreenPointToRay(vector);
			float distance;
			if (plane.Raycast(ray, out distance))
			{
				Vector3 point = ray.GetPoint(distance);
				Vector3 vector3 = activeCamera.transform.InverseTransformPoint(point);
				vector3.x *= Vector3.Dot(base.transform.forward, activeCamera.transform.forward);
				vector3.y += 0.5f;
				vector3.z *= 2f;
				Vector3 vector4 = base.transform.position + base.transform.right * vector3.x + base.transform.up * vector3.y + base.transform.forward * vector3.z;
				this.lookAtPosition = vector4;
			}
		}
		this.playerMesh.LookAt(this.lookAtPosition, Time.deltaTime, true, true);
	}

	// Token: 0x06000073 RID: 115 RVA: 0x0000352E File Offset: 0x0000172E
	public void SetRotationFromPreset(string name)
	{
		if (!this.rotationPresets.ContainsKey(name))
		{
			LockerRoomPlayer.Logger.Error("Rotation preset " + name + " does not exist");
			return;
		}
		this.targetRotation = this.rotationPresets[name];
	}

	// Token: 0x06000074 RID: 116 RVA: 0x0000356B File Offset: 0x0000176B
	public void SetUsername(string username)
	{
		this.playerMesh.SetUsername(username);
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00003579 File Offset: 0x00001779
	public void SetNumber(string number)
	{
		this.playerMesh.SetNumber(number);
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00003587 File Offset: 0x00001787
	public void SetLegsPadsActive(bool isActive)
	{
		this.playerMesh.SetLegsPadsActive(isActive);
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00003595 File Offset: 0x00001795
	public void SetFlagID(int flagID)
	{
		this.playerMesh.SetFlagID(flagID);
	}

	// Token: 0x06000078 RID: 120 RVA: 0x000035A3 File Offset: 0x000017A3
	public void SetHeadgearID(int headgearID, PlayerRole role)
	{
		this.playerMesh.SetHeadgearID(headgearID, role);
	}

	// Token: 0x06000079 RID: 121 RVA: 0x000035B2 File Offset: 0x000017B2
	public void SetMustacheID(int mustacheID)
	{
		this.playerMesh.SetMustacheID(mustacheID);
	}

	// Token: 0x0600007A RID: 122 RVA: 0x000035C0 File Offset: 0x000017C0
	public void SetBeardID(int beardID)
	{
		this.playerMesh.SetBeardID(beardID);
	}

	// Token: 0x0600007B RID: 123 RVA: 0x000035CE File Offset: 0x000017CE
	public void SetJerseyID(int jerseyID, PlayerTeam team)
	{
		this.playerMesh.SetJerseyID(jerseyID, team);
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000035DD File Offset: 0x000017DD
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		Gizmos.DrawSphere(this.lookAtPosition, 0.05f);
	}

	// Token: 0x0400003C RID: 60
	private static readonly global::Logger Logger = new global::Logger("LockerRoomPlayer");

	// Token: 0x0400003D RID: 61
	[Header("Settings")]
	[SerializeField]
	private float rotationSpeed = 10f;

	// Token: 0x0400003E RID: 62
	[SerializeField]
	private float rotationSmoothing = 0.1f;

	// Token: 0x0400003F RID: 63
	[SerializeField]
	private SerializedDictionary<string, Vector3> rotationPresets = new SerializedDictionary<string, Vector3>();

	// Token: 0x04000040 RID: 64
	[SerializeField]
	private string defaultRotationPreset = "front";

	// Token: 0x04000041 RID: 65
	[Header("References")]
	[SerializeField]
	private PlayerMesh playerMesh;

	// Token: 0x04000042 RID: 66
	[HideInInspector]
	public bool AllowRotation;

	// Token: 0x04000043 RID: 67
	[HideInInspector]
	public bool IsRotating;

	// Token: 0x04000044 RID: 68
	private Vector2 lastPointerPosition = Vector2.zero;

	// Token: 0x04000045 RID: 69
	private Vector3 initialRotation;

	// Token: 0x04000046 RID: 70
	private Vector3 targetRotation;

	// Token: 0x04000047 RID: 71
	private Vector3 lookAtPosition;
}
