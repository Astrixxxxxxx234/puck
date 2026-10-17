using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x02000209 RID: 521
public class UIServerBrowser : UIView
{
	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06001014 RID: 4116 RVA: 0x0004C1C6 File Offset: 0x0004A3C6
	public int ServerCount
	{
		get
		{
			return this.endPointVisualElementMap.Count;
		}
	}

	// Token: 0x06001015 RID: 4117 RVA: 0x0004C1D4 File Offset: 0x0004A3D4
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("ServerBrowserView", null);
		this.serverBrowser = base.View.Query("ServerBrowser", null);
		this.serverList = this.serverBrowser.Query("ServerList", null);
		this.filters = base.View.Query("Filters", null);
		this.closeIconButton = this.serverBrowser.Query("CloseIconButtonContainer", null).First().Query(null, null);
		this.closeIconButton.clicked += this.OnServerBrowserClickClose;
		this.nameButton = this.serverBrowser.Query("NameButton", null);
		this.nameButton.clicked += this.OnClickNameSort;
		this.nameSortArrow = this.nameButton.Query("NameSortArrow", null);
		this.playersButton = this.serverBrowser.Query("PlayersButton", null);
		this.playersButton.clicked += this.OnClickPlayersSort;
		this.playersSortArrow = this.playersButton.Query("PlayersSortArrow", null);
		this.pingButton = this.serverBrowser.Query("PingButton", null);
		this.pingButton.clicked += this.OnClickPingSort;
		this.pingSortArrow = this.pingButton.Query("PingSortArrow", null);
		this.refreshButton = this.filters.Query("RefreshButton", null);
		this.refreshButton.clicked += this.OnClickRefresh;
		this.hostGameButton = base.View.Query("HostGameButton", null);
		this.hostGameButton.clicked += this.OnClickHostGame;
		this.directConnectButton = base.View.Query("DirectConnectButton", null);
		this.directConnectButton.clicked += this.OnClickDirectConnect;
		this.searchTextField = this.filters.Query("SearchTextField", null).First().Query(null, null);
		this.searchTextField.value = string.Empty;
		this.searchTextField.RegisterCallback<ChangeEvent<string>>(new EventCallback<ChangeEvent<string>>(this.OnChangeSearchTextField), TrickleDown.NoTrickleDown);
		this.resultCountLabel = this.filters.Query("ResultCountLabel", null);
		this.maxPingSlider = this.filters.Query("MaxPingSlider", null);
		this.maxPingSlider.SetMappedValueWithoutNotify(100);
		CurvedSlider curvedSlider = this.maxPingSlider;
		curvedSlider.MappedValueChanged = (Action<int>)Delegate.Combine(curvedSlider.MappedValueChanged, new Action<int>(this.OnChangeMaxPing));
		this.showFullToggle = this.filters.Query("ShowFullToggle", null).First().Query(null, null);
		this.showFullToggle.value = true;
		this.showFullToggle.RegisterCallback<ChangeEvent<bool>>(new EventCallback<ChangeEvent<bool>>(this.OnChangeShowFullToggle), TrickleDown.NoTrickleDown);
		this.showEmptyToggle = this.filters.Query("ShowEmptyToggle", null).First().Query(null, null);
		this.showEmptyToggle.value = true;
		this.showEmptyToggle.RegisterCallback<ChangeEvent<bool>>(new EventCallback<ChangeEvent<bool>>(this.OnChangeShowEmptyToggle), TrickleDown.NoTrickleDown);
		this.showPasswordProtectedToggle = this.filters.Query("ShowPasswordProtectedToggle", null).First().Query(null, null);
		this.showPasswordProtectedToggle.value = true;
		this.showPasswordProtectedToggle.RegisterCallback<ChangeEvent<bool>>(new EventCallback<ChangeEvent<bool>>(this.OnChangeShowPasswordProtectedToggle), TrickleDown.NoTrickleDown);
		this.showModdedToggle = this.filters.Query("ShowModdedToggle", null).First().Query(null, null);
		this.showModdedToggle.value = true;
		this.showModdedToggle.RegisterCallback<ChangeEvent<bool>>(new EventCallback<ChangeEvent<bool>>(this.OnChangeShowModdedToggle), TrickleDown.NoTrickleDown);
		this.showUnreachableToggle = this.filters.Query("ShowUnreachableToggle", null).First().Query(null, null);
		this.showUnreachableToggle.value = false;
		this.showUnreachableToggle.RegisterCallback<ChangeEvent<bool>>(new EventCallback<ChangeEvent<bool>>(this.OnChangeShowUnreachableToggle), TrickleDown.NoTrickleDown);
		this.serverList.Clear();
		this.StyleSortButtons();
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x0004C66F File Offset: 0x0004A86F
	public override bool Show()
	{
		bool flag = base.Show();
		if (flag)
		{
			EventManager.TriggerEvent("Event_OnServerBrowserShow", null);
		}
		return flag;
	}

