using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.Default.MenuBar
{
    public class MenuBarQuery : IRequest<MenuBarViewModel>
    {
        private readonly List<string> devicesUseCustomScrollbar;

        public MenuBarQuery(
            byte roleId,
            string userAgent,
            List<string> devicesUseCustomScrollbar = null,
            string darkModeName = null,
            bool isHideBanner = false)
        {
            UserLevel = Enumeration.FromValue<UserLevel>(roleId);
            UserAgent = userAgent;
            this.devicesUseCustomScrollbar = devicesUseCustomScrollbar ?? [.. AppSettings.DevicesUseCustomScrollbar];
            DarkModeName = darkModeName;
            IsHideBanner = isHideBanner;
        }

        public UserLevel UserLevel { get; }

        public string UserAgent { get; }

        public bool IsSmallDevice
        {
            get
            {
                var strUserAgent = (UserAgent ?? string.Empty).ToLower();
                return devicesUseCustomScrollbar.Exists(str => strUserAgent.Contains(str.ToLower())) || strUserAgent.Contains("version");
            }
        }

        // TODO
        // public bool IsCookieLess => HttpContext.Current != null ? HttpContext.Current.Session.IsCookieless : false;
        public bool IsCookieLess => false;

        public string DarkModeName { get; }

        public bool IsHideBanner { get; }
    }
}