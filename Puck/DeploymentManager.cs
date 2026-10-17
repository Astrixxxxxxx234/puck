using System;
using DG.Tweening;
using UnityEngine;

// Token: 0x0200014F RID: 335
public class DeploymentManager : MonoBehaviour
{
	// Token: 0x17000103 RID: 259
	// (get) Token: 0x060009B0 RID: 2480 RVA: 0x0002F2BC File Offset: 0x0002D4BC
	// (set) Token: 0x060009B1 RID: 2481 RVA: 0x0002F2C4 File Offset: 0x0002D4C4
	public DeploymentProvider Provider { get; private set; }

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x060009B2 RID: 2482 RVA: 0x0002F2CD File Offset: 0x0002D4CD
	// (set) Token: 0x060009B3 RID: 2483 RVA: 0x0002F2D5 File Offset: 0x0002D4D5
	public ushort DeploymentPort { get; private set; }

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x060009B4 RID: 2484 RVA: 0x0002F2DE File Offset: 0x0002D4DE
	// (set) Token: 0x060009B5 RID: 2485 RVA: 0x0002F2E6 File Offset: 0x0002D4E6
	public string DeploymentId { get; private set; }

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0002F2EF File Offset: 0x0002D4EF
	// (set) Token: 0x060009B7 RID: 2487 RVA: 0x0002F2F7 File Offset: 0x0002D4F7
	public bool StopWhenEmpty { get; private set; }

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x060009B8 RID: 2488 RVA: 0x0002F300 File Offset: 0x0002D500
	public bool IsDeployment
	{
		get
		{
			return this.Provider > DeploymentProvider.None;
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x060009B9 RID: 2489 RVA: 0x0002F30B File Offset: 0x0002D50B
	public bool BindsDeploymentPort
	{
		get
		{
			return this.Provider == DeploymentProvider.PuckOrchestrator;
		}
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x0002F318 File Offset: 0x0002D518
	private void Awake()
	{
		string environmentVariable = Environment.GetEnvironmentVariable("ARBITRIUM_PORT_PUCK_EXTERNAL");
		string environmentVariable2 = Environment.GetEnvironmentVariable("PUCK_DEPLOYMENT_PORT");
		string environmentVariable3 = Environment.GetEnvironmentVariable("PUCK_DEPLOYMENT_STOP_WHEN_EMPTY");
		this.StopWhenEmpty = string.Equals(environmentVariable3, "true", StringComparison.OrdinalIgnoreCase);
		this.DeploymentId = (Environment.GetEnvironmentVariable("PUCK_DEPLOYMENT_ID") ?? Utils.GetCommandLineArgument("--deploymentId", null));
		if (!string.IsNullOrEmpty(environmentVariable))
		{
			this.Provider = DeploymentProvider.Edgegap;
			this.DeploymentPort = ushort.Parse(environmentVariable);
			DeploymentManager.Logger.Info(string.Format("Running in Edgegap deployment (Provider: {0}, DeploymentPort: {1}, StopWhenEmpty: {2})", this.Provider, this.DeploymentPort, this.StopWhenEmpty));
			return;
		}
		if (!string.IsNullOrEmpty(environmentVariable2))
		{
			this.Provider = DeploymentProvider.PuckOrchestrator;
			this.DeploymentPort = ushort.Parse(environmentVariable2);
			DeploymentManager.Logger.Info(string.Format("Running in Puck Orchestrator deployment (Provider: {0}, DeploymentPort: {1}, StopWhenEmpty: {2})", this.Provider, this.DeploymentPort, this.StopWhenEmpty));
		}
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x0002F418 File Offset: 0x0002D618
	public void StartEmptyTimeout()
	{
		if (!this.StopWhenEmpty)
		{
			return;
		}
		DeploymentManager.Logger.Info("Starting empty timeout");
		Tween tween = this.emptyTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.emptyTween = DOVirtual.DelayedCall(60f, delegate
		{
			this.StopDeployment();
		}, true);
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x0002F46C File Offset: 0x0002D66C
	public void StopEmptyTimeout()
	{
		if (!this.StopWhenEmpty)
		{
			return;
		}
		DeploymentManager.Logger.Info("Stopping empty timeout");
		Tween tween = this.emptyTween;
		if (tween == null)
		{
			return;
		}
		tween.Kill(false);
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x0002F498 File Offset: 0x0002D698
	public void StartAuthenticationTimeout()
	{
		if (!this.IsDeployment)
		{
			return;
		}
		DeploymentManager.Logger.Info("Starting authentication timeout");
		Tween tween = this.authenticationTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.authenticationTween = DOVirtual.DelayedCall(60f, delegate
		{
			this.StopDeployment();
		}, true);
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x0002F4EC File Offset: 0x0002D6EC
	public void StopAuthenticationTimeout()
	{
		if (!this.IsDeployment)
		{
			return;
		}
		DeploymentManager.Logger.Info("Stopping authentication timeout");
		Tween tween = this.authenticationTween;
		if (tween == null)
		{
			return;
		}
		tween.Kill(false);
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x0002F517 File Offset: 0x0002D717
	public void StopDeployment()
	{
		if (!this.IsDeployment)
		{
			return;
		}
		DeploymentManager.Logger.Info(string.Format("Stopping deployment (Provider: {0})", this.Provider));
		Application.Quit();
	}

	// Token: 0x040005FA RID: 1530
	private static readonly global::Logger Logger = new global::Logger("DeploymentManager");

	// Token: 0x040005FF RID: 1535
	private Tween emptyTween;

	// Token: 0x04000600 RID: 1536
	private Tween authenticationTween;
}
