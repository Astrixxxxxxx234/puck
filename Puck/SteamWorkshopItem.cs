using System;
using System.Collections.Generic;

// Token: 0x0200016F RID: 367
public class SteamWorkshopItem
{
	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06000B3C RID: 2876 RVA: 0x00035BE8 File Offset: 0x00033DE8
	// (set) Token: 0x06000B3D RID: 2877 RVA: 0x00035BF0 File Offset: 0x00033DF0
	public SteamWorkshopItemState State
	{
		get
		{
			return this.state;
		}
		set
		{
			if (this.state.Equals(value))
			{
				return;
			}
			SteamWorkshopItemState oldState = this.state;
			this.state = value;
			this.OnStateChanged(oldState, this.state);
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06000B3E RID: 2878 RVA: 0x00035C27 File Offset: 0x00033E27
	public string Path
	{
		get
		{
			return this.State.Path;
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06000B3F RID: 2879 RVA: 0x00035C34 File Offset: 0x00033E34
	public SteamWorkshopItemDetails Details
	{
		get
		{
			return this.State.Details;
		}
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00035C41 File Offset: 0x00033E41
	public SteamWorkshopItemPhase Phase
	{
		get
		{
			return this.State.Phase;
		}
	}

	// Token: 0x06000B41 RID: 2881 RVA: 0x00035C4E File Offset: 0x00033E4E
	public SteamWorkshopItem(string id, string path = null)
	{
		this.Id = id;
		this.state = new SteamWorkshopItemState
		{
			Path = path
		};
	}

	// Token: 0x06000B42 RID: 2882 RVA: 0x00035C6F File Offset: 0x00033E6F
	public virtual void Initialize()
	{
		EventManager.AddEventListener("Event_OnSteamWorkshopItemDetails", new Action<Dictionary<string, object>>(this.Event_OnSteamWorkshopItemDetails));
		SteamWorkshopManager.GetItemDetails(new string[]
		{
			this.Id
		});
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x00035C9C File Offset: 0x00033E9C
	public virtual void Dispose()
	{
		EventManager.RemoveEventListener("Event_OnSteamWorkshopItemDetails", new Action<Dictionary<string, object>>(this.Event_OnSteamWorkshopItemDetails));
		if (this.State.Details != null)
		{
			this.State.Details.Dispose();
		}
		this.StateChanged = null;
		this.DetailsStateChanged = null;
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x00035CEC File Offset: 0x00033EEC
	public void SetState(Dictionary<string, object> updates)
	{
		SteamWorkshopItemState steamWorkshopItemState = new SteamWorkshopItemState
		{
			Path = (updates.ContainsKey("path") ? ((string)updates["path"]) : this.State.Path),
			Details = (updates.ContainsKey("details") ? ((SteamWorkshopItemDetails)updates["details"]) : this.State.Details),
			Phase = (updates.ContainsKey("phase") ? ((SteamWorkshopItemPhase)updates["phase"]) : this.State.Phase)
		};
		this.State = steamWorkshopItemState;
	}

	// Token: 0x06000B45 RID: 2885 RVA: 0x00035D98 File Offset: 0x00033F98
	private void Event_OnSteamWorkshopItemDetails(Dictionary<string, object> message)
	{
		string b = (string)message["id"];
		string text = (string)message["title"];
		string text2 = (string)message["description"];
		string text3 = (string)message["previewUrl"];
		int num = (int)message["subscriptions"];
		int num2 = (int)message["upvotes"];
		int num3 = (int)message["downvotes"];
		string text4 = (string)message["metadata"];
		if (this.Id != b)
		{
			return;
		}
		if (this.state.Details == null)
		{
			SteamWorkshopItemDetails steamWorkshopItemDetails = new SteamWorkshopItemDetails(text, text2, text3, num, num2, num3, text4);
			this.SetState(new Dictionary<string, object>
			{
				{
					"details",
					steamWorkshopItemDetails
				}
			});
			SteamWorkshopItemDetails steamWorkshopItemDetails2 = steamWorkshopItemDetails;
			steamWorkshopItemDetails2.StateChanged = (Action<SteamWorkshopItemDetailsState, SteamWorkshopItemDetailsState>)Delegate.Combine(steamWorkshopItemDetails2.StateChanged, new Action<SteamWorkshopItemDetailsState, SteamWorkshopItemDetailsState>(this.OnDetailsStateChanged));
			steamWorkshopItemDetails.Initialize();
			return;
		}
		this.Details.SetState(new Dictionary<string, object>
		{
			{
				"title",
				text
			},
			{
				"description",
				text2
			},
			{
				"previewUrl",
				text3
			},
			{
				"subscriptions",
				num
			},
			{
				"upvotes",
				num2
			},
			{
				"downvotes",
				num3
			},
			{
				"metadata",
				text4
			}
		});
	}

	// Token: 0x06000B46 RID: 2886 RVA: 0x00035F19 File Offset: 0x00034119
	private void OnStateChanged(SteamWorkshopItemState oldState, SteamWorkshopItemState newState)
	{
		Action<SteamWorkshopItemState, SteamWorkshopItemState> stateChanged = this.StateChanged;
		if (stateChanged == null)
		{
			return;
		}
		stateChanged(oldState, newState);
	}

	// Token: 0x06000B47 RID: 2887 RVA: 0x00035F2D File Offset: 0x0003412D
	private void OnDetailsStateChanged(SteamWorkshopItemDetailsState oldDetailsState, SteamWorkshopItemDetailsState newDetailsState)
	{
		Action<SteamWorkshopItemDetailsState, SteamWorkshopItemDetailsState> detailsStateChanged = this.DetailsStateChanged;
		if (detailsStateChanged == null)
		{
			return;
		}
		detailsStateChanged(oldDetailsState, newDetailsState);
	}

	// Token: 0x040006BD RID: 1725
	public readonly string Id;

	// Token: 0x040006BE RID: 1726
	private SteamWorkshopItemState state;

	// Token: 0x040006BF RID: 1727
	public Action<SteamWorkshopItemState, SteamWorkshopItemState> StateChanged;

	// Token: 0x040006C0 RID: 1728
	public Action<SteamWorkshopItemDetailsState, SteamWorkshopItemDetailsState> DetailsStateChanged;
}
