using System;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
	// Token: 0x02000004 RID: 4
	public class SerializedDictionarySample : MonoBehaviour
	{
		// Token: 0x04000006 RID: 6
		public SerializedDictionary<SerializedDictionarySample.ElementType, string> ElementDescriptions;

		// Token: 0x02000005 RID: 5
		public enum ElementType
		{
			// Token: 0x04000008 RID: 8
			Fire,
			// Token: 0x04000009 RID: 9
			Air,
			// Token: 0x0400000A RID: 10
			Earth,
			// Token: 0x0400000B RID: 11
			Water
		}
	}
}
