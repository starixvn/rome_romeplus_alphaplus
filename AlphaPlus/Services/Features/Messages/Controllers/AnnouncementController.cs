using AlphaPlus.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Features.Messages.API.Requests;
using AlphaPlus.Features.Messages.API.ViewModels;

namespace AlphaPlus.Features.Messages.API.Controllers
{
    public class AnnouncementController : BaseController
    {
        private readonly IMediator mediator;

        public AnnouncementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("announcements")]
        public async Task<IEnumerable<AnnouncementViewModel>> GetAnnouncements([FromBody] GetAnnouncementsQuery message)
            => await mediator.Send(message);
    }
}