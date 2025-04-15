using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Settings.ViewLogs.API.Constants
{
    public static class DefaultVariables
    {
        public const int PageSize = 50;

        public static MemoryCacheEntryOptions CacheItemOptions
        {
            get
            {
                return new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(1)
                };
            }
        }
    }
}
