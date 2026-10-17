using System;
using DG.Tweening;

// Token: 0x02000245 RID: 581
public static class TweenUtils
{
	// Token: 0x0600124D RID: 4685 RVA: 0x0005512C File Offset: 0x0005332C
	public static Sequence RampHoldRamp(float rest, float depth, float rampInSeconds, float holdSeconds, float rampOutSeconds, Ease rampInEase, Ease rampOutEase, TweenCallback<float> onValue)
	{
		return TweenUtils.RampHoldRamp(rest, depth, rest, rampInSeconds, holdSeconds, rampOutSeconds, rampInEase, rampOutEase, onValue);
	}

	// Token: 0x0600124E RID: 4686 RVA: 0x0005514C File Offset: 0x0005334C
	public static Sequence RampHoldRamp(float from, float depth, float to, float rampInSeconds, float holdSeconds, float rampOutSeconds, Ease rampInEase, Ease rampOutEase, TweenCallback<float> onValue)
	{
		Sequence sequence = DOTween.Sequence().SetUpdate(true);
		sequence.Append(DOVirtual.Float(from, depth, rampInSeconds, onValue).SetEase(rampInEase));
		sequence.AppendInterval(holdSeconds);
		sequence.Append(DOVirtual.Float(depth, to, rampOutSeconds, onValue).SetEase(rampOutEase));
		return sequence;
	}
}
