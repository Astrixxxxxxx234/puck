using System;
using System.Collections.Generic;
using Unity.Netcode;

// Token: 0x02000094 RID: 148
public class CreaseProtection
{
	// Token: 0x060004C9 RID: 1225 RVA: 0x0001A2B4 File Offset: 0x000184B4
	public CreaseProtection()
	{
		EventManager.AddEventListener("Event_Everyone_OnPlayerBodyDespawned", new Action<Dictionary<string, object>>(this.OnPlayerBodyDespawned));
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x0001A2E8 File Offset: 0x000184E8
	public void Dispose()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerBodyDespawned", new Action<Dictionary<string, object>>(this.OnPlayerBodyDespawned));
		foreach (CreaseProtection.Crease crease in this.creases.Values)
		{
			crease.Goalie = null;
			crease.Enemies.Clear();
			this.Reconcile(crease);
		}
		this.creases.Clear();
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0001A374 File Offset: 0x00018574
	public void OnPlayerEnterZone(Zone zone, PlayerBody playerBody)
	{
		if (!CreaseProtection.IsServer)
		{
			return;
		}
		if (!CreaseProtection.CreaseZoneIds.Contains(zone.ZoneId))
		{
			return;
		}
		CreaseProtection.Crease orCreateCrease = this.GetOrCreateCrease(zone);
		if (orCreateCrease == null)
		{
			return;
		}
		if (CreaseProtection.IsDefendingGoalie(playerBody, orCreateCrease.DefendingTeam))
		{
			orCreateCrease.Goalie = playerBody;
		}
		else if (CreaseProtection.IsEnemySkater(playerBody, orCreateCrease.DefendingTeam))
		{
			orCreateCrease.Enemies.Add(playerBody);
		}
		this.Reconcile(orCreateCrease);
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x0001A3E4 File Offset: 0x000185E4
	public void OnPlayerExitZone(Zone zone, PlayerBody playerBody)
	{
		if (!CreaseProtection.IsServer)
		{
			return;
		}
		CreaseProtection.Crease crease;
		if (!this.creases.TryGetValue(zone, out crease))
		{
			return;
		}
		if (crease.Goalie == playerBody)
		{
			crease.Goalie = null;
		}
		else
		{
			crease.Enemies.Remove(playerBody);
		}
		this.Reconcile(crease);
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x0001A438 File Offset: 0x00018638
	private void OnPlayerBodyDespawned(Dictionary<string, object> message)
	{
		if (!CreaseProtection.IsServer)
		{
			return;
		}
		PlayerBody playerBody = (PlayerBody)message["playerBody"];
		foreach (CreaseProtection.Crease crease in this.creases.Values)
		{
			bool flag = crease.Enemies.Remove(playerBody);
			if (crease.Goalie == playerBody)
			{
				crease.Goalie = null;
				flag = true;
			}
			if (flag)
			{
				this.Reconcile(crease);
			}
		}
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x0001A4D0 File Offset: 0x000186D0
	private void Reconcile(CreaseProtection.Crease crease)
	{
		this.restoreScratch.Clear();
		foreach (PlayerBody item in crease.Filtered)
		{
			if (crease.PhasedGoalie != crease.Goalie || crease.Goalie == null || !crease.Enemies.Contains(item))
			{
				this.restoreScratch.Add(item);
			}
		}
		foreach (PlayerBody playerBody in this.restoreScratch)
		{
			if (crease.PhasedGoalie != null)
			{
				crease.PhasedGoalie.SetCollisionFilteredWith(playerBody, false);
			}
			crease.Filtered.Remove(playerBody);
		}
		crease.PhasedGoalie = crease.Goalie;
		if (crease.Goalie == null)
		{
			return;
		}
		foreach (PlayerBody playerBody2 in crease.Enemies)
		{
			if (crease.Filtered.Add(playerBody2))
			{
				crease.Goalie.SetCollisionFilteredWith(playerBody2, true);
			}
		}
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x0001A63C File Offset: 0x0001883C
	private CreaseProtection.Crease GetOrCreateCrease(Zone zone)
	{
		CreaseProtection.Crease crease;
		if (this.creases.TryGetValue(zone, out crease))
		{
			return crease;
		}
		Goal componentInParent = zone.GetComponentInParent<Goal>();
		if (componentInParent == null)
		{
			CreaseProtection.Logger.Warning("Crease zone '" + zone.name + "' is not under a Goal — can't resolve a defending team.");
			return null;
		}
		crease = new CreaseProtection.Crease
		{
			DefendingTeam = componentInParent.Team
		};
		this.creases[zone] = crease;
		return crease;
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x0001A6AC File Offset: 0x000188AC
	private static bool IsDefendingGoalie(PlayerBody playerBody, PlayerTeam defendingTeam)
	{
		return playerBody != null && playerBody.Player != null && playerBody.Player.Role == PlayerRole.Goalie && playerBody.Player.Team == defendingTeam;
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x0001A6E4 File Offset: 0x000188E4
	private static bool IsEnemySkater(PlayerBody playerBody, PlayerTeam defendingTeam)
	{
		if (playerBody == null || playerBody.Player == null)
		{
			return false;
		}
		PlayerTeam team = playerBody.Player.Team;
		return playerBody.Player.Role != PlayerRole.Goalie && (team == PlayerTeam.Blue || team == PlayerTeam.Red) && team != defendingTeam;
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x060004D2 RID: 1234 RVA: 0x0001A735 File Offset: 0x00018935
	private static bool IsServer
	{
		get
		{
			return NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer;
		}
	}

	// Token: 0x04000354 RID: 852
	private static readonly Logger Logger = new Logger("CreaseProtection");

	// Token: 0x04000355 RID: 853
	private static readonly HashSet<string> CreaseZoneIds = new HashSet<string>
	{
		"Crease"
	};

	// Token: 0x04000356 RID: 854
	private readonly Dictionary<Zone, CreaseProtection.Crease> creases = new Dictionary<Zone, CreaseProtection.Crease>();

	// Token: 0x04000357 RID: 855
	private readonly List<PlayerBody> restoreScratch = new List<PlayerBody>();

	// Token: 0x02000095 RID: 149
	private class Crease
	{
		// Token: 0x04000358 RID: 856
		public PlayerTeam DefendingTeam;

		// Token: 0x04000359 RID: 857
		public PlayerBody Goalie;

		// Token: 0x0400035A RID: 858
		public readonly HashSet<PlayerBody> Enemies = new HashSet<PlayerBody>();

		// Token: 0x0400035B RID: 859
		public PlayerBody PhasedGoalie;

		// Token: 0x0400035C RID: 860
		public readonly HashSet<PlayerBody> Filtered = new HashSet<PlayerBody>();
	}
}
