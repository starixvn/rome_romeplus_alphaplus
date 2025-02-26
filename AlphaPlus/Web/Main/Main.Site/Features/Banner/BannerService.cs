using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.Banner
{
    public interface IBannerService
    {
        Task<BannerItem> GetBanner(string languageCode, byte type);
    }

    public class BannerService(IDynamicRepository dynamicRepository) : IBannerService
    {
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        public Task<BannerItem> GetBanner(string languageCode, byte type)
            => dynamicRepository.GetAsync<BannerItem>(
                new GetBannerCriteria(languageCode, AppSettings.BannerEnvironment, AppSettings.SiteLabel, type));
    }
}