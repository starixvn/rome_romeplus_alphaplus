using AlphaPlus.CashService.API.Criteria.Payments;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

public interface IBaseService
{
    Task<IEnumerable<string>> GetCountryCodes(int custId);
}

public class BaseService : IBaseService
{
    private readonly IDynamicRepository _repository;

    public BaseService(IDynamicRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<string>> GetCountryCodes(int custId)
    {
        var criteria = new GetCountryCodesCriteria(custId);
        var result = await _repository.FetchAsync<string>(criteria);
        return result;
    }
} 