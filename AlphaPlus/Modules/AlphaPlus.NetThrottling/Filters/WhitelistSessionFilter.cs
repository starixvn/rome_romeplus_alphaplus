using AlphaPlus.NetThrottling.Models;
using AlphaPlus.NetThrottling.Utils;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.NetThrottling.Filters
{
    public class WhitelistSessionFilter(IEnumerable<int> custIds) : IFilter
    {
        private readonly IEnumerable<int> custIds = custIds;

        public CacheIdentity GetCacheIdentity(HttpContext httpContext, ThrottlePolicy policy)
        {
            var userSession = UserSessionFactory.Create();
            if (userSession == null)
            {
                return null;
            }

            var custId = userSession.SRecommendId;
            if (custIds.Contains(custId))
            {
                return null;
            }

            return CacheIdentity.From(policy, [httpContext.Request.Path, userSession.UserName, userSession.SubUserName]);
        }
    }
}
