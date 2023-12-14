using Alpha.Main.Models.Banner;
using Alpha.Main.Services;
using Alpha.Main.Site.Requests.Banner;
using Alpha.Main.Site.ViewModels.Banner;
using MediatR;

namespace Alpha.Main.Site.Handlers
{
    public class BannerHandler(BannerService bannerService) : IRequestHandler<BannerQuery, BannerViewModel>
    {
        private readonly BannerService bannerService = bannerService;

        public async Task<BannerViewModel> Handle(BannerQuery request, CancellationToken cancellationToken)
        {
            var banner = await bannerService.GetBanner(request.Language, request.Type);

            return new BannerViewModel(request.Type, request.Language, ToBase64String(banner));
        }

        private static string ToBase64String(BannerItem banner)
            => banner == null || banner.ImageContent == null || banner.ImageContent.Length == 0
            ? string.Empty
            : $"data:image/png;base64,{Convert.ToBase64String(banner.ImageContent)}";
    }
}