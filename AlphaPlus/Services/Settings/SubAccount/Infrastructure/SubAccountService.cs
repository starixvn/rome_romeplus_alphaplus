using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Domain;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Infrastruture;

public interface ISubAccountService
{
    public Task<IEnumerable<SubAccountModel>> GetSubAccounts(int custId);

    public Task<SubAccountModel> GetSubAccount(int subAccId);

    public Task<Result> UpdateSubAccountInfo(SubAccountModel subAccountModel);

    public Task<Result> UpdateSubAccountPassword(int subAccId, string password);

    public Task<Result> UpdateSubAccountSecurityCode(int subAccId, string securityCode);

    public Task<Result> UpdateSubAccountPageAccess(int subAccId, string pageAccess);

    public Task<Result> RemoveSubAccountOtp(int subAccId);

    public Task<Result> AddSubAccount(SubAccountModel subAccountModel);
}

public class SubAccountService : ISubAccountService
{
    private readonly IDynamicRepository dynamicRepository;

    public SubAccountService(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public Task<Result> AddSubAccount(SubAccountModel subAccountModel)
    {
        throw new NotImplementedException();
    }

    public async Task<SubAccountModel> GetSubAccount(int subAccId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<SubAccountModel>> GetSubAccounts(int custId)
    {
        var result = await dynamicRepository.FetchAsync<SubAccountModel>(new SubAccountListCriteria{ CustId = custId });
        return result;
    }

    public async Task<Result> RemoveSubAccountOtp(int subAccId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateSubAccountInfo(SubAccountModel subAccountModel)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateSubAccountPageAccess(int subAccId, string pageAccess)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateSubAccountPassword(int subAccId, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateSubAccountSecurityCode(int subAccId, string securityCode)
    {
        throw new NotImplementedException();
    }
}
