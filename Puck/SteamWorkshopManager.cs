using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DG.Tweening;
using Steamworks;

// Token: 0x02000173 RID: 371
public static class SteamWorkshopManager
{
	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000B65 RID: 2917 RVA: 0x0003651C File Offset: 0x0003471C
	public static string[] ItemIds
	{
		get
		{
			return (from item in SteamWorkshopManager.Items
			select item.Id).ToArray<string>();
		}
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x0003654C File Offset: 0x0003474C
	public static void Initialize()
	{
		SteamWorkshopManager.RegisterCallbacks();
		SteamWorkshopManagerController.Initialize();
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x00036558 File Offset: 0x00034758
	public static void Dispose()
	{
		SteamWorkshopManager.ugcQueryCompletedCallResultMap.Values.ToList<CallResult<SteamUGCQueryCompleted_t>>().ForEach(delegate(CallResult<SteamUGCQueryCompleted_t> callResult)
		{
			callResult.Dispose();
		});
		SteamWorkshopManager.ugcQueryCompletedCallResultMap.Clear();
		SteamWorkshopManager.debouncedGetItemDetailsItemIds.Clear();
		Tween tween = SteamWorkshopManager.getItemDetailsDebounceTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		SteamWorkshopManagerController.Dispose();
		SteamWorkshopManager.UnregisterCallbacks();
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x000365C8 File Offset: 0x000347C8
	private static void RegisterCallbacks()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		if (ApplicationManager.IsDedicatedGameServer)
		{
			SteamWorkshopManager.DownloadItemResult = Callback<DownloadItemResult_t>.CreateGameServer(new Callback<DownloadItemResult_t>.DispatchDelegate(SteamWorkshopManager.OnDownloadItemResult));
			SteamWorkshopManager.UserSubscribedItemsListChanged = Callback<UserSubscribedItemsListChanged_t>.CreateGameServer(new Callback<UserSubscribedItemsListChanged_t>.DispatchDelegate(SteamWorkshopManager.OnUserSubscribedItemsListChanged));
			SteamWorkshopManager.RemoteStorageSubscribePublishedFileResult = Callback<RemoteStorageSubscribePublishedFileResult_t>.CreateGameServer(new Callback<RemoteStorageSubscribePublishedFileResult_t>.DispatchDelegate(SteamWorkshopManager.OnRemoteStorageSubscribePublishedFileResult));
			SteamWorkshopManager.RemoteStorageUnsubscribePublishedFileResult = Callback<RemoteStorageUnsubscribePublishedFileResult_t>.CreateGameServer(new Callback<RemoteStorageUnsubscribePublishedFileResult_t>.DispatchDelegate(SteamWorkshopManager.OnRemoteStorageUnsubscribePublishedFileResult));
			return;
		}
		SteamWorkshopManager.DownloadItemResult = Callback<DownloadItemResult_t>.Create(new Callback<DownloadItemResult_t>.DispatchDelegate(SteamWorkshopManager.OnDownloadItemResult));
		SteamWorkshopManager.UserSubscribedItemsListChanged = Callback<UserSubscribedItemsListChanged_t>.Create(new Callback<UserSubscribedItemsListChanged_t>.DispatchDelegate(SteamWorkshopManager.OnUserSubscribedItemsListChanged));
		SteamWorkshopManager.RemoteStorageSubscribePublishedFileResult = Callback<RemoteStorageSubscribePublishedFileResult_t>.Create(new Callback<RemoteStorageSubscribePublishedFileResult_t>.DispatchDelegate(SteamWorkshopManager.OnRemoteStorageSubscribePublishedFileResult));
		SteamWorkshopManager.RemoteStorageUnsubscribePublishedFileResult = Callback<RemoteStorageUnsubscribePublishedFileResult_t>.Create(new Callback<RemoteStorageUnsubscribePublishedFileResult_t>.DispatchDelegate(SteamWorkshopManager.OnRemoteStorageUnsubscribePublishedFileResult));
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x00036695 File Offset: 0x00034895
	private static void UnregisterCallbacks()
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		SteamWorkshopManager.DownloadItemResult.Unregister();
		SteamWorkshopManager.UserSubscribedItemsListChanged.Unregister();
		SteamWorkshopManager.RemoteStorageSubscribePublishedFileResult.Unregister();
		SteamWorkshopManager.RemoteStorageUnsubscribePublishedFileResult.Unregister();
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x000366C8 File Offset: 0x000348C8
	private static SteamWorkshopItem AddItem(string id, string path = null)
	{
		if (SteamWorkshopManager.GetItemById(id) != null)
		{
			return null;
		}
		SteamWorkshopItem steamWorkshopItem = new SteamWorkshopItem(id, path);
		SteamWorkshopManager.Items.Add(steamWorkshopItem);
		steamWorkshopItem.Initialize();
		EventManager.TriggerEvent("Event_OnSteamWorkshopItemAdded", new Dictionary<string, object>
		{
			{
				"item",
				steamWorkshopItem
			}
		});
		return steamWorkshopItem;
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x00036714 File Offset: 0x00034914
	private static SteamWorkshopItem RemoveItem(string id)
	{
		SteamWorkshopItem itemById = SteamWorkshopManager.GetItemById(id);
		if (itemById == null)
		{
			return null;
		}
		SteamWorkshopManager.Items.Remove(itemById);
		itemById.Dispose();
		EventManager.TriggerEvent("Event_OnSteamWorkshopItemRemoved", new Dictionary<string, object>
		{
			{
				"item",
				itemById
			}
		});
		return itemById;
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x0003675C File Offset: 0x0003495C
	public static SteamWorkshopItem GetItemById(string id)
	{
		return SteamWorkshopManager.Items.Find((SteamWorkshopItem item) => item.Id == id);
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x0003678C File Offset: 0x0003498C
	public static void VerifyIntegrity()
	{
		string[] subscribedItemIds = SteamWorkshopManager.GetSubscribedItemIds();
		string[] array = SteamWorkshopManager.ItemIds.Union(subscribedItemIds).ToArray<string>();
		for (int i = 0; i < array.Length; i++)
		{
			SteamWorkshopManager.VerifyItemIntegrity(array[i]);
		}
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x000367C8 File Offset: 0x000349C8
	public static void VerifyItemIntegrity(string itemId)
	{
		SteamWorkshopItem steamWorkshopItem = SteamWorkshopManager.GetItemById(itemId);
		if (!SteamWorkshopManager.IsItemSubscribed(itemId) && !ApplicationManager.IsDedicatedGameServer)
		{
			if (!SteamWorkshopManager.IsItemSubscribed(itemId) && !ApplicationManager.IsDedicatedGameServer && steamWorkshopItem != null)
			{
				SteamWorkshopManager.RemoveItem(itemId);
			}
			return;
		}
		if (steamWorkshopItem == null)
		{
			steamWorkshopItem = SteamWorkshopManager.AddItem(itemId, null);
		}
		if (!SteamWorkshopManager.IsItemInstalled(itemId))
		{
			steamWorkshopItem.SetState(new Dictionary<string, object>
			{
				{
					"phase",
					SteamWorkshopItemPhase.Downloading
				}
			});
			SteamWorkshopManager.DownloadItem(itemId);
			return;
		}
		if (SteamWorkshopManager.IsItemNeedsUpdate(itemId))
		{
			steamWorkshopItem.SetState(new Dictionary<string, object>
			{
				{
					"phase",
					SteamWorkshopItemPhase.Updating
				}
			});
			SteamWorkshopManager.DownloadItem(itemId);
			return;
		}
		string value;
		if (SteamWorkshopManager.GetItemInstallInfo(itemId, out value))
		{
			steamWorkshopItem.SetState(new Dictionary<string, object>
			{
				{
					"path",
					value
				},
				{
					"phase",
					SteamWorkshopItemPhase.Installed
				}
			});
			return;
		}
		SteamWorkshopManager.Logger.Error("Failed to get install info for item " + itemId);
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x000368B3 File Offset: 0x00034AB3
	public static bool IsItemInstalled(string itemId)
	{
		return SteamManager.IsInitialized && (SteamWorkshopManager.GetItemState(itemId) & 4U) > 0U;
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x000368C9 File Offset: 0x00034AC9
	public static bool IsItemSubscribed(string itemId)
	{
		return SteamManager.IsInitialized && (SteamWorkshopManager.GetItemState(itemId) & 1U) > 0U;
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x000368DF File Offset: 0x00034ADF
	public static bool IsItemNeedsUpdate(string itemId)
	{
		return SteamManager.IsInitialized && (SteamWorkshopManager.GetItemState(itemId) & 8U) > 0U;
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x000368F5 File Offset: 0x00034AF5
	public static uint GetNumSubscribedItems()
	{
		if (!SteamManager.IsInitialized)
		{
			return 0U;
		}
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return SteamGameServerUGC.GetNumSubscribedItems(false);
		}
		return SteamUGC.GetNumSubscribedItems(false);
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x00036914 File Offset: 0x00034B14
	public static string[] GetSubscribedItemIds()
	{
		if (!SteamManager.IsInitialized)
		{
			return null;
		}
		uint numSubscribedItems = SteamWorkshopManager.GetNumSubscribedItems();
		PublishedFileId_t[] array = new PublishedFileId_t[numSubscribedItems];
		if (ApplicationManager.IsDedicatedGameServer)
		{
			SteamGameServerUGC.GetSubscribedItems(array, numSubscribedItems, false);
		}
		else
		{
			SteamUGC.GetSubscribedItems(array, numSubscribedItems, false);
		}
		return (from id in array
		select id.m_PublishedFileId.ToString()).ToArray<string>();
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x0003697C File Offset: 0x00034B7C
	public static bool GetItemInstallInfo(string itemId, out string path)
	{
		path = null;
		if (!SteamManager.IsInitialized)
		{
			return false;
		}
		PublishedFileId_t nPublishedFileID = new PublishedFileId_t(ulong.Parse(itemId));
		if (ApplicationManager.IsDedicatedGameServer)
		{
			ulong num;
			uint num2;
			return SteamGameServerUGC.GetItemInstallInfo(nPublishedFileID, out num, out path, 4096U, out num2);
		}
		ulong num3;
		uint num4;
		return SteamUGC.GetItemInstallInfo(nPublishedFileID, out num3, out path, 4096U, out num4);
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x000369CC File Offset: 0x00034BCC
	public static bool GetItemDownloadInfo(string itemId, out ulong bytesDownloaded, out ulong bytesTotal)
	{
		bytesDownloaded = 0UL;
		bytesTotal = 0UL;
		if (!SteamManager.IsInitialized)
		{
			return false;
		}
		PublishedFileId_t nPublishedFileID = new PublishedFileId_t(ulong.Parse(itemId));
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return SteamGameServerUGC.GetItemDownloadInfo(nPublishedFileID, out bytesDownloaded, out bytesTotal);
		}
		return SteamUGC.GetItemDownloadInfo(nPublishedFileID, out bytesDownloaded, out bytesTotal);
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x00036A10 File Offset: 0x00034C10
	public static uint GetItemState(string itemId)
	{
		if (!SteamManager.IsInitialized)
		{
			return 0U;
		}
		PublishedFileId_t nPublishedFileID = new PublishedFileId_t(ulong.Parse(itemId));
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return SteamGameServerUGC.GetItemState(nPublishedFileID);
		}
		return SteamUGC.GetItemState(nPublishedFileID);
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x00036A48 File Offset: 0x00034C48
	public static void GetItemDetails(params string[] itemIds)
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		if (itemIds == null || itemIds.Length == 0)
		{
			return;
		}
		SteamWorkshopManager.debouncedGetItemDetailsItemIds.AddRange(from id in itemIds
		where !SteamWorkshopManager.debouncedGetItemDetailsItemIds.Contains(id)
		select id);
		Tween tween = SteamWorkshopManager.getItemDetailsDebounceTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		SteamWorkshopManager.getItemDetailsDebounceTween = DOVirtual.DelayedCall(0f, delegate
		{
			UGCQueryHandle_t ugcqueryHandle_t = SteamWorkshopManager.CreateQueryUGCDetailsRequest(SteamWorkshopManager.debouncedGetItemDetailsItemIds.ToArray());
			SteamWorkshopManager.debouncedGetItemDetailsItemIds.Clear();
			CallResult<SteamUGCQueryCompleted_t> callResult = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(SteamWorkshopManager.OnUGCQueryCompleted));
			SteamWorkshopManager.ugcQueryCompletedCallResultMap[ugcqueryHandle_t] = callResult;
			SteamAPICall_t steamAPICall_t = SteamWorkshopManager.SendQueryUGCRequest(ugcqueryHandle_t);
			if (steamAPICall_t == SteamAPICall_t.Invalid)
			{
				return;
			}
			callResult.Set(steamAPICall_t, null);
		}, true);
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x00036AD4 File Offset: 0x00034CD4
	public static UGCQueryHandle_t CreateQueryUGCDetailsRequest(string[] itemIds)
	{
		if (!SteamManager.IsInitialized)
		{
			return UGCQueryHandle_t.Invalid;
		}
		PublishedFileId_t[] array = new PublishedFileId_t[itemIds.Length];
		for (int i = 0; i < itemIds.Length; i++)
		{
			array[i] = new PublishedFileId_t(ulong.Parse(itemIds[i]));
		}
		UGCQueryHandle_t ugcqueryHandle_t;
		if (ApplicationManager.IsDedicatedGameServer)
		{
			ugcqueryHandle_t = SteamGameServerUGC.CreateQueryUGCDetailsRequest(array, (uint)array.Length);
			SteamGameServerUGC.SetReturnLongDescription(ugcqueryHandle_t, true);
		}
		else
		{
			ugcqueryHandle_t = SteamUGC.CreateQueryUGCDetailsRequest(array, (uint)array.Length);
			SteamUGC.SetReturnLongDescription(ugcqueryHandle_t, true);
		}
		return ugcqueryHandle_t;
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x00036B47 File Offset: 0x00034D47
	public static SteamAPICall_t SendQueryUGCRequest(UGCQueryHandle_t queryHandle)
	{
		if (!SteamManager.IsInitialized)
		{
			return SteamAPICall_t.Invalid;
		}
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return SteamGameServerUGC.SendQueryUGCRequest(queryHandle);
		}
		return SteamUGC.SendQueryUGCRequest(queryHandle);
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x00036B6A File Offset: 0x00034D6A
	private static bool GetQueryUGCResult(UGCQueryHandle_t queryHandle, uint index, out SteamUGCDetails_t details)
	{
		details = default(SteamUGCDetails_t);
		if (!SteamManager.IsInitialized)
		{
			return false;
		}
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return SteamGameServerUGC.GetQueryUGCResult(queryHandle, index, out details);
		}
		return SteamUGC.GetQueryUGCResult(queryHandle, index, out details);
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x00036B94 File Offset: 0x00034D94
	private static bool GetQueryUGCPreviewURL(UGCQueryHandle_t queryHandle, uint index, out string previewUrl)
	{
		previewUrl = null;
		if (!SteamManager.IsInitialized)
		{
			return false;
		}
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return SteamGameServerUGC.GetQueryUGCPreviewURL(queryHandle, index, out previewUrl, 2048U);
		}
		return SteamUGC.GetQueryUGCPreviewURL(queryHandle, index, out previewUrl, 2048U);
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x00036BC4 File Offset: 0x00034DC4
	private static bool GetQueryUGCMetadata(UGCQueryHandle_t queryHandle, uint index, out string metadata)
	{
		metadata = null;
		if (!SteamManager.IsInitialized)
		{
			return false;
		}
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return SteamGameServerUGC.GetQueryUGCMetadata(queryHandle, index, out metadata, 8000U);
		}
		return SteamUGC.GetQueryUGCMetadata(queryHandle, index, out metadata, 8000U);
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x00036BF4 File Offset: 0x00034DF4
	private static bool GetQueryUGCStatistic(UGCQueryHandle_t queryHandle, uint index, EItemStatistic eStatType, out ulong statValue)
	{
		statValue = 0UL;
		if (!SteamManager.IsInitialized)
		{
			return false;
		}
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return SteamGameServerUGC.GetQueryUGCStatistic(queryHandle, index, eStatType, out statValue);
		}
		return SteamUGC.GetQueryUGCStatistic(queryHandle, index, eStatType, out statValue);
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x00036C20 File Offset: 0x00034E20
	public static bool DownloadItem(string itemId)
	{
		if (!SteamManager.IsInitialized)
		{
			return false;
		}
		SteamWorkshopManager.Logger.Info("Downloading item " + itemId);
		PublishedFileId_t nPublishedFileID = new PublishedFileId_t(ulong.Parse(itemId));
		if (ApplicationManager.IsDedicatedGameServer)
		{
			return SteamGameServerUGC.DownloadItem(nPublishedFileID, true);
		}
		return SteamUGC.DownloadItem(nPublishedFileID, true);
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x00036C70 File Offset: 0x00034E70
	public static void SubscribeItem(string itemId)
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		SteamWorkshopManager.Logger.Info("Subscribing item " + itemId);
		PublishedFileId_t nPublishedFileID = new PublishedFileId_t(ulong.Parse(itemId));
		if (ApplicationManager.IsDedicatedGameServer)
		{
			SteamGameServerUGC.SubscribeItem(nPublishedFileID);
			return;
		}
		SteamUGC.SubscribeItem(nPublishedFileID);
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x00036CC0 File Offset: 0x00034EC0
	public static void UnsubscribeItem(string itemId)
	{
		if (!SteamManager.IsInitialized)
		{
			return;
		}
		SteamWorkshopManager.Logger.Info("Unsubscribing item " + itemId);
		PublishedFileId_t nPublishedFileID = new PublishedFileId_t(ulong.Parse(itemId));
		if (ApplicationManager.IsDedicatedGameServer)
		{
			SteamGameServerUGC.UnsubscribeItem(nPublishedFileID);
			return;
		}
		SteamUGC.UnsubscribeItem(nPublishedFileID);
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x00036D10 File Offset: 0x00034F10
	private static void OnItemUpdated(object sender, PropertyChangedEventArgs e)
	{
		SteamWorkshopItem value = (SteamWorkshopItem)sender;
		EventManager.TriggerEvent("Event_OnSteamWorkshopItemUpdated", new Dictionary<string, object>
		{
			{
				"item",
				value
			}
		});
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x00036D40 File Offset: 0x00034F40
	private static void OnDownloadItemResult(DownloadItemResult_t response)
	{
		if (response.m_unAppID != new AppId_t(2994020U))
		{
			return;
		}
		if (response.m_eResult != EResult.k_EResultOK)
		{
			return;
		}
		string value = response.m_nPublishedFileId.ToString();
		EventManager.TriggerEvent("Event_OnSteamWorkshopItemDownloaded", new Dictionary<string, object>
		{
			{
				"itemId",
				value
			}
		});
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x00036D9D File Offset: 0x00034F9D
	private static void OnUserSubscribedItemsListChanged(UserSubscribedItemsListChanged_t response)
	{
		if (response.m_nAppID != new AppId_t(2994020U))
		{
			return;
		}
		EventManager.TriggerEvent("Event_OnSteamWorkshopSubscribedItemsListChanged", null);
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x00036DC4 File Offset: 0x00034FC4
	private static void OnRemoteStorageSubscribePublishedFileResult(RemoteStorageSubscribePublishedFileResult_t response)
	{
		if (response.m_eResult != EResult.k_EResultOK)
		{
			return;
		}
		string value = response.m_nPublishedFileId.ToString();
		EventManager.TriggerEvent("Event_OnSteamWorkshopItemSubscribed", new Dictionary<string, object>
		{
			{
				"itemId",
				value
			}
		});
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x00036E0C File Offset: 0x0003500C
	private static void OnRemoteStorageUnsubscribePublishedFileResult(RemoteStorageUnsubscribePublishedFileResult_t response)
	{
		if (response.m_eResult != EResult.k_EResultOK)
		{
			return;
		}
		string value = response.m_nPublishedFileId.ToString();
		EventManager.TriggerEvent("Event_OnSteamWorkshopItemUnsubscribed", new Dictionary<string, object>
		{
			{
				"itemId",
				value
			}
		});
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x00036E54 File Offset: 0x00035054
	private static void OnUGCQueryCompleted(SteamUGCQueryCompleted_t response, bool bIOFailure)
	{
		if (response.m_eResult != EResult.k_EResultOK)
		{
			return;
		}
		for (uint num = 0U; num < response.m_unNumResultsReturned; num += 1U)
		{
			SteamUGCDetails_t steamUGCDetails_t;
			if (SteamWorkshopManager.GetQueryUGCResult(response.m_handle, num, out steamUGCDetails_t) && steamUGCDetails_t.m_eResult == EResult.k_EResultOK)
			{
				PublishedFileId_t nPublishedFileId = steamUGCDetails_t.m_nPublishedFileId;
				string rgchTitle = steamUGCDetails_t.m_rgchTitle;
				string rgchDescription = steamUGCDetails_t.m_rgchDescription;
				int unVotesUp = (int)steamUGCDetails_t.m_unVotesUp;
				int unVotesDown = (int)steamUGCDetails_t.m_unVotesDown;
				string value;
				SteamWorkshopManager.GetQueryUGCPreviewURL(response.m_handle, num, out value);
				ulong num2;
				SteamWorkshopManager.GetQueryUGCStatistic(response.m_handle, num, EItemStatistic.k_EItemStatistic_NumSubscriptions, out num2);
				string value2;
				SteamWorkshopManager.GetQueryUGCMetadata(response.m_handle, num, out value2);
				EventManager.TriggerEvent("Event_OnSteamWorkshopItemDetails", new Dictionary<string, object>
				{
					{
						"id",
						nPublishedFileId.ToString()
					},
					{
						"title",
						rgchTitle
					},
					{
						"description",
						rgchDescription
					},
					{
						"previewUrl",
						value
					},
					{
						"subscriptions",
						(int)num2
					},
					{
						"upvotes",
						unVotesUp
					},
					{
						"downvotes",
						unVotesDown
					},
					{
						"metadata",
						value2
					}
				});
			}
		}
		if (SteamWorkshopManager.ugcQueryCompletedCallResultMap.ContainsKey(response.m_handle))
		{
			SteamWorkshopManager.ugcQueryCompletedCallResultMap[response.m_handle].Dispose();
			SteamWorkshopManager.ugcQueryCompletedCallResultMap.Remove(response.m_handle);
		}
		if (ApplicationManager.IsDedicatedGameServer)
		{
			SteamGameServerUGC.ReleaseQueryUGCRequest(response.m_handle);
			return;
		}
		SteamUGC.ReleaseQueryUGCRequest(response.m_handle);
	}

	// Token: 0x040006D0 RID: 1744
	private static readonly Logger Logger = new Logger("SteamWorkshopManager");

	// Token: 0x040006D1 RID: 1745
	public static List<SteamWorkshopItem> Items = new List<SteamWorkshopItem>();

	// Token: 0x040006D2 RID: 1746
	private static Callback<DownloadItemResult_t> DownloadItemResult;

	// Token: 0x040006D3 RID: 1747
	private static Callback<UserSubscribedItemsListChanged_t> UserSubscribedItemsListChanged;

	// Token: 0x040006D4 RID: 1748
	private static Callback<RemoteStorageSubscribePublishedFileResult_t> RemoteStorageSubscribePublishedFileResult;

	// Token: 0x040006D5 RID: 1749
	private static Callback<RemoteStorageUnsubscribePublishedFileResult_t> RemoteStorageUnsubscribePublishedFileResult;

	// Token: 0x040006D6 RID: 1750
	private static Dictionary<UGCQueryHandle_t, CallResult<SteamUGCQueryCompleted_t>> ugcQueryCompletedCallResultMap = new Dictionary<UGCQueryHandle_t, CallResult<SteamUGCQueryCompleted_t>>();

	// Token: 0x040006D7 RID: 1751
	private static List<string> debouncedGetItemDetailsItemIds = new List<string>();

	// Token: 0x040006D8 RID: 1752
	private static Tween getItemDetailsDebounceTween;
}
