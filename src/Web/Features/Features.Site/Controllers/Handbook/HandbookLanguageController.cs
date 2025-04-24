using AlphaPlus.Features.Site.Handbook.Filters;
using AlphaPlus.Features.Site.Handbook.Requests;
using AlphaPlus.Features.Site.Messages.Controllers;
using MediatR;

namespace AlphaPlus.Features.Site.Handbook.Controllers
{
    [HandbookFilter]
    public class HandbookLanguageController : SiteBaseController
    {
        public HandbookLanguageController(IMediator mediator) : base(mediator)
        {
        }

        public async Task Change(ChangeLanguageRequest request)
        {
           await mediator.Send(request);
        }
    }
}