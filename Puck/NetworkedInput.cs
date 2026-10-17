using System;
using UnityEngine;

// Token: 0x02000231 RID: 561
public class NetworkedInput<T>
{
	// Token: 0x17000177 RID: 375
	// (get) Token: 0x060011B5 RID: 4533 RVA: 0x00053339 File Offset: 0x00051539
	public bool HasChanged
	{
		get
		{
			return this.HasChangedValidator(this.LastSentValue, this.ClientValue);
		}
	}

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x060011B6 RID: 4534 RVA: 0x00053352 File Offset: 0x00051552
	public bool ShouldChange
	{
		get
		{
			return this.ShouldChangeValidator(this.LastReceivedValue, this.LastReceivedTime, this.ServerValue);
		}
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x00053374 File Offset: 0x00051574
	public NetworkedInput(T initialValue = default(T), NetworkedInput<T>.HasChangedDelegate hasChangedValidator = null, NetworkedInput<T>.ShouldChangeDelegate shouldChangeValidator = null)
	{
		this.ClientValue = initialValue;
		this.LastSentValue = default(T);
		this.ServerValue = default(T);
		if (hasChangedValidator != null)
		{
			this.HasChangedValidator = hasChangedValidator;
		}
		else
		{
			this.HasChangedValidator = ((T lastSentValue, T clientValue) => !this.ClientValue.Equals(this.LastSentValue));
		}
		if (shouldChangeValidator != null)
		{
			this.ShouldChangeValidator = shouldChangeValidator;
			return;
		}
		this.ShouldChangeValidator = ((T lastReceivedValue, double lastReceivedTime, T serverValue) => !this.ServerValue.Equals(this.LastReceivedValue));
	}

	// Token: 0x060011B8 RID: 4536 RVA: 0x000533E1 File Offset: 0x000515E1
	public void ClientTick()
	{
		this.LastSentValue = this.ClientValue;
		this.LastSentTime = Time.timeAsDouble;
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x000533FA File Offset: 0x000515FA
	public void ServerTick()
	{
		this.LastReceivedValue = this.ServerValue;
		this.LastReceivedTime = Time.timeAsDouble;
	}

	// Token: 0x04000B87 RID: 2951
	public T ClientValue;

	// Token: 0x04000B88 RID: 2952
	public T ServerValue;

	// Token: 0x04000B89 RID: 2953
	public T LastSentValue;

	// Token: 0x04000B8A RID: 2954
	public double LastSentTime;

	// Token: 0x04000B8B RID: 2955
	public T LastReceivedValue;

	// Token: 0x04000B8C RID: 2956
	public double LastReceivedTime;

	// Token: 0x04000B8D RID: 2957
	private NetworkedInput<T>.HasChangedDelegate HasChangedValidator;

	// Token: 0x04000B8E RID: 2958
	private NetworkedInput<T>.ShouldChangeDelegate ShouldChangeValidator;

	// Token: 0x02000232 RID: 562
	// (Invoke) Token: 0x060011BD RID: 4541
	public delegate bool HasChangedDelegate(T LastSentValue, T ClientValue);

	// Token: 0x02000233 RID: 563
	// (Invoke) Token: 0x060011C1 RID: 4545
	public delegate bool ShouldChangeDelegate(T LastSentValue, double lastReceivedTime, T ClientValue);
}
