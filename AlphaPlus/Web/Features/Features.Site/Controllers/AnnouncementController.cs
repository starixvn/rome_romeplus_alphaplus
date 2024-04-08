namespace AlphaPlus.Features.Site.Messages.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Alpha.Announcements.Models;
    using MediatR;
    using Requests.Announcement;
    using AlphaPlus.Core.Filters;

    public class AnnouncementController : SiteBaseController
    {
        private readonly IParam specificationParam;

        public AnnouncementController(IMediator mediator, IParam specificationParam)
            : base(mediator)
        {
            this.specificationParam = specificationParam;
        }

        [AllowProtectedMode]
        public async Task<ActionResult> Index(AnnouncementsHistoryQuery message)
        {
            message.Specification = specificationParam;
            ViewData["IsProtectedMode"] = message.IsProtectedMode;

            return await View(message);
        }

        [AllowProtectedMode]
        public async Task<ActionResult> Popup(PopupAnnouncementQuery message)
        {
            if (message.IsProtectedMode)
            {
                return new EmptyResult();
            }

            message.Specification = specificationParam;

            return await View(message);
        }
    }
}