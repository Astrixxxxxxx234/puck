using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x0200021C RID: 540
public class UIUsernames : UIView
{
	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06001139 RID: 4409 RVA: 0x00051BFD File Offset: 0x0004FDFD
	[HideInInspector]
	public float MaximumDistance
	{
		get
		{
			return Mathf.Max(this.Bounds.size.x, this.Bounds.size.z);
		}
	}

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x0600113A RID: 4410 RVA: 0x00051C24 File Offset: 0x0004FE24
	[HideInInspector]
	public float FadeRange
	{
		get
		{
			return this.MaximumDistance / 4f;
		}
	}

	// Token: 0x0600113B RID: 4411 RVA: 0x00051C34 File Offset: 0x0004FE34
	public void Initialize(VisualElement rootVisualElement)
	{
		this.RootVisualElement = rootVisualElement;
		base.View = rootVisualElement.Query("UsernamesView", null);
		this.usernames = base.View.Query("Usernames", null);
		this.usernames.Clear();
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x00051C88 File Offset: 0x0004FE88
	private void Update()
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		foreach (KeyValuePair<PlayerBody, VisualElement> keyValuePair in this.playerBodyVisualElementMap)
		{
			PlayerBody key = keyValuePair.Key;
			VisualElement value = keyValuePair.Value;
			if (!(key == null))
			{
				this.UsernameWorldToScreen(value, key);
			}
		}
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x00051D00 File Offset: 0x0004FF00
	public override bool Show()
	{
		return SettingsManager.ShowPlayerUsernames && base.Show();
	}

	// Token: 0x0600113E RID: 4414 RVA: 0x00051D14 File Offset: 0x0004FF14
	public void AddPlayerBody(PlayerBody playerBody)
	{
		TemplateContainer templateContainer = this.playerUsernameAsset.Instantiate();
		this.playerBodyVisualElementMap.Add(playerBody, templateContainer);
		this.StyleUsername(playerBody);
		this.usernames.Add(templateContainer);
	}

	// Token: 0x0600113F RID: 4415 RVA: 0x00051D50 File Offset: 0x0004FF50
	public void RemovePlayerBody(PlayerBody playerBody)
	{
		if (!this.playerBodyVisualElementMap.ContainsKey(playerBody))
		{
			return;
		}
		VisualElement element = this.playerBodyVisualElementMap[playerBody];
		this.playerBodyVisualElementMap.Remove(playerBody);
		this.usernames.Remove(element);
	}

	// Token: 0x06001140 RID: 4416 RVA: 0x00051D94 File Offset: 0x0004FF94
	public void StyleUsername(PlayerBody playerBody)
	{
		if (!this.playerBodyVisualElementMap.ContainsKey(playerBody))
		{
			return;
		}
		this.playerBodyVisualElementMap[playerBody].Query("UsernameLabel", null).text = string.Format("#{0} {1}", playerBody.Player.Number.Value, playerBody.Player.Username.Value);
	}

	// Token: 0x06001141 RID: 4417 RVA: 0x00051E08 File Offset: 0x00050008
	private void UsernameWorldToScreen(VisualElement playerVisualElement, PlayerBody playerBody)
	{
		if (Camera.main == null)
		{
			return;
		}
		Vector3 position = Camera.main.transform.position;
		Vector3 position2 = playerBody.transform.position;
		float value = Vector3.Distance(position, position2);
		Vector3 vector = Camera.main.WorldToScreenPoint(position2 + Vector3.up * this.yOffset);
		vector.y = (float)Screen.height - vector.y;
		RuntimePanelUtils.ScreenToPanel(this.RootVisualElement.panel, vector);
		Vector2 vector2 = RuntimePanelUtils.ScreenToPanel(this.RootVisualElement.panel, vector);
		if (vector.z < 0f)
		{
			playerVisualElement.style.display = DisplayStyle.None;
			return;
		}
		float num = Utils.Map(value, this.MaximumDistance * this.FadeThreshold, this.MaximumDistance * this.FadeThreshold + this.FadeRange, 1f, 0f);
		num = Mathf.Clamp01(num);
		playerVisualElement.style.display = DisplayStyle.Flex;
		playerVisualElement.style.left = vector2.x;
		playerVisualElement.style.top = vector2.y;
		playerVisualElement.style.opacity = new StyleFloat(num);
	}

	// Token: 0x04000ABC RID: 2748
	private static readonly global::Logger Logger = new global::Logger("UIUsernames");

	// Token: 0x04000ABD RID: 2749
	[Header("Settings")]
	[SerializeField]
	private float yOffset = 2.5f;

	// Token: 0x04000ABE RID: 2750
	[Header("References")]
	[SerializeField]
	private VisualTreeAsset playerUsernameAsset;

	// Token: 0x04000ABF RID: 2751
	[HideInInspector]
	public float FadeThreshold = 0.5f;

	// Token: 0x04000AC0 RID: 2752
	[HideInInspector]
	public Bounds Bounds;

	// Token: 0x04000AC1 RID: 2753
	private Dictionary<PlayerBody, VisualElement> playerBodyVisualElementMap = new Dictionary<PlayerBody, VisualElement>();

	// Token: 0x04000AC2 RID: 2754
	private VisualElement usernames;
}
