using System;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class RookieBotBrain : BotBrain
{
	// Token: 0x060002EB RID: 747 RVA: 0x00013430 File Offset: 0x00011630
	public override void Tick(in BotContext context, BotInputDriver driver)
	{
		Player player = driver.Player;
		PlayerBody playerBody = player.PlayerBody;
		if (!playerBody)
		{
			return;
		}
		Puck targetPuck = context.TargetPuck;
		Vector3 position = playerBody.transform.position;
		Vector3 vector = context.HasHomePosition ? context.HomePosition : position;
		Vector3 vector2 = targetPuck ? targetPuck.transform.position : vector;
		Vector3 vector3;
		if (player.Role == PlayerRole.Goalie)
		{
			vector3 = vector;
			vector3.x += Mathf.Clamp(vector2.x - vector.x, -1.5f, 1.5f);
		}
		else if (context.IsClosestTeammateToPuck || !context.HasHomePosition)
		{
			vector3 = vector2;
		}
		else
		{
			Vector3 b = new Vector3(Mathf.Sin(context.Seed * 7f), 0f, Mathf.Cos(context.Seed * 7f)) * 2f;
			vector3 = Vector3.Lerp(vector, vector2, 0.35f) + b;
		}
		Vector3 vector4 = vector3 - position;
		vector4.y = 0f;
		Vector3 toPuck = vector2 - position;
		toPuck.y = 0f;
		float magnitude = vector4.magnitude;
		if (magnitude < 1.5f)
		{
			driver.HoldFacing(vector2);
		}
		else
		{
			driver.MoveToward(vector3, magnitude > 8f);
		}
		driver.AimStickAt(targetPuck, toPuck, toPuck.magnitude);
	}
}
