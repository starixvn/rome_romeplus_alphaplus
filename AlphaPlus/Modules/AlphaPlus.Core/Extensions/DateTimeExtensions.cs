namespace AlphaPlus.Core.Extensions;

public static class DateTimeExtensions
{
    private const string ShortDateWithSlash = "MM/dd/yyyy";

    public static string ToShortDateWithSlash(this DateTime dateTime)
        => dateTime.ToString(ShortDateWithSlash);
}