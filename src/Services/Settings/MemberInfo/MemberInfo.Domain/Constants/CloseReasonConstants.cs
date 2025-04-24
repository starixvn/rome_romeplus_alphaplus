namespace AlphaPlus.Settings.MemberInfo.Domain.Constants
{
    public static class CloseReasonConstants
    {
        public const int ByAdmin = 1;
        public const int ByUpline = 2;
        public const int ByArchive = 4;

        public static IDictionary<int, string> Items { get; } = new Dictionary<int, string>
        {
            [ByAdmin] = nameof(ByAdmin),
            [ByUpline] = nameof(ByUpline),
            [ByArchive] = nameof(ByArchive)
        };

        public static string GetName(int key)
            => Items.ContainsKey(key) ? Items[key] : string.Empty;
    }
}
