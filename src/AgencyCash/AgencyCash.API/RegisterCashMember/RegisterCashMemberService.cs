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
        // await _dynamicRepository.ExecuteAsync(command);
        // return new Result
        // {
        //     ErrCode = command.ErrCode,
        //     ErrMsg = command.ErrMsg
        // };
        return new RegisterCashMemberResult();
    }
}
