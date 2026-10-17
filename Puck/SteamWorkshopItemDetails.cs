using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class SteamWorkshopItemDetails
{
	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000B4D RID: 2893 RVA: 0x000360A8 File Offset: 0x000342A8
	// (set) Token: 0x06000B4E RID: 2894 RVA: 0x000360B0 File Offset: 0x000342B0
	public SteamWorkshopItemDetailsState State
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
			SteamWorkshopItemDetailsState oldState = this.state;
			this.state = value;
			this.OnStateChanged(oldState, this.state);
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000B4F RID: 2895 RVA: 0x000360E7 File Offset: 0x000342E7
	public string Title
	{
		get
		{
			return this.State.Title;
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000B50 RID: 2896 RVA: 0x000360F4 File Offset: 0x000342F4
	public string Description
	{
		get
		{
			return this.State.Description;
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000B51 RID: 2897 RVA: 0x00036101 File Offset: 0x00034301
	public string PreviewUrl
	{
		get
		{
			return this.State.PreviewUrl;
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000B52 RID: 2898 RVA: 0x0003610E File Offset: 0x0003430E
	public int Subscriptions
	{
		get
		{
			return this.State.Subscriptions;
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06000B53 RID: 2899 RVA: 0x0003611B File Offset: 0x0003431B
	public int Upvotes
	{
		get
		{
			return this.State.Upvotes;
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00036128 File Offset: 0x00034328
	public int Downvotes
	{
		get
		{
			return this.State.Downvotes;
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000B55 RID: 2901 RVA: 0x00036135 File Offset: 0x00034335
	public string Metadata
	{
		get
		{
			return this.State.Metadata;
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000B56 RID: 2902 RVA: 0x00036142 File Offset: 0x00034342
	public Texture2D PreviewTexture
	{
		get
		{
			return this.State.PreviewTexture;
		}
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x00036150 File Offset: 0x00034350
	public SteamWorkshopItemDetails(string title, string description, string previewUrl, int subscriptions, int upvotes, int downvotes, string metadata)
	{
		this.state = new SteamWorkshopItemDetailsState
		{
			Title = title,
			Description = description,
			PreviewUrl = previewUrl,
			Subscriptions = subscriptions,
			Upvotes = upvotes,
			Downvotes = downvotes,
			Metadata = metadata
		};
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x000361A3 File Offset: 0x000343A3
	public void Initialize()
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		MonoBehaviourSingleton<ThreadManager>.Instance.Enqueue(this.DownloadPreviewTexture());
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x000361BD File Offset: 0x000343BD
	public void Dispose()
	{
		this.StateChanged = null;
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x000361C8 File Offset: 0x000343C8
	public void SetState(Dictionary<string, object> updates)
	{
		SteamWorkshopItemDetailsState steamWorkshopItemDetailsState = new SteamWorkshopItemDetailsState
		{
			Title = (updates.ContainsKey("title") ? ((string)updates["title"]) : this.State.Title),
			Description = (updates.ContainsKey("description") ? ((string)updates["description"]) : this.State.Description),
			PreviewUrl = (updates.ContainsKey("previewUrl") ? ((string)updates["previewUrl"]) : this.State.PreviewUrl),
			Subscriptions = (updates.ContainsKey("subscriptions") ? ((int)updates["subscriptions"]) : this.State.Subscriptions),
			Upvotes = (updates.ContainsKey("upvotes") ? ((int)updates["upvotes"]) : this.State.Upvotes),
			Downvotes = (updates.ContainsKey("downvotes") ? ((int)updates["downvotes"]) : this.State.Downvotes),
			Metadata = (updates.ContainsKey("metadata") ? ((string)updates["metadata"]) : this.State.Metadata),
			PreviewTexture = (updates.ContainsKey("previewTexture") ? ((Texture2D)updates["previewTexture"]) : this.State.PreviewTexture)
		};
		this.State = steamWorkshopItemDetailsState;
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x00036362 File Offset: 0x00034562
	private IEnumerator DownloadPreviewTexture()
	{
		SteamWorkshopItemDetails.<DownloadPreviewTexture>d__26 <DownloadPreviewTexture>d__ = new SteamWorkshopItemDetails.<DownloadPreviewTexture>d__26(0);
		<DownloadPreviewTexture>d__.<>4__this = this;
		return <DownloadPreviewTexture>d__;
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x00036371 File Offset: 0x00034571
	private void OnStateChanged(SteamWorkshopItemDetailsState oldState, SteamWorkshopItemDetailsState newState)
	{
		Action<SteamWorkshopItemDetailsState, SteamWorkshopItemDetailsState> stateChanged = this.StateChanged;
		if (stateChanged == null)
		{
			return;
		}
		stateChanged(oldState, newState);
	}

	// Token: 0x040006C9 RID: 1737
	private static readonly global::Logger Logger = new global::Logger("SteamWorkshopItemDetails");

	// Token: 0x040006CA RID: 1738
	private SteamWorkshopItemDetailsState state;

	// Token: 0x040006CB RID: 1739
	public Action<SteamWorkshopItemDetailsState, SteamWorkshopItemDetailsState> StateChanged;
}
