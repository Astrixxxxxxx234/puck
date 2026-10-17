using System;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
	// Token: 0x02000006 RID: 6
	public class SerializedDictionarySampleThree : MonoBehaviour
	{
		// Token: 0x0400000C RID: 12
		[SerializeField]
		private SerializedDictionary<ScriptableObject, string> _nameOverrides;
	}
}
