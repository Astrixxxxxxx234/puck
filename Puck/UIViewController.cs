using System;
using UnityEngine;

// Token: 0x020001AA RID: 426
public class UIViewController<T> : MonoBehaviour where T : UIView
{
	// Token: 0x06000D18 RID: 3352 RVA: 0x0003E26A File Offset: 0x0003C46A
	public virtual void Awake()
	{
		this.uiView = base.GetComponent<T>();
	}

	// Token: 0x06000D19 RID: 3353 RVA: 0x000023BA File Offset: 0x000005BA
	public virtual void OnDestroy()
	{
	}

	// Token: 0x0400082B RID: 2091
	private T uiView;
}
