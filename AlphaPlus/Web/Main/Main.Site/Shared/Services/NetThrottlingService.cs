using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.NetThrottling;
using AlphaPlus.NetThrottling.Models;

namespace AlphaPlus.Main.Site.Shared.Services
{
    public interface INetThrottlingService
    {
        ThrottlePolicy ThrottlePolicy { get; }

        int GetBlockRemainingSeconds();

        bool IsBlacklist();

        void Throttle();
    }

    public class NetThrottlingService(INetThrottler netThrottler) : INetThrottlingService
    {
        private readonly INetThrottler netThrottler = netThrottler;

        public ThrottlePolicy ThrottlePolicy { get; } = netThrottler.Policies.FirstOrDefault(p => p.Name == "Main");

        public int GetBlockRemainingSeconds()
        {
            var timeSpan = netThrottler.GetBlockRemainingTime(ThrottlePolicy, GetCacheIdentity());
            return Convert.ToInt32(timeSpan.TotalSeconds);
        }

        public bool IsBlacklist() => netThrottler.IsBlacklist(GetCacheIdentity());

        public void Throttle()
        {
            var cacheIdentity = GetCacheIdentity();
            if (netThrottler.IsBlacklist(cacheIdentity))
            {
                return;
            }

            netThrottler.Throttle(ThrottlePolicy, cacheIdentity, true);
        }

        private CacheIdentity GetCacheIdentity()
        {
            return CacheIdentity.From(
                ThrottlePolicy.Name,
                AppSettings.SiteLabel,
                [AlphaContext.Current.Request.Path, UserSession.Current.UserName, UserSession.Current.SubUserName]);
        }
    }
}