using System;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000012 RID: 18
	public static class DOTweenCYInstruction
	{
		// Token: 0x02000013 RID: 19
		public class WaitForCompletion : CustomYieldInstruction
		{
			// Token: 0x17000001 RID: 1
			// (get) Token: 0x06000029 RID: 41 RVA: 0x00002C0A File Offset: 0x00000E0A
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && !this.t.IsComplete();
				}
			}

			// Token: 0x0600002A RID: 42 RVA: 0x00002C29 File Offset: 0x00000E29
			public WaitForCompletion(Tween tween)
			{
				this.t = tween;
			}

			// Token: 0x0400002E RID: 46
			private readonly Tween t;
		}

		// Token: 0x02000014 RID: 20
		public class WaitForRewind : CustomYieldInstruction
		{
			// Token: 0x17000002 RID: 2
			// (get) Token: 0x0600002B RID: 43 RVA: 0x00002C38 File Offset: 0x00000E38
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && (!this.t.playedOnce || this.t.position * (float)(this.t.CompletedLoops() + 1) > 0f);
				}
			}

			// Token: 0x0600002C RID: 44 RVA: 0x00002C84 File Offset: 0x00000E84
			public WaitForRewind(Tween tween)
			{
				this.t = tween;
			}

			// Token: 0x0400002F RID: 47
			private readonly Tween t;
		}

		// Token: 0x02000015 RID: 21
		public class WaitForKill : CustomYieldInstruction
		{
			// Token: 0x17000003 RID: 3
			// (get) Token: 0x0600002D RID: 45 RVA: 0x00002C93 File Offset: 0x00000E93
			public override bool keepWaiting
			{
				get
				{
					return this.t.active;
				}
			}

			// Token: 0x0600002E RID: 46 RVA: 0x00002CA0 File Offset: 0x00000EA0
			public WaitForKill(Tween tween)
			{
				this.t = tween;
			}

			// Token: 0x04000030 RID: 48
			private readonly Tween t;
		}

		// Token: 0x02000016 RID: 22
		public class WaitForElapsedLoops : CustomYieldInstruction
		{
			// Token: 0x17000004 RID: 4
			// (get) Token: 0x0600002F RID: 47 RVA: 0x00002CAF File Offset: 0x00000EAF
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && this.t.CompletedLoops() < this.elapsedLoops;
				}
			}

			// Token: 0x06000030 RID: 48 RVA: 0x00002CD3 File Offset: 0x00000ED3
			public WaitForElapsedLoops(Tween tween, int elapsedLoops)
			{
				this.t = tween;
				this.elapsedLoops = elapsedLoops;
			}

			// Token: 0x04000031 RID: 49
			private readonly Tween t;

			// Token: 0x04000032 RID: 50
			private readonly int elapsedLoops;
		}

		// Token: 0x02000017 RID: 23
		public class WaitForPosition : CustomYieldInstruction
		{
			// Token: 0x17000005 RID: 5
			// (get) Token: 0x06000031 RID: 49 RVA: 0x00002CE9 File Offset: 0x00000EE9
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && this.t.position * (float)(this.t.CompletedLoops() + 1) < this.position;
				}
			}

			// Token: 0x06000032 RID: 50 RVA: 0x00002D1C File Offset: 0x00000F1C
			public WaitForPosition(Tween tween, float position)
			{
				this.t = tween;
				this.position = position;
			}

			// Token: 0x04000033 RID: 51
			private readonly Tween t;

			// Token: 0x04000034 RID: 52
			private readonly float position;
		}

		// Token: 0x02000018 RID: 24
		public class WaitForStart : CustomYieldInstruction
		{
			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000033 RID: 51 RVA: 0x00002D32 File Offset: 0x00000F32
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && !this.t.playedOnce;
				}
			}

			// Token: 0x06000034 RID: 52 RVA: 0x00002D51 File Offset: 0x00000F51
			public WaitForStart(Tween tween)
			{
				this.t = tween;
			}

			// Token: 0x04000035 RID: 53
			private readonly Tween t;
		}
	}
}
