using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x0200000A RID: 10
public class CrowdMember : MonoBehaviour
{
	// Token: 0x06000024 RID: 36 RVA: 0x00002488 File Offset: 0x00000688
	private void Awake()
	{
		this.animator = base.GetComponent<Animator>();
		this.animator.playableGraph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x000024B5 File Offset: 0x000006B5
	public void PlayAnimation(string animationName)
	{
		this.StopAnimations();
		this.animator.SetBool(animationName, true);
		this.animationRequested = true;
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000024D1 File Offset: 0x000006D1
	public void StopAnimations()
	{
		this.animator.SetBool("Seated", false);
		this.animator.SetBool("Cheering", false);
		this.animator.SetBool("Standing", false);
		this.animationRequested = true;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002510 File Offset: 0x00000710
	public void UpdateAnimation()
	{
		if (!this.playerMesh)
		{
			return;
		}
		AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
		bool flag = currentAnimatorStateInfo.loop || currentAnimatorStateInfo.normalizedTime < 1f || this.animator.IsInTransition(0) || this.animationRequested;
		double num = Time.timeAsDouble - this.lastUpdateTime;
		if (flag)
		{
			this.animator.Update((float)num);
		}
		else
		{
			this.playerMesh.LookAt(this.LookTarget ? this.LookTarget.position : Vector3.zero, (float)num, false, true);
		}
		if (this.animationRequested)
		{
			this.animationRequested = false;
		}
		this.lastUpdateTime = Time.timeAsDouble;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x000025CC File Offset: 0x000007CC
	public void RandomizeAppearance()
	{
		this.playerMesh.SetUsername(null);
		this.playerMesh.SetNumber(null);
		this.playerMesh.SetLegsPadsActive(false);
		int headgearID = this.headgearOptions[Random.Range(0, this.headgearOptions.Length)];
		this.playerMesh.SetHeadgearID(headgearID, PlayerRole.None);
		this.RandomizeJersey();
		int mustacheID = this.mustacheOptions[Random.Range(0, this.mustacheOptions.Length)];
		this.playerMesh.SetMustacheID(mustacheID);
		int beardID = this.beardOptions[Random.Range(0, this.beardOptions.Length)];
		this.playerMesh.SetBeardID(beardID);
	}

	// Token: 0x06000029 RID: 41 RVA: 0x0000266C File Offset: 0x0000086C
	private void RandomizeJersey()
	{
		int jerseyID = this.spectatorJerseyOptions[Random.Range(0, this.spectatorJerseyOptions.Length)];
		this.playerMesh.PlayerGroin.SetJerseyID(jerseyID, PlayerTeam.None);
		if (Random.value < 0.45f)
		{
			PlayerTeam team = (Random.value < 0.5f) ? PlayerTeam.Blue : PlayerTeam.Red;
			int jerseyID2 = this.teamJerseyOptions[Random.Range(0, this.teamJerseyOptions.Length)];
			this.playerMesh.PlayerTorso.SetJerseyID(jerseyID2, team);
			return;
		}
		int jerseyID3 = this.spectatorJerseyOptions[Random.Range(0, this.spectatorJerseyOptions.Length)];
		this.playerMesh.PlayerTorso.SetJerseyID(jerseyID3, PlayerTeam.None);
	}

	// Token: 0x04000014 RID: 20
	[Header("References")]
	[SerializeField]
	private PlayerMesh playerMesh;

	// Token: 0x04000015 RID: 21
	[SerializeField]
	private Animator animator;

	// Token: 0x04000016 RID: 22
	[HideInInspector]
	public Transform LookTarget;

	// Token: 0x04000017 RID: 23
	private bool animationRequested;

	// Token: 0x04000018 RID: 24
	private double lastUpdateTime;

	// Token: 0x04000019 RID: 25
	private int[] headgearOptions = new int[]
	{
		-1,
		537,
		538,
		539
	};

	// Token: 0x0400001A RID: 26
	private int[] teamJerseyOptions = new int[]
	{
		2048,
		2049,
		2050,
		2051,
		2052,
		2053,
		2054
	};

	// Token: 0x0400001B RID: 27
	private int[] spectatorJerseyOptions = new int[]
	{
		2118,
		2119,
		2120,
		2121,
		2122
	};

	// Token: 0x0400001C RID: 28
	private int[] mustacheOptions = new int[]
	{
		-1,
		1024,
		1025,
		1026,
		1027,
		1029,
		1030
	};

	// Token: 0x0400001D RID: 29
	private int[] beardOptions = new int[]
	{
		-1,
		1536,
		1537,
		1538,
		1539,
		1540
	};

	// Token: 0x0400001E RID: 30
	private const float teamJerseyChance = 0.45f;
}
