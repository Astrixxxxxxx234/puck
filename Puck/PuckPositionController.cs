using System;
using UnityEngine;

// Token: 0x02000070 RID: 112
public class PuckPositionController : MonoBehaviour
{
	// Token: 0x0600037A RID: 890 RVA: 0x00015949 File Offset: 0x00013B49
	private void Awake()
	{
		this.puckPosition = base.GetComponent<PuckPosition>();
	}

	// Token: 0x04000297 RID: 663
	private PuckPosition puckPosition;
}
