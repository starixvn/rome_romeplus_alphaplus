using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class MonitoringBetsController(
    IMediator mediator,
    IOptions<AppSettings> options,
    IUserSession userSession) : BetListBaseController(mediator)
{
    private readonly AppSettings appSettings = options.Value;
    private readonly IUserSession userSession = userSession;

    // Filter Configuration
    public IActionResult FilterConfiguration(FilterConfigurationQuery message)
        => View(GetViewPath("MonitoringBets", nameof(FilterConfiguration)), new FilterConfigurationViewModel(message, appSettings.MonitoringBetsBetTypes));

    public Task<IActionResult> FilterBySports(FilterBySportsQuery message) => JsonData(message);

    public Task<IActionResult> GetMyWatchlistMembers(GetMyWatchlistMembersQuery message) => JsonData(message);

    [HttpPost]
    public async Task<IActionResult> ApplyFilter(ApplyFilterCommand message)
        => message.IsValid() ? await JsonData(message) : JsonBadRequest();

    [HttpPost]
    public Task<IActionResult> AddFilter(AddFilterCommand message)
        => ValidateAndDoAction(message);

    // Manage Filters
    public Task<IActionResult> ManageFilters(ManageFiltersQuery message)
        => View(message, GetViewPath("MonitoringBets", nameof(ManageFilters)));

    [HttpPost]
    public Task<IActionResult> AddUnsavedFilter(AddUnsavedFilterCommand message)
        => ValidateAndDoAction(message);

    [HttpPost]
    public Task<IActionResult> UpdateFilter(UpdateFilterCommand message)
        => ValidateAndDoAction(message);

    [HttpPost]
    public async Task<IActionResult> DeleteFilters(DeleteFiltersCommand message)
    {
        if (message.IsInternal)
        {
            return JsonForbidden();
        }

        return message.IsValid() ? await JsonData(message) : JsonBadRequest();
    }

    private async Task<IActionResult> ValidateAndDoAction(AddUnsavedFilterCommand message)
    {
        if (message.IsInternal)
        {
            return JsonForbidden();
        }

        return message.IsValid() ? await JsonData(message) : JsonBadRequest();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (userSession.IsInternal
        || appSettings.EnableBetsListMonitoring
        || appSettings.UsersAlwaysAllowAccessBetsListMonitoring.Contains(userSession.CustId))
        {
            base.OnActionExecuting(context);
        }
        else
        {
            context.Result = RedirectToAction("invalid", "shared");
        }
    }
}