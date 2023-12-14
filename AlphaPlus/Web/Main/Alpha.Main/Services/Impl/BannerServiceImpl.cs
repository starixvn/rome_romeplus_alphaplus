namespace Alpha.Main.Services.Impl
{
    using System.Threading.Tasks;
    using Alpha.Main.Configurations;
    using Alpha.Main.Criteria.Banner;
    using Alpha.Main.Models.Banner;
    using Fanex.Data.Repository;

    public class BannerServiceImpl(IDynamicRepository dynamicRepository) : BannerService
    {
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        public Task<BannerItem> GetBanner(string languageCode, byte type)
            => dynamicRepository.GetAsync<BannerItem>(
                new GetBannerCriteria(languageCode, AppSettings.BannerEnvironment, AppSettings.SiteLabel, type));
    }
}
