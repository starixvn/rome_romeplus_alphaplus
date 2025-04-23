using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.SuperWinLossDetailByPT;

public class SuperWinLossDetailByPTController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(SuperWinLossDetailByPTQuery message) => View(message);

    public Task<IActionResult> Excel(SuperWinLossDetailByPTExcelQuery message) => ExcelView(message);
}