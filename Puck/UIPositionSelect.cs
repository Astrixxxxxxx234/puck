using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x02000200 RID: 512
public class UIPositionSelect : UIView
{
	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000FE1 RID: 4065 RVA: 0x0004B0C6 File Offset: 0x000492C6
	// (set) Token: 0x06000FE2 RID: 4066 RVA: 0x0004B0D0 File Offset: 0x000492D0
	public PlayerTeam Team
	{
		get
		{
			return this.team;
		}
		set
		{
			if (this.team == value)
			{
				return;
			}
			PlayerTeam oldTeam = this.team;
			this.team = value;
			this.OnTeamChanged(oldTeam, this.team);
		}
	}

	// Token: 0x06000FE3 RID: 4067 RVA: 0x0004B102 File Offset: 0x00049302
	public void Initialize(VisualElement rootVisualElement)
	{
		this.RootVisualElement = rootVisualElement;
		base.View = rootVisualElement.Query("PositionsView", null);
		this.positions = base.View.Query("Positions", null);
	}

	// Token: 0x06000FE4 RID: 4068 RVA: 0x0004B140 File Offset: 0x00049340
	private void Update()
	{
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return;
		}
		this.updateAccumulator += Time.deltaTime;
		if (this.updateAccumulator < 1f / (float)this.updateRate)
		{
			return;
		}
		this.updateAccumulator = 0f;
		foreach (KeyValuePair<PlayerPosition, VisualElement> keyValuePair in this.playerPositionVisualElementMap)
		{
			PlayerPosition key = keyValuePair.Key;
			VisualElement value = keyValuePair.Value;
			if (!(key == null))
			{
				this.PositionWorldToScreen(value, key);
			}
		}
	}

	// Token: 0x06000FE5 RID: 4069 RVA: 0x0004B1E8 File Offset: 0x000493E8
	public void AddPosition(PlayerPosition playerPosition)
	{
		if (this.playerPositionVisualElementMap.ContainsKey(playerPosition))
		{
			return;
		}
		VisualElement visualElement = this.positionAsset.Instantiate();
		visualElement.Query(null, null).RegisterCallback<ClickEvent>(delegate(ClickEvent e)
		{
			this.OnPositionClicked(playerPosition);
		}, TrickleDown.NoTrickleDown);
		this.positions.Add(visualElement);
		this.playerPositionVisualElementMap.Add(playerPosition, visualElement);
		this.StylePosition(playerPosition);
	}

	// Token: 0x06000FE6 RID: 4070 RVA: 0x0004B274 File Offset: 0x00049474
	public void StylePosition(PlayerPosition playerPosition)
	{
		if (!this.playerPositionVisualElementMap.ContainsKey(playerPosition))
		{
			return;
		}
		VisualElement visualElement = this.playerPositionVisualElementMap[playerPosition];
		VisualElement visualElement2 = visualElement.Query("Position", null);
		Button button = visualElement.Query(null, null);
		Label label = visualElement.Query("UsernameLabel", null);
		UIUtils.SetTeamClass(visualElement2, playerPosition.Team);
		visualElement2.EnableInClassList("claimed", playerPosition.IsClaimed);
		button.text = playerPosition.Name.ToString();
		if (playerPosition.IsClaimed)
		{
			label.text = playerPosition.ClaimedByPlayer.Username.Value.ToString();
		}
		else
		{
			label.text = null;
		}
		visualElement.style.display = ((this.Team == playerPosition.Team) ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06000FE7 RID: 4071 RVA: 0x0004B352 File Offset: 0x00049552
	public void RemovePosition(PlayerPosition playerPosition)
	{
		if (!this.playerPositionVisualElementMap.ContainsKey(playerPosition))
		{
			return;
		}
		this.positions.Remove(this.playerPositionVisualElementMap[playerPosition]);
		this.playerPositionVisualElementMap.Remove(playerPosition);
	}

	// Token: 0x06000FE8 RID: 4072 RVA: 0x0004B388 File Offset: 0x00049588
	private void PositionWorldToScreen(VisualElement positionVisualElement, PlayerPosition playerPosition)
	{
		if (Camera.main == null)
		{
			return;
		}
		Vector3 vector = Camera.main.WorldToScreenPoint(playerPosition.transform.position);
		vector.y = (float)Screen.height - vector.y;
		RuntimePanelUtils.ScreenToPanel(this.RootVisualElement.panel, vector);
		Vector2 vector2 = RuntimePanelUtils.ScreenToPanel(this.RootVisualElement.panel, vector);
		if (vector.z < 0f)
		{
			positionVisualElement.style.visibility = Visibility.Hidden;
			return;
		}
		positionVisualElement.style.visibility = Visibility.Visible;
		positionVisualElement.style.left = vector2.x;
		positionVisualElement.style.top = vector2.y;
	}

	// Token: 0x06000FE9 RID: 4073 RVA: 0x0004B459 File Offset: 0x00049659
	private void OnPositionClicked(PlayerPosition playerPosition)
	{
		EventManager.TriggerEvent("Event_OnPositionSelectClickPosition", new Dictionary<string, object>
		{
			{
				"playerPosition",
				playerPosition
			}
		});
	}

	// Token: 0x06000FEA RID: 4074 RVA: 0x0004B478 File Offset: 0x00049678
	private void OnTeamChanged(PlayerTeam oldTeam, PlayerTeam newTeam)
	{
		foreach (PlayerPosition playerPosition in this.playerPositionVisualElementMap.Keys)
		{
			this.StylePosition(playerPosition);
		}
	}

	// Token: 0x04000A0D RID: 2573
	[Header("Settings")]
	[SerializeField]
	private int updateRate = 30;

	// Token: 0x04000A0E RID: 2574
	[Header("References")]
	[SerializeField]
	private VisualTreeAsset positionAsset;

	// Token: 0x04000A0F RID: 2575
	private PlayerTeam team;

	// Token: 0x04000A10 RID: 2576
	private Dictionary<PlayerPosition, VisualElement> playerPositionVisualElementMap = new Dictionary<PlayerPosition, VisualElement>();

	// Token: 0x04000A11 RID: 2577
	private float updateAccumulator;

	// Token: 0x04000A12 RID: 2578
	private VisualElement positions;
}
