using System;
using System.Collections.Generic;

// Token: 0x0200009B RID: 155
public class GameResult
{
	// Token: 0x17000084 RID: 132
	// (get) Token: 0x0600050D RID: 1293 RVA: 0x0001B4A2 File Offset: 0x000196A2
	// (set) Token: 0x0600050E RID: 1294 RVA: 0x0001B4AA File Offset: 0x000196AA
	public PlayerTeam winningTeam { get; set; }

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x0600050F RID: 1295 RVA: 0x0001B4B3 File Offset: 0x000196B3
	// (set) Token: 0x06000510 RID: 1296 RVA: 0x0001B4BB File Offset: 0x000196BB
	public int blueScore { get; set; }

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06000511 RID: 1297 RVA: 0x0001B4C4 File Offset: 0x000196C4
	// (set) Token: 0x06000512 RID: 1298 RVA: 0x0001B4CC File Offset: 0x000196CC
	public int redScore { get; set; }

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06000513 RID: 1299 RVA: 0x0001B4D5 File Offset: 0x000196D5
	// (set) Token: 0x06000514 RID: 1300 RVA: 0x0001B4DD File Offset: 0x000196DD
	public bool forfeit { get; set; }

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06000515 RID: 1301 RVA: 0x0001B4E6 File Offset: 0x000196E6
	// (set) Token: 0x06000516 RID: 1302 RVA: 0x0001B4EE File Offset: 0x000196EE
	public Dictionary<string, PlayerResult> playerResults { get; set; } = new Dictionary<string, PlayerResult>();
}
