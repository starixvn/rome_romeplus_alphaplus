namespace AlphaPlus.Reports.Features.Shared.Extensions;

public static class NumberExtensions
{
    public static string FormatNumber(this int number)
        => number.ToString("N0");

    public static string FormatNumber(this decimal number)
        => number.ToString("N2");

    public static string FormatNumber4(this decimal number)
        => number.ToString("N4");

    public static string FormatNullableNumber(this decimal? number)
        => number.HasValue ? number.Value.FormatNumber() : string.Empty;
}