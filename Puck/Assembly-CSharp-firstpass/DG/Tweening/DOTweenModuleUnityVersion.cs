using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000009 RID: 9
	public static class DOTweenModuleUnityVersion
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000020C8 File Offset: 0x000002C8
		public static Sequence DOGradientColor(this Material target, Gradient gradient, float duration)
		{
			Sequence sequence = DOTween.Sequence();
			GradientColorKey[] colorKeys = gradient.colorKeys;
			int num = colorKeys.Length;
			for (int i = 0; i < num; i++)
			{
				GradientColorKey gradientColorKey = colorKeys[i];
				if (i == 0 && gradientColorKey.time <= 0f)
				{
					target.color = gradientColorKey.color;
				}
				else
				{
					float duration2 = (i == num - 1) ? (duration - sequence.Duration(false)) : (duration * ((i == 0) ? gradientColorKey.time : (gradientColorKey.time - colorKeys[i - 1].time)));
					sequence.Append(target.DOColor(gradientColorKey.color, duration2).SetEase(Ease.Linear));
				}
			}
			sequence.SetTarget(target);
			return sequence;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002180 File Offset: 0x00000380
		public static Sequence DOGradientColor(this Material target, Gradient gradient, string property, float duration)
		{
			Sequence sequence = DOTween.Sequence();
			GradientColorKey[] colorKeys = gradient.colorKeys;
			int num = colorKeys.Length;
			for (int i = 0; i < num; i++)
			{
				GradientColorKey gradientColorKey = colorKeys[i];
				if (i == 0 && gradientColorKey.time <= 0f)
				{
					target.SetColor(property, gradientColorKey.color);
				}
				else
				{
					float duration2 = (i == num - 1) ? (duration - sequence.Duration(false)) : (duration * ((i == 0) ? gradientColorKey.time : (gradientColorKey.time - colorKeys[i - 1].time)));
					sequence.Append(target.DOColor(gradientColorKey.color, property, duration2).SetEase(Ease.Linear));
				}
			}
			sequence.SetTarget(target);
			return sequence;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002237 File Offset: 0x00000437
		public static CustomYieldInstruction WaitForCompletion(this Tween t, bool returnCustomYieldInstruction)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return new DOTweenCYInstruction.WaitForCompletion(t);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002257 File Offset: 0x00000457
		public static CustomYieldInstruction WaitForRewind(this Tween t, bool returnCustomYieldInstruction)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return new DOTweenCYInstruction.WaitForRewind(t);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002277 File Offset: 0x00000477
		public static CustomYieldInstruction WaitForKill(this Tween t, bool returnCustomYieldInstruction)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return new DOTweenCYInstruction.WaitForKill(t);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002297 File Offset: 0x00000497
		public static CustomYieldInstruction WaitForElapsedLoops(this Tween t, int elapsedLoops, bool returnCustomYieldInstruction)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return new DOTweenCYInstruction.WaitForElapsedLoops(t, elapsedLoops);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022B8 File Offset: 0x000004B8
		public static CustomYieldInstruction WaitForPosition(this Tween t, float position, bool returnCustomYieldInstruction)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return new DOTweenCYInstruction.WaitForPosition(t, position);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022D9 File Offset: 0x000004D9
		public static CustomYieldInstruction WaitForStart(this Tween t, bool returnCustomYieldInstruction)
		{
			if (!t.active)
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogInvalidTween(t);
				}
				return null;
			}
			return new DOTweenCYInstruction.WaitForStart(t);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022FC File Offset: 0x000004FC
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOOffset(this Material target, Vector2 endValue, int propertyID, float duration)
		{
			if (!target.HasProperty(propertyID))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(propertyID);
				}
				return null;
			}
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.GetTextureOffset(propertyID), delegate(Vector2 x)
			{
				target.SetTextureOffset(propertyID, x);
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002374 File Offset: 0x00000574
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOTiling(this Material target, Vector2 endValue, int propertyID, float duration)
		{
			if (!target.HasProperty(propertyID))
			{
				if (Debugger.logPriority > 0)
				{
					Debugger.LogMissingMaterialProperty(propertyID);
				}
				return null;
			}
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => target.GetTextureScale(propertyID), delegate(Vector2 x)
			{
				target.SetTextureScale(propertyID, x);
			}, endValue, duration);
			tweenerCore.SetTarget(target);
			return tweenerCore;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000023EC File Offset: 0x000005EC
		public static Task AsyncWaitForCompletion(this Tween t)
		{
			DOTweenModuleUnityVersion.<AsyncWaitForCompletion>d__10 <AsyncWaitForCompletion>d__;
			<AsyncWaitForCompletion>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AsyncWaitForCompletion>d__.t = t;
			<AsyncWaitForCompletion>d__.<>1__state = -1;
			<AsyncWaitForCompletion>d__.<>t__builder.Start<DOTweenModuleUnityVersion.<AsyncWaitForCompletion>d__10>(ref <AsyncWaitForCompletion>d__);
			return <AsyncWaitForCompletion>d__.<>t__builder.Task;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002430 File Offset: 0x00000630
		public static Task AsyncWaitForRewind(this Tween t)
		{
			DOTweenModuleUnityVersion.<AsyncWaitForRewind>d__11 <AsyncWaitForRewind>d__;
			<AsyncWaitForRewind>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AsyncWaitForRewind>d__.t = t;
			<AsyncWaitForRewind>d__.<>1__state = -1;
			<AsyncWaitForRewind>d__.<>t__builder.Start<DOTweenModuleUnityVersion.<AsyncWaitForRewind>d__11>(ref <AsyncWaitForRewind>d__);
			return <AsyncWaitForRewind>d__.<>t__builder.Task;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002474 File Offset: 0x00000674
		public static Task AsyncWaitForKill(this Tween t)
		{
			DOTweenModuleUnityVersion.<AsyncWaitForKill>d__12 <AsyncWaitForKill>d__;
			<AsyncWaitForKill>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AsyncWaitForKill>d__.t = t;
			<AsyncWaitForKill>d__.<>1__state = -1;
			<AsyncWaitForKill>d__.<>t__builder.Start<DOTweenModuleUnityVersion.<AsyncWaitForKill>d__12>(ref <AsyncWaitForKill>d__);
			return <AsyncWaitForKill>d__.<>t__builder.Task;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000024B8 File Offset: 0x000006B8
		public static Task AsyncWaitForElapsedLoops(this Tween t, int elapsedLoops)
		{
			DOTweenModuleUnityVersion.<AsyncWaitForElapsedLoops>d__13 <AsyncWaitForElapsedLoops>d__;
			<AsyncWaitForElapsedLoops>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AsyncWaitForElapsedLoops>d__.t = t;
			<AsyncWaitForElapsedLoops>d__.elapsedLoops = elapsedLoops;
			<AsyncWaitForElapsedLoops>d__.<>1__state = -1;
			<AsyncWaitForElapsedLoops>d__.<>t__builder.Start<DOTweenModuleUnityVersion.<AsyncWaitForElapsedLoops>d__13>(ref <AsyncWaitForElapsedLoops>d__);
			return <AsyncWaitForElapsedLoops>d__.<>t__builder.Task;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002504 File Offset: 0x00000704
		public static Task AsyncWaitForPosition(this Tween t, float position)
		{
			DOTweenModuleUnityVersion.<AsyncWaitForPosition>d__14 <AsyncWaitForPosition>d__;
			<AsyncWaitForPosition>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AsyncWaitForPosition>d__.t = t;
			<AsyncWaitForPosition>d__.position = position;
			<AsyncWaitForPosition>d__.<>1__state = -1;
			<AsyncWaitForPosition>d__.<>t__builder.Start<DOTweenModuleUnityVersion.<AsyncWaitForPosition>d__14>(ref <AsyncWaitForPosition>d__);
			return <AsyncWaitForPosition>d__.<>t__builder.Task;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002550 File Offset: 0x00000750
		public static Task AsyncWaitForStart(this Tween t)
		{
			DOTweenModuleUnityVersion.<AsyncWaitForStart>d__15 <AsyncWaitForStart>d__;
			<AsyncWaitForStart>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AsyncWaitForStart>d__.t = t;
			<AsyncWaitForStart>d__.<>1__state = -1;
			<AsyncWaitForStart>d__.<>t__builder.Start<DOTweenModuleUnityVersion.<AsyncWaitForStart>d__15>(ref <AsyncWaitForStart>d__);
			return <AsyncWaitForStart>d__.<>t__builder.Task;
		}
	}
}
