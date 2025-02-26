using AlphaPlus.Core.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public partial class ResultsController(IMediator mediator) : ResultBaseController(mediator)
{
    [AllowProtectedMode]
    public Task<IActionResult> Normal(NormalResultQuery message)
        => View(message, GetViewPath(nameof(Normal), "Index"));

    [AllowProtectedMode]
    public Task<IActionResult> NormalExcel(ExcelNormalResultQuery message) => ExcelView(message);

    [AllowProtectedMode]
    public Task<IActionResult> Details(NormalResultDetailsQuery message)
        => View<NormalDetailViewModel>(message, GetViewPath(nameof(Normal), nameof(Details)));
}