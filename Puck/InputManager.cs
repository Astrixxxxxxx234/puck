using System;
using System.Collections.Generic;
using System.Text.Json;
using UnityEngine.InputSystem;

// Token: 0x020000DC RID: 220
public static class InputManager
{
	// Token: 0x060006DC RID: 1756 RVA: 0x000254A4 File Offset: 0x000236A4
	public static void Initialize()
	{
		InputSystem.RegisterInteraction<DoublePressInteraction>(null);
		InputSystem.RegisterInteraction<ToggleInteraction>(null);
		InputManager.DisableControllers();
		InputSystem.onDeviceChange += InputManager.OnInputDeviceChange;
		InputActionAsset actions = InputSystem.actions;
		InputManager.MoveForwardAction = actions.FindAction("Move Forward", false);
		InputManager.MoveBackwardAction = actions.FindAction("Move Backward", false);
		InputManager.TurnLeftAction = actions.FindAction("Turn Left", false);
		InputManager.TurnRightAction = actions.FindAction("Turn Right", false);
		InputManager.StickAction = actions.FindAction("Stick", false);
		InputManager.BladeAngleUpAction = actions.FindAction("Blade Angle Up", false);
		InputManager.BladeAngleDownAction = actions.FindAction("Blade Angle Down", false);
		InputManager.SlideAction = actions.FindAction("Slide", false);
		InputManager.SprintAction = actions.FindAction("Sprint", false);
		InputManager.TrackAction = actions.FindAction("Track", false);
		InputManager.LookAction = actions.FindAction("Look", false);
		InputManager.JumpAction = actions.FindAction("Jump", false);
		InputManager.StopAction = actions.FindAction("Stop", false);
		InputManager.TwistLeftAction = actions.FindAction("Twist Left", false);
		InputManager.TwistRightAction = actions.FindAction("Twist Right", false);
		InputManager.DashLeftAction = actions.FindAction("Dash Left", false);
		InputManager.DashRightAction = actions.FindAction("Dash Right", false);
		InputManager.ExtendLeftAction = actions.FindAction("Extend Left", false);
		InputManager.ExtendRightAction = actions.FindAction("Extend Right", false);
		InputManager.LateralLeftAction = actions.FindAction("Lateral Left", false);
		InputManager.LateralRightAction = actions.FindAction("Lateral Right", false);
		InputManager.TalkAction = actions.FindAction("Talk", false);
		InputManager.AllChatAction = actions.FindAction("All Chat", false);
		InputManager.TeamChatAction = actions.FindAction("Team Chat", false);
		InputManager.PauseAction = actions.FindAction("Pause", false);
		InputManager.PositionSelectAction = actions.FindAction("Position Select", false);
		InputManager.ScoreboardAction = actions.FindAction("Scoreboard", false);
		InputManager.QuickChat1Action = actions.FindAction("Quick Chat 1", false);
		InputManager.QuickChat2Action = actions.FindAction("Quick Chat 2", false);
		InputManager.QuickChat3Action = actions.FindAction("Quick Chat 3", false);
		InputManager.QuickChat4Action = actions.FindAction("Quick Chat 4", false);
		InputManager.QuickChat5Action = actions.FindAction("Quick Chat 5", false);
		InputManager.Debug1Action = actions.FindAction("Debug 1", false);
		InputManager.Debug2Action = actions.FindAction("Debug 2", false);
		InputManager.Debug3Action = actions.FindAction("Debug 3", false);
		InputManager.Debug4Action = actions.FindAction("Debug 4", false);
		InputManager.PointAction = actions.FindAction("Point", false);
		InputManager.ClickAction = actions.FindAction("Click", false);
		InputManager.InputActions = new List<InputAction>
		{
			InputManager.MoveForwardAction,
			InputManager.MoveBackwardAction,
			InputManager.TurnLeftAction,
			InputManager.TurnRightAction,
			InputManager.StickAction,
			InputManager.BladeAngleUpAction,
			InputManager.BladeAngleDownAction,
			InputManager.SlideAction,
			InputManager.SprintAction,
			InputManager.TrackAction,
			InputManager.LookAction,
			InputManager.JumpAction,
			InputManager.StopAction,
			InputManager.TwistLeftAction,
			InputManager.TwistRightAction,
			InputManager.DashLeftAction,
			InputManager.DashRightAction,
			InputManager.ExtendLeftAction,
			InputManager.ExtendRightAction,
			InputManager.LateralLeftAction,
			InputManager.LateralRightAction,
			InputManager.TalkAction,
			InputManager.AllChatAction,
			InputManager.TeamChatAction,
			InputManager.PauseAction,
			InputManager.PositionSelectAction,
			InputManager.ScoreboardAction,
			InputManager.QuickChat1Action,
			InputManager.QuickChat2Action,
			InputManager.QuickChat3Action,
			InputManager.QuickChat4Action,
			InputManager.QuickChat5Action,
			InputManager.Debug1Action,
			InputManager.Debug2Action,
			InputManager.Debug3Action,
			InputManager.Debug4Action,
			InputManager.PointAction,
			InputManager.ClickAction
		};
		InputManager.RebindableInputActions = new List<InputAction>
		{
			InputManager.MoveForwardAction,
			InputManager.MoveBackwardAction,
			InputManager.TurnLeftAction,
			InputManager.TurnRightAction,
			InputManager.BladeAngleUpAction,
			InputManager.BladeAngleDownAction,
			InputManager.SlideAction,
			InputManager.SprintAction,
			InputManager.TrackAction,
			InputManager.LookAction,
			InputManager.JumpAction,
			InputManager.StopAction,
			InputManager.TwistLeftAction,
			InputManager.TwistRightAction,
			InputManager.DashLeftAction,
			InputManager.DashRightAction,
			InputManager.ExtendLeftAction,
			InputManager.ExtendRightAction,
			InputManager.LateralLeftAction,
			InputManager.LateralRightAction,
			InputManager.TalkAction,
			InputManager.AllChatAction,
			InputManager.TeamChatAction,
			InputManager.PositionSelectAction,
			InputManager.ScoreboardAction,
			InputManager.Debug1Action
		};
		InputManagerController.Initialize();
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x00025A36 File Offset: 0x00023C36
	public static void Dispose()
	{
		InputSystem.onDeviceChange -= InputManager.OnInputDeviceChange;
		InputManagerController.Dispose();
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x00025A4E File Offset: 0x00023C4E
	private static void OnInputDeviceChange(InputDevice device, InputDeviceChange change)
	{
		if (change == InputDeviceChange.Added || change == InputDeviceChange.Reconnected || change == InputDeviceChange.Enabled)
		{
			InputManager.TryDisableController(device);
		}
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x00025A64 File Offset: 0x00023C64
	private static void DisableControllers()
	{
		foreach (InputDevice device in InputSystem.devices)
		{
			InputManager.TryDisableController(device);
		}
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x00025AB8 File Offset: 0x00023CB8
	private static bool IsController(InputDevice device)
	{
		return device is Gamepad || device is Joystick;
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x00025ACD File Offset: 0x00023CCD
	private static void TryDisableController(InputDevice device)
	{
		if (!InputManager.IsController(device))
		{
			return;
		}
		if (device.enabled)
		{
			InputSystem.DisableDevice(device, false);
		}
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x00025AE8 File Offset: 0x00023CE8
	public static void LoadKeyBinds()
	{
		try
		{
			string @string = SaveManager.GetString("keyBinds", null);
			if (string.IsNullOrEmpty(@string))
			{
				throw new Exception("No saved key binds found");
			}
			Dictionary<string, KeyBind> dictionary = JsonSerializer.Deserialize<Dictionary<string, KeyBind>>(@string, null);
			List<string> list = new List<string>();
			foreach (InputAction inputAction in InputManager.RebindableInputActions)
			{
				if (!dictionary.ContainsKey(inputAction.name))
				{
					list.Add(inputAction.name);
				}
			}
			if (list.Count > 0)
			{
				throw new Exception("Missing keys in loaded key binds (" + string.Join(", ", list) + ")");
			}
			InputManager.KeyBinds.Clear();
			foreach (KeyValuePair<string, KeyBind> keyValuePair in dictionary)
			{
				string actionName = keyValuePair.Key;
				KeyBind value = keyValuePair.Value;
				InputAction inputAction2 = InputManager.RebindableInputActions.Find((InputAction action) => action.name == actionName);
				value.InputAction = inputAction2;
				if (value.InputAction == null)
				{
					InputManager.Logger.Warning("Cannot load key bind for " + actionName + " because it is not rebindable");
				}
				else
				{
					InputManager.KeyBinds.Add(actionName, value);
					InputManager.ApplyKeyBind(value);
				}
			}
			InputManager.Logger.Info(string.Format("Loaded {0} key binds: {1}", InputManager.KeyBinds.Count, @string));
			EventManager.TriggerEvent("Event_OnKeyBindsLoaded", new Dictionary<string, object>
			{
				{
					"keyBinds",
					InputManager.KeyBinds
				}
			});
		}
		catch (Exception ex)
		{
			InputManager.Logger.Warning("Failed to load key binds: " + ex.Message);
			InputManager.SaveKeyBinds();
			InputManager.LoadKeyBinds();
		}
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x00025D14 File Offset: 0x00023F14
	public static void ApplyKeyBinds()
	{
		foreach (KeyBind keyBind in InputManager.KeyBinds.Values)
		{
			InputManager.ApplyKeyBind(keyBind);
		}
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x00025D68 File Offset: 0x00023F68
	public static void ApplyKeyBind(KeyBind keyBind)
	{
		string name = keyBind.InputAction.name;
		InputManager.RebindAction(name, keyBind.ModifierPath, keyBind.Path);
		InputManager.SetActionInteractions(name, keyBind.Interactions);
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x00025D94 File Offset: 0x00023F94
	public static void MigrateChatOpenInteractionToPress()
	{
		if (SaveManager.GetBool("keyBindMigration_chatOpenInteractionReleaseToPress_v1", false))
		{
			return;
		}
		bool flag = false;
		foreach (string text in new string[]
		{
			"All Chat",
			"Team Chat"
		})
		{
			KeyBind keyBind;
			if (InputManager.KeyBinds.TryGetValue(text, out keyBind) && keyBind.Interactions == "Press(behavior=1)")
			{
				InputManager.SetActionInteractions(text, string.Empty);
				flag = true;
			}
		}
		if (flag)
		{
			InputManager.SaveKeyBinds();
		}
		SaveManager.SetBool("keyBindMigration_chatOpenInteractionReleaseToPress_v1", true);
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x00025E1C File Offset: 0x0002401C
	public static string GetKeyBindDisplayString(KeyBind keyBind)
	{
		if (keyBind.IsComposite)
		{
			string str = null;
			if (!string.IsNullOrEmpty(keyBind.ModifierPath))
			{
				str = str + keyBind.InputAction.GetBindingDisplayString(1, InputBinding.DisplayStringOptions.DontIncludeInteractions).ToUpper() + "+";
			}
			return str + keyBind.InputAction.GetBindingDisplayString(2, InputBinding.DisplayStringOptions.DontIncludeInteractions).ToUpper();
		}
		return keyBind.InputAction.GetBindingDisplayString(0, InputBinding.DisplayStringOptions.DontIncludeInteractions).ToUpper();
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x00025E8C File Offset: 0x0002408C
	public static void SaveKeyBinds()
	{
		try
		{
			foreach (InputAction inputAction in InputManager.RebindableInputActions)
			{
				if (!InputManager.KeyBinds.ContainsKey(inputAction.name))
				{
					KeyBind value = new KeyBind(inputAction);
					InputManager.KeyBinds.Add(inputAction.name, value);
				}
				else
				{
					InputManager.KeyBinds[inputAction.name].Update(inputAction);
				}
			}
			string text = JsonSerializer.Serialize<Dictionary<string, KeyBind>>(InputManager.KeyBinds, new JsonSerializerOptions
			{
				WriteIndented = true
			});
			SaveManager.SetString("keyBinds", text);
			InputManager.Logger.Info(string.Format("Saved {0} key binds: {1}", InputManager.KeyBinds.Count, text));
			EventManager.TriggerEvent("Event_OnKeyBindsSaved", new Dictionary<string, object>
			{
				{
					"keyBinds",
					InputManager.KeyBinds
				}
			});
		}
		catch (Exception ex)
		{
			InputManager.Logger.Error("Failed to save key binds: " + ex.Message);
		}
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x00025FAC File Offset: 0x000241AC
	public static void ResetToDefault()
	{
		InputManager.Logger.Info("Resetting key binds to default");
		foreach (InputAction action in InputManager.RebindableInputActions)
		{
			action.RemoveAllBindingOverrides();
		}
		InputManager.SaveKeyBinds();
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x00026010 File Offset: 0x00024210
	public static void RebindButtonInteractively(string actionName)
	{
		InputManager.<>c__DisplayClass56_0 CS$<>8__locals1 = new InputManager.<>c__DisplayClass56_0();
		CS$<>8__locals1.actionName = actionName;
		CS$<>8__locals1.inputAction = InputManager.RebindableInputActions.Find((InputAction action) => action.name == CS$<>8__locals1.actionName);
		if (CS$<>8__locals1.inputAction == null)
		{
			InputManager.Logger.Warning("Cannot rebind action " + CS$<>8__locals1.actionName + " because it is not rebindable");
			return;
		}
		CS$<>8__locals1.inputAction.Disable();
		InputActionRebindingExtensions.RebindingOperation rebindingOperation = CS$<>8__locals1.<RebindButtonInteractively>g__GenerateRebindingOperation|1();
		CS$<>8__locals1.rebindingOperation = CS$<>8__locals1.<RebindButtonInteractively>g__GenerateRebindingOperation|1();
		bool isComposite = CS$<>8__locals1.inputAction.bindings[0].isComposite;
		CS$<>8__locals1.interactions = CS$<>8__locals1.inputAction.bindings[0].effectiveInteractions;
		InputManager.Logger.Info("Rebinding " + CS$<>8__locals1.actionName);
		if (isComposite)
		{
			InputManager.Logger.Info("Rebinding " + CS$<>8__locals1.actionName + " as composite");
			rebindingOperation.WithTargetBinding(1).OnComplete(delegate(InputActionRebindingExtensions.RebindingOperation modifierOperation)
			{
				string modifierPath = modifierOperation.action.bindings[1].effectivePath;
				InputManager.Logger.Info("Rebound " + CS$<>8__locals1.actionName + " modifierPath to " + modifierPath);
				CS$<>8__locals1.rebindingOperation.WithControlsExcluding(modifierPath).WithTargetBinding(2).WithTimeout(0.5f).OnComplete(delegate(InputActionRebindingExtensions.RebindingOperation operation)
				{
					CS$<>8__locals1.inputAction.Enable();
					string effectivePath = operation.action.bindings[2].effectivePath;
					InputManager.RebindAction(CS$<>8__locals1.actionName, modifierPath, effectivePath);
					InputManager.SetActionInteractions(CS$<>8__locals1.actionName, CS$<>8__locals1.interactions);
					InputManager.Logger.Info(string.Concat(new string[]
					{
						"Rebound ",
						CS$<>8__locals1.actionName,
						" to ",
						modifierPath,
						" + ",
						effectivePath
					}));
					EventManager.TriggerEvent("Event_OnKeyBindRebindComplete", new Dictionary<string, object>
					{
						{
							"actionName",
							CS$<>8__locals1.actionName
						}
					});
					InputManager.SaveKeyBinds();
				}).OnCancel(delegate(InputActionRebindingExtensions.RebindingOperation operation)
				{
					CS$<>8__locals1.inputAction.Enable();
					InputManager.RebindAction(CS$<>8__locals1.actionName, null, modifierPath);
					InputManager.SetActionInteractions(CS$<>8__locals1.actionName, CS$<>8__locals1.interactions);
					InputManager.Logger.Info("Rebinding " + CS$<>8__locals1.actionName + " path was cancelled, using modifier path as path " + modifierPath);
					EventManager.TriggerEvent("Event_OnKeyBindRebindComplete", new Dictionary<string, object>
					{
						{
							"actionName",
							CS$<>8__locals1.actionName
						}
					});
					InputManager.SaveKeyBinds();
				}).Start();
			}).OnCancel(delegate(InputActionRebindingExtensions.RebindingOperation operation)
			{
				CS$<>8__locals1.inputAction.Enable();
				InputManager.Logger.Info("Rebinding " + CS$<>8__locals1.actionName + " was cancelled");
				EventManager.TriggerEvent("Event_OnKeyBindRebindCancel", new Dictionary<string, object>
				{
					{
						"actionName",
						CS$<>8__locals1.actionName
					}
				});
			});
		}
		else
		{
			rebindingOperation.OnComplete(delegate(InputActionRebindingExtensions.RebindingOperation operation)
			{
				CS$<>8__locals1.inputAction.Enable();
				string effectivePath = operation.action.bindings[0].effectivePath;
				InputManager.RebindAction(CS$<>8__locals1.actionName, null, effectivePath);
				InputManager.SetActionInteractions(CS$<>8__locals1.actionName, CS$<>8__locals1.interactions);
				InputManager.Logger.Info("Rebound " + CS$<>8__locals1.actionName + " to " + effectivePath);
				EventManager.TriggerEvent("Event_OnKeyBindRebindComplete", new Dictionary<string, object>
				{
					{
						"actionName",
						CS$<>8__locals1.actionName
					}
				});
				InputManager.SaveKeyBinds();
			}).OnCancel(delegate(InputActionRebindingExtensions.RebindingOperation operation)
			{
				CS$<>8__locals1.inputAction.Enable();
				InputManager.Logger.Info("Rebinding " + CS$<>8__locals1.actionName + " was cancelled");
				EventManager.TriggerEvent("Event_OnKeyBindRebindCancel", new Dictionary<string, object>
				{
					{
						"actionName",
						CS$<>8__locals1.actionName
					}
				});
			});
		}
		rebindingOperation.Start();
		EventManager.TriggerEvent("Event_OnKeyBindRebindStart", new Dictionary<string, object>
		{
			{
				"actionName",
				CS$<>8__locals1.actionName
			},
			{
				"isComposite",
				isComposite
			}
		});
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x00026194 File Offset: 0x00024394
	public static void RebindAction(string actionName, string modifierPath = null, string path = null)
	{
		InputAction inputAction = InputManager.RebindableInputActions.Find((InputAction action) => action.name == actionName);
		if (inputAction == null)
		{
			InputManager.Logger.Warning("Cannot rebind action " + actionName + " because it is not rebindable");
			return;
		}
		if (inputAction.bindings[0].isComposite)
		{
			inputAction.ApplyBindingOverride(1, new InputBinding
			{
				overridePath = modifierPath
			});
			inputAction.ApplyBindingOverride(2, new InputBinding
			{
				overridePath = path
			});
			return;
		}
		inputAction.ApplyBindingOverride(0, new InputBinding
		{
			overridePath = path
		});
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x0002624C File Offset: 0x0002444C
	public static void SetActionInteractions(string actionName, string interactions)
	{
		InputAction inputAction = InputManager.RebindableInputActions.Find((InputAction action) => action.name == actionName);
		if (inputAction == null)
		{
			InputManager.Logger.Warning("Cannot set interactions for action " + actionName + " because it is not rebindable");
			return;
		}
		InputBinding inputBinding = inputAction.bindings[0];
		inputAction.ApplyBindingOverride(0, new InputBinding
		{
			overridePath = inputBinding.overridePath,
			overrideProcessors = inputBinding.overrideProcessors,
			overrideInteractions = interactions
		});
	}

	// Token: 0x0400046D RID: 1133
	private static readonly Logger Logger = new Logger("InputManager");

	// Token: 0x0400046E RID: 1134
	public static InputAction MoveForwardAction;

	// Token: 0x0400046F RID: 1135
	public static InputAction MoveBackwardAction;

	// Token: 0x04000470 RID: 1136
	public static InputAction TurnLeftAction;

	// Token: 0x04000471 RID: 1137
	public static InputAction TurnRightAction;

	// Token: 0x04000472 RID: 1138
	public static InputAction StickAction;

	// Token: 0x04000473 RID: 1139
	public static InputAction BladeAngleUpAction;

	// Token: 0x04000474 RID: 1140
	public static InputAction BladeAngleDownAction;

	// Token: 0x04000475 RID: 1141
	public static InputAction SlideAction;

	// Token: 0x04000476 RID: 1142
	public static InputAction SprintAction;

	// Token: 0x04000477 RID: 1143
	public static InputAction TrackAction;

	// Token: 0x04000478 RID: 1144
	public static InputAction LookAction;

	// Token: 0x04000479 RID: 1145
	public static InputAction JumpAction;

	// Token: 0x0400047A RID: 1146
	public static InputAction StopAction;

	// Token: 0x0400047B RID: 1147
	public static InputAction TwistLeftAction;

	// Token: 0x0400047C RID: 1148
	public static InputAction TwistRightAction;

	// Token: 0x0400047D RID: 1149
	public static InputAction DashLeftAction;

	// Token: 0x0400047E RID: 1150
	public static InputAction DashRightAction;

	// Token: 0x0400047F RID: 1151
	public static InputAction ExtendLeftAction;

	// Token: 0x04000480 RID: 1152
	public static InputAction ExtendRightAction;

	// Token: 0x04000481 RID: 1153
	public static InputAction LateralLeftAction;

	// Token: 0x04000482 RID: 1154
	public static InputAction LateralRightAction;

	// Token: 0x04000483 RID: 1155
	public static InputAction TalkAction;

	// Token: 0x04000484 RID: 1156
	public static InputAction AllChatAction;

	// Token: 0x04000485 RID: 1157
	public static InputAction TeamChatAction;

	// Token: 0x04000486 RID: 1158
	public static InputAction PauseAction;

	// Token: 0x04000487 RID: 1159
	public static InputAction PositionSelectAction;

	// Token: 0x04000488 RID: 1160
	public static InputAction ScoreboardAction;

	// Token: 0x04000489 RID: 1161
	public static InputAction QuickChat1Action;

	// Token: 0x0400048A RID: 1162
	public static InputAction QuickChat2Action;

	// Token: 0x0400048B RID: 1163
	public static InputAction QuickChat3Action;

	// Token: 0x0400048C RID: 1164
	public static InputAction QuickChat4Action;

	// Token: 0x0400048D RID: 1165
	public static InputAction QuickChat5Action;

	// Token: 0x0400048E RID: 1166
	public static InputAction Debug1Action;

	// Token: 0x0400048F RID: 1167
	public static InputAction Debug2Action;

	// Token: 0x04000490 RID: 1168
	public static InputAction Debug3Action;

	// Token: 0x04000491 RID: 1169
	public static InputAction Debug4Action;

	// Token: 0x04000492 RID: 1170
	public static InputAction PointAction;

	// Token: 0x04000493 RID: 1171
	public static InputAction ClickAction;

	// Token: 0x04000494 RID: 1172
	public static Dictionary<string, KeyBind> KeyBinds = new Dictionary<string, KeyBind>();

	// Token: 0x04000495 RID: 1173
	public static List<InputAction> InputActions = new List<InputAction>();

	// Token: 0x04000496 RID: 1174
	public static List<InputAction> RebindableInputActions = new List<InputAction>();

	// Token: 0x04000497 RID: 1175
	private const string ChatOpenInteractionReleaseToPressMigrationKey = "keyBindMigration_chatOpenInteractionReleaseToPress_v1";
}
