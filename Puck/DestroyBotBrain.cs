using System;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class DestroyBotBrain : BotBrain
{
	// Token: 0x060000EC RID: 236 RVA: 0x0000545C File Offset: 0x0000365C
	public override void Tick(in BotContext context, BotInputDriver driver)
	{
		PlayerBody playerBody = driver.Player.PlayerBody;
		PlayerBody targetPlayerBody = context.TargetPlayerBody;
		if (!playerBody)
		{
			return;
		}
		if (!targetPlayerBody)
		{
			driver.HoldFacing(playerBody.transform.position + playerBody.transform.forward);
			return;
		}
		if (!this.isInitialized)
		{
			this.EnterPhase(DestroyBotBrain.Phase.Retreat);
			this.isInitialized = true;
		}
		Vector3 position = playerBody.transform.position;
		Vector3 position2 = targetPlayerBody.transform.position;
		Vector3 vector = position2 - position;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		DestroyBotBrain.Phase phase = this.phase;
		if (phase == DestroyBotBrain.Phase.Retreat)
		{
			this.TickRetreat(driver, position, position2, magnitude);
			return;
		}
		if (phase != DestroyBotBrain.Phase.Charge)
		{
			return;
		}
		this.TickCharge(driver, playerBody, position2, magnitude);
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00005524 File Offset: 0x00003724
	private void TickRetreat(BotInputDriver driver, Vector3 botPosition, Vector3 targetPosition, float distance)
	{
		if (!this.hasRetreatDirection)
		{
			Vector3 vector = botPosition - targetPosition;
			vector.y = 0f;
			this.retreatDirection = ((vector.sqrMagnitude > 0.01f) ? vector.normalized : Vector3.forward);
			this.hasRetreatDirection = true;
		}
		driver.MoveToward(botPosition + this.retreatDirection * 8f, true);
		if (distance >= 26f || Time.fixedTime - this.phaseStartTime >= 4f)
		{
			this.EnterPhase(DestroyBotBrain.Phase.Charge);
		}
	}

	// Token: 0x060000EE RID: 238 RVA: 0x000055B8 File Offset: 0x000037B8
	private void TickCharge(BotInputDriver driver, PlayerBody body, Vector3 targetPosition, float distance)
	{
		driver.MoveToward(targetPosition, true);
		this.closestApproachThisCharge = Mathf.Min(this.closestApproachThisCharge, distance);
		if (!this.hasJumpedThisCharge && distance <= 2f && body.IsGrounded)
		{
			driver.Jump();
			this.hasJumpedThisCharge = true;
		}
		bool flag = this.closestApproachThisCharge <= 1.4f;
		bool flag2 = distance > 3f;
		if ((flag && flag2) || Time.fixedTime - this.phaseStartTime >= 8f)
		{
			this.EnterPhase(DestroyBotBrain.Phase.Retreat);
		}
	}

	// Token: 0x060000EF RID: 239 RVA: 0x0000563E File Offset: 0x0000383E
	private void EnterPhase(DestroyBotBrain.Phase newPhase)
	{
		this.phase = newPhase;
		this.phaseStartTime = Time.fixedTime;
		if (newPhase == DestroyBotBrain.Phase.Retreat)
		{
			this.hasRetreatDirection = false;
		}
		if (newPhase == DestroyBotBrain.Phase.Charge)
		{
			this.hasJumpedThisCharge = false;
			this.closestApproachThisCharge = float.MaxValue;
		}
	}

	// Token: 0x04000083 RID: 131
	private const float RetreatDistance = 26f;

	// Token: 0x04000084 RID: 132
	private const float MaxRetreatDuration = 4f;

	// Token: 0x04000085 RID: 133
	private const float JumpRange = 2f;

	// Token: 0x04000086 RID: 134
	private const float ContactReached = 1.4f;

	// Token: 0x04000087 RID: 135
	private const float LeavingDistance = 3f;

	// Token: 0x04000088 RID: 136
	private const float MaxChargeDuration = 8f;

	// Token: 0x04000089 RID: 137
	private DestroyBotBrain.Phase phase;

	// Token: 0x0400008A RID: 138
	private bool isInitialized;

	// Token: 0x0400008B RID: 139
	private float phaseStartTime;

	// Token: 0x0400008C RID: 140
	private Vector3 retreatDirection = Vector3.forward;

	// Token: 0x0400008D RID: 141
	private bool hasRetreatDirection;

	// Token: 0x0400008E RID: 142
	private bool hasJumpedThisCharge;

	// Token: 0x0400008F RID: 143
	private float closestApproachThisCharge = float.MaxValue;

	// Token: 0x0200002F RID: 47
	private enum Phase
	{
		// Token: 0x04000091 RID: 145
		Retreat,
		// Token: 0x04000092 RID: 146
		Charge
	}
}
