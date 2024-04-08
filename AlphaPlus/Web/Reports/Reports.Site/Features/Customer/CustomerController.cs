using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.Customer;

public class CustomerController(IMediator mediator) : ReportBaseController(mediator)
{
    public Task<IActionResult> Suggest(UsernameSuggestionQuery message) => Json(message);
}