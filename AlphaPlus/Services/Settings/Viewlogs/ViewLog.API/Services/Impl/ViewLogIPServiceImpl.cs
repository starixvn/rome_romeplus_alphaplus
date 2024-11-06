using System.Net;
using System.Text.RegularExpressions;
using AlphaPlus.Core.Models;
using AlphaPlus.Utilities.IP;

namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl
{
    public class ViewLogIPServiceImpl : ViewLogIPService
    {
        private const string IPValidateRegex = @"(?:(?:25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(?:25[0-5]|2[0-4]\d|[01]?\d\d?)";


        private readonly IUserSession userSession;
        private readonly IPService ipService;

        public ViewLogIPServiceImpl(IUserSession userSession,
            IPService ipService)
        {
            this.ipService = ipService;
            this.userSession = userSession;
        }

        public string FormatIP(string ip)
        {
            if (!IsValidIP(ip))
            {
                return ip;
            }

            Regex regex = new(IPValidateRegex);
            if (!regex.IsMatch(ip, 0))
            {
                return string.Format("<a class=\"iplink\" href=\"javascript:;\">{0}</a>", ip);
            }

            if (ipService.IsVNIP(ip)
                && !userSession.IsAdminCanViewVNIP)
            {
                return string.Empty;
            }

            return string.Format("<a class=\"iplink\" href=\"javascript:OpenIPInfo('{0}');\">{0}</a>", ip);
        }

        public string FormatCountryCode(string ip, string country)
        {
            var isShowIP = IsValidIP(ip)
                && (!ipService.IsVNIP(ip) || userSession.IsAdminCanViewVNIP);

            return isShowIP ? country : string.Empty;
        }

        private static bool IsValidIP(string ipAddress)
        {
            return !string.IsNullOrWhiteSpace(ipAddress)
                && IPAddress.TryParse(ipAddress, out _);
        }
    }
}