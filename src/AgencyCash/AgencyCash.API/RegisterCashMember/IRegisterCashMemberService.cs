using AgencyCash.API.RegisterCashMember.Criteria;
using AgencyCash.API.RegisterCashMember.Requests;
using AgencyCash.API.RegisterCashMember.Responses;

namespace AgencyCash.API.RegisterCashMember;

public interface IRegisterCashMemberService
{
    Task<RegisterCashMemberResult> RegisterCashMember(RegisterCashMemberRequest request);
    Task<CheckUsernameResult> CheckUsernameAvailable(CheckUsernameCommand criteria);
}