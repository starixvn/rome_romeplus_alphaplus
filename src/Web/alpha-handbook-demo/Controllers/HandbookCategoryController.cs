using Microsoft.AspNetCore.Mvc;
using MediatR;
using Alpha.Handbook.Demo.Requests;

namespace Alpha.Handbook.Demo.Controllers
{
    public class HandbookCategoryController : BaseController
    {
        public HandbookCategoryController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> GetBySlug(GetCategoryBySlugRequest request)
            => await View(request);
    }
}