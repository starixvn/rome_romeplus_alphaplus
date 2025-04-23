using AlphaPlus.Features.Site.Handbook.Filters;
using AlphaPlus.Features.Site.Handbook.Requests;
using AlphaPlus.Features.Site.Messages.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Features.Site.Handbook.Controllers
{
    [HandbookFilter]
    public class HandbookCategoryController : SiteBaseController
    {
        public HandbookCategoryController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> GetBySlug(GetCategoryBySlugRequest request)
            => await View(request);
    }
}