using AlphaPlus.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.MemberSites
{
    public class MemberSitesController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IMediator _mediator;

        public MemberSitesController(
            IWebHostEnvironment environment,
            IMediator mediator)
        {
            _environment = environment;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["IsDevelopment"] = _environment.IsDevelopment();
            return View();
        }
    }
}