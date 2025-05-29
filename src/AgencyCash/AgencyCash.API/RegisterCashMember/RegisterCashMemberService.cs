using AgencyCash.API.RegisterCashMember.Constants;
using AgencyCash.API.RegisterCashMember.Criteria;
using AgencyCash.API.RegisterCashMember.Requests;
using AgencyCash.API.RegisterCashMember.Responses;
using Fanex.Data.Repository;

namespace AgencyCash.API.RegisterCashMember;

public class RegisterCashMemberService : IRegisterCashMemberService
{
    private readonly IDynamicRepository _dynamicRepository;

    public RegisterCashMemberService(IDynamicRepository dynamicRepository)
    {
        _dynamicRepository = dynamicRepository;
    }

    public async Task<RegisterCashMemberResult> RegisterCashMember(RegisterCashMemberRequest request)
    {
        var validateResult = await ValidateSetting(request.UserName, request.Domain, request.AgentId);
        if (!validateResult.IsSuccess)
            return new RegisterCashMemberResult(validateResult.Code, validateResult.Message);
        string actorName = validateResult.AgentUsername;
        byte roleId = CustomerLevel.Agent;
        const int UsernameExisted = 201;
        const int MaxRetryCount = 5;
        for (int retryCount = 0; retryCount < MaxRetryCount; retryCount++)
        {
            var suggestResult = await GetSuggestUsername(actorName, roleId);
            if (!suggestResult.IsSuccess)
                return new RegisterCashMemberResult(suggestResult.Code, suggestResult.Message);

            string newUserName = actorName + suggestResult.SeqName;
            var registerBeforeAddNewResult = await RegisterUsernameBeforeAddNew(request, newUserName);

            if (registerBeforeAddNewResult.Code == UsernameExisted)
            {
                if (retryCount == MaxRetryCount - 1)
                    return new RegisterCashMemberResult(registerBeforeAddNewResult.Code, registerBeforeAddNewResult.Message);
                continue;
            }

            if (!registerBeforeAddNewResult.IsSuccess)
                return new RegisterCashMemberResult(registerBeforeAddNewResult.Code, registerBeforeAddNewResult.Message);

            var createResult = await CreateNewCashMember(request, registerBeforeAddNewResult.CustId, newUserName);
            if (!createResult.IsSuccess)
            {
                await Rollback(registerBeforeAddNewResult.CustId);
                return new RegisterCashMemberResult(createResult.Code, createResult.Message);
            }

            return new RegisterCashMemberResult(0, "Registration Successful");
        }

        return new RegisterCashMemberResult(-1, "Registration failed after maximum retries.");
    }

    public Task<CheckUsernameResult> CheckUsernameAvailable(CheckUsernameCommand criteria)
    {
        _dynamicRepository.Execute(criteria);
        return Task.FromResult(new CheckUsernameResult(criteria.ErrCode == 0));
    }

    internal async Task<ValidateCashMemberResult> ValidateSetting(string username, string domain, int agentId)
    {
        var criteria = new ValidateSettingCommand(username, domain, agentId);
        await _dynamicRepository.ExecuteAsync(criteria);
        return new ValidateCashMemberResult(criteria.ErrCode, criteria.ErrMsg, criteria.AgentName);
    }

    internal async Task<SuggestUsernameResult> GetSuggestUsername(string actorName, byte roleId)
    {
        var command = new GetSuggestUsernameCommand(actorName, roleId);
        await _dynamicRepository.ExecuteAsync(command);
        return new SuggestUsernameResult(command.ErrCode, command.ErrMsg, command.SeqName);
    }

    internal async Task<RegisterBeforeAddNewResult> RegisterUsernameBeforeAddNew(RegisterCashMemberRequest request, string newUserName)
    {
        var command = new RegisterCustomerBeforeAddNewCommand(
            username: newUserName,
            username2: request.UserName,
            roleId: CustomerLevel.Member,
            uplineId: request.AgentId
        );
        await _dynamicRepository.ExecuteAsync(command);
        return new RegisterBeforeAddNewResult(command.ErrCode, command.ErrMsg, command.CustId);
    }

    internal async Task<RegisterCashMemberResult> CreateNewCashMember(RegisterCashMemberRequest request, int custId, string newUserName)
    {
        var command = new RegisterCashMemberCommand(
            custId: custId,
            firstName: request.FirstName,
            lastName: request.LastName,
            username: newUserName,
            username2: request.UserName,
            userPwd: request.Password,
            mobilePhone: request.Mobile,
            uplineId: request.AgentId,
            ip: request.Ip
        );

        await _dynamicRepository.ExecuteAsync(command);
        return new RegisterCashMemberResult(command.ErrCode, command.ErrMsg);
    }

    internal async Task Rollback(int custId)
    {
        var command = new RollbackRegisterCustomerCommand(custId);
        await _dynamicRepository.ExecuteAsync(command);
    }
}
