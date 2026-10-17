using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000243 RID: 579
public static class TerminalUtils
{
	// Token: 0x06001234 RID: 4660 RVA: 0x00054C34 File Offset: 0x00052E34
	public static void TryEnableUtf8Output()
	{
		if (!TerminalUtils.IsWindowsPlatform())
		{
			return;
		}
		try
		{
			uint consoleOutputCP = TerminalUtils.GetConsoleOutputCP();
			if (consoleOutputCP != 65001U)
			{
				if (TerminalUtils.SetConsoleOutputCP(65001U))
				{
					TerminalUtils.Log.Info(string.Format("Set console output code page to UTF-8 (was {0})", consoleOutputCP));
				}
				else
				{
					TerminalUtils.Log.Warning(string.Format("SetConsoleOutputCP(UTF-8) failed (code page stays {0})", consoleOutputCP));
				}
			}
		}
		catch (Exception ex)
		{
			TerminalUtils.Log.Warning("Failed to set UTF-8 console output: " + ex.GetType().Name + ": " + ex.Message);
		}
	}

	// Token: 0x06001235 RID: 4661 RVA: 0x00054CDC File Offset: 0x00052EDC
	private static bool IsWindowsPlatform()
	{
		RuntimePlatform platform = Application.platform;
		return platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsServer;
	}

	// Token: 0x06001236 RID: 4662 RVA: 0x00054CFF File Offset: 0x00052EFF
	public static string StripColors(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}
		return TerminalUtils.RichTextColorRegex.Replace(TerminalUtils.AnsiColorRegex.Replace(input, string.Empty), string.Empty);
	}

	// Token: 0x06001237 RID: 4663 RVA: 0x00054D2C File Offset: 0x00052F2C
	private static bool DetectAnsiSupport()
	{
		bool result;
		try
		{
			string value = Utils.GetCommandLineArgument("--logColor", null) ?? Environment.GetEnvironmentVariable("PUCK_LOG_COLOR");
			if (!string.IsNullOrEmpty(value))
			{
				result = !TerminalUtils.IsFalsey(value);
			}
			else if (Environment.GetEnvironmentVariable("NO_COLOR") != null)
			{
				result = false;
			}
			else if (TerminalUtils.IsPipeBasedColorTerminal())
			{
				TerminalUtils.TryEnableWindowsVirtualTerminal();
				result = true;
			}
			else if (Console.IsOutputRedirected)
			{
				result = false;
			}
			else
			{
				RuntimePlatform platform = Application.platform;
				if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsServer)
				{
					result = TerminalUtils.TryEnableWindowsVirtualTerminal();
				}
				else
				{
					string environmentVariable = Environment.GetEnvironmentVariable("TERM");
					result = (!string.IsNullOrEmpty(environmentVariable) && environmentVariable != "dumb");
				}
			}
		}
		catch
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06001238 RID: 4664 RVA: 0x00054DE8 File Offset: 0x00052FE8
	private static bool IsPipeBasedColorTerminal()
	{
		return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MSYSTEM")) || string.Equals(Environment.GetEnvironmentVariable("ConEmuANSI"), "ON", StringComparison.OrdinalIgnoreCase);
	}

	// Token: 0x06001239 RID: 4665 RVA: 0x00054E18 File Offset: 0x00053018
	private static bool IsFalsey(string value)
	{
		string a = value.Trim().ToLowerInvariant();
		return a == "0" || a == "false";
	}

	// Token: 0x0600123A RID: 4666
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern uint GetConsoleOutputCP();

	// Token: 0x0600123B RID: 4667
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool SetConsoleOutputCP(uint wCodePageID);

	// Token: 0x0600123C RID: 4668
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr GetStdHandle(int nStdHandle);

	// Token: 0x0600123D RID: 4669
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

	// Token: 0x0600123E RID: 4670
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

	// Token: 0x0600123F RID: 4671 RVA: 0x00054E4C File Offset: 0x0005304C
	private static bool TryEnableWindowsVirtualTerminal()
	{
		IntPtr stdHandle = TerminalUtils.GetStdHandle(-11);
		uint num;
		return !(stdHandle == IntPtr.Zero) && !(stdHandle == new IntPtr(-1)) && TerminalUtils.GetConsoleMode(stdHandle, out num) && ((num & 4U) != 0U || TerminalUtils.SetConsoleMode(stdHandle, num | 4U));
	}

	// Token: 0x04000BC4 RID: 3012
	public static readonly bool SupportsAnsiColor = TerminalUtils.DetectAnsiSupport();

	// Token: 0x04000BC5 RID: 3013
	private static readonly Regex AnsiColorRegex = new Regex("\\x1b\\[[0-9;]*m", RegexOptions.Compiled);

	// Token: 0x04000BC6 RID: 3014
	private static readonly Regex RichTextColorRegex = new Regex("</?color(=[^>]*)?>", RegexOptions.Compiled);

	// Token: 0x04000BC7 RID: 3015
	private static readonly global::Logger Log = new global::Logger("TerminalUtils");

	// Token: 0x04000BC8 RID: 3016
	private const int STD_OUTPUT_HANDLE = -11;

	// Token: 0x04000BC9 RID: 3017
	private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4U;

	// Token: 0x04000BCA RID: 3018
	private const uint CP_UTF8 = 65001U;
}
