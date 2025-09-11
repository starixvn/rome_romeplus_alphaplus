namespace AlphaPlus.Core.Helpers;

public static class SpNameHelpers
{
    public static string SiteDbPrefix(string spName, int? siteDbGroupId)
    => siteDbGroupId.HasValue ? $"SiteDb{siteDbGroupId.Value}_{spName}" : spName;

    public static string CustDbPrefix(string spName, int? custDbGroupId)
        => custDbGroupId.HasValue ? $"CustDb{custDbGroupId.Value}_{spName}" : spName;
}