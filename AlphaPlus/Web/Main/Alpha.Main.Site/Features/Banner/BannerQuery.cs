namespace Alpha.Main.Site.Features.Banner
{
    using AlphaPlus.Core.Models;
    using MediatR;

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