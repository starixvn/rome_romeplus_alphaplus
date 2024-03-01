namespace Alpha.MemberInfo.Site.Helpers
{
    using System;
    using System.Diagnostics;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.IndirectLoginAccount;
    using Fanex.Logging.Extensions.Web;
    using UAParser;

    public static class RequestInfoHelper
    {
        public static RequestInfo GetRequestInfo(HttpRequest request, string ip, string browserSize, string countryCode)
        {
            var browserName = string.Empty;
            var platform = string.Empty;

            try
            {
                var userAgentParser = Parser.GetDefault();
                var clientInfo = userAgentParser.Parse(request.Headers.UserAgent[0].ToString() ?? string.Empty);

                browserName = GetBrowserName(clientInfo.UserAgent, browserSize);
                platform = GetPlatform(clientInfo);
            }
            catch
            {
                ExceptionHandler.Handle(new Exception("Cannot get request info"), TraceEventType.Information);
            }

            return new RequestInfo(
                ip,
                request.Headers["SERVER_NAME"],//TODO
                browserName,
                platform,
                countryCode);
        }
        
        private static string GetBrowserName(UserAgent userAgent, string browserSize)
        {
            var browser = $"{userAgent.Family} {userAgent.Major}.{userAgent.Minor}.{userAgent.Patch ?? "0"}";

            return string.IsNullOrWhiteSpace(browserSize)
                ? browser
                : browser + $", {browserSize}";
        }

        private static string GetPlatform(ClientInfo clientInfo)
        {
            var deviceInfo = clientInfo.Device;
            var device = $"{deviceInfo.Family}, IsCrawler: {deviceInfo.IsSpider}";

            var os = clientInfo.OS;
            var majorVersion = os.Major ?? "0";
            var minorVersion = os.Minor ?? "0";
            var osVersion = os.Patch ?? "0";
            var operateSystem = $"{os.Family} {majorVersion}.{minorVersion}.{osVersion}";

            return $"{operateSystem}, Device: {device}";
        }
    }
}