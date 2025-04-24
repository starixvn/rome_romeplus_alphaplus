namespace AlphaPlus.Settings.ViewLogs.API.Extensions
{
    public static class DecimalExtension
    {
        public static string ToNumericString(this decimal value)
        {
            return value.ToString($"#,##0.##");
        }
    }
}
