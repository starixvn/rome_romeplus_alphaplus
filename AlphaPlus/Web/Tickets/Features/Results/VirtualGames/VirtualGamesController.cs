using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.VirtualGames;

public partial class ResultsController(IMediator mediator) : ResultBaseController(mediator)
{
    public Task<IActionResult> VirtualGames(VirtualGamesResultQuery message)
        => View(message, string.Format(ViewPath, nameof(VirtualGames), "Index"));
}