using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class SmoothPositioner : MonoBehaviour
{
	// Token: 0x060003D5 RID: 981 RVA: 0x00016B0B File Offset: 0x00014D0B
	private void Start()
	{
		this.SetPosition(this.initialPosition, true);
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x00016B1A File Offset: 0x00014D1A
	private void OnDestroy()
	{
		Tween tween = this.positionTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		Tween tween2 = this.rotationTween;
		if (tween2 == null)
		{
			return;
		}
		tween2.Kill(false);
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x00016B40 File Offset: 0x00014D40
	public void SetPosition(string positionName, bool instant = false)
	{
		if (!this.positions.ContainsKey(positionName))
		{
			SmoothPositioner.Logger.Error("Target position " + positionName + " does not exist");
			return;
		}
		if (instant)
		{
			Tween tween = this.positionTween;
			if (tween != null)
			{
				tween.Kill(false);
			}
			Tween tween2 = this.rotationTween;
			if (tween2 != null)
			{
				tween2.Kill(false);
			}
			base.transform.position = this.positions[positionName].position;
			base.transform.rotation = this.positions[positionName].rotation;
		}
		else
		{
			if (this.currentPosition == positionName)
			{
				return;
			}
			Tween tween3 = this.positionTween;
			if (tween3 != null)
			{
				tween3.Kill(false);
			}
			Tween tween4 = this.rotationTween;
			if (tween4 != null)
			{
				tween4.Kill(false);
			}
			this.positionTween = base.transform.DOMove(this.positions[positionName].position, this.transitionDuration, false).SetEase(this.transitionEase);
			this.rotationTween = base.transform.DORotateQuaternion(this.positions[positionName].rotation, this.transitionDuration).SetEase(this.transitionEase);
		}
		this.currentPosition = positionName;
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x00016C7C File Offset: 0x00014E7C
	private void OnDrawGizmos()
	{
		if (!Application.isEditor)
		{
			return;
		}
		foreach (KeyValuePair<string, Transform> keyValuePair in this.positions)
		{
			string key = keyValuePair.Key;
			Transform value = keyValuePair.Value;
			Gizmos.color = Color.black;
			Gizmos.DrawSphere(value.position, 0.05f);
			Gizmos.matrix = value.localToWorldMatrix;
			Gizmos.DrawFrustum(Vector3.zero, 60f, 1f, 0f, 1f);
			Gizmos.matrix = Matrix4x4.identity;
			Gizmos.color = Color.green;
			Gizmos.DrawLine(value.position, value.position + value.forward * 1f);
		}
	}

	// Token: 0x040002D8 RID: 728
	private static readonly global::Logger Logger = new global::Logger("SmoothPositioner");

	// Token: 0x040002D9 RID: 729
	[Header("Settings")]
	[SerializeField]
	private SerializedDictionary<string, Transform> positions = new SerializedDictionary<string, Transform>();

	// Token: 0x040002DA RID: 730
	[SerializeField]
	private string initialPosition;

	// Token: 0x040002DB RID: 731
	[SerializeField]
	private float transitionDuration = 0.5f;

	// Token: 0x040002DC RID: 732
	[SerializeField]
	private Ease transitionEase = Ease.Linear;

	// Token: 0x040002DD RID: 733
	private string currentPosition;

	// Token: 0x040002DE RID: 734
	private Tween positionTween;

	// Token: 0x040002DF RID: 735
	private Tween rotationTween;
}
