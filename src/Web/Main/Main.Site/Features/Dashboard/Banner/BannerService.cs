using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.Dashboard.Banner
{
    public interface IBannerService
    {
        Task<bool> LogClick(string bannerName, byte roleId);
    }

    public class BannerService(IDynamicRepository dynamicRepository) : IBannerService
    {
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        public async Task<bool> LogClick(string bannerName, byte roleId)
        {
            await dynamicRepository.ExecuteAsync(new BannerClickLogCommand(bannerName, roleId));
            return true;
        }
    }
}