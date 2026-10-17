using System;
using DG.Tweening;
using UnityEngine;

// Token: 0x02000141 RID: 321
public static class SaveManager
{
	// Token: 0x06000939 RID: 2361 RVA: 0x000023BA File Offset: 0x000005BA
	public static void Initialize()
	{
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0002D8DA File Offset: 0x0002BADA
	public static void Dispose()
	{
		Tween tween = SaveManager.saveDebounceTween;
		if (tween == null)
		{
			return;
		}
		tween.Kill(false);
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x0002D8EC File Offset: 0x0002BAEC
	public static void SetBool(string key, bool value)
	{
		PlayerPrefs.SetInt(key, value ? 1 : 0);
		SaveManager.Save();
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x0002D900 File Offset: 0x0002BB00
	public static bool GetBool(string key, bool defaultValue)
	{
		int defaultValue2 = defaultValue ? 1 : 0;
		return PlayerPrefs.GetInt(key, defaultValue2) == 1;
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x0002D91F File Offset: 0x0002BB1F
	public static void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
		SaveManager.Save();
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0002D92D File Offset: 0x0002BB2D
	public static int GetInt(string key, int defaultValue)
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x0002D936 File Offset: 0x0002BB36
	public static void SetFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
		SaveManager.Save();
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0002D944 File Offset: 0x0002BB44
	public static float GetFloat(string key, float defaultValue)
	{
		return PlayerPrefs.GetFloat(key, defaultValue);
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0002D94D File Offset: 0x0002BB4D
	public static void SetString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
		SaveManager.Save();
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0002D95B File Offset: 0x0002BB5B
	public static string GetString(string key, string defaultValue)
	{
		return PlayerPrefs.GetString(key, defaultValue);
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0002D964 File Offset: 0x0002BB64
	public static void SetEnum<T>(string key, T value) where T : Enum
	{
		PlayerPrefs.SetInt(key, Convert.ToInt32(value));
		SaveManager.Save();
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x0002D97C File Offset: 0x0002BB7C
	public static T GetEnum<T>(string key, T defaultValue) where T : Enum
	{
		int defaultValue2 = Convert.ToInt32(defaultValue);
		int @int = PlayerPrefs.GetInt(key, defaultValue2);
		return (T)((object)Enum.ToObject(typeof(T), @int));
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x0002D9B4 File Offset: 0x0002BBB4
	private static void Save()
	{
		Tween tween = SaveManager.saveDebounceTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		SaveManager.saveDebounceTween = DOVirtual.DelayedCall(0f, delegate
		{
			PlayerPrefs.Save();
		}, true);
	}

	// Token: 0x040005D1 RID: 1489
	private static Tween saveDebounceTween;
}
