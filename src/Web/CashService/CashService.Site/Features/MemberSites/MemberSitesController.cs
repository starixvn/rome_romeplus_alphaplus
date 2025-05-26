using AlphaPlus.CashService.Site.Features.MemberSites.Models;
using AlphaPlus.CashService.Site.Features.MemberSites.ViewModels;
using AlphaPlus.Core.Models;
using AlphaPlus.CashService.Site.Features.Shared;

using Microsoft.AspNetCore.Mvc;
namespace AlphaPlus.CashService.Site.Features.MemberSites;

public class MemberSitesController : SiteBaseController
{
    private readonly string _apiBaseUrl;
    private readonly ILogger<MemberSitesController> _logger;
    private readonly IConfiguration _configuration;

    private readonly HttpClient _httpClient;

    public MemberSitesController(
        IConfiguration configuration,
         ILogger<MemberSitesController> logger,
          IUserSession userSession,
          HttpClient httpClient) : base(userSession)
    {
        _apiBaseUrl = configuration["CashServiceAPIUrl"];
        _logger = logger;
        _configuration = configuration;
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
        try
        {
            var baseUrl = _configuration["CashServiceAPIUrl"];
            var response = await _httpClient.GetFromJsonAsync<MemberSiteManagementModel>(
                //$"{baseUrl}/api/member-site-info?custId={1176086}",
                $"{baseUrl}/member-site-info?custId={_userSession.CustId}&site={_userSession.Site}",
                cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting member site management data");
            return StatusCode(500, new { error = "Failed to fetch member site management data" });
        }
    }

    [HttpPut]
    public async Task<IActionResult> MemberSiteManagement([FromBody] UpdateMemberSiteManagementModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            model.CustId = _userSession.CustId;
            model.UserId = _userSession.UserId;
            //model.CustId = 1176086;
            model.SubAccId = _userSession.SubAccId;
            model.IP = _userSession.UserLoginIp;
            model.Language = _userSession.LangKey;
            model.SiteDB = _userSession.CustDBGroupId;

            var response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}/member-site-info", model, cancellationToken);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating member site management data");
            return StatusCode(500, new { error = "Failed to update member site management data" });
        }
    }
}