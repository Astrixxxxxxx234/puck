using System;
using UnityEngine;

// Token: 0x0200024B RID: 587
public class Vector3PIDController
{
	// Token: 0x0600129C RID: 4764 RVA: 0x00056360 File Offset: 0x00054560
	public Vector3PIDController(float proportionalGain = 0f, float integralGain = 0f, float derivativeGain = 0f)
	{
		this.proportionalGain = proportionalGain;
		this.integralGain = integralGain;
		this.derivativeGain = derivativeGain;
	}

	// Token: 0x0600129D RID: 4765 RVA: 0x000563E0 File Offset: 0x000545E0
	public Vector3 Update(float deltaTime, Vector3 currentValue, Vector3 targetValue)
	{
		if (deltaTime <= 0f)
		{
			return Vector3.zero;
		}
		Vector3 a = targetValue - currentValue;
		Vector3 vector = (a - this.errorLast) / deltaTime;
		this.errorLast = a;
		Vector3 a2 = (currentValue - this.valueLast) / deltaTime;
		this.valueLast = currentValue;
		Vector3 value = this.integrationStored + a * deltaTime;
		this.integrationStored = this.ClampVector3(value, -this.integralSaturation, this.integralSaturation);
		Vector3 vector2 = Vector3.zero;
		if (this.derivativeInitialized)
		{
			if (this.derivativeMeasurement == DerivativeMeasurement.Velocity)
			{
				vector2 = -a2;
			}
			else
			{
				vector2 = vector;
			}
			vector2 = Vector3.Lerp(this.derivativeLast, vector2, this.derivativeSmoothing);
			this.derivativeLast = vector2;
		}
		else
		{
			this.derivativeInitialized = true;
			this.derivativeLast = Vector3.zero;
		}
		Vector3 a3 = this.proportionalGain * a;
		Vector3 b = this.integralGain * this.integrationStored;
		Vector3 b2 = this.derivativeGain * vector2;
		Vector3 value2 = a3 + b + b2;
		return this.ClampVector3(value2, this.outputMin, this.outputMax);
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x0005650C File Offset: 0x0005470C
	public Vector3 UpdateAngle(float deltaTime, Vector3 currentValue, Vector3 targetValue)
	{
		if (deltaTime <= 0f)
		{
			return Vector3.zero;
		}
		Vector3 a = this.AngleDifference(targetValue, currentValue);
		Vector3 vector = this.AngleDifference(a, this.errorLast) / deltaTime;
		this.errorLast = a;
		Vector3 a2 = this.AngleDifference(currentValue, this.valueLast) / deltaTime;
		this.valueLast = currentValue;
		Vector3 value = this.integrationStored + a * deltaTime;
		this.integrationStored = this.ClampVector3(value, -this.integralSaturation, this.integralSaturation);
		Vector3 vector2 = Vector3.zero;
		if (this.derivativeInitialized)
		{
			if (this.derivativeMeasurement == DerivativeMeasurement.Velocity)
			{
				vector2 = -a2;
			}
			else
			{
				vector2 = vector;
			}
			vector2 = Vector3.Lerp(this.derivativeLast, vector2, this.derivativeSmoothing);
			this.derivativeLast = vector2;
		}
		else
		{
			this.derivativeInitialized = true;
			this.derivativeLast = Vector3.zero;
		}
		Vector3 a3 = this.proportionalGain * a;
		Vector3 b = this.integralGain * this.integrationStored;
		Vector3 b2 = this.derivativeGain * vector2;
		Vector3 value2 = a3 + b + b2;
		return this.ClampVector3(value2, this.outputMin, this.outputMax);
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x0005663A File Offset: 0x0005483A
	public void Reset()
	{
		this.derivativeInitialized = false;
		this.errorLast = Vector3.zero;
		this.valueLast = Vector3.zero;
		this.integrationStored = Vector3.zero;
		this.derivativeLast = Vector3.zero;
	}

	// Token: 0x060012A0 RID: 4768 RVA: 0x0005666F File Offset: 0x0005486F
	private Vector3 AngleDifference(Vector3 a, Vector3 b)
	{
		return new Vector3(Mathf.DeltaAngle(b.x, a.x), Mathf.DeltaAngle(b.y, a.y), Mathf.DeltaAngle(b.z, a.z));
	}

	// Token: 0x060012A1 RID: 4769 RVA: 0x000566A9 File Offset: 0x000548A9
	private Vector3 ClampVector3(Vector3 value, float min, float max)
	{
		return new Vector3(Mathf.Clamp(value.x, min, max), Mathf.Clamp(value.y, min, max), Mathf.Clamp(value.z, min, max));
	}

	// Token: 0x04000BDE RID: 3038
	public float proportionalGain;

	// Token: 0x04000BDF RID: 3039
	public float integralGain;

	// Token: 0x04000BE0 RID: 3040
	public float integralSaturation = float.MaxValue;

	// Token: 0x04000BE1 RID: 3041
	public float derivativeGain;

	// Token: 0x04000BE2 RID: 3042
	public float derivativeSmoothing = 1f;

	// Token: 0x04000BE3 RID: 3043
	public float outputMin = float.MinValue;

	// Token: 0x04000BE4 RID: 3044
	public float outputMax = float.MaxValue;

	// Token: 0x04000BE5 RID: 3045
	private Vector3 errorLast = Vector3.zero;

	// Token: 0x04000BE6 RID: 3046
	private Vector3 valueLast = Vector3.zero;

	// Token: 0x04000BE7 RID: 3047
	private Vector3 integrationStored = Vector3.zero;

	// Token: 0x04000BE8 RID: 3048
	private Vector3 derivativeLast = Vector3.zero;

	// Token: 0x04000BE9 RID: 3049
	private bool derivativeInitialized;

	// Token: 0x04000BEA RID: 3050
	public DerivativeMeasurement derivativeMeasurement;
}
