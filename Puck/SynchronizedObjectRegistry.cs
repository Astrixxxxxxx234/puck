using System;
using System.Collections.Generic;

// Token: 0x02000184 RID: 388
public class SynchronizedObjectRegistry
{
	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06000C12 RID: 3090 RVA: 0x00039DAF File Offset: 0x00037FAF
	public List<SynchronizedObject> Objects
	{
		get
		{
			return this.objects;
		}
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x00039DB7 File Offset: 0x00037FB7
	public void Add(SynchronizedObject synchronizedObject)
	{
		this.objects.Add(synchronizedObject);
		this.networkObjectIdObjectMap[synchronizedObject.NetworkObjectId] = synchronizedObject;
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x00039DD7 File Offset: 0x00037FD7
	public void Remove(SynchronizedObject synchronizedObject)
	{
		this.objects.Remove(synchronizedObject);
		this.networkObjectIdObjectMap.Remove(synchronizedObject.NetworkObjectId);
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x00039DF8 File Offset: 0x00037FF8
	public bool TryGet(ulong networkObjectId, out SynchronizedObject synchronizedObject)
	{
		return this.networkObjectIdObjectMap.TryGetValue(networkObjectId, out synchronizedObject) && synchronizedObject != null;
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x00039E13 File Offset: 0x00038013
	public void Clear()
	{
		this.objects.Clear();
		this.networkObjectIdObjectMap.Clear();
	}

	// Token: 0x04000774 RID: 1908
	private List<SynchronizedObject> objects = new List<SynchronizedObject>();

	// Token: 0x04000775 RID: 1909
	private Dictionary<ulong, SynchronizedObject> networkObjectIdObjectMap = new Dictionary<ulong, SynchronizedObject>();
}
