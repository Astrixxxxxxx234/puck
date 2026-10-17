using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class PlayerMesh : MonoBehaviour
{
	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000156 RID: 342 RVA: 0x00007691 File Offset: 0x00005891
	// (set) Token: 0x06000157 RID: 343 RVA: 0x00007699 File Offset: 0x00005899
	public float Stretch
	{
		get
		{
			return this.stretch;
		}
		set
		{
			if (this.stretch == value)
			{
				return;
			}
			this.stretch = value;
			this.OnStretchChanged();
		}
	}

	// Token: 0x06000158 RID: 344 RVA: 0x000076B4 File Offset: 0x000058B4
	private void Awake()
	{
		this.initialGroinBonePosition = this.groinBone.localPosition;
		this.initialTorsoBonePosition = this.torsoBone.localPosition;
		this.initialHeadBonePosition = this.headBone.localPosition;
		this.meshGhoster = base.GetComponent<MeshGhoster>();
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00007700 File Offset: 0x00005900
	public void SetTransparency(float strength)
	{
		if (this.meshGhoster != null)
		{
			this.meshGhoster.SetTransparency(strength);
		}
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000771C File Offset: 0x0000591C
	public void LookAt(Vector3 targetPosition, float deltaTime, bool rotateTorso = true, bool rotateHead = true)
	{
		Quaternion b = Utils.GetLocalLookRotation(this.torsoBone, targetPosition);
		if (rotateTorso && rotateHead)
		{
			b = Utils.GetLocalLookRotation(this.torsoBone, targetPosition);
			b = Quaternion.Slerp(Quaternion.identity, b, 0.5f);
		}
		else if (rotateTorso)
		{
			b = Utils.GetLocalLookRotation(this.torsoBone, targetPosition);
		}
		else if (rotateHead)
		{
			b = Utils.GetLocalLookRotation(this.headBone, targetPosition);
		}
		Vector3 vector = Utils.WrapEulerAngles(b.eulerAngles);
		vector = Utils.Vector3Clamp(vector, new Vector3(-11.25f, -45f, 0f), new Vector3(45f, 45f, 0f));
		if (rotateTorso)
		{
			this.torsoBone.localRotation = Quaternion.Lerp(this.torsoBone.localRotation, Quaternion.Euler(vector), this.lookAtSpeed * deltaTime);
		}
		if (rotateHead)
		{
			this.headBone.localRotation = Quaternion.Lerp(this.headBone.localRotation, Quaternion.Euler(vector), this.lookAtSpeed * deltaTime);
		}
	}

	// Token: 0x0600015B RID: 347 RVA: 0x00007814 File Offset: 0x00005A14
	public void SetUsername(string username)
	{
		this.PlayerTorso.SetUsername(username);
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00007822 File Offset: 0x00005A22
	public void SetNumber(string number)
	{
		this.PlayerTorso.SetNumber(number);
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00007830 File Offset: 0x00005A30
	public void SetLegsPadsActive(bool isActive)
	{
		this.PlayerLegPadLeft.gameObject.SetActive(isActive);
		this.PlayerLegPadRight.gameObject.SetActive(isActive);
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00007854 File Offset: 0x00005A54
	public void SetFlagID(int flagID)
	{
		this.PlayerHead.SetFlagID(flagID);
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00007862 File Offset: 0x00005A62
	public void SetHeadgearID(int headgearID, PlayerRole role)
	{
		this.PlayerHead.SetHeadgearID(headgearID, role);
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00007871 File Offset: 0x00005A71
	public void SetMustacheID(int mustacheID)
	{
		this.PlayerHead.SetMustacheID(mustacheID);
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0000787F File Offset: 0x00005A7F
	public void SetBeardID(int beardID)
	{
		this.PlayerHead.SetBeardID(beardID);
	}

	// Token: 0x06000162 RID: 354 RVA: 0x0000788D File Offset: 0x00005A8D
	public void SetJerseyID(int jerseyID, PlayerTeam team)
	{
		this.PlayerTorso.SetJerseyID(jerseyID, team);
		this.PlayerGroin.SetJerseyID(jerseyID, team);
	}

	// Token: 0x06000163 RID: 355 RVA: 0x000078AC File Offset: 0x00005AAC
	private void OnStretchChanged()
	{
		this.groinBone.localPosition = this.initialGroinBonePosition * this.Stretch;
		this.torsoBone.localPosition = this.initialTorsoBonePosition * this.Stretch;
		this.headBone.localPosition = this.initialHeadBonePosition * this.Stretch;
	}

	// Token: 0x0400011C RID: 284
	[Header("Settings")]
	[SerializeField]
	private float lookAtSpeed = 10f;

	// Token: 0x0400011D RID: 285
	[Header("References")]
	[SerializeField]
	private Transform groinBone;

	// Token: 0x0400011E RID: 286
	[SerializeField]
	private Transform torsoBone;

	// Token: 0x0400011F RID: 287
	[SerializeField]
	private Transform headBone;

	// Token: 0x04000120 RID: 288
	[SerializeField]
	public PlayerHead PlayerHead;

	// Token: 0x04000121 RID: 289
	[SerializeField]
	public PlayerTorso PlayerTorso;

	// Token: 0x04000122 RID: 290
	[SerializeField]
	public PlayerGroin PlayerGroin;

	// Token: 0x04000123 RID: 291
	[SerializeField]
	public PlayerLegPad PlayerLegPadLeft;

	// Token: 0x04000124 RID: 292
	[SerializeField]
	public PlayerLegPad PlayerLegPadRight;

	// Token: 0x04000125 RID: 293
	private MeshGhoster meshGhoster;

	// Token: 0x04000126 RID: 294
	private float stretch = 1f;

	// Token: 0x04000127 RID: 295
	private Vector3 initialGroinBonePosition;

	// Token: 0x04000128 RID: 296
	private Vector3 initialTorsoBonePosition;

	// Token: 0x04000129 RID: 297
	private Vector3 initialHeadBonePosition;
}
