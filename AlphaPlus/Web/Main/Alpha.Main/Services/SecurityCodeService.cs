using Alpha.Main.Params.SecurityCode;
using AlphaPlus.Core.Models;

namespace Alpha.Main.Services
{
    public interface SecurityCodeService
    {
        Task<Result> ChangeMainAccountSecurityCode(ChangeSecurityCodeParam param);

        Task<Result> ChangeSubAccountSecurityCode(ChangeSecurityCodeParam param);

        Task<Result> ResetSubAccountSecurityCode(ResetSecurityCodeParam param);

        string GenerateSecurityToken();

        string GenerateSalt(int custId);

        Task<Result> VerifySecurityCode(VerifySecurityCodeParam param);

        Task<Result> UpdateSecurityCodeFunction(UpdateSecurityCodeFunctionParam param);
    }
}
