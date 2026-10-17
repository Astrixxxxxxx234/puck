using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class EventDependencyTrigger : MonoBehaviour
{
	// Token: 0x06000036 RID: 54 RVA: 0x0000283C File Offset: 0x00000A3C
	private void Start()
	{
		foreach (string eventName in this.dependencyEvents.Keys)
		{
			EventManager.AddEventListener(eventName, new Action<Dictionary<string, object>>(this.OnDependencyEvent));
		}
	}

	// Token: 0x06000037 RID: 55 RVA: 0x000028A0 File Offset: 0x00000AA0
	private void OnDestroy()
	{
		Tween tween = this.timeoutTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		foreach (string eventName in this.dependencyEvents.Keys)
		{
			EventManager.RemoveEventListener(eventName, new Action<Dictionary<string, object>>(this.OnDependencyEvent));
		}
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002914 File Offset: 0x00000B14
	private void OnDependencyEvent(Dictionary<string, object> message)
	{
		string key = (string)message["eventName"];
		if (!this.dependencyEvents.ContainsValue(true))
		{
			Tween tween = this.timeoutTween;
			if (tween != null)
			{
				tween.Kill(false);
			}
			this.timeoutTween = DOVirtual.DelayedCall(this.timeout, delegate
			{
				EventDependencyTrigger.Logger.Warning("Event " + this.triggerEventName + " timed out waiting for dependencies");
				if (this.isRepeating)
				{
					this.Reset();
				}
			}, true);
		}
		this.dependencyEvents[key] = true;
		if (!this.dependencyEvents.ContainsValue(false))
		{
			EventDependencyTrigger.Logger.Info("All dependencies met, triggering event " + this.triggerEventName);
			EventManager.TriggerEvent(this.triggerEventName, null);
			if (this.isRepeating)
			{
				this.Reset();
			}
		}
	}

	// Token: 0x06000039 RID: 57 RVA: 0x000029BF File Offset: 0x00000BBF
	private void Reset()
	{
		Tween tween = this.timeoutTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.dependencyEvents.Keys.ToList<string>().ForEach(delegate(string key)
		{
			this.dependencyEvents[key] = false;
		});
	}

	// Token: 0x04000021 RID: 33
	private static readonly global::Logger Logger = new global::Logger("EventDependencyTrigger");

	// Token: 0x04000022 RID: 34
	[Header("Settings")]
	[SerializeField]
	private SerializedDictionary<string, bool> dependencyEvents = new SerializedDictionary<string, bool>();

	// Token: 0x04000023 RID: 35
	[SerializeField]
	private string triggerEventName;

	// Token: 0x04000024 RID: 36
	[SerializeField]
	private float timeout = 3f;

	// Token: 0x04000025 RID: 37
	[SerializeField]
	private bool isRepeating = true;

	// Token: 0x04000026 RID: 38
	private Tween timeoutTween;
}
