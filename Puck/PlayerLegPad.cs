using System;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using DG.Tweening.CustomPlugins;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class PlayerLegPad : MonoBehaviour
{
	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000149 RID: 329 RVA: 0x0000738D File Offset: 0x0000558D
	// (set) Token: 0x0600014A RID: 330 RVA: 0x00007395 File Offset: 0x00005595
	public PlayerLegPadState State
	{
		get
		{
			return this.state;
		}
		set
		{
			this.OnStateChanged(this.state, value);
			this.state = value;
		}
	}

	// Token: 0x0600014B RID: 331 RVA: 0x000073AB File Offset: 0x000055AB
	private void Awake()
	{
		this.localPosition = base.transform.localPosition;
		this.localRotation = base.transform.localRotation;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x000073D0 File Offset: 0x000055D0
	private void Update()
	{
		this.ShootLegPadRaycast();
		base.transform.localPosition = new Vector3(this.localPosition.x, this.localYPosition, this.localPosition.z);
		base.transform.localRotation = this.localRotation;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x000023BA File Offset: 0x000005BA
	private void FixedUpdate()
	{
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00007420 File Offset: 0x00005620
	private void OnDestroy()
	{
		Tween tween = this.localPositionTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		Tween tween2 = this.localRotationTween;
		if (tween2 == null)
		{
			return;
		}
		tween2.Kill(false);
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00007448 File Offset: 0x00005648
	public void ShootLegPadRaycast()
	{
		Vector3 vector = base.transform.parent.TransformPoint(this.localPosition);
		vector.y = base.transform.parent.position.y;
		vector += base.transform.parent.up;
		Vector3 vector2 = -base.transform.parent.up;
		Debug.DrawRay(vector, vector2 * this.raycastDistance, Color.red);
		RaycastHit raycastHit;
		if (Physics.Raycast(vector, vector2, out raycastHit, this.raycastDistance, this.raycastLayerMask))
		{
			this.localYPosition = base.transform.parent.InverseTransformPoint(raycastHit.point).y + this.localPosition.y;
			return;
		}
		this.localYPosition = base.transform.parent.InverseTransformPoint(vector + vector2 * this.raycastDistance).y + this.localPosition.y;
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00007550 File Offset: 0x00005750
	private void OnStateChanged(PlayerLegPadState oldState, PlayerLegPadState newState)
	{
		Tween tween = this.localPositionTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		Tween tween2 = this.localRotationTween;
		if (tween2 != null)
		{
			tween2.Kill(false);
		}
		if (oldState == PlayerLegPadState.Butterfly && newState == PlayerLegPadState.ButterflyExtended)
		{
			this.localPosition = this.positions[oldState].localPosition;
			this.localRotation = this.positions[oldState].localRotation;
		}
		this.localPositionTween = DOTween.To(() => this.localPosition, delegate(Vector3 value)
		{
			this.localPosition = value;
		}, this.positions[newState].localPosition, this.transitionDuration);
		this.localRotationTween = DOTween.To<Quaternion, Quaternion, NoOptions>(PureQuaternionPlugin.Plug(), () => this.localRotation, delegate(Quaternion value)
		{
			this.localRotation = value;
		}, this.positions[newState].localRotation, this.transitionDuration);
	}

	// Token: 0x04000112 RID: 274
	[Header("Settings")]
	[SerializeField]
	private float raycastDistance = 1f;

	// Token: 0x04000113 RID: 275
	[SerializeField]
	private float transitionDuration = 0.15f;

	// Token: 0x04000114 RID: 276
	[Space(20f)]
	[SerializeField]
	private LayerMask raycastLayerMask;

	// Token: 0x04000115 RID: 277
	[Header("References")]
	[SerializeField]
	private SerializedDictionary<PlayerLegPadState, Transform> positions = new SerializedDictionary<PlayerLegPadState, Transform>();

	// Token: 0x04000116 RID: 278
	private PlayerLegPadState state;

	// Token: 0x04000117 RID: 279
	private Vector3 localPosition = Vector3.zero;

	// Token: 0x04000118 RID: 280
	private float localYPosition;

	// Token: 0x04000119 RID: 281
	private Quaternion localRotation = Quaternion.identity;

	// Token: 0x0400011A RID: 282
	private Tween localPositionTween;

	// Token: 0x0400011B RID: 283
	private Tween localRotationTween;
}
