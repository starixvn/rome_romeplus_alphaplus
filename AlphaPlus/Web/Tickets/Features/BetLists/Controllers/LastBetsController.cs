using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class LastBetsController(
    IMediator mediator,
    IOptions<AppSettings> options,
    IUserSession userSession) : BetListBaseController(mediator)
{
    private readonly AppSettings appSettings = options.Value;

    public Task<IActionResult> Index(LastBetsQuery message)
        => View(message, GetViewPath("LastBets", nameof(Index)));

    public Task<IActionResult> BetsData(GetLastBetsDataQuery message)
        => View<LastBetsDataViewModel>(message, GetViewPath("LastBets", nameof(BetsData)));

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
            context.Result = new RedirectResult("/tickets/shared/invalid");
        }
    }
}