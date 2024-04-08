namespace AlphaPlus.Features.Messages.API.Controllers
{
    using System.Collections.Generic;
    using AlphaPlus.Core;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Requests;
    using ViewModels;

    //TODO: [HmacAuthorizationFilter]
    public class AnnouncementController : BaseController
    {
        private readonly IMediator mediator;

        public AnnouncementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("announcements")]
        public async Task<IEnumerable<AnnouncementViewModel>> GetAnnouncements([FromBody]GetAnnouncementsQuery message)
            => await mediator.Send(message);
    }
}