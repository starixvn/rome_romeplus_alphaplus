using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Features.Site.Handbook.Requests;
using AlphaPlus.Features.Site.Messages.Controllers;
using AlphaPlus.Features.Site.Handbook.Filters;
using AlphaPlus.NetThrottling;

namespace AlphaPlus.Features.Site.Handbook.Controllers
{
    [HandbookFilter]
    public class HandbookArticleController : SiteBaseController
    {
        public HandbookArticleController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Search(SearchArticlesRequest request)
            => await View(request);

        public async Task<ActionResult> GetBySlug(GetArticleBySlugRequest request)
            => await View(request);

        [HttpGet, DisableNetThrottling]
        public async Task<ActionResult> Suggest(SuggestArticleRequest request)
            => await JsonAsync(request);

        [HttpPost]
        public async Task<ActionResult> GetBySlugSuggest(GetArticleBySlugRequest request)
        {
            var article = await mediator.Send(request);

            return Json(article);
        }
    }
}