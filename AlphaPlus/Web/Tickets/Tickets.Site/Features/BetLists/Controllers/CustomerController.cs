using AlphaPlus.Tickets.Site.Features.BetLists.Requests.Customer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Controllers;

public class CustomerController(IMediator mediator) : BetListBaseController(mediator)
{
    public Task<IActionResult> Suggest(UsernameSuggestionQuery message) => JsonData(message);
}