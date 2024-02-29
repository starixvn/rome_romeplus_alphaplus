namespace AlphaPlus.Settings.MemberInfo.Domain.Constants
{
    using System.Collections.Generic;

    public static class ReachedThresholdLimitTypeConstants
    {
        public const byte All = 0;
        public const byte Global = 1;
        public const byte Personal = 2;

        public static IDictionary<byte, string> Items { get; } = new Dictionary<byte, string>
        {
            [All] = nameof(All),
            [Global] = nameof(Global),
            [Personal] = nameof(Personal)
        };

        public static string GetReachedThresholdLimitType(byte key)
            => Items.ContainsKey(key) ? Items[key] : string.Empty;
    }
}
