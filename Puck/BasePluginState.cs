using System;

// Token: 0x020000EE RID: 238
public class BasePluginState
{
	// Token: 0x0600074F RID: 1871 RVA: 0x000272D1 File Offset: 0x000254D1
	public bool Equals(BasePluginState other)
	{
		return this.Path == other.Path && this.IsReady == other.IsReady && this.IsEnabled == other.IsEnabled;
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x00027304 File Offset: 0x00025504
	public override bool Equals(object obj)
	{
		BasePluginState basePluginState = obj as BasePluginState;
		return basePluginState != null && this.Equals(basePluginState);
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x00027324 File Offset: 0x00025524
	public override int GetHashCode()
	{
		return HashCode.Combine<string, bool, bool>(this.Path, this.IsReady, this.IsEnabled);
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x0002733D File Offset: 0x0002553D
	public override string ToString()
	{
		return string.Format("Path={0}, IsReady={1}, IsEnabled={2}", this.Path, this.IsReady, this.IsEnabled);
	}

	// Token: 0x040004B9 RID: 1209
	public string Path;

	// Token: 0x040004BA RID: 1210
	public bool IsReady;

	// Token: 0x040004BB RID: 1211
	public bool IsEnabled;
}
