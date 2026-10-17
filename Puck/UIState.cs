using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020000CF RID: 207
public struct UIState
{
	// Token: 0x1700009E RID: 158
	// (get) Token: 0x06000699 RID: 1689 RVA: 0x00024436 File Offset: 0x00022636
	public bool IsInteracting
	{
		get
		{
			return this.InteractingViews != null && this.InteractingViews.Count > 0;
		}
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x00024450 File Offset: 0x00022650
	public UIState()
	{
		this.Phase = UIPhase.None;
		this.IsMouseRequired = false;
		this.IsMouseOverUI = false;
		this.InteractingViews = new List<UIView>();
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x00024474 File Offset: 0x00022674
	public bool IsViewInteracting<T>() where T : UIView
	{
		if (this.InteractingViews == null)
		{
			return false;
		}
		using (List<UIView>.Enumerator enumerator = this.InteractingViews.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is T)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x000244D8 File Offset: 0x000226D8
	public bool IsViewTopmostInteracting<T>() where T : UIView
	{
		return this.InteractingViews != null && this.InteractingViews.Count != 0 && this.InteractingViews[this.InteractingViews.Count - 1] is T;
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x00024511 File Offset: 0x00022711
	public UIView GetTopmostInteractingView()
	{
		if (this.InteractingViews == null || this.InteractingViews.Count == 0)
		{
			return null;
		}
		return this.InteractingViews[this.InteractingViews.Count - 1];
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x00024542 File Offset: 0x00022742
	public bool Equals(UIState other)
	{
		return this.Phase == other.Phase && this.IsMouseRequired == other.IsMouseRequired && this.IsMouseOverUI == other.IsMouseOverUI && this.InteractingViews.SequenceEqual(other.InteractingViews);
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x00024584 File Offset: 0x00022784
	public override bool Equals(object obj)
	{
		if (obj is UIState)
		{
			UIState other = (UIState)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x000245A9 File Offset: 0x000227A9
	public override int GetHashCode()
	{
		return HashCode.Combine<UIPhase, bool, bool, List<UIView>>(this.Phase, this.IsMouseRequired, this.IsMouseOverUI, this.InteractingViews);
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x000245C8 File Offset: 0x000227C8
	public override string ToString()
	{
		return string.Format("Phase: {0}, IsMouseRequired: {1}, IsMouseOverUI: {2}, IsInteracting: {3}", new object[]
		{
			this.Phase,
			this.IsMouseRequired,
			this.IsMouseOverUI,
			this.IsInteracting
		});
	}

	// Token: 0x0400043B RID: 1083
	public UIPhase Phase;

	// Token: 0x0400043C RID: 1084
	public bool IsMouseRequired;

	// Token: 0x0400043D RID: 1085
	public bool IsMouseOverUI;

	// Token: 0x0400043E RID: 1086
	public List<UIView> InteractingViews;
}
