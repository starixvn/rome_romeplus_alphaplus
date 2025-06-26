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
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IDisableNetThrottling>() is object || context.Request.Path.StartsWithSegments("/health"))
            {
                await _next(context);
                return;
            }

            var throttleResult = Throttle(context);
            if (throttleResult == ThrottleResult.Blocked)
            {
                await _netThrottler.Resolver.Block(context);
                return;
            }

            await _next(context);
        }

        private ThrottleResult Throttle(HttpContext httpContext)
        {
            foreach (var throttlePolicy in _netThrottler.Policies)
            {
                if (httpContext.Request.IsMatchRequest(throttlePolicy))
                {
                    var filter = _filterFactory.GetFilter(throttlePolicy.Filter);
                    return _netThrottler.Throttle(httpContext, throttlePolicy, filter);
                }
            }

            return ThrottleResult.Allowed;
        }
    }

}