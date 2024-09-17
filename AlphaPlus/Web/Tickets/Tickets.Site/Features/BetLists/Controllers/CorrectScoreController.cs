using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CorrectScore;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CorrectScore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class CorrectScoreController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Index(CorrectScoreQuery message)
        => View<CorrectScoreViewModel>(message, GetViewPath("CorrectScore", nameof(Index)));

    public Task<FileContentResult> Excel(CorrectScoreExcelQuery message) => ExcelView(message);
}
