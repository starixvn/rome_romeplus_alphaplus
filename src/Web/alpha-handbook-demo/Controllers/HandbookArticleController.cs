using Microsoft.AspNetCore.Mvc;
using MediatR;
using Alpha.Handbook.Demo.Requests;

namespace Alpha.Handbook.Demo.Controllers
{
    public class HandbookArticleController : BaseController
    {
        public HandbookArticleController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> GetBySlug(GetArticleBySlugRequest request)
            => await View(request);

        public async Task<ActionResult> GetById(GetArticleByIdRequest request)
            => await View(request);

        [HttpGet]
        public async Task<ActionResult> Search(SearchArticlesRequest request)
            => await View(request);

        [HttpGet]
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