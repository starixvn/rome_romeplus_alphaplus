using System.Web;
using AlphaPlus.SecurityCode.Configurations;
using AlphaPlus.SecurityCode.Web;
using AlphaPlus.Core.Helpers;
using Microsoft.Extensions.Options;

namespace AlphaPlus.SecurityCode.Services;

public interface ISecurityCodePathBuilder
{
    string BuildResetSecurityCodePath();

    string BuildVerifyingSecurityCodePath(
        string redirectUrl = "",
        IDictionary<string, string> queryStringItems = null);
}

public class SecurityCodePathBuilder(IOptions<SecurityCodeOptions> options) : ISecurityCodePathBuilder
{
    private readonly SecurityCodeOptions options = options.Value;

    public string BuildResetSecurityCodePath()
        => MicroSites.MainUrl($"~/{options.ResetSecurityCodePath}");

    public string BuildVerifyingSecurityCodePath(string redirectUrl = "", IDictionary<string, string> queryStringItems = null)
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


        return MicroSites.MainUrl($"~/{BuildSecurityCheckingPath()}{queryString}").Replace("http%3a", string.Empty).Replace("http:", string.Empty);
    }

    private string BuildSecurityCheckingPath()
        => SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp
            ? options.VerifyingOtpPath
            : options.VerifyingSecurityCodePath;
}
