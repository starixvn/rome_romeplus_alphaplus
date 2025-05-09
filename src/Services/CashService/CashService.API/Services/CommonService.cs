using AlphaPlus.CashService.API.Criteria.Common;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

public interface ICommonService
{
    Task<IEnumerable<string>> GetCountryCodes(string siteName);
}

public class CommonService(IDynamicRepository repository) : ICommonService
{
    public async Task<IEnumerable<string>> GetCountryCodes(string siteName)
    {
        var criteria = new GetCountryCodesCriteria(siteName);
        var result = await repository.FetchAsync<string>(criteria);
        return result;
    }
}