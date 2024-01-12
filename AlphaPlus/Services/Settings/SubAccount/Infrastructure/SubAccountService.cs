using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Domain;

namespace AlphaPlus.Settings.Infrastruture;

public interface ISubAccountService
{
    public Task<IEnumerable<SubAccountModel>> GetSubAccountList(int custId);

    public Task<SubAccountModel> GetSubAccount(int subAccId);

    public Task<Result> Update(SubAccountModel subAccountModel);
}

public class SubAccountService : ISubAccountService
{
    public async Task<SubAccountModel> GetSubAccount(int subAccId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<SubAccountModel>> GetSubAccountList(int custId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> Update(SubAccountModel subAccountModel)
    {
        throw new NotImplementedException();
    }

}
