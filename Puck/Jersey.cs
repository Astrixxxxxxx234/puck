using System;
using UnityEngine;

// Token: 0x02000047 RID: 71
[Serializable]
public class Jersey
{
	// Token: 0x06000165 RID: 357 RVA: 0x0000792C File Offset: 0x00005B2C
	public bool IsForTeam(PlayerTeam team)
	{
		bool result;
		if (team != PlayerTeam.Blue)
		{
			if (team != PlayerTeam.Red)
			{
				result = ((this.Team & JerseyTeam.Any) > (JerseyTeam)0);
			}
			else
			{
				result = ((this.Team & JerseyTeam.Red) > (JerseyTeam)0);
			}
		}
		else
		{
			result = ((this.Team & JerseyTeam.Blue) > (JerseyTeam)0);
		}
		return result;
	}

	// Token: 0x0400012E RID: 302
	public int ID;

	// Token: 0x0400012F RID: 303
	public JerseyTeam Team;

	// Token: 0x04000130 RID: 304
	public Texture Texture;
}
