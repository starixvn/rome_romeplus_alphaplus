using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Domain;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Infrastruture;

public interface ISubAccountService
{
    public Task<IEnumerable<SubAccountModel>> GetSubAccountList(int custId);

    public Task<SubAccountModel> GetSubAccount(int subAccId);

    public Task<Result> Update(SubAccountModel subAccountModel);
}

public class SubAccountService : ISubAccountService
{
    private readonly IDynamicRepository dynamicRepository;

    public SubAccountService(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<SubAccountModel> GetSubAccount(int subAccId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<SubAccountModel>> GetSubAccountList(int custId)
    {
        var result = await dynamicRepository.FetchAsync<SubAccountModel>(new SubAccountListCriteria{ CustId = custId });
        return result;
    }

    public async Task<Result> Update(SubAccountModel subAccountModel)
    {
        throw new NotImplementedException();
    }

}
