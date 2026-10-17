using System;
using UnityEngine;

// Token: 0x02000039 RID: 57
[Serializable]
public class Headgear
{
	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000122 RID: 290 RVA: 0x00006787 File Offset: 0x00004987
	public MeshRendererTexturer FlagMeshRendererTexturer
	{
		get
		{
			if (!this.FlagGameObject)
			{
				return null;
			}
			return this.FlagGameObject.GetComponent<MeshRendererTexturer>();
		}
	}

	// Token: 0x06000123 RID: 291 RVA: 0x000067A4 File Offset: 0x000049A4
	public bool IsForRole(PlayerRole role)
	{
		bool result;
		if (role != PlayerRole.Attacker)
		{
			if (role != PlayerRole.Goalie)
			{
				result = ((this.Role & HeadgearRole.Any) > (HeadgearRole)0);
			}
			else
			{
				result = ((this.Role & HeadgearRole.Goalie) > (HeadgearRole)0);
			}
		}
		else
		{
			result = ((this.Role & HeadgearRole.Attacker) > (HeadgearRole)0);
		}
		return result;
	}

	// Token: 0x040000DC RID: 220
	public int ID;

	// Token: 0x040000DD RID: 221
	public GameObject GameObject;

	// Token: 0x040000DE RID: 222
	public GameObject FlagGameObject;

	// Token: 0x040000DF RID: 223
	public HeadgearRole Role;
}
