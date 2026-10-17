using System;
using System.Collections.Generic;
using Unity.Netcode;

// Token: 0x02000220 RID: 544
public class CompressedNetworkVariable<TRaw, TNetwork> : NetworkVariable<TNetwork> where TRaw : struct where TNetwork : struct
{
	// Token: 0x14000005 RID: 5
	// (add) Token: 0x06001159 RID: 4441 RVA: 0x00052398 File Offset: 0x00050598
	// (remove) Token: 0x0600115A RID: 4442 RVA: 0x000523D0 File Offset: 0x000505D0
	public event Action<TRaw, TRaw> OnRawValueChanged;

	// Token: 0x0600115B RID: 4443 RVA: 0x00052408 File Offset: 0x00050608
	public CompressedNetworkVariable(Func<TRaw, TNetwork> compressor, Func<TNetwork, TRaw> decompressor, TRaw initialValue = default(TRaw), NetworkVariableReadPermission readPerm = NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission writePerm = NetworkVariableWritePermission.Server) : base(compressor(initialValue), readPerm, writePerm)
	{
		this.compressor = compressor;
		this.decompressor = decompressor;
		this.cachedValue = initialValue;
		this.OnValueChanged = (NetworkVariable<TNetwork>.OnValueChangedDelegate)Delegate.Combine(this.OnValueChanged, new NetworkVariable<TNetwork>.OnValueChangedDelegate(this.OnCompressedValueChanged));
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x0600115C RID: 4444 RVA: 0x0005245D File Offset: 0x0005065D
	// (set) Token: 0x0600115D RID: 4445 RVA: 0x00052468 File Offset: 0x00050668
	public new TRaw Value
	{
		get
		{
			return this.cachedValue;
		}
		set
		{
			TRaw traw = this.cachedValue;
			this.cachedValue = value;
			base.Value = this.compressor(value);
			if (!EqualityComparer<TRaw>.Default.Equals(this.cachedValue, traw))
			{
				Action<TRaw, TRaw> onRawValueChanged = this.OnRawValueChanged;
				if (onRawValueChanged == null)
				{
					return;
				}
				onRawValueChanged(traw, this.cachedValue);
			}
		}
	}

	// Token: 0x0600115E RID: 4446 RVA: 0x000524C0 File Offset: 0x000506C0
	private void OnCompressedValueChanged(TNetwork previousCompressed, TNetwork newCompressed)
	{
		TRaw arg = this.cachedValue;
		this.cachedValue = this.decompressor(newCompressed);
		Action<TRaw, TRaw> onRawValueChanged = this.OnRawValueChanged;
		if (onRawValueChanged == null)
		{
			return;
		}
		onRawValueChanged(arg, this.cachedValue);
	}

	// Token: 0x0600115F RID: 4447 RVA: 0x00052500 File Offset: 0x00050700
	public static CompressedNetworkVariable<float, short> CreateFloatToShort(float minValue, float maxValue, float initialValue = 0f, NetworkVariableReadPermission readPerm = NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission writePerm = NetworkVariableWritePermission.Server)
	{
		return new CompressedNetworkVariable<float, short>(delegate(float value)
		{
			float val = (value - minValue) / (maxValue - minValue);
			return (short)(Math.Max(0f, Math.Min(1f, val)) * 32767f);
		}, delegate(short compressed)
		{
			float num = (float)compressed / 32767f;
			return minValue + num * (maxValue - minValue);
		}, initialValue, readPerm, writePerm);
	}

	// Token: 0x06001160 RID: 4448 RVA: 0x00052544 File Offset: 0x00050744
	public static CompressedNetworkVariable<float, byte> CreateFloatToByte(float minValue, float maxValue, float initialValue = 0f, NetworkVariableReadPermission readPerm = NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission writePerm = NetworkVariableWritePermission.Server)
	{
		return new CompressedNetworkVariable<float, byte>(delegate(float value)
		{
			float val = (value - minValue) / (maxValue - minValue);
			return (byte)(Math.Max(0f, Math.Min(1f, val)) * 255f);
		}, delegate(byte compressed)
		{
			float num = (float)compressed / 255f;
			return minValue + num * (maxValue - minValue);
		}, initialValue, readPerm, writePerm);
	}

	// Token: 0x04000AC9 RID: 2761
	private readonly Func<TRaw, TNetwork> compressor;

	// Token: 0x04000ACA RID: 2762
	private readonly Func<TNetwork, TRaw> decompressor;

	// Token: 0x04000ACB RID: 2763
	private TRaw cachedValue;
}
