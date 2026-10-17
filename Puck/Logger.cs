using System;
using UnityEngine;

// Token: 0x0200022A RID: 554
public readonly struct Logger
{
	// Token: 0x06001187 RID: 4487 RVA: 0x00052CE5 File Offset: 0x00050EE5
	private static string GetColor(LogType type)
	{
		if (Application.isEditor)
		{
			return "<color=white>";
		}
		if (!TerminalUtils.SupportsAnsiColor)
		{
			return "";
		}
		return "\u001b[37m";
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x00027228 File Offset: 0x00025428
	private static string GetReset()
	{
		if (Application.isEditor)
		{
			return "</color>";
		}
		if (!TerminalUtils.SupportsAnsiColor)
		{
			return "";
		}
		return "\u001b[0m";
	}

	// Token: 0x06001189 RID: 4489 RVA: 0x00052D08 File Offset: 0x00050F08
	public Logger(string tag)
	{
		string color = global::Logger.GetColor(LogType.Log);
		string reset = global::Logger.GetReset();
		this.prefix = string.Concat(new string[]
		{
			color,
			"[",
			tag,
			"]",
			reset,
			" "
		});
	}

	// Token: 0x0600118A RID: 4490 RVA: 0x00052D57 File Offset: 0x00050F57
	[HideInCallstack]
	public void Info(string message)
	{
		Debug.Log(this.prefix + message);
	}

	// Token: 0x0600118B RID: 4491 RVA: 0x00052D6A File Offset: 0x00050F6A
	[HideInCallstack]
	public void Warning(string message)
	{
		Debug.LogWarning(this.prefix + message);
	}

	// Token: 0x0600118C RID: 4492 RVA: 0x00052D7D File Offset: 0x00050F7D
	[HideInCallstack]
	public void Error(string message)
	{
		Debug.LogError(this.prefix + message);
	}

	// Token: 0x0600118D RID: 4493 RVA: 0x00052D90 File Offset: 0x00050F90
	[HideInCallstack]
	public void Exception(Exception exception)
	{
		Debug.LogException(exception);
	}

	// Token: 0x04000B75 RID: 2933
	private readonly string prefix;
}
