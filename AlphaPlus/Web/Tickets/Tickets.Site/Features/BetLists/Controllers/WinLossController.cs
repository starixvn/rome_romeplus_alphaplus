using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class WinLossController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(WinLossQuery message)
        => View(message, GetViewPath("WinLoss", nameof(Index)));

    public Task<FileContentResult> Excel(WinLossExcelQuery message) => ExcelView(message);

    public async Task<IActionResult> IndexNew(WinLossNewQuery message)
        => message.IsValid()
        ? await View<WinLossNewViewModel>(message, GetViewPath("WinLoss", nameof(IndexNew)))
        : View(InvalidBetListView);

    public async Task<IActionResult> BetListWeekSelection(WinLossWeekSelectionQuery message)
        => message.IsValid()
        ? await View<WinLossWeekSelectionViewModel>(message, GetViewPath("WinLoss", nameof(BetListWeekSelection)))
        : View(InvalidBetListView);

    public async Task<IActionResult> BetList(WinLossBetListQuery message)
        => message.IsValid()
        ? await View<WinLossViewModel>(message, GetViewPath("WinLoss", nameof(BetList)))
        : View(InvalidBetListView);
}