using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000112 RID: 274
public class PhysicsManager : MonoBehaviourSingleton<PhysicsManager>
{
	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000842 RID: 2114 RVA: 0x00029CAC File Offset: 0x00027EAC
	// (remove) Token: 0x06000843 RID: 2115 RVA: 0x00029CE0 File Offset: 0x00027EE0
	public static event Action<float> OnBeforeSimulate;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000844 RID: 2116 RVA: 0x00029D14 File Offset: 0x00027F14
	// (remove) Token: 0x06000845 RID: 2117 RVA: 0x00029D48 File Offset: 0x00027F48
	public static event Action<float> OnAfterSimulate;

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000846 RID: 2118 RVA: 0x00029D7B File Offset: 0x00027F7B
	// (set) Token: 0x06000847 RID: 2119 RVA: 0x00029D82 File Offset: 0x00027F82
	[HideInInspector]
	public SimulationMode SimulationMode
	{
		get
		{
			return Physics.simulationMode;
		}
		set
		{
			if (Physics.simulationMode == value)
			{
				return;
			}
			Physics.simulationMode = value;
			this.tickAccumulator = 0f;
		}
	}

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06000848 RID: 2120 RVA: 0x00029D9E File Offset: 0x00027F9E
	// (set) Token: 0x06000849 RID: 2121 RVA: 0x00029DA8 File Offset: 0x00027FA8
	[HideInInspector]
	public int TickRate
	{
		get
		{
			return this.tickRate;
		}
		set
		{
			if (this.tickRate == value)
			{
				return;
			}
			this.tickRate = value;
			Time.fixedDeltaTime = this.TickInterval;
			EventManager.TriggerEvent("Event_OnPhysicsManagerTickRateChanged", new Dictionary<string, object>
			{
				{
					"value",
					this.tickRate
				}
			});
		}
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x0600084A RID: 2122 RVA: 0x00029DF6 File Offset: 0x00027FF6
	[HideInInspector]
	public float TickInterval
	{
		get
		{
			return 1f / (float)this.TickRate;
		}
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x00029E05 File Offset: 0x00028005
	public override void Awake()
	{
		base.Awake();
		Physics.simulationMode = this.simulationMode;
		Time.fixedDeltaTime = this.TickInterval;
		base.StartCoroutine(this.AfterFixedUpdateSimulate());
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x00029E30 File Offset: 0x00028030
	private void Start()
	{
		EventManager.TriggerEvent("Event_OnPhysicsManagerInitialized", new Dictionary<string, object>
		{
			{
				"tickRate",
				this.tickRate
			}
		});
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x00029E57 File Offset: 0x00028057
	private void FixedUpdate()
	{
		if (Physics.simulationMode != SimulationMode.FixedUpdate)
		{
			return;
		}
		Action<float> onBeforeSimulate = PhysicsManager.OnBeforeSimulate;
		if (onBeforeSimulate == null)
		{
			return;
		}
		onBeforeSimulate(Time.fixedDeltaTime);
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x00029E78 File Offset: 0x00028078
	private void Update()
	{
		if (Physics.simulationMode != SimulationMode.Script)
		{
			return;
		}
		this.tickAccumulator += Time.deltaTime;
		if (this.tickAccumulator >= this.TickInterval)
		{
			Action<float> onBeforeSimulate = PhysicsManager.OnBeforeSimulate;
			if (onBeforeSimulate != null)
			{
				onBeforeSimulate(this.TickInterval);
			}
			Physics.Simulate(this.TickInterval);
			Action<float> onAfterSimulate = PhysicsManager.OnAfterSimulate;
			if (onAfterSimulate != null)
			{
				onAfterSimulate(this.TickInterval);
			}
			this.tickAccumulator -= this.TickInterval;
		}
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x00029EF8 File Offset: 0x000280F8
	private IEnumerator AfterFixedUpdateSimulate()
	{
		WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
		for (;;)
		{
			yield return waitForFixedUpdate;
			if (Physics.simulationMode == SimulationMode.FixedUpdate)
			{
				Action<float> onAfterSimulate = PhysicsManager.OnAfterSimulate;
				if (onAfterSimulate != null)
				{
					onAfterSimulate(Time.fixedDeltaTime);
				}
			}
		}
		yield break;
	}

	// Token: 0x04000531 RID: 1329
	[Header("Settings")]
	[SerializeField]
	private SimulationMode simulationMode;

	// Token: 0x04000532 RID: 1330
	[SerializeField]
	private int tickRate = 100;

	// Token: 0x04000535 RID: 1333
	private float tickAccumulator;
}
