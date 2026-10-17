using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine.UIElements;

// Token: 0x0200010E RID: 270
public static class VisualElementHarmonyPatch
{
	// Token: 0x0600083A RID: 2106 RVA: 0x00029A5E File Offset: 0x00027C5E
	private static bool IsEditor(VisualElement element)
	{
		return element != null && element.panel != null && element.panel.contextType == ContextType.Editor;
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x00029A7D File Offset: 0x00027C7D
	public static void Patch()
	{
		VisualElementHarmonyPatch.Logger.Info("Applying patches");
		VisualElementHarmonyPatch.harmony.PatchAll();
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x00029A98 File Offset: 0x00027C98
	public static void Unpatch()
	{
		VisualElementHarmonyPatch.Logger.Info("Removing patches");
		VisualElementHarmonyPatch.harmony.UnpatchSelf();
	}

	// Token: 0x0400052F RID: 1327
	private static readonly Logger Logger = new Logger("VisualElementHarmonyPatch");

	// Token: 0x04000530 RID: 1328
	private static readonly Harmony harmony = new Harmony("Puck.VisualElement");

	// Token: 0x0200010F RID: 271
	[HarmonyPatch(typeof(VisualElement), "IncrementVersion")]
	private static class IncrementVersionPatch
	{
		// Token: 0x0600083E RID: 2110 RVA: 0x00029AD4 File Offset: 0x00027CD4
		[HarmonyPostfix]
		public static void Postfix(VisualElement __instance, VersionChangeType changeType)
		{
			if (VisualElementHarmonyPatch.IsEditor(__instance))
			{
				return;
			}
			if (changeType == VersionChangeType.Hierarchy)
			{
				HierarchyChangedEvent pooled = EventBase<HierarchyChangedEvent>.GetPooled();
				pooled.target = __instance;
				__instance.SendEvent(pooled);
				return;
			}
			if (changeType == VersionChangeType.DisableRendering)
			{
				RenderingToggledEvent pooled2 = EventBase<RenderingToggledEvent>.GetPooled();
				pooled2.target = __instance;
				__instance.SendEvent(pooled2);
				return;
			}
		}
	}

	// Token: 0x02000110 RID: 272
	[HarmonyPatch(typeof(VisualElement.Hierarchy), "PutChildAtIndex")]
	private static class PutChildAtIndexPatch
	{
		// Token: 0x0600083F RID: 2111 RVA: 0x00029B20 File Offset: 0x00027D20
		[HarmonyPostfix]
		public static void Postfix(VisualElement.Hierarchy __instance, VisualElement child, int index)
		{
			FieldInfo fieldInfo = AccessTools.Field(typeof(VisualElement.Hierarchy), "m_Owner");
			VisualElement visualElement = ((fieldInfo != null) ? fieldInfo.GetValue(__instance) : null) as VisualElement;
			if (VisualElementHarmonyPatch.IsEditor(visualElement))
			{
				return;
			}
			if (visualElement == null)
			{
				return;
			}
			ChildAddedEvent pooled = EventBase<ChildAddedEvent>.GetPooled();
			pooled.index = index;
			pooled.child = child;
			pooled.target = visualElement;
			visualElement.SendEvent(pooled);
		}
	}

	// Token: 0x02000111 RID: 273
	[HarmonyPatch(typeof(VisualElement.Hierarchy), "RemoveChildAtIndex")]
	private static class RemoveChildAtIndexPatch
	{
		// Token: 0x06000840 RID: 2112 RVA: 0x00029B88 File Offset: 0x00027D88
		[HarmonyPrefix]
		public static bool Prefix(VisualElement.Hierarchy __instance, int index)
		{
			FieldInfo fieldInfo = AccessTools.Field(typeof(VisualElement.Hierarchy), "m_Owner");
			VisualElement visualElement = ((fieldInfo != null) ? fieldInfo.GetValue(__instance) : null) as VisualElement;
			if (VisualElementHarmonyPatch.IsEditor(visualElement))
			{
				return true;
			}
			if (visualElement == null || index < 0 || index >= visualElement.childCount)
			{
				return true;
			}
			try
			{
				BeforeChildRemovedEvent pooled = EventBase<BeforeChildRemovedEvent>.GetPooled();
				pooled.index = index;
				pooled.child = visualElement.ElementAt(index);
				pooled.target = visualElement;
				visualElement.SendEvent(pooled);
			}
			catch (Exception arg)
			{
				VisualElementHarmonyPatch.Logger.Error(string.Format("Exception in BeforeChildRemovedEvent on {0}: {1}", ((visualElement != null) ? visualElement.name : null) ?? "null", arg));
			}
			return true;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00029C48 File Offset: 0x00027E48
		[HarmonyPostfix]
		public static void Postfix(VisualElement.Hierarchy __instance, int index)
		{
			FieldInfo fieldInfo = AccessTools.Field(typeof(VisualElement.Hierarchy), "m_Owner");
			VisualElement visualElement = ((fieldInfo != null) ? fieldInfo.GetValue(__instance) : null) as VisualElement;
			if (VisualElementHarmonyPatch.IsEditor(visualElement))
			{
				return;
			}
			if (visualElement == null)
			{
				return;
			}
			ChildRemovedEvent pooled = EventBase<ChildRemovedEvent>.GetPooled();
			pooled.index = index;
			pooled.target = visualElement;
			visualElement.SendEvent(pooled);
		}
	}
}
