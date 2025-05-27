using AgencyCash.API.RegisterCashMember.Criteria;
using AgencyCash.API.RegisterCashMember.Responses;

namespace AgencyCash.API.RegisterCashMember;

public interface IRegisterCashMemberService
{
    Task<RegisterCashMemberResult> RegisterCashMember(RegisterCashMemberCommand command);
    Task<CheckUsernameResult> CheckUsernameAvailable(CheckUsernameCommand criteria);
}