using AlphaPlus.Tickets.Site.Features.BetLists.Models;
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
    IUser user) : BetListBaseController(mediator)
{
    private readonly AppSettings appSettings = options.Value;
    private readonly IUser user = user;

    public Task<IActionResult> Index(LastBetsQuery message)
        => View(message, GetViewPath("LastBets", nameof(Index)));

    public Task<IActionResult> BetsData(GetLastBetsDataQuery message)
        => View<LastBetsDataViewModel>(message, GetViewPath("LastBets", nameof(BetsData)));

    public Task<IActionResult> IndexNew(LastBetsNewQuery message)
        => View(message, GetViewPath("LastBets", nameof(IndexNew)));

    [HttpPost]
    public Task<IActionResult> BetsDataNew(GetLastBetsDataNewQuery message)
        => View(message, GetViewPath("LastBets", nameof(BetsDataNew)));

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (user.IsInternal
        || appSettings.EnableBetsListMonitoring
        || appSettings.UsersAlwaysAllowAccessBetsListMonitoring.Contains(user.Id))
        {
            base.OnActionExecuting(context);
        }
        else
        {
            context.Result = new RedirectResult("/tickets/shared/invalid");
        }
    }
}