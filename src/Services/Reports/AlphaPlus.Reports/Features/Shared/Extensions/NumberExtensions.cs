using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Features.Shared.Extensions;

public static class NumberExtensions
{
    public const string NotAvailable = "N/A";

    private const int Percentage = 100;

    public static string FormatNumber(this int number)
        => number.ToString("N0");

    public static string FormatNumber(this decimal number)
        => number.ToString("N2");

    public static string FormatNumber4(this decimal number)
        => number.ToString("N4");

    public static string FormatPTPercentage(this decimal pt)
    {
        var ptPercentage = (pt * Percentage).ToString("N1");

        return $"{ptPercentage}% {SharedResources.PT}";
    }

    public static string FormatNumber(this decimal? numberValue)
    => numberValue.HasValue ? FormatNumber(numberValue.Value) : NotAvailable;

    public static decimal RoundTo2(this decimal value) => Math.Round(value, 2);
}