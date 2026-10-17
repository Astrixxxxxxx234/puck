using System;
using System.Collections.Generic;

// Token: 0x02000092 RID: 146
public class CompetitiveGameModeConfig : StandardGameModeConfig
{
	// Token: 0x17000079 RID: 121
	// (get) Token: 0x060004C0 RID: 1216 RVA: 0x0001A0A6 File Offset: 0x000182A6
	// (set) Token: 0x060004C1 RID: 1217 RVA: 0x0001A0AE File Offset: 0x000182AE
	public Dictionary<PlayerTeam, string[]> teamAssignments { get; set; } = new Dictionary<PlayerTeam, string[]>();
}
