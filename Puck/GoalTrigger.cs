using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class GoalTrigger : MonoBehaviour
{
	// Token: 0x0600004C RID: 76 RVA: 0x00002D84 File Offset: 0x00000F84
	private void OnTriggerEnter(Collider collider)
	{
		Puck componentInParent = collider.GetComponentInParent<Puck>();
		if (!componentInParent)
		{
			return;
		}
		this.goal.Server_OnPuckEnterGoal(componentInParent);
	}

	// Token: 0x0400002E RID: 46
	[Header("References")]
	[SerializeField]
	private Goal goal;
}
