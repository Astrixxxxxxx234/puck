using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// Token: 0x02000247 RID: 583
public static class Utils
{
	// Token: 0x06001252 RID: 4690 RVA: 0x000552B4 File Offset: 0x000534B4
	public static string GetReadableTypeName(Type type)
	{
		return type.Name.Split('`', StringSplitOptions.None)[0];
	}

	// Token: 0x06001253 RID: 4691 RVA: 0x000552C6 File Offset: 0x000534C6
	public static float WrapEulerAngle(float angle)
	{
		angle %= 360f;
		if (angle > 180f)
		{
			angle -= 360f;
		}
		if (angle < -180f)
		{
			angle += 360f;
		}
		return angle;
	}

	// Token: 0x06001254 RID: 4692 RVA: 0x000552F4 File Offset: 0x000534F4
	public static Vector3 WrapEulerAngles(Vector3 eulerAngles)
	{
		eulerAngles.x %= 360f;
		if (eulerAngles.x > 180f)
		{
			eulerAngles.x -= 360f;
		}
		if (eulerAngles.x < -180f)
		{
			eulerAngles.x += 360f;
		}
		eulerAngles.y %= 360f;
		if (eulerAngles.y > 180f)
		{
			eulerAngles.y -= 360f;
		}
		if (eulerAngles.y < -180f)
		{
			eulerAngles.y += 360f;
		}
		eulerAngles.z %= 360f;
		if (eulerAngles.z > 180f)
		{
			eulerAngles.z -= 360f;
		}
		if (eulerAngles.z < -180f)
		{
			eulerAngles.z += 360f;
		}
		return eulerAngles;
	}

