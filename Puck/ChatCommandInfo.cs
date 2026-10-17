using System;

// Token: 0x02000091 RID: 145
public readonly struct ChatCommandInfo
{
	// Token: 0x060004BF RID: 1215 RVA: 0x0001A08F File Offset: 0x0001828F
	public ChatCommandInfo(string usage, string description, int requiredAdminLevel = 0)
	{
		this.Usage = usage;
		this.Description = description;
		this.RequiredAdminLevel = requiredAdminLevel;
	}

	// Token: 0x0400034F RID: 847
	public readonly string Usage;

	// Token: 0x04000350 RID: 848
	public readonly string Description;

	// Token: 0x04000351 RID: 849
	public readonly int RequiredAdminLevel;
}
