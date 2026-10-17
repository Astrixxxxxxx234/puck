using System;
using Unity.Netcode;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class BotInputDriver : MonoBehaviour
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x060000CC RID: 204 RVA: 0x00004494 File Offset: 0x00002694
	// (set) Token: 0x060000CD RID: 205 RVA: 0x0000449C File Offset: 0x0000269C
	public Player Player { get; private set; }

	// Token: 0x060000CE RID: 206 RVA: 0x000044A5 File Offset: 0x000026A5
	private void Awake()
	{
		this.Player = base.GetComponent<Player>();
		this.seed = this.Player.OwnerClientId % 100UL * 0.63f;
		this.brain = BotManager.CreateBrain(BotManager.DefaultBehavior);
	}

	// Token: 0x060000CF RID: 207 RVA: 0x000044E0 File Offset: 0x000026E0
	public void SetBrain(BotBrain newBrain)
	{
		this.brain = newBrain;
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x000044EC File Offset: 0x000026EC
	private void FixedUpdate()
	{
		if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer)
		{
			return;
		}
		if (!this.Player || !this.Player.PlayerInput || !this.Player.IsCharacterSpawned)
		{
			return;
		}
		if (Time.fixedTime >= this.nextTargetRefreshTime)
		{
			this.RefreshContext();
			this.nextTargetRefreshTime = Time.fixedTime + 0.5f;
		}
		BotBrain botBrain = this.brain;
		if (botBrain == null)
		{
			return;
		}
		botBrain.Tick(this.context, this);
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x0000457C File Offset: 0x0000277C
	private void RefreshContext()
	{
		this.context.Seed = this.seed;
		this.context.TargetPuck = (MonoBehaviourSingleton<PuckManager>.Instance ? MonoBehaviourSingleton<PuckManager>.Instance.GetPuck(false) : null);
		if (this.Player.PlayerPosition)
		{
			this.context.HomePosition = this.Player.PlayerPosition.transform.position;
			this.context.HasHomePosition = true;
		}
		else
		{
			this.context.HasHomePosition = false;
		}
		this.context.IsClosestTeammateToPuck = false;
		this.context.TargetPlayerBody = null;
		PlayerBody playerBody = this.Player.PlayerBody;
		if (!playerBody || !MonoBehaviourSingleton<PlayerManager>.Instance)
		{
			return;
		}
		Vector3 position = playerBody.transform.position;
		bool flag = this.context.TargetPuck;
		Vector3 a = flag ? this.context.TargetPuck.transform.position : Vector3.zero;
		float num = flag ? (a - position).sqrMagnitude : 0f;
		bool flag2 = flag;
		float num2 = float.MaxValue;
		foreach (Player player in MonoBehaviourSingleton<PlayerManager>.Instance.GetPlayers(false))
		{
			if (!(player == this.Player) && player.IsCharacterSpawned && player.PlayerBody)
			{
				Vector3 position2 = player.PlayerBody.transform.position;
				if (player.OwnerClientId < 100000UL)
				{
					float sqrMagnitude = (position2 - position).sqrMagnitude;
					if (sqrMagnitude < num2)
					{
						num2 = sqrMagnitude;
						this.context.TargetPlayerBody = player.PlayerBody;
					}
				}
				if (flag2 && player.Team == this.Player.Team && player.Role != PlayerRole.Goalie && (a - position2).sqrMagnitude < num)
				{
					flag2 = false;
				}
			}
		}
		this.context.IsClosestTeammateToPuck = flag2;
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x000047B8 File Offset: 0x000029B8
	public void Jump()
	{
		this.Player.PlayerBody.Jump();
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x000047CC File Offset: 0x000029CC
	public void MoveToward(Vector3 position, bool sprint)
	{
		PlayerInput playerInput = this.Player.PlayerInput;
		PlayerBody playerBody = this.Player.PlayerBody;
		Vector3 to = position - playerBody.transform.position;
		to.y = 0f;
		float num = Vector3.SignedAngle(playerBody.transform.forward, to, Vector3.up);
		playerInput.MoveInput.ServerValue = new Vector2(Mathf.Clamp(num / 45f, -1f, 1f), 1f);
		playerInput.SprintInput.ServerValue = sprint;
		playerInput.StopInput.ServerValue = false;
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00004868 File Offset: 0x00002A68
	public void HoldFacing(Vector3 facePosition)
	{
		PlayerInput playerInput = this.Player.PlayerInput;
		PlayerBody playerBody = this.Player.PlayerBody;
		Vector3 to = facePosition - playerBody.transform.position;
		to.y = 0f;
		float num = Vector3.SignedAngle(playerBody.transform.forward, to, Vector3.up);
		playerInput.MoveInput.ServerValue = new Vector2(Mathf.Clamp(num / 45f, -1f, 1f), 0f);
		playerInput.SprintInput.ServerValue = false;
		playerInput.StopInput.ServerValue = (playerBody.Rigidbody.linearVelocity.magnitude > 1f);
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x0000491C File Offset: 0x00002B1C
	public void SkateForwardWeaving()
	{
		float num = Time.fixedTime + this.seed;
		this.Player.PlayerInput.MoveInput.ServerValue = new Vector2(Mathf.Sin(num * 0.7f), 1f);
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00004964 File Offset: 0x00002B64
	public void WaveStick()
	{
		float num = Time.fixedTime + this.seed;
		this.Player.PlayerInput.StickRaycastOriginAngleInput.ServerValue = new Vector2(Mathf.Lerp(20f, 70f, (Mathf.Sin(num * 1.3f) + 1f) * 0.5f), Mathf.Sin(num * 0.9f) * 80f);
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x000049D4 File Offset: 0x00002BD4
	public void LookAround()
	{
		float num = Time.fixedTime + this.seed;
		this.Player.PlayerInput.LookAngleInput.ServerValue = new Vector2(30f, Mathf.Sin(num * 0.5f) * 90f);
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00004A20 File Offset: 0x00002C20
	public void AimStickAt(Puck puck, Vector3 toPuck, float puckDistance)
	{
		PlayerInput playerInput = this.Player.PlayerInput;
		PlayerBody playerBody = this.Player.PlayerBody;
		float num = Vector3.SignedAngle(playerBody.transform.forward, toPuck, Vector3.up);
		float x = 40f;
		StickPositioner stickPositioner = this.Player.StickPositioner;
		if (stickPositioner && puck)
		{
			Vector3 raycastOriginPosition = stickPositioner.RaycastOriginPosition;
			Vector3 vector = puck.transform.position - raycastOriginPosition;
			float magnitude = new Vector2(vector.x, vector.z).magnitude;
			float num2 = Mathf.Max(raycastOriginPosition.y - puck.transform.position.y, 0.1f);
			float a = Mathf.Acos(Mathf.Clamp01(magnitude / 2.5f)) * 57.29578f;
			float b = Mathf.Asin(Mathf.Clamp01(num2 / 2.5f)) * 57.29578f + 3f;
			x = Mathf.Max(a, b);
			num = Vector3.SignedAngle(playerBody.transform.forward, new Vector3(vector.x, 0f, vector.z), Vector3.up);
		}
		float y = num;
		if (puckDistance < 3.5f)
		{
			if (Time.fixedTime >= this.nextSwingTime)
			{
				this.swingStartTime = Time.fixedTime;
				this.nextSwingTime = Time.fixedTime + 0.4f + 1.2f + Mathf.Sin(this.seed * 13f) * 0.3f;
			}
			float num3 = (Time.fixedTime - this.swingStartTime) / 0.4f;
			if (num3 <= 1f)
			{
				y = num + Mathf.Lerp(-50f, 50f, num3);
			}
		}
		Vector2 serverValue = Utils.Vector2Clamp(new Vector2(x, y), playerInput.MinimumStickRaycastOriginAngle, playerInput.MaximumStickRaycastOriginAngle);
		playerInput.StickRaycastOriginAngleInput.ServerValue = serverValue;
		playerInput.LookAngleInput.ServerValue = new Vector2(25f, Mathf.Clamp(num, -90f, 90f));
	}

	// Token: 0x04000069 RID: 105
	private const float StickMaximumReach = 2.5f;

	// Token: 0x0400006A RID: 106
	private const float TargetRefreshInterval = 0.5f;

	// Token: 0x0400006B RID: 107
	private const float SwingRange = 3.5f;

	// Token: 0x0400006C RID: 108
	private const float SwingDuration = 0.4f;

	// Token: 0x0400006D RID: 109
	private const float SwingCooldown = 1.2f;

	// Token: 0x0400006E RID: 110
	private const float SwingSweepAngle = 50f;

	// Token: 0x04000070 RID: 112
	private BotBrain brain;

	// Token: 0x04000071 RID: 113
	private float seed;

	// Token: 0x04000072 RID: 114
	private BotContext context;

	// Token: 0x04000073 RID: 115
	private float nextTargetRefreshTime;

	// Token: 0x04000074 RID: 116
	private float swingStartTime = float.MinValue;

	// Token: 0x04000075 RID: 117
	private float nextSwingTime;
}
