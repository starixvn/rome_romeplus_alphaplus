using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.Keno;

public partial class ResultsController(IMediator mediator) : ResultBaseController(mediator)
{
    public Task<IActionResult> Keno(KenoResultQuery message)
        => View(message, string.Format(ViewPath, nameof(Keno), "Index"));
}