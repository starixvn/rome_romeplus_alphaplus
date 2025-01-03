
using AlphaPlus.Core.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.Outright;

public partial class ResultsController(IMediator mediator) : ResultBaseController(mediator)
{
    [AllowProtectedMode]
    public Task<IActionResult> Outright(OutrightResultQuery message)
        => View(message, GetViewPath(nameof(Outright), "Index"));

    [AllowProtectedMode]
    public Task<IActionResult> OutrightExcel(ExcelOutrightResultQuery message) => ExcelView(message);
}