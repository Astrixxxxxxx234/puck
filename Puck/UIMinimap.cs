using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x020001DF RID: 479
public class UIMinimap : UIView
{
	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000EAB RID: 3755 RVA: 0x000454F0 File Offset: 0x000436F0
	[HideInInspector]
	public Vector2 Position
	{
		get
		{
			return new Vector2(this.minimap.style.left.value.value, this.minimap.style.top.value.value);
		}
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x00045544 File Offset: 0x00043744
	private void Update()
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		if (!base.IsVisible)
		{
			return;
		}
		this.updateAccumulator += Time.deltaTime;
		if (this.updateAccumulator < 1f / (float)this.updateRate)
		{
			return;
		}
		this.updateAccumulator = Mathf.Min(this.updateAccumulator - 1f / (float)this.updateRate, 1f / (float)this.updateRate);
		foreach (KeyValuePair<PlayerBody, ValueTuple<VisualElement, VisualElement>> keyValuePair in this.playerBodyVisualElementMap)
		{
			PlayerBody key = keyValuePair.Key;
			VisualElement item = keyValuePair.Value.Item1;
			VisualElement item2 = keyValuePair.Value.Item2;
			if (key)
			{
				float value = (this.Team == PlayerTeam.Blue) ? key.transform.rotation.eulerAngles.y : (key.transform.rotation.eulerAngles.y + 180f);
				this.ApplyMinimapTranslate(item, key.transform.position);
				item2.style.rotate = new Rotate(value);
				bool flag = SettingsManager.ShowMinimapFallenIndicator && key.HasFallen.Value;
				item.style.opacity = (flag ? 0.4f : 1f);
			}
		}
		foreach (KeyValuePair<Stick, VisualElement> keyValuePair2 in this.stickVisualElementMap)
		{
			Stick key2 = keyValuePair2.Key;
			VisualElement value2 = keyValuePair2.Value;
			if (key2 && key2.Player)
			{
				if (!SettingsManager.ShowMinimapSticks)
				{
					value2.parent.style.display = DisplayStyle.None;
				}
				else
				{
					value2.parent.style.display = DisplayStyle.Flex;
					Vector3 bladeHandlePosition = key2.BladeHandlePosition;
					float num = key2.BladeHandleRotation.eulerAngles.y;
					if (this.Team == PlayerTeam.Red)
					{
						num += 180f;
					}
					num -= key2.BladeAngle;
					this.ApplyMinimapTranslate(value2.parent, bladeHandlePosition);
					value2.style.rotate = new Rotate(num);
					float num2 = UIMinimap.HeightToScale(bladeHandlePosition.y, 3f, 0.5f);
					float num3 = num2;
					if (SettingsManager.ShowMinimapFallenIndicator && key2.Player.PlayerBody && key2.Player.PlayerBody.HasFallen.Value)
					{
						num3 *= 0.4f;
					}
					value2.style.scale = new StyleScale(new Scale(new Vector2(num2, 1f)));
					value2.style.opacity = num3;
				}
			}
		}
		foreach (KeyValuePair<Puck, VisualElement> keyValuePair3 in this.puckVisualElementMap)
		{
			Puck key3 = keyValuePair3.Key;
			VisualElement value3 = keyValuePair3.Value;
			if (key3)
			{
				this.ApplyMinimapTranslate(value3, key3.transform.position);
				if (SettingsManager.ShowMinimapPuckElevation)
				{
					float num4 = UIMinimap.HeightToScale(key3.transform.position.y, 8f, 0.6f);
					value3.style.opacity = num4;
					value3.style.scale = new StyleScale(new Scale(new Vector2(num4, num4)));
				}
				else
				{
					value3.style.opacity = 1f;
					value3.style.scale = new StyleScale(new Scale(Vector2.one));
				}
			}
		}
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x000459A0 File Offset: 0x00043BA0
	private void ApplyMinimapTranslate(VisualElement element, Vector3 worldPosition)
	{
		Vector3 position = (this.Team == PlayerTeam.Blue) ? worldPosition : (-worldPosition);
		Vector2 vector = this.WorldPositionToMinimapPosition(position, this.Bounds);
		element.style.translate = new Translate(-vector.x, vector.y);
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x000459FA File Offset: 0x00043BFA
	private static float HeightToScale(float worldY, float maxHeight, float minScale)
	{
		return Mathf.Lerp(1f, minScale, Mathf.Clamp01(Mathf.Abs(worldY) / maxHeight));
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x00045A14 File Offset: 0x00043C14
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("MinimapView", null);
		this.minimap = base.View.Query("Minimap", null);
		this.background = this.minimap.Query("Background", null);
		this.foreground = this.minimap.Query("Foreground", null);
		this.content = this.minimap.Query("Content", null);
		this.content.Clear();
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x0003E32E File Offset: 0x0003C52E
	public override bool Show()
	{
		return SettingsManager.ShowGameUserInterface && base.Show();
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x00045AB4 File Offset: 0x00043CB4
	public void AddPlayerBody(PlayerBody playerBody)
	{
		if (!playerBody)
		{
			return;
		}
		if (this.playerBodyVisualElementMap.ContainsKey(playerBody))
		{
			return;
		}
		TemplateContainer templateContainer = this.playerAsset.Instantiate();
		this.playerBodyVisualElementMap.Add(playerBody, new ValueTuple<VisualElement, VisualElement>(templateContainer, templateContainer.Query("Body", null)));
		this.content.Add(templateContainer);
		templateContainer.SendToBack();
		this.StylePlayer(playerBody);
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x00045B24 File Offset: 0x00043D24
	public void StylePlayer(PlayerBody playerBody)
	{
		if (!playerBody)
		{
			return;
		}
		if (!this.playerBodyVisualElementMap.ContainsKey(playerBody))
		{
			return;
		}
		Player player = playerBody.Player;
		if (!player)
		{
			return;
		}
		VisualElement visualElement = this.playerBodyVisualElementMap[playerBody].Item1.Query("Player", null);
		Label label = visualElement.Query("NumberLabel", null);
		UIUtils.SetTeamClass(visualElement, player.Team);
		visualElement.EnableInClassList("isLocalPlayer", player.IsLocalPlayer);
		label.text = player.Number.Value.ToString();
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x00045BC4 File Offset: 0x00043DC4
	public void RemovePlayerBody(PlayerBody playerBody)
	{
		if (!playerBody)
		{
			return;
		}
		if (!this.playerBodyVisualElementMap.ContainsKey(playerBody))
		{
			return;
		}
		this.content.Remove(this.playerBodyVisualElementMap[playerBody].Item1);
		this.playerBodyVisualElementMap.Remove(playerBody);
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x00045C14 File Offset: 0x00043E14
	public void AddPuck(Puck puck)
	{
		if (!puck)
		{
			return;
		}
		if (this.puckVisualElementMap.ContainsKey(puck))
		{
			return;
		}
		TemplateContainer templateContainer = this.puckAsset.Instantiate();
		VisualElement visualElement = templateContainer.Q(null, "minimapPuck");
		if (visualElement != null)
		{
			visualElement.generateVisualContent = new Action<MeshGenerationContext>(UIMinimap.DrawPuckDot);
		}
		this.puckVisualElementMap.Add(puck, templateContainer);
		this.content.Add(templateContainer);
		templateContainer.BringToFront();
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x00045C88 File Offset: 0x00043E88
	private static void DrawPuckDot(MeshGenerationContext context)
	{
		Rect contentRect = context.visualElement.contentRect;
		if (contentRect.width <= 0f || contentRect.height <= 0f)
		{
			return;
		}
		float radius = Mathf.Min(contentRect.width, contentRect.height) / 2f;
		Painter2D painter2D = context.painter2D;
		painter2D.fillColor = Color.black;
		painter2D.BeginPath();
		painter2D.Arc(contentRect.center, radius, new Angle(0f, AngleUnit.Degree), new Angle(360f, AngleUnit.Degree), ArcDirection.Clockwise);
		painter2D.Fill(FillRule.NonZero);
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x00045D1A File Offset: 0x00043F1A
	public void RemovePuck(Puck puck)
	{
		if (!puck)
		{
			return;
		}
		if (!this.puckVisualElementMap.ContainsKey(puck))
		{
			return;
		}
		this.content.Remove(this.puckVisualElementMap[puck]);
		this.puckVisualElementMap.Remove(puck);
	}

	// Token: 0x06000EB7 RID: 3767 RVA: 0x00045D58 File Offset: 0x00043F58
	public void AddStick(Stick stick)
	{
		if (!stick)
		{
			return;
		}
		if (this.stickVisualElementMap.ContainsKey(stick))
		{
			return;
		}
		TemplateContainer templateContainer = this.stickAsset.Instantiate();
		VisualElement visualElement = templateContainer.Q(null, "minimapStick");
		UIUtils.SetTeamClass(visualElement, stick.Player ? stick.Player.Team : PlayerTeam.None);
		this.stickVisualElementMap.Add(stick, visualElement);
		this.content.Add(templateContainer);
		templateContainer.SendToBack();
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x00045DD8 File Offset: 0x00043FD8
	public void RemoveStick(Stick stick)
	{
		if (!stick)
		{
			return;
		}
		if (!this.stickVisualElementMap.ContainsKey(stick))
		{
			return;
		}
		this.content.Remove(this.stickVisualElementMap[stick].parent);
		this.stickVisualElementMap.Remove(stick);
	}

	// Token: 0x06000EB9 RID: 3769 RVA: 0x00045E28 File Offset: 0x00044028
	private Vector2 WorldPositionToMinimapPosition(Vector3 position, Bounds bounds)
	{
		Vector2 vector = new Vector2((position.x + bounds.center.x) / bounds.size.x, (position.z + bounds.center.z) / bounds.size.z);
		Vector2 vector2 = new Vector2(this.content.resolvedStyle.width, this.content.resolvedStyle.height);
		return new Vector2(vector2.x * vector.x, vector2.y * vector.y);
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x00045EC2 File Offset: 0x000440C2
	public void SetOpacity(float opacity)
	{
		if (this.minimap == null)
		{
			return;
		}
		this.minimap.style.opacity = opacity;
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x00045EE4 File Offset: 0x000440E4
	public void SetPosition(Vector2 position)
	{
		if (this.minimap == null)
		{
			return;
		}
		Length x = new Length(Utils.Map(position.x, 0f, 100f, 0f, -100f), LengthUnit.Percent);
		Length y = new Length(Utils.Map(position.y, 0f, 100f, 0f, -100f), LengthUnit.Percent);
		Length x2 = new Length(-x.value, LengthUnit.Percent);
		Length y2 = new Length(-y.value, LengthUnit.Percent);
		this.minimap.style.left = new Length(position.x, LengthUnit.Percent);
		this.minimap.style.top = new Length(position.y, LengthUnit.Percent);
		this.minimap.style.translate = new Translate(x, y);
		this.minimap.style.transformOrigin = new TransformOrigin(x2, y2);
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x00045FE2 File Offset: 0x000441E2
	public void SetBackgroundOpacity(float opacity)
	{
		if (this.background == null)
		{
			return;
		}
		this.background.style.opacity = opacity;
	}

	// Token: 0x06000EBD RID: 3773 RVA: 0x00046003 File Offset: 0x00044203
	public void SetScale(float scale)
	{
		if (this.minimap == null)
		{
			return;
		}
		this.minimap.style.scale = new Vector2(scale, scale);
	}

	// Token: 0x04000950 RID: 2384
	[Header("Settings")]
	[SerializeField]
	private int updateRate = 120;

	// Token: 0x04000951 RID: 2385
	[Header("References")]
	[SerializeField]
	private VisualTreeAsset playerAsset;

	// Token: 0x04000952 RID: 2386
	[SerializeField]
	private VisualTreeAsset puckAsset;

	// Token: 0x04000953 RID: 2387
	[SerializeField]
	private VisualTreeAsset stickAsset;

	// Token: 0x04000954 RID: 2388
	[HideInInspector]
	public PlayerTeam Team;

	// Token: 0x04000955 RID: 2389
	[HideInInspector]
	public Bounds Bounds;

	// Token: 0x04000956 RID: 2390
	private VisualElement minimap;

	// Token: 0x04000957 RID: 2391
	private VisualElement background;

	// Token: 0x04000958 RID: 2392
	private VisualElement foreground;

	// Token: 0x04000959 RID: 2393
	private VisualElement content;

	// Token: 0x0400095A RID: 2394
	[TupleElementNames(new string[]
	{
		"Root",
		"Body"
	})]
	private Dictionary<PlayerBody, ValueTuple<VisualElement, VisualElement>> playerBodyVisualElementMap = new Dictionary<PlayerBody, ValueTuple<VisualElement, VisualElement>>();

	// Token: 0x0400095B RID: 2395
	private Dictionary<Puck, VisualElement> puckVisualElementMap = new Dictionary<Puck, VisualElement>();

	// Token: 0x0400095C RID: 2396
	private Dictionary<Stick, VisualElement> stickVisualElementMap = new Dictionary<Stick, VisualElement>();

	// Token: 0x0400095D RID: 2397
	private float updateAccumulator;
}
