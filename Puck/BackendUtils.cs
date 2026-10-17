using System;
using System.Linq;

// Token: 0x0200021E RID: 542
public static class BackendUtils
{
	// Token: 0x06001150 RID: 4432 RVA: 0x00052273 File Offset: 0x00050473
	public static PlayerBan GetActivePlayerDataBan(PlayerData playerData)
	{
		if (playerData == null)
		{
			return null;
		}
		return playerData.bans.FirstOrDefault((PlayerBan ban) => Utils.GetTimestamp() <= ban.expiresAt);
	}

	// Token: 0x06001151 RID: 4433 RVA: 0x000522A4 File Offset: 0x000504A4
	public static PlayerMute GetActivePlayerDataMute(PlayerData playerData)
	{
		if (playerData == null)
		{
			return null;
		}
		return playerData.mutes.FirstOrDefault((PlayerMute mute) => Utils.GetTimestamp() <= mute.expiresAt);
	}

	// Token: 0x06001152 RID: 4434 RVA: 0x000522D5 File Offset: 0x000504D5
	public static PlayerCooldown GetActivePlayerDataCooldown(PlayerData playerData)
	{
		if (playerData == null)
		{
			return null;
		}
		return playerData.cooldowns.FirstOrDefault((PlayerCooldown cooldown) => Utils.GetTimestamp() <= cooldown.expiresAt);
	}

	// Token: 0x06001153 RID: 4435 RVA: 0x00052308 File Offset: 0x00050508
	public static bool IsConnectedToMatchEndPoint()
	{
		Connection connection = GlobalStateManager.ConnectionState.Connection;
		EndPoint a = (connection != null) ? connection.EndPoint : null;
		PlayerMatchData matchData = BackendManager.PlayerState.MatchData;
		EndPoint b = (matchData != null) ? matchData.endPoint : null;
		return a != null && a == b;
	}
}
