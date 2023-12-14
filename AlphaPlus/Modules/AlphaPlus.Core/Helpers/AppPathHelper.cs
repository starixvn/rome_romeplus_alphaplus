using System.Text.RegularExpressions;
using AlphaPlus.Core.Utils;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.Core.Helpers;

public static class AppPathHelper
{
    public static string GetAppPath(string appName, string virtualPath)
        => GetAppPath(appName, virtualPath, null, null);

    public static string GetAppPath(
        string appName,
        string virtualPath,
        Func<HttpContext> getCurrentContext,
        Func<string> getAppDomainVirtualPath)
    {
        if (string.IsNullOrWhiteSpace(appName))
        {
            throw new ArgumentNullException(nameof(appName));
        }

        if (string.IsNullOrWhiteSpace(virtualPath))
        {
            throw new ArgumentNullException(nameof(virtualPath));
        }

        getCurrentContext ??= (() => AlphaContext.Current);
        getAppDomainVirtualPath ??= (() => AlphaContext.Current.Request.PathBase.Value);

        var context = getCurrentContext();

        var appPath = VirtualPathUtility.ToAbsolute(virtualPath);
        var appDomainPath = getAppDomainVirtualPath();
        if (!appDomainPath.Equals("/", StringComparison.OrdinalIgnoreCase))
        {
            appPath = Regex.Replace(appPath, appDomainPath, appName, RegexOptions.IgnoreCase);
        }
        else
        {
            appPath = $"{appName}{appPath}";
        }

        return appPath;
    }

    public static string GetRelativePath(string virtualPath, string absolutePath)
    {
        if (string.IsNullOrWhiteSpace(virtualPath) || string.IsNullOrWhiteSpace(absolutePath))
        {
            return virtualPath;
        }

        if (absolutePath.StartsWith(virtualPath, StringComparison.OrdinalIgnoreCase))
        {
            return absolutePath[(virtualPath.Length + 1)..];
        }

        return absolutePath;
    }
}
