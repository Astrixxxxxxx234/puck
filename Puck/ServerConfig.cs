using System;
using System.Linq;
using System.Text.Json.Serialization;

// Token: 0x02000152 RID: 338
public class ServerConfig
{
	// Token: 0x1700010C RID: 268
	// (get) Token: 0x060009D2 RID: 2514 RVA: 0x0002F6F7 File Offset: 0x0002D8F7
	// (set) Token: 0x060009D3 RID: 2515 RVA: 0x0002F6FF File Offset: 0x0002D8FF
	public ushort port { get; set; } = 30609;

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x060009D4 RID: 2516 RVA: 0x0002F708 File Offset: 0x0002D908
	// (set) Token: 0x060009D5 RID: 2517 RVA: 0x0002F710 File Offset: 0x0002D910
	public string name { get; set; } = "MY PUCK SERVER";

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x060009D6 RID: 2518 RVA: 0x0002F719 File Offset: 0x0002D919
	// (set) Token: 0x060009D7 RID: 2519 RVA: 0x0002F721 File Offset: 0x0002D921
	public int maxPlayers { get; set; } = 12;

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0002F72A File Offset: 0x0002D92A
	// (set) Token: 0x060009D9 RID: 2521 RVA: 0x0002F732 File Offset: 0x0002D932
	public string password { get; set; }

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x060009DA RID: 2522 RVA: 0x0002F73B File Offset: 0x0002D93B
	// (set) Token: 0x060009DB RID: 2523 RVA: 0x0002F743 File Offset: 0x0002D943
	public int tickRate { get; set; } = 200;

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x060009DC RID: 2524 RVA: 0x0002F74C File Offset: 0x0002D94C
	// (set) Token: 0x060009DD RID: 2525 RVA: 0x0002F754 File Offset: 0x0002D954
	public bool isPublic { get; set; } = true;

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x060009DE RID: 2526 RVA: 0x0002F75D File Offset: 0x0002D95D
	// (set) Token: 0x060009DF RID: 2527 RVA: 0x0002F765 File Offset: 0x0002D965
	public bool useVoip { get; set; }

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x060009E0 RID: 2528 RVA: 0x0002F76E File Offset: 0x0002D96E
	// (set) Token: 0x060009E1 RID: 2529 RVA: 0x0002F776 File Offset: 0x0002D976
	public bool useWhitelist { get; set; }

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x060009E2 RID: 2530 RVA: 0x0002F77F File Offset: 0x0002D97F
	// (set) Token: 0x060009E3 RID: 2531 RVA: 0x0002F787 File Offset: 0x0002D987
	public ModConfig[] mods { get; set; } = Constants.DEFAULT_SERVER_MODS;

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x060009E4 RID: 2532 RVA: 0x0002F790 File Offset: 0x0002D990
	// (set) Token: 0x060009E5 RID: 2533 RVA: 0x0002F798 File Offset: 0x0002D998
	public string gameMode { get; set; } = "public";

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0002F7A1 File Offset: 0x0002D9A1
	// (set) Token: 0x060009E7 RID: 2535 RVA: 0x0002F7A9 File Offset: 0x0002D9A9
	public string level { get; set; } = "default";

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0002F7B4 File Offset: 0x0002D9B4
	[JsonIgnore]
	public string[] EnabledModIds
	{
		get
		{
			return (from mod in this.mods
			where mod.isEnabled
			select mod.id).ToArray<string>();
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0002F814 File Offset: 0x0002DA14
	[JsonIgnore]
	public string[] ClientRequiredModIds
	{
		get
		{
			return (from mod in this.mods
			where mod.isClientRequired
			select mod.id).ToArray<string>();
		}
	}
}
