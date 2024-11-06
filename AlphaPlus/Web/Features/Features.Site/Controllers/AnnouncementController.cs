using Microsoft.AspNetCore.Mvc;
using Alpha.Announcements.Models;
using MediatR;
using AlphaPlus.Features.Site.Messages.Requests.Announcement;
using AlphaPlus.Core.Filters;

namespace AlphaPlus.Features.Site.Messages.Controllers
{
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