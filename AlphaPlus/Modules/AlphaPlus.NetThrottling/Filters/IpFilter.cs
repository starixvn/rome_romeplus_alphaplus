using AlphaPlus.NetThrottling.Models;
using AlphaPlus.Utilities.IP;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.NetThrottling.Filters
{
    public class IpFilter(IPHelper ipHelper) : IFilter
    {
        private readonly IPHelper ipHelper = ipHelper;

        public CacheIdentity GetCacheIdentity(HttpContext httpContext, ThrottlePolicy policy)
        {
            var clientIp = ipHelper.GetClientIP();
            return CacheIdentity.From(policy, [httpContext.Request.Path, clientIp]);
        }
    }
}
