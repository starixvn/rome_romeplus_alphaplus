using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Index(ProgressiveContributionQuery message) => View(message);
}