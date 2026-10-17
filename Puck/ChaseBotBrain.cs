using System;

// Token: 0x0200002D RID: 45
public class ChaseBotBrain : BotBrain
{
	// Token: 0x060000EA RID: 234 RVA: 0x000053D0 File Offset: 0x000035D0
	public override void Tick(in BotContext context, BotInputDriver driver)
	{
		Puck targetPuck = context.TargetPuck;
		PlayerBody playerBody = driver.Player.PlayerBody;
		if (targetPuck && playerBody)
		{
			float magnitude = (targetPuck.transform.position - playerBody.transform.position).magnitude;
			driver.MoveToward(targetPuck.transform.position, magnitude > 10f);
		}
		else
		{
			driver.SkateForwardWeaving();
		}
		driver.WaveStick();
		driver.LookAround();
	}
}
