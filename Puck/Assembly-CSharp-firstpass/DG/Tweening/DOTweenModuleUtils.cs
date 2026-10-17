using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Scripting;

namespace DG.Tweening
{
	// Token: 0x02000019 RID: 25
	public static class DOTweenModuleUtils
	{
		// Token: 0x06000035 RID: 53 RVA: 0x00002D60 File Offset: 0x00000F60
		[Preserve]
		public static void Init()
		{
			if (DOTweenModuleUtils._initialized)
			{
				return;
			}
			DOTweenModuleUtils._initialized = true;
			DOTweenExternalCommand.SetOrientationOnPath += DOTweenModuleUtils.Physics.SetOrientationOnPath;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002D81 File Offset: 0x00000F81
		[Preserve]
		private static void Preserver()
		{
			AppDomain.CurrentDomain.GetAssemblies();
			typeof(MonoBehaviour).GetMethod("Stub");
		}

		// Token: 0x04000036 RID: 54
		private static bool _initialized;

		// Token: 0x0200001A RID: 26
		public static class Physics
		{
			// Token: 0x06000037 RID: 55 RVA: 0x00002DA3 File Offset: 0x00000FA3
			public static void SetOrientationOnPath(PathOptions options, Tween t, Quaternion newRot, Transform trans)
			{
				trans.rotation = newRot;
			}

			// Token: 0x06000038 RID: 56 RVA: 0x00002DAC File Offset: 0x00000FAC
			public static bool HasRigidbody2D(Component target)
			{
				return false;
			}

			// Token: 0x06000039 RID: 57 RVA: 0x00002DAC File Offset: 0x00000FAC
			[Preserve]
			public static bool HasRigidbody(Component target)
			{
				return false;
			}

			// Token: 0x0600003A RID: 58 RVA: 0x00002DB0 File Offset: 0x00000FB0
			[Preserve]
			public static TweenerCore<Vector3, Path, PathOptions> CreateDOTweenPathTween(MonoBehaviour target, bool tweenRigidbody, bool isLocal, Path path, float duration, PathMode pathMode)
			{
				TweenerCore<Vector3, Path, PathOptions> result = null;
				if (!false)
				{
					result = (isLocal ? target.transform.DOLocalPath(path, duration, pathMode) : target.transform.DOPath(path, duration, pathMode));
				}
				return result;
			}
		}
	}
}
