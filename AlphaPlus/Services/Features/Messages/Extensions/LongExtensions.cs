using System;

namespace AlphaPlus.Features.Messages.API.Extensions
{
    public static class LongExtensions
    {
        public static DateTime ToLocalTime(this long value)
            => DateTimeOffset.FromUnixTimeMilliseconds(value).DateTime.ToLocalTime();
    }
}