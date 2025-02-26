using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Shared.Services;

namespace AlphaPlus.Main.Site.Features.SecurityCode
{
    public interface ISecurityCodeService
    {
        Task<Result> ChangeMainAccountSecurityCode(ChangeSecurityCodeParam param);

        Task<Result> ChangeSubAccountSecurityCode(ChangeSecurityCodeParam param);

        Task<Result> ResetSubAccountSecurityCode(ResetSecurityCodeParam param);

        Task<Result> VerifySecurityCode(VerifySecurityCodeParam param);

        Task<Result> UpdateSecurityCodeFunction(UpdateSecurityCodeFunctionParam param);
    }

    public class SecurityCodeService(HttpClient httpClient, INetThrottlingService netThrottlingService) : ISecurityCodeService
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly INetThrottlingService netThrottlingService = netThrottlingService;

        public Task<Result> ChangeMainAccountSecurityCode(ChangeSecurityCodeParam param)
            => httpClient.PostAsJsonAsync<Result>("security-code/main-account/change", param);

        public Task<Result> ChangeSubAccountSecurityCode(ChangeSecurityCodeParam param)
            => httpClient.PostAsJsonAsync<Result>("security-code/sub-account/change", param);

        public async Task<Result> ResetSubAccountSecurityCode(ResetSecurityCodeParam param)
        {
            if (netThrottlingService.IsBlacklist())
            {
                return Result.CreateErrorResult("Too many requests");
            }

            var result = await httpClient.PostAsJsonAsync<Result>("security-code/sub-account/reset", param);

            if (result.IsSuccessful && !param.IsMemberLevel)
            {
                netThrottlingService.Throttle();
            }

            return result;
        }

        public Task<Result> UpdateSecurityCodeFunction(UpdateSecurityCodeFunctionParam param)
            => httpClient.PostAsJsonAsync<Result>("security-code/update-function", param);

        public Task<Result> VerifySecurityCode(VerifySecurityCodeParam param)
            => httpClient.PostAsJsonAsync<Result>("security-code/check", param);
    }
}
