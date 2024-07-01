namespace AlphaPlus.Core.Extensions;

public static class DateTimeExtensions
{
    private const int NumberOfWeek = 7;
    private const int MonthsInYear = 12;
    private const string ShortDate = "MMddyyyy";
    private const string ShortDateWithSlash = "MM/dd/yyyy";
    private const string LongDateTimeWithSlash = "MM/dd/yyyy hh:mm:ss tt";
    private const string ShortMonthNameAndShortYear = "MMM-yy";
    private const string ShortMonthNameAndYear = "MMM-yyyy";

    public static string ToShortDate(this DateTime dateTime)
        => dateTime.ToString(ShortDate);

    public static string ToShortDateWithSlash(this DateTime dateTime)
        => dateTime.ToString(ShortDateWithSlash);

    public static string ToLongDateTimeWithSlash(this DateTime dateTime)
        => dateTime.ToString(LongDateTimeWithSlash);

    public static string ToShortMonthNameAndShortYear(this DateTime dateTime)
        => dateTime.ToString(ShortMonthNameAndShortYear);

    public static string ToShortMonthNameAndYear(this DateTime dateTime)
        => dateTime.ToString(ShortMonthNameAndYear);

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

    public static DateTime LastDayOfMonth(this DateTime dateTime)
        => new DateTime(dateTime.Year, dateTime.Month, dateTime.DaysInMonth());

    public static bool IsSameMonthWith(this DateTime dateTime, DateTime otherDateTime)
        => dateTime.Month == otherDateTime.Month && dateTime.Year == otherDateTime.Year;

    public static bool IsEndOfMonth(this DateTime dateTime)
        => dateTime.Day == DaysInMonth(dateTime);

    public static int GetMonthDiff(this DateTime dateTime, DateTime otherDateTime)
        => dateTime.Month - otherDateTime.Month + (MonthsInYear * (dateTime.Year - otherDateTime.Year));

    public static DateTime FirstDayOfYear(this DateTime dateTime)
        => new DateTime(dateTime.Year, 1, 1);

    public static DateTime LastDayOfYear(this DateTime dateTime)
        => new DateTime(dateTime.Year, 12, 31);

    public static DateTime GetGreaterDate(this DateTime fromDate, DateTime limitedDate)
        => fromDate < limitedDate ? limitedDate : fromDate;

    private static int DaysInMonth(this DateTime dateTime)
        => DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
}