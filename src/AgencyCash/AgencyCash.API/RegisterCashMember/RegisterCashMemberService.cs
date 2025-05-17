using AgencyCash.API.RegisterCashMember.Criteria;
using AgencyCash.API.RegisterCashMember.Responses;
using Fanex.Data.Repository;

namespace AgencyCash.API.RegisterCashMember;

public interface IRegisterCashMemberService
{
    Task<RegisterCashMemberResult> RegisterCashMember(RegisterCashMemberCommand command);
}

public class RegisterCashMemberService: IRegisterCashMemberService
{
    private readonly IDynamicRepository _dynamicRepository;

    public RegisterCashMemberService(IDynamicRepository dynamicRepository)
    {
        _dynamicRepository = dynamicRepository;
    }

    public async Task<RegisterCashMemberResult> RegisterCashMember(RegisterCashMemberCommand command)
    {
        // 1. Register customer (API_CustomerIns_CashMember_Register)
        var registerCustomerCmd = new RegisterCustomerBeforeAddNew(
            command.UserName,
            command.UserName2,
            2, // Assuming 2 is the roleId for cash member, adjust as needed
            command.AgentId
        );
        await _dynamicRepository.ExecuteAsync(registerCustomerCmd);
        if (registerCustomerCmd.ErrCode != 0)
        {
            return new RegisterCashMemberResult
            {
                ErrCode = registerCustomerCmd.ErrCode,
                ErrMsg = registerCustomerCmd.ErrMsg
            };
        }

        // 2. Register cash member
        command.ErrCode = 0;
        command.ErrMsg = string.Empty;
        await _dynamicRepository.ExecuteAsync(command);
        if (command.ErrCode != 0)
        {
            // 4. Rollback if failed
            var rollbackCmd = new RollbackRegisterCustomer(registerCustomerCmd.CustId);
            await _dynamicRepository.ExecuteAsync(rollbackCmd);
            return new RegisterCashMemberResult
            {
                ErrCode = command.ErrCode,
                ErrMsg = command.ErrMsg
            };
        }

        // 3. Write log (ignore errors)
        try
        {
            var logXml = $"<Log><CustId>{registerCustomerCmd.CustId}</CustId><UserName>{command.UserName}</UserName></Log>";
            var writeLogCmd = new WriteLogCashMemberCommand(logXml);
            await _dynamicRepository.ExecuteAsync(writeLogCmd);
        }
        catch
        {
            // Ignore log errors
        }

        // 5. Return success
        return new RegisterCashMemberResult
        {
            ErrCode = 0,
            ErrMsg = "Success"
        };
    }
}
