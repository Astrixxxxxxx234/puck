using System;

// Token: 0x02000285 RID: 645
public class Probe
{
	// Token: 0x170001CC RID: 460
	// (get) Token: 0x06001376 RID: 4982 RVA: 0x00056E69 File Offset: 0x00055069
	// (set) Token: 0x06001377 RID: 4983 RVA: 0x00056E71 File Offset: 0x00055071
	public string id { get; set; }

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x06001378 RID: 4984 RVA: 0x00056E7A File Offset: 0x0005507A
	// (set) Token: 0x06001379 RID: 4985 RVA: 0x00056E82 File Offset: 0x00055082
	public string providerId { get; set; }

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x0600137A RID: 4986 RVA: 0x00056E8B File Offset: 0x0005508B
	// (set) Token: 0x0600137B RID: 4987 RVA: 0x00056E93 File Offset: 0x00055093
	public string host { get; set; }

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x0600137C RID: 4988 RVA: 0x00056E9C File Offset: 0x0005509C
	// (set) Token: 0x0600137D RID: 4989 RVA: 0x00056EA4 File Offset: 0x000550A4
	public ushort port { get; set; }

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x0600137E RID: 4990 RVA: 0x00056EAD File Offset: 0x000550AD
	// (set) Token: 0x0600137F RID: 4991 RVA: 0x00056EB5 File Offset: 0x000550B5
	public float latitude { get; set; }

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x06001380 RID: 4992 RVA: 0x00056EBE File Offset: 0x000550BE
	// (set) Token: 0x06001381 RID: 4993 RVA: 0x00056EC6 File Offset: 0x000550C6
	public float longitude { get; set; }

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x06001382 RID: 4994 RVA: 0x00056ECF File Offset: 0x000550CF
	// (set) Token: 0x06001383 RID: 4995 RVA: 0x00056ED7 File Offset: 0x000550D7
	public string continent { get; set; }

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x06001384 RID: 4996 RVA: 0x00056EE0 File Offset: 0x000550E0
	// (set) Token: 0x06001385 RID: 4997 RVA: 0x00056EE8 File Offset: 0x000550E8
	public string country { get; set; }

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x06001386 RID: 4998 RVA: 0x00056EF1 File Offset: 0x000550F1
	// (set) Token: 0x06001387 RID: 4999 RVA: 0x00056EF9 File Offset: 0x000550F9
	public string city { get; set; }

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x06001388 RID: 5000 RVA: 0x00056F02 File Offset: 0x00055102
	// (set) Token: 0x06001389 RID: 5001 RVA: 0x00056F0A File Offset: 0x0005510A
	public int priority { get; set; }

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x0600138A RID: 5002 RVA: 0x00056F13 File Offset: 0x00055113
	// (set) Token: 0x0600138B RID: 5003 RVA: 0x00056F1B File Offset: 0x0005511B
	public int? freeCpuUnits { get; set; }

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x0600138C RID: 5004 RVA: 0x00056F24 File Offset: 0x00055124
	// (set) Token: 0x0600138D RID: 5005 RVA: 0x00056F2C File Offset: 0x0005512C
	public int? freeMemoryMb { get; set; }

	// Token: 0x0600138E RID: 5006 RVA: 0x00056F35 File Offset: 0x00055135
	public string ToLabel()
	{
		return (this.continent + ": " + this.city).ToUpper();
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x00056F54 File Offset: 0x00055154
	public override string ToString()
	{
		return string.Format("id: {0}, providerId: {1}, host: {2}, port: {3}, latitude: {4}, longitude: {5}, ", new object[]
		{
			this.id,
			this.providerId,
			this.host,
			this.port,
			this.latitude,
			this.longitude
		}) + string.Format("continent: {0}, country: {1}, city: {2}, priority: {3}, freeCpuUnits: {4}, freeMemoryMb: {5}", new object[]
		{
			this.continent,
			this.country,
			this.city,
			this.priority,
			this.freeCpuUnits,
			this.freeMemoryMb
		});
	}
}
