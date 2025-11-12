namespace AlphaPlus.Tickets.Site.Features.Shared.Models;

public class AllowedSitesToShareLoad(IDictionary<string, string> siteNameToIP)
{
    private readonly IDictionary<string, string> siteNameToIP = siteNameToIP;

    public string GetIP(string siteName)
        => siteNameToIP.TryGetValue(siteName, out var ip) ? ip : string.Empty;
}
