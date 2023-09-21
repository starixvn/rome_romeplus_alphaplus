using System;
using System.Web;
using Fanex.Logging.Extensions.Web;

namespace Alpha.Web.Helpers
{
    public class IPHelper
    {
        public static string[] GetClientIPs()
        {
            var request = HttpContext.Current.Request;
            var actualIP = request.Headers["X-Actual-IP"];

            actualIP = string.IsNullOrWhiteSpace(actualIP) ?
                request.ServerVariables["HTTP_X_FORWARDED_FOR"] : actualIP;

            actualIP = string.IsNullOrWhiteSpace(actualIP) ?
                request.ServerVariables["REMOTE_ADDR"] : actualIP;

            return string.IsNullOrWhiteSpace(actualIP) || actualIP.Equals("localhost") || actualIP.Equals("::1") ?
                new[] { "127.0.0.1" } : actualIP.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string GetClientIP()
        {
            string[] ips = GetClientIPs();
            return ips[0].Trim();
        }
    }
}