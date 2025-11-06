namespace AlphaPlus.Reports.Features.Shared.Models;

public class AllowedSitesToShareLoad(IDictionary<string, string> siteNameToSuffix)
{
    public string GetSuffixBySiteName(string siteName)
        => siteNameToSuffix.TryGetValue(siteName, out var ip) ? ip : string.Empty;
}