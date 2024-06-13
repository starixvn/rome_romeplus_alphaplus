namespace AlphaPlus.Core.Extensions;

public static class DateTimeExtensions
{
    private const string ShortDateWithSlash = "MM/dd/yyyy";

    public static string ToShortDateWithSlash(this DateTime dateTime)
        => dateTime.ToString(ShortDateWithSlash);

    public static DateTime GetGreaterDate(this DateTime fromDate, DateTime limitedDate)
        => fromDate < limitedDate ? limitedDate : fromDate;

    public static DateTime FirstDayOfMonth(this DateTime dateTime)
        => new DateTime(dateTime.Year, dateTime.Month, 1);
}