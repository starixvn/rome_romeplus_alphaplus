using System.Net;
using AlphaPlus.Core.Helpers;
using AlphaPlus.NetThrottling.Resources;
using AlphaPlus.NetThrottling.Utils;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.NetThrottling.Resolvers
{
    public class BaseResolver : IResolver
    {
        public Task Block() => Block(AlphaContext.Current);

        public virtual Task Block(HttpContext context)
        {
            if (context.Request.IsAjaxRequest())
            {
                return BlockAjaxRequest(context);
            }
            else
            {
                return BlockNonAjaxRequest(context);
            }
        }

        public virtual Task BlockAjaxRequest(HttpContext context) => BlockRequest(context);

        public virtual Task BlockNonAjaxRequest(HttpContext context) => BlockRequest(context);

        private static Task BlockRequest(HttpContext context)
        {
            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            return response.WriteAsync(Lang.BlockedContent);
        }
    }
}
