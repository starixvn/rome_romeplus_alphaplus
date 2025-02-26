namespace AlphaPlus.Settings.MemberInfo.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        private const string ShortDateWithSlash = "MM/dd/yyyy";
        private const string LongDateTime24h = "MM/dd/yyyy HH:mm";
        private const string ShortDateWithHyphen = "yyyy-MM-dd";
        private const string LongDateTime12h = "MM/dd/yyyy hh:mm tt";
        private const string LongDateTime12hWithHyphen = "MM/dd/yyyy hh:mm:ss tt";
        private const string MMDDWithSlash = "MM/dd";

        public static string ToShortDateWithHyphen(this DateTime dateTime)
            => dateTime.ToString(ShortDateWithHyphen);

        public static string ToShortDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(ShortDateWithSlash);

        public static string ToLongDateTime24h(this DateTime dateTime)
            => dateTime.ToString(LongDateTime24h);

        public static string ToLongDateTime12h(this DateTime dateTime)
            => dateTime.ToString(LongDateTime12h);

        public static string ToLongDateTime12h(this DateTime? dateTime)
            => dateTime.HasValue ? dateTime.Value.ToLongDateTime12h() : string.Empty;

        public static string ToLongDateTime12hWithHyphen(this DateTime dateTime)
            => dateTime.ToString(LongDateTime12hWithHyphen);

        public static DateTime GetGreaterDate(this DateTime fromDate, DateTime limitedDate)
         => fromDate < limitedDate ? limitedDate : fromDate;

        public static string ToMMDDWithSlash(this DateTime dateTime)
            => dateTime.ToString(MMDDWithSlash);

        public static DateTime FirstDayOfYear(this DateTime dateTime)
            => new(dateTime.Year, 1, 1);

        public static DateTime FirstDayOfLastMonth(this DateTime dateTime)
            => dateTime.FirstDayOfMonth().AddMonths(-1);

        public static DateTime FirstDayOfMonth(this DateTime dateTime)
            => new(dateTime.Year, dateTime.Month, 1);
    }
}
