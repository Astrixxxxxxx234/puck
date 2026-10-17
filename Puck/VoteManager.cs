using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

// Token: 0x02000196 RID: 406
public class VoteManager : MonoBehaviourSingleton<VoteManager>
{
	// Token: 0x06000CB4 RID: 3252 RVA: 0x0003C934 File Offset: 0x0003AB34
	public void Server_AddVote(string name, string title, string description, PlayerTeam[] teams, float timeout, string steamId, int requiredVotes, object data = null)
	{
		Vote vote = new Vote(name, title, description, teams, timeout, steamId, requiredVotes, data);
		this.Votes.Add(vote);
		VoteManager.Logger.Info(string.Format("Vote '{0}' started by {1} (teams: {2}, required: {3})", new object[]
		{
			name,
			steamId,
			string.Join<PlayerTeam>(", ", teams),
			requiredVotes
		}));
		EventManager.TriggerEvent("Event_Server_OnVoteAdded", new Dictionary<string, object>
		{
			{
				"vote",
				vote
			},
			{
				"teams",
				teams
			}
		});
		Vote vote2 = vote;
		vote2.Progressed = (Action<Vote, string, bool>)Delegate.Combine(vote2.Progressed, new Action<Vote, string, bool>(this.Server_OnVoteProgressed));
		Vote vote3 = vote;
		vote3.Ended = (Action<Vote>)Delegate.Combine(vote3.Ended, new Action<Vote>(this.Server_OnVoteEnded));
		vote.Initialize();
	}

	// Token: 0x06000CB5 RID: 3253 RVA: 0x0003CA10 File Offset: 0x0003AC10
	public void Server_RemoveVote(Vote vote)
	{
		if (!this.Votes.Contains(vote))
		{
			return;
		}
		this.Votes.Remove(vote);
		EventManager.TriggerEvent("Event_Server_OnVoteRemoved", new Dictionary<string, object>
		{
			{
				"vote",
				vote
			}
		});
		vote.Progressed = (Action<Vote, string, bool>)Delegate.Remove(vote.Progressed, new Action<Vote, string, bool>(this.Server_OnVoteProgressed));
		vote.Ended = (Action<Vote>)Delegate.Remove(vote.Ended, new Action<Vote>(this.Server_OnVoteEnded));
		vote.Dispose();
	}

	// Token: 0x06000CB6 RID: 3254 RVA: 0x0003CAA0 File Offset: 0x0003ACA0
	public Vote[] Server_GetVotesByName(string name)
	{
		return (from v in this.Votes
		where v.Name == name
		select v).ToArray<Vote>();
	}

	// Token: 0x06000CB7 RID: 3255 RVA: 0x0003CAD8 File Offset: 0x0003ACD8
	public Vote Server_GetVoteByName(string name)
	{
		return this.Votes.Find((Vote v) => v.Name == name);
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x0003CB0C File Offset: 0x0003AD0C
	public Vote[] Server_GetTeamVotesByName(string name, PlayerTeam team)
	{
		return (from v in this.Votes
		where v.Name == name && v.Teams.Contains(team)
		select v).ToArray<Vote>();
	}

	// Token: 0x06000CB9 RID: 3257 RVA: 0x0003CB4C File Offset: 0x0003AD4C
	public Vote Server_GetTeamVoteByName(string name, PlayerTeam team)
	{
		return this.Votes.Find((Vote v) => v.Name == name && v.Teams.Contains(team));
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x0003CB84 File Offset: 0x0003AD84
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_NotifyVoteStartedRpc(string name, string description, string steamId, int inFavourVotes, int againstVotes, int requiredVotes, object data)
	{
		EventManager.TriggerEvent("Event_OnVoteStarted", new Dictionary<string, object>
		{
			{
				"name",
				name
			},
			{
				"description",
				description
			},
			{
				"steamId",
				steamId
			},
			{
				"inFavourVotes",
				inFavourVotes
			},
			{
				"againstVotes",
				againstVotes
			},
			{
				"requiredVotes",
				requiredVotes
			},
			{
				"data",
				data
			}
		});
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x0003CC07 File Offset: 0x0003AE07
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_NotifyVoteProgressedRpc(string name, int inFavourVotes, int againstVotes)
	{
		EventManager.TriggerEvent("Event_OnVoteProgressed", new Dictionary<string, object>
		{
			{
				"name",
				name
			},
			{
				"inFavourVotes",
				inFavourVotes
			},
			{
				"againstVotes",
				againstVotes
			}
		});
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x0003CC46 File Offset: 0x0003AE46
	[Rpc(SendTo.SpecifiedInParams, InvokePermission = RpcInvokePermission.Server, DeferLocal = true)]
	public void Server_NotifyVoteEndedRpc(string name, bool succeeded)
	{
		EventManager.TriggerEvent("Event_OnVoteEnded", new Dictionary<string, object>
		{
			{
				"name",
				name
			},
			{
				"succeeded",
				succeeded
			}
		});
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x0003CC74 File Offset: 0x0003AE74
	private void Server_OnVoteProgressed(Vote vote, string steamId, bool inFavour)
	{
		EventManager.TriggerEvent("Event_Server_OnVoteProgressed", new Dictionary<string, object>
		{
			{
				"vote",
				vote
			},
			{
				"steamId",
				steamId
			},
			{
				"inFavour",
				inFavour
			}
		});
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x0003CCAE File Offset: 0x0003AEAE
	private void Server_OnVoteEnded(Vote vote)
	{
		this.Server_RemoveVote(vote);
	}

	// Token: 0x040007E8 RID: 2024
	private static readonly Logger Logger = new Logger("VoteManager");

	// Token: 0x040007E9 RID: 2025
	public List<Vote> Votes = new List<Vote>();
}
