using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x020001C5 RID: 453
public class UIChat : UIView
{
	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000D95 RID: 3477 RVA: 0x00040DD6 File Offset: 0x0003EFD6
	// (set) Token: 0x06000D96 RID: 3478 RVA: 0x00040DDE File Offset: 0x0003EFDE
	public bool IsTeamChat { get; private set; }

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000D97 RID: 3479 RVA: 0x00040DE7 File Offset: 0x0003EFE7
	// (set) Token: 0x06000D98 RID: 3480 RVA: 0x00040DEF File Offset: 0x0003EFEF
	public bool IsQuickChatVisible { get; private set; }

	// Token: 0x06000D99 RID: 3481 RVA: 0x00040DF8 File Offset: 0x0003EFF8
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("ChatView", null);
		this.chat = base.View.Query("Chat", null);
		this.quickChat = base.View.Query("QuickChat", null);
		this.scrollView = this.chat.Query(null, null);
		this.messages = this.scrollView.Query("Messages", null);
		this.inputPrefix = this.chat.Query("InputPrefix", null);
		this.textField = this.chat.Query(null, null);
		this.quickChatCategoryLabel = this.quickChat.Query(null, null);
		this.quickChatMessages = this.quickChat.Query("Messages", null);
		this.textField.RegisterCallback<NavigationSubmitEvent>(delegate(NavigationSubmitEvent e)
		{
			this.SubmitMessage();
		}, TrickleDown.TrickleDown);
		this.textField.RegisterCallback<NavigationCancelEvent>(delegate(NavigationCancelEvent e)
		{
			this.StopInput();
		}, TrickleDown.TrickleDown);
		this.chat.RegisterCallback<FocusOutEvent>(delegate(FocusOutEvent e)
		{
			if (UIUtils.GetVisualElementChildren(this.chat, true).Contains(e.relatedTarget))
			{
				return;
			}
			this.StopInput();
		}, TrickleDown.TrickleDown);
		this.scrollView.verticalScroller.valueChanged += delegate(float value)
		{
			if (this.isScrolling)
			{
				return;
			}
			this.autoScroll = (this.scrollView.verticalScroller.highValue - value <= 1f);
		};
		UIUtils.GetVisualElementChildren(this.chat, true).ForEach(delegate(VisualElement visualElement)
		{
			visualElement.focusable = true;
		});
		this.ClearChatMessages();
		this.StopInput();
		this.HideQuickChat();
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x0003E32E File Offset: 0x0003C52E
	public override bool Show()
	{
		return SettingsManager.ShowGameUserInterface && base.Show();
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x00040F98 File Offset: 0x0003F198
	public void StartInput(bool isTeamChat = false, string openCharacter = null)
	{
		base.IsFocused = true;
		this.IsTeamChat = isTeamChat;
		this.ShowTextField();
		this.SwallowOpenCharacter(openCharacter);
		this.uiChatMessages.ForEach(delegate(UIChatMessage uiChatMessage)
		{
			uiChatMessage.Focus();
		});
	}

	// Token: 0x06000D9C RID: 3484 RVA: 0x00040FEA File Offset: 0x0003F1EA
	public void StopInput()
	{
		base.IsFocused = false;
		this.IsTeamChat = false;
		this.HideTextField();
		this.uiChatMessages.ForEach(delegate(UIChatMessage uiChatMessage)
		{
			uiChatMessage.Blur();
		});
	}

	// Token: 0x06000D9D RID: 3485 RVA: 0x0004102C File Offset: 0x0003F22C
	private void ShowTextField()
	{
		this.scrollView.verticalScrollerVisibility = ScrollerVisibility.AlwaysVisible;
		this.inputPrefix.style.display = (this.IsTeamChat ? DisplayStyle.Flex : DisplayStyle.None);
		this.textField.style.opacity = 1f;
		this.textField.pickingMode = PickingMode.Position;
		this.textField.value = string.Empty;
		this.textField.Focus();
	}

	// Token: 0x06000D9E RID: 3486 RVA: 0x000410A8 File Offset: 0x0003F2A8
	private void HideTextField()
	{
		this.scrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;
		this.inputPrefix.style.display = DisplayStyle.None;
		this.textField.style.opacity = 0f;
		this.textField.pickingMode = PickingMode.Ignore;
		this.textField.value = string.Empty;
		this.textField.Blur();
		this.textField.UnregisterCallback<ChangeEvent<string>>(new EventCallback<ChangeEvent<string>>(this.OnTextFieldFirstChange), TrickleDown.NoTrickleDown);
		this.pendingOpenCharacter = null;
	}

	// Token: 0x06000D9F RID: 3487 RVA: 0x00041137 File Offset: 0x0003F337
	private void SwallowOpenCharacter(string openCharacter)
	{
		if (string.IsNullOrEmpty(openCharacter))
		{
			return;
		}
		this.pendingOpenCharacter = openCharacter;
		this.textField.RegisterCallback<ChangeEvent<string>>(new EventCallback<ChangeEvent<string>>(this.OnTextFieldFirstChange), TrickleDown.NoTrickleDown);
	}

	// Token: 0x06000DA0 RID: 3488 RVA: 0x00041164 File Offset: 0x0003F364
	private void OnTextFieldFirstChange(ChangeEvent<string> e)
	{
		this.textField.UnregisterCallback<ChangeEvent<string>>(new EventCallback<ChangeEvent<string>>(this.OnTextFieldFirstChange), TrickleDown.NoTrickleDown);
		if (string.Equals(e.newValue, this.pendingOpenCharacter, StringComparison.OrdinalIgnoreCase))
		{
			this.textField.SetValueWithoutNotify(string.Empty);
		}
		this.pendingOpenCharacter = null;
	}

	// Token: 0x06000DA1 RID: 3489 RVA: 0x000411B4 File Offset: 0x0003F3B4
	public void ShowQuickChat(QuickChatCategory category, QuickChat[] quickChats)
	{
		this.quickChatCategoryLabel.text = category.ToString().ToUpper();
		for (int i = 0; i < quickChats.Length; i++)
		{
			VisualElement visualElement = this.quickChatMessageAsset.Instantiate();
			TextElement textElement = visualElement.Query(null, null);
			string content = quickChats[i].Content;
			textElement.text = string.Format("{0}. {1}", i + 1, content);
			this.quickChatMessages.Add(visualElement);
		}
		this.quickChat.style.display = DisplayStyle.Flex;
		this.IsQuickChatVisible = true;
	}

	// Token: 0x06000DA2 RID: 3490 RVA: 0x00041253 File Offset: 0x0003F453
	public void HideQuickChat()
	{
		this.quickChat.style.display = DisplayStyle.None;
		this.quickChatMessages.Clear();
		this.IsQuickChatVisible = false;
	}

	// Token: 0x06000DA3 RID: 3491 RVA: 0x00041280 File Offset: 0x0003F480
	private void PinLabelHeight(Label label)
	{
		float width = label.contentRect.width;
		if (width <= 1f)
		{
			return;
		}
		float num = label.MeasureTextSize(label.text, width, VisualElement.MeasureMode.Exactly, 0f, VisualElement.MeasureMode.Undefined).y + label.resolvedStyle.paddingTop + label.resolvedStyle.paddingBottom;
		if (num <= 33f)
		{
			return;
		}
		if (!Mathf.Approximately(label.resolvedStyle.height, num))
		{
			label.style.height = num;
		}
	}

	// Token: 0x06000DA4 RID: 3492 RVA: 0x00041304 File Offset: 0x0003F504
	private void RefreshContentAndScroll()
	{
		float num = 0f;
		for (int i = 0; i < this.messages.childCount; i++)
		{
			num += this.messages[i].resolvedStyle.height;
		}
		if (float.IsNaN(num))
		{
			return;
		}
		if (!this.autoScroll)
		{
			if (!Mathf.Approximately(this.messages.resolvedStyle.height, num))
			{
				this.messages.style.height = num;
			}
			return;
		}
		this.isScrolling = true;
		if (!Mathf.Approximately(this.messages.resolvedStyle.height, num))
		{
			this.messages.style.height = num;
		}
		float num2 = Mathf.Max(0f, num - this.scrollView.contentViewport.resolvedStyle.height);
		this.scrollView.verticalScroller.highValue = num2;
		Vector2 endValue = new Vector2(this.scrollView.scrollOffset.x, num2);
		Tween tween = this.smoothScrollTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.smoothScrollTween = DOTween.To(() => this.scrollView.scrollOffset, delegate(Vector2 x)
		{
			this.scrollView.scrollOffset = x;
		}, endValue, 0.2f).OnComplete(delegate
		{
			this.isScrolling = false;
		}).SetEase(Ease.Linear);
	}

	// Token: 0x06000DA5 RID: 3493 RVA: 0x00041458 File Offset: 0x0003F658
	private void SubmitMessage()
	{
		EventManager.TriggerEvent("Event_OnChatSubmitMessage", new Dictionary<string, object>
		{
			{
				"content",
				this.textField.value
			},
			{
				"isTeamChat",
				this.IsTeamChat
			}
		});
		this.StopInput();
	}

	// Token: 0x06000DA6 RID: 3494 RVA: 0x000414A8 File Offset: 0x0003F6A8
	public void AddChatMessage(ChatMessage chatMessage, Units units, bool filterProfanity)
	{
		UIChat.<>c__DisplayClass37_0 CS$<>8__locals1 = new UIChat.<>c__DisplayClass37_0();
		CS$<>8__locals1.<>4__this = this;
		VisualElement visualElement = this.messageAsset.Instantiate();
		visualElement.focusable = true;
		CS$<>8__locals1.label = visualElement.Query(null, null);
		CS$<>8__locals1.label.focusable = true;
		string chatMessagePrefix = this.GetChatMessagePrefix(chatMessage);
		string text = this.ParseChatContent(chatMessage.Content.ToString(), chatMessage.IsSystem, units, filterProfanity);
		CS$<>8__locals1.label.text = chatMessagePrefix + text;
		CS$<>8__locals1.label.style.display = ((text.Length > 0) ? DisplayStyle.Flex : DisplayStyle.None);
		CS$<>8__locals1.label.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(CS$<>8__locals1.<AddChatMessage>g__OnRowGeometryChanged|0), TrickleDown.NoTrickleDown);
		UIChatMessage uichatMessage = new UIChatMessage(chatMessage, visualElement, 5f);
		this.messages.Add(uichatMessage.VisualElement);
		this.uiChatMessages.Add(uichatMessage);
	}

