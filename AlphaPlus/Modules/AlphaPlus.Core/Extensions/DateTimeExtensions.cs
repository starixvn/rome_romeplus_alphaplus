namespace AlphaPlus.Core.Extensions;

public static class DateTimeExtensions
{
    private const int NumberOfWeek = 7;
    private const string ShortDate = "MMddyyyy";
    private const string ShortDateWithSlash = "MM/dd/yyyy";
    private const string LongDateTimeWithSlash = "MM/dd/yyyy hh:mm:ss tt";

    public static string ToShortDate(this DateTime dateTime)
        => dateTime.ToString(ShortDate);

    public static string ToShortDateWithSlash(this DateTime dateTime)
        => dateTime.ToString(ShortDateWithSlash);

    public static string ToLongDateTimeWithSlash(this DateTime dateTime)
        => dateTime.ToString(LongDateTimeWithSlash);

    public static DateTime ToStartOfDay(this DateTime dateTime)
        => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);

    public static DateTime ToEndOfDay(this DateTime dateTime)
        => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);

    public static DateTime StartOfWeek(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
    {
        int diff = (NumberOfWeek + (date.DayOfWeek - startOfWeek)) % NumberOfWeek;

        return date.AddDays(-1 * diff).Date;
    }

    public static DateTime FirstDayOfMonth(this DateTime dateTime)
        => new DateTime(dateTime.Year, dateTime.Month, 1);

    public static DateTime FirstDayOfYear(this DateTime dateTime)
        => new DateTime(dateTime.Year, 1, 1);

    public static DateTime GetGreaterDate(this DateTime fromDate, DateTime limitedDate)
        => fromDate < limitedDate ? limitedDate : fromDate;
}