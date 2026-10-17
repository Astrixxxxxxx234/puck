using System;
using UnityEngine.UIElements;

// Token: 0x0200010C RID: 268
public class BeforeChildRemovedEvent : EventBase<BeforeChildRemovedEvent>
{
	// Token: 0x0400052C RID: 1324
	public int index;

	// Token: 0x0400052D RID: 1325
	public VisualElement child;
}
