using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x020001B0 RID: 432
public class UIAppearance : UIView
{
	// Token: 0x06000D26 RID: 3366 RVA: 0x0003E660 File Offset: 0x0003C860
	public void Initialize(VisualElement rootVisualElement)
	{
		this.ValidateAppearanceItems();
		base.View = rootVisualElement.Query("AppearanceView", null);
		this.appearance = base.View.Query("Appearance", null);
		this.closeIconButton = this.appearance.Query("CloseIconButtonContainer", null).First().Query(null, null);
		this.closeIconButton.clicked += this.OnClickClose;
		this.categoryTabView = this.appearance.Query(null, null);
		this.categoryTabView.activeTabChanged += this.OnCategoryTabChanged;
		this.headTab = this.categoryTabView.Query("HeadTab", null);
		this.headTabView = this.headTab.Query(null, null);
		this.headTabView.activeTabChanged += this.OnSubcategoryTabChanged;
		this.flagsTab = this.headTabView.Query("FlagsTab", null);
		this.flagsRadioButtonGroup = this.flagsTab.Query("AppearanceItemRadioButtonGroup", null);
		this.headgearTab = this.headTabView.Query("HeadgearTab", null);
		this.headgearRadioButtonGroup = this.headgearTab.Query("AppearanceItemRadioButtonGroup", null);
		this.mustachesTab = this.headTabView.Query("MustachesTab", null);
		this.mustachesRadioButtonGroup = this.mustachesTab.Query("AppearanceItemRadioButtonGroup", null);
		this.beardsTab = this.headTabView.Query("BeardsTab", null);
		this.beardsRadioButtonGroup = this.beardsTab.Query("AppearanceItemRadioButtonGroup", null);
		this.bodyTab = this.categoryTabView.Query("BodyTab", null);
		this.bodyTabView = this.bodyTab.Query(null, null);
		this.bodyTabView.activeTabChanged += this.OnSubcategoryTabChanged;
		this.jerseysTab = this.bodyTabView.Query("JerseysTab", null);
		this.jerseysRadioButtonGroup = this.jerseysTab.Query("AppearanceItemRadioButtonGroup", null);
		this.stickTab = this.categoryTabView.Query("StickTab", null);
		this.stickTabView = this.stickTab.Query(null, null);
		this.stickTabView.activeTabChanged += this.OnSubcategoryTabChanged;
		this.stickSkinsTab = this.stickTabView.Query("SkinsTab", null);
		this.stickSkinsRadioButtonGroup = this.stickSkinsTab.Query("AppearanceItemRadioButtonGroup", null);
		this.stickShaftTapesTab = this.stickTabView.Query("ShaftTapesTab", null);
		this.stickShaftTapesRadioButtonGroup = this.stickShaftTapesTab.Query("AppearanceItemRadioButtonGroup", null);
		this.stickBladeTapesTab = this.stickTabView.Query("BladeTapesTab", null);
		this.stickBladeTapesRadioButtonGroup = this.stickBladeTapesTab.Query("AppearanceItemRadioButtonGroup", null);
		this.teamDropdown = this.appearance.Query("TeamInput", null).First().Query(null, null);
		this.teamDropdown.choices = Utils.GetTeamNames();
		this.teamDropdown.value = Utils.GetNameFromTeam(SettingsManager.Team);
		this.teamDropdown.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnTeamDropdownChanged));
		this.roleDropdown = this.appearance.Query("RoleInput", null).First().Query(null, null);
		this.roleDropdown.choices = Utils.GetRoleNames();
		this.roleDropdown.value = Utils.GetNameFromRole(SettingsManager.Role);
		this.roleDropdown.RegisterValueChangedCallback(new EventCallback<ChangeEvent<string>>(this.OnRoleDropdownChanged));
		this.applyForBothTeamsToggle = this.appearance.Query("ApplyForBothTeamsInput", null).First().Query(null, null);
		this.applyForBothTeamsToggle.value = SettingsManager.ApplyForBothTeams;
		this.applyForBothTeamsToggle.RegisterValueChangedCallback(new EventCallback<ChangeEvent<bool>>(this.OnApplyForBothTeamsToggleChanged));
		this.categoryTabView.activeTab = this.headTab;
		this.headTabView.activeTab = this.flagsTab;
		this.PopulateRadioButtonGroups();
	}

	// Token: 0x06000D27 RID: 3367 RVA: 0x0003EB04 File Offset: 0x0003CD04
	public override bool Show()
	{
		bool flag = base.Show();
		if (flag)
		{
			EventManager.TriggerEvent("Event_OnAppearanceShow", new Dictionary<string, object>
			{
				{
					"category",
					this.category
				},
				{
					"subcategory",
					this.categorySubcategoryMap[this.category]
				}
			});
		}
		return flag;
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x0003EB60 File Offset: 0x0003CD60
	public override bool Hide()
	{
		bool flag = base.Hide();
		if (flag)
		{
			EventManager.TriggerEvent("Event_OnAppearanceHide", null);
		}
		return flag;
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x0003EB76 File Offset: 0x0003CD76
	public void SetTeam(PlayerTeam value)
	{
		this.team = value;
		this.StyleRadioButtonGroups();
		this.UpdateRadioButtons();
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x0003EB8B File Offset: 0x0003CD8B
	public void SetRole(PlayerRole value)
	{
		this.role = value;
		this.StyleRadioButtonGroups();
		this.UpdateRadioButtons();
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x0003EBA0 File Offset: 0x0003CDA0
	public void SetApplyForBothTeams(bool value)
	{
		this.applyForBothTeams = value;
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x0003EBA9 File Offset: 0x0003CDA9
	public void SetFlagID(int value)
	{
		this.flagID = value;
		this.UpdateFlagsRadioButtons();
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x0003EBB8 File Offset: 0x0003CDB8
	public void SetHeadgearID(PlayerTeam team, PlayerRole role, int value)
	{
		if (team == PlayerTeam.Blue && role == PlayerRole.Attacker)
		{
			this.headgearIDBlueAttacker = value;
		}
		else if (team == PlayerTeam.Red && role == PlayerRole.Attacker)
		{
			this.headgearIDRedAttacker = value;
		}
		else if (team == PlayerTeam.Blue && role == PlayerRole.Goalie)
		{
			this.headgearIDBlueGoalie = value;
		}
		else if (team == PlayerTeam.Red && role == PlayerRole.Goalie)
		{
			this.headgearIDRedGoalie = value;
		}
		this.UpdateHeadgearRadioButtons();
	}

	// Token: 0x06000D2E RID: 3374 RVA: 0x0003EC0D File Offset: 0x0003CE0D
	public void SetMustacheID(int value)
	{
		this.mustacheID = value;
		this.UpdateMustacheRadioButtons();
	}

	// Token: 0x06000D2F RID: 3375 RVA: 0x0003EC1C File Offset: 0x0003CE1C
	public void SetBeardID(int value)
	{
		this.beardID = value;
		this.UpdateBeardRadioButtons();
	}

	// Token: 0x06000D30 RID: 3376 RVA: 0x0003EC2C File Offset: 0x0003CE2C
	public void SetJerseyID(PlayerTeam team, PlayerRole role, int value)
	{
		if (team == PlayerTeam.Blue && role == PlayerRole.Attacker)
		{
			this.jerseyIDBlueAttacker = value;
		}
		else if (team == PlayerTeam.Red && role == PlayerRole.Attacker)
		{
			this.jerseyIDRedAttacker = value;
		}
		else if (team == PlayerTeam.Blue && role == PlayerRole.Goalie)
		{
			this.jerseyIDBlueGoalie = value;
		}
		else if (team == PlayerTeam.Red && role == PlayerRole.Goalie)
		{
			this.jerseyIDRedGoalie = value;
		}
		this.UpdateJerseyRadioButtons();
	}

	// Token: 0x06000D31 RID: 3377 RVA: 0x0003EC84 File Offset: 0x0003CE84
	public void SetStickSkinID(PlayerTeam team, PlayerRole role, int value)
	{
		if (team == PlayerTeam.Blue && role == PlayerRole.Attacker)
		{
			this.stickSkinIDBlueAttacker = value;
		}
		else if (team == PlayerTeam.Red && role == PlayerRole.Attacker)
		{
			this.stickSkinIDRedAttacker = value;
		}
		else if (team == PlayerTeam.Blue && role == PlayerRole.Goalie)
		{
			this.stickSkinIDBlueGoalie = value;
		}
		else if (team == PlayerTeam.Red && role == PlayerRole.Goalie)
		{
			this.stickSkinIDRedGoalie = value;
		}
		this.UpdateStickSkinRadioButtons();
	}

	// Token: 0x06000D32 RID: 3378 RVA: 0x0003ECDC File Offset: 0x0003CEDC
	public void SetStickShaftTapeID(PlayerTeam team, PlayerRole role, int value)
	{
		if (team == PlayerTeam.Blue && role == PlayerRole.Attacker)
		{
			this.stickShaftTapeIDBlueAttacker = value;
		}
		else if (team == PlayerTeam.Red && role == PlayerRole.Attacker)
		{
			this.stickShaftTapeIDRedAttacker = value;
		}
		else if (team == PlayerTeam.Blue && role == PlayerRole.Goalie)
		{
			this.stickShaftTapeIDBlueGoalie = value;
		}
		else if (team == PlayerTeam.Red && role == PlayerRole.Goalie)
		{
			this.stickShaftTapeIDRedGoalie = value;
		}
		this.UpdateStickShaftTapeRadioButtons();
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x0003ED34 File Offset: 0x0003CF34
	public void SetStickBladeTapeID(PlayerTeam team, PlayerRole role, int value)
	{
		if (team == PlayerTeam.Blue && role == PlayerRole.Attacker)
		{
			this.stickBladeTapeIDBlueAttacker = value;
		}
		else if (team == PlayerTeam.Red && role == PlayerRole.Attacker)
		{
			this.stickBladeTapeIDRedAttacker = value;
		}
		else if (team == PlayerTeam.Blue && role == PlayerRole.Goalie)
		{
			this.stickBladeTapeIDBlueGoalie = value;
		}
		else if (team == PlayerTeam.Red && role == PlayerRole.Goalie)
		{
			this.stickBladeTapeIDRedGoalie = value;
		}
		this.UpdateStickBladeTapeRadioButtons();
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x0003ED8C File Offset: 0x0003CF8C
	public void StyleRadioButtonGroups()
	{
		this.StyleRadioButtonGroup(this.flagsRadioButtonGroup);
		this.StyleRadioButtonGroup(this.headgearRadioButtonGroup);
		this.StyleRadioButtonGroup(this.mustachesRadioButtonGroup);
		this.StyleRadioButtonGroup(this.beardsRadioButtonGroup);
		this.StyleRadioButtonGroup(this.jerseysRadioButtonGroup);
		this.StyleRadioButtonGroup(this.stickSkinsRadioButtonGroup);
		this.StyleRadioButtonGroup(this.stickShaftTapesRadioButtonGroup);
		this.StyleRadioButtonGroup(this.stickBladeTapesRadioButtonGroup);
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x0003EDF9 File Offset: 0x0003CFF9
	public void UpdateRadioButtons()
	{
		this.UpdateFlagsRadioButtons();
		this.UpdateHeadgearRadioButtons();
		this.UpdateMustacheRadioButtons();
		this.UpdateBeardRadioButtons();
		this.UpdateJerseyRadioButtons();
		this.UpdateStickSkinRadioButtons();
		this.UpdateStickShaftTapeRadioButtons();
		this.UpdateStickBladeTapeRadioButtons();
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x0003EE2C File Offset: 0x0003D02C
	private void ValidateAppearanceItems()
	{
		ItemManager.GetItemsByCategories(new string[]
		{
			"flag"
		}).ForEach(delegate(Item item)
		{
			if (!this.flags.Any((AppearanceItem appearanceItem) => appearanceItem.Id == item.id))
			{
				UIAppearance.Logger.Warning(string.Format("Flag item {0} ({1}) is missing from the appearance flags list", item.name, item.id));
			}
		});
		ItemManager.GetItemsByCategories(new string[]
		{
			"headgear"
		}).ForEach(delegate(Item item)
		{
			if (!this.headgear.Any((AppearanceItem appearanceItem) => appearanceItem.Id == item.id))
			{
				UIAppearance.Logger.Warning(string.Format("Headgear item {0} ({1}) is missing from the appearance headgear list", item.name, item.id));
			}
		});
		ItemManager.GetItemsByCategories(new string[]
		{
			"mustache"
		}).ForEach(delegate(Item item)
		{
			if (!this.mustaches.Any((AppearanceItem appearanceItem) => appearanceItem.Id == item.id))
			{
				UIAppearance.Logger.Warning(string.Format("Mustache item {0} ({1}) is missing from the appearance mustaches list", item.name, item.id));
			}
		});
		ItemManager.GetItemsByCategories(new string[]
		{
			"beard"
		}).ForEach(delegate(Item item)
		{
			if (!this.beards.Any((AppearanceItem appearanceItem) => appearanceItem.Id == item.id))
			{
				UIAppearance.Logger.Warning(string.Format("Beard item {0} ({1}) is missing from the appearance beards list", item.name, item.id));
			}
		});
		ItemManager.GetItemsByCategories(new string[]
		{
			"jersey"
		}).ForEach(delegate(Item item)
		{
			if (!this.jerseys.Any((AppearanceItem appearanceItem) => appearanceItem.Id == item.id))
			{
				UIAppearance.Logger.Warning(string.Format("Jersey item {0} ({1}) is missing from the appearance jerseys list", item.name, item.id));
			}
		});
		ItemManager.GetItemsByCategories(new string[]
		{
			"stickSkin"
		}).ForEach(delegate(Item item)
		{
			if (!this.stickSkins.Any((AppearanceItem appearanceItem) => appearanceItem.Id == item.id))
			{
				UIAppearance.Logger.Warning(string.Format("Stick skin item {0} ({1}) is missing from the appearance stick skins list", item.name, item.id));
			}
		});
		ItemManager.GetItemsByCategories(new string[]
		{
			"stickShaftTape"
		}).ForEach(delegate(Item item)
		{
			if (!this.stickShaftTapes.Any((AppearanceItem appearanceItem) => appearanceItem.Id == item.id))
			{
				UIAppearance.Logger.Warning(string.Format("Stick shaft tape item {0} ({1}) is missing from the appearance stick shaft tapes list", item.name, item.id));
			}
		});
		ItemManager.GetItemsByCategories(new string[]
		{
			"stickBladeTape"
		}).ForEach(delegate(Item item)
		{
			if (!this.stickBladeTapes.Any((AppearanceItem appearanceItem) => appearanceItem.Id == item.id))
			{
				UIAppearance.Logger.Warning(string.Format("Stick blade tape item {0} ({1}) is missing from the appearance stick blade tapes list", item.name, item.id));
			}
		});
	}

	// Token: 0x06000D37 RID: 3383 RVA: 0x0003EF5C File Offset: 0x0003D15C
	private void PopulateRadioButtonGroups()
	{
		this.PopulateRadioButtonGroup(this.flagsRadioButtonGroup, this.flags);
		this.PopulateRadioButtonGroup(this.headgearRadioButtonGroup, this.headgear);
		this.PopulateRadioButtonGroup(this.mustachesRadioButtonGroup, this.mustaches);
		this.PopulateRadioButtonGroup(this.beardsRadioButtonGroup, this.beards);
		this.PopulateRadioButtonGroup(this.jerseysRadioButtonGroup, this.jerseys);
		this.PopulateRadioButtonGroup(this.stickSkinsRadioButtonGroup, this.stickSkins);
		this.PopulateRadioButtonGroup(this.stickShaftTapesRadioButtonGroup, this.stickShaftTapes);
		this.PopulateRadioButtonGroup(this.stickBladeTapesRadioButtonGroup, this.stickBladeTapes);
	}

	// Token: 0x06000D38 RID: 3384 RVA: 0x0003EFFC File Offset: 0x0003D1FC
	private void PopulateRadioButtonGroup(RadioButtonGroup radioButtonGroup, List<AppearanceItem> appearanceItems)
	{
		VisualElement visualElement = radioButtonGroup.Query("AppearanceItemList", null);
		visualElement.Clear();
		foreach (AppearanceItem appearanceItem in appearanceItems)
		{
			RadioButton radioButton = this.appearanceItemAsset.Instantiate().Query("AppearanceItemRadioButton", null);
			Button button = radioButton.Query("PurchaseButton", null).First().Query(null, null);
			Item item;
			if (appearanceItem.Id == -1)
			{
				item = new Item
				{
					id = -1,
					name = "NONE"
				};
			}
			else
			{
				item = ItemManager.GetItemById(appearanceItem.Id);
				if (item == null)
				{
					UIAppearance.Logger.Error(string.Format("Could not populate appearance item with ID {0} because the item was not found in ItemManager", appearanceItem.Id));
					continue;
				}
				button.RegisterCallback<ClickEvent>(delegate(ClickEvent _)
				{
					this.OnClickPurchase(item);
				}, TrickleDown.NoTrickleDown);
				button.text = "BUY $" + ((float)item.price / 100f).ToString("F2", CultureInfo.InvariantCulture);
			}
			radioButton.label = item.name.ToUpper();
			radioButton.userData = new Dictionary<string, object>
			{
				{
					"item",
					item
				}
			};
			radioButton.RegisterCallback<ClickEvent>(delegate(ClickEvent _)
			{
				this.OnClickAppearanceItem(item);
			}, TrickleDown.NoTrickleDown);
			visualElement.Add(radioButton);
		}
	}

	// Token: 0x06000D39 RID: 3385 RVA: 0x0003F1C0 File Offset: 0x0003D3C0
	private void StyleRadioButtonGroup(RadioButtonGroup radioButtonGroup)
	{
		VisualElement visualElement = radioButtonGroup.Query("AppearanceItemList", null);
		visualElement.hierarchy.Sort(delegate(VisualElement a, VisualElement b)
		{
			Item item = (a.userData as Dictionary<string, object>)["item"] as Item;
			Item item2 = (b.userData as Dictionary<string, object>)["item"] as Item;
			if ((item.IsOwned && !item2.IsOwned) || item.id == -1)
			{
				return -1;
			}
			if ((!item.IsOwned && item2.IsOwned) || item2.id == -1)
			{
				return 1;
			}
			return string.Compare(item.name, item2.name, StringComparison.OrdinalIgnoreCase);
		});
		foreach (RadioButton radioButton in visualElement.Query(null, null).ToList())
		{
			this.StyleRadioButton(radioButton);
		}
	}

	// Token: 0x06000D3A RID: 3386 RVA: 0x0003F25C File Offset: 0x0003D45C
	private void StyleRadioButton(RadioButton radioButton)
	{
		Item item = (radioButton.userData as Dictionary<string, object>)["item"] as Item;
		bool flag = (this.role == PlayerRole.Attacker) ? item.IsAttackerItem : item.IsGoalieItem;
		bool flag2 = (item.IsUnlisted && !item.IsPurchased) || !flag;
		radioButton.EnableInClassList("owned", item.IsOwned);
		radioButton.style.display = (flag2 ? DisplayStyle.None : DisplayStyle.Flex);
	}

	// Token: 0x06000D3B RID: 3387 RVA: 0x0003F2DC File Offset: 0x0003D4DC
	private void UpdateFlagsRadioButtons()
	{
		List<RadioButton> radioButtons = this.flagsRadioButtonGroup.Query(null, null).ToList();
		radioButtons.ForEach(delegate(RadioButton radioButton)
		{
			if (((radioButton.userData as Dictionary<string, object>)["item"] as Item).id == this.flagID)
			{
				this.flagsRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
			}
		});
	}

	// Token: 0x06000D3C RID: 3388 RVA: 0x0003F328 File Offset: 0x0003D528
	private void UpdateHeadgearRadioButtons()
	{
		List<RadioButton> radioButtons = this.headgearRadioButtonGroup.Query(null, null).ToList();
		radioButtons.ForEach(delegate(RadioButton radioButton)
		{
			Item item = (radioButton.userData as Dictionary<string, object>)["item"] as Item;
			if (this.team == PlayerTeam.Blue && this.role == PlayerRole.Attacker && item.id == this.headgearIDBlueAttacker)
			{
				this.headgearRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Red && this.role == PlayerRole.Attacker && item.id == this.headgearIDRedAttacker)
			{
				this.headgearRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Blue && this.role == PlayerRole.Goalie && item.id == this.headgearIDBlueGoalie)
			{
				this.headgearRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Red && this.role == PlayerRole.Goalie && item.id == this.headgearIDRedGoalie)
			{
				this.headgearRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
			}
		});
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x0003F374 File Offset: 0x0003D574
	private void UpdateMustacheRadioButtons()
	{
		List<RadioButton> radioButtons = this.mustachesRadioButtonGroup.Query(null, null).ToList();
		radioButtons.ForEach(delegate(RadioButton radioButton)
		{
			if (((radioButton.userData as Dictionary<string, object>)["item"] as Item).id == this.mustacheID)
			{
				this.mustachesRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
			}
		});
	}

	// Token: 0x06000D3E RID: 3390 RVA: 0x0003F3C0 File Offset: 0x0003D5C0
	private void UpdateBeardRadioButtons()
	{
		List<RadioButton> radioButtons = this.beardsRadioButtonGroup.Query(null, null).ToList();
		radioButtons.ForEach(delegate(RadioButton radioButton)
		{
			if (((radioButton.userData as Dictionary<string, object>)["item"] as Item).id == this.beardID)
			{
				this.beardsRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
			}
		});
	}

	// Token: 0x06000D3F RID: 3391 RVA: 0x0003F40C File Offset: 0x0003D60C
	private void UpdateJerseyRadioButtons()
	{
		List<RadioButton> radioButtons = this.jerseysRadioButtonGroup.Query(null, null).ToList();
		radioButtons.ForEach(delegate(RadioButton radioButton)
		{
			Item item = (radioButton.userData as Dictionary<string, object>)["item"] as Item;
			if (this.team == PlayerTeam.Blue && this.role == PlayerRole.Attacker && item.id == this.jerseyIDBlueAttacker)
			{
				this.jerseysRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Red && this.role == PlayerRole.Attacker && item.id == this.jerseyIDRedAttacker)
			{
				this.jerseysRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Blue && this.role == PlayerRole.Goalie && item.id == this.jerseyIDBlueGoalie)
			{
				this.jerseysRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Red && this.role == PlayerRole.Goalie && item.id == this.jerseyIDRedGoalie)
			{
				this.jerseysRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
			}
		});
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x0003F458 File Offset: 0x0003D658
	private void UpdateStickSkinRadioButtons()
	{
		List<RadioButton> radioButtons = this.stickSkinsRadioButtonGroup.Query(null, null).ToList();
		radioButtons.ForEach(delegate(RadioButton radioButton)
		{
			Item item = (radioButton.userData as Dictionary<string, object>)["item"] as Item;
			if (this.team == PlayerTeam.Blue && this.role == PlayerRole.Attacker && item.id == this.stickSkinIDBlueAttacker)
			{
				this.stickSkinsRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Red && this.role == PlayerRole.Attacker && item.id == this.stickSkinIDRedAttacker)
			{
				this.stickSkinsRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Blue && this.role == PlayerRole.Goalie && item.id == this.stickSkinIDBlueGoalie)
			{
				this.stickSkinsRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Red && this.role == PlayerRole.Goalie && item.id == this.stickSkinIDRedGoalie)
			{
				this.stickSkinsRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
			}
		});
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x0003F4A4 File Offset: 0x0003D6A4
	private void UpdateStickShaftTapeRadioButtons()
	{
		List<RadioButton> radioButtons = this.stickShaftTapesRadioButtonGroup.Query(null, null).ToList();
		radioButtons.ForEach(delegate(RadioButton radioButton)
		{
			Item item = (radioButton.userData as Dictionary<string, object>)["item"] as Item;
			if (this.team == PlayerTeam.Blue && this.role == PlayerRole.Attacker && item.id == this.stickShaftTapeIDBlueAttacker)
			{
				this.stickShaftTapesRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Red && this.role == PlayerRole.Attacker && item.id == this.stickShaftTapeIDRedAttacker)
			{
				this.stickShaftTapesRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Blue && this.role == PlayerRole.Goalie && item.id == this.stickShaftTapeIDBlueGoalie)
			{
				this.stickShaftTapesRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Red && this.role == PlayerRole.Goalie && item.id == this.stickShaftTapeIDRedGoalie)
			{
				this.stickShaftTapesRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
			}
		});
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x0003F4F0 File Offset: 0x0003D6F0
	private void UpdateStickBladeTapeRadioButtons()
	{
		List<RadioButton> radioButtons = this.stickBladeTapesRadioButtonGroup.Query(null, null).ToList();
		radioButtons.ForEach(delegate(RadioButton radioButton)
		{
			Item item = (radioButton.userData as Dictionary<string, object>)["item"] as Item;
			if (this.team == PlayerTeam.Blue && this.role == PlayerRole.Attacker && item.id == this.stickBladeTapeIDBlueAttacker)
			{
				this.stickBladeTapesRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Red && this.role == PlayerRole.Attacker && item.id == this.stickBladeTapeIDRedAttacker)
			{
				this.stickBladeTapesRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Blue && this.role == PlayerRole.Goalie && item.id == this.stickBladeTapeIDBlueGoalie)
			{
				this.stickBladeTapesRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
				return;
			}
			if (this.team == PlayerTeam.Red && this.role == PlayerRole.Goalie && item.id == this.stickBladeTapeIDRedGoalie)
			{
				this.stickBladeTapesRadioButtonGroup.value = radioButtons.IndexOf(radioButton);
			}
		});
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x0003F53C File Offset: 0x0003D73C
	private void OnClickClose()
	{
		EventManager.TriggerEvent("Event_OnAppearanceClickClose", null);
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x0003F549 File Offset: 0x0003D749
	private void OnClickPurchase(Item item)
	{
		EventManager.TriggerEvent("Event_OnAppearanceClickPurchaseItem", new Dictionary<string, object>
		{
			{
				"item",
				item
			}
		});
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x0003F566 File Offset: 0x0003D766
	private void OnTeamDropdownChanged(ChangeEvent<string> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnAppearanceTeamChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x0003F588 File Offset: 0x0003D788
	private void OnRoleDropdownChanged(ChangeEvent<string> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnAppearanceRoleChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x0003F5AA File Offset: 0x0003D7AA
	private void OnApplyForBothTeamsToggleChanged(ChangeEvent<bool> changeEvent)
	{
		EventManager.TriggerEvent("Event_OnAppearanceApplyForBothTeamsChanged", new Dictionary<string, object>
		{
			{
				"value",
				changeEvent.newValue
			}
		});
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x0003F5D4 File Offset: 0x0003D7D4
	private void OnCategoryTabChanged(Tab oldTab, Tab newTab)
	{
		string name = newTab.name;
		if (!(name == "HeadTab"))
		{
			if (!(name == "BodyTab"))
			{
				if (name == "StickTab")
				{
					this.category = AppearanceCategory.Stick;
				}
			}
			else
			{
				this.category = AppearanceCategory.Body;
			}
		}
		else
		{
			this.category = AppearanceCategory.Head;
		}
		EventManager.TriggerEvent("Event_OnAppearanceCategoryChanged", new Dictionary<string, object>
		{
			{
				"category",
				this.category
			},
			{
				"subcategory",
				this.categorySubcategoryMap[this.category]
			}
		});
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x0003F670 File Offset: 0x0003D870
	private void OnSubcategoryTabChanged(Tab oldTab, Tab newTab)
	{
		Dictionary<AppearanceCategory, AppearanceSubcategory> dictionary = this.categorySubcategoryMap;
		AppearanceCategory key = this.category;
		string name = newTab.name;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
		AppearanceSubcategory value;
		if (num <= 2933584645U)
		{
			if (num <= 154799819U)
			{
				if (num != 108854953U)
				{
					if (num == 154799819U)
					{
						if (name == "FlagsTab")
						{
							value = AppearanceSubcategory.Flags;
							goto IL_14A;
						}
					}
				}
				else if (name == "JerseysTab")
				{
					value = AppearanceSubcategory.Jerseys;
					goto IL_14A;
				}
			}
			else if (num != 2528141619U)
			{
				if (num == 2933584645U)
				{
					if (name == "BladeTapesTab")
					{
						value = AppearanceSubcategory.StickBladeTapes;
						goto IL_14A;
					}
				}
			}
			else if (name == "ShaftTapesTab")
			{
				value = AppearanceSubcategory.StickShaftTapes;
				goto IL_14A;
			}
		}
		else if (num <= 3172671651U)
		{
			if (num != 3105626632U)
			{
				if (num == 3172671651U)
				{
					if (name == "MustachesTab")
					{
						value = AppearanceSubcategory.Mustaches;
						goto IL_14A;
					}
				}
			}
			else if (name == "SkinsTab")
			{
				value = AppearanceSubcategory.StickSkins;
				goto IL_14A;
			}
		}
		else if (num != 3753505521U)
		{
			if (num == 3900078259U)
			{
				if (name == "HeadgearTab")
				{
					value = AppearanceSubcategory.Headgear;
					goto IL_14A;
				}
			}
		}
		else if (name == "BeardsTab")
		{
			value = AppearanceSubcategory.Beards;
			goto IL_14A;
		}
		value = this.categorySubcategoryMap[this.category];
		IL_14A:
		dictionary[key] = value;
		EventManager.TriggerEvent("Event_OnAppearanceCategoryChanged", new Dictionary<string, object>
		{
			{
				"category",
				this.category
			},
			{
				"subcategory",
				this.categorySubcategoryMap[this.category]
			}
		});
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x0003F818 File Offset: 0x0003DA18
	private void OnClickAppearanceItem(Item item)
	{
		EventManager.TriggerEvent("Event_OnAppearanceClickItem", new Dictionary<string, object>
		{
			{
				"item",
				item
			},
			{
				"category",
				this.category
			},
			{
				"subcategory",
				this.categorySubcategoryMap[this.category]
			},
			{
				"team",
				this.team
			},
			{
				"role",
				this.role
			}
		});
	}

	// Token: 0x04000840 RID: 2112
	private static readonly global::Logger Logger = new global::Logger("UIAppearance");

	// Token: 0x04000841 RID: 2113
	[Header("Settings")]
	[SerializeField]
	private List<AppearanceItem> flags = new List<AppearanceItem>();

	// Token: 0x04000842 RID: 2114
	[SerializeField]
	private List<AppearanceItem> headgear = new List<AppearanceItem>();

	// Token: 0x04000843 RID: 2115
	[SerializeField]
	private List<AppearanceItem> mustaches = new List<AppearanceItem>();

	// Token: 0x04000844 RID: 2116
	[SerializeField]
	private List<AppearanceItem> beards = new List<AppearanceItem>();

	// Token: 0x04000845 RID: 2117
	[SerializeField]
	private List<AppearanceItem> jerseys = new List<AppearanceItem>();

	// Token: 0x04000846 RID: 2118
	[SerializeField]
	private List<AppearanceItem> stickSkins = new List<AppearanceItem>();

	// Token: 0x04000847 RID: 2119
	[SerializeField]
	private List<AppearanceItem> stickShaftTapes = new List<AppearanceItem>();

	// Token: 0x04000848 RID: 2120
	[SerializeField]
	private List<AppearanceItem> stickBladeTapes = new List<AppearanceItem>();

	// Token: 0x04000849 RID: 2121
	[Header("References")]
	public VisualTreeAsset appearanceItemAsset;

	// Token: 0x0400084A RID: 2122
	private AppearanceCategory category;

	// Token: 0x0400084B RID: 2123
	private Dictionary<AppearanceCategory, AppearanceSubcategory> categorySubcategoryMap = new Dictionary<AppearanceCategory, AppearanceSubcategory>
	{
		{
			AppearanceCategory.Head,
			AppearanceSubcategory.Flags
		},
		{
			AppearanceCategory.Body,
			AppearanceSubcategory.Jerseys
		},
		{
			AppearanceCategory.Stick,
			AppearanceSubcategory.StickSkins
		}
	};

	// Token: 0x0400084C RID: 2124
	private PlayerTeam team;

	// Token: 0x0400084D RID: 2125
	private PlayerRole role;

	// Token: 0x0400084E RID: 2126
	private bool applyForBothTeams;

	// Token: 0x0400084F RID: 2127
	private int flagID;

	// Token: 0x04000850 RID: 2128
	private int headgearIDBlueAttacker;

	// Token: 0x04000851 RID: 2129
	private int headgearIDRedAttacker;

	// Token: 0x04000852 RID: 2130
	private int headgearIDBlueGoalie;

	// Token: 0x04000853 RID: 2131
	private int headgearIDRedGoalie;

	// Token: 0x04000854 RID: 2132
	private int mustacheID;

	// Token: 0x04000855 RID: 2133
	private int beardID;

	// Token: 0x04000856 RID: 2134
	private int jerseyIDBlueAttacker;

	// Token: 0x04000857 RID: 2135
	private int jerseyIDRedAttacker;

	// Token: 0x04000858 RID: 2136
	private int jerseyIDBlueGoalie;

	// Token: 0x04000859 RID: 2137
	private int jerseyIDRedGoalie;

	// Token: 0x0400085A RID: 2138
	private int stickSkinIDBlueAttacker;

	// Token: 0x0400085B RID: 2139
	private int stickSkinIDRedAttacker;

	// Token: 0x0400085C RID: 2140
	private int stickSkinIDBlueGoalie;

	// Token: 0x0400085D RID: 2141
	private int stickSkinIDRedGoalie;

	// Token: 0x0400085E RID: 2142
	private int stickShaftTapeIDBlueAttacker;

	// Token: 0x0400085F RID: 2143
	private int stickShaftTapeIDRedAttacker;

	// Token: 0x04000860 RID: 2144
	private int stickShaftTapeIDBlueGoalie;

	// Token: 0x04000861 RID: 2145
	private int stickShaftTapeIDRedGoalie;

	// Token: 0x04000862 RID: 2146
	private int stickBladeTapeIDBlueAttacker;

	// Token: 0x04000863 RID: 2147
	private int stickBladeTapeIDRedAttacker;

	// Token: 0x04000864 RID: 2148
	private int stickBladeTapeIDBlueGoalie;

	// Token: 0x04000865 RID: 2149
	private int stickBladeTapeIDRedGoalie;

	// Token: 0x04000866 RID: 2150
	private VisualElement appearance;

	// Token: 0x04000867 RID: 2151
	private IconButton closeIconButton;

	// Token: 0x04000868 RID: 2152
	private TabView categoryTabView;

	// Token: 0x04000869 RID: 2153
	private Tab headTab;

	// Token: 0x0400086A RID: 2154
	private Tab bodyTab;

	// Token: 0x0400086B RID: 2155
	private Tab stickTab;

	// Token: 0x0400086C RID: 2156
	private TabView headTabView;

	// Token: 0x0400086D RID: 2157
	private Tab flagsTab;

	// Token: 0x0400086E RID: 2158
	private Tab headgearTab;

	// Token: 0x0400086F RID: 2159
	private Tab mustachesTab;

	// Token: 0x04000870 RID: 2160
	private Tab beardsTab;

	// Token: 0x04000871 RID: 2161
	private TabView bodyTabView;

	// Token: 0x04000872 RID: 2162
	private Tab jerseysTab;

	// Token: 0x04000873 RID: 2163
	private TabView stickTabView;

	// Token: 0x04000874 RID: 2164
	private Tab stickSkinsTab;

	// Token: 0x04000875 RID: 2165
	private Tab stickShaftTapesTab;

	// Token: 0x04000876 RID: 2166
	private Tab stickBladeTapesTab;

	// Token: 0x04000877 RID: 2167
	private Toggle applyForBothTeamsToggle;

	// Token: 0x04000878 RID: 2168
	private DropdownField teamDropdown;

	// Token: 0x04000879 RID: 2169
	private DropdownField roleDropdown;

	// Token: 0x0400087A RID: 2170
	private RadioButtonGroup flagsRadioButtonGroup;

	// Token: 0x0400087B RID: 2171
	private RadioButtonGroup headgearRadioButtonGroup;

	// Token: 0x0400087C RID: 2172
	private RadioButtonGroup mustachesRadioButtonGroup;

	// Token: 0x0400087D RID: 2173
	private RadioButtonGroup beardsRadioButtonGroup;

	// Token: 0x0400087E RID: 2174
	private RadioButtonGroup jerseysRadioButtonGroup;

	// Token: 0x0400087F RID: 2175
	private RadioButtonGroup stickSkinsRadioButtonGroup;

	// Token: 0x04000880 RID: 2176
	private RadioButtonGroup stickShaftTapesRadioButtonGroup;

	// Token: 0x04000881 RID: 2177
	private RadioButtonGroup stickBladeTapesRadioButtonGroup;
}
