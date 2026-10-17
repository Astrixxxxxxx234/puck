using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class CreaseGhostView : MonoBehaviour
{
	// Token: 0x17000007 RID: 7
	// (get) Token: 0x060000F1 RID: 241 RVA: 0x00005690 File Offset: 0x00003890
	public Zone CreaseZone
	{
		get
		{
			return this.creaseZone;
		}
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00005698 File Offset: 0x00003898
	public void EnterCrease(Zone zone)
	{
		this.creaseZone = zone;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x000056A1 File Offset: 0x000038A1
	public void ExitCrease()
	{
		this.creaseZone = null;
		this.enemiesInCrease.Clear();
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x000056B5 File Offset: 0x000038B5
	public void AddEnemy(PlayerBody enemy)
	{
		this.enemiesInCrease.Add(enemy);
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x000056C4 File Offset: 0x000038C4
	public void RemoveEnemy(PlayerBody enemy)
	{
		this.enemiesInCrease.Remove(enemy);
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x000056D3 File Offset: 0x000038D3
	public void ForgetEnemy(PlayerBody enemy)
	{
		this.enemiesInCrease.Remove(enemy);
		this.strengthByEnemy.Remove(enemy);
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x000056F0 File Offset: 0x000038F0
	public void ResetAll()
	{
		foreach (PlayerBody playerBody in this.strengthByEnemy.Keys)
		{
			if (playerBody != null && playerBody.PlayerMesh != null)
			{
				playerBody.PlayerMesh.SetTransparency(0f);
			}
		}
		this.creaseZone = null;
		this.enemiesInCrease.Clear();
		this.strengthByEnemy.Clear();
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00005788 File Offset: 0x00003988
	private void Update()
	{
		bool flag = this.creaseZone != null;
		if (flag)
		{
			foreach (PlayerBody playerBody in this.enemiesInCrease)
			{
				if (playerBody != null && !this.strengthByEnemy.ContainsKey(playerBody))
				{
					this.strengthByEnemy[playerBody] = 0f;
				}
			}
		}
		if (this.strengthByEnemy.Count == 0)
		{
			return;
		}
		this.scratch.Clear();
		this.scratch.AddRange(this.strengthByEnemy.Keys);
		foreach (PlayerBody playerBody2 in this.scratch)
		{
			float num = (flag && playerBody2 != null && this.enemiesInCrease.Contains(playerBody2)) ? this.TargetStrength(playerBody2) : 0f;
			float num2 = Mathf.MoveTowards(this.strengthByEnemy[playerBody2], num, this.fadeSpeed * Time.deltaTime);
			if (playerBody2 != null && playerBody2.PlayerMesh != null)
			{
				playerBody2.PlayerMesh.SetTransparency(num2);
			}
			if (num2 <= 0f && num <= 0f)
			{
				this.strengthByEnemy.Remove(playerBody2);
			}
			else
			{
				this.strengthByEnemy[playerBody2] = num2;
			}
		}
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00005928 File Offset: 0x00003B28
	private float TargetStrength(PlayerBody enemy)
	{
		Vector3 vector = enemy.transform.position - this.creaseZone.Origin;
		vector.y = 0f;
		return Mathf.Clamp01(Mathf.InverseLerp(this.noGhostDistance, this.fullGhostDistance, vector.magnitude));
	}

	// Token: 0x04000093 RID: 147
	[Header("Settings")]
	[SerializeField]
	private float fullGhostDistance = 0.5f;

	// Token: 0x04000094 RID: 148
	[SerializeField]
	private float noGhostDistance = 2.5f;

	// Token: 0x04000095 RID: 149
	[SerializeField]
	private float fadeSpeed = 6f;

	// Token: 0x04000096 RID: 150
	private Zone creaseZone;

	// Token: 0x04000097 RID: 151
	private readonly HashSet<PlayerBody> enemiesInCrease = new HashSet<PlayerBody>();

	// Token: 0x04000098 RID: 152
	private readonly Dictionary<PlayerBody, float> strengthByEnemy = new Dictionary<PlayerBody, float>();

	// Token: 0x04000099 RID: 153
	private readonly List<PlayerBody> scratch = new List<PlayerBody>();
}
