using System;
using System.Collections.Generic;

// Token: 0x02000129 RID: 297
public class ReplayBindings
{
	// Token: 0x170000FA RID: 250
	// (get) Token: 0x060008CB RID: 2251 RVA: 0x0002B40A File Offset: 0x0002960A
	public List<ReplayPoseBinding> PoseBindings
	{
		get
		{
			return this.poseBindings;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x060008CC RID: 2252 RVA: 0x0002B412 File Offset: 0x00029612
	public List<ReplayPlayerBodyBinding> PlayerBodyBindings
	{
		get
		{
			return this.playerBodyBindings;
		}
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x0002B41C File Offset: 0x0002961C
	public void AddPose(SynchronizedObject synchronizedObject, ReplayTrack<ReplayPoseSample> track)
	{
		this.poseBindings.Add(new ReplayPoseBinding
		{
			SynchronizedObject = synchronizedObject,
			Track = track
		});
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x0002B450 File Offset: 0x00029650
	public void AddPlayerBody(PlayerBody playerBody, ReplayTrack<ReplayPlayerBodyStateSample> stateTrack, ReplayTrack<ReplayPlayerInputSample> inputTrack)
	{
		this.playerBodyBindings.Add(new ReplayPlayerBodyBinding
		{
			PlayerBody = playerBody,
			StateTrack = stateTrack,
			InputTrack = inputTrack
		});
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x0002B48C File Offset: 0x0002968C
	public void RemovePose(SynchronizedObject synchronizedObject)
	{
		for (int i = 0; i < this.poseBindings.Count; i++)
		{
			if (!(this.poseBindings[i].SynchronizedObject != synchronizedObject))
			{
				this.poseBindings.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x0002B4D8 File Offset: 0x000296D8
	public void RemovePlayerBody(PlayerBody playerBody)
	{
		this.RemovePose(playerBody.SynchronizedObject);
		for (int i = 0; i < this.playerBodyBindings.Count; i++)
		{
			if (!(this.playerBodyBindings[i].PlayerBody != playerBody))
			{
				this.playerBodyBindings.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x0002B52D File Offset: 0x0002972D
	public void Clear()
	{
		this.poseBindings.Clear();
		this.playerBodyBindings.Clear();
	}

	// Token: 0x04000573 RID: 1395
	private List<ReplayPoseBinding> poseBindings = new List<ReplayPoseBinding>();

	// Token: 0x04000574 RID: 1396
	private List<ReplayPlayerBodyBinding> playerBodyBindings = new List<ReplayPlayerBodyBinding>();
}
