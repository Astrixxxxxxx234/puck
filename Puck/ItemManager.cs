using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using UnityEngine;

// Token: 0x020000E5 RID: 229
public static class ItemManager
{
	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000724 RID: 1828 RVA: 0x00026AC4 File Offset: 0x00024CC4
	// (set) Token: 0x06000725 RID: 1829 RVA: 0x00026ACB File Offset: 0x00024CCB
	public static List<Item> Items { get; private set; } = new List<Item>();

	// Token: 0x06000726 RID: 1830 RVA: 0x00026AD3 File Offset: 0x00024CD3
	static ItemManager()
	{
		ItemManager.LoadItems();
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00026AF3 File Offset: 0x00024CF3
	public static void Initialize()
	{
		ItemManagerController.Initialize();
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x00026AFA File Offset: 0x00024CFA
	public static void Dispose()
	{
		ItemManagerController.Dispose();
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x00026B04 File Offset: 0x00024D04
	public static Item GetItemById(int id)
	{
		return ItemManager.Items.Find((Item item) => item.id == id);
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x00026B34 File Offset: 0x00024D34
	public static bool IsItemOwned(int itemId, PlayerData playerData)
	{
		Item itemById = ItemManager.GetItemById(itemId);
		return itemById == null || itemById.price == 0 || (((playerData != null) ? playerData.items : null) != null && playerData.items.Any((PlayerItem ownedItem) => ownedItem.itemId == itemId));
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x00026B90 File Offset: 0x00024D90
	public static int ValidateEquippedItem(int itemId, int defaultItemId, Func<Item, bool> isValidForSlot, PlayerData playerData)
	{
		if (itemId == -1)
		{
			if (defaultItemId == -1)
			{
				return -1;
			}
		}
		else
		{
			Item itemById = ItemManager.GetItemById(itemId);
			if (itemById != null && isValidForSlot(itemById) && ItemManager.IsItemOwned(itemId, playerData))
			{
				return itemId;
			}
		}
		ItemManager.Logger.Warning(string.Format("Player {0} equipped invalid or unowned item {1}; resetting to default {2}", (playerData != null) ? playerData.steamId : null, itemId, defaultItemId));
		return defaultItemId;
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00026BF4 File Offset: 0x00024DF4
	public static List<Item> GetItemsByCategories(string[] categories)
	{
		Predicate<string> <>9__1;
		return ItemManager.Items.FindAll(delegate(Item item)
		{
			string[] categories2 = item.categories;
			Predicate<string> match;
			if ((match = <>9__1) == null)
			{
				match = (<>9__1 = ((string itemCategory) => Array.IndexOf<string>(categories, itemCategory) >= 0));
			}
			return Array.Exists<string>(categories2, match);
		});
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x00026C24 File Offset: 0x00024E24
	private static void LoadItems()
	{
		try
		{
			ItemManager.Items = JsonSerializer.Deserialize<List<Item>>(Resources.Load<TextAsset>("items").text, null);
			ItemManager.Logger.Info(string.Format("Loaded {0} items", ItemManager.Items.Count));
		}
		catch (Exception ex)
		{
			ItemManager.Logger.Error("Error loading items asset: " + ex.Message);
		}
	}

	// Token: 0x040004A8 RID: 1192
	private static readonly global::Logger Logger = new global::Logger("ItemManager");
}
