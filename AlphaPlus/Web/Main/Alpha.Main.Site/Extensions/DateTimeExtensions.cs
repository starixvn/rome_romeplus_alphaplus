namespace Alpha.Main.Site.Extensions
{
    public static class DateTimeExtensions
    {
        private const string LongDateWithSlash = "dd/MM/yyyy hh:mm tt";

        public static string ToLongDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(LongDateWithSlash);
    }
}
