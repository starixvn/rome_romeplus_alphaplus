using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.WhiteLabel
{
    public interface IWhiteLabelService
    {
        Task<bool> IsOfferedWhiteLabel(int custId);

        Task<WhiteLabelSSOResponse> SendSSORequest(WhiteLabelSSORequest request);
    }

    public class WhiteLabelService : IWhiteLabelService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly HttpClient httpClient;

        public WhiteLabelService(IDynamicRepository dynamicRepository, HttpClient httpClient)
        {
            this.dynamicRepository = dynamicRepository;
            this.httpClient = httpClient;
        }

        public async Task<bool> IsOfferedWhiteLabel(int custId)
        {
            var commmand = new CheckExistsWhiteLabelCommand(custId);
            await dynamicRepository.ExecuteAsync(commmand);
            return commmand.IsExists;
        }

        public Task<WhiteLabelSSOResponse> SendSSORequest(WhiteLabelSSORequest request)
            => httpClient.PostAsJsonAsync<WhiteLabelSSOResponse>("Signin", request);
    }
}