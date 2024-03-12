using AlphaPlus.Core.Filters;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

[AllowProtectedMode]
public class CommissionByBetTypeController(IMediator mediator) : ReportBaseController(mediator)
{
    [Route("commissionbybettype/master")]
    [Route("commissionbybettype/agent")]
    [Route("commissionbybettype/member")]
    public Task<IActionResult> Index(CommissionByBetTypeQuery message) => View(message);
}