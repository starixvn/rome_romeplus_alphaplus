using AlphaPlus.CashService.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.CashService.Site.Features.MemberSites
{
    public class MemberSitesController(IMediator mediator)
        : SiteBaseController(mediator)
    {
        public IActionResult Index()
            => View();
    }
}