	// Token: 0x06000DA7 RID: 3495 RVA: 0x00041598 File Offset: 0x0003F798
	public void RemoveChatMessage(ChatMessage chatMessage)
	{
		UIChatMessage uichatMessage = this.uiChatMessages.FirstOrDefault((UIChatMessage m) => m.ChatMessage == chatMessage);
		if (uichatMessage == null)
		{
			return;
		}
		float height = uichatMessage.VisualElement.resolvedStyle.height;
		uichatMessage.Dispose();
		this.messages.Remove(uichatMessage.VisualElement);
		this.uiChatMessages.Remove(uichatMessage);
		if (!this.autoScroll)
		{
			this.scrollView.scrollOffset = new Vector2(this.scrollView.scrollOffset.x, Mathf.Max(0f, this.scrollView.scrollOffset.y - height));
		}
		this.scrollView.schedule.Execute(new Action(this.RefreshContentAndScroll));
	}

	// Token: 0x06000DA8 RID: 3496 RVA: 0x00041664 File Offset: 0x0003F864
	public void ClearChatMessages()
	{
		this.uiChatMessages.ForEach(delegate(UIChatMessage uiChatMessage)
		{
			uiChatMessage.Dispose();
		});
		this.messages.Clear();
		this.uiChatMessages.Clear();
		Tween tween = this.smoothScrollTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.autoScroll = true;
		this.isScrolling = false;
	}

