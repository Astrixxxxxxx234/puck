using System;
using System.Collections.Generic;
using DG.Tweening;

// Token: 0x02000158 RID: 344
public class TimeoutManager : MonoBehaviourSingleton<TimeoutManager>
{
	// Token: 0x06000A2F RID: 2607 RVA: 0x00030D68 File Offset: 0x0002EF68
	public void Dispose()
	{
		foreach (Tween t in this.steamIdTweenMap.Values)
		{
			t.Kill(false);
		}
		this.steamIdTweenMap.Clear();
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x00030DCC File Offset: 0x0002EFCC
	public void AddSteamIdTimeout(string steamId, float timeout)
	{
		if (this.steamIdTweenMap.ContainsKey(steamId))
		{
			return;
		}
		this.steamIdTweenMap[steamId] = DOVirtual.DelayedCall(timeout, delegate
		{
			this.RemoveSteamIdTimeout(steamId);
		}, true);
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x00030E25 File Offset: 0x0002F025
	public void RemoveSteamIdTimeout(string steamId)
	{
		if (!this.steamIdTweenMap.ContainsKey(steamId))
		{
			return;
		}
		this.steamIdTweenMap[steamId].Kill(false);
		this.steamIdTweenMap.Remove(steamId);
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x00030E55 File Offset: 0x0002F055
	public bool IsSteamIdTimedOut(string steamId)
	{
		return this.steamIdTweenMap.ContainsKey(steamId);
	}

	// Token: 0x0400062D RID: 1581
	private Dictionary<string, Tween> steamIdTweenMap = new Dictionary<string, Tween>();
}
