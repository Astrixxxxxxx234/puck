using System;
using UnityEngine.InputSystem;

// Token: 0x020000E3 RID: 227
public class ToggleInteraction : IInputInteraction
{
	// Token: 0x06000704 RID: 1796 RVA: 0x0002687C File Offset: 0x00024A7C
	public void Process(ref InputInteractionContext context)
	{
		if (!context.action.IsPressed())
		{
			return;
		}
		InputActionPhase phase = context.phase;
		if (phase != InputActionPhase.Waiting)
		{
			if (phase != InputActionPhase.Started)
			{
				return;
			}
			if (this.isToggled)
			{
				this.isToggled = false;
				context.Canceled();
			}
		}
		else if (!this.isToggled)
		{
			this.isToggled = true;
			context.Started();
			return;
		}
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x000268D2 File Offset: 0x00024AD2
	public void Reset()
	{
		this.isToggled = false;
	}

	// Token: 0x040004A1 RID: 1185
	private bool isToggled;
}
