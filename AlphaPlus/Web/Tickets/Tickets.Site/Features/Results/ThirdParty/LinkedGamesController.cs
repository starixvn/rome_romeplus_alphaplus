using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public partial class ResultsController : ResultBaseController
{
    private const string IndexView = "~/Features/Results/ThirdParty/Views/Index.cshtml";

    public Task<IActionResult> GetSoccer5Result(Soccer5ResultQuery message) => View(message, IndexView);
}
