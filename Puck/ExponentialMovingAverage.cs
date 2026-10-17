using System;

// Token: 0x02000229 RID: 553
public struct ExponentialMovingAverage
{
	// Token: 0x06001184 RID: 4484 RVA: 0x00052BDC File Offset: 0x00050DDC
	public ExponentialMovingAverage(int n)
	{
		this.alpha = 2.0 / (double)(n + 1);
		this.initialized = false;
		this.Value = 0.0;
		this.Variance = 0.0;
		this.StandardDeviation = 0.0;
	}

	// Token: 0x06001185 RID: 4485 RVA: 0x00052C34 File Offset: 0x00050E34
	public void Add(double newValue)
	{
		if (this.initialized)
		{
			double num = newValue - this.Value;
			this.Value += this.alpha * num;
			this.Variance = (1.0 - this.alpha) * (this.Variance + this.alpha * num * num);
			this.StandardDeviation = Math.Sqrt(this.Variance);
			return;
		}
		this.Value = newValue;
		this.initialized = true;
	}

	// Token: 0x06001186 RID: 4486 RVA: 0x00052CAF File Offset: 0x00050EAF
	public void Reset()
	{
		this.initialized = false;
		this.Value = 0.0;
		this.Variance = 0.0;
		this.StandardDeviation = 0.0;
	}

	// Token: 0x04000B70 RID: 2928
	private readonly double alpha;

	// Token: 0x04000B71 RID: 2929
	private bool initialized;

	// Token: 0x04000B72 RID: 2930
	public double Value;

	// Token: 0x04000B73 RID: 2931
	public double Variance;

	// Token: 0x04000B74 RID: 2932
	public double StandardDeviation;
}
