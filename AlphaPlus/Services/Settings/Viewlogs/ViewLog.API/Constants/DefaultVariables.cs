using Fanex.Caching;

namespace AlphaPlus.Settings.ViewLogs.API.Constants
{
    public static class DefaultVariables
    {
        public const int PageSize = 50;

        public static CacheItemOptions CacheItemOptions
        {
            get
            {
                return new CacheItemOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(1)
                };
            }
        }
    }
}
