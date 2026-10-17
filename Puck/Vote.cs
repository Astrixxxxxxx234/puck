using System;
using System.Collections.Generic;
using DG.Tweening;

// Token: 0x02000195 RID: 405
public class Vote
{
	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000CAB RID: 3243 RVA: 0x0003C6BB File Offset: 0x0003A8BB
	public int InFavourVotes
	{
		get
		{
			return this.InFavourSteamIds.Count;
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000CAC RID: 3244 RVA: 0x0003C6C8 File Offset: 0x0003A8C8
	public int AgainstVotes
	{
		get
		{
			return this.AgainstSteamIds.Count;
		}
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x0003C6D8 File Offset: 0x0003A8D8
	public Vote(string name, string title, string description, PlayerTeam[] teams, float timeout, string steamId, int requiredVotes, object data = null)
	{
		this.Name = name;
		this.Title = title;
		this.Description = description;
		this.Teams = teams;
		this.Timeout = timeout;
		this.SteamId = steamId;
		this.RequiredVotes = requiredVotes;
		this.Data = data;
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x0003C73E File Offset: 0x0003A93E
	public void Initialize()
	{
		Tween tween = this.timeoutTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.timeoutTween = DOVirtual.DelayedCall(this.Timeout, delegate
		{
			this.End();
		}, true);
		this.CastVote(this.SteamId, true);
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x0003C77D File Offset: 0x0003A97D
	public void Dispose()
	{
		Tween tween = this.timeoutTween;
		if (tween == null)
		{
			return;
		}
		tween.Kill(false);
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x0003C790 File Offset: 0x0003A990
	public void CastVote(string steamId, bool inFavour)
	{
		if (this.InFavourSteamIds.Contains(steamId) || this.AgainstSteamIds.Contains(steamId))
		{
			return;
		}
		if (inFavour)
		{
			this.InFavourSteamIds.Add(steamId);
		}
		else
		{
			this.AgainstSteamIds.Add(steamId);
		}
		Vote.Logger.Info(string.Concat(new string[]
		{
			"Vote '",
			this.Name,
			"': ",
			steamId,
			" voted ",
			inFavour ? "in favour" : "against",
			" ",
			string.Format("({0} for, {1} against, {2} required)", this.InFavourVotes, this.AgainstVotes, this.RequiredVotes)
		}));
		if (this.InFavourVotes >= this.RequiredVotes)
		{
			this.End();
			return;
		}
		if (this.SteamId != steamId)
		{
			Action<Vote, string, bool> progressed = this.Progressed;
			if (progressed == null)
			{
				return;
			}
			progressed(this, steamId, inFavour);
		}
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x0003C890 File Offset: 0x0003AA90
	private void End()
	{
		this.Passed = (this.InFavourVotes >= this.RequiredVotes);
		Vote.Logger.Info(string.Format("Vote '{0}' ended: {1} ({2}/{3})", new object[]
		{
			this.Name,
			this.Passed ? "passed" : "failed",
			this.InFavourVotes,
			this.RequiredVotes
		}));
		Action<Vote> ended = this.Ended;
		if (ended == null)
		{
			return;
		}
		ended(this);
	}

	// Token: 0x040007D9 RID: 2009
	private static readonly Logger Logger = new Logger("Vote");

	// Token: 0x040007DA RID: 2010
	public string Name;

	// Token: 0x040007DB RID: 2011
	public string Title;

	// Token: 0x040007DC RID: 2012
	public string Description;

	// Token: 0x040007DD RID: 2013
	public PlayerTeam[] Teams;

	// Token: 0x040007DE RID: 2014
	public float Timeout;

	// Token: 0x040007DF RID: 2015
	public string SteamId;

	// Token: 0x040007E0 RID: 2016
	public int RequiredVotes;

	// Token: 0x040007E1 RID: 2017
	public object Data;

	// Token: 0x040007E2 RID: 2018
	public List<string> InFavourSteamIds = new List<string>();

	// Token: 0x040007E3 RID: 2019
	public List<string> AgainstSteamIds = new List<string>();

	// Token: 0x040007E4 RID: 2020
	public Action<Vote, string, bool> Progressed;

	// Token: 0x040007E5 RID: 2021
	public Action<Vote> Ended;

	// Token: 0x040007E6 RID: 2022
	public bool Passed;

	// Token: 0x040007E7 RID: 2023
	private Tween timeoutTween;
}
