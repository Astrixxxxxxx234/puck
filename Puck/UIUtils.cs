using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

// Token: 0x02000246 RID: 582
public static class UIUtils
{
	// Token: 0x0600124F RID: 4687 RVA: 0x000551A0 File Offset: 0x000533A0
	public static void SetTeamClass(VisualElement element, PlayerTeam team)
	{
		foreach (object obj in Enum.GetValues(typeof(PlayerTeam)))
		{
			PlayerTeam team2 = (PlayerTeam)obj;
			element.EnableInClassList(UIUtils.GetClassFromTeam(team2), false);
		}
		element.EnableInClassList(UIUtils.GetClassFromTeam(team), true);
	}

	// Token: 0x06001250 RID: 4688 RVA: 0x00055218 File Offset: 0x00053418
	public static string GetClassFromTeam(PlayerTeam team)
	{
		return "team" + team.ToString();
	}

	// Token: 0x06001251 RID: 4689 RVA: 0x00055234 File Offset: 0x00053434
	public static List<VisualElement> GetVisualElementChildren(VisualElement element, bool recursive = false)
	{
		if (recursive)
		{
			List<VisualElement> list = new List<VisualElement>();
			foreach (VisualElement visualElement in element.hierarchy.Children())
			{
				list.Add(visualElement);
				list.AddRange(UIUtils.GetVisualElementChildren(visualElement, true));
			}
			return list;
		}
		return new List<VisualElement>(element.hierarchy.Children());
	}
}
