using AlphaPlus.NetThrottling.Filters;
using AlphaPlus.NetThrottling.Models;
using AlphaPlus.NetThrottling.Utils;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.NetThrottling
{
    public class NetThrottlingMiddleware(RequestDelegate next, INetThrottler netThrottler, IFilterFactory filterFactory)
    {
        private readonly RequestDelegate _next = next;
        private readonly INetThrottler _netThrottler = netThrottler;
        private readonly IFilterFactory _filterFactory = filterFactory;

        public async Task InvokeAsync(HttpContext context)
        {
            var throttleResult = await ThrottleAsync(context);
            if (throttleResult == ThrottleResult.Blocked)
            {
                await _netThrottler.Resolver.Block(context);
                return;
            }

            await _next(context);
        }

        private async Task<ThrottleResult> ThrottleAsync(HttpContext httpContext)
        {
            foreach (var throttlePolicy in _netThrottler.Policies)
            {
                if (httpContext.Request.IsMatchRequest(throttlePolicy))
                {
                    var filter = _filterFactory.GetFilter(throttlePolicy.Filter);
                    return await _netThrottler.ThrottleAsync(httpContext, throttlePolicy, filter);
                }
            }

            return ThrottleResult.Allowed;
        }
    }

}