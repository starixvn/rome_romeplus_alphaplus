using AlphaPlus.Core.Models;

namespace Alpha.Main.Site.Features.SecurityCode
{
    public interface ISecurityCodeService
    {
        Task<Result> ChangeMainAccountSecurityCode(ChangeSecurityCodeParam param);

        Task<Result> ChangeSubAccountSecurityCode(ChangeSecurityCodeParam param);

        Task<Result> ResetSubAccountSecurityCode(ResetSecurityCodeParam param);

        Task<Result> VerifySecurityCode(VerifySecurityCodeParam param);

        Task<Result> UpdateSecurityCodeFunction(UpdateSecurityCodeFunctionParam param);
    }

    public class SecurityCodeService(HttpClient httpClient) : ISecurityCodeService
    {
        private readonly HttpClient httpClient = httpClient;

        public Task<Result> ChangeMainAccountSecurityCode(ChangeSecurityCodeParam param)
            => httpClient.PostAsJsonAsync<Result>("security-code/main-account/change", param);

        public Task<Result> ChangeSubAccountSecurityCode(ChangeSecurityCodeParam param)
            => httpClient.PostAsJsonAsync<Result>("security-code/sub-account/change", param);

        public Task<Result> ResetSubAccountSecurityCode(ResetSecurityCodeParam param)
            => httpClient.PostAsJsonAsync<Result>("security-code/sub-account/reset", param);

        public Task<Result> UpdateSecurityCodeFunction(UpdateSecurityCodeFunctionParam param)
            => httpClient.PostAsJsonAsync<Result>("security-code/update-function", param);

        public Task<Result> VerifySecurityCode(VerifySecurityCodeParam param)
            => httpClient.PostAsJsonAsync<Result>("security-code/verify", param);
    }
}
