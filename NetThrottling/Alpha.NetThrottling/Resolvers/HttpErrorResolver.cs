using AlphaPlus.Web.Helpers;
using System.Web;

namespace Alpha.NetThrottling.Resolvers
{
    public class HttpErrorResolver : BaseResolver
    {
        public override void BlockNonAjaxRequest(HttpContextBase context)
        {
            var blockedUrl = MicroSites.MainUrl($"~/error/blocked?path={context.Request.Path}");
            context.Response.Redirect(blockedUrl);
        }
    }
}
