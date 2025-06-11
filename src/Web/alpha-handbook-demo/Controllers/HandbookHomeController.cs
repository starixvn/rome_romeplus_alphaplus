using Microsoft.AspNetCore.Mvc;
using MediatR;
using Alpha.Handbook.Demo.Requests;

namespace Alpha.Handbook.Demo.Controllers
{
    public class HandbookHomeController : BaseController
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