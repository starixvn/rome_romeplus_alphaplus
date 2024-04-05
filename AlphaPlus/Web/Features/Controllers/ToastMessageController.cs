namespace AlphaPlus.Features.Site.Messages.Controllers
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Features.Messages.AppResources;
    using AlphaPlus.Features.Site.Messages.Requests.ToastMessage;
    using MediatR;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Core.Filters;

    public class ToastMessageController : SiteBaseController
    {
        private readonly IUserSession userSession;

        public ToastMessageController(IMediator mediator, IUserSession userSession)
            : base(mediator)
        {
            this.userSession = userSession;
        }

        [AllowProtectedMode]
        public async Task<ActionResult> Get(ToastMessageQuery message)
        {
            var model = await mediator.Send(message);

            return Json(model);
        }

        [AllowProtectedMode]
        public async Task<ActionResult> MarkRead(MarkReadMessageQuery message)
        {
            if (userSession.IsInternal)
            {
                return Json(new Result((int)HttpStatusCode.Unauthorized, Resources.NotAuthorized));
            }

            var model = await mediator.Send(message);
            return Json(model);
        }
    }
}