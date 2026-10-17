using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

// Token: 0x02000223 RID: 547
public static class ConfigUtils
{
	// Token: 0x06001167 RID: 4455 RVA: 0x00052660 File Offset: 0x00050860
	public static T LoadConfigFromFile<T>(string filePath, bool createIfNotExists = true) where T : class, new()
	{
		if (string.IsNullOrEmpty(filePath))
		{
			return default(T);
		}
		if (!File.Exists(filePath) && createIfNotExists)
		{
			ConfigUtils.SaveConfigToFile<T>(filePath, Activator.CreateInstance<T>());
		}
		string serializedString;
		if (!ConfigUtils.TryReadFile(filePath, out serializedString))
		{
			return default(T);
		}
		return ConfigUtils.LoadConfigFromSerializedString<T>(serializedString, filePath);
	}

	// Token: 0x06001168 RID: 4456 RVA: 0x000526B4 File Offset: 0x000508B4
	public static T LoadConfigFromSerializedString<T>(string serializedString, string source = "serialized string") where T : class, new()
	{
		if (string.IsNullOrEmpty(serializedString))
		{
			return default(T);
		}
		T result;
		if (!ConfigUtils.TryDeserialize<T>(serializedString, source, out result))
		{
			return default(T);
		}
		return result;
	}

	// Token: 0x06001169 RID: 4457 RVA: 0x000526EC File Offset: 0x000508EC
	public static bool TryLoadFromFile<T>(string filePath, out T result)
	{
		result = default(T);
		string serializedString;
		return ConfigUtils.TryReadFile(filePath, out serializedString) && ConfigUtils.TryDeserialize<T>(serializedString, filePath, out result);
	}

	// Token: 0x0600116A RID: 4458 RVA: 0x00052714 File Offset: 0x00050914
	public static bool TryDeserialize<T>(string serializedString, string source, out T result)
	{
		result = default(T);
		bool result2;
		try
		{
			result = JsonSerializer.Deserialize<T>(serializedString, null);
			result2 = true;
		}
		catch (JsonException exception)
		{
			ConfigUtils.Logger.Error("Malformed JSON in " + source + ": " + ConfigUtils.DescribeJsonError(exception));
			result2 = false;
		}
		catch (Exception ex)
		{
			ConfigUtils.Logger.Error("Error loading config from " + source + ": " + ex.Message);
			result2 = false;
		}
		return result2;
	}

	// Token: 0x0600116B RID: 4459 RVA: 0x000527A0 File Offset: 0x000509A0
	private static bool TryReadFile(string filePath, out string contents)
	{
		contents = null;
		bool result;
		try
		{
			contents = File.ReadAllText(filePath);
			result = true;
		}
		catch (Exception ex)
		{
			ConfigUtils.Logger.Error("Could not read config file " + filePath + ": " + ex.Message);
			result = false;
		}
		return result;
	}

	// Token: 0x0600116C RID: 4460 RVA: 0x000527F4 File Offset: 0x000509F4
	private static string DescribeJsonError(JsonException exception)
	{
		string text = "";
		if (exception.LineNumber != null)
		{
			text = string.Format("line {0}", exception.LineNumber + 1L);
			if (exception.BytePositionInLine != null)
			{
				text += string.Format(", position {0}", exception.BytePositionInLine + 1L);
			}
		}
		if (!string.IsNullOrEmpty(exception.Path) && exception.Path != "$")
		{
			text += (string.IsNullOrEmpty(text) ? ("at " + exception.Path) : (" (at " + exception.Path + ")"));
		}
		if (!string.IsNullOrEmpty(text))
		{
			return text + ": " + exception.Message;
		}
		return exception.Message;
	}

	// Token: 0x0600116D RID: 4461 RVA: 0x00052920 File Offset: 0x00050B20
	public static void SaveConfigToFile<T>(string filePath, T config)
	{
		try
		{
			JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
			{
				WriteIndented = true
			};
			jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			string text = JsonSerializer.Serialize<T>(config, jsonSerializerOptions);
			ConfigUtils.Logger.Info("Serialized config " + typeof(T).Name + ": " + text);
			File.WriteAllText(filePath, text);
		}
		catch (Exception ex)
		{
			ConfigUtils.Logger.Error("Error saving config to " + filePath + ": " + ex.Message);
		}
	}

	// Token: 0x04000AD1 RID: 2769
	private static readonly Logger Logger = new Logger("ConfigUtils");
}
