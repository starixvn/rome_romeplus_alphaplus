namespace AlphaPlus.Reports.Features.Shared.Extensions;

public static class NumberExtensions
{
    public static string FormatNumber(this int number)
        => number.ToString("N0");

    public static string FormatNumber(this decimal number, int decimalPlace = 2)
        => number.ToString($"N{decimalPlace}");
}