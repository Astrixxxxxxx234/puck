using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class StickMesh : MonoBehaviour
{
	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060002F0 RID: 752 RVA: 0x000135DC File Offset: 0x000117DC
	[HideInInspector]
	public Collider ShaftCollider
	{
		get
		{
			return this.shaftCollider;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x060002F1 RID: 753 RVA: 0x000135E4 File Offset: 0x000117E4
	[HideInInspector]
	public Collider BladeCollider
	{
		get
		{
			return this.bladeCollider;
		}
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x000135EC File Offset: 0x000117EC
	private void OnDestroy()
	{
		Object.Destroy(this.stickMeshRenderer.material);
		Object.Destroy(this.shaftTapeMeshRenderer.material);
		Object.Destroy(this.bladeTapeMeshRenderer.material);
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x00013620 File Offset: 0x00011820
	public void SetSkinID(int skinID, PlayerTeam team)
	{
		StickSkin stickSkin = this.skins.Find((StickSkin s) => s.ID == skinID && s.IsForTeam(team));
		if (stickSkin == null)
		{
			StickMesh.Logger.Warning(string.Format("Tried to set invalid skinID {0}", skinID));
			return;
		}
		Utils.SwapMaterial(this.stickMeshRenderer, stickSkin.Material, 0);
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x00013690 File Offset: 0x00011890
	public void SetShaftTapeID(int shaftTapeID)
	{
		if (shaftTapeID == -1)
		{
			this.shaftTapeGameObject.SetActive(false);
			return;
		}
		StickTape stickTape = this.shaftTapes.Find((StickTape t) => t.ID == shaftTapeID && t.Material != null);
		if (stickTape == null)
		{
			StickMesh.Logger.Warning(string.Format("Tried to set invalid shaftTapeID {0}", shaftTapeID));
			return;
		}
		this.shaftTapeGameObject.SetActive(true);
		Utils.SwapMaterial(this.shaftTapeMeshRenderer, stickTape.Material, 0);
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x0001371C File Offset: 0x0001191C
	public void SetBladeTapeID(int bladeTapeID)
	{
		if (bladeTapeID == -1)
		{
			this.bladeTapeGameObject.SetActive(false);
			return;
		}
		StickTape stickTape = this.bladeTapes.Find((StickTape t) => t.ID == bladeTapeID && t.Material != null);
		if (stickTape == null)
		{
			StickMesh.Logger.Warning(string.Format("Tried to set invalid bladeTapeID {0}", bladeTapeID));
			return;
		}
		this.bladeTapeGameObject.SetActive(true);
		Utils.SwapMaterial(this.bladeTapeMeshRenderer, stickTape.Material, 0);
	}

	// Token: 0x04000229 RID: 553
	private static readonly global::Logger Logger = new global::Logger("StickMesh");

	// Token: 0x0400022A RID: 554
	[Header("Settings")]
	[SerializeField]
	private List<StickSkin> skins = new List<StickSkin>();

	// Token: 0x0400022B RID: 555
	[SerializeField]
	private List<StickTape> shaftTapes = new List<StickTape>();

	// Token: 0x0400022C RID: 556
	[SerializeField]
	private List<StickTape> bladeTapes = new List<StickTape>();

	// Token: 0x0400022D RID: 557
	[Header("References")]
	[SerializeField]
	private MeshRenderer stickMeshRenderer;

	// Token: 0x0400022E RID: 558
	[SerializeField]
	private GameObject shaftTapeGameObject;

	// Token: 0x0400022F RID: 559
	[SerializeField]
	private MeshRenderer shaftTapeMeshRenderer;

	// Token: 0x04000230 RID: 560
	[SerializeField]
	private GameObject bladeTapeGameObject;

	// Token: 0x04000231 RID: 561
	[SerializeField]
	private MeshRenderer bladeTapeMeshRenderer;

	// Token: 0x04000232 RID: 562
	[Space(20f)]
	[SerializeField]
	private Collider shaftCollider;

	// Token: 0x04000233 RID: 563
	[SerializeField]
	private Collider bladeCollider;
}
