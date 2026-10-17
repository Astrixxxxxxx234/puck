using System;
using System.Linq;
using System.Text.Json.Serialization;

// Token: 0x020000E4 RID: 228
public class Item
{
	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x06000707 RID: 1799 RVA: 0x000268DB File Offset: 0x00024ADB
	// (set) Token: 0x06000708 RID: 1800 RVA: 0x000268E3 File Offset: 0x00024AE3
	public int id { get; set; }

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x06000709 RID: 1801 RVA: 0x000268EC File Offset: 0x00024AEC
	// (set) Token: 0x0600070A RID: 1802 RVA: 0x000268F4 File Offset: 0x00024AF4
	public string name { get; set; }

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x0600070B RID: 1803 RVA: 0x000268FD File Offset: 0x00024AFD
	// (set) Token: 0x0600070C RID: 1804 RVA: 0x00026905 File Offset: 0x00024B05
	public string description { get; set; }

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x0600070D RID: 1805 RVA: 0x0002690E File Offset: 0x00024B0E
	// (set) Token: 0x0600070E RID: 1806 RVA: 0x00026916 File Offset: 0x00024B16
	public string[] categories { get; set; } = new string[0];

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x0600070F RID: 1807 RVA: 0x0002691F File Offset: 0x00024B1F
	// (set) Token: 0x06000710 RID: 1808 RVA: 0x00026927 File Offset: 0x00024B27
	public int price { get; set; }

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000711 RID: 1809 RVA: 0x00026930 File Offset: 0x00024B30
	// (set) Token: 0x06000712 RID: 1810 RVA: 0x00026938 File Offset: 0x00024B38
	public string code { get; set; }

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000713 RID: 1811 RVA: 0x00026941 File Offset: 0x00024B41
	[JsonIgnore]
	public bool IsFlag
	{
		get
		{
			return this.categories.Contains("flag");
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06000714 RID: 1812 RVA: 0x00026953 File Offset: 0x00024B53
	[JsonIgnore]
	public bool IsHeadgear
	{
		get
		{
			return this.categories.Contains("headgear");
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x06000715 RID: 1813 RVA: 0x00026965 File Offset: 0x00024B65
	[JsonIgnore]
	public bool IsMustache
	{
		get
		{
			return this.categories.Contains("mustache");
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06000716 RID: 1814 RVA: 0x00026977 File Offset: 0x00024B77
	[JsonIgnore]
	public bool IsBeard
	{
		get
		{
			return this.categories.Contains("beard");
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06000717 RID: 1815 RVA: 0x00026989 File Offset: 0x00024B89
	[JsonIgnore]
	public bool IsJersey
	{
		get
		{
			return this.categories.Contains("jersey");
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000718 RID: 1816 RVA: 0x0002699B File Offset: 0x00024B9B
	[JsonIgnore]
	public bool IsStickSkin
	{
		get
		{
			return this.categories.Contains("stickSkin");
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000719 RID: 1817 RVA: 0x000269AD File Offset: 0x00024BAD
	[JsonIgnore]
	public bool IsStickShaftTape
	{
		get
		{
			return this.categories.Contains("stickShaftTape");
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x0600071A RID: 1818 RVA: 0x000269BF File Offset: 0x00024BBF
	[JsonIgnore]
	public bool IsStickBladeTape
	{
		get
		{
			return this.categories.Contains("stickBladeTape");
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x0600071B RID: 1819 RVA: 0x000269D1 File Offset: 0x00024BD1
	[JsonIgnore]
	public bool HasRolePostfix
	{
		get
		{
			return this.categories.Contains("attacker") || this.categories.Contains("goalie");
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x0600071C RID: 1820 RVA: 0x000269F7 File Offset: 0x00024BF7
	[JsonIgnore]
	public bool IsAttackerItem
	{
		get
		{
			return !this.HasRolePostfix || this.categories.Contains("attacker");
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x0600071D RID: 1821 RVA: 0x00026A13 File Offset: 0x00024C13
	[JsonIgnore]
	public bool IsGoalieItem
	{
		get
		{
			return !this.HasRolePostfix || this.categories.Contains("goalie");
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x0600071E RID: 1822 RVA: 0x00026A2F File Offset: 0x00024C2F
	[JsonIgnore]
	public bool IsPurchased
	{
		get
		{
			return BackendManager.PlayerState.PlayerData != null && BackendManager.PlayerState.PlayerData.items.Any((PlayerItem item) => item.itemId == this.id);
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x0600071F RID: 1823 RVA: 0x00026A5F File Offset: 0x00024C5F
	[JsonIgnore]
	public bool IsOwned
	{
		get
		{
			return this.price == 0 || this.IsPurchased;
		}
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06000720 RID: 1824 RVA: 0x00026A71 File Offset: 0x00024C71
	[JsonIgnore]
	public bool IsUnlisted
	{
		get
		{
			return this.categories.Contains("unlisted");
		}
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06000721 RID: 1825 RVA: 0x00026A83 File Offset: 0x00024C83
	[JsonIgnore]
	public string EditorDisplayName
	{
		get
		{
			return string.Format("{0} ({1})", this.name, this.id);
		}
	}
}
