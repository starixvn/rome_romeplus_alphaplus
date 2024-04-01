using AlphaPlus.Core.Models;

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

    public class PasswordService(HttpClient httpClient) : IPasswordService
    {
        private readonly HttpClient httpClient = httpClient;

        public Task<Result> ChangeMainAccountPassword(ChangeMainAccountPasswordParam param)
            => httpClient.PostAsJsonAsync<Result>("main-account/password/change", param);

        public Task<Result> ChangeSubAccountPassword(ChangeSubAccountPasswordParam param)
            => httpClient.PostAsJsonAsync<Result>("sub-account/password/change", param);

        public Task<Result> ResetMainAccountPassword(ResetMainAccountPasswordParam param)
            => httpClient.PostAsJsonAsync<Result>("main-account/password/reset", param);

        public Task<Result> ResetSubAccountPassword(ResetSubAccountPasswordParam param)
            => httpClient.PostAsJsonAsync<Result>("sub-account/password/reset", param);

        public Task<Result> SkipForceChangePassword(SkipForceChangePasswordParam param)
            => httpClient.PostAsJsonAsync<Result>("account/password/skip", param);
    }
}
