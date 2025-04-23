using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Features.Site.Messages.Requests.BackupDomains;
using MediatR;
using AlphaPlus.Core.Filters;

namespace AlphaPlus.Features.Site.Messages.Controllers
{
    public class BackupDomainsController : SiteBaseController
    {
        public BackupDomainsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet, AllowProtectedMode]
        public async Task<ActionResult> Index(BackupDomainsQuery message)
            => View("~/Views/BackupDomains/Index.cshtml", await mediator.Send(message));
    }
}