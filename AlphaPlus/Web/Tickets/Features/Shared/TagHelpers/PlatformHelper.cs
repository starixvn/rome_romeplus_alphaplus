using AlphaPlus.Tickets.Site.App_GlobalResources;

namespace AlphaPlus.Tickets.Site.Features.Shared.TagHelpers;

public static class PlatformHelper
{
    public static string GetPlatform(int? device)
    {
        var platform = Resources.Platform_NA;

        if (device != null)
        {
            platform = Resources.ResourceManager.GetString(string.Format("Platform_{0}", device));
            platform = platform ?? Resources.Platform_Uncategorized;
        }

        return platform;
    }
}
