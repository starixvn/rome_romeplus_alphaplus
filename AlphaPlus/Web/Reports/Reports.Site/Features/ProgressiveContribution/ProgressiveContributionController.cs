using AlphaPlus.Reports.Site.Features.Legends;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(ProgressiveContributionQuery message) => View(message);

    public Task<IActionResult> Excel(ProgressiveContributionExcelQuery message) => ExcelView(message);

    public Task<IActionResult> Legend(ProgressiveContributionLegendQuery message)
        => View<LegendViewModel>(message, "~/Features/Legends/Views/Index.cshtml");
}