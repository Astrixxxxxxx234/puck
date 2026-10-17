using System;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000230 RID: 560
public static class NetworkingUtils
{
	// Token: 0x060011AD RID: 4525 RVA: 0x00053200 File Offset: 0x00051400
	public static Player GetPlayerFromNetworkObjectReference(NetworkObjectReference reference)
	{
		NetworkObject networkObject;
		if (reference.TryGet(out networkObject, null))
		{
			return networkObject.GetComponent<Player>();
		}
		return null;
	}

	// Token: 0x060011AE RID: 4526 RVA: 0x00053224 File Offset: 0x00051424
	public static PlayerPosition GetPlayerPositionFromNetworkObjectReference(NetworkObjectReference reference)
	{
		NetworkObject networkObject;
		if (reference.TryGet(out networkObject, null))
		{
			return networkObject.GetComponent<PlayerPosition>();
		}
		return null;
	}

	// Token: 0x060011AF RID: 4527 RVA: 0x00053248 File Offset: 0x00051448
	public static Puck GetPuckFromNetworkObjectReference(NetworkObjectReference reference)
	{
		NetworkObject networkObject;
		if (reference.TryGet(out networkObject, null))
		{
			return networkObject.GetComponent<Puck>();
		}
		return null;
	}

	// Token: 0x060011B0 RID: 4528 RVA: 0x0005326C File Offset: 0x0005146C
	public static byte CompressFloatToByte(float value, float minValue, float maxValue)
	{
		int num = -128;
		int num2 = 127;
		float t = Mathf.InverseLerp(minValue, maxValue, value);
		return (byte)((sbyte)Mathf.Clamp(Mathf.RoundToInt(Mathf.Lerp((float)num, (float)num2, t)), num, num2));
	}

	// Token: 0x060011B1 RID: 4529 RVA: 0x000532A0 File Offset: 0x000514A0
	public static short CompressFloatToShort(float value, float minValue, float maxValue)
	{
		int num = -32768;
		int num2 = 32767;
		float t = Mathf.InverseLerp(minValue, maxValue, value);
		return (short)Mathf.Clamp(Mathf.RoundToInt(Mathf.Lerp((float)num, (float)num2, t)), num, num2);
	}

	// Token: 0x060011B2 RID: 4530 RVA: 0x000532DC File Offset: 0x000514DC
	public static float DecompressByteToFloat(byte compressedValue, float minValue, float maxValue)
	{
		float num = (float)-128;
		int num2 = 127;
		float t = Mathf.InverseLerp(num, (float)num2, (float)((sbyte)compressedValue));
		return Mathf.Lerp(minValue, maxValue, t);
	}

	// Token: 0x060011B3 RID: 4531 RVA: 0x00053304 File Offset: 0x00051504
	public static float DecompressShortToFloat(short compressedValue, float minValue, float maxValue)
	{
		float num = (float)-32768;
		int num2 = 32767;
		float t = Mathf.InverseLerp(num, (float)num2, (float)compressedValue);
		return Mathf.Lerp(minValue, maxValue, t);
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x0005332F File Offset: 0x0005152F
	public static void StepTime(double deltaTime, ref double localTimeline, double localTimescale)
	{
		localTimeline += deltaTime * localTimescale;
	}
}
