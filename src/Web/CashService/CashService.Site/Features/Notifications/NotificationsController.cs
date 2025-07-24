using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;
using AlphaPlus.CashService.Site.Features.Shared;
using Microsoft.AspNetCore.Antiforgery;
using Fanex.Customer.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.Notifications;

public class NotificationsController(
    IConfiguration configuration,
    IUserSession userSession,
    IAntiforgery antiforgery,
    HttpClient httpClient,
    ICustomerService customerService) : SiteBaseController(userSession, antiforgery, httpClient, customerService)
{
    private readonly string _apiBaseUrl = configuration["CashService:APIUrl"];

    public async Task<IActionResult> GetPendingCount()
    {
        object pendingCount = null;

        if (AppSettings.IsOfferedCashService(userSession.Site)
                && (!userSession.IsSubAcc || userSession.SubPageAccess.CashService || userSession.SubPageAccess.CashPromotion))
        {
            var url = $"{_apiBaseUrl}/notifications/GetPendingCount?custId={userSession.CustId}";

            pendingCount = await _httpClient.GetFromJsonAsync<object>(url);
        }

        return Json(pendingCount);
    }
}