using Microsoft.AspNetCore.Http;

namespace AlphaPlus.NetThrottling.Resolvers
{
    public class HttpErrorResolver : BaseResolver
    {
        public override Task BlockNonAjaxRequest(HttpContext context)
        {
            // var blockedUrl = MicroSites.MainUrl($"~/error/blocked?path={context.Request.Path}");
            // context.Response.Redirect(blockedUrl);
            context.Response.StatusCode = 429;
            context.Response.ContentType = "text/plain";
            context.Response.WriteAsync("Too many requests.");

            return Task.CompletedTask;
        }
    }
}
