using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth.SiteDb
{
    public interface ISiteDBService
    {
        Task<SiteDBConfig> GetSiteDBConfig(int custId);
    }

    public class SiteDBService(IDynamicRepository dynamicRepository) : ISiteDBService
    {
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        public Task<SiteDBConfig> GetSiteDBConfig(int custId)
            => dynamicRepository.GetAsync<SiteDBConfig>(new GetSiteDBConfigCriteria(custId));
    }
}