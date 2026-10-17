using System;

// Token: 0x0200016E RID: 366
public class SteamWorkshopItemState
{
	// Token: 0x06000B38 RID: 2872 RVA: 0x00035B57 File Offset: 0x00033D57
	public bool Equals(SteamWorkshopItemState other)
	{
		return this.Path == other.Path && this.Details == other.Details && this.Phase == other.Phase;
	}

	// Token: 0x06000B39 RID: 2873 RVA: 0x00035B8C File Offset: 0x00033D8C
	public override bool Equals(object obj)
	{
		SteamWorkshopItemState steamWorkshopItemState = obj as SteamWorkshopItemState;
		return steamWorkshopItemState != null && this.Equals(steamWorkshopItemState);
	}

	// Token: 0x06000B3A RID: 2874 RVA: 0x00035BAC File Offset: 0x00033DAC
	public override int GetHashCode()
	{
		return HashCode.Combine<string, SteamWorkshopItemDetails, SteamWorkshopItemPhase>(this.Path, this.Details, this.Phase);
	}

	// Token: 0x06000B3B RID: 2875 RVA: 0x00035BC5 File Offset: 0x00033DC5
	public override string ToString()
	{
		return string.Format("Path={0}, Details={1}, Phase={2}", this.Path, this.Details, this.Phase);
	}

	// Token: 0x040006BA RID: 1722
	public string Path;

	// Token: 0x040006BB RID: 1723
	public SteamWorkshopItemDetails Details;

	// Token: 0x040006BC RID: 1724
	public SteamWorkshopItemPhase Phase;
}
