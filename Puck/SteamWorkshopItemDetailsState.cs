using System;
using UnityEngine;

// Token: 0x02000170 RID: 368
public class SteamWorkshopItemDetailsState
{
	// Token: 0x06000B49 RID: 2889 RVA: 0x00035F44 File Offset: 0x00034144
	public bool Equals(SteamWorkshopItemDetailsState other)
	{
		return this.Title == other.Title && this.Description == other.Description && this.PreviewUrl == other.PreviewUrl && this.Subscriptions == other.Subscriptions && this.Upvotes == other.Upvotes && this.Downvotes == other.Downvotes && this.Metadata == other.Metadata && this.PreviewTexture == other.PreviewTexture;
	}

	// Token: 0x06000B4A RID: 2890 RVA: 0x00035FDC File Offset: 0x000341DC
	public override bool Equals(object obj)
	{
		SteamWorkshopItemDetailsState steamWorkshopItemDetailsState = obj as SteamWorkshopItemDetailsState;
		return steamWorkshopItemDetailsState != null && this.Equals(steamWorkshopItemDetailsState);
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x00035FFC File Offset: 0x000341FC
	public override int GetHashCode()
	{
		return HashCode.Combine<string, string, string, int, int, int, string, Texture2D>(this.Title, this.Description, this.PreviewUrl, this.Subscriptions, this.Upvotes, this.Downvotes, this.Metadata, this.PreviewTexture);
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x00036034 File Offset: 0x00034234
	public override string ToString()
	{
		return string.Format("Title={0}, Description={1}, PreviewUrl={2}, Subscriptions={3}, Upvotes={4}, Downvotes={5}, Metadata={6}, PreviewTexture={7}", new object[]
		{
			this.Title,
			this.Description,
			this.PreviewUrl,
			this.Subscriptions,
			this.Upvotes,
			this.Downvotes,
			this.Metadata,
			this.PreviewTexture
		});
	}

	// Token: 0x040006C1 RID: 1729
	public string Title;

	// Token: 0x040006C2 RID: 1730
	public string Description;

	// Token: 0x040006C3 RID: 1731
	public string PreviewUrl;

	// Token: 0x040006C4 RID: 1732
	public int Subscriptions;

	// Token: 0x040006C5 RID: 1733
	public int Upvotes;

	// Token: 0x040006C6 RID: 1734
	public int Downvotes;

	// Token: 0x040006C7 RID: 1735
	public string Metadata;

	// Token: 0x040006C8 RID: 1736
	public Texture2D PreviewTexture;
}
