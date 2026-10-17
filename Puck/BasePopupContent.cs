using System;
using UnityEngine.UIElements;

// Token: 0x020001F4 RID: 500
public class BasePopupContent
{
	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000F93 RID: 3987 RVA: 0x00049D74 File Offset: 0x00047F74
	// (set) Token: 0x06000F94 RID: 3988 RVA: 0x00049D7C File Offset: 0x00047F7C
	public VisualElement VisualElement { get; set; }

	// Token: 0x06000F95 RID: 3989 RVA: 0x00049D85 File Offset: 0x00047F85
	public BasePopupContent(VisualTreeAsset asset)
	{
		this.asset = asset;
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x00049D94 File Offset: 0x00047F94
	public virtual void Initialize()
	{
		this.VisualElement = this.asset.Instantiate();
		Action initialized = this.Initialized;
		if (initialized == null)
		{
			return;
		}
		initialized();
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x00049DB7 File Offset: 0x00047FB7
	public virtual void Dispose()
	{
		Action disposed = this.Disposed;
		if (disposed == null)
		{
			return;
		}
		disposed();
	}

	// Token: 0x06000F98 RID: 3992 RVA: 0x000023BA File Offset: 0x000005BA
	internal virtual void Update()
	{
	}

	// Token: 0x040009DD RID: 2525
	public Action Initialized;

	// Token: 0x040009DE RID: 2526
	public Action Disposed;

	// Token: 0x040009DF RID: 2527
	private VisualTreeAsset asset;
}
