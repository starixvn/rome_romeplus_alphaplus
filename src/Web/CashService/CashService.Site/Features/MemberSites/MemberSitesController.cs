using AlphaPlus.CashService.Site.Features.MemberSites.Models;
using AlphaPlus.CashService.Site.Features.MemberSites.ViewModels;
using AlphaPlus.CashService.Site.Features.Shared;
using AlphaPlus.CashService.Site.Features.Shared.Filters;
using AlphaPlus.CashService.Site.Features.Shared.Models;
using AlphaPlus.CashService.Site.Features.Shared.Resources;
using AlphaPlus.Core.Models;
using Fanex.Customer.Client.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
namespace AlphaPlus.CashService.Site.Features.MemberSites;

[SiteAuthorizationFilter(PageAccessType.CashService)]
public class MemberSitesController : SiteBaseController
{
    private readonly string _apiBaseUrl;

    public MemberSitesController(
        IConfiguration configuration,
        IUserSession userSession,
        HttpClient httpClient,
        IAntiforgery antiforgery,
        ICustomerService customerService
        ) : base(userSession, antiforgery, httpClient, customerService)
    {
        _apiBaseUrl = configuration["CashService:APIUrl"];
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
        var model = new MemberSiteBaseViewModel
        {
            DarkModeName = _userSession.DarkModeName,
            IsCashServiceEnabled = isCashServiceEnabled
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> MemberSiteManagement(CancellationToken cancellationToken)
    {

        var url = $"{_apiBaseUrl}/member-site-info?custId={_userSession.CustId}&site={_userSession.Site}";
        var response = await _httpClient.GetFromJsonAsync<MemberSiteManagementModel>(url, cancellationToken);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> MemberSiteManagement([FromBody] UpdateMemberSiteManagementModel model, CancellationToken cancellationToken)
    {
        if (_userSession.IsInternal)
        {
            return Unauthorized();
        }

        var isCashServiceEnabled = await _customerService.GetCashServiceStatus(_userSession.CustId);
        if (isCashServiceEnabled == 0)
        {
            return Json(new ResultModel(401, Messages.CashServiceIsDisabled));
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        model.CustId = _userSession.CustId;
        model.UserId = _userSession.UserId;
        model.SubAccId = _userSession.SubAccId;
        model.IP = _userSession.UserLoginIp;
        model.Language = _userSession.LangKey;
        model.SiteDB = _userSession.CustDBGroupId;

        var response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}/member-site-info", model, cancellationToken);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
        return Ok(result);
    }
}