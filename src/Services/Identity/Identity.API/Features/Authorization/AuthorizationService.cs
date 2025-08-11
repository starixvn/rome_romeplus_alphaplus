using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Authorization;

public interface IAuthorizationService
{
    Task<CustomerStatus> GetCustomerStatus(int custId, int subAccId);

    Task<BelongingInfo> CheckBelonging(int custId, int subAccId, long transId, int downlineCustId);

    Task<BelongingInfo> CheckBelongingInDBCust(int custId, int subAccId, long transId, int downlineCustId);

    Task<DownlineInfo> CheckBelongingByUsername(int custId, int subAccId, string username);

    Task<BelongingInfo> CheckBelongingConsolidatedGroup(int custId, int subAccId, long transId, int downlineCustId);

    Task<BelongingInfo> CheckBelongingMultipleCustomers(int custId, int subAccId, string custIds);
}

public class AuthorizationService(IDynamicRepository dynamicRepository) : IAuthorizationService
{
    private readonly IDynamicRepository dynamicRepository = dynamicRepository;

    public async Task<CustomerStatus> GetCustomerStatus(int custId, int subAccId)
    {
        var command = new GetAccountStatusCommand(custId, subAccId);
        await dynamicRepository.ExecuteAsync(command);

        return new CustomerStatus(command.IsActive, command.Locked);
    }

    public Task<BelongingInfo> CheckBelonging(int custId, int subAccId, long transId, int downlineCustId)
        => CheckBelonging(new CheckBelongingCommand(custId, subAccId, transId, downlineCustId));

    public Task<BelongingInfo> CheckBelongingInDBCust(int custId, int subAccId, long transId, int downlineCustId)
        => CheckBelonging(new CheckBelongingInDBCustCommand(custId, subAccId, transId, downlineCustId));

    public async Task<DownlineInfo> CheckBelongingByUsername(int custId, int subAccId, string username)
    {
        var command = new CheckBelongingByUsernameCommand(custId, subAccId, username);
        await dynamicRepository.ExecuteAsync(command);

        return new DownlineInfo(
            command.CustId > 0 && command.IsActive,
            command.CustId,
            command.IsSubAcc,
            command.RoleId,
            command.AgentId);
    }

    public Task<BelongingInfo> CheckBelongingConsolidatedGroup(int custId, int subAccId, long transId, int downlineCustId)
        => CheckBelonging(new CheckBelongingConsolidatedGroupCommand(custId, subAccId, transId, downlineCustId));

    public Task<BelongingInfo> CheckBelongingMultipleCustomers(int custId, int subAccId, string custIds)
        => CheckBelonging(new CheckBelongingMultipleCustomersCommand(custId, subAccId, custIds));

    private async Task<BelongingInfo> CheckBelonging(CheckBelongingBaseCommand criteria)
    {
        await dynamicRepository.ExecuteAsync(criteria);

        return new BelongingInfo(criteria.RoleId, criteria.RoleId > 0 && criteria.IsActive);
    }
}
