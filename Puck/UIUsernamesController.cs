using System;
using System.Collections.Generic;

// Token: 0x0200021D RID: 541
public class UIUsernamesController : UIViewController<UIUsernames>
{
	// Token: 0x06001144 RID: 4420 RVA: 0x00051F90 File Offset: 0x00050190
	public override void Awake()
	{
		base.Awake();
		this.uiUsernames = base.GetComponent<UIUsernames>();
		EventManager.AddEventListener("Event_Everyone_OnLevelSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnLevelSpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerBodySpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodySpawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerBodyDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodyDespawned));
		EventManager.AddEventListener("Event_Everyone_OnPlayerUsernameChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerUsernameChanged));
		EventManager.AddEventListener("Event_Everyone_OnPlayerNumberChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerNumberChanged));
		EventManager.AddEventListener("Event_OnShowPlayerUsernamesChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPlayerUsernamesChanged));
		EventManager.AddEventListener("Event_OnPlayerUsernamesFadeThresholdChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerUsernamesFadeThresholdChanged));
	}

	// Token: 0x06001145 RID: 4421 RVA: 0x00052049 File Offset: 0x00050249
	private void Start()
	{
		this.uiUsernames.FadeThreshold = SettingsManager.PlayerUsernamesFadeThreshold;
	}

	// Token: 0x06001146 RID: 4422 RVA: 0x0005205C File Offset: 0x0005025C
	public override void OnDestroy()
	{
		EventManager.RemoveEventListener("Event_Everyone_OnLevelSpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnLevelSpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerBodySpawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodySpawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerBodyDespawned", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerBodyDespawned));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerUsernameChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerUsernameChanged));
		EventManager.RemoveEventListener("Event_Everyone_OnPlayerNumberChanged", new Action<Dictionary<string, object>>(this.Event_Everyone_OnPlayerNumberChanged));
		EventManager.RemoveEventListener("Event_OnShowPlayerUsernamesChanged", new Action<Dictionary<string, object>>(this.Event_OnShowPlayerUsernamesChanged));
		EventManager.RemoveEventListener("Event_OnPlayerUsernamesFadeThresholdChanged", new Action<Dictionary<string, object>>(this.Event_OnPlayerUsernamesFadeThresholdChanged));
		base.OnDestroy();
	}

	// Token: 0x06001147 RID: 4423 RVA: 0x0005210C File Offset: 0x0005030C
	private void Event_Everyone_OnLevelSpawned(Dictionary<string, object> message)
	{
		Level level = (Level)message["level"];
		this.uiUsernames.Bounds = level.Bounds;
	}

	// Token: 0x06001148 RID: 4424 RVA: 0x0005213C File Offset: 0x0005033C
	private void Event_Everyone_OnPlayerBodySpawned(Dictionary<string, object> message)
	{
		PlayerBody playerBody = (PlayerBody)message["playerBody"];
		this.uiUsernames.AddPlayerBody(playerBody);
	}

	// Token: 0x06001149 RID: 4425 RVA: 0x00052168 File Offset: 0x00050368
	private void Event_Everyone_OnPlayerBodyDespawned(Dictionary<string, object> message)
	{
		PlayerBody playerBody = (PlayerBody)message["playerBody"];
		this.uiUsernames.RemovePlayerBody(playerBody);
	}

	// Token: 0x0600114A RID: 4426 RVA: 0x00052194 File Offset: 0x00050394
	private void Event_Everyone_OnPlayerUsernameChanged(Dictionary<string, object> message)
	{
		PlayerBody playerBody = ((Player)message["player"]).PlayerBody;
		if (playerBody)
		{
			this.uiUsernames.StyleUsername(playerBody);
		}
	}

	// Token: 0x0600114B RID: 4427 RVA: 0x000521CC File Offset: 0x000503CC
	private void Event_Everyone_OnPlayerNumberChanged(Dictionary<string, object> message)
	{
		PlayerBody playerBody = ((Player)message["player"]).PlayerBody;
		if (playerBody)
		{
			this.uiUsernames.StyleUsername(playerBody);
		}
	}

	// Token: 0x0600114C RID: 4428 RVA: 0x00052203 File Offset: 0x00050403
	private void Event_OnShowPlayerUsernamesChanged(Dictionary<string, object> message)
	{
		if ((bool)message["value"])
		{
			this.uiUsernames.Show();
			return;
		}
		this.uiUsernames.Hide();
	}

	// Token: 0x0600114D RID: 4429 RVA: 0x00052230 File Offset: 0x00050430
	private void Event_OnPlayerUsernamesFadeThresholdChanged(Dictionary<string, object> message)
	{
		float fadeThreshold = (float)message["value"];
		this.uiUsernames.FadeThreshold = fadeThreshold;
	}

	// Token: 0x04000AC3 RID: 2755
	private static readonly Logger Logger = new Logger("UIUsernamesController");

	// Token: 0x04000AC4 RID: 2756
	private UIUsernames uiUsernames;
}
