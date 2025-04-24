namespace AlphaPlus.Settings.Site.Features.Shared
{
    public static class MemberStatisticDateRange
    {
        public const int Last30Days = 30;
        public const int Last60Days = 60;
        public const int Last90Days = 90;

        public static IDictionary<string, int> Items { get; } = new Dictionary<string, int>
        {
            [nameof(Last30Days)] = Last30Days,
            [nameof(Last60Days)] = Last60Days,
            [nameof(Last90Days)] = Last90Days
        };
    }
}