using System;
using DG.Tweening;
using UnityEngine;

// Token: 0x0200022C RID: 556
public class MatchAbandonment
{
	// Token: 0x14000006 RID: 6
	// (add) Token: 0x0600118E RID: 4494 RVA: 0x00052D98 File Offset: 0x00050F98
	// (remove) Token: 0x0600118F RID: 4495 RVA: 0x00052DD0 File Offset: 0x00050FD0
	public event Action<MatchAbandonment> Abandoned;

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06001190 RID: 4496 RVA: 0x00052E05 File Offset: 0x00051005
	public string SteamId { get; }

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06001191 RID: 4497 RVA: 0x00052E0D File Offset: 0x0005100D
	public float AbandonmentTimeout { get; }

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x06001192 RID: 4498 RVA: 0x00052E15 File Offset: 0x00051015
	// (set) Token: 0x06001193 RID: 4499 RVA: 0x00052E1D File Offset: 0x0005101D
	public MatchAbandonmentPhase Phase { get; private set; }

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x06001194 RID: 4500 RVA: 0x00052E26 File Offset: 0x00051026
	public float RemainingSeconds
	{
		get
		{
			if (this.timeout == null || !this.timeout.IsActive())
			{
				return 0f;
			}
			return Mathf.Max(0f, this.timeout.Duration(true) - this.timeout.Elapsed(true));
		}
	}

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06001195 RID: 4501 RVA: 0x00052E66 File Offset: 0x00051066
	// (set) Token: 0x06001196 RID: 4502 RVA: 0x00052E6E File Offset: 0x0005106E
	public bool HasAbandoned { get; private set; }

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06001197 RID: 4503 RVA: 0x00052E77 File Offset: 0x00051077
	public bool IsPaused
	{
		get
		{
			return this.Phase == MatchAbandonmentPhase.Paused;
		}
	}

	// Token: 0x06001198 RID: 4504 RVA: 0x00052E84 File Offset: 0x00051084
	public MatchAbandonment(string steamId, float abandonmentTimeout)
	{
		this.SteamId = steamId;
		this.AbandonmentTimeout = Mathf.Max(0f, abandonmentTimeout);
		this.Phase = MatchAbandonmentPhase.None;
		this.StartTimeout(this.AbandonmentTimeout);
		Tween tween = this.timeout;
		if (tween == null)
		{
			return;
		}
		tween.Pause<Tween>();
	}

	// Token: 0x06001199 RID: 4505 RVA: 0x00052ED4 File Offset: 0x000510D4
	public void Stop()
	{
		if (this.isDisposed || this.HasAbandoned || this.Phase == MatchAbandonmentPhase.Paused)
		{
			return;
		}
		this.Phase = MatchAbandonmentPhase.Paused;
		Tween tween = this.timeout;
		if (tween != null)
		{
			tween.Pause<Tween>();
		}
		MatchAbandonment.Logger.Info(string.Format("{0} abandonment paused with {1:F2}s of budget remaining", this.SteamId, this.RemainingSeconds));
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x00052F3C File Offset: 0x0005113C
	public void Start()
	{
		if (this.isDisposed || this.HasAbandoned || this.Phase != MatchAbandonmentPhase.Paused)
		{
			return;
		}
		this.Phase = MatchAbandonmentPhase.Running;
		Tween tween = this.timeout;
		if (tween != null)
		{
			tween.Play<Tween>();
		}
		MatchAbandonment.Logger.Info(string.Format("{0} abandonment running with {1:F2}s of budget remaining", this.SteamId, this.RemainingSeconds));
	}

	// Token: 0x0600119B RID: 4507 RVA: 0x00052FA1 File Offset: 0x000511A1
	public void Dispose()
	{
		if (this.isDisposed)
		{
			return;
		}
		this.isDisposed = true;
		this.KillTimeout();
		this.Abandoned = null;
	}

	// Token: 0x0600119C RID: 4508 RVA: 0x00052FC0 File Offset: 0x000511C0
	private void StartTimeout(float duration)
	{
		this.timeout = DOVirtual.DelayedCall(duration, new TweenCallback(this.Abandon), true).SetTarget(this);
	}

	// Token: 0x0600119D RID: 4509 RVA: 0x00052FE1 File Offset: 0x000511E1
	private void KillTimeout()
	{
		Tween tween = this.timeout;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.timeout = null;
	}

	// Token: 0x0600119E RID: 4510 RVA: 0x00052FFC File Offset: 0x000511FC
	private void Abandon()
	{
		if (this.isDisposed || this.HasAbandoned)
		{
			return;
		}
		this.timeout = null;
		this.HasAbandoned = true;
		MatchAbandonment.Logger.Info(this.SteamId + " abandoned the match");
		Action<MatchAbandonment> abandoned = this.Abandoned;
		if (abandoned == null)
		{
			return;
		}
		abandoned(this);
	}

	// Token: 0x04000B7A RID: 2938
	private static readonly global::Logger Logger = new global::Logger("MatchAbandonment");

	// Token: 0x04000B80 RID: 2944
	private Tween timeout;

	// Token: 0x04000B81 RID: 2945
	private bool isDisposed;
}
