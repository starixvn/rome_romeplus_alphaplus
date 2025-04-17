using AlphaPlus.Identity.Features.Authorization.Criteria;
using AlphaPlus.Identity.Features.Authorization.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Identity.Features.Authorization;
public interface IAuthorizationService
{
    Task<CustomerStatus> GetCustomerStatus(int custId, int subAccId);

    Task<BelongingInfo> CheckBelonging(int custId, int subAccId, long transId, int downlineCustId);

    Task<BelongingInfo> CheckBelongingConsolidatedGroup(int custId, int subAccId, long transId, int downlineCustId);

    Task<BelongingInfo> CheckBelongingMultipleCustomers(int custId, int subAccId, string custIds);
}

internal class AuthorizationService(IDynamicRepository dynamicRepository) : IAuthorizationService
{
    private readonly IDynamicRepository dynamicRepository = dynamicRepository;

    public async Task<CustomerStatus> GetCustomerStatus(int custId, int subAccId)
    {
        var command = new GetAccountStatusCommand(custId, subAccId);
        await dynamicRepository.ExecuteAsync(command);

        return new CustomerStatus(command.IsActive, command.Locked);
    }

    public async Task<BelongingInfo> CheckBelonging(int custId, int subAccId, long transId, int downlineCustId)
    {
        var command = new CheckBelongingCommand(custId, subAccId, transId, downlineCustId);
        await dynamicRepository.ExecuteAsync(command);

        return new BelongingInfo(command.RoleId, command.RoleId > 0 && command.IsActive);
    }

    public async Task<BelongingInfo> CheckBelongingConsolidatedGroup(int custId, int subAccId, long transId, int downlineCustId)
    {
        var command = new CheckBelongingConsolidatedGroupCommand(custId, subAccId, transId, downlineCustId);
        await dynamicRepository.ExecuteAsync(command);

        return new BelongingInfo(command.RoleId, command.RoleId > 0 && command.IsActive);
    }

    public async Task<BelongingInfo> CheckBelongingMultipleCustomers(int custId, int subAccId, string custIds)
    {
        var command = new CheckBelongingMultipleCustomersCommand(custId, subAccId, custIds);
        await dynamicRepository.ExecuteAsync(command);

        return new BelongingInfo(command.RoleId, command.RoleId > 0 && command.IsActive);
    }
}
