using System;
using UnityEngine;

// Token: 0x0200023B RID: 571
public class PIDController
{
	// Token: 0x060011E2 RID: 4578 RVA: 0x000536D4 File Offset: 0x000518D4
	public PIDController(float proportionalGain = 0f, float integralGain = 0f, float derivativeGain = 0f)
	{
		this.proportionalGain = proportionalGain;
		this.integralGain = integralGain;
		this.derivativeGain = derivativeGain;
	}

	// Token: 0x060011E3 RID: 4579 RVA: 0x00053728 File Offset: 0x00051928
	public float Update(float deltaTime, float currentValue, float targetValue)
	{
		if (deltaTime <= 0f)
		{
			return 0f;
		}
		float num = targetValue - currentValue;
		float num2 = (num - this.errorLast) / deltaTime;
		this.errorLast = num;
		float num3 = (currentValue - this.valueLast) / deltaTime;
		this.valueLast = currentValue;
		float value = this.integrationStored + num * deltaTime;
		this.integrationStored = Mathf.Clamp(value, -this.integralSaturation, this.integralSaturation);
		float num4 = 0f;
		if (this.derivativeInitialized)
		{
			if (this.derivativeMeasurement == DerivativeMeasurement.Velocity)
			{
				num4 = -num3;
			}
			else
			{
				num4 = num2;
			}
			num4 = Mathf.Lerp(this.derivativeLast, num4, this.derivativeSmoothing);
			this.derivativeLast = num4;
		}
		else
		{
			this.derivativeInitialized = true;
			this.derivativeLast = 0f;
		}
		float num5 = this.proportionalGain * num;
		float num6 = this.integralGain * this.integrationStored;
		float num7 = this.derivativeGain * num4;
		return Mathf.Clamp(num5 + num6 + num7, this.outputMin, this.outputMax);
	}

	// Token: 0x060011E4 RID: 4580 RVA: 0x0005381C File Offset: 0x00051A1C
	public float UpdateAngle(float deltaTime, float currentValue, float targetValue)
	{
		if (deltaTime <= 0f)
		{
			return 0f;
		}
		float num = this.AngleDifference(targetValue, currentValue);
		float num2 = this.AngleDifference(num, this.errorLast) / deltaTime;
		this.errorLast = num;
		float num3 = this.AngleDifference(currentValue, this.valueLast) / deltaTime;
		this.valueLast = currentValue;
		float value = this.integrationStored + num * deltaTime;
		this.integrationStored = Mathf.Clamp(value, -this.integralSaturation, this.integralSaturation);
		float num4 = 0f;
		if (this.derivativeInitialized)
		{
			if (this.derivativeMeasurement == DerivativeMeasurement.Velocity)
			{
				num4 = -num3;
			}
			else
			{
				num4 = num2;
			}
			num4 = Mathf.Lerp(this.derivativeLast, num4, this.derivativeSmoothing);
			this.derivativeLast = num4;
		}
		else
		{
			this.derivativeInitialized = true;
			this.derivativeLast = 0f;
		}
		float num5 = this.proportionalGain * num;
		float num6 = this.integralGain * this.integrationStored;
		float num7 = this.derivativeGain * num4;
		return Mathf.Clamp(num5 + num6 + num7, this.outputMin, this.outputMax);
	}

	// Token: 0x060011E5 RID: 4581 RVA: 0x0005391C File Offset: 0x00051B1C
	public void Reset()
	{
		this.derivativeInitialized = false;
		this.errorLast = 0f;
		this.valueLast = 0f;
		this.integrationStored = 0f;
		this.derivativeLast = 0f;
	}

	// Token: 0x060011E6 RID: 4582 RVA: 0x00053951 File Offset: 0x00051B51
	private float AngleDifference(float a, float b)
	{
		return Mathf.DeltaAngle(b, a);
	}

	// Token: 0x04000B98 RID: 2968
	public float proportionalGain;

	// Token: 0x04000B99 RID: 2969
	public float integralGain;

	// Token: 0x04000B9A RID: 2970
	public float integralSaturation = float.MaxValue;

	// Token: 0x04000B9B RID: 2971
	public float derivativeGain;

	// Token: 0x04000B9C RID: 2972
	public float derivativeSmoothing = 1f;

	// Token: 0x04000B9D RID: 2973
	public float outputMin = float.MinValue;

	// Token: 0x04000B9E RID: 2974
	public float outputMax = float.MaxValue;

	// Token: 0x04000B9F RID: 2975
	private float errorLast;

	// Token: 0x04000BA0 RID: 2976
	private float valueLast;

	// Token: 0x04000BA1 RID: 2977
	private float integrationStored;

	// Token: 0x04000BA2 RID: 2978
	private float derivativeLast;

	// Token: 0x04000BA3 RID: 2979
	private bool derivativeInitialized;

	// Token: 0x04000BA4 RID: 2980
	public DerivativeMeasurement derivativeMeasurement;
}
