using AlphaPlus.NetThrottling.Models;
using AlphaPlus.NetThrottling.Resolvers;

namespace AlphaPlus.NetThrottling
{
    public sealed class NetThrottlingConfiguration
    {
        public bool IsUseRedis { get; set; }

        public string Resolver { get; set; } = ResolverTypes.Base;

        public string RedisConfigurationOptions { get; set; }

        public ThrottlePolicy[] Policies { get; set; } = [];

        public int[] WhiteListCustIds { get; set; } = [];
    }
}