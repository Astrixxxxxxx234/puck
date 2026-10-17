using System;
using UnityEngine;

// Token: 0x0200024D RID: 589
public class VoiceLevelMeter
{
	// Token: 0x17000181 RID: 385
	// (get) Token: 0x060012A3 RID: 4771 RVA: 0x0005675D File Offset: 0x0005495D
	public float Level
	{
		get
		{
			return this.level;
		}
	}

	// Token: 0x060012A4 RID: 4772 RVA: 0x00056765 File Offset: 0x00054965
	public void Reset()
	{
		this.level = 0f;
		this.target = 0f;
		this.timeSinceVoice = 0f;
	}

	// Token: 0x060012A5 RID: 4773 RVA: 0x00056788 File Offset: 0x00054988
	public float Update(float rawLevel, bool hasVoice, float deltaTime)
	{
		if (hasVoice)
		{
			this.target = rawLevel;
			this.timeSinceVoice = 0f;
		}
		else
		{
			this.timeSinceVoice += deltaTime;
			if (this.timeSinceVoice >= 0.1f)
			{
				this.target = 0f;
			}
		}
		float t = 1f - Mathf.Exp(-15f * deltaTime);
		this.level = Mathf.Lerp(this.level, this.target, t);
		return this.level;
	}

	// Token: 0x04000BEB RID: 3051
	private float level;

	// Token: 0x04000BEC RID: 3052
	private float target;

	// Token: 0x04000BED RID: 3053
	private float timeSinceVoice;
}
