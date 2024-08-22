using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.BetChoiceMatching;

public class BetChoiceMatchingController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(BetChoiceMatchingQuery message) => View(message);

    public Task<IActionResult> Query(BetChoiceMatchingJsonQuery message) => Json<BetChoiceMatchingJsonModel>(message);

    public Task<IActionResult> Uplines(BetChoiceUplinesJsonQuery message) => Json(message);

    public Task<IActionResult> Excel(BetChoiceMatchingExcelQuery message) => ExcelView(message);
}