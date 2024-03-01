using AlphaPlus.Core;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.Shared;

public abstract class ReportBaseController(IMediator mediator) : BaseController
{
    private const string InvalidViewName = "InvalidReport";

    protected async Task<IActionResult> View<TResponse>(IRequest<TResponse> message, CancellationToken cancellation)
    {
        var baseQuery = message as BaseQuery;

        return baseQuery != null && baseQuery.IsValid()
            ? View(await mediator.Send(message, CancellationToken cancellation))
            : View(InvalidViewName);
    }
}
