using AlphaPlus.Features.Site.Handbook.Filters;
using AlphaPlus.Features.Site.Handbook.Requests;
using AlphaPlus.Features.Site.Messages.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Features.Site.Handbook.Controllers
{
    [HandbookFilter]
    public class HandbookHomeController : SiteBaseController
    {
        public HandbookHomeController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(HomeRequest request)
            => await View(request);

        public ActionResult Blank()
            => View();
    }
}