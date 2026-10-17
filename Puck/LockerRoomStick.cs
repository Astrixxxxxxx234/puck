using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class LockerRoomStick : MonoBehaviour
{
	// Token: 0x0600008C RID: 140 RVA: 0x00003A35 File Offset: 0x00001C35
	public void SetSkinID(int skinID, PlayerTeam team, PlayerRole role)
	{
		((role == PlayerRole.Goalie) ? this.goalieStickMesh : this.attackerStickMesh).SetSkinID(skinID, team);
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00003A50 File Offset: 0x00001C50
	public void SetShaftTapeID(int shaftTapeID, PlayerRole role)
	{
		((role == PlayerRole.Goalie) ? this.goalieStickMesh : this.attackerStickMesh).SetShaftTapeID(shaftTapeID);
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00003A6A File Offset: 0x00001C6A
	public void SetBladeTapeID(int bladeTapeID, PlayerRole role)
	{
		((role == PlayerRole.Goalie) ? this.goalieStickMesh : this.attackerStickMesh).SetBladeTapeID(bladeTapeID);
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00003A84 File Offset: 0x00001C84
	public void ShowRoleStick(PlayerRole role)
	{
		this.attackerStickMesh.gameObject.SetActive(role == PlayerRole.Attacker);
		this.goalieStickMesh.gameObject.SetActive(role == PlayerRole.Goalie);
	}

	// Token: 0x04000049 RID: 73
	[Header("References")]
	[SerializeField]
	private StickMesh attackerStickMesh;

	// Token: 0x0400004A RID: 74
	[SerializeField]
	private StickMesh goalieStickMesh;
}
