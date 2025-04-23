using AlphaPlus.NetThrottling.Models;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.NetThrottling.Filters
{
    public interface IFilter
    {
        CacheIdentity GetCacheIdentity(HttpContext httpContext, ThrottlePolicy policy);
    }
}
