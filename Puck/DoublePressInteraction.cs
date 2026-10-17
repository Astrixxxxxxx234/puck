using System;
using UnityEngine.InputSystem;

// Token: 0x020000D8 RID: 216
public class DoublePressInteraction : IInputInteraction
{
	// Token: 0x060006CF RID: 1743 RVA: 0x000252E0 File Offset: 0x000234E0
	public void Process(ref InputInteractionContext context)
	{
		if (context.timerHasExpired)
		{
			context.Canceled();
			return;
		}
		InputActionPhase phase = context.phase;
		if (phase != InputActionPhase.Waiting)
		{
			if (phase != InputActionPhase.Started)
			{
				return;
			}
			if (this.released)
			{
				if (context.ReadValue<float>() > this.pressThreshold)
				{
					context.Performed();
					return;
				}
			}
			else if (context.ReadValue<float>() < this.releaseThreshold)
			{
				this.released = true;
			}
		}
		else if (context.ReadValue<float>() > this.pressThreshold)
		{
			context.Started();
			context.SetTimeout(this.maxTapDuration);
			return;
		}
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x0002535F File Offset: 0x0002355F
	public void Reset()
	{
		this.released = false;
	}

	// Token: 0x0400045B RID: 1115
	public float maxTapDuration = 0.2f;

	// Token: 0x0400045C RID: 1116
	public float pressThreshold = 0.5f;

	// Token: 0x0400045D RID: 1117
	public float releaseThreshold = 0.5f;

	// Token: 0x0400045E RID: 1118
	private bool released;
}