	// Token: 0x06001255 RID: 4693 RVA: 0x000553E0 File Offset: 0x000535E0
	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		Vector3 vector = point - pivot;
		vector = Quaternion.Euler(angles) * vector;
		point = vector + pivot;
		return point;
	}

	// Token: 0x06001256 RID: 4694 RVA: 0x0005540C File Offset: 0x0005360C
	public static Vector3 Vector2Clamp(Vector2 value, Vector2 min, Vector2 max)
	{
		return new Vector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x00055444 File Offset: 0x00053644
	public static Vector3 Vector3Clamp(Vector3 value, Vector3 min, Vector3 max)
	{
		return new Vector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));
	}

	// Token: 0x06001258 RID: 4696 RVA: 0x0005549B File Offset: 0x0005369B
	public static Vector3 Vector3Abs(Vector3 value)
	{
		return new Vector3(Mathf.Abs(value.x), Mathf.Abs(value.y), Mathf.Abs(value.z));
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x000554C3 File Offset: 0x000536C3
	public static Vector3 Vector3Slerp3(Vector3 a, Vector3 b, Vector3 c, float t)
	{
		if (t <= 0f)
		{
			return Vector3.Slerp(a, b, t + 1f);
		}
		return Vector3.Slerp(b, c, t);
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x000554E4 File Offset: 0x000536E4
	public static float Map(float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x000554F4 File Offset: 0x000536F4
	public static Quaternion GetLocalLookRotation(Transform transform, Vector3 target)
	{
		if (transform.parent == null)
		{
			return Quaternion.LookRotation(target - transform.position);
		}
		Quaternion rhs = Quaternion.LookRotation(target - transform.position);
		return Quaternion.Inverse(transform.parent.rotation) * rhs;
	}

	// Token: 0x0600125C RID: 4700 RVA: 0x00055549 File Offset: 0x00053749
	public static float GameUnitsToMetric(float value)
	{
		return value * 3.6f;
	}

	// Token: 0x0600125D RID: 4701 RVA: 0x00055552 File Offset: 0x00053752
	public static float GameUnitsToImperial(float value)
	{
		return value * 2.2369363f;
	}

	// Token: 0x0600125E RID: 4702 RVA: 0x0005555C File Offset: 0x0005375C
	public static float GetCollisionForce(Collision collision)
	{
		if (collision == null)
		{
			return 0f;
		}
		float result = 0f;
		if (collision.contactCount > 0)
		{
			result = Vector3.Dot(collision.GetContact(0).normal, collision.relativeVelocity.normalized) * collision.relativeVelocity.magnitude;
		}
		return result;
	}

	// Token: 0x0600125F RID: 4703 RVA: 0x000555B4 File Offset: 0x000537B4
	public static void SetRigidbodyCollisionDetectionMode(Rigidbody rigidbody, CollisionDetectionMode mode)
	{
		if (rigidbody == null)
		{
			return;
		}
		if (rigidbody.collisionDetectionMode == mode)
		{
			return;
		}
		bool isKinematic = rigidbody.isKinematic;
		Vector3 linearVelocity = rigidbody.linearVelocity;
		Vector3 angularVelocity = rigidbody.angularVelocity;
		rigidbody.collisionDetectionMode = mode;
		rigidbody.isKinematic = true;
		rigidbody.isKinematic = false;
		rigidbody.isKinematic = isKinematic;
		rigidbody.linearVelocity = linearVelocity;
		rigidbody.angularVelocity = angularVelocity;
	}

	// Token: 0x06001260 RID: 4704 RVA: 0x00055614 File Offset: 0x00053814
	public static List<string> GetTeamNames()
	{
		return new List<string>
		{
			"BLUE",
			"RED"
		};
	}

	// Token: 0x06001261 RID: 4705 RVA: 0x00055634 File Offset: 0x00053834
	public static PlayerTeam GetTeamFromName(string name)
	{
		PlayerTeam result;
		if (!(name == "BLUE"))
		{
			if (!(name == "RED"))
			{
				result = PlayerTeam.Blue;
			}
			else
			{
				result = PlayerTeam.Red;
			}
		}
		else
		{
			result = PlayerTeam.Blue;
		}
		return result;
	}

	// Token: 0x06001262 RID: 4706 RVA: 0x00055668 File Offset: 0x00053868
	public static string GetNameFromTeam(PlayerTeam team)
	{
		string result;
		if (team != PlayerTeam.Blue)
		{
			if (team != PlayerTeam.Red)
			{
				result = "UNKNOWN";
			}
			else
			{
				result = "RED";
			}
		}
		else
		{
			result = "BLUE";
		}
		return result;
	}

	// Token: 0x06001263 RID: 4707 RVA: 0x00055698 File Offset: 0x00053898
	public static Color GetTeamColor(PlayerTeam team)
	{
		string htmlString;
		if (team != PlayerTeam.Blue)
		{
			if (team != PlayerTeam.Red)
			{
				htmlString = "#404040";
			}
			else
			{
				htmlString = "#d13333";
			}
		}
		else
		{
			htmlString = "#3b82f6";
		}
		Color result;
		ColorUtility.TryParseHtmlString(htmlString, out result);
		return result;
	}

	// Token: 0x06001264 RID: 4708 RVA: 0x000556CF File Offset: 0x000538CF
	public static List<string> GetRoleNames()
	{
		return new List<string>
		{
			"SKATER",
			"GOALIE"
		};
	}

	// Token: 0x06001265 RID: 4709 RVA: 0x000556EC File Offset: 0x000538EC
	public static PlayerRole GetRoleFromName(string name)
	{
		PlayerRole result;
		if (!(name == "SKATER"))
		{
			if (!(name == "GOALIE"))
			{
				result = PlayerRole.Attacker;
			}
			else
			{
				result = PlayerRole.Goalie;
			}
		}
		else
		{
			result = PlayerRole.Attacker;
		}
		return result;
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x00055720 File Offset: 0x00053920
	public static string GetNameFromRole(PlayerRole role)
	{
		string result;
		if (role != PlayerRole.Attacker)
		{
			if (role != PlayerRole.Goalie)
			{
				result = "UNKNOWN";
			}
			else
			{
				result = "GOALIE";
			}
		}
		else
		{
			result = "SKATER";
		}
		return result;
	}

	// Token: 0x06001267 RID: 4711 RVA: 0x0005574E File Offset: 0x0005394E
	public static List<string> GetHandednessNames()
	{
		return new List<string>
		{
			"LEFT",
			"RIGHT"
		};
	}

	// Token: 0x06001268 RID: 4712 RVA: 0x0005576C File Offset: 0x0005396C
	public static PlayerHandedness GetHandednessFromName(string name)
	{
		PlayerHandedness result;
		if (!(name == "LEFT"))
		{
			if (!(name == "RIGHT"))
			{
				result = PlayerHandedness.Right;
			}
			else
			{
				result = PlayerHandedness.Right;
			}
		}
		else
		{
			result = PlayerHandedness.Left;
		}
		return result;
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x000557A0 File Offset: 0x000539A0
	public static string GetNameFromHandedness(PlayerHandedness handedness)
	{
		string result;
		if (handedness != PlayerHandedness.Left)
		{
			if (handedness != PlayerHandedness.Right)
			{
				result = "UNKNOWN";
			}
			else
			{
				result = "RIGHT";
			}
		}
		else
		{
			result = "LEFT";
		}
		return result;
	}

	// Token: 0x0600126A RID: 4714 RVA: 0x000557CE File Offset: 0x000539CE
	public static List<string> GetUnitsNames()
	{
		return new List<string>
		{
			"METRIC",
			"IMPERIAL"
		};
	}

	// Token: 0x0600126B RID: 4715 RVA: 0x000557EC File Offset: 0x000539EC
	public static Units GetUnitsFromName(string name)
	{
		Units result;
		if (!(name == "METRIC"))
		{
			if (!(name == "IMPERIAL"))
			{
				result = Units.Metric;
			}
			else
			{
				result = Units.Imperial;
			}
		}
		else
		{
			result = Units.Metric;
		}
		return result;
	}

	// Token: 0x0600126C RID: 4716 RVA: 0x00055820 File Offset: 0x00053A20
	public static string GetNameFromUnits(Units units)
	{
		string result;
		if (units != Units.Metric)
		{
			if (units != Units.Imperial)
			{
				result = "UNKNOWN";
			}
			else
			{
				result = "IMPERIAL";
			}
		}
		else
		{
			result = "METRIC";
		}
		return result;
	}

	// Token: 0x0600126D RID: 4717 RVA: 0x00055850 File Offset: 0x00053A50
	public static NetworkBuffering GetNetworkBufferingFromName(string name)
	{
		NetworkBuffering result;
		if (!(name == "PREDICTIVE"))
		{
			if (!(name == "RESPONSIVE"))
			{
				if (!(name == "BALANCED"))
				{
					if (!(name == "SMOOTH"))
					{
						result = NetworkBuffering.Responsive;
					}
					else
					{
						result = NetworkBuffering.Smooth;
					}
				}
				else
				{
					result = NetworkBuffering.Balanced;
				}
			}
			else
			{
				result = NetworkBuffering.Responsive;
			}
		}
		else
		{
			result = NetworkBuffering.Predictive;
		}
		return result;
	}

	// Token: 0x0600126E RID: 4718 RVA: 0x000558A8 File Offset: 0x00053AA8
	public static string GetNameFromNetworkBuffering(NetworkBuffering networkBuffering)
	{
		string result;
		switch (networkBuffering)
		{
		case NetworkBuffering.Predictive:
			result = "PREDICTIVE";
			break;
		case NetworkBuffering.Responsive:
			result = "RESPONSIVE";
			break;
		case NetworkBuffering.Balanced:
			result = "BALANCED";
			break;
		case NetworkBuffering.Smooth:
			result = "SMOOTH";
			break;
		default:
			result = "UNKNOWN";
			break;
		}
		return result;
	}

	// Token: 0x0600126F RID: 4719 RVA: 0x000558F4 File Offset: 0x00053AF4
	public static float GetTargetTimelinePositionFromNetworkBuffering(NetworkBuffering networkBuffering)
	{
		float result;
		switch (networkBuffering)
		{
		case NetworkBuffering.Predictive:
			result = 0.5f;
			break;
		case NetworkBuffering.Responsive:
			result = 0f;
			break;
		case NetworkBuffering.Balanced:
			result = -0.5f;
			break;
		case NetworkBuffering.Smooth:
			result = -1f;
			break;
		default:
			result = 0f;
			break;
		}
		return result;
	}

	// Token: 0x06001270 RID: 4720 RVA: 0x00055940 File Offset: 0x00053B40
	public static List<string> GetFullScreenModeNames()
	{
		return new List<string>
		{
			"FULLSCREEN",
			"BORDERLESS",
			"WINDOWED"
		};
	}

	// Token: 0x06001271 RID: 4721 RVA: 0x00055968 File Offset: 0x00053B68
	public static FullScreenMode GetFullScreenModeFromName(string name)
	{
		FullScreenMode result;
		if (!(name == "FULLSCREEN"))
		{
			if (!(name == "BORDERLESS"))
			{
				if (!(name == "WINDOWED"))
				{
					result = FullScreenMode.FullScreenWindow;
				}
				else
				{
					result = FullScreenMode.Windowed;
				}
			}
			else
			{
				result = FullScreenMode.FullScreenWindow;
			}
		}
		else
		{
			result = FullScreenMode.ExclusiveFullScreen;
		}
		return result;
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x000559B0 File Offset: 0x00053BB0
	public static string GetNameFromFullScreenMode(FullScreenMode mode)
	{
		switch (mode)
		{
		case FullScreenMode.ExclusiveFullScreen:
			return "FULLSCREEN";
		case FullScreenMode.FullScreenWindow:
			return "BORDERLESS";
		case FullScreenMode.Windowed:
			return "WINDOWED";
		}
		return "UNKNOWN";
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x000559F4 File Offset: 0x00053BF4
	public static List<DisplayInfo> GetDisplayLayout()
	{
		List<DisplayInfo> list = new List<DisplayInfo>();
		Screen.GetDisplayLayout(list);
		return list;
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x00055A01 File Offset: 0x00053C01
	public static List<string> GetDisplayNames()
	{
		return Utils.GetDisplayLayout().Select((DisplayInfo displayInfo, int index) => Utils.FormatDisplay(index, displayInfo)).ToList<string>();
	}

	// Token: 0x06001275 RID: 4725 RVA: 0x00055A34 File Offset: 0x00053C34
	public static string GetDisplayNameFromIndex(int index)
	{
		List<DisplayInfo> displayLayout = Utils.GetDisplayLayout();
		if (index < 0 || index > displayLayout.Count - 1)
		{
			return "UNKNOWN";
		}
		return Utils.FormatDisplay(index, displayLayout[index]);
	}

	// Token: 0x06001276 RID: 4726 RVA: 0x00055A6C File Offset: 0x00053C6C
	public static int GetDisplayIndexFromName(string name)
	{
		List<DisplayInfo> displayLayout = Utils.GetDisplayLayout();
		for (int i = 0; i < displayLayout.Count; i++)
		{
			if (Utils.FormatDisplay(i, displayLayout[i]) == name)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001277 RID: 4727 RVA: 0x00055AA8 File Offset: 0x00053CA8
	public static string FormatDisplay(int index, DisplayInfo displayInfo)
	{
		return string.Format("{0} ({1})", displayInfo.name, index);
	}

	// Token: 0x06001278 RID: 4728 RVA: 0x00055AC0 File Offset: 0x00053CC0
	public static List<Resolution> GetResolutions()
	{
		return Screen.resolutions.ToList<Resolution>();
	}

	// Token: 0x06001279 RID: 4729 RVA: 0x00055ACC File Offset: 0x00053CCC
	public static List<string> GetResolutionNames()
	{
		return (from resolution in Utils.GetResolutions()
		select Utils.FormatResolution(resolution)).ToList<string>();
	}

	// Token: 0x0600127A RID: 4730 RVA: 0x00055AFC File Offset: 0x00053CFC
	public static string GetResolutionNameFromIndex(int index)
	{
		List<Resolution> resolutions = Utils.GetResolutions();
		if (index < 0 || index > resolutions.Count - 1)
		{
			return "UNKNOWN";
		}
		return Utils.FormatResolution(resolutions[index]);
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x00055B30 File Offset: 0x00053D30
	public static int GetResolutionIndexFromName(string name)
	{
		List<Resolution> resolutions = Utils.GetResolutions();
		for (int i = 0; i < resolutions.Count; i++)
		{
			if (Utils.FormatResolution(resolutions[i]) == name)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x00055B6C File Offset: 0x00053D6C
	public static string FormatResolution(Resolution resolution)
	{
		return string.Format("{0}x{1} @ {2}Hz", resolution.width, resolution.height, resolution.refreshRateRatio.value.ToString("F0"));
	}

	// Token: 0x0600127D RID: 4733 RVA: 0x00055BB7 File Offset: 0x00053DB7
	public static List<string> GetApplicationQualityNames()
	{
		return new List<string>
		{
			"LOW",
			"MEDIUM",
			"HIGH",
			"ULTRA"
		};
	}

	// Token: 0x0600127E RID: 4734 RVA: 0x00055BEC File Offset: 0x00053DEC
	public static ApplicationQuality GetApplicationQualityFromName(string name)
	{
		ApplicationQuality result;
		if (!(name == "LOW"))
		{
			if (!(name == "MEDIUM"))
			{
				if (!(name == "HIGH"))
				{
					if (!(name == "ULTRA"))
					{
						result = ApplicationQuality.High;
					}
					else
					{
						result = ApplicationQuality.Ultra;
					}
				}
				else
				{
					result = ApplicationQuality.High;
				}
			}
			else
			{
				result = ApplicationQuality.Medium;
			}
		}
		else
		{
			result = ApplicationQuality.Low;
		}
		return result;
	}

	// Token: 0x0600127F RID: 4735 RVA: 0x00055C44 File Offset: 0x00053E44
	public static string GetNameFromApplicationQuality(ApplicationQuality quality)
	{
		string result;
		switch (quality)
		{
		case ApplicationQuality.Low:
			result = "LOW";
			break;
		case ApplicationQuality.Medium:
			result = "MEDIUM";
			break;
		case ApplicationQuality.High:
			result = "HIGH";
			break;
		case ApplicationQuality.Ultra:
			result = "ULTRA";
			break;
		default:
			result = "UNKNOWN";
			break;
		}
		return result;
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x00055BB7 File Offset: 0x00053DB7
	public static List<string> GetShadowQualityNames()
	{
		return new List<string>
		{
			"LOW",
			"MEDIUM",
			"HIGH",
			"ULTRA"
		};
	}

	// Token: 0x06001281 RID: 4737 RVA: 0x00055C90 File Offset: 0x00053E90
	public static global::ShadowQuality GetShadowQualityFromName(string name)
	{
		global::ShadowQuality result;
		if (!(name == "LOW"))
		{
			if (!(name == "MEDIUM"))
			{
				if (!(name == "HIGH"))
				{
					if (!(name == "ULTRA"))
					{
						result = global::ShadowQuality.High;
					}
					else
					{
						result = global::ShadowQuality.Ultra;
					}
				}
				else
				{
					result = global::ShadowQuality.High;
				}
			}
			else
			{
				result = global::ShadowQuality.Medium;
			}
		}
		else
		{
			result = global::ShadowQuality.Low;
		}
		return result;
	}

	// Token: 0x06001282 RID: 4738 RVA: 0x00055CE8 File Offset: 0x00053EE8
	public static string GetNameFromShadowQuality(global::ShadowQuality shadowQuality)
	{
		string result;
		switch (shadowQuality)
		{
		case global::ShadowQuality.Low:
			result = "LOW";
			break;
		case global::ShadowQuality.Medium:
			result = "MEDIUM";
			break;
		case global::ShadowQuality.High:
			result = "HIGH";
			break;
		case global::ShadowQuality.Ultra:
			result = "ULTRA";
			break;
		default:
			result = "UNKNOWN";
			break;
		}
		return result;
	}

	// Token: 0x06001283 RID: 4739 RVA: 0x00055D34 File Offset: 0x00053F34
	public static KeyBindInteraction GetKeyBindInteractionFromInteraction(string interaction, KeyBindInteractionType interactionType)
	{
		if (interaction == "Press(behavior=1)")
		{
			return KeyBindInteraction.Release;
		}
		if (interaction == "DoublePress")
		{
			return KeyBindInteraction.DoublePress;
		}
		if (interaction == "Hold")
		{
			return KeyBindInteraction.Hold;
		}
		if (interaction == "Toggle")
		{
			return KeyBindInteraction.Toggle;
		}
		if (interactionType != KeyBindInteractionType.Press)
		{
			return KeyBindInteraction.Continuous;
		}
		return KeyBindInteraction.Press;
	}

	// Token: 0x06001284 RID: 4740 RVA: 0x00055D85 File Offset: 0x00053F85
	public static string GetInteractionFromKeyBindInteraction(KeyBindInteraction keyBindInteraction)
	{
		switch (keyBindInteraction)
		{
		case KeyBindInteraction.Release:
			return "Press(behavior=1)";
		case KeyBindInteraction.DoublePress:
			return "DoublePress";
		case KeyBindInteraction.Hold:
			return "Hold";
		case KeyBindInteraction.Toggle:
			return "Toggle";
		}
		return string.Empty;
	}

	// Token: 0x06001285 RID: 4741 RVA: 0x00055DC4 File Offset: 0x00053FC4
	public static string GetHumanizedGamePhase(GamePhase phase, int period, bool isOvertime, int regulationPeriods = 3)
	{
		string result;
		switch (phase)
		{
		case GamePhase.None:
			result = "";
			break;
		case GamePhase.Warmup:
			result = "WARMUP";
			break;
		case GamePhase.PreGame:
			result = "PRE-GAME";
			break;
		case GamePhase.FaceOff:
			result = "FACE-OFF";
			break;
		case GamePhase.Play:
			result = (isOvertime ? Utils.GetHumanizedOvertime(period, regulationPeriods) : string.Format("PERIOD {0}", period));
			break;
		case GamePhase.BlueScore:
			result = "SCORE!";
			break;
		case GamePhase.RedScore:
			result = "SCORE!";
			break;
		case GamePhase.Replay:
			result = "REPLAY";
			break;
		case GamePhase.Intermission:
			result = "INTERMISSION";
			break;
		case GamePhase.GameOver:
			result = "GAME OVER";
			break;
		case GamePhase.PostGame:
			result = "POST-GAME";
			break;
		default:
			result = phase.ToString();
			break;
		}
		return result;
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x00055E84 File Offset: 0x00054084
	private static string GetHumanizedOvertime(int period, int regulationPeriods)
	{
		int num = period - Mathf.Max(regulationPeriods, 1);
		if (num < 2)
		{
			return "OVERTIME";
		}
		return string.Format("OVERTIME {0}", num);
	}

	// Token: 0x06001287 RID: 4743 RVA: 0x00055EB8 File Offset: 0x000540B8
	public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(sourceDir);
		if (!directoryInfo.Exists)
		{
			throw new DirectoryNotFoundException("Source directory not found: " + directoryInfo.FullName);
		}
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		Directory.CreateDirectory(destinationDir);
		foreach (FileInfo fileInfo in directoryInfo.GetFiles())
		{
			string destFileName = Path.Combine(destinationDir, fileInfo.Name);
			fileInfo.CopyTo(destFileName, true);
		}
		if (recursive)
		{
			foreach (DirectoryInfo directoryInfo2 in directories)
			{
				string destinationDir2 = Path.Combine(destinationDir, directoryInfo2.Name);
				Utils.CopyDirectory(directoryInfo2.FullName, destinationDir2, true);
			}
		}
	}

	// Token: 0x06001288 RID: 4744 RVA: 0x00055F68 File Offset: 0x00054168
	public static string GetConnectionRejectionMessage(ConnectionRejectionCode code, string message = null)
	{
		if (!string.IsNullOrEmpty(message))
		{
			return message;
		}
		switch (code)
		{
		default:
			return "Server unreachable";
		case ConnectionRejectionCode.ServerFull:
			return "Server full";
		case ConnectionRejectionCode.TimedOut:
			return "Timed out";
		case ConnectionRejectionCode.Banned:
			return "Banned";
		case ConnectionRejectionCode.NotWhitelisted:
			return "Not whitelisted";
		case ConnectionRejectionCode.MissingPassword:
			return "Missing password";
		case ConnectionRejectionCode.InvalidPassword:
			return "Invalid password";
		case ConnectionRejectionCode.MissingMods:
			return "Missing mods";
		case ConnectionRejectionCode.ServerStarting:
			return "Server is still starting...";
		}
	}

	// Token: 0x06001289 RID: 4745 RVA: 0x00055FDE File Offset: 0x000541DE
	public static string GetDisconnectionMessage(DisconnectionCode code, string message = null)
	{
		if (!string.IsNullOrEmpty(message))
		{
			return message;
		}
		switch (code)
		{
		default:
			return "Connection lost";
		case DisconnectionCode.Disconnected:
			return "Disconnected";
		case DisconnectionCode.Kicked:
			return "Kicked";
		case DisconnectionCode.Banned:
			return "Banned";
		}
	}

	// Token: 0x0600128A RID: 4746 RVA: 0x00056018 File Offset: 0x00054218
	public static string GetCommandLineArgument(string name, string[] args = null)
	{
		if (args == null)
		{
			args = Environment.GetCommandLineArgs();
		}
		int i = 0;
		while (i < args.Length)
		{
			if (args[i] == (name ?? ""))
			{
				if (i + 1 >= args.Length)
				{
					return null;
				}
				return args[i + 1];
			}
			else
			{
				i++;
			}
		}
		return null;
	}

	// Token: 0x0600128B RID: 4747 RVA: 0x00056064 File Offset: 0x00054264
	public static void PrintUPnPLogs()
	{
		if (uPnPHelper.DebugMode)
		{
			List<string> debugMessageArray = uPnPHelper.GetDebugMessageArray();
			foreach (string text in debugMessageArray.ToList<string>())
			{
				Utils.uPnPLogger.Info(text ?? "");
			}
			debugMessageArray.Clear();
		}
		List<string> errorMessageArray = uPnPHelper.GetErrorMessageArray();
		foreach (string text2 in errorMessageArray.ToList<string>())
		{
			Utils.uPnPLogger.Error(text2 ?? "");
		}
		errorMessageArray.Clear();
	}

	// Token: 0x0600128C RID: 4748 RVA: 0x00056134 File Offset: 0x00054334
	public static double GetTimestamp()
	{
		return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
	}

	// Token: 0x0600128D RID: 4749 RVA: 0x00056164 File Offset: 0x00054364
	public static void WhenAllActions(Action callback, params Action<Action>[] subscriptions)
	{
		bool[] state = new bool[subscriptions.Length];
		bool callbackInvoked = false;
		for (int i = 0; i < subscriptions.Length; i++)
		{
			int index = i;
			subscriptions[index](delegate
			{
				if (state[index])
				{
					return;
				}
				state[index] = true;
				if (!callbackInvoked)
				{
					if (state.All((bool s) => s))
					{
						callbackInvoked = true;
						Action callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2();
					}
				}
			});
		}
	}

	// Token: 0x0600128E RID: 4750 RVA: 0x000561D0 File Offset: 0x000543D0
	public static bool IsValidSteamId64(string value)
	{
		ulong num;
		return ulong.TryParse(value, out num) && num >= 76561197960265728UL;
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x000561F8 File Offset: 0x000543F8
	public static int GetVoteMajority(int playerCount)
	{
		if (playerCount == 1)
		{
			return 1;
		}
		if (playerCount != 2)
		{
			return Mathf.CeilToInt((float)(playerCount - 1) * 0.75f);
		}
		return 2;
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x00056217 File Offset: 0x00054417
	public static bool IsGameInProgress(GamePhase phase)
	{
		return phase == GamePhase.FaceOff || phase == GamePhase.Play || phase == GamePhase.BlueScore || phase == GamePhase.RedScore || phase == GamePhase.Replay || phase == GamePhase.Intermission;
	}

	// Token: 0x06001291 RID: 4753 RVA: 0x00056234 File Offset: 0x00054434
	public static PlayerTeam? GetOpposingTeam(PlayerTeam team)
	{
		PlayerTeam? result;
		if (team != PlayerTeam.Blue)
		{
			if (team != PlayerTeam.Red)
			{
				result = null;
			}
			else
			{
				result = new PlayerTeam?(PlayerTeam.Blue);
			}
		}
		else
		{
			result = new PlayerTeam?(PlayerTeam.Red);
		}
		return result;
	}

	// Token: 0x06001292 RID: 4754 RVA: 0x00056268 File Offset: 0x00054468
	public static void SwapMaterial(MeshRenderer meshRenderer, Material material, int index = 0)
	{
		Material[] materials = meshRenderer.materials;
		if (index >= materials.Length)
		{
			return;
		}
		Object.Destroy(materials[index]);
		materials[index] = new Material(material);
		meshRenderer.materials = materials;
	}

	// Token: 0x04000BD4 RID: 3028
	private static readonly global::Logger uPnPLogger = new global::Logger("uPnPHelper");
}
