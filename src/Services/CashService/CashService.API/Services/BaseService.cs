using AlphaPlus.CashService.API.Criteria.Payments;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

public interface IBaseService
{
    Task<IEnumerable<string>> GetCountryCodes(string siteName);
}

public class BaseService(IDynamicRepository repository) : IBaseService
{
    public async Task<IEnumerable<string>> GetCountryCodes(string siteName)
    {
        var criteria = new GetCountryCodesCriteria(siteName);
        var result = await repository.FetchAsync<string>(criteria);
        return result;
    }
}