	// Token: 0x06000DA9 RID: 3497 RVA: 0x000416D1 File Offset: 0x0003F8D1
	public void SetOpacity(float opacity)
	{
		this.chat.style.opacity = new StyleFloat(opacity);
	}

	// Token: 0x06000DAA RID: 3498 RVA: 0x000416E9 File Offset: 0x0003F8E9
	public void SetScale(float scale)
	{
		this.chat.style.scale = new StyleScale(new Scale(new Vector2(scale, scale)));
	}

	// Token: 0x06000DAB RID: 3499 RVA: 0x0004170C File Offset: 0x0003F90C
	private string GetChatMessagePrefix(ChatMessage chatMessage)
	{
		string text = string.Empty;
		if (!chatMessage.IsSystem)
		{
			if (chatMessage.IsTeamChat)
			{
				text += "[TEAM] ";
			}
			string str = StringUtils.WrapInTeamColor(chatMessage.Username.ToString(), chatMessage.Team.Value);
			text = text + str + ": ";
		}
		return text;
	}

	// Token: 0x06000DAC RID: 3500 RVA: 0x0004176C File Offset: 0x0003F96C
	private string ParseChatContent(string content, bool isSystem, Units units, bool filterProfanity)
	{
		if (isSystem)
		{
			content = Regex.Replace(content, "<united>([^<]+)</united>", delegate(Match match)
			{
				string value = match.Groups[1].Value;
				float value2;
				if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out value2))
				{
					return ((units == Units.Metric) ? Utils.GameUnitsToMetric(value2) : Utils.GameUnitsToImperial(value2)).ToString("F1", CultureInfo.InvariantCulture);
				}
				return value;
			});
			content = Regex.Replace(content, "&units", (units == Units.Metric) ? "KPH" : "MPH");
		}
		else
		{
			content = StringUtils.FilterStringRichText(content);
			content = StringUtils.FilterStringSpecialCharacters(content, Constants.CHAT_WHITELIST, filterProfanity ? Constants.CHAT_BLACKLIST : null);
			if (filterProfanity)
			{
				content = StringUtils.FilterStringProfanity(content, true);
			}
		}
		return content;
	}

	// Token: 0x040008A4 RID: 2212
	private const float chatMessageMinHeight = 33f;

	// Token: 0x040008A5 RID: 2213
	[Header("References")]
	[SerializeField]
	private VisualTreeAsset messageAsset;

	// Token: 0x040008A6 RID: 2214
	[SerializeField]
	private VisualTreeAsset quickChatMessageAsset;

	// Token: 0x040008A9 RID: 2217
	private VisualElement chat;

	// Token: 0x040008AA RID: 2218
	private VisualElement quickChat;

	// Token: 0x040008AB RID: 2219
	private ScrollView scrollView;

	// Token: 0x040008AC RID: 2220
	private VisualElement messages;

	// Token: 0x040008AD RID: 2221
	private Label inputPrefix;

	// Token: 0x040008AE RID: 2222
	private TextField textField;

	// Token: 0x040008AF RID: 2223
	private string pendingOpenCharacter;

	// Token: 0x040008B0 RID: 2224
	private Label quickChatCategoryLabel;

	// Token: 0x040008B1 RID: 2225
	private VisualElement quickChatMessages;

	// Token: 0x040008B2 RID: 2226
	private List<UIChatMessage> uiChatMessages = new List<UIChatMessage>();

	// Token: 0x040008B3 RID: 2227
	private bool autoScroll = true;

	// Token: 0x040008B4 RID: 2228
	private bool isScrolling;

	// Token: 0x040008B5 RID: 2229
	private Tween smoothScrollTween;
}
