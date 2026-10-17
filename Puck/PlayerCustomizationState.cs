using System;
using Unity.Netcode;

// Token: 0x0200005A RID: 90
public struct PlayerCustomizationState : INetworkSerializable, IEquatable<PlayerCustomizationState>
{
	// Token: 0x06000278 RID: 632 RVA: 0x00010344 File Offset: 0x0000E544
	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		if (serializer.IsReader)
		{
			FastBufferReader fastBufferReader = serializer.GetFastBufferReader();
			fastBufferReader.ReadValueSafe<int>(out this.FlagID, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.HeadgearIDBlueAttacker, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.HeadgearIDRedAttacker, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.HeadgearIDBlueGoalie, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.HeadgearIDRedGoalie, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.MustacheID, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.BeardID, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.JerseyIDBlueAttacker, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.JerseyIDRedAttacker, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.JerseyIDBlueGoalie, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.JerseyIDRedGoalie, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.StickSkinIDBlueAttacker, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.StickSkinIDRedAttacker, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.StickSkinIDBlueGoalie, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.StickSkinIDRedGoalie, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.StickShaftTapeIDBlueAttacker, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.StickShaftTapeIDRedAttacker, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.StickShaftTapeIDBlueGoalie, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.StickShaftTapeIDRedGoalie, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.StickBladeTapeIDBlueAttacker, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.StickBladeTapeIDRedAttacker, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.StickBladeTapeIDBlueGoalie, default(FastBufferWriter.ForPrimitives));
			fastBufferReader.ReadValueSafe<int>(out this.StickBladeTapeIDRedGoalie, default(FastBufferWriter.ForPrimitives));
			return;
		}
		FastBufferWriter fastBufferWriter = serializer.GetFastBufferWriter();
		fastBufferWriter.WriteValueSafe<int>(this.FlagID, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.HeadgearIDBlueAttacker, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.HeadgearIDRedAttacker, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.HeadgearIDBlueGoalie, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.HeadgearIDRedGoalie, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.MustacheID, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.BeardID, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.JerseyIDBlueAttacker, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.JerseyIDRedAttacker, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.JerseyIDBlueGoalie, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.JerseyIDRedGoalie, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.StickSkinIDBlueAttacker, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.StickSkinIDRedAttacker, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.StickSkinIDBlueGoalie, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.StickSkinIDRedGoalie, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.StickShaftTapeIDBlueAttacker, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.StickShaftTapeIDRedAttacker, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.StickShaftTapeIDBlueGoalie, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.StickShaftTapeIDRedGoalie, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.StickBladeTapeIDBlueAttacker, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.StickBladeTapeIDRedAttacker, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.StickBladeTapeIDBlueGoalie, default(FastBufferWriter.ForPrimitives));
		fastBufferWriter.WriteValueSafe<int>(this.StickBladeTapeIDRedGoalie, default(FastBufferWriter.ForPrimitives));
	}

	// Token: 0x06000279 RID: 633 RVA: 0x00010764 File Offset: 0x0000E964
	public bool Equals(PlayerCustomizationState other)
	{
		return this.FlagID == other.FlagID && this.HeadgearIDBlueAttacker == other.HeadgearIDBlueAttacker && this.HeadgearIDRedAttacker == other.HeadgearIDRedAttacker && this.HeadgearIDBlueGoalie == other.HeadgearIDBlueGoalie && this.HeadgearIDRedGoalie == other.HeadgearIDRedGoalie && this.MustacheID == other.MustacheID && this.BeardID == other.BeardID && this.JerseyIDBlueAttacker == other.JerseyIDBlueAttacker && this.JerseyIDRedAttacker == other.JerseyIDRedAttacker && this.JerseyIDBlueGoalie == other.JerseyIDBlueGoalie && this.JerseyIDRedGoalie == other.JerseyIDRedGoalie && this.StickSkinIDBlueAttacker == other.StickSkinIDBlueAttacker && this.StickSkinIDRedAttacker == other.StickSkinIDRedAttacker && this.StickSkinIDBlueGoalie == other.StickSkinIDBlueGoalie && this.StickSkinIDRedGoalie == other.StickSkinIDRedGoalie && this.StickShaftTapeIDBlueAttacker == other.StickShaftTapeIDBlueAttacker && this.StickShaftTapeIDRedAttacker == other.StickShaftTapeIDRedAttacker && this.StickShaftTapeIDBlueGoalie == other.StickShaftTapeIDBlueGoalie && this.StickShaftTapeIDRedGoalie == other.StickShaftTapeIDRedGoalie && this.StickBladeTapeIDBlueAttacker == other.StickBladeTapeIDBlueAttacker && this.StickBladeTapeIDRedAttacker == other.StickBladeTapeIDRedAttacker && this.StickBladeTapeIDBlueGoalie == other.StickBladeTapeIDBlueGoalie && this.StickBladeTapeIDRedGoalie == other.StickBladeTapeIDRedGoalie;
	}

	// Token: 0x0600027A RID: 634 RVA: 0x000108DC File Offset: 0x0000EADC
	public override string ToString()
	{
		return string.Format("FlagID: {0}, HeadgearIDs: [{1}, {2}, {3}, {4}], MustacheID: {5}, BeardID: {6}, JerseyIDs: [{7}, {8}, {9}, {10}], StickSkinIDs: [{11}, {12}, {13}, {14}], StickShaftTapeIDs: [{15}, {16}, {17}, {18}], StickBladeTapeIDs: [{19}, {20}, {21}, {22}]", new object[]
		{
			this.FlagID,
			this.HeadgearIDBlueAttacker,
			this.HeadgearIDRedAttacker,
			this.HeadgearIDBlueGoalie,
			this.HeadgearIDRedGoalie,
			this.MustacheID,
			this.BeardID,
			this.JerseyIDBlueAttacker,
			this.JerseyIDRedAttacker,
			this.JerseyIDBlueGoalie,
			this.JerseyIDRedGoalie,
			this.StickSkinIDBlueAttacker,
			this.StickSkinIDRedAttacker,
			this.StickSkinIDBlueGoalie,
			this.StickSkinIDRedGoalie,
			this.StickShaftTapeIDBlueAttacker,
			this.StickShaftTapeIDRedAttacker,
			this.StickShaftTapeIDBlueGoalie,
			this.StickShaftTapeIDRedGoalie,
			this.StickBladeTapeIDBlueAttacker,
			this.StickBladeTapeIDRedAttacker,
			this.StickBladeTapeIDBlueGoalie,
			this.StickBladeTapeIDRedGoalie
		});
	}

	// Token: 0x040001E1 RID: 481
	public int FlagID;

	// Token: 0x040001E2 RID: 482
	public int HeadgearIDBlueAttacker;

	// Token: 0x040001E3 RID: 483
	public int HeadgearIDRedAttacker;

	// Token: 0x040001E4 RID: 484
	public int HeadgearIDBlueGoalie;

	// Token: 0x040001E5 RID: 485
	public int HeadgearIDRedGoalie;

	// Token: 0x040001E6 RID: 486
	public int MustacheID;

	// Token: 0x040001E7 RID: 487
	public int BeardID;

	// Token: 0x040001E8 RID: 488
	public int JerseyIDBlueAttacker;

	// Token: 0x040001E9 RID: 489
	public int JerseyIDRedAttacker;

	// Token: 0x040001EA RID: 490
	public int JerseyIDBlueGoalie;

	// Token: 0x040001EB RID: 491
	public int JerseyIDRedGoalie;

	// Token: 0x040001EC RID: 492
	public int StickSkinIDBlueAttacker;

	// Token: 0x040001ED RID: 493
	public int StickSkinIDRedAttacker;

	// Token: 0x040001EE RID: 494
	public int StickSkinIDBlueGoalie;

	// Token: 0x040001EF RID: 495
	public int StickSkinIDRedGoalie;

	// Token: 0x040001F0 RID: 496
	public int StickShaftTapeIDBlueAttacker;

	// Token: 0x040001F1 RID: 497
	public int StickShaftTapeIDRedAttacker;

	// Token: 0x040001F2 RID: 498
	public int StickShaftTapeIDBlueGoalie;

	// Token: 0x040001F3 RID: 499
	public int StickShaftTapeIDRedGoalie;

	// Token: 0x040001F4 RID: 500
	public int StickBladeTapeIDBlueAttacker;

	// Token: 0x040001F5 RID: 501
	public int StickBladeTapeIDRedAttacker;

	// Token: 0x040001F6 RID: 502
	public int StickBladeTapeIDBlueGoalie;

	// Token: 0x040001F7 RID: 503
	public int StickBladeTapeIDRedGoalie;
}
