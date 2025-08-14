using MediatR;

namespace AlphaPlus.Main.Site.Features.Dashboard.Banner
{
    public class BannerHandler(IBannerService bannerService) : IRequestHandler<BannerClickLogRequest, bool>
    {
        private readonly IBannerService bannerService = bannerService;

        public Task<bool> Handle(BannerClickLogRequest request, CancellationToken cancellationToken)
            => bannerService.LogClick(request.BannerName, request.RoleId);
    }
}