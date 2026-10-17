using System;
using System.Text.Json.Serialization;
using UnityEngine.InputSystem;

// Token: 0x020000D9 RID: 217
public class KeyBind
{
	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00025394 File Offset: 0x00023594
	[JsonIgnore]
	public bool IsComposite
	{
		get
		{
			return this.InputAction.bindings[0].isComposite;
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x060006D3 RID: 1747 RVA: 0x000253BD File Offset: 0x000235BD
	// (set) Token: 0x060006D4 RID: 1748 RVA: 0x000253C5 File Offset: 0x000235C5
	public string ModifierPath { get; set; }

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x060006D5 RID: 1749 RVA: 0x000253CE File Offset: 0x000235CE
	// (set) Token: 0x060006D6 RID: 1750 RVA: 0x000253D6 File Offset: 0x000235D6
	public string Path { get; set; }

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x060006D7 RID: 1751 RVA: 0x000253DF File Offset: 0x000235DF
	// (set) Token: 0x060006D8 RID: 1752 RVA: 0x000253E7 File Offset: 0x000235E7
	public string Interactions { get; set; }

	// Token: 0x060006D9 RID: 1753 RVA: 0x0000239A File Offset: 0x0000059A
	[JsonConstructor]
	public KeyBind()
	{
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x000253F0 File Offset: 0x000235F0
	public KeyBind(InputAction inputAction)
	{
		this.InputAction = inputAction;
		this.Update(this.InputAction);
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x0002540C File Offset: 0x0002360C
	public void Update(InputAction inputAction)
	{
		this.InputAction = inputAction;
		this.ModifierPath = (this.IsComposite ? inputAction.bindings[1].effectivePath : null);
		this.Path = (this.IsComposite ? inputAction.bindings[2].effectivePath : inputAction.bindings[0].effectivePath);
		this.Interactions = inputAction.bindings[0].effectiveInteractions;
	}

	// Token: 0x0400045F RID: 1119
	[JsonIgnore]
	public InputAction InputAction;
}
