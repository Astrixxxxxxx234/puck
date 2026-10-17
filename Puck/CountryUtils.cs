using System;
using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;

// Token: 0x02000226 RID: 550
public static class CountryUtils
{
	// Token: 0x1700016E RID: 366
	// (get) Token: 0x0600117A RID: 4474 RVA: 0x00052AA8 File Offset: 0x00050CA8
	// (set) Token: 0x0600117B RID: 4475 RVA: 0x00052AAF File Offset: 0x00050CAF
	public static List<Country> Countries { get; private set; } = new List<Country>();

	// Token: 0x0600117C RID: 4476 RVA: 0x00052AB7 File Offset: 0x00050CB7
	static CountryUtils()
	{
		CountryUtils.LoadCountries();
	}

	// Token: 0x0600117D RID: 4477 RVA: 0x00052AD8 File Offset: 0x00050CD8
	public static Country GetCountryByCode(string code)
	{
		return CountryUtils.Countries.Find((Country country) => country.code.Equals(code, StringComparison.OrdinalIgnoreCase));
	}

	// Token: 0x0600117E RID: 4478 RVA: 0x00052B08 File Offset: 0x00050D08
	public static Country GetCountryByName(string name)
	{
		return CountryUtils.Countries.Find((Country country) => country.name.Equals(name, StringComparison.OrdinalIgnoreCase));
	}

	// Token: 0x0600117F RID: 4479 RVA: 0x00052B38 File Offset: 0x00050D38
	private static void LoadCountries()
	{
		try
		{
			CountryUtils.Countries = JsonSerializer.Deserialize<List<Country>>(Resources.Load<TextAsset>("countries").text, null);
			CountryUtils.Logger.Info(string.Format("Loaded {0} countries", CountryUtils.Countries.Count));
		}
		catch (Exception ex)
		{
			CountryUtils.Logger.Error("Error loading countries asset: " + ex.Message);
		}
	}

	// Token: 0x04000B6C RID: 2924
	private static readonly global::Logger Logger = new global::Logger("CountryUtils");
}
