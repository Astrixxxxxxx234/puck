using System;

// Token: 0x020000CB RID: 203
internal interface IGameMode
{
	// Token: 0x1700009D RID: 157
	// (get) Token: 0x06000679 RID: 1657
	// (set) Token: 0x0600067A RID: 1658
	bool IsInitialized { get; set; }

	// Token: 0x0600067B RID: 1659
	bool Initialize(Level level, ServerManager serverManager, GameManager gameManager, PlayerManager playerManager, PuckManager puckManager, ChatManager chatManager, ReplayManager replayManager, VoteManager voteManager);

	// Token: 0x0600067C RID: 1660
	bool Dispose();
}
