namespace AlphaPlus.Settings.ViewLogs.API.Helpers
{
    using System;
    using AlphaPlus.Shared.AppResources.Settings.ViewLog;

    public static class DateTimeHelper
    {
        private const string LongDateWithSlash = "MM/dd/yyyy hh:mm:ss tt";
        private const string HtmlLongDateWithSlash = "MM/dd/yyyy<br/>hh:mm:ss tt";

        public static string[] DaysInWeek()
        {
            return
            [
                Resources.Sun,
                Resources.Mon,
                Resources.Tue,
                Resources.Wed,
                Resources.Thurs,
                Resources.Fri,
                Resources.Sat
            ];
        }

        public static DateTime GetBeginningOfDate(DateTime dateTime)
        {
            var beginningDateTime = dateTime.ToShortDateString() + " 00:00:00 AM";

            return Convert.ToDateTime(beginningDateTime);
        }

        public static DateTime GetEndingOfDate(DateTime dateTime)
        {
            var endingDateTime = dateTime.ToShortDateString() + " 23:59:59 PM";

            return Convert.ToDateTime(endingDateTime);
        }

        public static string ToLongDateWithSlash(DateTime dateTime)
        {
            return dateTime.ToString(LongDateWithSlash);
        }

        public static string ToHtmlLongDateWithSlash(DateTime dateTime)
        {
            return dateTime.ToString(HtmlLongDateWithSlash);
        }
    }
}
