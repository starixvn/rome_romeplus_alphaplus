using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public partial class ResultsController(IMediator mediator) : ResultBaseController(mediator)
{
    public Task<IActionResult> Normal(NormalResultQuery message)
        => View(message, string.Format(ViewPath, nameof(Normal), "Index"));

    public Task<IActionResult> NormalExcel(ExcelNormalResultQuery message) => ExcelView(message);

    public Task<IActionResult> Details(NormalResultDetailsQuery message)
        => View<NormalDetailViewModel>(message, string.Format(ViewPath, nameof(Normal), nameof(Details)));
}