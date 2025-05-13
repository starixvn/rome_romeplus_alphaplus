using AlphaPlus.CashService.API.Criteria.Common;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

using DTOs.Common;

public interface ICommonService
{
    Task<IEnumerable<string>> GetCountryCodes(string siteName);

    Task<SiteDbConfig> GetServerDBConfigByCustIdAsync(int custId);
}

public class CommonService(IDynamicRepository repository) : ICommonService
{
    public async Task<IEnumerable<string>> GetCountryCodes(string siteName)
    {
        var criteria = new GetCountryCodesCriteria(siteName);
        var result = await repository.FetchAsync<string>(criteria);
        return result;
    }

    public async Task<SiteDbConfig> GetServerDBConfigByCustIdAsync(int custId)
    {
        var criteria = new GetServerDBConfigByCustIdCriteria { CustId = custId };
        var siteDbConfig = await repository.GetAsync<SiteDbConfig>(criteria);
        return siteDbConfig;
    }
}