namespace AlphaPlus.Core.Extensions;

public static class NumberExtensions
{
    /// <summary>
    /// Format 2 decimals if any. For example:
    /// 1234.567    --> 1,234.57
    /// 0.7         --> 0.7
    /// 0           --> 0
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string FormatNumber2(this decimal value) => value.ToString("#,##0.##");

    /// <summary>
    /// Always format 2 decimals. For example:
    /// 1234.567    --> 1,234.57
    /// 0.7         --> 0.70
    /// 0           --> 0.00
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string FormatNumber20(this decimal value) => value.ToString("N2");

    public static string FormatNumber4(this decimal value) => value.ToString("#,####0.####");

    public static string FormatNumber40(this decimal number) => number.ToString("N4");
}