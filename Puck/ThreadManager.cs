using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200018F RID: 399
public class ThreadManager : MonoBehaviourSingleton<ThreadManager>
{
	// Token: 0x06000C58 RID: 3160 RVA: 0x0003AA84 File Offset: 0x00038C84
	private void Update()
	{
		Queue<Action> obj = this.executionQueue;
		lock (obj)
		{
			while (this.executionQueue.Count > 0)
			{
				this.executionQueue.Dequeue()();
			}
		}
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x0003AAE0 File Offset: 0x00038CE0
	public void Enqueue(IEnumerator action)
	{
		Queue<Action> obj = this.executionQueue;
		lock (obj)
		{
			this.executionQueue.Enqueue(delegate
			{
				this.StartCoroutine(action);
			});
		}
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x0003AB48 File Offset: 0x00038D48
	public void Enqueue(Action action)
	{
		this.Enqueue(this.ActionWrapper(action));
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x0003AB57 File Offset: 0x00038D57
	private IEnumerator ActionWrapper(Action action)
	{
		action();
		yield return null;
		yield break;
	}

	// Token: 0x040007AA RID: 1962
	private Queue<Action> executionQueue = new Queue<Action>();
}
