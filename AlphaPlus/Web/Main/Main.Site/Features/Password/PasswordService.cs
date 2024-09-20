using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Shared.Services;

namespace AlphaPlus.Main.Site.Features.Password
{
    public interface IPasswordService
    {
        Task<Result> ResetMainAccountPassword(ResetMainAccountPasswordParam param);

        Task<Result> ChangeMainAccountPassword(ChangeMainAccountPasswordParam param);

        Task<Result> ResetSubAccountPassword(ResetSubAccountPasswordParam param);

        Task<Result> ChangeSubAccountPassword(ChangeSubAccountPasswordParam param);

        Task<Result> SkipForceChangePassword(SkipForceChangePasswordParam param);
    }

    public class PasswordService(HttpClient httpClient, INetThrottlingService netThrottlingService) : IPasswordService
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly INetThrottlingService netThrottlingService = netThrottlingService;

        public Task<Result> ChangeMainAccountPassword(ChangeMainAccountPasswordParam param)
            => httpClient.PostAsJsonAsync<Result>("main-account/password/change", param);

        public Task<Result> ChangeSubAccountPassword(ChangeSubAccountPasswordParam param)
            => httpClient.PostAsJsonAsync<Result>("sub-account/password/change", param);

        public Task<Result> ResetMainAccountPassword(ResetMainAccountPasswordParam param)
            => httpClient.PostAsJsonAsync<Result>("main-account/password/reset", param);

        public async Task<Result> ResetSubAccountPassword(ResetSubAccountPasswordParam param)
        {
            if (netThrottlingService.IsBlacklist())
            {
                return Result.CreateErrorResult("Too many requests");
            }

            var result = await httpClient.PostAsJsonAsync<Result>("sub-account/password/reset", param);
            if (result.IsSuccessful)
            {
                netThrottlingService.Throttle();
            }

            return result;
        }

        public Task<Result> SkipForceChangePassword(SkipForceChangePasswordParam param)
            => httpClient.PostAsJsonAsync<Result>("account/password/skip", param);
    }
}
