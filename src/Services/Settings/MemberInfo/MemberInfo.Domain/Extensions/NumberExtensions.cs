using System.Globalization;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;

namespace AlphaPlus.Settings.MemberInfo.Domain.Extensions
{
    public static class NumberExtensions
    {
        private const string moneyFormat = "#,##0.##";
        private const string moneyWithZeroTailFormat = "#,##0.00";

        public static string ToMoneyFormat(this decimal number)
            => number.ToString(moneyFormat, CultureInfo.InvariantCulture);

        public static string ToMoneyFormatWithZeroTail(this decimal number)
            => number.ToString(moneyWithZeroTailFormat, CultureInfo.InvariantCulture);

        public static string ToMoneyFormatWithUnlimited(this decimal? number)
           => number.HasValue
           ? number.Value.ToMoneyFormat()
           : Resources.Unlimited;
    }
}