using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class StatementController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> SMAStatement(SMAStatementQuery message) => View(message);

    public IActionResult SMAReportLegend() => View();

    public Task<IActionResult> SMAStatementExcel(SMAStatementExcelQuery message) => ExcelView(message);

    public Task<IActionResult> StatementDetails(StatementDetailsQuery message) => View(message);

    public Task<IActionResult> StatementDetailExcel(StatementDetailExcelQuery message) => ExcelView(message);

    public Task<IActionResult> MemberStatement(MemberStatementQuery message) => View<MemberStatementViewModel>(message);

    public ActionResult MemberReportLegend() => View();

    public Task<IActionResult> MemberStatementExcel(MemberStatementExcelQuery message) => ExcelView(message);
}
