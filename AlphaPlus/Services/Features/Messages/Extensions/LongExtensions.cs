using System;

namespace AlphaPlus.Features.Messages.Api.Extensions
{
    public static class LongExtensions
    {
        public static DateTime ToLocalTime(this long value)
            => DateTimeOffset.FromUnixTimeMilliseconds(value).DateTime.ToLocalTime();
    }
}