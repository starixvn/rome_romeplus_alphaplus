using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.Outstanding;

public class OutstandingController(IMediator mediator) : ReportBaseController(mediator)
{
    [Route("outstanding/master")]
    [Route("outstanding/agent")]
    [Route("outstanding/member")]
    public Task<IActionResult> Index(OutstandingQuery message) => View(message);
}
