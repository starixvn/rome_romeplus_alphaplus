namespace AlphaPlus.Reports.Features.Shared.Extensions;

internal static class DateTimeExtensions
{
    private const string MMDDWithSlash = "MM/dd";
    private const string ShortDateWithSlash = "MM/dd/yyyy";
    private const string LongDateWithSlash = "MM/dd/yyyy hh:mm:ss tt";
    private const string LongDate24HourWithSlash = "MM/dd/yyyy HH:mm:ss";
    private const string LongDateWithoutSecondWithSlash = "MM/dd/yyyy hh:mm tt";

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
}