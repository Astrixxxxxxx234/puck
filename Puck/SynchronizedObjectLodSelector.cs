using System;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class SynchronizedObjectLodSelector
{
	// Token: 0x06000BD8 RID: 3032 RVA: 0x00038A87 File Offset: 0x00036C87
	public void Configure(SynchronizedObjectBandSettings[] lodBands, SynchronizedObjectBandSettings culling, byte noOriginTickRateDivisor, float hysteresis, bool useHighPrecisionRotation)
	{
		this.lodBands = lodBands;
		this.culling = culling;
		this.noOriginTickRateDivisor = noOriginTickRateDivisor;
		this.hysteresis = hysteresis;
		this.useHighPrecisionRotation = useHighPrecisionRotation;
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x00038AB0 File Offset: 0x00036CB0
	public void BeginPlayer(Player player)
	{
		PlayerCamera playerCamera = (player != null) ? player.PlayerCamera : null;
		this.hasOrigin = (playerCamera != null);
		if (!this.hasOrigin)
		{
			this.origin = Vector3.zero;
			this.viewDirection = Vector3.zero;
			return;
		}
		this.origin = playerCamera.transform.position;
		this.viewDirection = playerCamera.transform.forward;
		this.viewDirection.y = 0f;
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x00038B2E File Offset: 0x00036D2E
	public SynchronizedObjectLodSelection Select(Vector3 position, SynchronizedObjectLodSelection previousSelection, bool usesLod, bool usesCulling)
	{
		return this.WithHighPrecisionRotation(this.GetSelection(position, previousSelection, usesLod, usesCulling));
	}

	// Token: 0x06000BDB RID: 3035 RVA: 0x00038B44 File Offset: 0x00036D44
	private SynchronizedObjectLodSelection GetSelection(Vector3 position, SynchronizedObjectLodSelection previousSelection, bool usesLod, bool usesCulling)
	{
		if (!this.hasOrigin)
		{
			if (!usesLod)
			{
				return SynchronizedObjectLodSelection.Default;
			}
			return this.GetNoOriginSelection();
		}
		else
		{
			Vector3 offset = position - this.origin;
			float sqrMagnitude = offset.sqrMagnitude;
			SynchronizedObjectLodSelection result = usesLod ? this.GetBandSelection(sqrMagnitude, previousSelection.BandIndex) : SynchronizedObjectLodSelection.Default;
			if (!usesCulling || !this.IsCulled(offset, sqrMagnitude, previousSelection.IsCulled))
			{
				return result;
			}
			result.Source = SynchronizedObjectLodSource.Culled;
			result.TickRateDivisor = (byte)Mathf.Max(1, (int)this.culling.TickRateDivisor);
			return result;
		}
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x00038BCF File Offset: 0x00036DCF
	private SynchronizedObjectLodSelection WithHighPrecisionRotation(SynchronizedObjectLodSelection selection)
	{
		selection.UseHighPrecisionRotation = (this.useHighPrecisionRotation && selection.Source == SynchronizedObjectLodSource.Default);
		return selection;
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x00038BF0 File Offset: 0x00036DF0
	private bool IsCulled(Vector3 offset, float sqrDistance, bool wasCulled)
	{
		if (this.culling.TickRateDivisor <= 1)
		{
			return false;
		}
		float num = this.culling.MinDistance * this.GetHysteresisScale(wasCulled);
		return sqrDistance > num * num && Vector3.Dot(offset, this.viewDirection) < 0f;
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x00038C3C File Offset: 0x00036E3C
	private SynchronizedObjectLodSelection GetBandSelection(float sqrDistance, int previousBandIndex)
	{
		SynchronizedObjectLodSelection @default = SynchronizedObjectLodSelection.Default;
		if (this.lodBands == null)
		{
			return @default;
		}
		for (int i = 0; i < this.lodBands.Length; i++)
		{
			float num = this.lodBands[i].MinDistance * this.GetHysteresisScale(i <= previousBandIndex);
			if (sqrDistance < num * num)
			{
				break;
			}
			@default.BandIndex = i;
			@default.Source = SynchronizedObjectLodSource.Band;
			@default.TickRateDivisor = (byte)Mathf.Max(1, (int)this.lodBands[i].TickRateDivisor);
		}
		return @default;
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x00038CC4 File Offset: 0x00036EC4
	private SynchronizedObjectLodSelection GetNoOriginSelection()
	{
		return new SynchronizedObjectLodSelection
		{
			BandIndex = -1,
			Source = SynchronizedObjectLodSource.NoOrigin,
			TickRateDivisor = (byte)Mathf.Max(1, (int)this.noOriginTickRateDivisor)
		};
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x00038CFE File Offset: 0x00036EFE
	private float GetHysteresisScale(bool isAlreadySelected)
	{
		if (!isAlreadySelected)
		{
			return 1f + this.hysteresis;
		}
		return 1f - this.hysteresis;
	}

	// Token: 0x04000746 RID: 1862
	private SynchronizedObjectBandSettings[] lodBands;

	// Token: 0x04000747 RID: 1863
	private SynchronizedObjectBandSettings culling;

	// Token: 0x04000748 RID: 1864
	private byte noOriginTickRateDivisor = 1;

	// Token: 0x04000749 RID: 1865
	private float hysteresis;

	// Token: 0x0400074A RID: 1866
	private bool useHighPrecisionRotation = true;

	// Token: 0x0400074B RID: 1867
	private bool hasOrigin;

	// Token: 0x0400074C RID: 1868
	private Vector3 origin = Vector3.zero;

	// Token: 0x0400074D RID: 1869
	private Vector3 viewDirection = Vector3.zero;
}
