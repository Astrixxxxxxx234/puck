using System;
using Unity.Collections;

// Token: 0x0200012B RID: 299
public struct ReplayPlayerSpawned
{
	// Token: 0x04000578 RID: 1400
	public ulong OwnerClientId;

	// Token: 0x04000579 RID: 1401
	public PlayerGameState GameState;

	// Token: 0x0400057A RID: 1402
	public PlayerCustomizationState CustomizationState;

	// Token: 0x0400057B RID: 1403
	public PlayerHandedness Handedness;

	// Token: 0x0400057C RID: 1404
	public FixedString32Bytes SteamId;

	// Token: 0x0400057D RID: 1405
	public FixedString32Bytes Username;

	// Token: 0x0400057E RID: 1406
	public int Number;

	// Token: 0x0400057F RID: 1407
	public int PatreonLevel;

	// Token: 0x04000580 RID: 1408
	public int AdminLevel;

	// Token: 0x04000581 RID: 1409
	public bool IsMuted;
}
