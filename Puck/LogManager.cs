using System;
using System.IO;
using System.Text;
using UnityEngine;

// Token: 0x020000EC RID: 236
public static class LogManager
{
	// Token: 0x06000743 RID: 1859 RVA: 0x00026F4C File Offset: 0x0002514C
	public static void Initialize()
	{
		TerminalUtils.TryEnableUtf8Output();
		Debug.unityLogger.logHandler = new LogManager.LogHandler(Debug.unityLogger.logHandler);
		if (!Directory.Exists(LogManager.logDirectoryPath))
		{
			Directory.CreateDirectory(LogManager.logDirectoryPath);
		}
		try
		{
			LogManager.streamWriter = new StreamWriter(LogManager.logFilePath, false, Encoding.UTF8);
			LogManager.streamWriter.AutoFlush = true;
		}
		catch (Exception ex)
		{
			LogManager.Logger.Error("Failed to initialize StreamWriter: " + ex.Message);
			LogManager.streamWriter = null;
		}
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x00026FE4 File Offset: 0x000251E4
	public static void Dispose()
	{
		object obj = LogManager.streamWriterLock;
		lock (obj)
		{
			if (LogManager.streamWriter != null)
			{
				LogManager.streamWriter.Flush();
				LogManager.streamWriter.Close();
				LogManager.streamWriter = null;
			}
		}
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x00027040 File Offset: 0x00025240
	private static string ParseLogType(LogType type)
	{
		string result;
		switch (type)
		{
		case LogType.Error:
			result = "ERROR";
			break;
		case LogType.Assert:
			result = "ASSERT";
			break;
		case LogType.Warning:
			result = "WARNING";
			break;
		case LogType.Log:
			result = "INFO";
			break;
		case LogType.Exception:
			result = "EXCEPTION";
			break;
		default:
			result = "UNKNOWN";
			break;
		}
		return result;
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x00027098 File Offset: 0x00025298
	private static string FormatPrefix(LogType type)
	{
		return string.Format("[{0:yyyy-MM-dd HH:mm:ss.fff}] [{1}]", DateTime.UtcNow, LogManager.ParseLogType(type));
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x000270B4 File Offset: 0x000252B4
	private static void WriteToLogFile(string prefix, string message)
	{
		string value = TerminalUtils.StripColors(prefix + " " + message);
		object obj = LogManager.streamWriterLock;
		lock (obj)
		{
			if (LogManager.streamWriter != null)
			{
				LogManager.streamWriter.WriteLine(value);
			}
		}
	}

	// Token: 0x040004B1 RID: 1201
	public const string DefaultLogFilePath = "./Logs/Puck.log";

	// Token: 0x040004B2 RID: 1202
	private static readonly global::Logger Logger = new global::Logger("LogManager");

	// Token: 0x040004B3 RID: 1203
	private static string logFileRawPath = Utils.GetCommandLineArgument("--logPath", null) ?? "./Logs/Puck.log";

	// Token: 0x040004B4 RID: 1204
	private static string logFilePath = Path.GetFullPath(LogManager.logFileRawPath);

	// Token: 0x040004B5 RID: 1205
	private static string logDirectoryPath = Path.GetDirectoryName(LogManager.logFilePath);

	// Token: 0x040004B6 RID: 1206
	private static StreamWriter streamWriter;

	// Token: 0x040004B7 RID: 1207
	private static readonly object streamWriterLock = new object();

	// Token: 0x020000ED RID: 237
	private class LogHandler : ILogHandler
	{
		// Token: 0x06000749 RID: 1865 RVA: 0x00027171 File Offset: 0x00025371
		public LogHandler(ILogHandler blh)
		{
			this.baseLogHandler = blh;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00027180 File Offset: 0x00025380
		private static string GetColor(LogType type)
		{
			if (Application.isEditor)
			{
				switch (type)
				{
				case LogType.Error:
					return "<color=red>";
				case LogType.Assert:
					return "<color=magenta>";
				case LogType.Warning:
					return "<color=yellow>";
				case LogType.Exception:
					return "<color=red>";
				}
				return "<color=grey>";
			}
			if (!TerminalUtils.SupportsAnsiColor)
			{
				return "";
			}
			switch (type)
			{
			case LogType.Error:
				return "\u001b[31m";
			case LogType.Assert:
				return "\u001b[35m";
			case LogType.Warning:
				return "\u001b[33m";
			case LogType.Exception:
				return "\u001b[91m";
			}
			return "\u001b[90m";
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00027228 File Offset: 0x00025428
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

		// Token: 0x0600074C RID: 1868 RVA: 0x0002724C File Offset: 0x0002544C
		[HideInCallstack]
		public void LogFormat(LogType type, Object context, string format, params object[] args)
		{
			string text = LogManager.FormatPrefix(type);
			LogManager.WriteToLogFile(text, (args.Length != 0) ? string.Format(format, args) : format);
			this.baseLogHandler.LogFormat(type, context, string.Concat(new string[]
			{
				LogManager.LogHandler.GetColor(type),
				text,
				LogManager.LogHandler.GetReset(),
				" ",
				format
			}), args);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x000272B1 File Offset: 0x000254B1
		public void LogException(Exception exception, Object context)
		{
			LogManager.WriteToLogFile(LogManager.FormatPrefix(LogType.Exception), exception.ToString());
			this.baseLogHandler.LogException(exception, context);
		}

		// Token: 0x040004B8 RID: 1208
		private ILogHandler baseLogHandler;
	}
}
