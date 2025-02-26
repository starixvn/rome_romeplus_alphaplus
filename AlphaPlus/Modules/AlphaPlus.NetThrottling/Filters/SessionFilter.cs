using AlphaPlus.NetThrottling.Models;
using AlphaPlus.NetThrottling.Utils;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.NetThrottling.Filters
{
    public class SessionFilter : IFilter
    {
        public CacheIdentity GetCacheIdentity(HttpContext httpContext, ThrottlePolicy policy)
        {
            var userSession = UserSessionFactory.Create();
            return CacheIdentity.From(policy, [httpContext.Request.Path, userSession.UserName, userSession.SubUserName]);
        }
    }
}
