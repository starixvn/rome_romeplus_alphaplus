using AlphaPlus.SecurityCode.Services;
using AlphaPlus.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace AlphaPlus.SecurityCode.Web;

public interface ISecurityCodeHttpContext
{
    bool IsValidRequest();

    string GetRelativePath();

    bool IsAjaxRequest();

    Task DenyAjaxRequest();

    void RedirectToVerifySecurityCodePage();
}

public class SecurityCodeHttpContext(ISecurityCodePathBuilder securityCodePathBuilder) : ISecurityCodeHttpContext
{
    private readonly ISecurityCodePathBuilder securityCodePathBuilder = securityCodePathBuilder;

    public Task DenyAjaxRequest()
    {
        var response = AlphaContext.Current.Response;
        response.Clear();
        return response.WriteAsJsonAsync(new { code = 999991, message = "This action is required security code." });
    }

    public string GetRelativePath()
    {
        var request = AlphaContext.Current.Request;
        var pathBase = request.PathBase;

        if (string.IsNullOrWhiteSpace(pathBase))
        {
            return request.GetEncodedPathAndQuery();
        }

        return request.GetEncodedPathAndQuery().Replace(request.PathBase, string.Empty);
    }

    public bool IsAjaxRequest()
    {
        var request = AlphaContext.Current.Request;
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return (request.Headers != null && request.Headers.XRequestedWith == "XMLHttpRequest")
            || (request.Headers != null && request.Headers["IsPostAjax"] == "true");
    }

    public bool IsValidRequest()
        => AlphaContext.Current != null
            && AlphaContext.Current.Request != null
            && AlphaContext.Current.Session != null;

    public void RedirectToVerifySecurityCodePage()
    {
        var httpContext = AlphaContext.Current;
        var request = httpContext.Request;
        var requestHeaders = request.Headers;
        var response = httpContext.Response;

        if (requestHeaders != null && requestHeaders.ContentType == "application/x-www-form-urlencoded")
        {
            response.Redirect(securityCodePathBuilder.BuildVerifyingSecurityCodePath(), false, true);
        }
        else
        {
            var rawUrl = request.GetDisplayUrl();
            response.Redirect(this.securityCodePathBuilder.BuildVerifyingSecurityCodePath(rawUrl), false, true);
        }
    }

}
