using System;
using UnityEngine.UIElements;

// Token: 0x0200010B RID: 267
public class ChildAddedEvent : EventBase<ChildAddedEvent>
{
	// Token: 0x0400052A RID: 1322
	public int index;

	// Token: 0x0400052B RID: 1323
	public VisualElement child;
}
