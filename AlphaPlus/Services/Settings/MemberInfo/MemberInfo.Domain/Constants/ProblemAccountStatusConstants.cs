namespace AlphaPlus.Settings.MemberInfo.Domain.Constants
{
    public static class ProblemAccountStatusConstants
    {
        public const int Open = 1;
        public const int Suspended = 2;
        public const int Closed = 3;
        public const int Disabled = 4;

        public static IDictionary<byte, string> Items { get; } = new Dictionary<byte, string>
        {
            [Open] = nameof(Open),
            [Suspended] = nameof(Suspended),
            [Closed] = nameof(Closed),
            [Disabled] = nameof(Disabled)
        };

        public static string GetProblemAccountStatus(byte key)
            => Items.ContainsKey(key) ? Items[key] : string.Empty;
    }
}
