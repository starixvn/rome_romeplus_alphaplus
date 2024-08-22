namespace AlphaPlus.Settings.MemberInfo.Domain.Constants
{
    using System.Collections.Generic;

    public static class ReviewStatusConstant
    {
        public const int All = 2;
        public const int Reviewed = 1;
        public const int NotReviewed = 0;

        public static readonly IReadOnlyDictionary<int, string> Items = new Dictionary<int, string>
        {
            [All] = nameof(All),
            [Reviewed] = nameof(Reviewed),
            [NotReviewed] = nameof(NotReviewed)
        };

        public static string GetReviewStatus(int key)
            => Items.ContainsKey(key) ? Items[key] : Items[All];
    }
}
