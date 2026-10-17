using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

// Token: 0x020000EF RID: 239
public abstract class BasePlugin<T> where T : BasePluginState, new()
{
	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000753 RID: 1875 RVA: 0x00027365 File Offset: 0x00025565
	// (set) Token: 0x06000754 RID: 1876 RVA: 0x00027370 File Offset: 0x00025570
	public T State
	{
		get
		{
			return this.state;
		}
		set
		{
			if (this.state.Equals(value))
			{
				return;
			}
			T oldState = this.state;
			this.state = value;
			this.OnStateChanged(oldState, this.state);
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06000755 RID: 1877 RVA: 0x000273B1 File Offset: 0x000255B1
	public string Path
	{
		get
		{
			return this.state.Path;
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x06000756 RID: 1878 RVA: 0x000273C3 File Offset: 0x000255C3
	public bool IsReady
	{
		get
		{
			return this.state.IsReady;
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x06000757 RID: 1879 RVA: 0x000273D5 File Offset: 0x000255D5
	public bool IsEnabled
	{
		get
		{
			return this.state.IsEnabled;
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06000758 RID: 1880 RVA: 0x000273E7 File Offset: 0x000255E7
	public bool HasAssembly
	{
		get
		{
			return this.assemblyPath != null;
		}
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x000273F2 File Offset: 0x000255F2
	public BasePlugin(T state)
	{
		this.state = state;
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00027401 File Offset: 0x00025601
	public virtual void Initialize()
	{
		this.assemblyPath = this.GetAssemblyPath();
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x0002740F File Offset: 0x0002560F
	public virtual void Dispose()
	{
		if (this.IsEnabled)
		{
			this.Disable();
		}
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x00027420 File Offset: 0x00025620
	public virtual void SetState(Dictionary<string, object> updates)
	{
		T t = Activator.CreateInstance<T>();
		t.Path = (updates.ContainsKey("path") ? ((string)updates["path"]) : this.state.Path);
		t.IsReady = (updates.ContainsKey("isReady") ? ((bool)updates["isReady"]) : this.state.IsReady);
		t.IsEnabled = (updates.ContainsKey("isEnabled") ? ((bool)updates["isEnabled"]) : this.state.IsEnabled);
		T t2 = t;
		this.State = t2;
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x000274E8 File Offset: 0x000256E8
	private string GetAssemblyPath()
	{
		if (this.Path == null || !Directory.Exists(this.Path))
		{
			return null;
		}
		return Directory.GetFiles(this.Path, "*.dll", SearchOption.TopDirectoryOnly).FirstOrDefault<string>();
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x00027518 File Offset: 0x00025718
	private void LoadAssembly(string path)
	{
		if (this.instance != null)
		{
			return;
		}
		this.assembly = Assembly.LoadFile(path);
		Type type2 = this.assembly.GetTypes().FirstOrDefault((Type type) => type.IsClass && !type.IsAbstract && typeof(IPuckPlugin).IsAssignableFrom(type));
		if (type2 == null)
		{
			throw new Exception("IPuckPlugin missing from assembly");
		}
		this.instance = Activator.CreateInstance(type2);
		this.onEnableMethod = type2.GetMethod("OnEnable");
		this.onDisableMethod = type2.GetMethod("OnDisable");
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x000275AC File Offset: 0x000257AC
	private void UnloadAssembly()
	{
		this.instance = null;
		this.assembly = null;
		this.onEnableMethod = null;
		this.onDisableMethod = null;
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x000275CC File Offset: 0x000257CC
	public bool Enable()
	{
		if (this.IsEnabled)
		{
			return false;
		}
		if (!this.IsReady)
		{
			return false;
		}
		try
		{
			if (this.HasAssembly)
			{
				this.LoadAssembly(this.assemblyPath);
				if (!(bool)this.onEnableMethod.Invoke(this.instance, null))
				{
					throw new Exception("OnEnable returned false");
				}
			}
			this.SetState(new Dictionary<string, object>
			{
				{
					"isEnabled",
					true
				}
			});
		}
		catch (Exception exception)
		{
			this.OnEnableFailed(exception);
			return false;
		}
		return true;
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x00027664 File Offset: 0x00025864
	public bool Disable()
	{
		if (!this.IsEnabled)
		{
			return false;
		}
		try
		{
			if (this.instance != null)
			{
				if (!(bool)this.onDisableMethod.Invoke(this.instance, null))
				{
					throw new Exception("OnDisable returned false");
				}
				this.UnloadAssembly();
			}
			this.SetState(new Dictionary<string, object>
			{
				{
					"isEnabled",
					false
				}
			});
		}
		catch (Exception exception)
		{
			this.OnDisableFailed(exception);
			return false;
		}
		return true;
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x000023BA File Offset: 0x000005BA
	public virtual void OnEnableFailed(Exception exception)
	{
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x000023BA File Offset: 0x000005BA
	public virtual void OnDisableFailed(Exception exception)
	{
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x000276EC File Offset: 0x000258EC
	protected virtual void OnStateChanged(T oldState, T newState)
	{
		if (oldState.Path != newState.Path)
		{
			this.assemblyPath = this.GetAssemblyPath();
		}
	}

	// Token: 0x040004BC RID: 1212
	private static readonly Logger Logger = new Logger("BasePlugin");

	// Token: 0x040004BD RID: 1213
	protected T state;

	// Token: 0x040004BE RID: 1214
	private string assemblyPath;

	// Token: 0x040004BF RID: 1215
	private Assembly assembly;

	// Token: 0x040004C0 RID: 1216
	private object instance;

	// Token: 0x040004C1 RID: 1217
	private MethodInfo onEnableMethod;

	// Token: 0x040004C2 RID: 1218
	private MethodInfo onDisableMethod;
}