	// Token: 0x06001017 RID: 4119 RVA: 0x0004C685 File Offset: 0x0004A885
	public void Refresh()
	{
		WebSocketManager.Emit("playerGetServerBrowserEndPointsRequest", null, "playerGetServerBrowserEndPointsResponse");
	}

	// Token: 0x06001018 RID: 4120 RVA: 0x0004C698 File Offset: 0x0004A898
	public void UpdateEndPoints(EndPoint[] endPoints)
	{
		UIServerBrowser.<>c__DisplayClass40_0 CS$<>8__locals1 = new UIServerBrowser.<>c__DisplayClass40_0();
		CS$<>8__locals1.<>4__this = this;
		this.RemoveAllServers();
		this.refreshButton.SetEnabled(false);
		foreach (EndPoint endPoint in endPoints)
		{
			EndPoint endPoint;
			this.AddServer(endPoint);
		}
		this.FilterServers();
		this.SortServers();
		UIServerBrowser.<>c__DisplayClass40_0 CS$<>8__locals2 = CS$<>8__locals1;
		int i = this.refreshGeneration + 1;
		this.refreshGeneration = i;
		CS$<>8__locals2.generation = i;
		if (endPoints.Length == 0)
		{
			this.refreshButton.SetEnabled(true);
			return;
		}
		CS$<>8__locals1.queue = new ConcurrentQueue<EndPoint>(endPoints);
		int num = Math.Max(1, Math.Min(8, endPoints.Length));
		CancellationTokenSource cancellationTokenSource = this.pingWaveCts;
		if (cancellationTokenSource != null)
		{
			cancellationTokenSource.Cancel();
		}
		CS$<>8__locals1.cts = (this.pingWaveCts = new CancellationTokenSource());
		CS$<>8__locals1.token = CS$<>8__locals1.cts.Token;
		UIServerBrowser.EnsurePingThreadPoolFloor(num);
		CS$<>8__locals1.workers = new Task[num];
		for (int j = 0; j < num; j++)
		{
			Task[] workers = CS$<>8__locals1.workers;
			int num2 = j;
			TaskFactory factory = Task.Factory;
			Action action;
			if ((action = CS$<>8__locals1.<>9__1) == null)
			{
				EndPoint endPoint;
				action = (CS$<>8__locals1.<>9__1 = delegate()
				{
					for (;;)
					{
						EndPoint endPoint;
						if (CS$<>8__locals1.token.IsCancellationRequested || !CS$<>8__locals1.queue.TryDequeue(out endPoint))
						{
							break;
						}
						ServerPreviewData previewData = CS$<>8__locals1.<>4__this.PingServer(endPoint, 1000, 1000);
						MonoBehaviourSingleton<ThreadManager>.Instance.Enqueue(delegate()
						{
							if (CS$<>8__locals1.generation != CS$<>8__locals1.<>4__this.refreshGeneration)
							{
								return;
							}
							CS$<>8__locals1.<>4__this.SetServerPreviewData(endPoint, previewData);
							CS$<>8__locals1.<>4__this.StyleServer(endPoint);
							CS$<>8__locals1.<>4__this.FilterServer(endPoint);
							CS$<>8__locals1.<>4__this.ScheduleSortFlush();
						});
					}
				});
			}
			workers[num2] = factory.StartNew(action, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}
		Task.Run(delegate()
		{
			UIServerBrowser.<>c__DisplayClass40_0.<<UpdateEndPoints>b__0>d <<UpdateEndPoints>b__0>d;
			<<UpdateEndPoints>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
			<<UpdateEndPoints>b__0>d.<>4__this = CS$<>8__locals1;
			<<UpdateEndPoints>b__0>d.<>1__state = -1;
			<<UpdateEndPoints>b__0>d.<>t__builder.Start<UIServerBrowser.<>c__DisplayClass40_0.<<UpdateEndPoints>b__0>d>(ref <<UpdateEndPoints>b__0>d);
			return <<UpdateEndPoints>b__0>d.<>t__builder.Task;
		});
	}

	// Token: 0x06001019 RID: 4121 RVA: 0x0004C7DC File Offset: 0x0004A9DC
	private void ScheduleSortFlush()
	{
		if (this.pendingSortFlush != null)
		{
			return;
		}
		this.pendingSortFlush = this.serverList.schedule.Execute(new Action(this.FlushSort)).StartingIn(200L);
	}

	// Token: 0x0600101A RID: 4122 RVA: 0x0004C814 File Offset: 0x0004AA14
	private void FlushSort()
	{
		IVisualElementScheduledItem visualElementScheduledItem = this.pendingSortFlush;
		if (visualElementScheduledItem != null)
		{
			visualElementScheduledItem.Pause();
		}
		this.pendingSortFlush = null;
		this.FilterServers();
		this.SortServers();
	}

	// Token: 0x0600101B RID: 4123 RVA: 0x0004C83C File Offset: 0x0004AA3C
	private static void EnsurePingThreadPoolFloor(int workerCount)
	{
		int num = workerCount * 3;
		int num2;
		int num3;
		ThreadPool.GetMinThreads(out num2, out num3);
		if (num2 >= num && num3 >= num)
		{
			return;
		}
		ThreadPool.SetMinThreads(Math.Max(num2, num), Math.Max(num3, num));
	}

	// Token: 0x0600101C RID: 4124 RVA: 0x0004C874 File Offset: 0x0004AA74
	private void AddServer(EndPoint endPoint)
	{
		if (this.endPointVisualElementMap.ContainsKey(endPoint))
		{
			return;
		}
		VisualElement visualElement = this.serverAsset.Instantiate();
		visualElement.userData = new Dictionary<string, object>();
		VisualElement visualElement2 = visualElement.Query("Server", null);
		visualElement2.userData = new Dictionary<string, object>();
		visualElement2.Query(null, null).RegisterCallback<ClickEvent, EndPoint>(new EventCallback<ClickEvent, EndPoint>(this.OnClickServer), endPoint, TrickleDown.NoTrickleDown);
		this.endPointVisualElementMap.Add(endPoint, visualElement);
		this.serverList.Add(visualElement);
		this.StyleServer(endPoint);
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x0004C904 File Offset: 0x0004AB04
	private EndPoint GetServerEndPoint(VisualElement visualElement)
	{
		if (!this.endPointVisualElementMap.ContainsValue(visualElement))
		{
			return null;
		}
		return this.endPointVisualElementMap.FirstOrDefault((KeyValuePair<EndPoint, VisualElement> x) => x.Value == visualElement).Key;
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x0004C954 File Offset: 0x0004AB54
	private void SetServerPreviewData(EndPoint endPoint, ServerPreviewData previewData)
	{
		if (!this.endPointVisualElementMap.ContainsKey(endPoint))
		{
			return;
		}
		(this.endPointVisualElementMap[endPoint].Query("Server", null).userData as Dictionary<string, object>)["previewData"] = previewData;
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x0004C9A4 File Offset: 0x0004ABA4
	private ServerPreviewData GetServerPreviewData(EndPoint endPoint)
	{
		if (!this.endPointVisualElementMap.ContainsKey(endPoint))
		{
			return null;
		}
		return (this.endPointVisualElementMap[endPoint].Query("Server", null).userData as Dictionary<string, object>).GetValueOrDefault("previewData", null) as ServerPreviewData;
	}

	// Token: 0x06001020 RID: 4128 RVA: 0x0004C9F8 File Offset: 0x0004ABF8
	private void StyleServer(EndPoint endPoint)
	{
		if (!this.endPointVisualElementMap.ContainsKey(endPoint))
		{
			return;
		}
		VisualElement visualElement = this.endPointVisualElementMap[endPoint].Query("Server", null);
		ServerPreviewData serverPreviewData = this.GetServerPreviewData(endPoint);
		Label label = visualElement.Query("NameLabel", null);
		Label label2 = visualElement.Query("PlayersLabel", null);
		Label label3 = visualElement.Query("PingLabel", null);
		if (serverPreviewData == null)
		{
			visualElement.EnableInClassList("passwordProtected", false);
			visualElement.EnableInClassList("modded", false);
			visualElement.EnableInClassList("unreachable", true);
			label.text = endPoint.ToString();
			label2.text = "?";
			label3.text = "?";
			return;
		}
		visualElement.EnableInClassList("passwordProtected", serverPreviewData.isPasswordProtected);
		visualElement.EnableInClassList("modded", serverPreviewData.clientRequiredModIds.Length != 0);
		visualElement.EnableInClassList("unreachable", false);
		label.text = serverPreviewData.name;
		label2.text = string.Format("{0}/{1}", serverPreviewData.players, serverPreviewData.maxPlayers);
		label3.text = string.Format("{0}ms", serverPreviewData.ping);
	}

	// Token: 0x06001021 RID: 4129 RVA: 0x0004CB3C File Offset: 0x0004AD3C
	private void RemoveServer(EndPoint endPoint)
	{
		if (!this.endPointVisualElementMap.ContainsKey(endPoint))
		{
			return;
		}
		this.endPointVisualElementMap[endPoint].Query("Server", null).Query(null, null).UnregisterCallback<ClickEvent, EndPoint>(new EventCallback<ClickEvent, EndPoint>(this.OnClickServer), TrickleDown.NoTrickleDown);
		this.serverList.Remove(this.endPointVisualElementMap[endPoint]);
		this.endPointVisualElementMap.Remove(endPoint);
	}

	// Token: 0x06001022 RID: 4130 RVA: 0x0004CBB8 File Offset: 0x0004ADB8
	private void RemoveAllServers()
	{
		foreach (EndPoint endPoint in this.endPointVisualElementMap.Keys.ToList<EndPoint>())
		{
			this.RemoveServer(endPoint);
		}
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x0004CC18 File Offset: 0x0004AE18
	private void StyleSortButtons()
	{
		this.StyleSortArrow(this.nameSortArrow, ServerSortType.Name);
		this.StyleSortArrow(this.playersSortArrow, ServerSortType.Players);
		this.StyleSortArrow(this.pingSortArrow, ServerSortType.Ping);
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x0004CC44 File Offset: 0x0004AE44
	private void StyleSortArrow(VisualElement arrow, ServerSortType columnSortType)
	{
		bool flag = this.sortType == columnSortType;
		arrow.EnableInClassList("active", flag);
		arrow.EnableInClassList("descending", flag && this.sortDirection == ServerSortDirection.Descending);
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x0004CC84 File Offset: 0x0004AE84
	private void SortServers()
	{
		this.serverList.hierarchy.Sort(delegate(VisualElement a, VisualElement b)
		{
			EndPoint serverEndPoint = this.GetServerEndPoint(a);
			EndPoint serverEndPoint2 = this.GetServerEndPoint(b);
			ServerPreviewData serverPreviewData = this.GetServerPreviewData(serverEndPoint);
			ServerPreviewData serverPreviewData2 = this.GetServerPreviewData(serverEndPoint2);
			string text = (serverPreviewData != null) ? serverPreviewData.name : serverEndPoint.ToString();
			string strB = (serverPreviewData2 != null) ? serverPreviewData2.name : serverEndPoint2.ToString();
			int num = (serverPreviewData != null) ? serverPreviewData.players : 0;
			int value = (serverPreviewData2 != null) ? serverPreviewData2.players : 0;
			int num2 = (serverPreviewData != null) ? serverPreviewData.ping : int.MaxValue;
			int value2 = (serverPreviewData2 != null) ? serverPreviewData2.ping : int.MaxValue;
			int num3 = 0;
			switch (this.sortType)
			{
			case ServerSortType.Name:
				num3 = text.CompareTo(strB) * ((this.sortDirection == ServerSortDirection.Ascending) ? 1 : -1);
				break;
			case ServerSortType.Players:
				num3 = num.CompareTo(value) * ((this.sortDirection == ServerSortDirection.Ascending) ? 1 : -1);
				if (num3 == 0)
				{
					num3 = text.CompareTo(strB);
				}
				break;
			case ServerSortType.Ping:
				num3 = num2.CompareTo(value2) * ((this.sortDirection == ServerSortDirection.Ascending) ? 1 : -1);
				if (num3 == 0)
				{
					num3 = text.CompareTo(strB);
				}
				break;
			}
			return num3;
		});
	}

	// Token: 0x06001026 RID: 4134 RVA: 0x0004CCB0 File Offset: 0x0004AEB0
	private void FilterServers()
	{
		foreach (EndPoint endPoint in this.endPointVisualElementMap.Keys)
		{
			this.FilterServer(endPoint);
		}
		this.UpdateResultCount();
	}

	// Token: 0x06001027 RID: 4135 RVA: 0x0004CD10 File Offset: 0x0004AF10
	private void FilterServer(EndPoint endPoint)
	{
		if (!this.endPointVisualElementMap.ContainsKey(endPoint))
		{
			return;
		}
		VisualElement visualElement = this.endPointVisualElementMap[endPoint];
		ServerPreviewData serverPreviewData = this.GetServerPreviewData(endPoint);
		bool flag;
		if (serverPreviewData == null)
		{
			string text = endPoint.ipAddress.ToLower();
			string value = string.IsNullOrEmpty(this.searchTextField.value) ? null : this.searchTextField.value.ToLower();
			flag = ((string.IsNullOrEmpty(value) || text.Contains(value)) && this.showUnreachableToggle.value);
		}
		else
		{
			string text2 = serverPreviewData.name.ToLower();
			string value2 = string.IsNullOrEmpty(this.searchTextField.value) ? null : this.searchTextField.value.ToLower();
			flag = ((string.IsNullOrEmpty(value2) || text2.Contains(value2)) && serverPreviewData.ping <= this.maxPingValue && (serverPreviewData.players > 0 || this.showEmptyToggle.value) && (serverPreviewData.players < serverPreviewData.maxPlayers || this.showFullToggle.value) && (!serverPreviewData.isPasswordProtected || this.showPasswordProtectedToggle.value) && (serverPreviewData.clientRequiredModIds.Length == 0 || this.showModdedToggle.value));
		}
		visualElement.style.display = (flag ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06001028 RID: 4136 RVA: 0x0004CE68 File Offset: 0x0004B068
	private void UpdateResultCount()
	{
		int num = this.endPointVisualElementMap.Values.Count((VisualElement serverAssetInstance) => serverAssetInstance.style.display.value == DisplayStyle.Flex);
		int count = this.endPointVisualElementMap.Count;
		this.resultCountLabel.text = string.Format("SHOWING {0} / {1} {2}", num, count, (count == 1) ? "SERVER" : "SERVERS");
	}

	// Token: 0x06001029 RID: 4137 RVA: 0x0004CEE4 File Offset: 0x0004B0E4
	private ServerPreviewData PingServer(EndPoint endPoint, int connectTimeout, int responseTimeout)
	{
		TCPClient tcpClient = new TCPClient(endPoint, connectTimeout, 1000);
		double pingTimestamp = 0.0;
		ServerPreviewData previewData = null;
		ManualResetEventSlim responseEvent = new ManualResetEventSlim(false);
		tcpClient.OnConnected += delegate()
		{
			string message = JsonSerializer.Serialize<TCPServerPreviewRequest>(new TCPServerPreviewRequest(), null);
			tcpClient.SendMessage(message);
		};
		tcpClient.OnMessageSent += delegate(string message)
		{
			try
			{
				if (JsonSerializer.Deserialize<TCPServerMessage>(message, null).type == TCPServerMessageType.PreviewRequest)
				{
					pingTimestamp = Utils.GetTimestamp();
				}
			}
			catch (Exception ex)
			{
				UIServerBrowser.Logger.Error(string.Format("Error parsing message sent to {0}: {1}", endPoint, ex.Message));
			}
		};
		tcpClient.OnMessageReceived += delegate(string message)
		{
			try
			{
				if (JsonSerializer.Deserialize<TCPServerMessage>(message, null).type == TCPServerMessageType.PreviewResponse)
				{
					TCPServerPreviewResponse tcpserverPreviewResponse = JsonSerializer.Deserialize<TCPServerPreviewResponse>(message, null);
					int ping = (int)(Utils.GetTimestamp() - pingTimestamp);
					previewData = new ServerPreviewData
					{
						name = tcpserverPreviewResponse.name,
						players = tcpserverPreviewResponse.players,
						maxPlayers = tcpserverPreviewResponse.maxPlayers,
						isPasswordProtected = tcpserverPreviewResponse.isPasswordProtected,
						clientRequiredModIds = tcpserverPreviewResponse.clientRequiredModIds,
						ping = ping
					};
					responseEvent.Set();
				}
			}
			catch (Exception ex)
			{
				UIServerBrowser.Logger.Error(string.Format("Error parsing message from {0}: {1}", endPoint, ex.Message));
			}
		};
		tcpClient.Connect();
		if (tcpClient.IsConnected)
		{
			responseEvent.Wait(responseTimeout);
			tcpClient.Disconnect();
		}
		return previewData;
	}

	// Token: 0x0600102A RID: 4138 RVA: 0x0004CFB2 File Offset: 0x0004B1B2
	private void OnClickNameSort()
	{
		if (this.sortType == ServerSortType.Name)
		{
			this.sortDirection = ((this.sortDirection == ServerSortDirection.Ascending) ? ServerSortDirection.Descending : ServerSortDirection.Ascending);
		}
		else
		{
			this.sortType = ServerSortType.Name;
			this.sortDirection = ServerSortDirection.Ascending;
		}
		this.StyleSortButtons();
		this.SortServers();
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x0004CFEA File Offset: 0x0004B1EA
	private void OnClickPlayersSort()
	{
		if (this.sortType == ServerSortType.Players)
		{
			this.sortDirection = ((this.sortDirection == ServerSortDirection.Ascending) ? ServerSortDirection.Descending : ServerSortDirection.Ascending);
		}
		else
		{
			this.sortType = ServerSortType.Players;
			this.sortDirection = ServerSortDirection.Descending;
		}
		this.StyleSortButtons();
		this.SortServers();
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x0004D023 File Offset: 0x0004B223
	private void OnClickPingSort()
	{
		if (this.sortType == ServerSortType.Ping)
		{
			this.sortDirection = ((this.sortDirection == ServerSortDirection.Ascending) ? ServerSortDirection.Descending : ServerSortDirection.Ascending);
		}
		else
		{
			this.sortType = ServerSortType.Ping;
			this.sortDirection = ServerSortDirection.Ascending;
		}
		this.StyleSortButtons();
		this.SortServers();
	}

	// Token: 0x0600102D RID: 4141 RVA: 0x0004D05C File Offset: 0x0004B25C
	private void OnServerBrowserClickClose()
	{
		EventManager.TriggerEvent("Event_OnServerBrowserClickClose", null);
	}

	// Token: 0x0600102E RID: 4142 RVA: 0x0004D069 File Offset: 0x0004B269
	private void OnClickRefresh()
	{
		EventManager.TriggerEvent("Event_OnServerBrowserClickRefresh", null);
	}

	// Token: 0x0600102F RID: 4143 RVA: 0x0004D076 File Offset: 0x0004B276
	private void OnClickHostGame()
	{
		EventManager.TriggerEvent("Event_OnServerBrowserClickNewServer", null);
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x0004D083 File Offset: 0x0004B283
	private void OnClickDirectConnect()
	{
		EventManager.TriggerEvent("Event_OnServerBrowserClickDirectConnect", null);
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x0004D090 File Offset: 0x0004B290
	private void OnClickServer(ClickEvent e, EndPoint endPoint)
	{
		EventManager.TriggerEvent("Event_OnServerBrowserClickEndPoint", new Dictionary<string, object>
		{
			{
				"endPoint",
				endPoint
			}
		});
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x0004D0AD File Offset: 0x0004B2AD
	private void OnChangeSearchTextField(ChangeEvent<string> e)
	{
		this.FilterServers();
	}

	// Token: 0x06001033 RID: 4147 RVA: 0x0004D0B5 File Offset: 0x0004B2B5
	private void OnChangeMaxPing(int maxPing)
	{
		this.maxPingValue = maxPing;
		this.FilterServers();
	}

	// Token: 0x06001034 RID: 4148 RVA: 0x0004D0AD File Offset: 0x0004B2AD
	private void OnChangeShowFullToggle(ChangeEvent<bool> e)
	{
		this.FilterServers();
	}

	// Token: 0x06001035 RID: 4149 RVA: 0x0004D0AD File Offset: 0x0004B2AD
	private void OnChangeShowEmptyToggle(ChangeEvent<bool> e)
	{
		this.FilterServers();
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x0004D0AD File Offset: 0x0004B2AD
	private void OnChangeShowPasswordProtectedToggle(ChangeEvent<bool> e)
	{
		this.FilterServers();
	}

	// Token: 0x06001037 RID: 4151 RVA: 0x0004D0AD File Offset: 0x0004B2AD
	private void OnChangeShowModdedToggle(ChangeEvent<bool> e)
	{
		this.FilterServers();
	}

	// Token: 0x06001038 RID: 4152 RVA: 0x0004D0AD File Offset: 0x0004B2AD
	private void OnChangeShowUnreachableToggle(ChangeEvent<bool> e)
	{
		this.FilterServers();
	}

	// Token: 0x04000A2D RID: 2605
	private static readonly global::Logger Logger = new global::Logger("UIServerBrowser");

	// Token: 0x04000A2E RID: 2606
	private const int PingConcurrency = 8;

	// Token: 0x04000A2F RID: 2607
	private const int PingConnectTimeoutMs = 1000;

	// Token: 0x04000A30 RID: 2608
	private const int PingResponseTimeoutMs = 1000;

	// Token: 0x04000A31 RID: 2609
	private const int SortDebounceMs = 200;

	// Token: 0x04000A32 RID: 2610
	private const int DefaultMaxPing = 100;

	// Token: 0x04000A33 RID: 2611
	[Header("References")]
	[SerializeField]
	private VisualTreeAsset serverAsset;

	// Token: 0x04000A34 RID: 2612
	private VisualElement serverBrowser;

	// Token: 0x04000A35 RID: 2613
	private VisualElement filters;

	// Token: 0x04000A36 RID: 2614
	private IconButton closeIconButton;

	// Token: 0x04000A37 RID: 2615
	private VisualElement serverList;

	// Token: 0x04000A38 RID: 2616
	private Button nameButton;

	// Token: 0x04000A39 RID: 2617
	private Button playersButton;

	// Token: 0x04000A3A RID: 2618
	private Button pingButton;

	// Token: 0x04000A3B RID: 2619
	private VisualElement nameSortArrow;

	// Token: 0x04000A3C RID: 2620
	private VisualElement playersSortArrow;

	// Token: 0x04000A3D RID: 2621
	private VisualElement pingSortArrow;

	// Token: 0x04000A3E RID: 2622
	private Button refreshButton;

	// Token: 0x04000A3F RID: 2623
	private Button hostGameButton;

	// Token: 0x04000A40 RID: 2624
	private Button directConnectButton;

	// Token: 0x04000A41 RID: 2625
	private TextField searchTextField;

	// Token: 0x04000A42 RID: 2626
	private CurvedSlider maxPingSlider;

	// Token: 0x04000A43 RID: 2627
	private int maxPingValue = 100;

	// Token: 0x04000A44 RID: 2628
	private Label resultCountLabel;

	// Token: 0x04000A45 RID: 2629
	private Toggle showFullToggle;

	// Token: 0x04000A46 RID: 2630
	private Toggle showEmptyToggle;

	// Token: 0x04000A47 RID: 2631
	private Toggle showPasswordProtectedToggle;

	// Token: 0x04000A48 RID: 2632
	private Toggle showModdedToggle;

	// Token: 0x04000A49 RID: 2633
	private Toggle showUnreachableToggle;

	// Token: 0x04000A4A RID: 2634
	private Dictionary<EndPoint, VisualElement> endPointVisualElementMap = new Dictionary<EndPoint, VisualElement>();

	// Token: 0x04000A4B RID: 2635
	private ServerSortType sortType = ServerSortType.Players;

	// Token: 0x04000A4C RID: 2636
	private ServerSortDirection sortDirection = ServerSortDirection.Descending;

	// Token: 0x04000A4D RID: 2637
	private int refreshGeneration;

	// Token: 0x04000A4E RID: 2638
	private IVisualElementScheduledItem pendingSortFlush;

	// Token: 0x04000A4F RID: 2639
	private CancellationTokenSource pingWaveCts;
}
