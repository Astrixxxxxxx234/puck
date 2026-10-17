using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	// Token: 0x020002A5 RID: 677
	[UxmlElement]
	public class Spinner : VisualElement
	{
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06001464 RID: 5220 RVA: 0x0005935E File Offset: 0x0005755E
		// (set) Token: 0x06001465 RID: 5221 RVA: 0x00059366 File Offset: 0x00057566
		[UxmlAttribute]
		public float speed { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06001466 RID: 5222 RVA: 0x0005936F File Offset: 0x0005756F
		// (set) Token: 0x06001467 RID: 5223 RVA: 0x00059377 File Offset: 0x00057577
		[UxmlAttribute]
		public SpinnerDirection direction { get; set; }

		// Token: 0x06001468 RID: 5224 RVA: 0x00059380 File Offset: 0x00057580
		public Spinner()
		{
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.NoTrickleDown);
			base.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnDetachFromPanel), TrickleDown.NoTrickleDown);
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x000593AE File Offset: 0x000575AE
		private void OnAttachToPanel(AttachToPanelEvent e)
		{
			this.scheduledItem = base.schedule.Execute(new Action(this.OnScheduleUpdate)).Every(16L);
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x000593D5 File Offset: 0x000575D5
		private void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			this.scheduledItem.Pause();
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x000593E4 File Offset: 0x000575E4
		private void OnScheduleUpdate()
		{
			base.style.rotate = new Rotate(base.style.rotate.value.angle.value + this.speed * (float)((this.direction == SpinnerDirection.Clockwise) ? 1 : -1));
		}

		// Token: 0x04000CCE RID: 3278
		private IVisualElementScheduledItem scheduledItem;

		// Token: 0x020002A6 RID: 678
		[CompilerGenerated]
		[Serializable]
		public new class UxmlSerializedData : VisualElement.UxmlSerializedData
		{
			// Token: 0x0600146C RID: 5228 RVA: 0x00059444 File Offset: 0x00057644
			[RegisterUxmlCache]
			[Conditional("UNITY_EDITOR")]
			public new static void Register()
			{
				UxmlDescriptionCache.RegisterType(typeof(Spinner.UxmlSerializedData), new UxmlAttributeNames[]
				{
					new UxmlAttributeNames("speed", "speed", null, Array.Empty<string>()),
					new UxmlAttributeNames("direction", "direction", null, Array.Empty<string>())
				}, false);
			}

			// Token: 0x0600146D RID: 5229 RVA: 0x0005949F File Offset: 0x0005769F
			public override object CreateInstance()
			{
				return new Spinner();
			}

			// Token: 0x0600146E RID: 5230 RVA: 0x000594A8 File Offset: 0x000576A8
			public override void Deserialize(object obj)
			{
				base.Deserialize(obj);
				Spinner spinner = (Spinner)obj;
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.speed_UxmlAttributeFlags))
				{
					spinner.speed = this.speed;
				}
				if (UnityEngine.UIElements.UxmlSerializedData.ShouldWriteAttributeValue(this.direction_UxmlAttributeFlags))
				{
					spinner.direction = this.direction;
				}
			}

			// Token: 0x04000CCF RID: 3279
			[SerializeField]
			private float speed;

			// Token: 0x04000CD0 RID: 3280
			[SerializeField]
			private SpinnerDirection direction;

			// Token: 0x04000CD1 RID: 3281
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags speed_UxmlAttributeFlags;

			// Token: 0x04000CD2 RID: 3282
			[SerializeField]
			[UxmlIgnore]
			[HideInInspector]
			private UnityEngine.UIElements.UxmlSerializedData.UxmlAttributeFlags direction_UxmlAttributeFlags;
		}
	}
}
