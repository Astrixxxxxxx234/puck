using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200018B RID: 395
public class SynchronizedObjectSnapshot
{
	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0003A4FC File Offset: 0x000386FC
	public int Count
	{
		get
		{
			return this.data.Count;
		}
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x0003A509 File Offset: 0x00038709
	public Vector3 GetPosition(int index)
	{
		return this.positions[index];
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x0003A517 File Offset: 0x00038717
	public bool GetUsesLod(int index)
	{
		return this.objects[index].UseLod;
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x0003A52A File Offset: 0x0003872A
	public bool GetUsesCulling(int index)
	{
		return this.objects[index].UseCulling;
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x0003A53D File Offset: 0x0003873D
	public SynchronizedObjectData GetData(int index)
	{
		return this.data[index];
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x0003A54C File Offset: 0x0003874C
	public void Capture(SynchronizedObjectRegistry registry)
	{
		this.objects.Clear();
		this.positions.Clear();
		this.data.Clear();
		foreach (SynchronizedObject synchronizedObject in registry.Objects)
		{
			if (synchronizedObject)
			{
				ulong networkObjectId = synchronizedObject.NetworkObjectId;
				if (networkObjectId > 65535UL)
				{
					SynchronizedObjectSnapshot.Logger.Error(string.Format("NetworkObjectId {0} does not fit in a synchronization record and was skipped", networkObjectId));
				}
				else
				{
					SynchronizedObjectPose pose = synchronizedObject.GetPose();
					this.objects.Add(synchronizedObject);
					this.positions.Add(pose.Position);
					this.data.Add(new SynchronizedObjectData(networkObjectId, pose.Position, pose.Rotation, pose.LinearVelocity, pose.AngularVelocity));
				}
			}
		}
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x0003A640 File Offset: 0x00038840
	public void Clear()
	{
		this.objects.Clear();
		this.positions.Clear();
		this.data.Clear();
	}

	// Token: 0x04000791 RID: 1937
	private static readonly global::Logger Logger = new global::Logger("SynchronizedObjectSnapshot");

	// Token: 0x04000792 RID: 1938
	private List<SynchronizedObject> objects = new List<SynchronizedObject>();

	// Token: 0x04000793 RID: 1939
	private List<Vector3> positions = new List<Vector3>();

	// Token: 0x04000794 RID: 1940
	private List<SynchronizedObjectData> data = new List<SynchronizedObjectData>();
}
