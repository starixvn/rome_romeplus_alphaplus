namespace AlphaPlus.Settings.Site.Features.ViewLogs.Helpers
{
    public static class AppPathHelper
    {
        public static string GetAppPath(string appName, string virtualPath)
        {
            return Core.Helpers.AppPathHelper.GetAppPath(appName, virtualPath);
        }
    }
}