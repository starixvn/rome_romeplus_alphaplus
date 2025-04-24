using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.BetCreditTracking;

public class BetCreditTrackingController(IMediator mediator) : ReportBaseController(mediator)
{
    public IActionResult BetCreditHistory(BetCreditHistoryQuery message) => View(new BetCreditHistoryViewModel(message));

    public Task<IActionResult> BetCreditHistorySearch(BetCreditHistoryQuery message) => View(message, "BetCreditHistory");

    public Task<IActionResult> BetCreditHistoryExcel(BetCreditHistoryExcelQuery message) => ExcelView(message);

    public Task<IActionResult> MoveStatementHistory(MoveStatementHistoryQuery message) => View(message);

    public Task<IActionResult> MoveStatementHistoryExcel(MoveStatementHistoryExcelQuery message) => ExcelView(message);
}