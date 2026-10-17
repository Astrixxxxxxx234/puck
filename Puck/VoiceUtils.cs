using System;
using Steamworks;
using UnityEngine;

// Token: 0x0200024C RID: 588
public static class VoiceUtils
{
	// Token: 0x060012A2 RID: 4770 RVA: 0x000566D8 File Offset: 0x000548D8
	public static float ComputeLevel(byte[] voiceData, uint bytesRead, uint sampleRate, byte[] decompressBuffer)
	{
		if (sampleRate == 0U || decompressBuffer == null)
		{
			return 0f;
		}
		uint num;
		SteamUser.DecompressVoice(voiceData, bytesRead, decompressBuffer, (uint)decompressBuffer.Length, out num, sampleRate);
		double num2 = 0.0;
		int num3 = 0;
		int num4 = 0;
		while ((long)(num4 + 1) < (long)((ulong)num))
		{
			float num5 = (float)((short)((int)decompressBuffer[num4] | (int)decompressBuffer[num4 + 1] << 8)) / 32768f;
			num2 += (double)num5 * (double)num5;
			num3++;
			num4 += 2;
		}
		if (num3 == 0)
		{
			return 0f;
		}
		return Mathf.Clamp01(Mathf.Sqrt((float)(num2 / (double)num3)) * 14f);
	}
}
