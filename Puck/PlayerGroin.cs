using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000035 RID: 53
[RequireComponent(typeof(MeshRendererTexturer))]
[ExecuteInEditMode]
public class PlayerGroin : MonoBehaviour
{
	// Token: 0x0600011B RID: 283 RVA: 0x000066C9 File Offset: 0x000048C9
	private void Awake()
	{
		this.meshRendererTexturer = base.GetComponent<MeshRendererTexturer>();
	}

	// Token: 0x0600011C RID: 284 RVA: 0x000066D8 File Offset: 0x000048D8
	public void SetJerseyID(int jerseyID, PlayerTeam team)
	{
		Jersey jersey = this.jerseys.Find((Jersey j) => j.ID == jerseyID && j.IsForTeam(team));
		if (jersey == null)
		{
			PlayerGroin.Logger.Warning(string.Format("Tried to set invalid jerseyID {0}", jerseyID));
			return;
		}
		this.meshRendererTexturer.SetTexture(jersey.Texture);
	}

	// Token: 0x040000D1 RID: 209
	private static readonly global::Logger Logger = new global::Logger("PlayerGroin");

	// Token: 0x040000D2 RID: 210
	[Header("References")]
	[SerializeField]
	private List<Jersey> jerseys = new List<Jersey>();

	// Token: 0x040000D3 RID: 211
	private MeshRendererTexturer meshRendererTexturer;
}
