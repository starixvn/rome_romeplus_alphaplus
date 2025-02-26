namespace AlphaPlus.Settings.MemberInfo.Domain.Constants
{
    public static class ReachedThresholdStatusConstants
    {
        public const byte All = 0;
        public const byte Open = 1;
        public const byte Suspended = 2;
        public const byte Closed = 3;
        public const byte Disabled = 4;
        public const byte Locked = 5;

        public static IDictionary<byte, string> Items { get; } = new Dictionary<byte, string>
        {
            [All] = nameof(All),
            [Open] = nameof(Open),
            [Suspended] = nameof(Suspended),
            [Closed] = nameof(Closed),
            [Disabled] = nameof(Disabled),
            [Locked] = nameof(Locked),
        };

        public static string GetReachedThresholdStatus(byte key)
            => Items.TryGetValue(key, out var status) ? status : string.Empty;
    }
}
