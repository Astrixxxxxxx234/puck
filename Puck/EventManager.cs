using System;
using System.Collections.Generic;
using Unity.Netcode;

// Token: 0x020000C5 RID: 197
public static class EventManager
{
	// Token: 0x06000640 RID: 1600 RVA: 0x000023BA File Offset: 0x000005BA
	public static void Initialize()
	{
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x000023BA File Offset: 0x000005BA
	public static void Dispose()
	{
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x00022868 File Offset: 0x00020A68
	public static void AddEventListener(string eventName, Action<Dictionary<string, object>> listener)
	{
		List<Action<Dictionary<string, object>>> collection;
		if (!EventManager.events.TryGetValue(eventName, out collection))
		{
			EventManager.events.Add(eventName, new List<Action<Dictionary<string, object>>>
			{
				listener
			});
			return;
		}
		List<Action<Dictionary<string, object>>> list = new List<Action<Dictionary<string, object>>>(collection);
		list.Add(listener);
		EventManager.events[eventName] = list;
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x000228B8 File Offset: 0x00020AB8
	public static void RemoveEventListener(string eventName, Action<Dictionary<string, object>> listener)
	{
		List<Action<Dictionary<string, object>>> collection;
		if (!EventManager.events.TryGetValue(eventName, out collection))
		{
			EventManager.Logger.Warning("Tried to remove listener for event " + eventName + ", but no listener was registered for it");
			return;
		}
		List<Action<Dictionary<string, object>>> list = new List<Action<Dictionary<string, object>>>(collection);
		list.Remove(listener);
		if (list.Count == 0)
		{
			EventManager.events.Remove(eventName);
			return;
		}
		EventManager.events[eventName] = list;
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x0002291F File Offset: 0x00020B1F
	public static bool HasEventListeners(string eventName)
	{
		return EventManager.events.ContainsKey(eventName);
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x0002292C File Offset: 0x00020B2C
	public static void TriggerEvent(string eventName, Dictionary<string, object> message = null)
	{
		List<Action<Dictionary<string, object>>> list;
		if (!EventManager.events.TryGetValue(eventName, out list))
		{
			return;
		}
		bool flag = eventName.StartsWith("Event_Server_", StringComparison.Ordinal);
		bool flag2 = eventName.StartsWith("Event_Client_", StringComparison.Ordinal);
		bool flag3 = eventName.StartsWith("Event_Everyone_", StringComparison.Ordinal);
		if ((flag2 || flag || flag3) && (!NetworkManager.Singleton || !NetworkManager.Singleton.IsListening))
		{
			EventManager.Logger.Warning("Triggering network event " + eventName + " without a NetworkManager listening");
		}
		if (message == null)
		{
			message = new Dictionary<string, object>
			{
				{
					"eventName",
					eventName
				}
			};
		}
		else if (!message.ContainsKey("eventName"))
		{
			message.Add("eventName", eventName);
		}
		for (int i = 0; i < list.Count; i++)
		{
			try
			{
				list[i](message);
			}
			catch (Exception arg)
			{
				EventManager.Logger.Error(string.Format("Listener for event {0} threw an exception: {1}", eventName, arg));
			}
		}
	}

	// Token: 0x0400040B RID: 1035
	private static readonly Logger Logger = new Logger("EventManager");

	// Token: 0x0400040C RID: 1036
	private static Dictionary<string, List<Action<Dictionary<string, object>>>> events = new Dictionary<string, List<Action<Dictionary<string, object>>>>();
}
