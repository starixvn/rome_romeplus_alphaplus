using Microsoft.AspNetCore.Mvc;
using MediatR;
using Alpha.Handbook.Demo.Requests;

namespace Alpha.Handbook.Demo.Controllers
{
    public class HandbookGlossaryController : BaseController
    {
        public HandbookGlossaryController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(GetGlossaryByGroupRequest request)
            => await View(request);
    }
}