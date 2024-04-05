namespace AlphaPlus.Features.Site.Messages.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Alpha.Announcements.Models;
    using MediatR;
    using Requests.MessageHistory;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Core.Filters;
    using AlphaPlus.Features.Messages.Constants;
    using AlphaPlus.Core.Helpers;
    using AlphaPlus.Features.Messages.Configurations;

    public class MessageHistoryController : SiteBaseController
    {
        private readonly IUserSession userSession;
        private readonly IParam specificationParam;

        public MessageHistoryController(IMediator mediator, IUserSession userSession, IParam specificationParam)
            : base(mediator)
        {
            this.userSession = userSession;
            this.specificationParam = specificationParam;
        }

        [AllowProtectedMode]
        public ActionResult Index(int messageType)
        {
            if (IsDisableMessages(messageType))
            {
                return UnAuthorizedAction(AlphaContext.Current.Session);
            }

            AssignIsProtectedModeToViewData();
            var query = new MessageHistoryQuery(
                userSession,
                messageType,
                DateTime.MinValue,
                DateTime.Today,
                AppSettings.LocalTest, 1, 100);
            query.Specification = specificationParam;

            return View(query);
        }

        [AllowProtectedMode]
        public async Task<ActionResult> Messages(
            DateTime? fromDate,
            DateTime? toDate,
            int messageType = MessageType.Normal, int pageIndex = 0, int pageSize = 0)
        {
            if (IsDisableMessages(messageType))
            {
                return UnAuthorizedAction(AlphaContext.Current.Session);
            }

            AssignIsProtectedModeToViewData();
            var query = new MessageHistoryQuery(
                userSession,
                messageType,
                fromDate ?? DateTime.MinValue,
                toDate ?? DateTime.Today,
                AppSettings.LocalTest, pageIndex, pageSize);
            query.Specification = specificationParam;

            var viewModel = await mediator.Send(query);

            return View("Index", viewModel);
        }

        private bool IsDisableMessages(int messageType)
            => userSession.IsProtectedMode && messageType == MessageType.Personal;

        private static ActionResult UnAuthorizedAction(ISession session)
        {
            TerminateSession(session);

            return new RedirectResult(MicroSites.MainUrl());
        }

        private static void TerminateSession(ISession session)
        {
            session.Clear();
        }

        private void AssignIsProtectedModeToViewData()
            => ViewData["IsProtectedMode"] = userSession.IsProtectedMode;
    }
}