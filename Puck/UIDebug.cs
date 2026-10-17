using System;
using UnityEngine.UIElements;

// Token: 0x020001CB RID: 459
public class UIDebug : UIView
{
	// Token: 0x06000DCE RID: 3534 RVA: 0x00041C80 File Offset: 0x0003FE80
	public void Initialize(VisualElement rootVisualElement)
	{
		base.View = rootVisualElement.Query("DebugView", null);
		this.buildLabel = base.View.Query("BuildLabel", null);
		this.hintLabel = base.View.Query("HintLabel", null);
		this.simpleView = base.View.Query("Simple", null);
		this.simpleHintLabel = base.View.Query("SimpleHintLabel", null);
		this.networkingHintLabel = base.View.Query("NetworkingHintLabel", null);
		this.simpleRoundTripTimeRow = base.View.Query("SimpleRoundTripTimeRow", null);
		this.simpleLossRow = base.View.Query("SimpleLossRow", null);
		this.simpleFramesLabel = base.View.Query("SimpleFramesLabel", null);
		this.simpleFrameTimeP95Label = base.View.Query("SimpleFrameTimeP95Label", null);
		this.simpleRoundTripTimeLabel = base.View.Query("SimpleRoundTripTimeLabel", null);
		this.simpleLossLabel = base.View.Query("SimpleLossLabel", null);
		this.networkingView = base.View.Query("Networking", null);
		this.applicationSectionView = base.View.Query("Application", null);
		this.applicationFramesLabel = base.View.Query("ApplicationFramesLabel", null);
		this.applicationFrameTimeAverageLabel = base.View.Query("ApplicationFrameTimeAverageLabel", null);
		this.applicationFrameTimeP95Label = base.View.Query("ApplicationFrameTimeP95Label", null);
		this.applicationFrameTimeMaxLabel = base.View.Query("ApplicationFrameTimeMaxLabel", null);
		this.networkSectionView = base.View.Query("Network", null);
		this.networkSentWireBytesLabel = base.View.Query("NetworkSentWireBytesLabel", null);
		this.networkSentPacketsLabel = base.View.Query("NetworkSentPacketsLabel", null);
		this.networkReceivedWireBytesLabel = base.View.Query("NetworkReceivedWireBytesLabel", null);
		this.networkReceivedPacketsLabel = base.View.Query("NetworkReceivedPacketsLabel", null);
		this.networkRoundTripTimeLabel = base.View.Query("NetworkRoundTripTimeLabel", null);
		this.networkStallLabel = base.View.Query("NetworkStallLabel", null);
		this.objectSynchronizationSectionView = base.View.Query("ObjectSynchronization", null);
		this.objectSynchronizationTicksLabel = base.View.Query("ObjectSynchronizationTicksLabel", null);
		this.objectSynchronizationTickDeltaAverageLabel = base.View.Query("ObjectSynchronizationTickDeltaAverageLabel", null);
		this.objectSynchronizationTickDeltaP95Label = base.View.Query("ObjectSynchronizationTickDeltaP95Label", null);
		this.objectSynchronizationTickDeltaMaxLabel = base.View.Query("ObjectSynchronizationTickDeltaMaxLabel", null);
		this.objectSynchronizationLossLabel = base.View.Query("ObjectSynchronizationLossLabel", null);
		this.objectSynchronizationReorderedLabel = base.View.Query("ObjectSynchronizationReorderedLabel", null);
		this.objectSynchronizationTimelineLabel = base.View.Query("ObjectSynchronizationTimelineLabel", null);
		this.objectSynchronizationSpeedLabel = base.View.Query("ObjectSynchronizationSpeedLabel", null);
		this.objectSynchronizationSnapsLabel = base.View.Query("ObjectSynchronizationSnapsLabel", null);
		this.objectSynchronizationExtrapolationLabel = base.View.Query("ObjectSynchronizationExtrapolationLabel", null);
		this.SetApplicationVisible(false);
		this.SetNetworkVisible(false);
		this.SetObjectSynchronizationVisible(false);
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x0004208D File Offset: 0x0004028D
	public void SetMode(DebugMode value)
	{
		this.debugMode = value;
		this.RefreshSectionsVisible();
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x0004209C File Offset: 0x0004029C
	public void SetSimpleApplication(string framesText, string frameTimeP95Text)
	{
		this.simpleFramesLabel.text = framesText;
		this.simpleFrameTimeP95Label.text = frameTimeP95Text;
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x000420B6 File Offset: 0x000402B6
	public void SetSimpleRoundTripTime(string text)
	{
		this.simpleRoundTripTimeLabel.text = text;
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x000420C4 File Offset: 0x000402C4
	public void SetSimpleLoss(string text)
	{
		this.simpleLossLabel.text = text;
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x000420D2 File Offset: 0x000402D2
	public void SetApplicationVisible(bool value)
	{
		this.isApplicationVisible = value;
		this.applicationSectionView.style.display = (value ? DisplayStyle.Flex : DisplayStyle.None);
		this.RefreshSectionsVisible();
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x000420FD File Offset: 0x000402FD
	public void SetApplicationFrames(string text)
	{
		this.applicationFramesLabel.text = text;
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x0004210B File Offset: 0x0004030B
	public void SetApplicationFrameTime(string averageText, string p95Text, string maxText)
	{
		this.applicationFrameTimeAverageLabel.text = averageText;
		this.applicationFrameTimeP95Label.text = p95Text;
		this.applicationFrameTimeMaxLabel.text = maxText;
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x00042131 File Offset: 0x00040331
	public void SetNetworkVisible(bool value)
	{
		this.isNetworkVisible = value;
		this.networkSectionView.style.display = (value ? DisplayStyle.Flex : DisplayStyle.None);
		this.RefreshSectionsVisible();
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x0004215C File Offset: 0x0004035C
	public void SetNetworkSent(string wireBytesText, string packetsText)
	{
		this.networkSentWireBytesLabel.text = wireBytesText;
		this.networkSentPacketsLabel.text = packetsText;
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x00042176 File Offset: 0x00040376
	public void SetNetworkReceived(string wireBytesText, string packetsText)
	{
		this.networkReceivedWireBytesLabel.text = wireBytesText;
		this.networkReceivedPacketsLabel.text = packetsText;
	}

	// Token: 0x06000DD9 RID: 3545 RVA: 0x00042190 File Offset: 0x00040390
	public void SetNetworkRoundTripTime(string text)
	{
		this.networkRoundTripTimeLabel.text = text;
	}

	// Token: 0x06000DDA RID: 3546 RVA: 0x0004219E File Offset: 0x0004039E
	public void SetNetworkStall(string text)
	{
		this.networkStallLabel.text = text;
	}

	// Token: 0x06000DDB RID: 3547 RVA: 0x000421AC File Offset: 0x000403AC
	public void SetObjectSynchronizationVisible(bool value)
	{
		this.isObjectSynchronizationVisible = value;
		this.objectSynchronizationSectionView.style.display = (value ? DisplayStyle.Flex : DisplayStyle.None);
		this.RefreshSectionsVisible();
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x000421D8 File Offset: 0x000403D8
	private void RefreshSectionsVisible()
	{
		bool flag = this.isApplicationVisible || this.isNetworkVisible || this.isObjectSynchronizationVisible;
		bool flag2 = this.debugMode == DebugMode.Advanced && flag;
		this.simpleView.style.display = ((this.debugMode == DebugMode.Simple) ? DisplayStyle.Flex : DisplayStyle.None);
		this.networkingView.style.display = (flag2 ? DisplayStyle.Flex : DisplayStyle.None);
		this.simpleRoundTripTimeRow.style.display = (this.isNetworkVisible ? DisplayStyle.Flex : DisplayStyle.None);
		this.simpleLossRow.style.display = (this.isObjectSynchronizationVisible ? DisplayStyle.Flex : DisplayStyle.None);
	}

	// Token: 0x06000DDD RID: 3549 RVA: 0x0004228B File Offset: 0x0004048B
	public override bool Show()
	{
		return SettingsManager.Debug != DebugMode.Off && base.Show();
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x000422A1 File Offset: 0x000404A1
	public override bool Hide()
	{
		return SettingsManager.Debug == DebugMode.Off && base.Hide();
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x000422B7 File Offset: 0x000404B7
	public void SetBuild(string text)
	{
		this.buildLabel.text = text;
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x000422C5 File Offset: 0x000404C5
	public void SetHint(string text)
	{
		this.hintLabel.text = text;
		this.simpleHintLabel.text = text;
		this.networkingHintLabel.text = text;
	}

	// Token: 0x06000DE1 RID: 3553 RVA: 0x000422EB File Offset: 0x000404EB
	public void SetObjectSynchronizationTicks(string text)
	{
		this.objectSynchronizationTicksLabel.text = text;
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x000422F9 File Offset: 0x000404F9
	public void SetObjectSynchronizationTickDelta(string averageText, string p95Text, string maxText)
	{
		this.objectSynchronizationTickDeltaAverageLabel.text = averageText;
		this.objectSynchronizationTickDeltaP95Label.text = p95Text;
		this.objectSynchronizationTickDeltaMaxLabel.text = maxText;
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x0004231F File Offset: 0x0004051F
	public void SetObjectSynchronizationLoss(string text)
	{
		this.objectSynchronizationLossLabel.text = text;
	}

	// Token: 0x06000DE4 RID: 3556 RVA: 0x0004232D File Offset: 0x0004052D
	public void SetObjectSynchronizationReordered(string text)
	{
		this.objectSynchronizationReorderedLabel.text = text;
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x0004233B File Offset: 0x0004053B
	public void SetObjectSynchronizationTimeline(string text)
	{
		this.objectSynchronizationTimelineLabel.text = text;
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x00042349 File Offset: 0x00040549
	public void SetObjectSynchronizationSpeed(string text)
	{
		this.objectSynchronizationSpeedLabel.text = text;
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x00042357 File Offset: 0x00040557
	public void SetObjectSynchronizationSnaps(string text)
	{
		this.objectSynchronizationSnapsLabel.text = text;
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x00042365 File Offset: 0x00040565
	public void SetObjectSynchronizationExtrapolation(string text)
	{
		this.objectSynchronizationExtrapolationLabel.text = text;
	}

	// Token: 0x040008C0 RID: 2240
	private Label buildLabel;

	// Token: 0x040008C1 RID: 2241
	private Label hintLabel;

	// Token: 0x040008C2 RID: 2242
	private DebugMode debugMode;

	// Token: 0x040008C3 RID: 2243
	private VisualElement simpleView;

	// Token: 0x040008C4 RID: 2244
	private Label simpleHintLabel;

	// Token: 0x040008C5 RID: 2245
	private Label networkingHintLabel;

	// Token: 0x040008C6 RID: 2246
	private VisualElement simpleRoundTripTimeRow;

	// Token: 0x040008C7 RID: 2247
	private VisualElement simpleLossRow;

	// Token: 0x040008C8 RID: 2248
	private Label simpleFramesLabel;

	// Token: 0x040008C9 RID: 2249
	private Label simpleFrameTimeP95Label;

	// Token: 0x040008CA RID: 2250
	private Label simpleRoundTripTimeLabel;

	// Token: 0x040008CB RID: 2251
	private Label simpleLossLabel;

	// Token: 0x040008CC RID: 2252
	private VisualElement networkingView;

	// Token: 0x040008CD RID: 2253
	private bool isApplicationVisible;

	// Token: 0x040008CE RID: 2254
	private bool isNetworkVisible;

	// Token: 0x040008CF RID: 2255
	private bool isObjectSynchronizationVisible;

	// Token: 0x040008D0 RID: 2256
	private VisualElement applicationSectionView;

	// Token: 0x040008D1 RID: 2257
	private Label applicationFramesLabel;

	// Token: 0x040008D2 RID: 2258
	private Label applicationFrameTimeAverageLabel;

	// Token: 0x040008D3 RID: 2259
	private Label applicationFrameTimeP95Label;

	// Token: 0x040008D4 RID: 2260
	private Label applicationFrameTimeMaxLabel;

	// Token: 0x040008D5 RID: 2261
	private VisualElement networkSectionView;

	// Token: 0x040008D6 RID: 2262
	private Label networkSentWireBytesLabel;

	// Token: 0x040008D7 RID: 2263
	private Label networkSentPacketsLabel;

	// Token: 0x040008D8 RID: 2264
	private Label networkReceivedWireBytesLabel;

	// Token: 0x040008D9 RID: 2265
	private Label networkReceivedPacketsLabel;

	// Token: 0x040008DA RID: 2266
	private Label networkRoundTripTimeLabel;

	// Token: 0x040008DB RID: 2267
	private Label networkStallLabel;

	// Token: 0x040008DC RID: 2268
	private VisualElement objectSynchronizationSectionView;

	// Token: 0x040008DD RID: 2269
	private Label objectSynchronizationTicksLabel;

	// Token: 0x040008DE RID: 2270
	private Label objectSynchronizationTickDeltaAverageLabel;

	// Token: 0x040008DF RID: 2271
	private Label objectSynchronizationTickDeltaP95Label;

	// Token: 0x040008E0 RID: 2272
	private Label objectSynchronizationTickDeltaMaxLabel;

	// Token: 0x040008E1 RID: 2273
	private Label objectSynchronizationLossLabel;

	// Token: 0x040008E2 RID: 2274
	private Label objectSynchronizationReorderedLabel;

	// Token: 0x040008E3 RID: 2275
	private Label objectSynchronizationTimelineLabel;

	// Token: 0x040008E4 RID: 2276
	private Label objectSynchronizationSpeedLabel;

	// Token: 0x040008E5 RID: 2277
	private Label objectSynchronizationSnapsLabel;

	// Token: 0x040008E6 RID: 2278
	private Label objectSynchronizationExtrapolationLabel;
}
