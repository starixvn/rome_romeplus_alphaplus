namespace AlphaPlus.CashService.Site.Features.Promotions;

using Core.Models;
using Helpers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Filters;
using ViewModels;

[SiteAuthorizationFilter(PageAccessType.CashPromotion)]
public class ManagePromotionsController(
    HttpClientHelper httpClient,
    IConfiguration configuration,
    IUserSession userSession,
    IAntiforgery antiforgery)
    : SiteBaseController(userSession, antiforgery)
{
    private readonly string _apiBaseUrl = configuration["CashService:APIUrl"];

    public IActionResult Index()
    {
        var viewModel = new PromotionBaseViewModel
        {
            Language = _userSession.Language,
            DarkModeName = _userSession.DarkModeName,
        };

        return View(viewModel);
    }

    public async Task<IActionResult> GetPromotion(string promotionType, string status, CancellationToken cancellationToken)
    {
        var url = $"{_apiBaseUrl}/promotion/get-promotion/{promotionType}/{status}";

        var result = await httpClient.GetAsync<object>(url);

        return Json(result);
    }
}