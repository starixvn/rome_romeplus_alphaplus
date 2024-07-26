using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.API.Criteria;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Services;

public interface IAccountService
{
    string GetTransferOption(int custId, int roleId);

    Task<IEnumerable<CustInfo>> GetMasterList(int custId);

    Task<IEnumerable<CustInfo>> GetAgentList(int custId);
}

public class AccountService(IDynamicRepository repository) : IAccountService
{
    private readonly IDynamicRepository repository = repository;
    private const string DailyTransferCondition = "00000000";

    public string GetTransferOption(int custId, int roleId)
    {
        if (roleId == UserLevel.Super.Value)
        {
            return DailyTransferCondition;
        }

        var criteria = new GetTransferOptionCriteria(custId);
        repository.Execute(criteria);

        return criteria.TransferOption;
    }

    public Task<IEnumerable<CustInfo>> GetMasterList(int custId)
        => repository.FetchAsync<CustInfo>(new GetMasterListCriteria(custId));

    public Task<IEnumerable<CustInfo>> GetAgentList(int custId)
        => repository.FetchAsync<CustInfo>(new GetAgentListCriteria(custId));
}
