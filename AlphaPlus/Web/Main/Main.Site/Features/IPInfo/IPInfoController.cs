using System.Net;
using AlphaPlus.Core;
using AlphaPlus.Utilities.IP;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Main.Site.Features.IPInfo
{
    public class IPInfoController(IPService ipService) : BaseController
    {
        private readonly IPService ipService = ipService;

        public async Task<ActionResult> Index(string ip)
        {
            if (IsValidIP(ip))
            {
                var ipInfo = await ipService.GetIPInformation(ip);
                return View(new IPInfoViewModel(ip, true, ipInfo));
            }

            return View(new IPInfoViewModel(ip, false));
        }

        private static bool IsValidIP(string ip) => IPAddress.TryParse(ip, out _);
    }
}