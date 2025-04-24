using AlphaPlus.Features.Site.Handbook.Filters;
using AlphaPlus.Features.Site.Handbook.Requests;
using AlphaPlus.Features.Site.Messages.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AlphaPlus.Features.Site.Handbook.Controllers
{
    [HandbookFilter]
    public class HandbookDefaultController : SiteBaseController
    {
        public HandbookDefaultController(IMediator mediator) : base(mediator)
        {
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public async Task<ActionResult> Index(HandbookIndexRequest request)
        {
            var r = new HandbookGetBySlugRequest();
            return await View(r);
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public  async Task<ActionResult> GetBySlug(HandbookGetBySlugRequest request)
            => await View(request);
    }
}