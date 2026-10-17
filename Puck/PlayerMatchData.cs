using System;
using System.Text.Json.Serialization;

// Token: 0x02000279 RID: 633
public class PlayerMatchData : MatchData
{
	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x06001335 RID: 4917 RVA: 0x00056C28 File Offset: 0x00054E28
	[JsonIgnore]
	public int? JoinTimeoutRemainingSeconds
	{
		get
		{
			if (base.startedAt == null)
			{
				return null;
			}
			DateTime utcNow = DateTime.UtcNow;
			DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds((long)base.startedAt.Value).DateTime;
			int num = (int)(60.0 - utcNow.Subtract(dateTime).TotalSeconds);
			if (num <= 0)
			{
				return null;
			}
			return new int?(num);
		}
	}
}
