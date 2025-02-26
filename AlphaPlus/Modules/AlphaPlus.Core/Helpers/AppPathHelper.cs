namespace AlphaPlus.Core.Helpers;

public static class AppPathHelper
{
    public static string GetAppPath(string appName, string virtualPath)
    {
        if (string.IsNullOrWhiteSpace(appName))
        {
            throw new ArgumentNullException(nameof(appName));
        }

        if (string.IsNullOrWhiteSpace(virtualPath))
        {
            throw new ArgumentNullException(nameof(virtualPath));
        }

        return $"{appName}/{virtualPath.Replace("~/", string.Empty)}".Replace("//", "/");
    }
}