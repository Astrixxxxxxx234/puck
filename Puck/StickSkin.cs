using System;
using UnityEngine;

// Token: 0x02000060 RID: 96
[Serializable]
public class StickSkin
{
	// Token: 0x060002ED RID: 749 RVA: 0x0001359C File Offset: 0x0001179C
	public bool IsForTeam(PlayerTeam team)
	{
		bool result;
		if (team != PlayerTeam.Blue)
		{
			if (team != PlayerTeam.Red)
			{
				result = ((this.Team & StickSkinTeam.Any) > (StickSkinTeam)0);
			}
			else
			{
				result = ((this.Team & StickSkinTeam.Red) > (StickSkinTeam)0);
			}
		}
		else
		{
			result = ((this.Team & StickSkinTeam.Blue) > (StickSkinTeam)0);
		}
		return result;
	}

	// Token: 0x04000224 RID: 548
	public int ID;

	// Token: 0x04000225 RID: 549
	public StickSkinTeam Team;

	// Token: 0x04000226 RID: 550
	public Material Material;
}
