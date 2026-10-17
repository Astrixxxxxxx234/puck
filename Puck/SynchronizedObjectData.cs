using System;
using Unity.Netcode;
using UnityEngine;

// Token: 0x0200017B RID: 379
public struct SynchronizedObjectData : INetworkSerializable
{
	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000BBA RID: 3002 RVA: 0x00037D4D File Offset: 0x00035F4D
	public bool HasAllComponents
	{
		get
		{
			return (this.ChangeMask & 3071) == 3071;
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x06000BBB RID: 3003 RVA: 0x00037D62 File Offset: 0x00035F62
	public bool IsAsleep
	{
		get
		{
			return (this.ChangeMask & 4096) > 0;
		}
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x00037D74 File Offset: 0x00035F74
	public Vector3 GetPosition()
	{
		return new Vector3(NetworkingUtils.DecompressShortToFloat(this.X, -25f, 25f), NetworkingUtils.DecompressShortToFloat(this.Y, -50f, 50f), NetworkingUtils.DecompressShortToFloat(this.Z, -50f, 50f));
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06000BBD RID: 3005 RVA: 0x00037DC5 File Offset: 0x00035FC5
	public Quaternion Rotation
	{
		get
		{
			return this.GetRotation((this.ChangeMask & 1024) > 0);
		}
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x00037DDC File Offset: 0x00035FDC
	public Quaternion GetRotation(bool useHighPrecisionRotation)
	{
		if (useHighPrecisionRotation)
		{
			return Quaternion.Normalize(new Quaternion(NetworkingUtils.DecompressShortToFloat(this.Rx, -1f, 1f), NetworkingUtils.DecompressShortToFloat(this.Ry, -1f, 1f), NetworkingUtils.DecompressShortToFloat(this.Rz, -1f, 1f), NetworkingUtils.DecompressShortToFloat(this.Rw, -1f, 1f)));
		}
		Quaternion identity = Quaternion.identity;
		QuaternionCompressor.DecompressQuaternion(ref identity, this.CompressedRotation);
		return identity;
	}

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x06000BBF RID: 3007 RVA: 0x00037E60 File Offset: 0x00036060
	public Vector3 LinearVelocity
	{
		get
		{
			return new Vector3(NetworkingUtils.DecompressShortToFloat(this.Vx, -100f, 100f), NetworkingUtils.DecompressShortToFloat(this.Vy, -100f, 100f), NetworkingUtils.DecompressShortToFloat(this.Vz, -100f, 100f));
		}
	}

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x00037EB4 File Offset: 0x000360B4
	public Vector3 AngularVelocity
	{
		get
		{
			return new Vector3(NetworkingUtils.DecompressShortToFloat(this.Ax, -100f, 100f), NetworkingUtils.DecompressShortToFloat(this.Ay, -100f, 100f), NetworkingUtils.DecompressShortToFloat(this.Az, -100f, 100f));
		}
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x00037F08 File Offset: 0x00036108
	public SynchronizedObjectData(ulong networkObjectId, Vector3 position, Quaternion rotation, Vector3 linearVelocity, Vector3 angularVelocity)
	{
		this.NetworkObjectId = (ushort)networkObjectId;
		this.ChangeMask = 4095;
		this.X = NetworkingUtils.CompressFloatToShort(position.x, -25f, 25f);
		this.Y = NetworkingUtils.CompressFloatToShort(position.y, -50f, 50f);
		this.Z = NetworkingUtils.CompressFloatToShort(position.z, -50f, 50f);
		this.Rx = NetworkingUtils.CompressFloatToShort(rotation.x, -1f, 1f);
		this.Ry = NetworkingUtils.CompressFloatToShort(rotation.y, -1f, 1f);
		this.Rz = NetworkingUtils.CompressFloatToShort(rotation.z, -1f, 1f);
		this.Rw = NetworkingUtils.CompressFloatToShort(rotation.w, -1f, 1f);
		this.CompressedRotation = QuaternionCompressor.CompressQuaternion(ref rotation);
		this.Vx = NetworkingUtils.CompressFloatToShort(linearVelocity.x, -100f, 100f);
		this.Vy = NetworkingUtils.CompressFloatToShort(linearVelocity.y, -100f, 100f);
		this.Vz = NetworkingUtils.CompressFloatToShort(linearVelocity.z, -100f, 100f);
		this.Ax = NetworkingUtils.CompressFloatToShort(angularVelocity.x, -100f, 100f);
		this.Ay = NetworkingUtils.CompressFloatToShort(angularVelocity.y, -100f, 100f);
		this.Az = NetworkingUtils.CompressFloatToShort(angularVelocity.z, -100f, 100f);
		this.TickRateDivisor = 1;
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x000380A4 File Offset: 0x000362A4
	public SynchronizedObjectData WithTickRateDivisor(byte tickRateDivisor)
	{
		SynchronizedObjectData result = this;
		result.TickRateDivisor = tickRateDivisor;
		return result;
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x000380C4 File Offset: 0x000362C4
	public ushort GetChangeMask(SynchronizedObjectData other, bool useHighPrecisionRotation)
	{
		ushort num = SynchronizedObjectData.GetAxisChangeMask(this.GetPosition(), other.GetPosition(), 0.002f, 1, 2, 4);
		if (this.HasRotationChanged(other, useHighPrecisionRotation) && SynchronizedObjectData.GetAngleDegrees(this.GetRotation(useHighPrecisionRotation), other.GetRotation(useHighPrecisionRotation)) > 0.05f)
		{
			num |= 8;
		}
		num |= SynchronizedObjectData.GetAxisChangeMask(this.LinearVelocity, other.LinearVelocity, 0.05f, 16, 32, 64);
		num |= SynchronizedObjectData.GetAxisChangeMask(this.AngularVelocity, other.AngularVelocity, 0.1f, 128, 256, 512);
		if (this.TickRateDivisor != other.TickRateDivisor)
		{
			num |= 2048;
		}
		return num;
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x0003817C File Offset: 0x0003637C
	public SynchronizedObjectData WithAsleep()
	{
		SynchronizedObjectData result = this;
		result.ChangeMask |= 4096;
		return result;
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x000381A4 File Offset: 0x000363A4
	public SynchronizedObjectData WithComponentMask(ushort componentMask, bool useHighPrecisionRotation)
	{
		SynchronizedObjectData synchronizedObjectData = this;
		synchronizedObjectData.ChangeMask = (componentMask & 3071);
		if (useHighPrecisionRotation && (synchronizedObjectData.ChangeMask & 8) != 0)
		{
			synchronizedObjectData.ChangeMask |= 1024;
		}
		return synchronizedObjectData;
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x000381E8 File Offset: 0x000363E8
	public SynchronizedObjectData Merge(SynchronizedObjectData changes)
	{
		SynchronizedObjectData result = this;
		if ((changes.ChangeMask & 1) != 0)
		{
			result.X = changes.X;
		}
		if ((changes.ChangeMask & 2) != 0)
		{
			result.Y = changes.Y;
		}
		if ((changes.ChangeMask & 4) != 0)
		{
			result.Z = changes.Z;
		}
		if ((changes.ChangeMask & 8) != 0)
		{
			result.CompressedRotation = changes.CompressedRotation;
			result.Rx = changes.Rx;
			result.Ry = changes.Ry;
			result.Rz = changes.Rz;
			result.Rw = changes.Rw;
		}
		if ((changes.ChangeMask & 16) != 0)
		{
			result.Vx = changes.Vx;
		}
		if ((changes.ChangeMask & 32) != 0)
		{
			result.Vy = changes.Vy;
		}
		if ((changes.ChangeMask & 64) != 0)
		{
			result.Vz = changes.Vz;
		}
		if ((changes.ChangeMask & 128) != 0)
		{
			result.Ax = changes.Ax;
		}
		if ((changes.ChangeMask & 256) != 0)
		{
			result.Ay = changes.Ay;
		}
		if ((changes.ChangeMask & 512) != 0)
		{
			result.Az = changes.Az;
		}
		if ((changes.ChangeMask & 2048) != 0)
		{
			result.TickRateDivisor = changes.TickRateDivisor;
		}
		ushort num = ((changes.ChangeMask & 8) != 0) ? changes.ChangeMask : this.ChangeMask;
		result.ChangeMask = (3071 | (num & 1024));
		return result;
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x00038370 File Offset: 0x00036570
	private bool HasRotationChanged(SynchronizedObjectData other, bool useHighPrecisionRotation)
	{
		if (useHighPrecisionRotation)
		{
			return this.Rx != other.Rx || this.Ry != other.Ry || this.Rz != other.Rz || this.Rw != other.Rw;
		}
		return this.CompressedRotation != other.CompressedRotation;
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x000383D0 File Offset: 0x000365D0
	private static ushort GetAxisChangeMask(Vector3 value, Vector3 otherValue, float changeThreshold, ushort xAxisMask, ushort yAxisMask, ushort zAxisMask)
	{
		ushort num = 0;
		if (Mathf.Abs(value.x - otherValue.x) > changeThreshold)
		{
			num |= xAxisMask;
		}
		if (Mathf.Abs(value.y - otherValue.y) > changeThreshold)
		{
			num |= yAxisMask;
		}
		if (Mathf.Abs(value.z - otherValue.z) > changeThreshold)
		{
			num |= zAxisMask;
		}
		return num;
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x00038430 File Offset: 0x00036630
	private static float GetAngleDegrees(Quaternion a, Quaternion b)
	{
		if (Quaternion.Dot(a, b) < 0f)
		{
			b = new Quaternion(-b.x, -b.y, -b.z, -b.w);
		}
		float num = a.x - b.x;
		float num2 = a.y - b.y;
		float num3 = a.z - b.z;
		float num4 = a.w - b.w;
		float b2 = 0.5f * Mathf.Sqrt(num * num + num2 * num2 + num3 * num3 + num4 * num4);
		return 4f * Mathf.Asin(Mathf.Min(1f, b2)) * 57.29578f;
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x000384DF File Offset: 0x000366DF
	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		if (serializer.IsReader)
		{
			this.Read(serializer.GetFastBufferReader());
			return;
		}
		this.Write(serializer.GetFastBufferWriter());
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x00038508 File Offset: 0x00036708
	private void Read(FastBufferReader reader)
	{
		reader.ReadValueSafe<ushort>(out this.NetworkObjectId, default(FastBufferWriter.ForPrimitives));
		reader.ReadValueSafe<ushort>(out this.ChangeMask, default(FastBufferWriter.ForPrimitives));
		if ((this.ChangeMask & 1) != 0)
		{
			reader.ReadValueSafe<short>(out this.X, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 2) != 0)
		{
			reader.ReadValueSafe<short>(out this.Y, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 4) != 0)
		{
			reader.ReadValueSafe<short>(out this.Z, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 8) != 0)
		{
			if ((this.ChangeMask & 1024) != 0)
			{
				reader.ReadValueSafe<short>(out this.Rx, default(FastBufferWriter.ForPrimitives));
				reader.ReadValueSafe<short>(out this.Ry, default(FastBufferWriter.ForPrimitives));
				reader.ReadValueSafe<short>(out this.Rz, default(FastBufferWriter.ForPrimitives));
				reader.ReadValueSafe<short>(out this.Rw, default(FastBufferWriter.ForPrimitives));
			}
			else
			{
				reader.ReadValueSafe<uint>(out this.CompressedRotation, default(FastBufferWriter.ForPrimitives));
			}
		}
		if ((this.ChangeMask & 16) != 0)
		{
			reader.ReadValueSafe<short>(out this.Vx, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 32) != 0)
		{
			reader.ReadValueSafe<short>(out this.Vy, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 64) != 0)
		{
			reader.ReadValueSafe<short>(out this.Vz, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 128) != 0)
		{
			reader.ReadValueSafe<short>(out this.Ax, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 256) != 0)
		{
			reader.ReadValueSafe<short>(out this.Ay, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 512) != 0)
		{
			reader.ReadValueSafe<short>(out this.Az, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 2048) != 0)
		{
			reader.ReadValueSafe<byte>(out this.TickRateDivisor, default(FastBufferWriter.ForPrimitives));
		}
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x0003871C File Offset: 0x0003691C
	private void Write(FastBufferWriter writer)
	{
		writer.WriteValueSafe<ushort>(this.NetworkObjectId, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<ushort>(this.ChangeMask, default(FastBufferWriter.ForPrimitives));
		if ((this.ChangeMask & 1) != 0)
		{
			writer.WriteValueSafe<short>(this.X, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 2) != 0)
		{
			writer.WriteValueSafe<short>(this.Y, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 4) != 0)
		{
			writer.WriteValueSafe<short>(this.Z, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 8) != 0)
		{
			if ((this.ChangeMask & 1024) != 0)
			{
				writer.WriteValueSafe<short>(this.Rx, default(FastBufferWriter.ForPrimitives));
				writer.WriteValueSafe<short>(this.Ry, default(FastBufferWriter.ForPrimitives));
				writer.WriteValueSafe<short>(this.Rz, default(FastBufferWriter.ForPrimitives));
				writer.WriteValueSafe<short>(this.Rw, default(FastBufferWriter.ForPrimitives));
			}
			else
			{
				writer.WriteValueSafe<uint>(this.CompressedRotation, default(FastBufferWriter.ForPrimitives));
			}
		}
		if ((this.ChangeMask & 16) != 0)
		{
			writer.WriteValueSafe<short>(this.Vx, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 32) != 0)
		{
			writer.WriteValueSafe<short>(this.Vy, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 64) != 0)
		{
			writer.WriteValueSafe<short>(this.Vz, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 128) != 0)
		{
			writer.WriteValueSafe<short>(this.Ax, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 256) != 0)
		{
			writer.WriteValueSafe<short>(this.Ay, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 512) != 0)
		{
			writer.WriteValueSafe<short>(this.Az, default(FastBufferWriter.ForPrimitives));
		}
		if ((this.ChangeMask & 2048) != 0)
		{
			writer.WriteValueSafe<byte>(this.TickRateDivisor, default(FastBufferWriter.ForPrimitives));
		}
	}

	// Token: 0x04000709 RID: 1801
	public const ushort CHANGE_MASK_POSITION_X = 1;

	// Token: 0x0400070A RID: 1802
	public const ushort CHANGE_MASK_POSITION_Y = 2;

	// Token: 0x0400070B RID: 1803
	public const ushort CHANGE_MASK_POSITION_Z = 4;

	// Token: 0x0400070C RID: 1804
	public const ushort CHANGE_MASK_ROTATION = 8;

	// Token: 0x0400070D RID: 1805
	public const ushort CHANGE_MASK_LINEAR_VELOCITY_X = 16;

	// Token: 0x0400070E RID: 1806
	public const ushort CHANGE_MASK_LINEAR_VELOCITY_Y = 32;

	// Token: 0x0400070F RID: 1807
	public const ushort CHANGE_MASK_LINEAR_VELOCITY_Z = 64;

	// Token: 0x04000710 RID: 1808
	public const ushort CHANGE_MASK_ANGULAR_VELOCITY_X = 128;

	// Token: 0x04000711 RID: 1809
	public const ushort CHANGE_MASK_ANGULAR_VELOCITY_Y = 256;

	// Token: 0x04000712 RID: 1810
	public const ushort CHANGE_MASK_ANGULAR_VELOCITY_Z = 512;

	// Token: 0x04000713 RID: 1811
	public const ushort CHANGE_MASK_HIGH_PRECISION_ROTATION = 1024;

	// Token: 0x04000714 RID: 1812
	public const ushort CHANGE_MASK_TICK_RATE_DIVISOR = 2048;

	// Token: 0x04000715 RID: 1813
	public const ushort CHANGE_MASK_ASLEEP = 4096;

	// Token: 0x04000716 RID: 1814
	public const ushort CHANGE_MASK_POSITION = 7;

	// Token: 0x04000717 RID: 1815
	public const ushort CHANGE_MASK_LINEAR_VELOCITY = 112;

	// Token: 0x04000718 RID: 1816
	public const ushort CHANGE_MASK_ANGULAR_VELOCITY = 896;

	// Token: 0x04000719 RID: 1817
	public const ushort CHANGE_MASK_MOTION_COMPONENTS = 1023;

	// Token: 0x0400071A RID: 1818
	public const ushort CHANGE_MASK_ALL_COMPONENTS = 3071;

	// Token: 0x0400071B RID: 1819
	private const float POSITION_CHANGE_THRESHOLD = 0.002f;

	// Token: 0x0400071C RID: 1820
	private const float ROTATION_CHANGE_THRESHOLD = 0.05f;

	// Token: 0x0400071D RID: 1821
	private const float LINEAR_VELOCITY_CHANGE_THRESHOLD = 0.05f;

	// Token: 0x0400071E RID: 1822
	private const float ANGULAR_VELOCITY_CHANGE_THRESHOLD = 0.1f;

	// Token: 0x0400071F RID: 1823
	private const float POSITION_MIN_X = -25f;

	// Token: 0x04000720 RID: 1824
	private const float POSITION_MAX_X = 25f;

	// Token: 0x04000721 RID: 1825
	private const float POSITION_MIN_Y = -50f;

	// Token: 0x04000722 RID: 1826
	private const float POSITION_MAX_Y = 50f;

	// Token: 0x04000723 RID: 1827
	private const float POSITION_MIN_Z = -50f;

	// Token: 0x04000724 RID: 1828
	private const float POSITION_MAX_Z = 50f;

	// Token: 0x04000725 RID: 1829
	private const float ROTATION_MIN = -1f;

	// Token: 0x04000726 RID: 1830
	private const float ROTATION_MAX = 1f;

	// Token: 0x04000727 RID: 1831
	private const float LINEAR_VELOCITY_MIN = -100f;

	// Token: 0x04000728 RID: 1832
	private const float LINEAR_VELOCITY_MAX = 100f;

	// Token: 0x04000729 RID: 1833
	private const float ANGULAR_VELOCITY_MIN = -100f;

	// Token: 0x0400072A RID: 1834
	private const float ANGULAR_VELOCITY_MAX = 100f;

	// Token: 0x0400072B RID: 1835
	public ushort NetworkObjectId;

	// Token: 0x0400072C RID: 1836
	public ushort ChangeMask;

	// Token: 0x0400072D RID: 1837
	public byte TickRateDivisor;

	// Token: 0x0400072E RID: 1838
	public short X;

	// Token: 0x0400072F RID: 1839
	public short Y;

	// Token: 0x04000730 RID: 1840
	public short Z;

	// Token: 0x04000731 RID: 1841
	public uint CompressedRotation;

	// Token: 0x04000732 RID: 1842
	public short Rx;

	// Token: 0x04000733 RID: 1843
	public short Ry;

	// Token: 0x04000734 RID: 1844
	public short Rz;

	// Token: 0x04000735 RID: 1845
	public short Rw;

	// Token: 0x04000736 RID: 1846
	public short Vx;

	// Token: 0x04000737 RID: 1847
	public short Vy;

	// Token: 0x04000738 RID: 1848
	public short Vz;

	// Token: 0x04000739 RID: 1849
	public short Ax;

	// Token: 0x0400073A RID: 1850
	public short Ay;

	// Token: 0x0400073B RID: 1851
	public short Az;
}
