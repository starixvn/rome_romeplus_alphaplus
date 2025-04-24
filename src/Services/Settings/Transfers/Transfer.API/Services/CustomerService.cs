using AlphaPlus.Settings.Transfer.API.Criteria;
using AlphaPlus.Settings.Transfer.API.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Services;

public interface ICustomerService
{
    Task<CustInfo> GetCustInfo(string username, int actorId);

    Task<bool> CheckMultipleBelonging(int actorId, int subAccId, string custIds);
}

public class CustomerService(IDynamicRepository repository) : ICustomerService
{
    private readonly IDynamicRepository repository = repository;

    public Task<CustInfo> GetCustInfo(string username, int actorId)
        => repository.GetAsync<CustInfo>(new GetCustInfoCriteria(username, actorId));

    public async Task<bool> CheckMultipleBelonging(int actorId, int subAccId, string custIds)
    {
        var criteria = new CheckMultipleCustomesBelongingCriteria(actorId, subAccId, custIds);
        await repository.ExecuteAsync(criteria);

        return criteria.RoleId > 0 && criteria.IsActive;
    }
}