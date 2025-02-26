using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetTypeAndMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetTypeAndMatch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class BetListByBetTypeAndMatchController : BetListBaseController
{
    public BetListByBetTypeAndMatchController(IMediator mediator) : base(mediator)
    {
    }

    public Task<IActionResult> Index(BetListByBetTypeAndMatchQuery message)
        => View<BetListByBetTypeAndMatchViewModel>(message, GetViewPath("BetListByBetTypeAndMatch", nameof(Index)));

    public Task<FileContentResult> Excel(BetListByBetTypeAndMatchExcelQuery message) => ExcelView(message);
}