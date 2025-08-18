namespace AlphaPlus.Settings.ViewLogs.API.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Eg: 0.6000 => 0.6
        /// 1.A "number string" which (has real part != 0 and starts with 0) (example: 02.300) is a string, not a number
        /// => in this case, we return 02.300 (this is same with 01, 02, ...)
        /// 2.A "number string" which (has real part == 0 and not starts with 0.) (example: 00.300 or 00) is a string
        /// -Above rules affect to Race/Transfer Condition field which maybe starts with 0 and real part != 0 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveTrailingZeros(string value)
        {
            double numberValue;
            if(string.IsNullOrWhiteSpace(value)
                || value.IndexOf('.') == -1
                || !value.EndsWith("0")
                || !char.IsDigit(value[0])
                || !double.TryParse(value, out numberValue)
                || ((int)numberValue != 0 && value.StartsWith("0", StringComparison.OrdinalIgnoreCase))
                || ((int)numberValue == 0 && !value.StartsWith("0.", StringComparison.OrdinalIgnoreCase)))
            {
                return value;
            }

            return numberValue.ToString();
        }
    }
}
