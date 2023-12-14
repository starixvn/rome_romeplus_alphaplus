using System.Web;
using AlphaPlus.SecurityCode.Configurations;
using AlphaPlus.SecurityCode.Web;
using AlphaPlus.Core.Helpers;
using Microsoft.Extensions.Options;

namespace AlphaPlus.SecurityCode.Services;

public interface ISecurityCodePathBuilder
{
    string BuildResetSecurityCodePath(string rootPath = null, string appVirtualPath = null);

    string BuildVerifyingSecurityCodePath(
        string redirectUrl = "",
        IDictionary<string, string> queryStringItems = null,
        string rootPath = null,
        string appVirtualPath = null);
}

public class SecurityCodePathBuilder(IOptions<SecurityCodeOptions> options) : ISecurityCodePathBuilder
{
    private readonly SecurityCodeOptions options = options.Value;

    public string BuildResetSecurityCodePath(string rootPath = null, string appVirtualPath = null)
    {
        return BuildRootPath(rootPath, appVirtualPath) + options.ResetSecurityCodePath;
    }

    public string BuildVerifyingSecurityCodePath(string redirectUrl = "", IDictionary<string, string> queryStringItems = null, string rootPath = null, string appVirtualPath = null)
    {
        var queryString = string.Empty;
        if (queryStringItems != null
            && queryStringItems.Keys.Count != 0)
        {
            queryString = "?";

            foreach (var queryStringItem in queryStringItems)
            {
                queryString += $"{HttpUtility.UrlEncode(queryStringItem.Key)}={HttpUtility.UrlEncode(queryStringItem.Value)}&";
            }
        }

        if (!string.IsNullOrWhiteSpace(redirectUrl))
        {
            queryString += (string.IsNullOrWhiteSpace(queryString) ? "?" : string.Empty)
                + $"redirectUrl={HttpUtility.UrlEncode(redirectUrl)}";
        }

        var scRootPath = BuildRootPath(rootPath, appVirtualPath);

        return $"{scRootPath}{BuildSecurityCheckingPath()}{queryString}";
    }

    private string BuildRootPath(string rootPath = null, string appVirtualPath = null)
    {
        rootPath ??= AlphaContext.Current.Request.PathBase.Value;

        if (options.IsSiteMain)
        {
            return rootPath;
        }

        return rootPath.Replace(appVirtualPath ?? AlphaContext.Current.Request.Path.Value, options.SiteMainApp);
    }

    private string BuildSecurityCheckingPath()
        => SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp
            ? options.VerifyingOtpPath
            : options.VerifyingSecurityCodePath;
}
