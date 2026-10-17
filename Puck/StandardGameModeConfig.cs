using System;
using System.Collections.Generic;

// Token: 0x0200009A RID: 154
public class StandardGameModeConfig : BaseGameModeConfig
{
	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000502 RID: 1282 RVA: 0x0001B3A9 File Offset: 0x000195A9
	// (set) Token: 0x06000503 RID: 1283 RVA: 0x0001B3B1 File Offset: 0x000195B1
	public Dictionary<GamePhase, int> phaseDurationMap { get; set; } = new Dictionary<GamePhase, int>
	{
		{
			GamePhase.None,
			0
		},
		{
			GamePhase.Warmup,
			60
		},
		{
			GamePhase.PreGame,
			10
		},
		{
			GamePhase.FaceOff,
			4
		},
		{
			GamePhase.Play,
			300
		},
		{
			GamePhase.BlueScore,
			4
		},
		{
			GamePhase.RedScore,
			4
		},
		{
			GamePhase.Replay,
			9
		},
		{
			GamePhase.Intermission,
			10
		},
		{
			GamePhase.GameOver,
			30
		},
		{
			GamePhase.PostGame,
			10
		}
	};

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000504 RID: 1284 RVA: 0x0001B3BA File Offset: 0x000195BA
	// (set) Token: 0x06000505 RID: 1285 RVA: 0x0001B3C2 File Offset: 0x000195C2
	public float spawnDelay { get; set; } = 10f;

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06000506 RID: 1286 RVA: 0x0001B3CB File Offset: 0x000195CB
	// (set) Token: 0x06000507 RID: 1287 RVA: 0x0001B3D3 File Offset: 0x000195D3
	public int maxPeriods { get; set; } = 3;

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000508 RID: 1288 RVA: 0x0001B3DC File Offset: 0x000195DC
	// (set) Token: 0x06000509 RID: 1289 RVA: 0x0001B3E4 File Offset: 0x000195E4
	public bool goalSlowMotion { get; set; } = true;

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x0600050A RID: 1290 RVA: 0x0001B3ED File Offset: 0x000195ED
	// (set) Token: 0x0600050B RID: 1291 RVA: 0x0001B3F5 File Offset: 0x000195F5
	public bool goalieCreaseProtection { get; set; } = true;
}
