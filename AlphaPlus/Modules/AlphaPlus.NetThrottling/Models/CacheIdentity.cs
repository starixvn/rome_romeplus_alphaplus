using AlphaPlus.Core.Configurations;

namespace AlphaPlus.NetThrottling.Models
{
    public class CacheIdentity(string throttleKey, string blockKey)
    {
        public string ThrottleKey { get; private set; } = throttleKey;

        public string BlockKey { get; private set; } = blockKey;

        public static CacheIdentity From(ThrottlePolicy policy, string[] keys)
        {
            return From(policy.Name, AppSettings.SiteLabel, keys);
        }

        public static CacheIdentity From(string policyName, string siteLabel, string[] keys)
        {
            if (keys == null || !keys.Any())
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var prefix = $"{policyName}}}:{{{siteLabel}}}";
            var suffix = $"{{{string.Join("_", keys)}";
            var throttleKey = $"{prefix}:{{{Utils.Constants.Throttle}}}:{suffix}";
            var blockKey = $"{prefix}:{{{Utils.Constants.Block}}}:{suffix}";
            return new CacheIdentity(throttleKey, blockKey);
        }
    }
}
