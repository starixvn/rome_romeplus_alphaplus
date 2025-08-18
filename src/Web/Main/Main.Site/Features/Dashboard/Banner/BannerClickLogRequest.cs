using MediatR;

namespace AlphaPlus.Main.Site.Features.Dashboard.Banner
{
    public class BannerClickLogRequest(string bannerName, byte roleId) : IRequest<bool>
    {
        public string BannerName { get; } = bannerName;

        public byte RoleId { get; } = roleId;
    }
}