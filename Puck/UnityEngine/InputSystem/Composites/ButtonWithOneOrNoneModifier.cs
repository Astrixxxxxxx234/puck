using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Composites
{
	// Token: 0x020002A9 RID: 681
	[DisplayStringFormat("{modifier}+{button}")]
	public class ButtonWithOneOrNoneModifier : InputBindingComposite<float>
	{
		// Token: 0x0600147B RID: 5243 RVA: 0x00059699 File Offset: 0x00057899
		public override float ReadValue(ref InputBindingCompositeContext context)
		{
			if (this.ModifierIsPressed(ref context) || this.modifier == 0)
			{
				return context.ReadValue<float>(this.button);
			}
			return 0f;
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x000596C0 File Offset: 0x000578C0
		private bool ModifierIsPressed(ref InputBindingCompositeContext context)
		{
			bool flag = context.ReadValueAsButton(this.modifier);
			if (flag && !this.ignoreOrder)
			{
				double pressTime = context.GetPressTime(this.button);
				return context.GetPressTime(this.modifier) <= pressTime;
			}
			return flag;
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x00059706 File Offset: 0x00057906
		public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
		{
			return this.ReadValue(ref context);
		}

		// Token: 0x04000CDB RID: 3291
		[InputControl]
		public int modifier;

		// Token: 0x04000CDC RID: 3292
		[InputControl]
		public int button;

		// Token: 0x04000CDD RID: 3293
		public bool ignoreOrder;
	}
}
