using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Features.Messages.API.Configurations;
using MediatR;
using AlphaPlus.Core.Models;
using AlphaPlus.Features.Messages.Site.Requests.PortalMessage;

namespace AlphaPlus.Features.Site.Messages.Controllers
{
    public class PortalMessageController : SiteBaseController
    {
        private const int DefaultNumberMessage = 10;
        private readonly IUserSession userSession;

        public PortalMessageController(IMediator mediator, IUserSession userSession)
            : base(mediator)
        {
            this.userSession = userSession;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (AppSettings.DisableMessages)
            {
                return new EmptyResult();
            }

            return View(DefaultNumberMessage);
        }

        [HttpGet]
        public ActionResult Messages(int totalMessage)
        {
            if (AppSettings.DisableMessages)
            {
                return new EmptyResult();
            }

            var viewModel = mediator.Send(
                new PortalMessageQuery(userSession, totalMessage, AppSettings.LocalTest));

            return PartialView("Messages", viewModel);
        }

        [HttpPost]
        public async Task<bool> MarkMessageAsRead(int messageId)
        {
            if (AppSettings.DisableMessages)
            {
                return false;
            }

            var isSuccess = await mediator.Send(new MarkMessageAsReadQuery(userSession.CustId, messageId, userSession.IsInternal));

            return isSuccess;
        }

        [HttpGet]
        public ActionResult MoreMessages(int totalMessage)
        {
            if (AppSettings.DisableMessages)
            {
                return new EmptyResult();
            }

            return View("Index", totalMessage);
        }
    }
}