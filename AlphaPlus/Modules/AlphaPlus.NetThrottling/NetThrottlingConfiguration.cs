using AlphaPlus.NetThrottling.Models;

namespace AlphaPlus.NetThrottling
{
    public sealed class NetThrottlingConfiguration
    {
        public bool IsUseRedis { get; set; } = false;

        public string Resolver { get; set; }

        public string RedisConfigurationOptions { get; set; }

        public ThrottlePolicy[] Policies { get; set; } = [];

        public int[] WhiteListCustIds { get; set; } = [];
    }
}