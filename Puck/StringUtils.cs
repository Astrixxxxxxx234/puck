using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x0200023C RID: 572
public static class StringUtils
{
	// Token: 0x060011E7 RID: 4583 RVA: 0x0005395A File Offset: 0x00051B5A
	static StringUtils()
	{
		StringUtils.LoadProfanityWords();
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x00053970 File Offset: 0x00051B70
	private static void LoadProfanityWords()
	{
		try
		{
			string[] array = JsonSerializer.Deserialize<string[]>(Resources.Load<TextAsset>("profanity_words").text, null);
			string[] value = (from w in array
			where !string.IsNullOrWhiteSpace(w)
			orderby w.Length descending
			select Regex.Escape(w)).ToArray<string>();
			StringUtils.profanityRegex = new Regex("(?<![a-zA-Z])(" + string.Join("|", value) + ")(?![a-zA-Z])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			StringUtils.Logger.Info(string.Format("Loaded {0} profanity words and compiled regex", array.Length));
		}
		catch (Exception ex)
		{
			StringUtils.Logger.Error("Error loading profanity words asset: " + ex.Message);
		}
	}

	// Token: 0x060011E9 RID: 4585 RVA: 0x00053A78 File Offset: 0x00051C78
	public static string FilterStringNotLetters(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		StringBuilder stringBuilder = new StringBuilder(text.Length);
		foreach (char c in text)
		{
			if (char.IsLetter(c))
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060011EA RID: 4586 RVA: 0x00053ACC File Offset: 0x00051CCC
	public static string FilterStringSpecialCharacters(string text, string[] characterWhitelist = null, string[] characterBlacklist = null)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		StringBuilder stringBuilder = new StringBuilder(text.Length);
		TextElementEnumerator textElementEnumerator = StringInfo.GetTextElementEnumerator(text);
		while (textElementEnumerator.MoveNext())
		{
			string textElement = textElementEnumerator.GetTextElement();
			if (characterBlacklist == null || !characterBlacklist.Contains(textElement))
			{
				UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(textElement, 0);
				if (textElement.All(new Func<char, bool>(char.IsLetterOrDigit)) || textElement.All(new Func<char, bool>(char.IsWhiteSpace)) || unicodeCategory == UnicodeCategory.ConnectorPunctuation || unicodeCategory == UnicodeCategory.DashPunctuation || unicodeCategory == UnicodeCategory.OpenPunctuation || unicodeCategory == UnicodeCategory.ClosePunctuation || unicodeCategory == UnicodeCategory.InitialQuotePunctuation || unicodeCategory == UnicodeCategory.FinalQuotePunctuation || unicodeCategory == UnicodeCategory.OtherPunctuation || unicodeCategory == UnicodeCategory.MathSymbol || unicodeCategory == UnicodeCategory.CurrencySymbol || (characterWhitelist != null && characterWhitelist.Contains(textElement)))
				{
					stringBuilder.Append(textElement);
				}
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060011EB RID: 4587 RVA: 0x00053B90 File Offset: 0x00051D90
	public static string FilterStringProfanity(string text, bool replaceWithStars = false)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		if (StringUtils.profanityRegex == null)
		{
			return text;
		}
		string text2;
		if (replaceWithStars)
		{
			text2 = StringUtils.profanityRegex.Replace(text, (Match match) => new string('*', match.Length));
		}
		else
		{
			text2 = StringUtils.profanityRegex.Replace(text, string.Empty);
		}
		if (!replaceWithStars)
		{
			text2 = Regex.Replace(text2, "\\s+", " ").Trim();
		}
		return text2;
	}

	// Token: 0x060011EC RID: 4588 RVA: 0x00053C0C File Offset: 0x00051E0C
	public static string FilterStringRichText(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		string[] richTextTags = new string[]
		{
			"a",
			"align",
			"allcaps",
			"alpha",
			"b",
			"br",
			"color",
			"cspace",
			"font",
			"font-weight",
			"gradient",
			"i",
			"indent",
			"line-height",
			"line-indent",
			"link",
			"lowercase",
			"margin",
			"margin-left",
			"margin-right",
			"mark",
			"mspace",
			"nobr",
			"noparse",
			"pos",
			"s",
			"size",
			"smallcaps",
			"space",
			"sprite",
			"style",
			"sub",
			"sup",
			"u",
			"uppercase",
			"voffset",
			"width"
		};
		string pattern = "</?(#[0-9a-fA-F]*|\\w+(?:-\\w+)?)(?:[\\s=][^>]*)?>";
		MatchEvaluator evaluator = delegate(Match match)
		{
			string text4 = match.Groups[1].Value.ToLower();
			if (text4.StartsWith("#"))
			{
				return string.Empty;
			}
			if (richTextTags.Contains(text4))
			{
				return string.Empty;
			}
			return match.Value;
		};
		string text2 = text;
		string text3;
		do
		{
			text3 = text2;
			text2 = Regex.Replace(text3, pattern, evaluator, RegexOptions.IgnoreCase);
		}
		while (text2 != text3);
		return text2;
	}

	// Token: 0x060011ED RID: 4589 RVA: 0x00053DA8 File Offset: 0x00051FA8
	public static string WrapInColor(string text, string color)
	{
		if (string.IsNullOrEmpty(color))
		{
			return text;
		}
		return string.Concat(new string[]
		{
			"<color=",
			color,
			">",
			text,
			"</color>"
		});
	}

	// Token: 0x060011EE RID: 4590 RVA: 0x00053DE0 File Offset: 0x00051FE0
	public static List<string> ChunkLinesByByteBudget(IReadOnlyList<string> lines, int maxBytes)
	{
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		foreach (string text in lines)
		{
			int byteCount = Encoding.UTF8.GetByteCount(text);
			int num2 = (stringBuilder.Length > 0) ? 1 : 0;
			if (stringBuilder.Length > 0 && num + num2 + byteCount > maxBytes)
			{
				list.Add(stringBuilder.ToString());
				stringBuilder.Clear();
				num = 0;
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append('\n');
				num++;
			}
			stringBuilder.Append(text);
			num += byteCount;
		}
		if (stringBuilder.Length > 0)
		{
			list.Add(stringBuilder.ToString());
		}
		return list;
	}

	// Token: 0x060011EF RID: 4591 RVA: 0x00053EB0 File Offset: 0x000520B0
	public static string WrapInTeamColor(string text, PlayerTeam team)
	{
		string text2;
		if (team != PlayerTeam.Blue)
		{
			if (team != PlayerTeam.Red)
			{
				text2 = "#404040";
			}
			else
			{
				text2 = "#d13333";
			}
		}
		else
		{
			text2 = "#3b82f6";
		}
		string text3 = text2;
		if (text3 == null)
		{
			return text;
		}
		return string.Concat(new string[]
		{
			"<color=",
			text3,
			">",
			text,
			"</color>"
		});
	}

	// Token: 0x04000BA5 RID: 2981
	private static readonly global::Logger Logger = new global::Logger("StringUtils");

	// Token: 0x04000BA6 RID: 2982
	private static Regex profanityRegex;
}
