using System;

// Token: 0x02000017 RID: 23
public class LockerRoomCamera : BaseCamera
{
	// Token: 0x06000062 RID: 98 RVA: 0x000030C2 File Offset: 0x000012C2
	public override void Awake()
	{
		base.Awake();
		this.smoothPositioner = base.GetComponent<SmoothPositioner>();
	}

	// Token: 0x06000063 RID: 99 RVA: 0x000030D6 File Offset: 0x000012D6
	public void SetPosition(string positionName)
	{
		this.smoothPositioner.SetPosition(positionName, false);
	}

	// Token: 0x06000065 RID: 101 RVA: 0x000030F0 File Offset: 0x000012F0
	protected override void __initializeVariables()
	{
		base.__initializeVariables();
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00003106 File Offset: 0x00001306
	protected override void __initializeRpcs()
	{
		base.__initializeRpcs();
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00003110 File Offset: 0x00001310
	protected internal override string __getTypeName()
	{
		return "LockerRoomCamera";
	}

	// Token: 0x0400003A RID: 58
	private SmoothPositioner smoothPositioner;
}
