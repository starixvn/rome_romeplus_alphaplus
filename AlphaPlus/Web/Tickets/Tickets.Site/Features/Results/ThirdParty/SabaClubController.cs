using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public partial class ResultsController : ResultBaseController
{
    public Task<IActionResult> SabaClubResult(SabaClubResultQuery message)
        => View(message, GetViewPath("ThirdParty", "SabaClub"));
}
