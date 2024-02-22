using AlphaPlus.Core.Models;

namespace Alpha.Main.Site.Features.Nickname
{
    public interface INicknameService
    {
        Task<Result> CreateNickname(CreateNicknameParam param);
    }

    public class NicknameService(HttpClient httpClient) : INicknameService
    {
        private readonly HttpClient httpClient = httpClient;

        public Task<Result> CreateNickname(CreateNicknameParam param)
            => httpClient.PostAsJsonAsync<Result>("customer/nickname", param);
    }
}
