using Microsoft.AspNetCore.Mvc;
using MediatR;
using Alpha.Handbook.Demo.Requests;

namespace Alpha.Handbook.Demo.Controllers
{
    public class HandbookDefaultController : BaseController
    {
        public HandbookDefaultController(IMediator mediator) : base(mediator)
        {
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        public async Task<ActionResult> Index(HandbookIndexRequest request)
        {
            var r = new HandbookGetBySlugRequest();
            return await View(r);
        }

        public async Task<ActionResult> GetBySlug(HandbookGetBySlugRequest request)
            => await View(request);
    }
}