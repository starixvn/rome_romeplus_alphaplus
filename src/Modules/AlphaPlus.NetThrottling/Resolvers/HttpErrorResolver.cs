using AlphaPlus.Core.Helpers;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.NetThrottling.Resolvers
{
    public class HttpErrorResolver : BaseResolver
    {
        public override Task BlockNonAjaxRequest(HttpContext context)
        {
            var blockedUrl = MicroSites.MainUrl($"~/error/blocked?path={context.Request.Path}");
            context.Response.Redirect(blockedUrl);
            return Task.CompletedTask;
        }
    }
}
