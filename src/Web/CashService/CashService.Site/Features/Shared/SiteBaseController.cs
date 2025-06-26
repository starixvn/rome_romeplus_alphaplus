using AlphaPlus.CashService.Site.Features.Shared.Models;
using AlphaPlus.CashService.Site.Features.Shared.Resources;
using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using Fanex.Customer.Client.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaPlus.CashService.Site.Features.Shared
{
    [AutoValidateAntiforgeryToken]
    public class SiteBaseController : BaseController
    {
        protected readonly IUserSession _userSession;
        protected readonly IAntiforgery _antiforgery;
        protected readonly HttpClient _httpClient;
        protected readonly ICustomerService _customerService;

        protected SiteBaseController(
            IUserSession userSession, 
            IAntiforgery antiforgery, 
            HttpClient httpClient,
            ICustomerService customerService)
        {
            _userSession = userSession;
            _antiforgery = antiforgery;
            _httpClient = httpClient;
            _customerService = customerService;
        }


        protected string GetAntiforgeryToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            return tokens.RequestToken;
        }

        protected async Task<IActionResult> DoAction<TRequest, TResult>(
            string url,
            TRequest request,
            CancellationToken cancellationToken) where TRequest : Shared.Requests.BaseActionRequest
        {
            if (_userSession.IsInternal)
            {
                return Unauthorized();
            }

            var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
            if(isCashServiceEnabled == 0) {
                return Json(new ResultModel(401, Messages.CashServiceIsDisabled));
            }

            request.SetUserSession(_userSession);
            var response = await _httpClient.PostAsJsonAsync(url, request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken);
            return Json(result);
        } 

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewData["RequestVerificationToken"] = GetAntiforgeryToken();
        }
    }
}