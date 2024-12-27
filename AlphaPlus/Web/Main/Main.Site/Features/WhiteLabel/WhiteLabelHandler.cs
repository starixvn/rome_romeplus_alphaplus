using MediatR;

namespace AlphaPlus.Main.Site.Features.WhiteLabel
{
    using System.Text;
    using System.Text.Json;
    using AlphaPlus.Shared.AppResources.Main;

    public class WhiteLabelHandler : IRequestHandler<WhiteLabelSSORequest, WhiteLabelSSOResponseViewModel>
    {
        private readonly HttpClient httpClient;
        private readonly WhiteLabelService whiteLabelService;

        public WhiteLabelHandler(WhiteLabelService whiteLabelService)
        {
            httpClient = CreateHttpClient();
            this.whiteLabelService = whiteLabelService;
        }

        public async Task<WhiteLabelSSOResponseViewModel> Handle(WhiteLabelSSORequest request, CancellationToken cancellationToken)
        {
            var isOffered = await whiteLabelService.IsOfferedWhiteLabel(int.Parse(request.ReferenceId));
            if (isOffered)
            {
                var ssoResponse = await SendSSORequest(request);
                return new WhiteLabelSSOResponseViewModel { ReturnUrl = ssoResponse.ReturnUrl };
            }

            return new WhiteLabelSSOResponseViewModel
            {
                Code = 1,
                Message = Resources.NotSetupWhiteLabelMessage,
            };
        }

        private async Task<WhiteLabelSSOResponse> SendSSORequest(WhiteLabelSSORequest request)
        {
            using var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync("Signin", jsonContent);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WhiteLabelSSOResponse>(stringResponse);
        }

        private static HttpClient CreateHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = AppSettings.WhiteLabelBackOfficeApiUri,
                Timeout = TimeSpan.FromSeconds(15),
            };
        }
    }
}