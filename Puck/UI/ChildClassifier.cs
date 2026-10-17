using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

namespace UI
{
	// Token: 0x0200028C RID: 652
	[UxmlElement]
	public class ChildClassifier : VisualElement
	{
		// Token: 0x060013B2 RID: 5042 RVA: 0x00057284 File Offset: 0x00055484
		public ChildClassifier()
		{
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.NoTrickleDown);
			base.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnDetachFromPanel), TrickleDown.NoTrickleDown);
			base.RegisterCallback<ChildAddedEvent>(new EventCallback<ChildAddedEvent>(this.OnChildAdded), TrickleDown.NoTrickleDown);
			base.RegisterCallback<BeforeChildRemovedEvent>(new EventCallback<BeforeChildRemovedEvent>(this.OnBeforeChildRemoved), TrickleDown.NoTrickleDown);
			base.RegisterCallback<HierarchyChangedEvent>(new EventCallback<HierarchyChangedEvent>(this.OnHierarchyChanged), TrickleDown.NoTrickleDown);
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x000572F6 File Offset: 0x000554F6
		private List<VisualElement> GetChildren()
		{
			return base.Children().ToList<VisualElement>();
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x00057303 File Offset: 0x00055503
		private List<VisualElement> GetVisibleChildren()
		{
			return base.Children().ToList<VisualElement>().FindAll((VisualElement child) => child.resolvedStyle.display == DisplayStyle.Flex);
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00057334 File Offset: 0x00055534
		private void SetFirstChild(VisualElement child)
		{
			if (this.firstChild != null)
			{
				this.DisposeChild(this.firstChild);
			}
			this.firstChild = child;
			this.firstChild.EnableInClassList("firstChild", true);
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x00057362 File Offset: 0x00055562
		private void SetLastChild(VisualElement child)
		{
			if (this.lastChild != null)
			{
				this.DisposeChild(this.lastChild);
			}
			this.lastChild = child;
			this.lastChild.EnableInClassList("lastChild", true);
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00057390 File Offset: 0x00055590
		private void SetChildClasses(List<VisualElement> children)
		{
			if (children.Count == 0)
			{
				return;
			}
			if (children.Count == 1)
			{
				this.SetLastChild(children[0]);
				return;
			}
			this.SetFirstChild(children[0]);
			this.SetLastChild(children[children.Count - 1]);
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x000573DE File Offset: 0x000555DE
		private void DisposeChild(VisualElement child)
		{
			child.EnableInClassList("firstChild", false);
			child.EnableInClassList("lastChild", false);
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x000573F8 File Offset: 0x000555F8
		private void OnAttachToPanel(AttachToPanelEvent e)
		{
			this.GetChildren().ForEach(delegate(VisualElement child)
			{
				child.RegisterCallback<RenderingToggledEvent>(new EventCallback<RenderingToggledEvent>(this.OnChildRenderingToggled), TrickleDown.NoTrickleDown);
			});
			List<VisualElement> visibleChildren = this.GetVisibleChildren();
			this.SetChildClasses(visibleChildren);
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0005742A File Offset: 0x0005562A
		private void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			this.GetChildren().ForEach(delegate(VisualElement child)
			{
				child.UnregisterCallback<RenderingToggledEvent>(new EventCallback<RenderingToggledEvent>(this.OnChildRenderingToggled), TrickleDown.NoTrickleDown);
			});
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00057444 File Offset: 0x00055644
		private void OnChildAdded(ChildAddedEvent e)
		{
			int index = e.index;
			e.child.RegisterCallback<RenderingToggledEvent>(new EventCallback<RenderingToggledEvent>(this.OnChildRenderingToggled), TrickleDown.NoTrickleDown);
			List<VisualElement> visibleChildren = this.GetVisibleChildren();
			if (index == 0 || index == visibleChildren.Count - 1)
			{
				this.SetChildClasses(visibleChildren);
			}
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x0005748C File Offset: 0x0005568C
		private void OnBeforeChildRemoved(BeforeChildRemovedEvent e)
		{
			int index = e.index;
			VisualElement child = e.child;
			child.UnregisterCallback<RenderingToggledEvent>(new EventCallback<RenderingToggledEvent>(this.OnChildRenderingToggled), TrickleDown.NoTrickleDown);
			List<VisualElement> visibleChildren = this.GetVisibleChildren();
			if (index == 0 || index == visibleChildren.Count - 1)
			{
				List<VisualElement> childClasses = this.GetVisibleChildren().FindAll((VisualElement c) => c != child);
				this.SetChildClasses(childClasses);
			}
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x00057500 File Offset: 0x00055700
		private void OnHierarchyChanged(HierarchyChangedEvent e)
		{
			List<VisualElement> visibleChildren = this.GetVisibleChildren();
			this.SetChildClasses(visibleChildren);
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x0005751C File Offset: 0x0005571C
		private void OnChildRenderingToggled(RenderingToggledEvent e)
		{
			List<VisualElement> visibleChildren = this.GetVisibleChildren();
			this.SetChildClasses(visibleChildren);
		}

		// Token: 0x04000C57 RID: 3159
		private VisualElement firstChild;

		// Token: 0x04000C58 RID: 3160
		private VisualElement lastChild;

		// Token: 0x0200028D RID: 653
		[CompilerGenerated]
		[Serializable]
		public new class UxmlSerializedData : VisualElement.UxmlSerializedData
		{
			// Token: 0x060013C1 RID: 5057 RVA: 0x00057561 File Offset: 0x00055761
			public override object CreateInstance()
			{
				return new ChildClassifier();
			}
		}
	}
}
