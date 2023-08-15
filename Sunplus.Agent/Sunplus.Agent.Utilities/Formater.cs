namespace Sunplus.Agent.Utilities
{
    using System;
    using System.Globalization;

    public class Formater
    {
        private static readonly CultureInfo cult = new CultureInfo("en-US");

        public static string ShortDateFormat(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null) return string.Empty;
            return string.Format(cult, "{0:MM/dd/yyyy}", dt);
        }
    }
}