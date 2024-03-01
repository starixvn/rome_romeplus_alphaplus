namespace AlphaPlus.Reports.Features.Shared.Extensions;

public static class DateTimeExtensions
{
    private const string MMDDWithSlash = "MM/dd";
    private const string ShortDateWithSlash = "MM/dd/yyyy";

    public static string ToMMDDWithSlash(this DateTime dateTime)
        => dateTime.ToString(MMDDWithSlash);

    public static string ToShortDateWithSlash(this DateTime dateTime)
        => dateTime.ToString(ShortDateWithSlash);

    public static DateTime Yesterday(this DateTime dateTime)
        => dateTime.AddDays(-1);

    public static DateTime FirstDayOfMonth(this DateTime dateTime)
        => new DateTime(dateTime.Year, dateTime.Month, 1);

    public static DateTime FirstDayOf3MonthsAgo(this DateTime dateTime)
        => dateTime.AddMonths(-3).FirstDayOfMonth();

    public static bool IsSameMonthWith(this DateTime dateTime, DateTime otherDateTime)
        => dateTime.Month == otherDateTime.Month && dateTime.Year == otherDateTime.Year;

    public static DateTime FirstDayOfYears(this DateTime dateTime)
        => new DateTime(dateTime.Year, 1, 1);

    public static DateTime FirstDayOf3YearsAgo(this DateTime dateTime)
        => dateTime.AddYears(-3).FirstDayOfYears();

    public static DateTime GetGreaterDate(this DateTime fromDate, DateTime limitedDate)
        => fromDate < limitedDate ? limitedDate : fromDate;
}