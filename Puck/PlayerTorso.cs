using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000048 RID: 72
[RequireComponent(typeof(MeshRendererTexturer))]
[ExecuteInEditMode]
public class PlayerTorso : MonoBehaviour
{
	// Token: 0x06000167 RID: 359 RVA: 0x0000796C File Offset: 0x00005B6C
	private void Awake()
	{
		this.meshRendererTexturer = base.GetComponent<MeshRendererTexturer>();
		if (ApplicationManager.IsDedicatedGameServer)
		{
			this.usernameText.gameObject.SetActive(false);
			this.numberText.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000168 RID: 360 RVA: 0x000079A4 File Offset: 0x00005BA4
	public void SetUsername(string username)
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		if (string.IsNullOrEmpty(username))
		{
			this.usernameText.gameObject.SetActive(false);
			return;
		}
		this.usernameText.text = username;
		this.usernameText.gameObject.SetActive(true);
	}

	// Token: 0x06000169 RID: 361 RVA: 0x000079F0 File Offset: 0x00005BF0
	public void SetNumber(string number)
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		if (string.IsNullOrEmpty(number))
		{
			this.numberText.gameObject.SetActive(false);
			return;
		}
		this.numberText.text = number;
		this.numberText.gameObject.SetActive(true);
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00007A3C File Offset: 0x00005C3C
	public void SetJerseyID(int jerseyID, PlayerTeam team)
	{
		Jersey jersey = this.jerseys.Find((Jersey j) => j.ID == jerseyID && j.IsForTeam(team));
		if (jersey == null)
		{
			PlayerTorso.Logger.Warning(string.Format("Tried to set invalid jerseyID {0} for team {1}", jerseyID, team));
			return;
		}
		this.meshRendererTexturer.SetTexture(jersey.Texture);
	}

	// Token: 0x04000131 RID: 305
	private static readonly global::Logger Logger = new global::Logger("PlayerTorso");

	// Token: 0x04000132 RID: 306
	[Header("Settings")]
	[SerializeField]
	private List<Jersey> jerseys = new List<Jersey>();

	// Token: 0x04000133 RID: 307
	[Header("References")]
	[SerializeField]
	private TMP_Text usernameText;

	// Token: 0x04000134 RID: 308
	[SerializeField]
	private TMP_Text numberText;

	// Token: 0x04000135 RID: 309
	private MeshRendererTexturer meshRendererTexturer;
}
