using System;
using System.Collections.Generic;

// Token: 0x020000F9 RID: 249
public class Plugin : BasePlugin<BasePluginState>
{
	// Token: 0x060007A4 RID: 1956 RVA: 0x0002853A File Offset: 0x0002673A
	public Plugin(string id, string path) : base(new BasePluginState
	{
		Path = path,
		IsEnabled = false
	})
	{
		this.Id = id;
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x0002855C File Offset: 0x0002675C
	public override void Initialize()
	{
		base.Initialize();
		this.SetState(new Dictionary<string, object>
		{
			{
				"isReady",
				true
			}
		});
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x00028580 File Offset: 0x00026780
	public override void OnEnableFailed(Exception exception)
	{
		base.OnEnableFailed(exception);
		Plugin.Logger.Error("Failed to enable plugin " + this.Id + ": " + exception.Message);
		EventManager.TriggerEvent("Event_OnPluginEnableFailed", new Dictionary<string, object>
		{
			{
				"plugin",
				this
			}
		});
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x000285D4 File Offset: 0x000267D4
	public override void OnDisableFailed(Exception exception)
	{
		base.OnDisableFailed(exception);
		Plugin.Logger.Error("Failed to disable plugin " + this.Id + ": " + exception.Message);
		EventManager.TriggerEvent("Event_OnPluginDisableFailed", new Dictionary<string, object>
		{
			{
				"plugin",
				this
			}
		});
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x00028628 File Offset: 0x00026828
	protected override void OnStateChanged(BasePluginState oldState, BasePluginState newState)
	{
		base.OnStateChanged(oldState, newState);
		EventManager.TriggerEvent("Event_OnPluginStateChanged", new Dictionary<string, object>
		{
			{
				"plugin",
				this
			},
			{
				"oldState",
				oldState
			},
			{
				"newState",
				newState
			}
		});
	}

	// Token: 0x040004D5 RID: 1237
	private static readonly Logger Logger = new Logger("Plugin");

	// Token: 0x040004D6 RID: 1238
	public readonly string Id;
}
