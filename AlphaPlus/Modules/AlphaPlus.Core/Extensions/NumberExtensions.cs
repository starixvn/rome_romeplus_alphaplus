namespace AlphaPlus.Core.Extensions;

public static class NumberExtensions
{
    public static string FormatNumber2(this decimal value) => value.ToString("#,##0.##");
}