using System.Diagnostics;
using System.Reflection;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Utils;
using Microsoft.AspNetCore.Html;

namespace AlphaPlus.Core.Helpers;

public static class StaticResources
{
    private static string staticResourcesVersion;

    private static string minText = "";

    public static void Init(string version)
    {
        if (Debugger.IsAttached)
        {
            minText = string.Empty;
        }

        staticResourcesVersion = version;
    }

    public static string GetBuildVersion()
    {
        Assembly assembly = Assembly.GetCallingAssembly();
        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

        return versionInfo.FileVersion;
    }

    private static HtmlString RenderStyle(params string[] paths)
    {
        var styleHtml = string.Join(string.Empty, paths.Select(path => $"<link href=\"{path}\" rel=\"stylesheet\" type=\"text/css\" asp-append-version=\"true\">"));

        return new HtmlString(styleHtml);
    }

    private static HtmlString RenderStyleWithMedia(int maxDeviceWidth, params string[] paths)
    {
        var styleHtml = string.Join(string.Empty, paths.Select(path => $"<link href=\"{path}\" rel=\"stylesheet\" type=\"text/css\" media=\"screen and (max-device-width: {maxDeviceWidth}px)\">"));

        return new HtmlString(styleHtml);
    }

    private static HtmlString RenderScript(params string[] paths)
    {
        var scriptHtml = string.Join(string.Empty, paths.Select(path => $"<script src=\"{path}\" type=\"text/javascript\" asp-append-version=\"true\"></script>"));

        return new HtmlString(scriptHtml);
    }

    public static HtmlString RenderScripts(params string[] paths)
    {
        var scriptHtml = string.Join(
            string.Empty,
            paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{minText}.js")));

        return RenderScript(scriptHtml);
    }

    public static HtmlString RenderStyles(params string[] paths)
    {
        var styleHtml = string.Join(
            string.Empty,
            paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{minText}.css")));

        return RenderStyle(styleHtml);
    }

    public static HtmlString RenderStylesWithMedia(int maxDeviceWidth, params string[] paths)
    {
        var styleHtml = string.Join(
            string.Empty,
            paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{minText}.css")));

        return RenderStyleWithMedia(maxDeviceWidth, styleHtml);
    }

    public static HtmlString RenderStylesWithDarkMode(params string[] paths)
    {
        var darkModeName = GetDarkModeName();
        var styleHtml = string.Join(
            string.Empty,
            paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{darkModeName}{minText}.css")));

        return RenderStyle(styleHtml);
    }

    public static HtmlString RenderStylesWithRoleAndDarkMode(params string[] paths)
    {
        var userSession = GetCurrentUserSession();
        var roleName = GetRoleName(userSession);
        var darkModeName = GetDarkModeName(userSession);
        var styleHtml = string.Join(
            string.Empty,
            paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{roleName}{darkModeName}{minText}.css")));

        return RenderStyle(styleHtml);
    }

    public static HtmlString RenderStylesWithMediaDarkMode(int maxDeviceWidth, params string[] paths)
    {
        var darkModeName = GetDarkModeName();
        var styleHtml = string.Join(
            string.Empty,
            paths.Select(path => VirtualPathUtility.ToAbsolute($"{path}{darkModeName}{minText}.css")));

        return RenderStyleWithMedia(maxDeviceWidth, styleHtml);
    }

    public static class AssetScripts
    {
        public static HtmlString Render(params string[] paths)
        {
            var assetAppPath = GetAssetAppPath();

            return RenderScript(paths.Select(path => $"{assetAppPath}{path}{minText}.js?v={staticResourcesVersion}").ToArray());
        }

        public static HtmlString RenderCommon(string path)
        {
            var assetAppPath = GetAssetAppPath();
            var version = !string.IsNullOrEmpty(AppSettings.StaticResourcesVersion) ? $"?v={AppSettings.StaticResourcesVersion}" : string.Empty;

            return RenderScript($"{assetAppPath}{path}{minText}.js{version}");
        }
    }

    public static class AssetStyles
    {
        public static HtmlString Render(params string[] paths)
        {
            var assetAppPath = GetAssetAppPath();

            return RenderStyle(paths.Select(path => $"{assetAppPath}{path}{minText}.css?v={staticResourcesVersion}").ToArray());
        }

        public static HtmlString RenderCommon(string path)
        {
            var assetAppPath = GetAssetAppPath();
            var version = !string.IsNullOrEmpty(AppSettings.StaticResourcesVersion) ? $"?v={AppSettings.StaticResourcesVersion}" : string.Empty;

            return RenderStyle($"{assetAppPath}{path}{minText}.css{version}");
        }

        public static HtmlString RenderWithDarkMode(params string[] paths)
        {
            var userSession = GetCurrentUserSession();
            var assetAppPath = GetAssetAppPath(userSession);
            var darkModeName = GetDarkModeName(userSession);

            return RenderStyle(paths.Select(path => $"{assetAppPath}{path}{darkModeName}{minText}.css?v={staticResourcesVersion}").ToArray());
        }

        public static HtmlString RenderWithRoleAndDarkMode(params string[] paths)
        {
            var userSession = GetCurrentUserSession();
            var assetAppPath = GetAssetAppPath(userSession);
            var roleName = GetRoleName(userSession);
            var darkModeName = GetDarkModeName(userSession);

            return RenderStyle(paths.Select(path => $"{assetAppPath}{path}{roleName}{darkModeName}{minText}.css?v={staticResourcesVersion}").ToArray());
        }

        public static HtmlString RenderCommonWithDarkMode(string path)
        {
            var userSession = GetCurrentUserSession();
            var assetAppPath = GetAssetAppPath(userSession);
            var darkModeName = GetDarkModeName(userSession);
            var version = !string.IsNullOrEmpty(AppSettings.StaticResourcesVersion) ? $"?v={AppSettings.StaticResourcesVersion}" : string.Empty;

            return RenderStyle($"{assetAppPath}{path}{darkModeName}{minText}.css{version}");
        }
    }

    private static string GetAssetAppPath(IUserSession userSession = null)
    {
        userSession ??= GetCurrentUserSession();
        var assetAppPath = userSession.AssetAppPath;

        return string.IsNullOrWhiteSpace(assetAppPath)
            ? AppSettings.AssetAppPath
            : assetAppPath;
    }

    private static string GetRoleName(IUserSession userSession = null)
    {
        userSession ??= GetCurrentUserSession();

        return Enumeration.FromValue<UserLevel>(userSession.RoleId).DisplayName.ToLowerInvariant();
    }

    private static string GetDarkModeName(IUserSession userSession = null)
    {
        userSession ??= GetCurrentUserSession();

        return userSession.DarkModeName;
    }

    private static IUserSession GetCurrentUserSession() => UserSession.Current;
}
