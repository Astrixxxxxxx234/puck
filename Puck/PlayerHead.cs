using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200003C RID: 60
[ExecuteInEditMode]
public class PlayerHead : MonoBehaviour
{
	// Token: 0x06000127 RID: 295 RVA: 0x000067E4 File Offset: 0x000049E4
	public void SetFlagID(int flagID)
	{
		this.headgear.ForEach(delegate(Headgear h)
		{
			if (h.FlagGameObject != null)
			{
				h.FlagGameObject.SetActive(false);
			}
		});
		if (flagID == -1)
		{
			return;
		}
		Flag flag = this.flags.FirstOrDefault((Flag f) => f.ID == flagID);
		if (flag == null)
		{
			PlayerHead.Logger.Warning(string.Format("Tried to set invalid flagID {0}", flagID));
			return;
		}
		this.headgear.ForEach(delegate(Headgear h)
		{
			if (h.FlagGameObject != null)
			{
				h.FlagGameObject.SetActive(true);
				if (h.FlagMeshRendererTexturer != null)
				{
					h.FlagMeshRendererTexturer.SetTexture(flag.Texture);
				}
			}
		});
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00006890 File Offset: 0x00004A90
	public void SetHeadgearID(int headgearID, PlayerRole role)
	{
		this.headgear.ForEach(delegate(Headgear h)
		{
			h.GameObject.SetActive(false);
		});
		if (headgearID == -1)
		{
			return;
		}
		Headgear headgear = this.headgear.FirstOrDefault((Headgear h) => h.ID == headgearID && h.IsForRole(role));
		if (headgear == null)
		{
			PlayerHead.Logger.Warning(string.Format("Tried to set invalid headgearID {0} for role {1}", headgearID, role));
			return;
		}
		headgear.GameObject.SetActive(true);
	}

	// Token: 0x06000129 RID: 297 RVA: 0x00006938 File Offset: 0x00004B38
	public void SetMustacheID(int mustacheID)
	{
		this.mustaches.ForEach(delegate(Mustache m)
		{
			m.GameObject.SetActive(false);
		});
		if (mustacheID == -1)
		{
			return;
		}
		Mustache mustache = this.mustaches.FirstOrDefault((Mustache m) => m.ID == mustacheID);
		if (mustache == null)
		{
			PlayerHead.Logger.Warning(string.Format("Tried to set invalid mustacheID {0}", mustacheID));
			return;
		}
		mustache.GameObject.SetActive(true);
	}

	// Token: 0x0600012A RID: 298 RVA: 0x000069D0 File Offset: 0x00004BD0
	public void SetBeardID(int beardID)
	{
		this.beards.ForEach(delegate(Beard b)
		{
			b.GameObject.SetActive(false);
		});
		if (beardID == -1)
		{
			return;
		}
		Beard beard = this.beards.FirstOrDefault((Beard b) => b.ID == beardID);
		if (beard == null)
		{
			PlayerHead.Logger.Warning(string.Format("Tried to set invalid beardID {0}", beardID));
			return;
		}
		beard.GameObject.SetActive(true);
	}

	// Token: 0x040000E4 RID: 228
	private static readonly global::Logger Logger = new global::Logger("PlayerHead");

	// Token: 0x040000E5 RID: 229
	[Header("References")]
	[SerializeField]
	private List<Flag> flags = new List<Flag>();

	// Token: 0x040000E6 RID: 230
	[SerializeField]
	private List<Headgear> headgear = new List<Headgear>();

	// Token: 0x040000E7 RID: 231
	[SerializeField]
	private List<Mustache> mustaches = new List<Mustache>();

	// Token: 0x040000E8 RID: 232
	[SerializeField]
	private List<Beard> beards = new List<Beard>();
}
