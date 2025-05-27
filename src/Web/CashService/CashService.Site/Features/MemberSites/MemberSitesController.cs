using AlphaPlus.CashService.Site.Features.MemberSites.Models;
using AlphaPlus.CashService.Site.Features.MemberSites.ViewModels;
using AlphaPlus.CashService.Site.Features.Shared;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Mvc;
namespace AlphaPlus.CashService.Site.Features.MemberSites;

public class MemberSitesController : SiteBaseController
{
    private readonly string _apiBaseUrl;
    private readonly HttpClient _httpClient;

    public MemberSitesController(
        IConfiguration configuration,
        IUserSession userSession,
        HttpClient httpClient) : base(userSession)
    {
        _apiBaseUrl = configuration["CashServiceAPIUrl"];
        _httpClient = httpClient;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var model = new MemberSiteBaseViewModel
        {
            DarkModeName = _userSession.DarkModeName
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