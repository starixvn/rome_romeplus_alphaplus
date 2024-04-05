namespace AlphaPlus.Features.Messages.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        private const string ShortDateWithSlash = "MM/dd/yyyy";
        private const string LongTime = "hh:mm:ss tt";

        public static string ToShortDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(ShortDateWithSlash);

        public static string ToLongTime(this DateTime dateTime)
            => dateTime.ToString(LongTime);

        public static bool IsTodate(this DateTime dateTime)
        {
            var currentDate = DateTime.Now.Date;
            return dateTime.Date == currentDate.Date
                && dateTime.Month == currentDate.Month
                && dateTime.Year == currentDate.Year;
        }
    }
}