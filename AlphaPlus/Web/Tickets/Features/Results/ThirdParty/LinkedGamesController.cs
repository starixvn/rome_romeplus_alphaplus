using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public partial class ResultsController : ResultBaseController
{
    public Task<IActionResult> GetSoccer5Result(Soccer5ResultQuery message) => View(message, IndexView);
}
