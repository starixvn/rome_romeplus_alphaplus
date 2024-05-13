using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.TableGames;

public partial class ResultsController(IMediator mediator) : ResultBaseController(mediator)
{
    public Task<IActionResult> TableGames(TableGamesResultQuery message)
        => View(message, string.Format(ViewPath, nameof(TableGames), "Index"));
}