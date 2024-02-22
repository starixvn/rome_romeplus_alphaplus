using AlphaPlus.Core.Models;

namespace Alpha.Main.Site.Features.ProtectionCode
{
    public interface IProtectionCodeService
    {
        Task<string> GetAsync(int custId, int subAccId);

        Task<Result> UpdateAsync(UpdateProtectionCodeParam param);
    }

    public class ProtectionCodeService(HttpClient httpClient) : IProtectionCodeService
    {
        private readonly HttpClient httpClient = httpClient;

        public Task<string> GetAsync(int custId, int subAccId)
            => httpClient.GetStringAsync($"customer/protection-code?custId={custId}&subAccId={subAccId}");

        public Task<Result> UpdateAsync(UpdateProtectionCodeParam param)
            => httpClient.PostAsJsonAsync<Result>("customer/protection-code", param);
    }
}
