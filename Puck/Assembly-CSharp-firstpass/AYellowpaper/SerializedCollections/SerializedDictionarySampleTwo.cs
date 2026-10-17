using System;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
	// Token: 0x02000007 RID: 7
	public class SerializedDictionarySampleTwo : MonoBehaviour
	{
		// Token: 0x0400000D RID: 13
		public SerializedDictionary<int, SerializedDictionarySampleTwo.Person> People;

		// Token: 0x02000008 RID: 8
		[Serializable]
		public class Person
		{
			// Token: 0x0400000E RID: 14
			public string FirstName;

			// Token: 0x0400000F RID: 15
			public string LastName;
		}
	}
}
