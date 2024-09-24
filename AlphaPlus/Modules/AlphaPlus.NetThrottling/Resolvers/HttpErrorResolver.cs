using AlphaPlus.Core.Helpers;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.NetThrottling.Resolvers
{
    public class HttpErrorResolver : BaseResolver
    {
        public override Task BlockNonAjaxRequest(HttpContext context)
        {
            context.Response.Redirect($"~/main/error/blocked?path={context.Request.Path}");
            return Task.CompletedTask;
        }
    }
}
