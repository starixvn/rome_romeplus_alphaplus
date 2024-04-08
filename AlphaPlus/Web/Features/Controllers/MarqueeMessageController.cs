namespace AlphaPlus.Features.Site.Messages.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Features.Site.Messages.ViewModels;
    using MediatR;
    using Requests.MarqueeMessage;
    using AlphaPlus.Core.Filters;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Features.Messages.API.Models.Message;

    public class MarqueeMessageController : SiteBaseController
    {
        private readonly IUserSession userSession;

        public MarqueeMessageController(IMediator mediator, IUserSession userSession)
            : base(mediator)
        {
            this.userSession = userSession;
        }

        [HttpPost]
        [AllowProtectedMode]
        public async Task<ActionResult> Index()
        {
            var viewModel = userSession.IsProtectedMode
                ? new MarqueeMessageViewModel(
                    new GeneralMarqueeMessage(),
                    new GeneralMarqueeMessage())
                : await mediator.Send(new MarqueeMessageQuery(userSession));

            return Json(viewModel);
        }
    }
}
