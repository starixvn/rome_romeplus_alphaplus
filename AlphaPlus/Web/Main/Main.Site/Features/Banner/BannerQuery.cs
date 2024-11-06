using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.Banner
{
    public class BannerQuery : IRequest<BannerViewModel>
    {
        public BannerQuery() : this(null)
        {
        }

        internal BannerQuery(IUserSession userSession)
        {
            userSession ??= new UserSessionExt();
            Language = userSession.Language;
            IsHideBanner = AppSettings.BannerIsHide || userSession.IsProtectedMode;
        }

        public string Language { get; }

        public bool IsHideBanner { get; }

        public byte Type { get; set; }
    }
}