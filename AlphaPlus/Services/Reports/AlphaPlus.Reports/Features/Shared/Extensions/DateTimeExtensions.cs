namespace AlphaPlus.Reports.Features.Shared.Extensions;

public static class DateTimeExtensions
{
    private const string MMDDWithSlash = "MM/dd";
    private const string ShortDateWithSlash = "MM/dd/yyyy";
    private const string LongDateWithSlash = "MM/dd/yyyy hh:mm:ss tt";
    private const string LongDate24HourWithSlash = "MM/dd/yyyy HH:mm:ss";
    private const string LongDateWithoutSecondWithSlash = "MM/dd/yyyy hh:mm tt";
    private const string ShortMonthNameAndShortYear = "MMM-yy";
    private const string ShortMonthNameAndYear = "MMM-yyyy";

    private const int MonthsInYear = 12;

    public static string ToMMDDWithSlash(this DateTime dateTime)
        => dateTime.ToString(MMDDWithSlash);

    public static string ToShortDateWithSlash(this DateTime dateTime)
        => dateTime.ToString(ShortDateWithSlash);

    public static string ToLongDateWithSlash(this DateTime dateTime)
        => dateTime.ToString(LongDateWithSlash);

    public static string ToLongDate24HourWithSlash(this DateTime dateTime)
        => dateTime.ToString(LongDate24HourWithSlash);

    public static string ToLongDateWithoutSecondWithSlash(this DateTime dateTime)
        => dateTime.ToString(LongDateWithoutSecondWithSlash);

    public static string ToShortMonthNameAndShortYear(this DateTime dateTime)
        => dateTime.ToString(ShortMonthNameAndShortYear);

    public static string ToShortMonthNameAndYear(this DateTime dateTime)
        => dateTime.ToString(ShortMonthNameAndYear);

    public static DateTime Yesterday(this DateTime dateTime)
        => dateTime.AddDays(-1);

    public static DateTime OneWeekAgo(this DateTime dateTime)
        => dateTime.AddDays(-7);

    public static DateTime FirstDayOfMonth(this DateTime dateTime)
        => new DateTime(dateTime.Year, dateTime.Month, 1);

    public static DateTime FirstDayOf3MonthsAgo(this DateTime dateTime)
        => dateTime.AddMonths(-3).FirstDayOfMonth();

    public static DateTime LastDayOfMonth(this DateTime dateTime)
        => new DateTime(dateTime.Year, dateTime.Month, dateTime.DaysInMonth());

    public static bool IsSameMonthWith(this DateTime dateTime, DateTime otherDateTime)
        => dateTime.Month == otherDateTime.Month && dateTime.Year == otherDateTime.Year;

    public static bool IsEndOfMonth(this DateTime dateTime)
        => dateTime.Day == DaysInMonth(dateTime);

    private static int DaysInMonth(this DateTime dateTime)
        => DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

    public static DateTime FirstDayOfYears(this DateTime dateTime)
        => new DateTime(dateTime.Year, 1, 1);

    public static DateTime FirstDayOf3YearsAgo(this DateTime dateTime)
        => dateTime.AddYears(-3).FirstDayOfYears();

    public static DateTime LastDayOfYear(this DateTime dateTime)
        => new DateTime(dateTime.Year, 12, 31);

    public static int GetMonthDiff(this DateTime dateTime, DateTime otherDateTime)
        => dateTime.Month - otherDateTime.Month + (MonthsInYear * (dateTime.Year - otherDateTime.Year));

    public static DateTime GetGreaterDate(this DateTime fromDate, DateTime limitedDate)
        => fromDate < limitedDate ? limitedDate : fromDate;
}