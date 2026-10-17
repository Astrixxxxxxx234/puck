using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000031 RID: 49
[RequireComponent(typeof(CreaseGhostView))]
public class CreaseGhostViewController : MonoBehaviour
{
	// Token: 0x17000008 RID: 8
	// (get) Token: 0x060000FB RID: 251 RVA: 0x000059D4 File Offset: 0x00003BD4
	private bool IsLocalGoalie
	{
		get
		{
			return this.playerBody != null && this.playerBody.IsOwner && this.playerBody.Player != null && this.playerBody.Player.Role == PlayerRole.Goalie;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x060000FC RID: 252 RVA: 0x00005A24 File Offset: 0x00003C24
	private bool IsCreaseProtectionEnabled
	{
		get
		{
			return NetworkBehaviourSingleton<GameModeManager>.Instance != null && NetworkBehaviourSingleton<GameModeManager>.Instance.ClientConfig.Value.GoalieCreaseProtection;
		}
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00005A4C File Offset: 0x00003C4C
	private void Awake()
	{
		this.playerBody = base.GetComponent<PlayerBody>();
		this.creaseGhostView = base.GetComponent<CreaseGhostView>();
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		EventManager.AddEventListener("Event_Everyone_OnPlayerEnterZone", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerEnterZone));
		EventManager.AddEventListener("Event_Everyone_OnPlayerExitZone", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerExitZone));
		EventManager.AddEventListener("Event_Everyone_OnPlayerBodyDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodyDespawned));
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00005ABC File Offset: 0x00003CBC
	private void OnDestroy()
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerEnterZone", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerEnterZone));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerExitZone", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerExitZone));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerBodyDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodyDespawned));
		this.creaseGhostView.ResetAll();
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00005B20 File Offset: 0x00003D20
	private void Event_Everyone_OnPlayerEnterZone(Dictionary<string, object> message)
	{
		Zone zone = (Zone)message["zone"];
		if (!CreaseGhostViewController.CreaseZoneIds.Contains(zone.ZoneId))
		{
			return;
		}
		PlayerBody playerBody = (PlayerBody)message["playerBody"];
		if (!this.IsLocalGoalie || !this.IsCreaseProtectionEnabled)
		{
			return;
		}
		if (playerBody == this.playerBody)
		{
			Goal componentInParent = zone.GetComponentInParent<Goal>();
			if (componentInParent == null || componentInParent.Team != this.playerBody.Player.Team)
			{
				return;
			}
			this.creaseGhostView.EnterCrease(zone);
			using (IEnumerator<PlayerBody> enumerator = zone.Occupants.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PlayerBody playerBody2 = enumerator.Current;
					if (this.IsEnemy(playerBody2))
					{
						this.creaseGhostView.AddEnemy(playerBody2);
					}
				}
				return;
			}
		}
		if (zone == this.creaseGhostView.CreaseZone && this.IsEnemy(playerBody))
		{
			this.creaseGhostView.AddEnemy(playerBody);
		}
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00005C30 File Offset: 0x00003E30
	private void Event_Everyone_OnPlayerExitZone(Dictionary<string, object> message)
	{
		if ((Zone)message["zone"] != this.creaseGhostView.CreaseZone)
		{
			return;
		}
		PlayerBody playerBody = (PlayerBody)message["playerBody"];
		if (playerBody == this.playerBody)
		{
			this.creaseGhostView.ExitCrease();
			return;
		}
		this.creaseGhostView.RemoveEnemy(playerBody);
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00005C98 File Offset: 0x00003E98
	private void Event_Everyone_OnPlayerBodyDespawned(Dictionary<string, object> message)
	{
		PlayerBody enemy = (PlayerBody)message["playerBody"];
		this.creaseGhostView.ForgetEnemy(enemy);
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00005CC4 File Offset: 0x00003EC4
	private bool IsEnemy(PlayerBody body)
	{
		if (body == null || body.Player == null || this.playerBody.Player == null)
		{
			return false;
		}
		PlayerTeam team = body.Player.Team;
		return body.Player.Role != PlayerRole.Goalie && (team == PlayerTeam.Blue || team == PlayerTeam.Red) && team != this.playerBody.Player.Team;
	}

	// Token: 0x0400009A RID: 154
	private static readonly HashSet<string> CreaseZoneIds = new HashSet<string>
	{
		"Crease"
	};

	// Token: 0x0400009B RID: 155
	private PlayerBody playerBody;

	// Token: 0x0400009C RID: 156
	private CreaseGhostView creaseGhostView;
}
