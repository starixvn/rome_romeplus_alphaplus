using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Features.Site.Messages.Requests;
using Alpha.Announcements.Models;
using MediatR;
using AlphaPlus.Features.Site.Messages.Requests.NewMessage;
using AlphaPlus.Core.Filters;
using AlphaPlus.Features.Messages.API.Configurations;

namespace AlphaPlus.Features.Site.Messages.Controllers
{
    public class NewMessageController : SiteBaseController
    {
        private readonly IParam specificationParam;

        public NewMessageController(IMediator mediator, IParam specificationParam)
            : base(mediator)
        {
            this.specificationParam = specificationParam;
        }

        [AllowProtectedMode]
        public async Task<ActionResult> GetNumberOfNewAnnouncement(NumberOfNewMessageQuery message)
        {
            if (IsDisableMessages(message))
            {
                return new EmptyResult();
            }

            message.Specification = specificationParam;

            return await JsonAsync(message);
        }

        private static bool IsDisableMessages(MessageHistoryBaseRequest message)
            => AppSettings.DisableMessages || message.IsProtectedMode;
    }
}