using AlphaPlus.CashService.Site.Features.Shared.Models;
using AlphaPlus.Core.Models;
using Fanex.Customer.Client.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.Shared
{
    public class CommonController : SiteBaseController
    {
        private readonly string _apiBaseUrl;

        public CommonController(
            IConfiguration configuration,
            HttpClient httpClient,
            IUserSession userSession,
            IAntiforgery antiforgery,
            ICustomerService customerService) : base(userSession, antiforgery, httpClient, customerService)
        {
            _apiBaseUrl = configuration["CashService:APIUrl"];
        }

        [HttpGet]
        public async Task<IActionResult> GetSetupChecklist(CancellationToken cancellationToken = default)
        { 
            var url = $"{_apiBaseUrl}/common/setup-checklist/{_userSession.CustId}";
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            var setupChecklist = await response.Content.ReadFromJsonAsync<SetupCheckListModel>(cancellationToken: cancellationToken);

            if (setupChecklist != null)
            {
                setupChecklist.IsCanEditCashService = !_userSession.IsSubAcc || _userSession.SubPageAccess.CashService;
            }

            return Json(setupChecklist);
        }

        public async Task<IActionResult> GetProviderBanks(int providerId, CancellationToken cancellationToken)
        {
            var url = $"{_apiBaseUrl}/common/banks?site={_userSession.Site}&providerId={providerId}&languageCode={_userSession.Language}";
            var banks = await _httpClient.GetFromJsonAsync<object>(url, cancellationToken);
            return Json(banks);
        }
    }
} 