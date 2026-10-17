using System;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x020001A9 RID: 425
public class UIView : MonoBehaviour
{
	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000D0A RID: 3338 RVA: 0x0003E0C7 File Offset: 0x0003C2C7
	// (set) Token: 0x06000D0B RID: 3339 RVA: 0x0003E0D0 File Offset: 0x0003C2D0
	public bool IsVisible
	{
		get
		{
			return this.isVisible;
		}
		set
		{
			if (this.isVisible == value)
			{
				return;
			}
			bool oldIsVisible = this.isVisible;
			this.isVisible = value;
			this.OnIsVisibileChanged(oldIsVisible, this.isVisible);
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000D0C RID: 3340 RVA: 0x0003E102 File Offset: 0x0003C302
	// (set) Token: 0x06000D0D RID: 3341 RVA: 0x0003E10C File Offset: 0x0003C30C
	public bool IsFocused
	{
		get
		{
			return this.isFocused;
		}
		set
		{
			if (this.isFocused == value)
			{
				return;
			}
			bool oldIsFocused = this.isFocused;
			this.isFocused = value;
			this.OnIsFocusedChanged(oldIsFocused, this.isFocused);
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000D0E RID: 3342 RVA: 0x0003E140 File Offset: 0x0003C340
	[HideInInspector]
	public int Order
	{
		get
		{
			if (!(this.View.parent is TemplateContainer))
			{
				return this.View.parent.IndexOf(this.View);
			}
			return this.View.parent.parent.IndexOf(this.View.parent);
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06000D0F RID: 3343 RVA: 0x0003E196 File Offset: 0x0003C396
	// (set) Token: 0x06000D10 RID: 3344 RVA: 0x0003E1A0 File Offset: 0x0003C3A0
	public VisualElement View
	{
		get
		{
			return this.view;
		}
		set
		{
			if (this.view == value)
			{
				return;
			}
			VisualElement oldView = this.view;
			this.view = value;
			this.OnViewChanged(oldView, this.view);
		}
	}

	// Token: 0x06000D11 RID: 3345 RVA: 0x0003E1D2 File Offset: 0x0003C3D2
	public virtual bool Show()
	{
		if (this.IsVisible)
		{
			return false;
		}
		this.IsVisible = true;
		return true;
	}

	// Token: 0x06000D12 RID: 3346 RVA: 0x0003E1E6 File Offset: 0x0003C3E6
	public virtual bool Hide()
	{
		if (!this.IsVisible || this.AlwaysVisible)
		{
			return false;
		}
		this.IsVisible = false;
		return true;
	}

	// Token: 0x06000D13 RID: 3347 RVA: 0x0003E202 File Offset: 0x0003C402
	public virtual bool Toggle()
	{
		if (this.IsVisible)
		{
			return this.Hide();
		}
		return this.Show();
	}

	// Token: 0x06000D14 RID: 3348 RVA: 0x0003E219 File Offset: 0x0003C419
	private void OnIsVisibileChanged(bool oldIsVisible, bool newIsVisible)
	{
		this.View.style.display = (newIsVisible ? DisplayStyle.Flex : DisplayStyle.None);
		Action<UIView> onVisibility = this.OnVisibility;
		if (onVisibility == null)
		{
			return;
		}
		onVisibility(this);
	}

	// Token: 0x06000D15 RID: 3349 RVA: 0x0003E248 File Offset: 0x0003C448
	private void OnIsFocusedChanged(bool oldIsFocused, bool newIsFocused)
	{
		Action<UIView> onFocus = this.OnFocus;
		if (onFocus == null)
		{
			return;
		}
		onFocus(this);
	}

	// Token: 0x06000D16 RID: 3350 RVA: 0x000023BA File Offset: 0x000005BA
	private void OnViewChanged(VisualElement oldView, VisualElement newView)
	{
	}

	// Token: 0x04000820 RID: 2080
	[Header("UI Settings")]
	public bool FocusRequiresMouse;

	// Token: 0x04000821 RID: 2081
	public bool FocusIsInteractive;

	// Token: 0x04000822 RID: 2082
	public bool VisibilityRequiresMouse;

	// Token: 0x04000823 RID: 2083
	public bool VisibilityIsInteractive;

	// Token: 0x04000824 RID: 2084
	public bool AlwaysVisible;

	// Token: 0x04000825 RID: 2085
	private bool isVisible = true;

	// Token: 0x04000826 RID: 2086
	private bool isFocused;

	// Token: 0x04000827 RID: 2087
	[HideInInspector]
	public Action<UIView> OnVisibility;

	// Token: 0x04000828 RID: 2088
	[HideInInspector]
	public Action<UIView> OnFocus;

	// Token: 0x04000829 RID: 2089
	[HideInInspector]
	public VisualElement RootVisualElement;

	// Token: 0x0400082A RID: 2090
	private VisualElement view;
}
