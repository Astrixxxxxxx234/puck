using System;
using System.Diagnostics;

// Token: 0x02000107 RID: 263
public static class PatchManager
{
	// Token: 0x06000832 RID: 2098 RVA: 0x000299A8 File Offset: 0x00027BA8
	public static void Initialize()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		VisualElementHarmonyPatch.Patch();
		stopwatch.Stop();
		PatchManager.Logger.Info(string.Format("Patching took {0}ms", stopwatch.ElapsedMilliseconds));
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x000299E8 File Offset: 0x00027BE8
	public static void Dispose()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		VisualElementHarmonyPatch.Unpatch();
		stopwatch.Stop();
		PatchManager.Logger.Info(string.Format("Unpatching took {0}ms", stopwatch.ElapsedMilliseconds));
	}

	// Token: 0x04000523 RID: 1315
	private static readonly Logger Logger = new Logger("PatchManager");
}
