using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetType;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class BetListByBetTypeController(IMediator mediator) : BetListBaseController(mediator)
{
    private readonly string ViewName = string.Format(BetListView, "BetListByBetType", "Index");

    public Task<IActionResult> CleanSheet(BetListByBetTypeQuery message)
        => View(message, ViewName);

    public Task<FileContentResult> CleanSheetExcel(BetListByBetTypeExcelQuery message)
        => ExcelView(message);

    public Task<IActionResult> DoubleChance(BetListByBetTypeQuery message)
        => View(message, ViewName);

    public Task<FileContentResult> DoubleChanceExcel(BetListByBetTypeExcelQuery message)
        => ExcelView(message);

    public Task<IActionResult> BothNoneNeitherTeamToScore(BetListByBetTypeQuery message)
        => View(message, ViewName);

    public Task<FileContentResult> BothNoneNeitherTeamToScoreExcel(BetListByBetTypeExcelQuery message)
        => ExcelView(message);

    public Task<IActionResult> ToWinToNil(BetListByBetTypeQuery message)
        => View(message, ViewName);

    public Task<FileContentResult> ToWinToNilExcel(BetListByBetTypeExcelQuery message)
        => ExcelView(message);

    public Task<IActionResult> ThreeWayHandicap(BetListByBetTypeQuery message)
        => View(message, ViewName);

    public Task<FileContentResult> ThreeWayHandicapExcel(BetListByBetTypeExcelQuery message)
        => ExcelView(message);
}